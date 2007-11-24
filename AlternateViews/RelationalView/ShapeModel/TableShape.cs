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
		}
		private static void UpdateShapeEvent(object sender, ElementPropertyChangedEventArgs e)
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
