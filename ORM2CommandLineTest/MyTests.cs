#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.ORM;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Reflection;

#endregion

namespace ORM2CommandLineTest
{
	[Tests]
	public class MyTests
	{
		private IORMToolServices myServices;

		public MyTests(IORMToolServices services)
		{
			this.myServices = services;
		}

		public IORMToolServices Services
		{
			get
			{
				return myServices;
			}
		}

		[Test(Category = "LoadORMFile")]
		public ORMStore LoadFile(string filename)
		{
			return ((ORMDocServices)myServices).LoadFile(filename);
		}

		[Test(Category = "SaveORMFile")]
		public void SaveFile(ORMStore store, string filename)
		{
			((ORMDocServices)myServices).SaveFile(store, filename);
		}
	}
}
