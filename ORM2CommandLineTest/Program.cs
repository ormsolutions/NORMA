#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Xml;
using Neumont.Tools.ORM;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using System.Reflection;
using Microsoft.XmlDiffPatch;
using System.Text.RegularExpressions;
#endregion

namespace ORM2CommandLineTest
{
	class Program
	{
		static string originalFile;
		static string savedFile = @"C:\temp\testSave.orm";

		static void Main(string[] args)
		{
			LoadSaveTest(args[0]);
			Console.ReadLine();
		}

		private static void LoadSaveTest(string fileName)
		{
			originalFile = fileName;
			ORMStore store = null;
			IORMToolServices services = new ORMDocServices();
			Assembly assembly = MethodInfo.GetCurrentMethod().DeclaringType.Assembly;
			//Get all the types in the project
			Type[] types = assembly.GetTypes();
			int typeCount = types.Length;

			//Loop through all of the types and find the ones with the TestsAttribute
			for (int i = 0; i < typeCount; ++i)
			{
				Type type = types[i];
				object[] attributes = type.GetCustomAttributes(typeof(TestsAttribute), true);
				int attributeCount = attributes.Length;

				//For all of the types with the TestsAttribute, find the members with the TestAttribute
				for (int j = 0; j < attributeCount; ++j)
				{
					foreach (MemberInfo member in type.GetMembers())
					{
						object[] memberAttributes = member.GetCustomAttributes(typeof(TestAttribute), true);
						int memberAttributeCount = memberAttributes.Length;
						for (int k = 0; k < memberAttributeCount; ++k)
						{
							TestAttribute test = memberAttributes[k] as TestAttribute;
							bool loadOK = test.SupportsCategory("LoadORMFile");
							bool saveOK = test.SupportsCategory("SaveORMFile");
							ConstructorInfo constructor = null;
							MyTests tests = null;
							if (loadOK)
							{
								constructor = type.GetConstructor(new Type[] { typeof(IORMToolServices) });
								tests = constructor.Invoke(new object[] { services }) as MyTests;
								if (tests != null)
								{
									store = ((ORMDocServices)tests.Services).LoadFile(originalFile);
								}
							}
							if (saveOK)
							{
								constructor = type.GetConstructor(new Type[] { typeof(IORMToolServices) });
								tests = constructor.Invoke(new object[] { services }) as MyTests;
								if (tests != null)
								{
									((ORMDocServices)tests.Services).SaveFile(store, savedFile);
								}
							}
						}
					}
				}
			}

			XmlDiff diff = new XmlDiff(	XmlDiffOptions.IgnoreComments |
										XmlDiffOptions.IgnoreWhitespace |
										XmlDiffOptions.IgnoreXmlDecl);

			const string xmlDiffGramFile = @"C:\temp\xmldiffgram.txt";
			XmlWriter writer = XmlWriter.Create(xmlDiffGramFile);
			if (!diff.Compare(originalFile, savedFile, true, writer))
			{
				Console.WriteLine("Files are Different");
			}
			else
			{
				Console.WriteLine("Files are the same");
			}
			writer.Close();

			XmlDocument doc = new XmlDocument();
			FileStream stream = File.Open(xmlDiffGramFile, FileMode.Open);
			doc.Load(stream);

			List<string> nodeNames = new List<string>(3);
			nodeNames.Add("xd:add");
			nodeNames.Add("xd:remove");
			nodeNames.Add("xd:change");
			XmlNodeList nodeList = null;

			for (int i = 0; i < nodeNames.Count; i++)
			{
				nodeList = doc.GetElementsByTagName(nodeNames[i]);
				//TODO: Go through all the differenct node types
				// and parse the relevant information
				foreach (XmlNode node in nodeList)
				{
					Console.WriteLine(node.InnerText);
				}
			}

			stream.Close();
			if (File.Exists(xmlDiffGramFile))
			{
				File.Delete(xmlDiffGramFile);
			}

			//Write out model errors
			Console.WriteLine("\nModel Errors:");
			foreach (IORMToolTaskItem item in ((ORMTaskProvider)services.TaskProvider).TaskItems)
			{
				Console.WriteLine(item.Text);
			}

		}
	}
}
