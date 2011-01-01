#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	#region AutomatedElementFilterPropertyDescriptor class
	/// <summary>
	/// An <see cref="ElementPropertyDescriptor"/> that can be used to
	/// filter side-effect elements creating while setting the property
	/// by adding a filter to the <see cref="IORMToolServices.AutomatedElementFilter"/>
	/// </summary>
	public class AutomatedElementFilterPropertyDescriptor : ElementPropertyDescriptor
	{
		/// <summary>
		/// Create a new <see cref="AutomatedElementFilterPropertyDescriptor"/>
		/// </summary>
		public AutomatedElementFilterPropertyDescriptor(ElementTypeDescriptor owner, ModelElement modelElement, DomainPropertyInfo domainProperty, Attribute[] attributes)
			: base(owner, modelElement, domainProperty, attributes)
		{
		}
		/// <summary>
		/// Set a value with a filter in place
		/// </summary>
		public override void SetValue(object component, object value)
		{
			IORMToolServices toolServices = null;
			ModelElement element;
			AutomatedElementFilterCallback callback = null;
			if (null != (element = component as ModelElement) &&
				null != (toolServices = element.Store as IORMToolServices))
			{
				callback = FilterAutomatedElement;
				toolServices.AutomatedElementFilter += callback;
			}
			try
			{
				base.SetValue(component, value);
			}
			finally
			{
				if (toolServices != null)
				{
					toolServices.AutomatedElementFilter -= callback;
				}
			}
		}
		/// <summary>
		/// Return <see cref="AutomatedElementDirective.Ignore"/> to classify the provided <see cref="ModelElement"/>
		/// as an automated element.
		/// The default implementation classifies all elements as automated.
		/// </summary>
		protected virtual AutomatedElementDirective FilterAutomatedElement(ModelElement element)
		{
			return AutomatedElementDirective.Ignore;
		}
	}
	#endregion // AutomatedElementFilterPropertyDescriptor class
	#region AutomatedElementFilterPropertyDescriptor class
	/// <summary>
	/// An <see cref="PropertyDescriptor"/> that can be used to
	/// filter side-effect elements creating while setting the property
	/// by adding a filter to the <see cref="IORMToolServices.AutomatedElementFilter"/>.
	/// Unlike <see cref="AutomatedElementFilterPropertyDescriptor"/>, this descriptor
	/// is designed to be used with properties that are not specified in the domain library
	/// but should appear alongside the domain properties.
	/// </summary>
	public class AutomatedElementFilterCustomPropertyDescriptor : StoreEnabledPropertyDescriptor
	{
		/// <summary>
		/// Create a new <see cref="AutomatedElementFilterCustomPropertyDescriptor"/>
		/// </summary>
		/// <param name="modifyDescriptor">A standard property descriptor to wrap.</param>
		/// <param name="displayName">A customized display name. If this is <see langword="null"/>, then the original display name is used.</param>
		/// <param name="description">A customized description. If this is <see langword="null"/>, then the original description is used.</param>
		/// <param name="category">A customized category. If this is <see langword="null"/>, then the original category is used.</param>
		public AutomatedElementFilterCustomPropertyDescriptor(PropertyDescriptor modifyDescriptor, string displayName, string description, string category)
			: base(modifyDescriptor, displayName, description, category)
		{
		}
		/// <summary>
		/// Set a value with a filter in place
		/// </summary>
		public override void SetValue(object component, object value)
		{
			IORMToolServices toolServices = null;
			ModelElement element;
			AutomatedElementFilterCallback callback = null;
			if (null != (element = component as ModelElement) &&
				null != (toolServices = element.Store as IORMToolServices))
			{
				callback = FilterAutomatedElement;
				toolServices.AutomatedElementFilter += callback;
			}
			try
			{
				base.SetValue(component, value);
			}
			finally
			{
				if (toolServices != null)
				{
					toolServices.AutomatedElementFilter -= callback;
				}
			}
		}
		/// <summary>
		/// Return <see cref="AutomatedElementDirective.Ignore"/> to classify the provided <see cref="ModelElement"/>
		/// as an automated element.
		/// The default implementation classifies all elements as automated.
		/// </summary>
		protected virtual AutomatedElementDirective FilterAutomatedElement(ModelElement element)
		{
			return AutomatedElementDirective.Ignore;
		}
	}
	#endregion // AutomatedElementFilterPropertyDescriptor class
}
