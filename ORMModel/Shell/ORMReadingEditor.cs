#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.ObjectModel.Editors;

#endregion

namespace Northface.Tools.ORM.Shell
{
	/// <summary>
	/// Defines the tool window that is used to modify the readings associated with a fact.
	/// </summary>
	[Guid("992C221B-4BE5-4A9B-900D-9882B4FA0F99")]
	[CLSCompliant(false)]
	public class ORMReadingEditorToolWindow : ToolWindow
	{
		#region Member variables
		private ReadingsViewForm myForm = new ReadingsViewForm();
		private ORMDesignerDocData myCurrentDocument;
		#endregion // Member variables

		#region construction
		/// <summary>
		/// Creates a new instance of the reading editor tool window.
		/// </summary>
		public ORMReadingEditorToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			IMonitorSelectionService monitor = (IMonitorSelectionService)serviceProvider.GetService(typeof(IMonitorSelectionService));
			monitor.DocumentWindowChanged += new MonitorSelectionEventHandler(DocumentWindowChangedEvent);
			monitor.SelectionChanged += new MonitorSelectionEventHandler(SelectionChangedEvent);
			CurrentDocument = monitor.CurrentDocument as ORMDesignerDocData;
		}
		#endregion

		#region selection monitor event handlers and helpers
		private void DocumentWindowChangedEvent(object sender, MonitorSelectionEventArgs e)
		{
			CurrentDocument = ((IMonitorSelectionService)sender).CurrentDocument as ORMDesignerDocData;
		}

		private void SelectionChangedEvent(object sender, MonitorSelectionEventArgs e)
		{
			ORMDesignerDocView theView = e.NewValue as ORMDesignerDocView;
			if (theView != null)
			{
				FactType theFact = ReadingTextEditor.ResolveUnderlyingFact(theView.PrimarySelection);
				FactType currentFact = EditingFactType;
				if (theFact == null && currentFact != null)
				{
					EditingFactType = null;
				}
				//selection could change between the shapes that are related to the fact
				else if (!object.ReferenceEquals(theFact, currentFact))
				{
					EditingFactType = theFact;
				}
			}
		}
		#endregion

		#region ToolWindow overrides
		/// <summary>
		/// Gets the title that will be displayed on the tool window.
		/// </summary>
		public override string WindowTitle
		{
			get 
			{
				return ResourceStrings.ModelReadingEditorWindowTitle;
			}
		}

		/// <summary>
		/// Gets the window object that will be hosted by the tool window.
		/// </summary>
		public override IWin32Window Window
		{
			get 
			{
				return myForm;
			}
		}
		#endregion

		#region Reading activation helper
		/// <summary>
		/// Select the current reading in the window. The
		/// reading must be the child of the current fact.
		/// </summary>
		/// <param name="reading">Reading</param>
		public void ActivateReading(Reading reading)
		{
			myForm.ActivateReading(reading);
		}

		/// <summary>
		/// Select the primary reading for the reading order
		/// matching the role order of the fact, if there
		/// isn't one activate the new entry for that order.
		/// </summary>
		/// <param name="fact">FactType</param>
		public void ActivateReading(FactType fact)
		{
			myForm.ActivateReading(fact);
		}
		#endregion // Reading activation helper

		#region properties
		/// <summary>
		/// Controls which fact is being displayed by the tool window.
		/// </summary>
		public FactType EditingFactType
		{
			get
			{
				return myForm.EditingFactType;
			}
			set
			{
				myForm.EditingFactType = value;
			}
		}

		private ORMDesignerDocData CurrentDocument
		{
			set
			{
				if (myCurrentDocument != null)
				{
					if (value != null && object.ReferenceEquals(myCurrentDocument, value))
					{
						return;
					}
					myForm.ReadingEditor.DetachEventHandlers(myCurrentDocument.Store);
				}
				myCurrentDocument = value;
				if (value != null)
				{
					myForm.ReadingEditor.AttachEventHandlers(myCurrentDocument.Store);
				}
				else
				{
					EditingFactType = null;
				}
			}
		}
		#endregion

		#region nested class ReadingsViewForm
		private class ReadingsViewForm : Form
		{
			private ReadingEditor myReadingEditor;
			private Label myNoSelectionLabel;

			#region construction
			public ReadingsViewForm()
			{
				Initialize();
			}

			private void Initialize()
			{
				myReadingEditor = new ReadingEditor();
				this.Controls.Add(myReadingEditor);
				myReadingEditor.Dock = DockStyle.Fill;
				myReadingEditor.Visible = false;

				myNoSelectionLabel = new Label();
				myNoSelectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
				myNoSelectionLabel.Text = ResourceStrings.ModelReadingEditorUnsupportedSelectionText;
				this.Controls.Add(myNoSelectionLabel);
				myNoSelectionLabel.Dock = DockStyle.Fill;
				myNoSelectionLabel.Visible = true;
			}
			#endregion

			#region properties
			public FactType EditingFactType
			{
				get
				{
					return myReadingEditor.EditingFactType;
				}
				set
				{
					bool editVisible = value != null;
					myReadingEditor.Visible = editVisible;
					myNoSelectionLabel.Visible = !editVisible;
					myReadingEditor.EditingFactType = value;
				}
			}

			public ReadingEditor ReadingEditor
			{
				get
				{
					return myReadingEditor;
				}
			}
			#endregion

			#region Reading activation helper
			/// <summary>
			/// Select the current reading in the window. The
			/// reading must be the child of the current fact.
			/// </summary>
			/// <param name="reading">Reading</param>
			public void ActivateReading(Reading reading)
			{
				myReadingEditor.ActivateReading(reading);
			}

			/// <summary>
			/// Select the primary reading of the order matching
			/// the display order of the fact, if one doesn't
			/// exist select the new entry.
			/// </summary>
			/// <param name="fact"></param>
			public void ActivateReading(FactType fact)
			{
				myReadingEditor.ActivateReading(fact);
			}
			#endregion // Reading activation helper
		}
		#endregion
	}
}
