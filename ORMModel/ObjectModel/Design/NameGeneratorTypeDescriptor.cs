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
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Neumont.Tools.ORM.Shell;
using System.Windows.Forms;

namespace Neumont.Tools.ORM.ObjectModel.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="DataType"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class NameGeneratorTypeDescriptor<TModelElement> : ElementTypeDescriptor<TModelElement>
		where TModelElement : NameGenerator
	{
		/// <summary>
		/// Initializes a new instance of <see cref="DataTypeTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public NameGeneratorTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}

		/// <summary>
		/// Add custom property descriptors 
		/// </summary>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection retVal = base.GetProperties(attributes);
			// UNDONE: Put back in to complete implementation of NameAlias dialog
			// retVal.Add(NameAliasPropertyDescriptor.Instance);
			return retVal;
		}

		/// <summary>
		/// Don't automatically display embedded relationship
		/// </summary>
		protected override bool IncludeEmbeddingRelationshipProperties(ModelElement requestor)
		{
			return true;
		}

		/// <summary>
		/// Don't automatically display relationships
		/// </summary>
		protected override bool IncludeOppositeRolePlayerProperties(ModelElement requestor)
		{
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		private class NameAliasPropertyDescriptor : PropertyDescriptor
		{
			public static readonly NameAliasPropertyDescriptor Instance = new NameAliasPropertyDescriptor();

			private NameAliasPropertyDescriptor()
				: base("NameAliasProperty", null)
			{

			}

			public override bool CanResetValue(object component)
			{
				return false;
			}

			public override Type ComponentType
			{
				get
				{
					return typeof(NameGenerator);
				}
			}

			public override object GetValue(object component)
			{
				return "Edit Aliases";
			}


			public override bool IsReadOnly
			{
				get { return true; }
			}


			public override Type PropertyType
			{
				get { return typeof(string); }
			}


			public override void ResetValue(object component)
			{
				//Fluff to keep the compiler happy.
			}

			public override void SetValue(object component, object value)
			{
				//Fluff to keep the compiler happy.
			}

			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}

			public override string Description
			{
				get
				{
					return base.Description;
				}
			}

			public override string DisplayName
			{
				get
				{
					return "Alias Tool";
				}
			}

			public override object GetEditor(Type editorBaseType)
			{
				if (editorBaseType == typeof(System.Drawing.Design.UITypeEditor))
				{
					return new AliasManager();
				}
				return base.GetEditor(editorBaseType);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public partial class AliasManager : UITypeEditor
		{
			/// <summary>
			/// 
			/// </summary>
			/// <param name="context"></param>
			/// <returns></returns>
			public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
			{
				if (context != null) return UITypeEditorEditStyle.Modal;
				return base.GetEditStyle(context);
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="context"></param>
			/// <param name="provider"></param>
			/// <param name="value"></param>
			/// <returns></returns>
			public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				AliasManagerForm.Show((NameGenerator)EditorUtility.ResolveContextInstance(context.Instance, false), provider);
				return null;
			}
		}
	
	}
}
