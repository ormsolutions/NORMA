#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Northface.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Drawing;

#endregion

namespace Northface.Tools.ORM.ShapeModel
{
	public partial class ReadingShape : IModelErrorActivation
	{
		private static AutoSizeTextField myTextShapeField;
		private static Regex regCountPlaces = new Regex(@"{(?<placeHolderNr>\d+)}");
		private static string ellipsis = ResourceStrings.ReadingShapeEllipsis;
		private static char c_ellipsis = ellipsis.ToCharArray()[0];

		#region Model Event Hookup and Handlers

		#region Event Hookup
		/// <summary>
		/// Attaches event listeners for the purpose of notifying the
		/// ReadingShape to invalidate its cached data.
		/// </summary>
		public static void AttachEventHandlers(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;

			// Track ElementLink changes
			MetaClassInfo classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasReading.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(ReadingAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(ReadingRemovedEvent));

			classInfo = dataDirectory.FindMetaClass(Reading.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Add(classInfo, new ElementAttributeChangedEventHandler(ReadingAttributeChangedEvent));

			classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasRole.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(RoleAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(RoleRemovedEvent));
		}

		/// <summary>
		/// Detaches event listeners for the purpose of notifying the
		/// ReadingShape to invalidate its cached data.
		/// </summary>
		public static void DetachEventHandlers(Store store)
		{
			if (store == null || store.Disposed)
			{
				return;
			}
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;

			// Track ElementLink changes
			MetaClassInfo classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasReading.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(ReadingAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(ReadingRemovedEvent));

			classInfo = dataDirectory.FindMetaClass(Reading.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Remove(classInfo, new ElementAttributeChangedEventHandler(ReadingAttributeChangedEvent));

			classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasRole.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(RoleAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(RoleRemovedEvent));
		}
		#endregion

		#region Reading Events
		/// <summary>
		/// Event handler that listens for when ReadingOrderHasReading link is being added
		/// and then tells associated model elements to invalidate their cache
		/// </summary>
		public static void ReadingAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
			if (link.ReadingCollection.IsPrimary)
			{
				RefreshPresentationElements(link.ReadingOrder.PresentationRolePlayers);
			}
		}

		/// <summary>
		/// Event handler that listens for when ReadingOrderHasReading link is being removed
		/// and then tells associated model elements to invalidate their cache
		/// </summary>
		public static void ReadingRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
			Reading read = link.ReadingCollection;
			ReadingOrder ord = link.ReadingOrder;

			if (!ord.IsRemoved && read.IsPrimary)
			{
				RefreshPresentationElements(ord.PresentationRolePlayers);
			}
		}

		/// <summary>
		/// Event handler that listens for when a Reading attribute is changed
		/// and then tells associated model elements to invalidate their cache
		/// </summary>
		public static void ReadingAttributeChangedEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			Reading read = e.ModelElement as Reading;
			Guid attrGuid = e.MetaAttribute.Id;

			if (read.IsPrimary &&
				(attrGuid == Reading.TextMetaAttributeGuid || attrGuid == Reading.IsPrimaryMetaAttributeGuid) &&
				!read.IsRemoved)
			{
				RefreshPresentationElements(read.ReadingOrder.PresentationRolePlayers);
			}
		}
		#endregion

		#region Role Events
		/// <summary>
		/// Event handler that listens for when ReadingOrderHasRole link is being added
		/// and then tells associated model elements to invalidate their cache
		/// </summary>
		public static void RoleAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ReadingOrderHasRole link = e.ModelElement as ReadingOrderHasRole;
			ReadingOrder ord = link.ReadingOrder;

			RefreshPresentationElements(ord.PresentationRolePlayers);
		}

		/// <summary>
		/// Event handler that listens for when ReadingOrderHasRole link is being removed
		/// and then tells associated model elements to invalidate their cache
		/// </summary>
		public static void RoleRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ReadingOrderHasRole link = e.ModelElement as ReadingOrderHasRole;
			ReadingOrder ord = link.ReadingOrder;

			if (!ord.IsRemoved)
			{
				RefreshPresentationElements(ord.PresentationRolePlayers);
			}
		}

		/// <summary>
		/// Used to invalidate caches on presentation elements.
		/// </summary>
		private static void RefreshPresentationElements(PresentationElementMoveableCollection pels)
		{
			ReadingShape rs;
			int numPels = pels.Count;
			for (int i = 0; i < numPels; ++i)
			{
				rs = pels[i] as ReadingShape;
				if (rs != null)
				{
					rs.InvalidateDisplayText();
				}
			}
		}
		#endregion

		#endregion

		private string myDisplayText;

		#region overrides
		/// <summary>
		/// Associate the reading text with this shape
		/// </summary>
		protected override Guid AssociatedShapeMetaAttributeGuid
		{
			get { return ReadingTextMetaAttributeGuid; }
		}

		/// <summary>
		/// Associate to the readints text attribute
		/// </summary>
		protected override Guid AssociatedModelMetaAttributeGuid
		{
			get { return ReadingOrder.ReadingTextMetaAttributeGuid; }
		}

		/// <summary>
		/// Store per-type value for the base class
		/// </summary>
		[CLSCompliant(false)]
		protected override AutoSizeTextField TextShapeField
		{
			get
			{
				return myTextShapeField;
			}
			set
			{
				Debug.Assert(myTextShapeField == null); // This should only be called once per type
				myTextShapeField = value;
			}
		}

		/// <summary>
		/// Putting the reading text under the fact.
		/// </summary>
		public override void OnBoundsFixup(BoundsFixupState fixupState, int iteration)
		{
			base.OnBoundsFixup(fixupState, iteration);
			if (fixupState != BoundsFixupState.Invalid)
			{
				SizeD size = Size;
				Location = new PointD(0, 1.5 * size.Height);
			}
		}

		/// <summary>
		/// Changed to allow resizing of the label
		/// </summary>
		public override NodeSides ResizableSides
		{
			get { return NodeSides.All; }
		}

		/// <summary>
		/// Overrides default implemenation to instantiate an Reading specific one.
		/// </summary>
		[CLSCompliant(false)]
		protected override AutoSizeTextField CreateAutoSizeTextField()
		{
			return new ReadingAutoSizeTextField();
		}
		#endregion

		#region Helper methods
		/// <summary>
		/// Notifies the shape that the currently cached display text may no longer
		/// be accurate, so it needs to be recreated.
		/// </summary>
		private void InvalidateDisplayText()
		{
			myDisplayText = null;
			//this is triggering code that needs a transaction
			if (Store.TransactionManager.InTransaction)
			{
				this.AutoResize();
			}
		}
		#endregion

		#region properties
		/// <summary>
		/// Constructs how the reading text should be displayed.
		/// </summary>
		public String DisplayText
		{
			get
			{
				StringBuilder retval = new StringBuilder();
				if (myDisplayText == null)
				{
					ReadingOrder readingOrd = this.ModelElement as ReadingOrder;
					Debug.Assert(readingOrd != null);

					FactType factType = readingOrd.FactType;
					ReadingOrderMoveableCollection readingCollection = factType.ReadingOrderCollection;
					int numReadings = readingCollection.Count;
					for (int i = 0; i < numReadings; ++i)
					{
						if (i > 0)
						{
							retval.Append(ResourceStrings.ReadingShapeReadingSeparator);
							if (numReadings > 2)
							{
								retval.Append("\u000A\u000D");
							}
						}
						ReadingOrder readingOrder = readingCollection[i];
						string aReading = readingOrder.ReadingText;
						RoleMoveableCollection roleCollection = readingOrder.RoleCollection;
						if (roleCollection.Count <= 2 || (numReadings > 1 && i == 0))
						{
							aReading = regCountPlaces.Replace(aReading, ellipsis).Trim();
							if (i == 0 && roleCollection[0] != factType.RoleCollection[0])
							{
								//Terry's preffered character to append is \u25C4 which can
								//be found in the "Arial Unicode MS" font
								retval.Append(ResourceStrings.ReadingShapeInverseReading);
							}
							if (numReadings <= 2 && roleCollection.Count <= 2 &&
								aReading.IndexOf(c_ellipsis) == 0 &&
								aReading.LastIndexOf(c_ellipsis) == aReading.Length - 1)
							{
								aReading = aReading.Replace(ellipsis, String.Empty).Trim();
							}
						}
						else
						{
							RoleMoveableCollection factRoleCollection = factType.RoleCollection;
							//UNDONE: the roleCount should be factRoleCollection.Count. However, this causes
							//an error when a role is added to a factType because the factType attempts to
							//update the ReadingShape before the ReadingOrders have had the role added to them.
							//Check the order of execution to see if the ReadingOrders can have the role added
							//to them before the ReadingShape is updated.
							int roleCount = roleCollection.Count;
							Debug.Assert(roleCount == roleCollection.Count);
							string[] roleTranslator = new string[roleCount];
							for (int readRoleNum = 0; readRoleNum < roleCount; ++readRoleNum)
							{
								int factRoleNum = factRoleCollection.IndexOf(roleCollection[readRoleNum]) + 1;
								roleTranslator[readRoleNum] = "{" + factRoleNum.ToString(CultureInfo.InvariantCulture) + "}";
							}
							aReading = string.Format(CultureInfo.InvariantCulture, aReading, roleTranslator);
						}
						retval.Append(aReading);
					}
					myDisplayText = retval.ToString();
				}
				return myDisplayText;
			}
		}
		#endregion

		#region Reading text display update rules
		[RuleOn(typeof(FactTypeHasReadingOrder), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class ReadingOrderAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasReadingOrder link = e.ModelElement as FactTypeHasReadingOrder;
				ReadingOrder readingOrd = link.ReadingOrderCollection;
				FactType fact = link.FactType;
				Diagram.FixUpDiagram(fact.Model, fact); // Make sure the fact is already there
				Diagram.FixUpDiagram(fact, readingOrd);
			}
		}
		[RuleOn(typeof(FactTypeHasReadingOrder), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class ReadingOrderRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasReadingOrder link = e.ModelElement as FactTypeHasReadingOrder;
				FactType factType = link.FactType;
				ReadingOrder readingOrder = link.ReadingOrderCollection;
				if (readingOrder.FactType == null)
				{
					ReadingOrderMoveableCollection newReadingOrders = factType.ReadingOrderCollection;
					if (newReadingOrders.Count > 0)
					{
						readingOrder = newReadingOrders[0];
					}
				}
				foreach (PresentationElement pel in factType.AssociatedPresentationElements)
				{
					FactTypeShape factShape = pel as FactTypeShape;
					if (factShape != null)
					{
						foreach (ShapeElement shape in factShape.RelativeChildShapes)
						{
							ReadingShape readingShape = shape as ReadingShape;
							if (readingShape != null)
							{
								readingShape.InvalidateDisplayText();
							}
						}
					}
				}
			}
		}
		#endregion

		#region nested class ReadingAutoSizeTextField
		/// <summary>
		/// Contains code to replace RolePlayer place holders with an ellipsis.
		/// </summary>
		private class ReadingAutoSizeTextField : AutoSizeTextField
		{
			/// <summary>
			/// Initialize a ReadingAutoSizeTextField
			/// </summary>
			public ReadingAutoSizeTextField()
			{
				StringFormat format = new StringFormat();
				format.Alignment = StringAlignment.Near;
				DefaultStringFormat = format;
			}
			/// <summary>
			/// Code that handles the displaying of ellipsis in place of place holders and also
			/// their suppression if the are on the outside of a binary fact.
			/// </summary>
			public override string GetDisplayText(ShapeElement parentShape)
			{
				string retval = null;
				ReadingShape parentReading = parentShape as ReadingShape;

				if (parentReading == null)
				{
					retval = base.GetDisplayText(parentShape);
				}
				else
				{
					retval = parentReading.DisplayText;
				}

				return retval;
			}

			/// <summary>
			/// Changed to return true to get multiple line support.
			/// </summary>
			public override bool GetMultipleLine(ShapeElement parentShape)
			{
				return true;
			}
		}
		#endregion // nested class ReadingAutoSizeTextField

		#region change rules
		/// <summary>
		/// Rule to detect changes to the ReadingText so that the shape knows the
		/// display text needs to be recreated.
		/// </summary>
		[RuleOn(typeof(ReadingOrder))]
		private class ReadingOrderReadingTextChanged : ChangeRule
		{
			/// <summary>
			/// Used to get notification of attribute changes.
			/// Current code interested in changes to:
			/// ReadingText
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attrId = e.MetaAttribute.Id;
				ReadingOrder readingOrd = e.ModelElement as ReadingOrder;
				Debug.Assert(readingOrd != null);
				if (attrId == ReadingOrder.ReadingTextMetaAttributeGuid)
				{
					PresentationElementMoveableCollection pelList = readingOrd.PresentationRolePlayers;
					foreach (ShapeElement pel in pelList)
					{
						ReadingShape reading = pel as ReadingShape;
						if (reading != null)
						{
							reading.InvalidateDisplayText();
						}
					}
				}
			}
		}

		/// <summary>
		/// Rule to notice changes to Reading.Text properties so that the
		/// reading shapes can have their display text invalidated.
		/// </summary>
		[RuleOn(typeof(Reading))]
		private class ReadingTextChanged : ChangeRule
		{
			/// <summary>
			/// Notice when Text attribute is changed and invalidate display text of the ReadingShapes
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attrId = e.MetaAttribute.Id;
				Reading read = e.ModelElement as Reading;
				Debug.Assert(read != null);
				ReadingOrder readingOrder = read.ReadingOrder;
				if (attrId == Reading.TextMetaAttributeGuid || attrId == Reading.IsPrimaryMetaAttributeGuid)
				{
					Debug.Assert(readingOrder != null);
					PresentationElementMoveableCollection pelList = readingOrder.FactType.PresentationRolePlayers;
					foreach (ShapeElement pel in pelList)
					{
						foreach (ShapeElement pel2 in pel.RelativeChildShapes)
						{
							ReadingShape readShape = pel2 as ReadingShape;
							if (readShape != null)
							{
								readShape.InvalidateDisplayText();
							}
						}
					}
				}
				if(attrId == Reading.TextMetaAttributeGuid){
					string newValue = (string)e.NewValue;
					if (newValue.Length == 0)
					{
						ReadingMoveableCollection readingColl = readingOrder.ReadingCollection;
						if (readingColl.Count > 1)
						{
							read.Remove();
						}
						else
						{
							//UNDONE: Removing the reading order when it is the one that the
							//shape is based on causes the shape to be removed. The shape should
							//remain if there are other reading orders available and it should
							//display those reading orders.
							readingOrder.Remove();
						}
					}
				}
			}
		}
		#endregion

		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError. Forwards errors to
		/// associated fact type
		/// </summary>
		/// <param name="error">Activated model error</param>
		protected void ActivateModelError(ModelError error)
		{
			IModelErrorActivation parent = ParentShape as IModelErrorActivation;
			if (parent != null)
			{
				parent.ActivateModelError(error);
			}
		}
		void IModelErrorActivation.ActivateModelError(ModelError error)
		{
			ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
	}
}
