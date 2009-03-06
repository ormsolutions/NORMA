#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using ORMSolutions.ORMArchitect.Framework;
using Microsoft.VisualStudio.Modeling;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	partial class DiagramDisplayDomainModel : IDeserializationFixupListenerProvider
	{
		/// <summary>
		/// An enum to return as a fixup phase type
		/// </summary>
		private enum DiagramDisplayFixupPhase
		{
			/// <summary>
			/// Load and validate diagram display elements, recreating as needed
			/// </summary>
			ValidateImplicitStoredPresentationElements = StandardFixupPhase.ValidateImplicitStoredPresentationElements,
		}
		#region IDeserializationFixupListenerProvider Implementation
		/// <summary>
		/// Implements <see cref="IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection"/>
		/// </summary>
		protected static IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				yield return new DiagramDisplayFixupListener();
			}
		}
		IEnumerable<IDeserializationFixupListener> IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		{
			get
			{
				return DeserializationFixupListenerCollection;
			}
		}
		/// <summary>
		/// Implements <see cref="IDeserializationFixupListenerProvider.DeserializationFixupPhaseType"/>
		/// </summary>
		protected static Type DeserializationFixupPhaseType
		{
			get
			{
				return typeof(DiagramDisplayFixupPhase);
			}
		}
		Type IDeserializationFixupListenerProvider.DeserializationFixupPhaseType
		{
			get
			{
				return DeserializationFixupPhaseType;
			}
		}
		#endregion // IDeserializationFixupListenerProvider Implementation
		#region Fixup Listener
		/// <summary>
		/// Fixup listener implementation. Adds display order information
		/// </summary>
		private sealed class DiagramDisplayFixupListener : DeserializationFixupListener<Diagram>
		{
			private DiagramDisplay myDiagramDisplay;
			private bool mySeenActive;
			/// <summary>
			/// DiagramDisplayFixupListener constructor
			/// </summary>
			public DiagramDisplayFixupListener()
				: base((int)DiagramDisplayFixupPhase.ValidateImplicitStoredPresentationElements)
			{
			}
			/// <summary>
			/// Process diagram items to make sure they have a current ordering representation
			/// </summary>
			protected sealed override void ProcessElement(Diagram element, Store store, INotifyElementAdded notifyAdded)
			{
				DiagramDisplayHasDiagramOrder link = DiagramDisplayHasDiagramOrder.GetLinkToDiagramDisplay(element);
				DiagramDisplay container = myDiagramDisplay;
				if (container == null)
				{
					if (link != null)
					{
						container = link.DiagramDisplay;
					}

					// Make sure we only have one container, use the one we've already grabbed
					ReadOnlyCollection<DiagramDisplay> containers = store.ElementDirectory.FindElements<DiagramDisplay>(false);
					int containerCount = containers.Count;
					for (int i = containerCount - 1; i >= 0; --i)
					{
						DiagramDisplay testContainer = containers[i];
						if (container != null)
						{
							if (testContainer != container)
							{
								testContainer.Delete();
							}
						}
						else if (i == 0)
						{
							container = testContainer;
						}
						else
						{
							testContainer.Delete();
						}
					}
					if (container == null)
					{
						container = new DiagramDisplay(store);
						notifyAdded.ElementAdded(container, false);
					}
					myDiagramDisplay = container;
					if (link != null)
					{
						// There is nothing else to do, the element has been validated
						if (link.IsActiveDiagram)
						{
							if (mySeenActive)
							{
								link.IsActiveDiagram = false;
							}
							else
							{
								mySeenActive = true;
							}
						}
						return;
					}
				}
				if (link == null)
				{
					// This will add to the end of the existing collection
					link = new DiagramDisplayHasDiagramOrder(container, element);
					notifyAdded.ElementAdded(link, false);
				}
				else
				{
					if (link.DiagramDisplay != container)
					{
						link.DiagramDisplay = container;
					}
					if (link.IsActiveDiagram)
					{
						if (mySeenActive)
						{
							link.IsActiveDiagram = false;
						}
						else
						{
							mySeenActive = true;
						}
					}
				}
			}
			/// <summary>
			/// Use the first element as the active diagram if one is not specified
			/// </summary>
			protected override void PhaseCompleted(Store store)
			{
				DiagramDisplay container;
				if (!mySeenActive &&
					null != (container = myDiagramDisplay))
				{
					foreach (DiagramDisplayHasDiagramOrder link in DiagramDisplayHasDiagramOrder.GetLinksToOrderedDiagramCollection(container))
					{
						link.IsActiveDiagram = true;
						break;
					}
				}
			}
		}
		#endregion // Fixup Listener
	}
}
