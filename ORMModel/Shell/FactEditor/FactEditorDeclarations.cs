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
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;
#endregion

namespace Neumont.Tools.ORM.Shell
{
	partial class FactEditorLanguageService
	{
		#region FactEditorDeclarationType
		private enum FactEditorDeclarationType
		{
			/// <summary>
			/// Get a list of object types in the model
			/// </summary>
			ObjectType,
			/// <summary>
			/// Get a list or reference modes in the model
			/// </summary>
			ReferenceMode,
		}
		#endregion // FactEditorDeclarationType
		/// <summary>
		/// Manages a list of ObjectType declarations to be shown in an
		/// intellisense drop-down list.
		/// </summary>
		private sealed class FactEditorDeclarations : Declarations
		{
			#region ObjectTypeDeclaration class
			/// <summary>
			/// Represents a declaration in the intellisense list.
			/// </summary>
			private struct ObjectTypeDeclaration
			{
				#region Private Members
				private ObjectType myObjectType;
				#endregion // Private Members
				#region Comparer
				public static readonly IComparer<ObjectTypeDeclaration> Comparer = new ObjectTypeDeclarationComparer();
				private class ObjectTypeDeclarationComparer : IComparer<ObjectTypeDeclaration>
				{
					#region Member variables and constructor
					private IComparer<ObjectType> myObjectTypeComparer = NamedElementComparer<ObjectType>.CurrentCulture;
					#endregion // Member variables and constructor
					#region IComparer<ObjectTypeDeclaration> Implementation
					int IComparer<ObjectTypeDeclaration>.Compare(ObjectTypeDeclaration x, ObjectTypeDeclaration y)
					{
						return myObjectTypeComparer.Compare(x.myObjectType, y.myObjectType);
					}
					#endregion // IComparer<ObjectTypeDeclaration> Implementation
				}
				#endregion // Comparer
				#region Constructors
				/// <summary>
				/// Initializes a new instance of the <see cref="ObjectTypeDeclaration"/> class.
				/// </summary>
				/// <param name="objectType">The object type.</param>
				public ObjectTypeDeclaration(ObjectType objectType)
				{
					myObjectType = objectType;
				}
				#endregion // Constructors
				#region Public Properties
				/// <summary>
				/// Gets the name displayed in the intellisense list.
				/// </summary>
				public string Name
				{
					get
					{
						return myObjectType.Name;
					}
				}

				/// <summary>
				/// Gets the description for the item selected in the intellisense list.
				/// </summary>
				public string Description
				{
					get
					{
						return myObjectType.Name;
					}
				}

				/// <summary>
				/// Gets the text that is inserted when the Name is chosen in the intellisense list.
				/// </summary>
				public string DisplayText
				{
					get
					{
						return myObjectType.Name;
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
				public bool IsNestedFactType
				{
					get
					{
						return (myObjectType.NestedFactType != null);
					}
				}

				/// <summary>
				/// Gets a value indicating whether this instance is value type.
				/// </summary>
				/// <value>
				/// 	<c>true</c> if this instance is value type; otherwise, <c>false</c>.
				/// </value>
				public bool IsValueType
				{
					get
					{
						return myObjectType.IsValueType;
					}
				}
				#endregion // Public Properties
			}
			#endregion // ObjectTypeDeclaration class
			#region FactEditorIconImageIndex enum
			/// <summary>
			/// Enumeration of the image to use in the intellisense list
			/// for the objecttype's type.
			/// </summary>
			private enum FactEditorIconImageIndex
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
			#endregion // FactEditorIconImageIndex enum
			#region Private Members
			private FactEditorLanguageService myFactLanguageService;
			private List<ObjectTypeDeclaration> myObjectTypeDeclarations;
			#endregion // Private Members
			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="FactEditorDeclarations"/> class.
			/// </summary>
			/// <param name="languageService">The language service.</param>
			/// <param name="declarationType">The <see cref="FactEditorDeclarationType"/> of the list to support.</param>
			public FactEditorDeclarations(FactEditorLanguageService languageService, FactEditorDeclarationType declarationType)
			{
				myFactLanguageService = languageService;
				myObjectTypeDeclarations = new List<ObjectTypeDeclaration>();
				if (declarationType == FactEditorDeclarationType.ObjectType)
				{
					ReloadModelElements();
				}
			}
			#endregion // Constructors
			#region Private Methods
			private void ReloadModelElements()
			{
				// UNDONE: This is ridiculously inefficient. Use the ActivationManager to
				// register events so we only need to change this list for a second request
				// on the same document instead of repopulating it every time.
				List<ObjectTypeDeclaration> objectEntries = myObjectTypeDeclarations;
				if (objectEntries.Count != 0)
				{
					objectEntries.Clear();
				}
				ActivationManager activationManager;
				ORMDesignerDocData docData;
				Store store;
				if (null != (activationManager = myFactLanguageService.m_ActivationManager) &&
					null != (docData = activationManager.CurrentDocument) &&
					null != (store = docData.Store) &&
					!store.Disposed)
				{
					foreach (ObjectType objectType in store.ElementDirectory.FindElements<ObjectType>())
					{
						objectEntries.Add(new ObjectTypeDeclaration(objectType));
					}
					objectEntries.Sort(ObjectTypeDeclaration.Comparer);
				}
			}
			#endregion // Private Methods
			#region Overridden Abstract Methods
			/// <summary>
			/// When implemented in a derived class, gets the number of items in the list of declarations.
			/// </summary>
			/// <returns>
			/// The count of items represented by this <see cref="T:Microsoft.VisualStudio.Package.Declarations"></see> class.
			/// </returns>
			public override int GetCount()
			{
				return myObjectTypeDeclarations.Count;
			}

			/// <summary>
			/// When implemented in a derived class, gets the name or text to be inserted for the specified item.
			/// </summary>
			/// <param name="index">[in] The index of the item for which to get the name.</param>
			/// <returns>
			/// If successful, returns the name of the item; otherwise, returns null.
			/// </returns>
			public override string GetName(int index)
			{
				if (index < 0 || index > (myObjectTypeDeclarations.Count - 1))
					return null;

				return myObjectTypeDeclarations[index].Name;
			}

			/// <summary>
			/// When implemented in a derived class, gets a description of the specified item.
			/// </summary>
			/// <param name="index">[in] The index of the item for which to get the description.</param>
			/// <returns>
			/// If successful, returns the description; otherwise, returns null.
			/// </returns>
			public override string GetDescription(int index)
			{
				if (index < 0 || index > (myObjectTypeDeclarations.Count - 1))
					return null;

				return myObjectTypeDeclarations[index].Description;
			}

			/// <summary>
			/// When implemented in a derived class, gets the text to be displayed in the completion list for the specified item.
			/// </summary>
			/// <param name="index">[in] The index of the item for which to get the display text.</param>
			/// <returns>
			/// The text to be displayed, otherwise null.
			/// </returns>
			public override string GetDisplayText(int index)
			{
				if (index < 0 || index > (myObjectTypeDeclarations.Count - 1))
					return null;

				return myObjectTypeDeclarations[index].DisplayText;
			}

			/// <summary>
			/// When implemented in a derived class, gets the image to show next to the specified item.
			/// </summary>
			/// <param name="index">[in] The index of the item for which to get the image index.</param>
			/// <returns>
			/// The index of the image from an image list, otherwise -1.
			/// </returns>
			public override int GetGlyph(int index)
			{
				if (index < 0 || index > (myObjectTypeDeclarations.Count - 1))
					return (Int32)FactEditorIconImageIndex.EntityType;

				return (Int32)myObjectTypeDeclarations[index].IconImageIndex;
			}
			#endregion // Overridden Abstract Methods
		}
	}
}
