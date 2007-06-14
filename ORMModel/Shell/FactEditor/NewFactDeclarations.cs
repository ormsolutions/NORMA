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

#region Using Directives
using System;
using Microsoft.VisualStudio.Package;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling.Shell;
using Neumont.Tools.ORM.ObjectModel;
#endregion

namespace Neumont.Tools.ORM.Shell.FactEditor
{
	/// <summary>
	/// Manages a list of ObjectType declarations to be shown in an
	/// intellisense drop-down list.
	/// </summary>
	[CLSCompliant(false)]
	public sealed class NewFactDeclarations : Declarations
	{
		#region Private Members
		private NewFactLanguageService myFactLanguageService;
		private List<Declaration> m_Declarations;
		private ORMDesignerDocView m_View;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="NewFactDeclarations"/> class.
		/// </summary>
		/// <param name="languageService">The language service.</param>
		public NewFactDeclarations(NewFactLanguageService languageService)
		{
			this.myFactLanguageService = languageService;
			m_Declarations = new List<Declaration>();

			//for (int i = 0; i < 5; i++)
			//{
			//    Declaration d = new Declaration(
			//        "Name" + i.ToString(),
			//        "Name" + i.ToString(),
			//        "Description for " + i.ToString()
			//    );
			//    myDeclarations.Add(d);
			//}

			IMonitorSelectionService monitor = languageService.GetService(typeof(IMonitorSelectionService)) as IMonitorSelectionService;
			//monitor.DocumentWindowChanged += new EventHandler<MonitorSelectionEventArgs>(DocumentWindowChangedEvent);
			//monitor.SelectionChanged += new EventHandler<MonitorSelectionEventArgs>(SelectionChangedEvent);
			m_View = monitor.CurrentDocumentView as ORMDesignerDocView;

			ReloadModelElements();
		}
		#endregion

		#region Private Methods
		private void ReloadModelElements()
		{
			List<Declaration> objectEntries = m_Declarations;
			if (objectEntries.Count > 0)
			{
				objectEntries.Clear();
			}
			ORMDesignerDocView currentDocView = m_View;
			if (currentDocView != null)
			{
				List<ObjectType> temp = new List<ObjectType>();
				temp.AddRange(currentDocView.DocData.Store.ElementDirectory.FindElements<ObjectType>());
				temp.Sort(Modeling.NamedElementComparer<ObjectType>.CurrentCulture);

				foreach (ObjectType o in temp)
				{
					objectEntries.Add(new Declaration(o));
				}
			}
		}
		#endregion

		#region Overriden Abstract Methods
		/// <summary>
		/// When implemented in a derived class, gets the number of items in the list of declarations.
		/// </summary>
		/// <returns>
		/// The count of items represented by this <see cref="T:Microsoft.VisualStudio.Package.Declarations"></see> class.
		/// </returns>
		public override Int32 GetCount()
		{
			return m_Declarations.Count;
		}

		/// <summary>
		/// When implemented in a derived class, gets the name or text to be inserted for the specified item.
		/// </summary>
		/// <param name="index">[in] The index of the item for which to get the name.</param>
		/// <returns>
		/// If successful, returns the name of the item; otherwise, returns null.
		/// </returns>
		public override string GetName(Int32 index)
		{
			if (index < 0 || index > (m_Declarations.Count - 1))
				return null;

			return m_Declarations[index].Name;
		}

		/// <summary>
		/// When implemented in a derived class, gets a description of the specified item.
		/// </summary>
		/// <param name="index">[in] The index of the item for which to get the description.</param>
		/// <returns>
		/// If successful, returns the description; otherwise, returns null.
		/// </returns>
		public override String GetDescription(Int32 index)
		{
			if (index < 0 || index > (m_Declarations.Count - 1))
				return null;

			return m_Declarations[index].Description;
		}

		/// <summary>
		/// When implemented in a derived class, gets the text to be displayed in the completion list for the specified item.
		/// </summary>
		/// <param name="index">[in] The index of the item for which to get the display text.</param>
		/// <returns>
		/// The text to be displayed, otherwise null.
		/// </returns>
		public override String GetDisplayText(Int32 index)
		{
			if (index < 0 || index > (m_Declarations.Count - 1))
				return null;

			return m_Declarations[index].DisplayText;
		}

		/// <summary>
		/// When implemented in a derived class, gets the image to show next to the specified item.
		/// </summary>
		/// <param name="index">[in] The index of the item for which to get the image index.</param>
		/// <returns>
		/// The index of the image from an image list, otherwise -1.
		/// </returns>
		public override Int32 GetGlyph(Int32 index)
		{
			if (index < 0 || index > (m_Declarations.Count - 1))
				return (Int32)FactEditorIconImageIndex.EntityType;

			return (Int32)m_Declarations[index].IconImageIndex;
		}
		#endregion
	}

	/// <summary>
	/// Represents a declaration in the intellisense list.
	/// </summary>
	internal sealed class Declaration
	{
		#region Private Members
		private ObjectType m_ObjectType;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Declaration"/> class.
		/// </summary>
		/// <param name="objectType">The object type.</param>
		public Declaration(ObjectType objectType)
		{
			m_ObjectType = objectType;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the name displayed in the intellisense list.
		/// </summary>
		public String Name
		{
			get
			{
				return m_ObjectType.Name;
			}
		}

		/// <summary>
		/// Gets the description for the item selected in the intellisense list.
		/// </summary>
		public String Description
		{
			get
			{
				return m_ObjectType.Name;
			}
		}

		/// <summary>
		/// Gets the text that is inserted when the Name is chosen in the intellisense list.
		/// </summary>
		public String DisplayText
		{
			get
			{
				return m_ObjectType.Name;
			}
		}

		/// <summary>
		/// Gets the FactEditorIconImageIndex that represents this declaration.
		/// </summary>
		public FactEditorIconImageIndex IconImageIndex
		{
			get
			{
				if (IsNestedFactType)
				{
					return FactEditorIconImageIndex.ObjectifiedType;
				}
				else if (IsValueType)
				{
					return FactEditorIconImageIndex.ValueType;
				}
				else
				{
					return FactEditorIconImageIndex.EntityType;
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is nested fact type.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is nested fact type; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsNestedFactType
		{
			get
			{
				return (m_ObjectType.NestedFactType != null);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is value type.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is value type; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsValueType
		{
			get
			{
				return m_ObjectType.IsValueType;
			}
		}
		#endregion
	}

	/// <summary>
	/// Enumeration of the image to use in the intellisense list
	/// for the objecttype's type.
	/// </summary>
	public enum FactEditorIconImageIndex
	{
		/// <summary>
		/// Index of the entity type image.
		/// </summary>
		EntityType = 0,

		/// <summary>
		/// Index of the value type image.
		/// </summary>
		ValueType = 1,

		/// <summary>
		/// Index of the objectified type image.
		/// </summary>
		ObjectifiedType = 2
	}

	//public enum IconImageIndex
	//{
	//    // Each icon type has 6 versions, corresponding to the following
	//    // access types.
	//    AccessPublic = 0,
	//    AccessInternal = 1,
	//    AccessFriend = 2,
	//    AccessProtected = 3,
	//    AccessPrivate = 4,
	//    AccessShortcut = 5,

	//    Base = 6,

	//    Class = Base * 0,
	//    Constant = Base * 1,
	//    Delegate = Base * 2,
	//    Enumeration = Base * 3,
	//    EnumMember = Base * 4,
	//    Event = Base * 5,
	//    Exception = Base * 6,
	//    Field = Base * 7,
	//    Interface = Base * 8,
	//    Macro = Base * 9,
	//    Map = Base * 10,
	//    MapItem = Base * 11,
	//    Method = Base * 12,
	//    OverloadedMethod = Base * 13,
	//    Module = Base * 14,
	//    Namespace = Base * 15,
	//    Operator = Base * 16,
	//    Property = Base * 17,
	//    Struct = Base * 18,
	//    Template = Base * 19,
	//    Typedef = Base * 20,
	//    Type = Base * 21,
	//    Union = Base * 22,
	//    Variable = Base * 23,
	//    ValueType = Base * 24,
	//    Intrinsic = Base * 25,
	//    JavaMethod = Base * 26,
	//    JavaField = Base * 27,
	//    JavaClass = Base * 28,
	//    JavaNamespace = Base * 29,
	//    JavaInterface = Base * 30,
	//    // Miscellaneous icons with one icon for each type.
	//    Error = 187,
	//    GreyedClass = 188,
	//    GreyedPrivateMethod = 189,
	//    GreyedProtectedMethod = 190,
	//    GreyedPublicMethod = 191,
	//    BrowseResourceFile = 192,
	//    Reference = 193,
	//    Library = 194,
	//    VBProject = 195,
	//    VBWebProject = 196,
	//    CSProject = 197,
	//    CSWebProject = 198,
	//    VB6Project = 199,
	//    CPlusProject = 200,
	//    Form = 201,
	//    OpenFolder = 202,
	//    ClosedFolder = 203,
	//    Arrow = 204,
	//    CSClass = 205,
	//    Snippet = 206,
	//    Keyword = 207,
	//    Info = 208,
	//    CallBrowserCall = 209,
	//    CallBrowserCallRecursive = 210,
	//    XMLEditor = 211,
	//    VJProject = 212,
	//    VJClass = 213,
	//    ForwardedType = 214,
	//    CallsTo = 215,
	//    CallsFrom = 216,
	//    Warning = 217,
	//}
}
