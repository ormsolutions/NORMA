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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using Neumont.Tools.Modeling.Shell;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region ModelErrorDisplayFilterAttribute
	/// <summary>
	/// An attribute to specify on an type derived from <see cref="ModelError"/> to
	/// model the error display information.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class ModelErrorDisplayFilterAttribute : Attribute
	{
		private Type myCategory;
		/// <summary>
		/// Create a new <see cref="ModelErrorDisplayFilterAttribute"/>
		/// </summary>
		/// <param name="category">The type of the category to
		/// filter by. Must be a type derived from <see cref="ModelErrorCategory"/>.</param>
		/// <exception cref="ArgumentException">Thrown if the specified type is not derived from <see cref="ModelErrorCategory"/></exception>
		public ModelErrorDisplayFilterAttribute(Type category)
		{
			// Don't use IsAssignableFrom here, we do not want to allow ModelErrorCategory itself
			if (category == null || !category.IsSubclassOf(typeof(ModelErrorCategory)))
			{
				throw new ArgumentException(ResourceStrings.ModelExceptionObjectificationImpliedElementModified, "category");
			}
			myCategory = category;
		}
		/// <summary>
		/// Get the category type
		/// </summary>
		public Type Category
		{
			get
			{
				return myCategory;
			}
		}

	}
	#endregion //ModelErrorDisplayFilterAttribute
	#region ModelErrorDisplayFilter class
	public partial class ModelErrorDisplayFilter : IXmlSerializable
	{
		private const string XMLCategoriesElement = "Categories";
		private const string XMLIncludedErrorsElement = "IncludedErrors";
		private const string XMLExcludedErrorsElement = "ExcludedErrors";
		private const string XMLElement = "ModelErrorDisplayFilter";
		private const string XMLPrefix = "orm";
		private const string XMLModelReferenceAttribute = "ref";
		private const char ListDelimiter = ' ';

		private bool myIncludedErrorsChanged = false;
		private bool myExcludedErrorsChanged = false;
		private bool myExcludedCategoriesChanged = false;
		private string myIncludedErrorsList = string.Empty;
		private string myExcludedErrorsList = string.Empty;
		private string myExcludedCategoriesList = string.Empty;
		private Dictionary<Type, Type> myIncludedErrors;
		private Dictionary<Type, Type> myExcludedErrors;
		private Dictionary<Type, Type> myExcludedCategories;

		private Dictionary<Type, Type> ExcludedCategoriesDictionary
		{
			get
			{
				if (!myExcludedCategoriesChanged)
				{
					myExcludedCategories = ParseList(myExcludedCategoriesList, myExcludedCategories);
			}
				return myExcludedCategories;
		}
		}
		private Dictionary<Type, Type> IncludedErrorsDictionary
		{
			get
			{
				if (!myIncludedErrorsChanged)
				{
					myIncludedErrors = ParseList(myIncludedErrorsList, myIncludedErrors);
			}
				return myIncludedErrors;
		}
		}
		private Dictionary<Type, Type> ExcludedErrorsDictionary
		{
			get
			{
				if (!myExcludedErrorsChanged)
				{
					myExcludedErrors = ParseList(myExcludedErrorsList, myExcludedErrors);
			}
				return myExcludedErrors;
		}
		}
		private Dictionary<Type, Type> ParseList(string list, Dictionary<Type, Type> cache)
		{
			if (list.Length != 0)
			{
				Dictionary<Type, Type> retVal = cache;
				if (retVal == null)
				{
					retVal = new Dictionary<Type, Type>();
				}
				if (retVal.Count == 0)
				{
					//synchronize the cache with the string
					DomainDataDirectory dataDir = Store.DomainDataDirectory;
					string[] typeNames = list.Split(new char[] { ListDelimiter }, StringSplitOptions.RemoveEmptyEntries);
					int typeCount = typeNames.Length;
					for (int i = 0; i < typeCount; ++i)
					{
						Type type = dataDir.GetDomainClass(typeNames[i]).ImplementationClass;
						retVal.Add(type, null);
					}
				}
				return retVal;
			}
			return cache;
		}
		private string GetExcludedCategoriesValue()
		{
			return myExcludedCategoriesList;
		}
		private void SetExcludedCategoriesValue(string newValue)
			{
			myExcludedCategoriesList = newValue;
			Dictionary<Type, Type> cache = ExcludedCategoriesDictionary;
			if (cache != null)
				{
				cache.Clear();
				}
			}
		private string GetExcludedErrorsValue()
		{
			return myExcludedErrorsList;
		}
		private void SetExcludedErrorsValue(string newValue)
		{
			myExcludedErrorsList = newValue;
			Dictionary<Type, Type> cache = ExcludedErrorsDictionary;
			if (cache != null)
			{
				cache.Clear();
			}
		}
		private string GetIncludedErrorsValue()
		{
			return myIncludedErrorsList;
		}
		private void SetIncludedErrorsValue(string newValue)
		{
			myIncludedErrorsList = newValue;
			Dictionary<Type, Type> cache = IncludedErrorsDictionary;
			if (cache != null)
			{
				cache.Clear();
			}
		}

		/// <summary>
		/// Determines if an error should be displayed or not.
		/// </summary>
		/// <param name="error">The model error to check.</param>
		/// <returns>True if the error should be displayed.</returns>
		public bool ShouldDisplay(ModelError error)
		{
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}

			return !IsErrorExcluded(error.GetType());
		}

		/// <summary>
		/// Toggles an error category to include or exclude.
		/// </summary>
		/// <param name="category">The error category.</param>
		/// <param name="exclude">True to exclude the error category.</param>
		public void ToggleCategory(Type category, bool exclude)
		{
			Dictionary<Type, Type> dictionary = ExcludedCategoriesDictionary;
			if (dictionary == null)
			{
				dictionary = myExcludedCategories = new Dictionary<Type, Type>();
			}

			if (exclude)
			{
				if (!(dictionary.ContainsKey(category)))
			{
					myExcludedCategoriesChanged = true;
				dictionary.Add(category, null);
			}
		}
			else
		{
				if ((dictionary.ContainsKey(category)))
			{
					myExcludedCategoriesChanged = true;
					dictionary.Remove(category);
					}
				}
			}
		/// <summary>
		/// Toggles an error type to include or exclude.
		/// </summary>
		/// <param name="error">The error type.</param>
		/// <param name="exclude">True to exclude the error type.</param>
		public void ToggleError(Type error, bool exclude)
		{
			Type category = GetCategory(error);
			Dictionary<Type, Type> dictionary;

			if (IsCategoryExcluded(GetCategory(error)))
			{
				dictionary = IncludedErrorsDictionary;
				if (dictionary == null)
				{
					dictionary = myIncludedErrors = new Dictionary<Type, Type>();
				}
				myIncludedErrorsChanged = true;

				//flip whether to include or exclude the error in the sub-list
				exclude = !exclude;
				}
			else
			{
				dictionary = ExcludedErrorsDictionary;
				if (dictionary == null)
				{
					dictionary = myExcludedErrors = new Dictionary<Type, Type>();
				}
				myExcludedErrorsChanged = true;
			}

			if (exclude)
				{
				if (!(dictionary.ContainsKey(error)))
				{
					dictionary.Add(error, null);
				}
			}
			else
			{
				if ((dictionary.ContainsKey(error)))
				{
					dictionary.Remove(error);
				}
			}
		}
		/// <summary>
		/// Determines if an error category is excluded.
		/// </summary>
		/// <param name="type">The error category.</param>
		/// <returns></returns>
		public bool IsCategoryExcluded(Type type)
		{
			Dictionary<Type, Type> excludedCategories = ExcludedCategoriesDictionary;
			return type != null && excludedCategories != null && excludedCategories.ContainsKey(type);
		}
		/// <summary>
		/// Determines if an error type is excluded.
		/// </summary>
		/// <param name="error">The error type.</param>
		/// <returns></returns>
		public bool IsErrorExcluded(Type error)
		{
			if (IsCategoryExcluded(GetCategory(error)))
			{
				Dictionary<Type, Type> dictionary = IncludedErrorsDictionary;
				return !(dictionary != null && dictionary.ContainsKey(error));
			}
				else
				{
				Dictionary<Type, Type> dictionary = ExcludedErrorsDictionary;
				return (dictionary != null && dictionary.ContainsKey(error));
				}
			}
		/// <summary>
		/// Commit any changes pending from calls to ToggleCategory or ToggleError.
		/// </summary>
		public void CommitChanges()
		{
			if (myExcludedCategoriesChanged)
			{
				this.ExcludedCategories = GetValue(myExcludedCategoriesList, ref myExcludedCategories);
			}
			if (myExcludedErrorsChanged)
			{
				this.ExcludedErrors = GetValue(myExcludedErrorsList, ref myExcludedErrors);
			}
			if (myIncludedErrorsChanged)
			{
				this.IncludedErrors = GetValue(myIncludedErrorsList, ref myIncludedErrors);
			}

			myExcludedCategoriesChanged = false;
			myExcludedErrorsChanged = false;
			myIncludedErrorsChanged = false;
		}
		private string GetValue(string myList, ref Dictionary<Type, Type> myCache)
		{
			string retVal = myList;
			Dictionary<Type, Type> cache;
			if ((cache = myCache) == null)
			{
				cache = myCache = new Dictionary<Type, Type>();
			}
			//write the cache to a string
			retVal = string.Empty;
			foreach (Type type in cache.Keys)
			{
				retVal += type.FullName + ListDelimiter;
			}
			return retVal;
		}
		private Type GetCategory(Type error)
		{
			Type category = null;

			object[] atributes = error.GetCustomAttributes(typeof(ModelErrorDisplayFilterAttribute), true);
			foreach (object o in atributes)
			{
				ModelErrorDisplayFilterAttribute attribute = o as ModelErrorDisplayFilterAttribute;
				if (attribute != null)
				{
					category = attribute.Category;
					break;
				}
			}

			return category;
		}

		/// <summary>
		/// returns string.Empty
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ResourceStrings.ModelErrorDisplayFilteredText;
		}

		#region IXmlSerializable Implementation
		XmlSchema IXmlSerializable.GetSchema()
		{
			// Schema is already validated in ORMCore.xsd
			return null;
		}
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string XmlNamespace = ORMCoreDomainModel.XmlNamespace;
			ISerializationContext serializationContext = ((ISerializationContextHost)Store).SerializationContext;

			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					if (reader.LocalName == XMLElement && reader.NamespaceURI == XmlNamespace)
					{
						Model = (ORMModel)serializationContext.RealizeElement(reader.GetAttribute(XMLModelReferenceAttribute), ORMModel.DomainClassId, true);
						if (!reader.IsEmptyElement)
						{
							Dictionary<string, string> namespaces = null;

							while (reader.Read())
							{
								if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
								{
									switch (reader.LocalName)
									{
										case XMLCategoriesElement:
											myExcludedCategoriesList = ReadInnerXMLList(reader, ref namespaces);
											break;
										case XMLIncludedErrorsElement:
											myIncludedErrorsList = ReadInnerXMLList(reader, ref namespaces);
											break;
										case XMLExcludedErrorsElement:
											myExcludedErrorsList = ReadInnerXMLList(reader, ref namespaces);
											break;
									}
								}
							}
						}
					}
				}
				else if (nodeType == XmlNodeType.EndElement)
				{
					break;
				}
			}
		}
		/// <summary>
		/// Reads a list of types and returns them in a string.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="namespaces"></param>
		/// <returns></returns>
		private string ReadInnerXMLList(XmlReader reader, ref Dictionary<string, string> namespaces)
		{
			string retVal = string.Empty;

			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element && reader.IsEmptyElement)
				{
					if (namespaces == null)
					{
						//synchronize namespaces
						foreach (ICustomSerializedDomainModel serializationInfo in Utility.EnumerateDomainModels<ICustomSerializedDomainModel>(Store.DomainModels))
						{
							if (namespaces == null)
							{
								namespaces = new Dictionary<string, string>();
							}
							namespaces.Add(serializationInfo.GetCustomElementNamespaces()[0, 1], serializationInfo.GetType().Namespace);
						}
					}
					if (namespaces != null)
					{
						string readerNamespaceURI = reader.NamespaceURI;
						if (namespaces.ContainsKey(readerNamespaceURI))
						{
							retVal += namespaces[readerNamespaceURI] + "." + reader.LocalName + ListDelimiter;
						}
					}
				}
				else if (nodeType == XmlNodeType.EndElement)
				{
					break;
				}
			}

			return retVal;
		}
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			string XmlNamespace = ORMCoreDomainModel.XmlNamespace;

			writer.WriteStartElement(XMLPrefix, XMLElement, XmlNamespace);
			writer.WriteAttributeString("id", "_" + this.Id);
			writer.WriteAttributeString(XMLModelReferenceAttribute, "_" + Model.Id.ToString("D"));

			writer.WriteStartElement(XMLPrefix, XMLCategoriesElement, XmlNamespace);
			string[] types = myExcludedCategoriesList.Split(new char[] { ListDelimiter }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < types.Length; ++i)
			{
				string type = types[i];
				writer.WriteElementString(XMLPrefix, type.Substring(type.LastIndexOf('.') + 1), XmlNamespace, string.Empty);
				}
			writer.WriteEndElement();

			writer.WriteStartElement(XMLPrefix, XMLIncludedErrorsElement, XmlNamespace);
			types = myIncludedErrorsList.Split(new char[] { ListDelimiter }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < types.Length; ++i)
			{
				string type = types[i];
				writer.WriteElementString(XMLPrefix, type.Substring(type.LastIndexOf('.') + 1), XmlNamespace, string.Empty);
				}
			writer.WriteEndElement();

			writer.WriteStartElement(XMLPrefix, XMLExcludedErrorsElement, XmlNamespace);
			types = myExcludedErrorsList.Split(new char[] { ListDelimiter }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < types.Length; ++i)
			{
				string type = types[i];
				writer.WriteElementString(XMLPrefix, type.Substring(type.LastIndexOf('.') + 1), XmlNamespace, string.Empty);
				}
			writer.WriteEndElement();

			writer.WriteEndElement();
		}
		#endregion // IXmlSerializable Implementation
	}
	#endregion //ModelErrorDisplayFilter class
}
