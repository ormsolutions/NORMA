#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Matthew Curland. All rights reserved.                        *
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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Collections.ObjectModel;
using Neumont.Tools.Modeling.Design;

namespace Neumont.Tools.Modeling.Shell
{
	#region DiagramDisplay class
	partial class DiagramDisplay
	{
		#region Public Methods
		/// <summary>
		/// Update the diagram display order
		/// </summary>
		/// <param name="store">The current <see cref="Store"/></param>
		/// <param name="orderedDiagrams">An ordered list of diagrams</param>
		public static void UpdateDiagramDisplayOrder(Store store, IList<Diagram> orderedDiagrams)
		{
			DiagramDisplay container = null;
			IList<DiagramDisplay> containers = store.ElementDirectory.FindElements<DiagramDisplay>(false);
			if (containers.Count != 0)
			{
				container = containers[0];
			}
			using (Transaction t = store.TransactionManager.BeginTransaction(FrameworkResourceStrings.DiagramDisplayReorderDiagramsTransactionName))
			{
				int orderCount = orderedDiagrams.Count;
				if (orderCount == 0)
				{
					if (container != null)
					{
						container.OrderedDiagramCollection.Clear();
					}
					return;
				}
				else if (container == null)
				{
					container = new DiagramDisplay(store);
				}
				LinkedElementCollection<Diagram> existingDiagrams = container.OrderedDiagramCollection;
				int existingDiagramCount = existingDiagrams.Count;
				for (int i = existingDiagramCount - 1; i >= 0; --i)
				{
					Diagram testDiagram = existingDiagrams[i];
					if (!orderedDiagrams.Contains(testDiagram))
					{
						existingDiagrams.RemoveAt(i);
						--existingDiagramCount;
					}
				}
				for (int i = 0; i < orderCount; ++i)
				{
					Diagram orderedDiagram = orderedDiagrams[i];
					int existingIndex = existingDiagrams.IndexOf(orderedDiagram);
					if (existingIndex == -1)
					{
						if (i < existingDiagramCount)
						{
							existingDiagrams.Insert(i, orderedDiagram);
						}
						else if (!existingDiagrams.Contains(orderedDiagram))
						{
							existingDiagrams.Add(orderedDiagram);
						}
						++existingDiagramCount;
					}
					else if (existingIndex != i)
					{
						existingDiagrams.Move(existingIndex, i);
					}
				}
				if (t.HasPendingChanges)
				{
					t.Commit();
				}
			}
		}
		#endregion // Public Methods
		#region Rule methods
		/// <summary>
		/// AddRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.Diagram), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Make sure a diagram is included in the ordered list
		/// </summary>
		private static void DiagramAddedRule(ElementAddedEventArgs e)
		{
			Diagram diagram = (Diagram)e.ModelElement;
			if (!diagram.IsDeleted)
			{
				Store store = diagram.Store;
				if (diagram.Partition == store.DefaultPartition)
				{
					DiagramDisplay displayContainer = DiagramDisplayHasDiagramOrder.GetDiagramDisplay(diagram);
					if (displayContainer == null)
					{
						ReadOnlyCollection<DiagramDisplay> displays = store.ElementDirectory.FindElements<DiagramDisplay>(false);
						displayContainer = (displays.Count != 0) ? displays[0] : new DiagramDisplay(store);
						new DiagramDisplayHasDiagramOrder(displayContainer, diagram);
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(DiagramDisplayHasDiagramOrder), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Make sure that deletion of a diagram order results in a reorder
		/// </summary>
		private static void DiagramOrderDeletedRule(ElementDeletedEventArgs e)
		{
			DiagramDisplayHasDiagramOrder link = (DiagramDisplayHasDiagramOrder)e.ModelElement;
			Diagram diagram = link.Diagram;
			if (!diagram.IsDeleted)
			{
				Store store = diagram.Store;
				if (diagram.Partition == store.DefaultPartition) // This should never happen, but it is easy to check
				{
					if (null == DiagramDisplayHasDiagramOrder.GetDiagramDisplay(diagram))
					{
						DiagramDisplay displayContainer = link.DiagramDisplay;
						if (displayContainer.IsDeleted)
						{
							ReadOnlyCollection<DiagramDisplay> displays = store.ElementDirectory.FindElements<DiagramDisplay>(false);
							displayContainer = (displays.Count != 0) ? displays[0] : new DiagramDisplay(store);
						}
						new DiagramDisplayHasDiagramOrder(displayContainer, diagram);
					}
				}
			}
			else
			{
				DiagramDisplay displayContainer = link.DiagramDisplay;
				if (!displayContainer.IsDeleted && displayContainer.OrderedDiagramCollection.Count == 0)
				{
					// No more diagrams, don't need the container
					displayContainer.Delete();
				}
			}
		}
		#endregion // Rule methods
		#region Property provider
		/// <summary>
		/// A <see cref="PropertyProvider"/> to attach additional properties to a <see cref="Diagram"/>
		/// </summary>
		/// <param name="element">The <see cref="ModelElement"/> to attach properties to.</param>
		/// <param name="properties">The set of <see cref="PropertyDescriptor"/> elements to extend.</param>
		public static void ProvideDiagramProperties(ModelElement element, PropertyDescriptorCollection properties)
		{
			PropertyDescriptor contextDescriptor = DiagramContextDescriptor.CreateDescriptor(element);
			if (contextDescriptor != null)
			{
				properties.Add(contextDescriptor);
			}
		}
		private sealed class DiagramContextDescriptor : PropertyDescriptor
		{
			private readonly PropertyDescriptor myInnerDescriptor;
			private readonly DiagramDisplay myInnerElement;
			public static PropertyDescriptor CreateDescriptor(ModelElement contextElement)
			{
				Store store = contextElement.Store;
				IList<DiagramDisplay> containers = store.ElementDirectory.FindElements<DiagramDisplay>();
				if (containers.Count != 0)
				{
					DiagramDisplay container = containers[0];
					PropertyDescriptor descriptor = DomainTypeDescriptor.CreatePropertyDescriptor(container, store.DomainDataDirectory.GetDomainProperty(DiagramDisplay.SaveDiagramPositionDomainPropertyId));
					return new DiagramContextDescriptor(container, descriptor);
				}
				return null;
			}
			private DiagramContextDescriptor(DiagramDisplay contextElement, PropertyDescriptor descriptor)
				: base(descriptor.Name, null)
			{
				myInnerElement = contextElement;
				myInnerDescriptor = descriptor;
			}
			public override bool CanResetValue(object component)
			{
				return myInnerDescriptor.CanResetValue(myInnerElement);			
			}
			public override Type ComponentType
			{
				get
				{
					return myInnerDescriptor.ComponentType;
				}
			}
			public override object GetValue(object component)
			{
				return myInnerDescriptor.GetValue(myInnerElement);
			}
			public override bool IsReadOnly
			{
				get
				{
					return myInnerDescriptor.IsReadOnly;
				}
			}
			public override Type PropertyType
			{
				get
				{
					return myInnerDescriptor.PropertyType;
				}
			}
			public override void ResetValue(object component)
			{
				myInnerDescriptor.ResetValue(myInnerElement);
			}
			public override void SetValue(object component, object value)
			{
				myInnerDescriptor.SetValue(myInnerElement, value);
			}
			public override bool ShouldSerializeValue(object component)
			{
				return myInnerDescriptor.ShouldSerializeValue(myInnerElement);
			}
			public override string Category
			{
				get
				{
					return myInnerDescriptor.Category;
				}
			}
			public override string Description
			{
				get
				{
					return myInnerDescriptor.Description;
				}
			}
			public override string DisplayName
			{
				get
				{
					return myInnerDescriptor.DisplayName;
				}
			}
			public override string Name
			{
				get
				{
					return myInnerDescriptor.Name;
				}
			}
		}
		#endregion // Property provider
	}
	#endregion // DiagramDisplay class
	#region DiagramPlaceHolder class
	partial class DiagramPlaceHolder
	{
		/// <summary>
		/// Required base override
		/// </summary>
		protected override StyleSet ClassStyleSet
		{
			get
			{
				return null;
			}
		}
		/// <summary>
		/// Required base override
		/// </summary>
		public override IList<ShapeField> ShapeFields
		{
			get
			{
				return null;
			}
		}
	}
	#endregion // DiagramPlaceHolder class
	#region DiagramDisplayHasDiagramOrder class
	partial class DiagramDisplayHasDiagramOrder
	{
		#region Member Variables
		private PointD myCenterPoint = PointD.Empty;
		private float myZoomFactor = 1f;
		private bool myIsActive;
		#endregion // Member Variables
		#region Public Methods
		/// <summary>
		/// Update the cached diagram position and zoom factors
		/// </summary>
		/// <param name="centerPoint">The center point of the displayed diagram</param>
		/// <param name="zoomFactor">The zoom factor for the diagram</param>
		public void UpdatePosition(PointD centerPoint, float zoomFactor)
		{
			myCenterPoint = centerPoint;
			myZoomFactor = zoomFactor;
		}
		/// <summary>
		/// Make this the active view for the display
		/// </summary>
		public void Activate()
		{
			if (!myIsActive)
			{
				DiagramDisplay container = DiagramDisplay;
				foreach (DiagramDisplayHasDiagramOrder link in DiagramDisplayHasDiagramOrder.GetLinksToOrderedDiagramCollection(DiagramDisplay))
				{
					link.myIsActive = false; // Doesn't matter if we set ours
				}
				myIsActive = true;
			}
		}
		#endregion // Public Methods
		#region Custom storage handlers
		private float GetZoomFactorValue()
		{
			return myZoomFactor;
		}
		private void SetZoomFactorValue(float value)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				// This value is set without triggering a transaction. This value should only be
				// set during deserialization
				myZoomFactor = value;
			}
		}
		private PointD GetCenterPointValue()
		{
			return myCenterPoint;
		}
		private void SetCenterPointValue(PointD value)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				// This value is set without triggering a transaction. This value should only be
				// set during deserialization
				myCenterPoint = value;
			}
		}
		private bool GetIsActiveDiagramValue()
		{
			return myIsActive;
		}
		private void SetIsActiveDiagramValue(bool value)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				// This value is set without triggering a transaction. This value should only be
				// set during deserialization
				myIsActive = value;
			}
		}
		#endregion // Custom storage handlers
	}
	#endregion // DiagramDisplayHasDiagramOrder class
}
