using System;
using System.Collections.Generic;
using System.Text;
using Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid;
using Neumont.Tools.ORM.ObjectModel;
using System.Windows.Forms;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Tool window to contain survey tree control
	/// </summary>
	[CLSCompliant(false)]
	public class NewORMModelBrowser : ORMToolWindow
	{
		private SurveyTreeControl myTreeControl;
		/// <summary>
		/// public constructor
		/// </summary>
		/// <param name="serviceProvider"></param>
		public NewORMModelBrowser(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			
		}
		#region LoadWindow method
		/// <summary>
		/// Loads the SurveyTreeControl from the current document
		/// </summary>
		protected void LoadWindow()
		{
			SurveyTreeControl treeControl = myTreeControl;
			if (treeControl == null)
			{
				myTreeControl = treeControl = new SurveyTreeControl();
			}
			ORMDesignerDocData currentDocument = this.CurrentDocument;
			treeControl.Tree = (currentDocument != null) ? currentDocument.SurveyTree : null;
		}
		#endregion //LoadWindow method
		#region ORMToolWindow overrides
		/// <summary>
		/// currently unimplimented, all events handled by tree directly
		/// </summary>
		/// <param name="store"></param>
		protected override void AttachEventHandlers(Microsoft.VisualStudio.Modeling.Store store) 
		{
			
		}
		/// <summary>
		/// currently unimplemented, all events handled by tree directly
		/// </summary>
		/// <param name="store"></param>
		protected override void DetachEventHandlers(Microsoft.VisualStudio.Modeling.Store store)
		{
			
		}
		/// <summary>
		/// called when document current selected document changes
		/// </summary>
		protected override void OnCurrentDocumentChanged()
		{
			base.OnCurrentDocumentChanged();
			LoadWindow();
		}
		/// <summary>
		/// returns string to be displayed as window title
		/// </summary>
		public override string WindowTitle //TODO: LOCALIZE
		{
			get
			{
				return "NewORMModelBrowser";
			}
		}
		/// <summary>
		/// returns int value for window icon
		/// </summary>
		protected override int BitmapResource //TODO: find correct bitmap resource to use
		{
			get
			{
				return 125;
			}
		}
		/// <summary>
		/// returns index for window icon
		/// </summary>
		protected override int BitmapIndex //TODO: find correct bitmap index to use
		{
			get
			{
				return 4;
			}
		}
		/// <summary>
		/// retuns the SurveyTreeControl this window contains
		/// </summary>
		public override System.Windows.Forms.IWin32Window Window
		{
			get
			{
				if (myTreeControl == null)
				{
					LoadWindow();
				}
				return myTreeControl;
			}
		}
		#endregion //ORMToolWindow overrides
	}
}
