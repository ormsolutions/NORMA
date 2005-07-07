#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace TestFramework
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public class TestsAttribute : Attribute
	{
		public TestsAttribute() { }
	}
}
