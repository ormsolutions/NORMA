#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Northface.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Drawing;

#endregion

namespace Northface.Tools.ORM.ShapeModel
{
	public partial class ReadingShape
	{
		private static AutoSizeTextField myTextShapeField;
		private static Regex regCountPlaces = new Regex(@"{(?<placeHolderNr>\d+)}");
		private const string ELLIPSIS = "\x2026";
		private const char C_ELLIPSIS = '\x2026';

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

			if (read.IsPrimary && (attrGuid == Reading.TextMetaAttributeGuid || attrGuid == Reading.IsPrimaryMetaAttributeGuid))
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

		private string myDisplayText = null;

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
				String retval = null;
				if (myDisplayText == null)
				{
					ReadingOrder readingOrd = this.ModelElement as ReadingOrder;
					Debug.Assert(readingOrd != null);

					string textVal = readingOrd.ReadingText;
					retval = regCountPlaces.Replace(textVal, ELLIPSIS).Trim();

					if (readingOrd.RoleCollection.Count == 2)
					{
						if (retval.IndexOf(C_ELLIPSIS) == 0 && retval.LastIndexOf(C_ELLIPSIS) == retval.Length - 1)
						{
							retval = retval.Replace(ELLIPSIS, String.Empty).Trim();
						}
					}
					myDisplayText = retval;
				}
				else
				{
					retval = myDisplayText;
				}
				return retval;
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
		#endregion

		#region nested class ReadingAutoSizeTextField
		/// <summary>
		/// Contains code to replace RolePlayer place holders with an ellipsis.
		/// </summary>
		private class ReadingAutoSizeTextField : AutoSizeTextField
		{
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
				if (attrId == Reading.TextMetaAttributeGuid || attrId == Reading.IsPrimaryMetaAttributeGuid)
				{
					Debug.Assert(read.ReadingOrder != null);
					PresentationElementMoveableCollection pelList = read.ReadingOrder.PresentationRolePlayers;
					foreach (ShapeElement pel in pelList)
					{
						ReadingShape readShape = pel as ReadingShape;
						if (readShape != null)
						{
							readShape.InvalidateDisplayText();
						}
					}
				}
			}
		}
		#endregion
	}
}
