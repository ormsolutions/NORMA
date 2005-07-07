#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.ShapeModel;
using Northface.Tools.ORM;
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
