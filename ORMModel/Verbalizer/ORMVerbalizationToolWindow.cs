using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Neumont.Tools.ORM.ObjectModel;

// TODO: Add something like: (AttachEventHandlers)
// eventDirectory.TransactionCommitted.Add(new TransactionCommittedEventHandler(TransactionCommittedEvent));
// On the store when it changes.
// Then for DetachEventHandlers
// eventDirectory.TransactionCommitted.Remove(new TransactionCommittedEventHandler(TransactionCommittedEvent));
namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// ToolWindow for hosting a web browser for verbalizations
	/// </summary>
	[Guid("C9AA5E71-9193-46c9-971A-CB6365ACA338")]
	[CLSCompliant(false)]
	public class ORMVerbalizationToolWindow : ToolWindow
	{
		#region Constants
		private const string HtmlNewLine = "<br/>\n";
		#endregion //Constants
		#region Member variables
		private WebBrowser myWebBrowser;
		private ORMDesignerDocData myCurrentDocument;
		private StringWriter myStringWriter;

		/// <summary>
		/// Callback for when verbalizations when the selection changes
		/// </summary>
		/// <param name="verbalizer"></param>
		/// <param name="isNegative"></param>
		/// <param name="indentationLevel"></param>
		public delegate void VerbalizationHandler(IVerbalize verbalizer, bool isNegative, int indentationLevel);
		#endregion // Member variables

		#region construction
		/// <summary>
		/// Construct a verbalization window with a monitor selection service
		/// </summary>
		/// <param name="serviceProvider">Service provider</param>
		public ORMVerbalizationToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			// create the string writer to hold the html
			StringBuilder builder = new StringBuilder();
			myStringWriter = new StringWriter(builder, CultureInfo.CurrentUICulture);
			myStringWriter.NewLine = HtmlNewLine;

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
			// Give IVerbalize a StringBuilder to append to

			myStringWriter.GetStringBuilder().Length = 0;
			VerbalizationHandler myHandler = new VerbalizationHandler(HandleVerbalization);

			ORMDesignerDocView theView = e.NewValue as ORMDesignerDocView;
			if (theView != null)
			{
				ICollection selectedObjects = theView.GetSelectedComponents();
				foreach (ModelElement melIter in selectedObjects)
				{
					ModelElement mel = melIter;
					PresentationElement pel = mel as PresentationElement;
					if (pel != null)
					{
						mel = pel.ModelElement;
					}
					if (mel != null)
					{
						VerbalizeElement(mel, false, myHandler);
						//string verbalizedText = verbalize.GetVerbalization(false);
						//myWebBrowser.DocumentText = verbalizedText;
					}
				}
			}
		}
		#endregion

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
					//myForm.ReadingEditor.DetachEventHandlers(myCurrentDocument.Store);
				}
				myCurrentDocument = value;
				if (value != null)
				{
					//myForm.ReadingEditor.AttachEventHandlers(myCurrentDocument.Store);
				}
				else
				{
					//EditingFactType = null;
				}
			}
		}

		#region Overrides
		/// <summary>
		/// Gets the title that will be displayed on the tool window.
		/// </summary>
		public override string WindowTitle
		{
			get
			{
				return ResourceStrings.ModelVerbalizationWindowTitle;
			}
		}

		/// <summary>
		/// Gets the web browser control hosted in the tool window
		/// </summary>
		public override IWin32Window Window
		{
			get 
			{
				WebBrowser browser = myWebBrowser;
				if (browser == null)
				{
					myWebBrowser = browser = new WebBrowser();
					browser.Dock = DockStyle.Fill;
					browser.DocumentText = "";
					// The container magically provides resize support, we don't have
					// to go all the way to a form
					ContainerControl container = new ContainerControl();
					container.Controls.Add(browser);
				}
				return browser.Parent;
			}
		}

		/// <summary>
		/// Clean up any existing objects
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				// TODO: Remove event handlers for monitor selection
				if (myWebBrowser != null)
				{
					(myWebBrowser as IDisposable).Dispose();
					myWebBrowser = null;
				}
				if (myStringWriter != null)
				{
					(myStringWriter as IDisposable).Dispose();
					myStringWriter = null;
				}
			}
		}
		#endregion // Overrides

		#region Verbalization Callback Implementation
		/// <summary>
		/// Determine the indentation level for verbalizing a ModelElement, and fire
		/// the delegate for verbalization
		/// </summary>
		/// <param name="element">The element to verbalize</param>
		/// <param name="isNegative">Use the negative form of the reading</param>
		/// <param name="callback">The handler for getting the verbalization to a string</param>
		public static void VerbalizeElement(ModelElement element, bool isNegative, VerbalizationHandler callback)
		{
			VerbalizeElement(element, isNegative, callback, 0);
		}

		private static void VerbalizeElement(ModelElement element, bool isNegative, VerbalizationHandler callback, int indentLevel)
		{
			IVerbalize parentVerbalize = element as IVerbalize;
			if (parentVerbalize != null)
			{
				callback(parentVerbalize, isNegative, indentLevel);
				++indentLevel;
				IList aggregateList = element.MetaClass.AggregatedRoles;
				int aggregateCount = aggregateList.Count;
				for (int i = 0; i < aggregateCount; ++i)
				{
					MetaRoleInfo roleInfo = (MetaRoleInfo)aggregateList[i];
					IList children = element.GetCounterpartRolePlayers(roleInfo.OppositeMetaRole, roleInfo, false);
					int childCount = children.Count;
					for (int j = 0; j < childCount; ++j)
					{
						VerbalizeElement((ModelElement)children[j], isNegative, callback, indentLevel);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="verbalizer"></param>
		/// <param name="isNegative"></param>
		/// <param name="indentationLevel"></param>
		public void HandleVerbalization(IVerbalize verbalizer, bool isNegative, int indentationLevel)
		{
			verbalizer.GetVerbalization(myStringWriter, isNegative);
			// UNDONE: This is supposed to handle multiple elements
			myWebBrowser.DocumentText = myStringWriter.ToString();
		}
#endregion // Verbalization Callback
	}
}
