#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.RelationalModels.ConceptualDatabase;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;
using System.Collections;
using System.Diagnostics;

namespace Neumont.Tools.ORM.Views.RelationalView
{
	partial class TableShape
	{
		#region Customize Appearance
		/// <summary>
		/// Gets whether the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableShape"/> can be expanded or collapsed.
		/// </summary>
		public override bool CanExpandAndCollapse
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Gets whether the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableShape"/> has a shadow.
		/// </summary>
		public override bool HasShadow
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Overridden to allow a read-only <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.TextField"/> to be added
		/// to the collection of <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.ShapeField"/> objects.
		/// </summary>
		/// <param name="shapeFields">A list of <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.ShapeField"/> objects
		/// which belong to the current shape.</param>
		protected override void InitializeShapeFields(IList<ShapeField> shapeFields)
		{
			base.InitializeShapeFields(shapeFields);
			// Removes the text field from the shape field list.
			shapeFields.RemoveAt(1);
			// 
			TableTextField textField = new TableTextField("TableNameDecorator");
			textField.DefaultText = RelationalShapeDomainModel.SingletonResourceManager.GetString("TableShapeTableNameDecoratorDefaultText");
			textField.DefaultFocusable = true;
			textField.DefaultAutoSize = true;
			textField.AnchoringBehavior.MinimumHeightInLines = 1;
			textField.AnchoringBehavior.MinimumWidthInCharacters = 1;
			textField.DefaultFontId = new StyleSetResourceId(string.Empty, "ShapeTextBold10");
			shapeFields.Add(textField);
		}

		/// <summary>
		/// Gets the resizable sides on this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableShape"/>.
		/// </summary>
		public override NodeSides ResizableSides
		{
			get
			{
				return NodeSides.None;
			}
		}
		#endregion // Customize Appearance
		#region Customize Column Order
		/// <summary>
		/// A sorted replacement for the unsorted element list provided
		/// by generated code. The goal of the class is to provide a replacement
		/// for the <see cref="ElementListCompartmentMapping.ElementListGetter"/>
		/// set by the generated framework code.
		/// </summary>
		/// <typeparam name="T">The type of element in the underlying list</typeparam>
		/// <typeparam name="S">The type of the associated <see cref="ElementListCompartment"/>. This
		/// type is used only to distinguish different uses of the same <typeparamref name="T"/></typeparam>
		private class OrderedElementList<T, S> : IList, IComparer<T>
			where T : ModelElement
			where S : ElementListCompartment
		{
			#region Member Variables and Constructor
			private List<T> mySortedList;
			private IList myUnsortedList;
			private static ElementListGetter myUnorderedGetter;
			private static Comparison<T> myComparison;
			/// <summary>
			/// Private constructor, provides initial sort order
			/// </summary>
			private OrderedElementList(IList unsortedList)
			{
				myUnsortedList = unsortedList;
				int listCount = unsortedList.Count;
				List<T> sortedList;
				mySortedList = sortedList = new List<T>(listCount);
				for (int i = 0; i < listCount; ++i)
				{
					sortedList.Add((T)unsortedList[i]);
				}
				sortedList.Sort(myComparison);
			}
			#endregion // Member Variables and Constructor
			#region Static Initialization
			/// <summary>
			/// Determine if the typed instance has been initialized
			/// </summary>
			public static bool IsInitialized
			{
				get
				{
					return myUnorderedGetter != null;
				}
			}
			/// <summary>
			/// Initial static information for this element
			/// </summary>
			/// <param name="unorderedGetter">The original value of the <see cref="ElementListCompartmentMapping.ElementListGetter"/>
			/// property being modified.</param>
			/// <param name="comparison">The comparison routine used to order the items.</param>
			public static void Initialize(ElementListGetter unorderedGetter, Comparison<T> comparison)
			{
				myUnorderedGetter = unorderedGetter;
				myComparison = comparison;
			}
			#endregion // Static Initialization
			#region Reorder methods
			/// <summary>
			/// The sort order for the specified element has potentially
			/// changed. See where it fits in the new list.
			/// </summary>
			/// <param name="element">The modified element</param>
			/// <param name="oldIndex">The old position. Set only if the position is modified.</param>
			/// <param name="newIndex">The new position. Set only if the position is modified.</param>
			/// <returns><see langword="true"/> if the position has changed.</returns>
			public bool OnElementReorder(T element, out int oldIndex, out int newIndex)
			{
				oldIndex = -1;
				newIndex = -1;
				List<T> sortedList = mySortedList;
				if (-1 != (oldIndex = sortedList.IndexOf(element)))
				{
					sortedList.RemoveAt(oldIndex);
					newIndex = ~sortedList.BinarySearch(element, this);
					sortedList.Insert(newIndex, element);
					return oldIndex != newIndex;
				}
				return false;
			}
			#endregion // Reorder methods
			#region Getter Delegate
			/// <summary>
			/// Use as a replacement for the <see cref="ElementListCompartmentMapping.ElementListGetter"/>
			/// passed to the <see cref="Initialize"/> method
			/// </summary>
			public static IList ElementListGetter(ModelElement startElement)
			{
				return new OrderedElementList<T, S>(myUnorderedGetter(startElement));
			}
			#endregion // Getter Delegate
			#region IList Implementation
			int IList.Add(object value)
			{
				T typedValue = (T)value;
				List<T> sortedList = mySortedList;
				int newLocation = ~sortedList.BinarySearch(typedValue, this);
				Debug.Assert(newLocation >= 0);
				myUnsortedList.Add(value);
				sortedList.Insert(newLocation, typedValue);
				return newLocation;
			}
			void IList.Clear()
			{
				mySortedList.Clear();
				myUnsortedList.Clear();
			}
			bool IList.Contains(object value)
			{
				return myUnsortedList.Contains(value);
			}
			int IList.IndexOf(object value)
			{
				T typedValue = value as T;
				return typedValue != null ? mySortedList.IndexOf(typedValue) : -1;
			}
			void IList.Insert(int index, object value)
			{
				mySortedList.Insert(index, (T)value);
				myUnsortedList.Add(value);
			}
			bool IList.IsFixedSize
			{
				get
				{
					return myUnsortedList.IsFixedSize;
				}
			}
			bool IList.IsReadOnly
			{
				get
				{
					return myUnsortedList.IsReadOnly;
				}
			}
			void IList.Remove(object value)
			{
				myUnsortedList.Remove(value);
				List<T> sortedList = mySortedList;
				sortedList.Remove((T)value);
			}
			void IList.RemoveAt(int index)
			{
				List<T> sortedList = mySortedList;
				object value = sortedList[index];
				sortedList.RemoveAt(index);
				myUnsortedList.Remove(value);
			}
			object IList.this[int index]
			{
				get
				{
					return mySortedList[index];
				}
				set
				{
					IList unsortedList = myUnsortedList;
					List<T> sortedList = mySortedList;
					unsortedList[unsortedList.IndexOf(sortedList[index])] = value;
					sortedList[index] = (T)value;
				}
			}
			#endregion // IList Implementation
			#region ICollection Implementation
			void ICollection.CopyTo(Array array, int index)
			{
				((ICollection)mySortedList).CopyTo(array, index);
			}

			int ICollection.Count
			{
				get
				{
					return mySortedList.Count;
				}
			}
			bool ICollection.IsSynchronized
			{
				get
				{
					return myUnsortedList.IsSynchronized;
				}
			}

			object ICollection.SyncRoot
			{
				get
				{
					return myUnsortedList.SyncRoot;
				}
			}
			#endregion // ICollection Implementation
			#region IEnumerable Implementation
			IEnumerator IEnumerable.GetEnumerator()
			{
				return mySortedList.GetEnumerator();
			}
			#endregion // IEnumerable Implementation
			#region IComparer<T> Implementation
			int IComparer<T>.Compare(T x, T y)
			{
				return myComparison(x, y);
			}
			#endregion // IComparer<T> Implementation
		}
		/// <summary>
		/// Provide an ordered list for displaying columns
		/// </summary>
		protected override CompartmentMapping[] GetCompartmentMappings(Type melType)
		{
			CompartmentMapping[] retVal = base.GetCompartmentMappings(melType);
			if (melType == typeof(Table) && !OrderedElementList<Column, ColumnElementListCompartment>.IsInitialized)
			{
				ElementListCompartmentMapping mapping = (ElementListCompartmentMapping)retVal[0];
				OrderedElementList<Column, ColumnElementListCompartment>.Initialize(
					mapping.ElementListGetter,
					delegate(Column x, Column y)
					{
						if (x == y)
						{
							return 0;
						}
						bool leftIsPrimary = x.IsPartOfPrimaryIdentifier;
						bool rightIsPrimary = y.IsPartOfPrimaryIdentifier;
						if (leftIsPrimary ^ rightIsPrimary)
						{
							return leftIsPrimary ? -1 : 1;
						}

						return x.Name.CompareTo(y.Name);
					});
				mapping.ElementListGetter = OrderedElementList<Column, ColumnElementListCompartment>.ElementListGetter;
			}
			return retVal;
		}
		#endregion // Customize Column Order
		#region Event Management
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for <see cref="TableShape"/>s.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public static void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(TableShape.UpdateCounterDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(UpdateShapeEvent), action);

			if (action == EventHandlerAction.Add)
			{
				// We don't want this rule on. This forces a full repopulation of the compartment,
				// which makes it impossible to determine the old index of a selected item. If this is
				// on, then the ColumnRenamedEvent does not work for the initial transaction.
				store.RuleManager.DisableRule(typeof(CompartmentItemChangeRule));
			}
			propertyInfo = dataDirectory.FindDomainProperty(Column.NameDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ColumnRenamedEvent), action);
		}
		/// <summary>
		/// Reorder compartment items when a column is renamed.
		/// </summary>
		private static void ColumnRenamedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			Column column = (Column)e.ModelElement;
			Table table;
			if (!column.IsDeleted &&
				null != (table = column.Table))
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(table))
				{
					TableShape shape = pel as TableShape;
					if (shape != null)
					{
						foreach (ShapeElement childShape in shape.NestedChildShapes)
						{
							ColumnElementListCompartment compartment;
							OrderedElementList<Column, ColumnElementListCompartment> columnList;
							int oldIndex;
							int newIndex;
							if (null != (compartment = childShape as ColumnElementListCompartment) &&
								null != (columnList = compartment.Items as OrderedElementList<Column, ColumnElementListCompartment>))
								
							{
								if (columnList.OnElementReorder(column, out oldIndex, out newIndex))
								{
									Diagram diagram;
									DiagramView view;
									SelectedShapesCollection selection;
									if (null != (diagram = shape.Diagram) &&
										null != (view = diagram.ActiveDiagramView) &&
										null != (selection = view.Selection))
									{
										ShapeField testField = compartment.ListField;
										foreach (DiagramItem selectedItem in selection)
										{
											ListItemSubField testSubField;
											if (selectedItem.Shape == compartment &&
												selectedItem.Field == testField &&
												null != (testSubField = selectedItem.SubField as ListItemSubField))
											{
												int testRow = testSubField.Row;
												if (testRow == oldIndex)
												{
													testSubField.Row = newIndex;
												}
												else
												{
													int adjustRow = testRow;
													if (testRow > oldIndex)
													{
														--adjustRow;
													}
													if (adjustRow >= newIndex)
													{
														++adjustRow;
													}
													if (adjustRow != testRow)
													{
														testSubField.Row = adjustRow;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		#endregion // Event Management
		#region TableTextField class
		/// <summary>
		/// A custom <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.TextField"/> that disallows selection and focus of the element.
		/// </summary>
		private sealed class TableTextField : TextField
		{
			/// <summary>
			/// Gets whether the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableTextField" /> is selectable.
			/// </summary>
			/// <param name="parentShape">parentShape</param>
			/// <returns><see langword="false" />.</returns>
			public override bool GetSelectable(ShapeElement parentShape)
			{
				return false;
			}
			/// <summary>
			/// Gets whether the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableTextField" /> is focusable.
			/// </summary>
			/// <param name="parentShape">parentShape</param>
			/// <returns><see langword="false" />.</returns>
			public override bool GetFocusable(ShapeElement parentShape)
			{
				return false;
			}
			/// <summary>
			/// Initializes a new instance of the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableTextField" /> class.	
			/// </summary>
			/// <param name="fieldName">The name of the field.</param>
			public TableTextField(string fieldName)
				: base(fieldName)
			{

			}
		}
		#endregion // TableTextField class
	}
	partial class TableShapeBase
	{
		#region Auto-invalidate tracking
		/// <summary>
		/// Call to automatically invalidate the shape during events.
		/// Invalidates during the original event sequence as well as undo and redo.
		/// </summary>
		public void InvalidateRequired()
		{
			InvalidateRequired(false);
		}
		/// <summary>
		/// Call to automatically invalidate the shape during events.
		/// Invalidates during the original event sequence as well as undo and redo.
		/// </summary>
		/// <param name="refreshBitmap">Value to forward to the Invalidate method's refreshBitmap property during event playback</param>
		public void InvalidateRequired(bool refreshBitmap)
		{
			TransactionManager tmgr = Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				UpdateCounter = unchecked(tmgr.CurrentTransaction.SequenceNumber - (refreshBitmap ? 0L : 1L));
			}
		}
		private long GetUpdateCounterValue()
		{
			TransactionManager tmgr = Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				// Using subtract 2 and set to 1 under to indicate
				// the difference between an Invalidate(true) and
				// and Invalidate(false)
				return unchecked(tmgr.CurrentTransaction.SequenceNumber - 2);
			}
			else
			{
				return 0L;
			}
		}
		private void SetUpdateCounterValue(long newValue)
		{
			// Nothing to do, we're just trying to create a transaction log
		}
		protected static void UpdateShapeEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			TableShape shape = (TableShape)e.ModelElement;
			if (!shape.IsDeleted)
			{
				//shape.BeforeInvalidate();
				shape.Invalidate(Math.Abs(unchecked((long)e.OldValue - (long)e.NewValue)) != 1L);
			}
		}
		#endregion // Auto-invalidate tracking
	}
}
