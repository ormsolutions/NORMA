#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Drawing;
using Microsoft.VisualStudio.Modeling;
using Northface.Tools.ORM.Shell;

#endregion

namespace Northface.Tools.ORM.ObjectModel.Editors
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
				FactType fact = ResolveUnderlyingFact(context.Instance);
				if (fact != null)
				{
					ORMReadingEditorToolWindow editorWindow = ORMDesignerPackage.ReadingEditorWindow;
					editorWindow.EditingFactType = fact;
					editorWindow.Show();
				}
			}
			return retval;
		}

		/// <summary>
		/// Selection context is often based on a wrapper shape, such
		/// as a NodeShape or a tree node in a model browser. Use this
		/// helper function to resolve known element containers to get to the
		/// backing element.
		/// </summary>
		/// <param name="instance">The selected object returned by ITypeDescriptorContext.Instance</param>
		/// <returns>A resolved object, or the starting instance if the item is not wrapped.</returns>
		public static FactType ResolveUnderlyingFact(object instance)
		{
			instance = EditorUtility.ResolveContextInstance(instance, true);
			FactType retval = null;
			ModelElement elem;
			Role role;
			InternalConstraint internalConstraint;

			if (null != (role = instance as Role))
			{
				//this one coming straight through on the selection so handling
				//and returning here.
				return role.FactType;
			}
			else if (null != (internalConstraint = instance as InternalConstraint))
			{
				return internalConstraint.FactType;
			}
			else
			{
				elem = instance as ModelElement;
			}

			if (elem != null)
			{
				FactType fact = elem as FactType;
				if (fact != null)
				{
					return fact;
				}

				Reading reading = elem as Reading;
				if (reading != null)
				{
					return reading.ReadingOrder.FactType;
				}

				ReadingOrder readingOrder = elem as ReadingOrder;
				if (readingOrder != null)
				{
					return readingOrder.FactType;
				}

				ObjectType objType = elem as ObjectType;
				if (objType != null)
				{
					return objType.NestedFactType;
				}
			}

			return retval;
		}

	}
}
