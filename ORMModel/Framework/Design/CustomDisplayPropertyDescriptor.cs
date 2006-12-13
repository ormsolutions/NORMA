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
using System.ComponentModel;

namespace Neumont.Tools.Modeling.Design
{
	/// <summary>
	/// Customize the display settings for any <see cref="PropertyDescriptor"/>
	/// </summary>
	public sealed class CustomDisplayPropertyDescriptor : PropertyDescriptor
	{
		private PropertyDescriptor myInnerDescriptor;
		private string myDisplayName;
		private string myDisplayCategory;
		private string myDisplayDescription;
		/// <summary>
		/// Wrap an existing <see cref="PropertyDescriptor"/> to modify its display settings
		/// </summary>
		/// <param name="innerDescriptor"><see cref="PropertyDescriptor"/> to modify</param>
		/// <param name="displayName">An alternate value for the <see cref="M:System.ComponentModel.PropertyDescriptor.DisplayName"/> property. Ignored if not set.</param>
		/// <param name="displayCategory">An alternate value for the <see cref="M:System.ComponentModel.PropertyDescriptor.Category"/> property. Ignored if not set.</param>
		/// <param name="displayDescription">An alternate value for the <see cref="M:System.ComponentModel.PropertyDescriptor.Description"/> property. Ignored if not set.</param>
		public CustomDisplayPropertyDescriptor(PropertyDescriptor innerDescriptor, string displayName, string displayCategory, string displayDescription)
			: base(innerDescriptor)
		{
			if (!string.IsNullOrEmpty(displayName))
			{
				myDisplayName = displayName;
			}
			if (!string.IsNullOrEmpty(displayCategory))
			{
				myDisplayCategory = displayCategory;
			}
			if (!string.IsNullOrEmpty(displayDescription))
			{
				myDisplayDescription = displayDescription;
			}
			myInnerDescriptor = innerDescriptor;
		}
		/// <summary>
		/// Customized DisplayName. Defers to wrapped descriptor if no value
		/// specified in the constructor.
		/// </summary>
		public override string DisplayName
		{
			get
			{
				string retVal = myDisplayName;
				return (retVal != null) ? retVal : myInnerDescriptor.DisplayName;
			}
		}
		/// <summary>
		/// Customized Category. Defers to wrapped descriptor if no value
		/// specified in the constructor.
		/// </summary>
		public override string Category
		{
			get
			{
				string retVal = myDisplayCategory;
				return (retVal != null) ? retVal : myInnerDescriptor.Category;
			}
		}
		/// <summary>
		/// Customized Description. Defers to wrapped descriptor if no value
		/// specified in the constructor.
		/// </summary>
		public override string Description
		{
			get
			{
				string retVal = myDisplayDescription;
				return (retVal != null) ? retVal : myInnerDescriptor.Description;
			}
		}
		/// <summary>
		/// Defers to <see cref="M:System.ComponentModel.PropertyDescriptor.CanResetValue"/> on wrapped descriptor.
		/// </summary>
		public override bool CanResetValue(object component)
		{
			return myInnerDescriptor.CanResetValue(component);
		}
		/// <summary>
		/// Defers to <see cref="M:System.ComponentModel.PropertyDescriptor.ComponentType"/> on wrapped descriptor.
		/// </summary>
		public override Type ComponentType
		{
			get
			{
				return myInnerDescriptor.ComponentType;
			}
		}
		/// <summary>
		/// Defers to <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue"/> on wrapped descriptor.
		/// </summary>
		public override object GetValue(object component)
		{
			return myInnerDescriptor.GetValue(component);
		}
		/// <summary>
		/// Defers to <see cref="M:System.ComponentModel.PropertyDescriptor.IsReadOnly"/> on wrapped descriptor.
		/// </summary>
		public override bool IsReadOnly
		{
			get
			{
				return myInnerDescriptor.IsReadOnly;
			}
		}
		/// <summary>
		/// Defers to <see cref="M:System.ComponentModel.PropertyDescriptor.PropertyType"/> on wrapped descriptor.
		/// </summary>
		public override Type PropertyType
		{
			get
			{
				return myInnerDescriptor.PropertyType;
			}
		}
		/// <summary>
		/// Defers to <see cref="M:System.ComponentModel.PropertyDescriptor.ResetValue"/> on wrapped descriptor.
		/// </summary>
		public override void ResetValue(object component)
		{
			myInnerDescriptor.ResetValue(component);
		}
		/// <summary>
		/// Defers to <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue"/> on wrapped descriptor.
		/// </summary>
		public override void SetValue(object component, object value)
		{
			myInnerDescriptor.SetValue(component, value);
		}
		/// <summary>
		/// Defers to <see cref="M:System.ComponentModel.PropertyDescriptor.ShouldSerializeValue"/> on wrapped descriptor.
		/// </summary>
		public override bool ShouldSerializeValue(object component)
		{
			return myInnerDescriptor.ShouldSerializeValue(component);
		}
	}
}
