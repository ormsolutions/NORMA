#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Northface.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

#endregion

namespace Northface.Tools.ORM.ShapeModel
{
	public partial class ReadingShape
	{
		private static AutoSizeTextField myTextShapeField;
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
			get { return Reading.TextMetaAttributeGuid; }
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


		#region Reading text display update rules
		[RuleOn(typeof(FactTypeHasReading), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class ReadingAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasReading link = e.ModelElement as FactTypeHasReading;
				Reading reading = link.ReadingCollection;
				FactType fact = link.FactType;

				if (reading.IsPrimary)
				{
					Diagram.FixUpDiagram(fact, reading);
				}
			}
		}
		#endregion
	}
}
