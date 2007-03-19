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
using Neumont.Tools.ORM.Shell;

namespace Neumont.Tools.ORM.ObjectModel
{
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
	public partial class ModelErrorDisplayFilter : IXmlSerializable
	{
		const string XMLCategoriesElement = "Categories";
		const string XMLIncludedErrorsElement = "IncludedErrors";
		const string XMLExcludedErrorsElement = "ExcludedErrors";
		const string XMLElement = "ModelErrorDisplayFilter";
		const string XMLPrefix = "orm";
		const string XMLModelReferenceAttribute = "ref";
		const char listDelimiter = ' ';

		private string myIncludedErrorsList = "";
		private string myExcludedErrorsList = "";
		private string myExcludedCategoriesList = "";
		private Dictionary<Type, Type> myIncludedErrors;
		private Dictionary<Type, Type> myExcludedErrors;
		private Dictionary<Type, Type> myExcludedCategories;
		private Dictionary<Type, Type> ExcludedCategoriesDictionary
		{
			get
			{
				return myExcludedCategories = parseList(myExcludedCategoriesList, myExcludedCategories);
			}
		}
		private Dictionary<Type, Type> IncludedErrorsDictionary
		{
			get
			{
				return myIncludedErrors = parseList(myIncludedErrorsList, myIncludedErrors);
			}
		}
		private Dictionary<Type, Type> ExcludedErrorsDictionary
		{
			get
			{
				return myExcludedErrors = parseList(myExcludedErrorsList, myExcludedErrors);
			}
		}
		private Dictionary<Type, Type> parseList(string list, Dictionary<Type, Type> cache)
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
					DomainDataDirectory dataDir = Store.DomainDataDirectory;
					string[] typeNames = list.Split(new char[] { listDelimiter }, StringSplitOptions.RemoveEmptyEntries);
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
		/// <summary>
		/// Get the string for the current categories value
		/// </summary>
		private string GetExcludedCategoriesValue()
		{
			string retVal = myExcludedCategoriesList;
			Dictionary<Type, Type> cache = myExcludedCategories;
			if (cache != null && cache.Count > 0)
			{
				retVal = "";
				foreach (Type category in cache.Keys)
				{
					retVal += category.FullName + listDelimiter;
				}
			}
			return myExcludedCategoriesList = retVal;
		}                                                           
		private void SetExcludedCategoriesValue(string newValue)
		{
			myExcludedCategoriesList = newValue;
			Dictionary<Type, Type> cache = myExcludedCategories;
			if (cache != null)
			{
				cache.Clear();
			}
		}
		/// <summary>
		/// Is the specified category type Excluded
		/// </summary>
		/// <param name="category">The type of a category</param>
		/// <returns><see langword="true"/> if the error category is Excluded.</returns>
		public bool IsCategoryExcluded(Type category)
		{
			Dictionary<Type, Type> dictionary = ExcludedCategoriesDictionary;
			return dictionary != null && dictionary.ContainsKey(category);
		}
		/// <summary>
		/// Toggles an error category to include or exclude.
		/// </summary>
		/// <param name="category">The error category.</param>
		/// <param name="exclude">True to exclude the error category.</param>
		public void ToggleCategory(Type category, bool exclude)
		{
			if (IsCategoryExcluded(category) != exclude)
			{
				ToggleCategory(category);
			}
		}
		/// <summary>
		/// Toggles an error category to switch its included/excluded state.
		/// </summary>
		/// <param name="category">The error category.</param>
		public void ToggleCategory(Type category)
		{
			Dictionary<Type, Type> dictionary = ExcludedCategoriesDictionary;
			if (dictionary == null)
			{
				dictionary = myExcludedCategories = new Dictionary<Type, Type>();
			}

			if (dictionary.ContainsKey(category))
			{
				dictionary.Remove(category);
				myExcludedCategoriesList = "";
			}
			else
			{
				dictionary.Add(category, null);
				myExcludedCategoriesList = "";
			}

			RemoveErrors(ExcludedErrorsDictionary, category);
			RemoveErrors(IncludedErrorsDictionary, category);
			myExcludedErrorsList = "";
			myIncludedErrorsList = "";
		}

		private void RemoveErrors(Dictionary<Type, Type> dictionary, Type category)
		{
			if (dictionary != null)
			{
				IList<Type> keys = new List<Type>(dictionary.Keys);
				foreach (Type error in keys)
				{
					if (GetCategory(error) == category)
					{
						dictionary.Remove(error);
					}
				}
			}
		}
		/// <summary>
		/// Determines if an error should be displayed or not.
		/// </summary>
		/// <param name="error">The model error to check.</param>
		/// <returns>True if the error should be displayed.</returns>
		public bool ShouldDisplay(ModelError error)
		{
			return !IsErrorExcluded(error.GetType());
		}
		/// <summary>
		/// Toggles an error type to include or exclude.
		/// </summary>
		/// <param name="error">The error type.</param>
		/// <param name="exclude">True to exclude the error type.</param>
		public void ToggleError(Type error, bool exclude)
		{
			if (IsErrorExcluded(error) != exclude)
			{
				ToggleError(error);
			}
		}
		/// <summary>
		/// Determines if an error is excluded or not.
		/// </summary>
		/// <param name="error">The error type.</param>
		/// <returns>True if the error is excluded.</returns>
		public bool IsErrorExcluded(Type error)
		{
			return CheckError(error, false);
		}
		/// <summary>
		/// Toggles an error type to switch its included/excluded state.
		/// </summary>
		/// <param name="error">The error type.</param>
		public void ToggleError(Type error)
		{
			CheckError(error, true);
		}

		private bool CheckError(Type error, bool toggle)
		{
			Type category = GetCategory(error);

			Dictionary<Type, Type> dictionary;

			bool categoryExcluded = category != null && IsCategoryExcluded(category);
			if (categoryExcluded)
			{
				dictionary = IncludedErrorsDictionary;
				if (dictionary == null)
				{
					dictionary = myIncludedErrors = new Dictionary<Type, Type>();
				}
				if (toggle)
				{
					myIncludedErrorsList = "";
				}
			}
			else
			{
				dictionary = ExcludedErrorsDictionary;
				if (dictionary == null)
				{
					dictionary = myExcludedErrors = new Dictionary<Type, Type>();
				}
				if (toggle)
				{
					myExcludedErrorsList = "";
				}
			}

			bool containsError = dictionary.ContainsKey(error);
			if (toggle)
			{
				if (containsError)
				{
					dictionary.Remove(error);
				}
				else
				{
					dictionary.Add(error, null);
				}
			}

			return categoryExcluded ^ containsError;
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

		#region IXmlSerializable Implementation
		XmlSchema IXmlSerializable.GetSchema()
		{
			// Schema is already validated in ORMCore.xsd
			return null;
		}
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string XmlNamespace = ORMCoreDomainModel.XmlNamespace;

			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					if (reader.LocalName == XMLElement && reader.NamespaceURI == XmlNamespace)
					{
						Model = (ORMModel)Store.ElementDirectory.FindElement(new Guid(reader.GetAttribute(XMLModelReferenceAttribute).Substring(1)));
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
											myExcludedCategoriesList = readInnerXMLList(reader, ref namespaces);
											break;
										case XMLIncludedErrorsElement:
											myIncludedErrorsList = readInnerXMLList(reader, ref namespaces);
											break;
										case XMLExcludedErrorsElement:
											myExcludedErrorsList = readInnerXMLList(reader, ref namespaces);
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
		string readInnerXMLList(XmlReader reader, ref Dictionary<string, string> namespaces)
		{
			string retVal = "";

			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element && reader.IsEmptyElement)
				{
					if (namespaces == null)
					{
						foreach (DomainModel model in Store.DomainModels)
						{
							IORMCustomSerializedDomainModel serializationInfo = model as IORMCustomSerializedDomainModel;
							if (serializationInfo != null)
							{
								if (namespaces == null)
								{
									namespaces = new Dictionary<string, string>();
								}
								namespaces.Add(serializationInfo.GetCustomElementNamespaces()[0, 1], model.GetType().Namespace);
							}
						}
					}
					if (namespaces != null)
					{
						string readerNamespaceURI = reader.NamespaceURI;
						if (namespaces.ContainsKey(readerNamespaceURI))
						{
							retVal += namespaces[readerNamespaceURI] + "." + reader.LocalName + listDelimiter;
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
			Dictionary<Type, Type> dictionary = ExcludedCategoriesDictionary;
			if (dictionary != null)
			{
				foreach (Type type in dictionary.Keys)
				{
					writer.WriteElementString(XMLPrefix, type.Name, XmlNamespace, string.Empty);
				}
			}
			writer.WriteEndElement();

			writer.WriteStartElement(XMLPrefix, XMLIncludedErrorsElement, XmlNamespace);
			dictionary = IncludedErrorsDictionary;
			if (dictionary != null)
			{
				foreach (Type type in dictionary.Keys)
				{
					writer.WriteElementString(XMLPrefix, type.Name, XmlNamespace, string.Empty);
				}
			}
			writer.WriteEndElement();

			writer.WriteStartElement(XMLPrefix, XMLExcludedErrorsElement, XmlNamespace);
			dictionary = ExcludedErrorsDictionary;
			if (dictionary != null)
			{
				foreach (Type type in dictionary.Keys)
				{
					writer.WriteElementString(XMLPrefix, type.Name, XmlNamespace, string.Empty);
				}
			}
			writer.WriteEndElement();

			writer.WriteEndElement();
		}
		#endregion // IXmlSerializable Implementation
	}
}
