#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using System.Globalization;
using Microsoft.VisualStudio.TextManager.Interop;
#endregion

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	partial class FactEditorLanguageService
	{
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
			/// Index of the general reference mode image
			/// </summary>
			GeneralReferenceMode = ValueType,
			/// <summary>
			/// Index of the objectified type image.
			/// </summary>
			ObjectifiedType = 2,
			/// <summary>
			/// Index of the popular reference mode image
			/// </summary>
			PopularReferenceMode = 3,
			/// <summary>
			/// Index of the unit based reference mode image
			/// </summary>
			UnitBasedReferenceMode = 4,
		}
		#endregion // FactEditorIconImageIndex enum
		#region FactEditorObjectTypeDeclarations class
		/// <summary>
		/// Manages a list of ObjectType declarations to be shown in an
		/// intellisense drop-down list.
		/// </summary>
		private sealed class FactEditorObjectTypeDeclarations : Declarations
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
			#region Private Members
			private FactEditorLanguageService myFactLanguageService;
			private List<ObjectTypeDeclaration> myObjectTypeDeclarations;
			#endregion // Private Members
			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="FactEditorObjectTypeDeclarations"/> class.
			/// </summary>
			/// <param name="languageService">The language service.</param>
			public FactEditorObjectTypeDeclarations(FactEditorLanguageService languageService)
			{
				myFactLanguageService = languageService;
				myObjectTypeDeclarations = new List<ObjectTypeDeclaration>();
				ReloadModelElements();
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
						Objectification objectification;
						if (objectType.IsImplicitBooleanValue ||
							(objectType.IsIndependent && // Preliminary check before pulling objectification
							null != (objectification = objectType.Objectification) &&
							objectification.IsImplied))
						{
							continue;
						}
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
			/// <summary>
			/// Let ']' and '(' be the only commit character for object types
			/// </summary>
			public override bool IsCommitChar(string textSoFar, int selected, char commitCharacter)
			{
				return commitCharacter == '(' || commitCharacter == ']';
			}
			/// <summary>
			/// Don't eat completion characters if no match is found
			/// </summary>
			public override string OnCommit(IVsTextView textView, string textSoFar, char commitCharacter, int index, ref TextSpan initialExtent)
			{
				return base.OnCommit(textView, textSoFar, commitCharacter, index, ref initialExtent) ?? textSoFar;
			}
			#endregion // Overridden Abstract Methods
		}
		#endregion // FactEditorObjectTypeDeclarations class
		#region FactEditorReferenceModeDeclarations class
		/// <summary>
		/// Manages a list of ReferenceMode declarations to be shown in an
		/// intellisense drop-down list.
		/// </summary>
		private sealed class FactEditorReferenceModeDeclarations : Declarations
		{
			#region ReferenceModeDeclaration class
			/// <summary>
			/// Represents a declaration in the intellisense list.
			/// </summary>
			private struct ReferenceModeDeclaration
			{
				#region Private Members
				private ReferenceMode myReferenceMode;
				#endregion // Private Members
				#region Comparer
				public static readonly IComparer<ReferenceModeDeclaration> Comparer = new ReferenceModeDeclarationComparer();
				private class ReferenceModeDeclarationComparer : IComparer<ReferenceModeDeclaration>
				{
					#region IComparer<ReferenceModeDeclaration> Implementation
					int IComparer<ReferenceModeDeclaration>.Compare(ReferenceModeDeclaration x, ReferenceModeDeclaration y)
					{
						return string.Compare(x.myReferenceMode.DecoratedName, y.myReferenceMode.DecoratedName, true, CultureInfo.CurrentCulture);
					}
					#endregion // IComparer<ReferenceModeDeclaration> Implementation
				}
				#endregion // Comparer
				#region Constructors
				/// <summary>
				/// Initializes a new instance of the <see cref="ReferenceModeDeclaration"/> class.
				/// </summary>
				/// <param name="referenceMode">The object type.</param>
				public ReferenceModeDeclaration(ReferenceMode referenceMode)
				{
					myReferenceMode = referenceMode;
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
						return myReferenceMode.DecoratedName;
					}
				}

				/// <summary>
				/// Gets the description for the item selected in the intellisense list.
				/// </summary>
				public string Description
				{
					get
					{
						return myReferenceMode.DecoratedName;
					}
				}

				/// <summary>
				/// Gets the text that is inserted when the Name is chosen in the intellisense list.
				/// </summary>
				public string DisplayText
				{
					get
					{
						return myReferenceMode.DecoratedName;
					}
				}

				/// <summary>
				/// Gets the FactEditorIconImageIndex that represents this declaration.
				/// </summary>
				public FactEditorIconImageIndex IconImageIndex
				{
					get
					{
						FactEditorIconImageIndex retVal = FactEditorIconImageIndex.GeneralReferenceMode;
						ReferenceModeKind kind = myReferenceMode.Kind;
						if (kind != null)
						{
							switch (kind.ReferenceModeType)
							{
								case ReferenceModeType.Popular:
									retVal = FactEditorIconImageIndex.PopularReferenceMode;
									break;
								case ReferenceModeType.UnitBased:
									retVal = FactEditorIconImageIndex.UnitBasedReferenceMode;
									break;
							}
						}
						return retVal;
					}
				}
				#endregion // Public Properties
			}
			#endregion // ReferenceModeDeclaration class
			#region Private Members
			private FactEditorLanguageService myFactLanguageService;
			private List<ReferenceModeDeclaration> myReferenceModeDeclarations;
			#endregion // Private Members
			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="FactEditorReferenceModeDeclarations"/> class.
			/// </summary>
			/// <param name="languageService">The language service.</param>
			public FactEditorReferenceModeDeclarations(FactEditorLanguageService languageService)
			{
				myFactLanguageService = languageService;
				myReferenceModeDeclarations = new List<ReferenceModeDeclaration>();
				ReloadModelElements();
			}
			#endregion // Constructors
			#region Private Methods
			private void ReloadModelElements()
			{
				// UNDONE: This is ridiculously inefficient. Use the ActivationManager to
				// register events so we only need to change this list for a second request
				// on the same document instead of repopulating it every time.
				List<ReferenceModeDeclaration> referenceModeEntries = myReferenceModeDeclarations;
				if (referenceModeEntries.Count != 0)
				{
					referenceModeEntries.Clear();
				}
				ActivationManager activationManager;
				ORMDesignerDocData docData;
				Store store;
				if (null != (activationManager = myFactLanguageService.m_ActivationManager) &&
					null != (docData = activationManager.CurrentDocument) &&
					null != (store = docData.Store) &&
					!store.Disposed)
				{
					foreach (ReferenceMode referenceMode in store.ElementDirectory.FindElements<ReferenceMode>())
					{
						referenceModeEntries.Add(new ReferenceModeDeclaration(referenceMode));
					}
					referenceModeEntries.Sort(ReferenceModeDeclaration.Comparer);
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
				return myReferenceModeDeclarations.Count;
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
				if (index < 0 || index > (myReferenceModeDeclarations.Count - 1))
					return null;

				return myReferenceModeDeclarations[index].Name;
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
				if (index < 0 || index > (myReferenceModeDeclarations.Count - 1))
					return null;

				return myReferenceModeDeclarations[index].Description;
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
				if (index < 0 || index > (myReferenceModeDeclarations.Count - 1))
					return null;

				return myReferenceModeDeclarations[index].DisplayText;
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
				if (index < 0 || index > (myReferenceModeDeclarations.Count - 1))
					return (Int32)FactEditorIconImageIndex.EntityType;

				return (Int32)myReferenceModeDeclarations[index].IconImageIndex;
			}
			/// <summary>
			/// Let ')' be the only commit character for reference modes
			/// </summary>
			public override bool IsCommitChar(string textSoFar, int selected, char commitCharacter)
			{
				return commitCharacter == ')';
			}
			/// <summary>
			/// Don't eat completion characters if no match is found
			/// </summary>
			public override string OnCommit(IVsTextView textView, string textSoFar, char commitCharacter, int index, ref TextSpan initialExtent)
			{
				return base.OnCommit(textView, textSoFar, commitCharacter, index, ref initialExtent) ?? textSoFar;
			}
			#endregion // Overridden Abstract Methods
		}
		#endregion // FactEditorReferenceModeDeclarations class
	}
}
