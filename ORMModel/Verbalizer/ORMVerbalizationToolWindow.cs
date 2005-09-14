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
		private const string HtmlIncreaseIndent = @"<span style=""left:30px;position:relative"">";
		private const string HtmlDecreaseIndent = @"</span>";
		#endregion // Constants
		#region Member variables
		private WebBrowser myWebBrowser;
		private ORMDesignerDocView myCurrentDocumentView;
		private StringWriter myStringWriter;

		/// <summary>
		/// Callback for child verbalizations
		/// </summary>
		private delegate bool VerbalizationHandler(IVerbalize verbalizer, int indentationLevel);
		#endregion // Member variables
		#region Construction
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
			CurrentDocumentView = monitor.CurrentDocumentView as ORMDesignerDocView;
		}
		#endregion // Construction
		#region Selection monitor event handlers and helpers
		private void DocumentWindowChangedEvent(object sender, MonitorSelectionEventArgs e)
		{
			CurrentDocumentView = ((IMonitorSelectionService)sender).CurrentDocumentView as ORMDesignerDocView;
		}
		private void SelectionChangedEvent(object sender, MonitorSelectionEventArgs e)
		{
			UpdateVerbalization();
		}
		private void ModelStateChangedEvent(object sender, ElementEventsEndedEventArgs e)
		{
			UpdateVerbalization();
		}
		private ORMDesignerDocView CurrentDocumentView
		{
			get
			{
				return myCurrentDocumentView;
			}
			set
			{
				ORMDesignerDocView oldView = myCurrentDocumentView;
				if (oldView != null)
				{
					ORMDesignerDocData oldDoc = oldView.DocData as ORMDesignerDocData;
					if (value != null)
					{
						if (object.ReferenceEquals(oldView, value))
						{
							return;
						}
						else if (object.ReferenceEquals(oldDoc, value.DocData))
						{
							myCurrentDocumentView = value;
							return;
						}
					}
					if (oldDoc != null)
					{
						Store store = oldDoc.Store;
						if (!store.Disposed)
						{
							store.EventManagerDirectory.ElementEventsEnded.Remove(new ElementEventsEndedEventHandler(ModelStateChangedEvent));
						}
					}
				}
				myCurrentDocumentView = value;
				if (value != null)
				{
					ORMDesignerDocData docData = value.DocData as ORMDesignerDocData;
					if (docData != null)
					{
						docData.Store.EventManagerDirectory.ElementEventsEnded.Add(new ElementEventsEndedEventHandler(ModelStateChangedEvent));
					}
				}
			}
		}
		#endregion // Selection monitor event handlers and helpers
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
		#region Verbalization Implementation
		private void UpdateVerbalization()
		{
			ORMDesignerDocView theView = CurrentDocumentView;
			if (theView == null)
			{
				return;
			}

			myStringWriter.GetStringBuilder().Length = 0;

			ICollection selectedObjects = theView.GetSelectedComponents();
			bool isNegative = false; // UNDONE: Get this value from somewhere real
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
					VerbalizeElement(mel, isNegative, myStringWriter);
				}
			}
			myWebBrowser.DocumentText = myStringWriter.ToString();
		}
		/// <summary>
		/// Determine the indentation level for verbalizing a ModelElement, and fire
		/// the delegate for verbalization
		/// </summary>
		/// <param name="element">The element to verbalize</param>
		/// <param name="isNegative">Use the negative form of the reading</param>
		/// <param name="writer">The TextWriter for verbalization output</param>
		public static void VerbalizeElement(ModelElement element, bool isNegative, TextWriter writer)
		{
			int lastLevel = 0;
			bool firstWrite = true;
			VerbalizeElement(
				element,
				delegate(IVerbalize verbalizer, int indentationLevel)
				{
					return verbalizer.GetVerbalization(
						writer,
						delegate(VerbalizationContent content)
						{
							// UNDONE: Tags for error content, which always
							// comes through as straight text

							// Prepare for verbalization on this element. Everything
							// is delayed to this point in case the verbalization implementation
							// does not callback to the text writer.
							if (firstWrite)
							{
								firstWrite = false;
							}
							else
							{
								writer.WriteLine();
							}

							// Write indentation tags as needed
							if (indentationLevel > lastLevel)
							{
								do
								{
									writer.Write(HtmlIncreaseIndent);
									++lastLevel;
								} while (lastLevel != indentationLevel);
							}
							else if (lastLevel > indentationLevel)
							{
								do
								{
									writer.Write(HtmlDecreaseIndent);
									--lastLevel;
								} while (lastLevel != indentationLevel);
							}
						},
						isNegative);
				},
				0);
			while (lastLevel > 0)
			{
				writer.Write(HtmlDecreaseIndent);
				--lastLevel;
			}
		}
		/// <summary>
		/// Verbalize the passed in element and all its children
		/// </summary>
		private static void VerbalizeElement(ModelElement element, VerbalizationHandler callback, int indentLevel)
		{
			IVerbalize parentVerbalize = element as IVerbalize;
			if (parentVerbalize != null)
			{
				if (callback(parentVerbalize, indentLevel))
				{
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
							VerbalizeElement((ModelElement)children[j], callback, indentLevel);
						}
					}
				}
			}
		}
		#endregion // Verbalization Implementation
	}
}
