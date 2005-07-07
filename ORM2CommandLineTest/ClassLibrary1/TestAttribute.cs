#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace TestFramework
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false)]
	public class TestAttribute : Attribute
	{
		private Collection<string> myCategories;

		public TestAttribute()
		{
		}

		public string Category
		{
			get
			{
				return null;
			}
			set
			{
				if (myCategories == null)
				{
					myCategories = new Collection<string>();
				}
				value = value.ToLowerInvariant();
				if (!myCategories.Contains(value))
				{
					myCategories.Add(value);
				}
			}
		}
		public bool SupportsCategory(string category)
		{
			if (myCategories != null)
			{
				category = category.ToLowerInvariant();
				return myCategories.Contains(category);
			}
			return false;
		}
	}
}
