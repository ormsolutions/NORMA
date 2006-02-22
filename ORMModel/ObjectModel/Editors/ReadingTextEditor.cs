#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Drawing;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.Shell;

#endregion

namespace Neumont.Tools.ORM.ObjectModel.Editors
{
	/// <summary>
	/// Type editor for use on ReadingText properties in the property grid.
	/// </summary>
	public class ReadingTextEditor : UITypeEditor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ReadingTextEditor()
		{
		}

		/// <summary>
		/// Changes the style of the editor.
		/// </summary>
		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		/// <summary>
		/// Called when a value using this editor is modified.
		/// </summary>
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			object retval = value;
			IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (edSvc != null)
			{
				FactType fact = EditorUtility.ResolveContextFactType(context.Instance);
				if (fact != null)
				{
					ORMReadingEditorToolWindow editorWindow = ORMDesignerPackage.ReadingEditorWindow;
					editorWindow.EditingFactType = fact;
					editorWindow.Show();
				}
			}
			return retval;
		}
	}
}
