using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region ORMModelElement
	public abstract partial class ORMModelElement : IORMExtendableElement
	{
		/// <summary>See <see cref="IORMExtendableElement.GetDisplayProperties"/></summary>
		public override PropertyDescriptorCollection GetDisplayProperties(ModelElement requestor, ref PropertyDescriptor defaultPropertyDescriptor)
		{
			return ExtendableElementUtility.MergeExtensionProperties(this, base.GetDisplayProperties(requestor, ref defaultPropertyDescriptor));
		}
	}
	#endregion // ORMModelElement

	#region ORMNamedElement
	public abstract partial class ORMNamedElement : IORMExtendableElement
	{
		/// <summary>See <see cref="IORMExtendableElement.GetDisplayProperties"/></summary>
		public override PropertyDescriptorCollection GetDisplayProperties(ModelElement requestor, ref PropertyDescriptor defaultPropertyDescriptor)
		{
			return ExtendableElementUtility.MergeExtensionProperties(this, base.GetDisplayProperties(requestor, ref defaultPropertyDescriptor));
		}
	}
	#endregion // ORMNamedElement
}
