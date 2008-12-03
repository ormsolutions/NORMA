#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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

namespace Neumont.Tools.Modeling.Shell
{
	partial class MultiDiagramDocView
	{
		private sealed class DiagramTabPage : TabPage
		{
			#region Member Variables
			private readonly MultiDiagramDocViewControl myDocViewControl;
			private bool myTurnOffResizeEventInFocusEvent;
			public readonly DiagramView Designer;
			#endregion // Member Variables
			#region Constructor
			public DiagramTabPage(MultiDiagramDocViewControl docViewControl, DiagramView designer)
			{
				myDocViewControl = docViewControl;
				Designer = designer;
				base.SuspendLayout();
				base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
				base.UseVisualStyleBackColor = false;
				Diagram diagram = designer.Diagram;
				base.Text = base.Name = diagram.Name;
				base.Controls.Add(designer);
				// Find the correct tab location for this diagram, depending on the diagram order and the
				// pages that have already been added
				TabControl.TabPageCollection pages = docViewControl.TabPages;
				bool inserted = false;
				Store store = diagram.Store;
				bool useDiagramDisplay = store.FindDomainModel(DiagramDisplayDomainModel.DomainModelId) != null;
				if (useDiagramDisplay)
				{
					DiagramDisplay display = DiagramDisplayHasDiagramOrder.GetDiagramDisplay(diagram);
					if (display != null)
					{
						// Walk the existing pages and match up the expected display order. Note that
						// there is no guarantee that all of the preceding diagrams have tab pages already.
						// If the previous pages are out of order, then we will get a reorder event later on
						// that puts them in the correct order. This will add them in an unpredictable order
						// if the sequences are different.
						IList<Diagram> orderedDiagrams = display.OrderedDiagramCollection;
						int diagramCount = orderedDiagrams.Count;
						int nextDiagramIndex = 0;
						Diagram nextDiagram = orderedDiagrams[nextDiagramIndex];
						int pageCount = pages.Count;
						if (nextDiagram == diagram)
						{
							if (pageCount != 0)
							{
								// The new diagram is first, insert at the beginning
								pages.Insert(0, this);
								inserted = true;
							}
						}
						else
						{
							for (int pageIndex = 0; pageIndex < pageCount && !inserted; ++pageIndex)
							{
								DiagramTabPage page = (DiagramTabPage)pages[pageIndex];
								Diagram pageDiagram = page.Diagram;
								bool getNextDiagram = false;
								if (pageDiagram == nextDiagram)
								{
									getNextDiagram = true;
								}
								else
								{
									// Keep walking diagrams until we get a match
									while (nextDiagramIndex < diagramCount)
									{
										nextDiagram = orderedDiagrams[++nextDiagramIndex];
										if (pageDiagram == nextDiagram)
										{
											getNextDiagram = true;
										}
										else if (nextDiagram == diagram)
										{
											if ((pageIndex + 1) < pageCount)
											{
												pages.Insert(pageIndex + 1, this);
												inserted = true;
												break;
											}
										}
									}
								}
								if (getNextDiagram)
								{
									if (nextDiagramIndex < diagramCount)
									{
										nextDiagram = orderedDiagrams[++nextDiagramIndex];
										if (nextDiagram == diagram)
										{
											// Insert immediately after the current page
											if ((pageIndex + 1) < pageCount)
											{
												pages.Insert(pageIndex + 1, this);
												inserted = true;
											}
											break;
										}
									}
									else
									{
										break;
									}
								}
							}
						}
					}
				}
				if (!inserted)
				{
					pages.Add(this);
				}
				// If the image key is set before the page is inserted then the tab size is incorrect
				// and nothing draws property. I have no idea why.
				base.ImageKey = diagram.GetType().GUID.ToString("N", null);
				base.ResumeLayout(false);
				store.EventManagerDirectory.ElementPropertyChanged.Add(diagram.GetDomainClass().NameDomainProperty, diagram.Id, (EventHandler<ElementPropertyChangedEventArgs>)DiagramNameChanged);
				if (useDiagramDisplay)
				{
					designer.DiagramClientView.GotFocus += new EventHandler(ViewGotFocus);
					designer.DiagramClientView.Resize += new EventHandler(InitialViewResize);
					myTurnOffResizeEventInFocusEvent = true;
				}
				designer.DiagramClientView.DiagramDisassociating += DiagramDisassociating;
			}
			#endregion // Constructor
			#region Accessor Properties
			public Diagram Diagram
			{
				[DebuggerStepThrough]
				get
				{
					return Designer.Diagram;
				}
			}
			#endregion // Accessor Properties
			#region Event handlers
			private static void InitialViewResize(object sender, EventArgs e)
			{
				DiagramClientView view;
				Diagram diagram;
				DiagramDisplayHasDiagramOrder link;
				if (null != (view = sender as DiagramClientView) &&
					null != (diagram = view.Diagram))
				{
					if (null != (link = DiagramDisplayHasDiagramOrder.GetLinkToDiagramDisplay(diagram)))
					{
						// Update the display position from the diagram display information
						float desiredZoomFactor = link.ZoomFactor;
						if (desiredZoomFactor != view.ZoomFactor)
						{
							view.SetZoomFactor(desiredZoomFactor, link.CenterPoint, true);
						}
						else
						{
							view.ScrollTo(new PointD(link.CenterPoint.X - view.ViewBounds.Width / 2, link.CenterPoint.Y - view.ViewBounds.Height / 2));
						}
					}
				}
			}
			private void ViewGotFocus(object sender, EventArgs e)
			{
				DiagramClientView view;
				Diagram diagram;
				DiagramDisplayHasDiagramOrder link;
				if (null != (view = sender as DiagramClientView) &&
					null != (diagram = view.Diagram) &&
					null != (link = DiagramDisplayHasDiagramOrder.GetLinkToDiagramDisplay(diagram)))
				{
					if (myTurnOffResizeEventInFocusEvent)
					{
						// Use the cached until the view has been focused once, then
						// the view gets control of the settings
						myTurnOffResizeEventInFocusEvent = false;
						view.Resize -= new EventHandler(InitialViewResize);
					}
					link.UpdatePosition(view.ViewBounds.Center, view.ZoomFactor);
					link.Activate();
					view.ScrollPositionChanged += new ScrollPositionChangedEventHandler(ViewDisplayChanged);
					view.ZoomChanged += new ZoomChangedEventHandler(ViewDisplayChanged);
					view.Resize += new EventHandler(ViewSizeChanged);
					view.LostFocus += new EventHandler(ViewLostFocus);
				}
			}
			private static void ViewLostFocus(object sender, EventArgs e)
			{
				DiagramClientView view;
				if (null != (view = sender as DiagramClientView))
				{
					view.LostFocus -= new EventHandler(ViewLostFocus);
					view.ScrollPositionChanged -= new ScrollPositionChangedEventHandler(ViewDisplayChanged);
					view.ZoomChanged -= new ZoomChangedEventHandler(ViewDisplayChanged);
					view.Resize -= new EventHandler(ViewSizeChanged);
				}
			}
			private static void ViewDisplayChanged(object sender, DiagramEventArgs e)
			{
				DiagramClientView view = e.DiagramClientView;
				Diagram diagram;
				DiagramDisplayHasDiagramOrder link;
				if (null != (diagram = view.Diagram) &&
					null != (link = DiagramDisplayHasDiagramOrder.GetLinkToDiagramDisplay(diagram)))
				{
					link.UpdatePosition(view.ViewBounds.Center, view.ZoomFactor);
				}
			}
			private static void ViewSizeChanged(object sender, EventArgs e)
			{
				DiagramClientView view;
				Diagram diagram;
				DiagramDisplayHasDiagramOrder link;
				if (null != (view = sender as DiagramClientView) &&
					null != (diagram = view.Diagram) &&
					null != (link = DiagramDisplayHasDiagramOrder.GetLinkToDiagramDisplay(diagram)))
				{
					link.UpdatePosition(view.ViewBounds.Center, view.ZoomFactor);
				}
			}
			private void DiagramNameChanged(object sender, ElementPropertyChangedEventArgs e)
			{
				string newName = ((Diagram)e.ModelElement).Name;
				if (base.Name != newName)
				{
					base.Name = newName;
				}
				if (base.Text != newName)
				{
					base.Text = newName;
				}
			}
			private bool myDisassociating;
			
			private void DiagramDisassociating(object sender, EventArgs e)
			{
				if (!base.IsDisposed && !myDisassociating)
				{
					try
					{
						myDisassociating = true;
						DiagramView designer = Designer;
						Diagram diagram = designer.Diagram;
						if (diagram != null)
						{
							diagram.Store.EventManagerDirectory.ElementPropertyChanged.Remove(diagram.GetDomainClass().NameDomainProperty, diagram.Id, (EventHandler<ElementPropertyChangedEventArgs>)DiagramNameChanged);
						}
						myDocViewControl.DocView.RemoveDesigner(designer);
					}
					finally
					{
						myDisassociating = false;
					}
				}
			}
			#endregion // Event handlers
			#region Dispose method
			protected sealed override void Dispose(bool disposing)
			{
				if (disposing && !base.IsDisposed)
				{
					// HACK: BUG: UNDONE: TODO: BUGBUG: MSBUG: DIRTY_HACK: Don't dispose the DiagramClientView if it is in the process of disassociating.
					// Bad things happen if we do.
					if (!myDisassociating)
					{
						DiagramView designer = Designer;
						DiagramClientView designerClient = designer.DiagramClientView;
						Diagram diagram = designer.Diagram;
						if (diagram != null)
						{
							diagram.Disassociate(designer);
						}
						designerClient.DiagramDisassociating -= DiagramDisassociating;
						designerClient.Dispose();
						designer.Dispose();
					}
					else
					{
						base.Controls.Clear();
					}
				}
				base.Dispose(disposing);
			}
			#endregion // Dispose method
			#region OnGotFocus method
			protected sealed override void OnGotFocus(EventArgs e)
			{
				DiagramView designer = Designer;
				if (designer != null)
				{
					designer.DiagramClientView.Focus();
				}
			}
			#endregion // OnGotFocus method
			#region ResetText method
			public sealed override void ResetText()
			{
				Diagram diagram = Diagram;
				if (diagram != null)
				{
					string diagramName = diagram.Name;
					if (base.Text != diagramName)
					{
						base.Text = diagramName;
					}
				}
			}
			#endregion
			#region Text property
			public sealed override string Text
			{
				get
				{
					return base.Text;
				}
				set
				{
					// We don't ever want to clear the value without providing a new one.
					if (!string.IsNullOrEmpty(value))
					{
						DiagramView designer = Designer;
						// Ignore any changes to Text when we don't have a DiagramView (e.g. while being constructed and initialized)
						if (designer != null)
						{
							Diagram diagram = designer.Diagram;
							// We should always have a Diagram if we have a Designer
							Debug.Assert(diagram != null);
							// Our text and the Diagram's name should always be in sync already when this method is called
							Debug.Assert(diagram.Name == base.Text);
							if (diagram.Name != value)
							{
								new ElementPropertyDescriptor(diagram, diagram.GetDomainClass().NameDomainProperty, null).SetValue(diagram, value);
							}
							// base.Text will be updated via the DiagramNameChanged event handler
						}
					}
				}
			}
			#endregion // Text property
			#region Font property
			// This is not currently used by TabControl (it just uses its own), but just in case that changes in the future...
			public sealed override Font Font
			{
				get
				{
					return myDocViewControl.Font;
				}
				set
				{
					myDocViewControl.Font = value;
				}
			}
			#endregion // Font property
			#region DefaultMargin property
			protected sealed override Padding DefaultMargin
			{
				get
				{
					return Padding.Empty;
				}
			}
			#endregion // DefaultMargin property
		}
	}
}
