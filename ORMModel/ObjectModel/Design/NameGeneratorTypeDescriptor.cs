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
	/// <see cref="ElementTypeDescriptor"/> for <see cref="NameGenerator"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class NameGeneratorTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : NameGenerator
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="NameGeneratorTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public NameGeneratorTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}
		#endregion // Constructor
		#region Base overrides
		/// <summary>
		/// Don't create property descriptors for properties that are
		/// modifiers for other settings.
		/// </summary>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			Guid attributeId = domainProperty.Id;
			if (attributeId == NameGenerator.SpacingReplacementDomainPropertyId)
			{
				return ((NameGenerator)requestor).SpacingFormat == NameGeneratorSpacingFormat.ReplaceWith;
			}
			else if (attributeId == NameGenerator.UserDefinedMaximumDomainPropertyId)
			{
				return !((NameGenerator)requestor).UseTargetDefaultMaximum;
			}
			return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
		}
		/// <summary>
		/// Create a custom property descriptor for all properties to
		/// enable custom handling of property serialization and resets
		/// based on the hierarchy.
		/// </summary>
		protected override ElementPropertyDescriptor CreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainPropertyInfo, Attribute[] attributes)
		{
			return new NameGeneratorPropertyDescriptor(this, requestor, domainPropertyInfo, attributes);
		}

		/// <summary>
		/// Add custom property descriptors 
		/// </summary>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection retVal = base.GetProperties(attributes);
			retVal.Add(AbbreviationsPropertyDescriptor.Instance);
			return retVal;
		}
		#endregion // Base overrides
		#region NameGeneratorPropertyDescriptor class
		/// <summary>
		/// A property descriptor with default values based on the refined <see cref="NameGenerator"/>
		/// </summary>
		private class NameGeneratorPropertyDescriptor : ElementPropertyDescriptor
		{
			public NameGeneratorPropertyDescriptor(ElementTypeDescriptor owner, ModelElement modelElement, DomainPropertyInfo domainProperty, Attribute[] attributes)
				: base(owner, modelElement, domainProperty, attributes)
			{
			}
			/// <summary>
			/// Values are non-default if they differ from the refined parent
			/// </summary>
			public override bool ShouldSerializeValue(object component)
			{
				NameGenerator generator = (NameGenerator)component;
				NameGenerator parentGenerator = generator.RefinesGenerator;
				return (parentGenerator != null) ? !DomainPropertyInfo.GetValue(parentGenerator).Equals(DomainPropertyInfo.GetValue(generator)) : base.ShouldSerializeValue(component);
			}
			/// <summary>
			/// Reset the value to the value of the refined generator
			/// </summary>
			public override void ResetValue(object component)
			{
				NameGenerator generator = (NameGenerator)component;
				NameGenerator parentGenerator = generator.RefinesGenerator;
				if (parentGenerator != null)
				{
					// This gives a different transaction name that the default ResetValue, but
					// the correct string is not accessible and the duplication is not worth the effort
					base.SetValue(component, DomainPropertyInfo.GetValue(parentGenerator));
				}
				else
				{
					base.ResetValue(component);
				}
			}
		}
		#endregion // NameGeneratorPropertyDescriptor class
		#region AbbreviationsPropertyDescriptor class
		/// <summary>
		/// A property descriptor to show name alias (aka abbreviations) in the context of this name generator
		/// </summary>
		private class AbbreviationsPropertyDescriptor : PropertyDescriptor
		{
			#region AliasManagerEditor class
			/// <summary>
			/// A <see cref="UITypeEditor"/> to show the <see cref="AliasManagerForm"/>
			/// </summary>
			private class AliasManagerEditor : UITypeEditor
			{
				/// <summary>
				/// Make this a modal editor
				/// </summary>
				public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
				{
					return (context != null) ? UITypeEditorEditStyle.Modal : base.GetEditStyle(context);
				}
				/// <summary>
				/// Show the form to change the value
				/// </summary>
				public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
				{
					AliasManagerForm.Show((NameGenerator)EditorUtility.ResolveContextInstance(context.Instance, false), provider);
					return null;
				}
			}
			#endregion // AliasManagerEditor class
			#region Singleton Accessor
			public static readonly PropertyDescriptor Instance = new AbbreviationsPropertyDescriptor();
			#endregion // Singleton Accessor
			#region Constructor
			private AbbreviationsPropertyDescriptor()
				: base("AbbreviationsPropertyDescriptor", null)
			{
			}
			#endregion // Constructor
			#region Base overrides
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
				return null;
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
			}
			public override void SetValue(object component, object value)
			{
			}
			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}
			public override string Description
			{
				get
				{
					return ResourceStrings.NameGeneratorAbbreviationsPropertyDescriptorDescription;
				}
			}
			public override string DisplayName
			{
				get
				{
					return ResourceStrings.NameGeneratorAbbreviationsPropertyDescriptorDisplayName;
				}
			}
			public override object GetEditor(Type editorBaseType)
			{
				if (editorBaseType == typeof(System.Drawing.Design.UITypeEditor))
				{
					return new AliasManagerEditor();
				}
				return base.GetEditor(editorBaseType);
			}
			#endregion // Base overrides
		}
		#endregion // AbbreviationsPropertyDescriptor class
	}
}
