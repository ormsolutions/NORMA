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

namespace Neumont.Tools.ORM.Framework
{
	partial class MultiDiagramDocView
	{
		private sealed class DiagramTabPage : TabPage
		{
			private readonly MultiDiagramDocViewControl myDocViewControl;
			public readonly DiagramView Designer;
			public Diagram Diagram
			{
				[DebuggerStepThrough]
				get
				{
					return Designer.Diagram;
				}
			}

			#region Constructor
			public DiagramTabPage(MultiDiagramDocViewControl docViewControl, DiagramView designer)
			{
				myDocViewControl = docViewControl;
				Designer = designer;
				base.SuspendLayout();
				base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
				base.SetStyle(ControlStyles.Selectable, false);
				base.UseVisualStyleBackColor = false;
				Diagram diagram = designer.Diagram;
				base.Text = base.Name = diagram.Name;
				base.Controls.Add(designer);
				docViewControl.TabPages.Add(this);
				base.ImageKey = diagram.GetType().GUID.ToString("N", null);
				base.ResumeLayout(false);
				diagram.Store.EventManagerDirectory.ElementPropertyChanged.Add(diagram.GetDomainClass().NameDomainProperty, diagram.Id, (EventHandler<ElementPropertyChangedEventArgs>)DiagramNameChanged);
				designer.DiagramClientView.DiagramDisassociating += DiagramDisassociating;
			}
			#endregion // Constructor
			
			#region Event handlers
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
			protected override void Dispose(bool disposing)
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
			protected override void OnGotFocus(EventArgs e)
			{
				DiagramView designer = Designer;
				if (designer != null)
				{
					designer.DiagramClientView.Focus();
				}
			}
			#endregion // OnGotFocus method

			#region ResetText method
			public override void ResetText()
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
			public override string Text
			{
				get
				{
					// HACK: The rectangles returned by Windows for each tab are far too large when ControlStyles.UserPaint
					// is enabled, so we are only returning about 80% of the text to Windows to force it to give us a more
					// appropriately sized rectangle.
					// An investigation into what messages are processed differently when UserPaint is enabled may lead to a
					// more elegant workaround.
					string text = base.Text;
					int textLength = text.Length;
					int reducedLength = (int)(text.Length * 0.8);
					return (reducedLength < textLength) ? text.Remove(reducedLength) : text;
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
								DomainClassInfo.SetName(diagram, value);
							}
							// base.Text will be updated via the DiagramNameChanged event handler
						}
					}
				}
			}
			#endregion // Text property

			#region Font property
			// This is not currently used by TabControl (it just uses its own), but just in case that changes in the future...
			public override Font Font
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
			protected override Padding DefaultMargin
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
