#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ObjectModel.Design;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Shell;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	#region Shape enums
	/// <summary>
	/// Valid shapes for object types
	/// </summary>
	public enum ObjectTypeDisplayShape
	{
		/// <summary>
		/// Draw object shapes with soft rectangles
		/// </summary>
		SoftRectangle,
		/// <summary>
		/// Draw object shapes with hard rectangles
		/// </summary>
		HardRectangle,
		/// <summary>
		/// Draw object shapes with ellipses
		/// </summary>
		Ellipse,
	}

	/// <summary>
	/// Valid shapes for objectified facts
	/// </summary>
	public enum ObjectifiedFactDisplayShape
	{
		/// <summary>
		/// Draw objectified facts with soft rectangles
		/// </summary>
		SoftRectangle,
		/// <summary>
		/// Draw objectified facts with hard rectangles
		/// </summary>
		HardRectangle,
	}

	/// <summary>
	/// Valid placements for mandatory dots
	/// </summary>
	public enum MandatoryDotPlacement
	{
		/// <summary>
		/// Place the mandatory dot at the object shape end of the connector
		/// </summary>
		ObjectShapeEnd,
		/// <summary>
		/// Place the mandatory dot at the role box end of the connector
		/// </summary>
		RoleBoxEnd,
	}
	/// <summary>
	/// Determines when to display external constraint attach
	/// point bars.
	/// </summary>
	public enum ExternalConstraintRoleBarDisplay
	{
		/// <summary>
		/// Only display a bar if one or more roles in
		/// the set of associated roles have a role in between them.
		/// This is the default setting.
		/// </summary>
		SplitRoles,
		/// <summary>
		/// Display a bar if there is more than one role, even
		/// if the roles are adjacent.
		/// </summary>
		AdjacentRoles,
		/// <summary>
		/// Display a bar even if there is only a single role.
		/// This is useful for navigation with keyboard and
		/// accessibility readers because it guarantees that all
		/// constraints attached to a fact have an associated
		/// visible and selectable item in a constraint box.
		/// </summary>
		AnyRole,
	}
	/// <summary>
	/// Determine when an internal uniqueness constraint
	/// that is a preferred identifier displays as a double line.
	/// These options are cumulative, so any higher display option
	/// turns on all other displays.
	/// </summary>
	public enum PreferredInternalUniquenessConstraintDisplay
	{
		/// <summary>
		/// Never display as preferred
		/// </summary>
		Never,
		/// <summary>
		/// Display as preferred when the fact is not
		/// explicitly or implicitly objectified. This only
		/// occurs with single-role uniqueness constraints
		/// on binary facts.
		/// </summary>
		UnobjectifiedInternalConstraint,
		/// <summary>
		/// Display as preferred when the fact is explicitly
		/// objectified and has multiple internal uniqueness constraints.
		/// </summary>
		MultipleObjectifiedInternalConstraints,
		/// <summary>
		/// Display as preferred when the fact is explicitly
		/// objectified and has a single internal constraint.
		/// </summary>
		SingleObjectifiedInternalConstraint,
		/// <summary>
		/// Display as preferred when the fact is implicitly
		/// objectified and has multiple internal uniqueness constraints.
		/// Implicit objectification occurs for n-aries and binaries with
		/// a spanning internal uniqueness constraint, so this will generally
		/// apply only to n-ary facts with more than one uniqueness constraint.
		/// </summary>
		MultipleImpliedObjectifiedInternalConstraints,
		/// <summary>
		/// Display as preferred when the fact is implicitly
		/// objectified and has a single internal constraint.
		/// Implicit objectification occurs for n-aries and binaries with
		/// a spanning internal uniqueness constraint and will draw
		/// spanning internal uniqueness constraints on binaries as preferred.
		/// </summary>
		SingleImpliedObjectifiedInternalConstraint,
	}
	/// <summary>
	/// Determine when a reading direction indicator is displayed for
	/// readings on binary fact types. These options are cumulative,
	/// so any higher display option turns on all other displays.
	/// </summary>
	public enum ReadingDirectionIndicatorDisplay
	{
		/// <summary>
		/// Display for reverse readings only
		/// </summary>
		Reversed,
		/// <summary>
		/// If the forward and reverse readings are split into
		/// two shapes, then display for both of them.
		/// </summary>
		Separated,
		/// <summary>
		/// Display if the fact type is rotated, even if the
		/// reading order is top-down.
		/// </summary>
		Rotated,
		/// <summary>
		/// Always display a reading direction indicator
		/// </summary>
		Always,
	}
	/// <summary>
	/// Options for overlaying EntityRelationship (ER) multiplicity
	/// display on top of ORM binary fact types. Meant as a temporary
	/// help for users with ER experience who are learning ORM.
	/// </summary>
	public enum EntityRelationshipBinaryMultiplicityDisplay
	{
		/// <summary>
		/// Normal ORM display, no ER overlay
		/// </summary>
		Off,
		/// <summary>
		/// Display just the crows foot only, indicating
		/// a 'many' multiplicity.
		/// </summary>
		CrowsFootOnly,
		/// <summary>
		/// Display Barker ER notation on role connectors for binary fact types.
		/// Optional roles use dotted lines, mandatory uses solid, and many
		/// is indicated by the crowsfoot.
		/// </summary>
		Barker,
		/// <summary>
		/// Display Information Engineering notation on binary fact types. Optional
		/// roles get an 0 on the line, mandatory roles get a 1, many is indicated
		/// by the crowsfoot.
		/// </summary>
		InformationEngineering,
	}
	#endregion // Shape enums
	#region Other Options Enums
	/// <summary>
	/// Provide options for showing and hiding role names on object types
	/// </summary>
	public enum RoleNameDisplay
	{
		/// <summary>
		/// Show role names
		/// </summary>
		On,
		/// <summary>
		/// Hide role names
		/// </summary>
		Off,
	}
	/// <summary>
	/// Provide options for the behavior of the Delete and Control-Delete keys
	/// </summary>
	public enum PrimaryDeleteBehavior
	{
		/// <summary>
		/// The Delete key deletes shapes, Ctrl-Delete deletes elements from
		/// the model
		/// </summary>
		DeleteShape,
		/// <summary>
		/// The Delete key deletes elements from the  model, Ctrl-Delete
		/// deletes shapes from the current diagram
		/// </summary>
		DeleteElement,
	}
	/// <summary>
	/// Provide options for the designer behavior when the final shape for
	/// an element is deleted from a diagram.
	/// </summary>
	public enum FinalShapeDeleteBehavior
	{
		/// <summary>
		/// Do not delete the underlying element from the object model
		/// </summary>
		DeleteShapeOnly,
		/// <summary>
		/// Delete the shape and the underlying element
		/// </summary>
		DeleteShapeAndElement,
		/// <summary>
		/// Ask the user if they would like to delete the underlying element
		/// </summary>
		Prompt,
	}
	/// <summary>
	/// Provide options for the behavior of hyperlinks in the verbalization browser
	/// </summary>
	public enum HyperlinkTargetWindow
	{
		/// <summary>
		/// Hyperlink to a shape in the primary document window
		/// </summary>
		DocumentWindow,
		/// <summary>
		/// Hyperlink to a shape in the Diagram Spy tool window
		/// </summary>
		DiagramSpyWindow,
	}
	#endregion
	#region NotifyDocument Delegate
	/// <summary>
	/// Used as a callback delegate for the OptionsPage.NotifySettingsChange
	/// method. Allow other global notification methods to walk all open
	/// documents.
	/// </summary>
	/// <param name="docData">A running ORMDesignerDocData to modify</param>
	[CLSCompliant(false)]
	public delegate void NotifyDocument(ORMDesignerDocData docData);
	#endregion // NotifyDocument Delegate
	/// <summary>
	/// Options dialog for ORM designers
	/// see https://projects.neumont.edu/orm2/wiki/HowToAddOptionPageOptions for adding options
	/// </summary>
	[CLSCompliant(false)]
	[Guid("B4ABD9FD-CE79-4B26-8D36-F345CB53B525")]
	public class OptionsPage : DialogPage
	{
		#region Localized PropertyDescriptor attribute classes
		//UNDONE: [Obsolete("UNDONE: 2006-06 DSL Tools port: This should probably be replaced with Microsoft.VisualStudio.Modeling.Design.CategoryResourceAttribute")]
		private sealed class LocalizedCategoryAttribute : CategoryAttribute
		{
			public LocalizedCategoryAttribute(string category) : base(category)
			{
			}
			protected sealed override string GetLocalizedString(string value)
			{
				return ResourceStrings.GetOptionsPageString(value);
			}
		}
		//UNDONE: [Obsolete("UNDONE: 2006-06 DSL Tools port: This should probably be replaced with Microsoft.VisualStudio.Modeling.Design.DescriptionResourceAttribute")]
		private sealed class LocalizedDescriptionAttribute : DescriptionAttribute
		{
			public LocalizedDescriptionAttribute(string description) : base(description)
			{
			}

			public sealed override string Description
			{
				get { return ResourceStrings.GetOptionsPageString(base.Description); }
			}
		}
		//UNDONE: [Obsolete("UNDONE: 2006-06 DSL Tools port: This should probably be replaced with Microsoft.VisualStudio.Modeling.Design.DisplayNameResourceAttribute")]
		private sealed class LocalizedDisplayNameAttribute : DisplayNameAttribute
		{
			public LocalizedDisplayNameAttribute(string displayName) : base(displayName)
			{
			}
			public sealed override string DisplayName
			{
				get { return ResourceStrings.GetOptionsPageString(base.DisplayName); }
			}
		}
		#endregion // Localized PropertyDescriptor attribute classes
		#region Member variables
		// If more settings are added, add a corresponding check in the OnApply override below
		private const ObjectTypeDisplayShape ObjectTypeDisplayShape_Default = ObjectTypeDisplayShape.SoftRectangle;
		private static ObjectTypeDisplayShape myCurrentObjectTypeDisplayShape = ObjectTypeDisplayShape_Default;
		private ObjectTypeDisplayShape myObjectTypeDisplayShape = ObjectTypeDisplayShape_Default;

		private const ObjectifiedFactDisplayShape ObjectifiedFactDisplayShape_Default = ObjectifiedFactDisplayShape.SoftRectangle;
		private static ObjectifiedFactDisplayShape myCurrentObjectifiedFactDisplayShape = ObjectifiedFactDisplayShape_Default;
		private ObjectifiedFactDisplayShape myObjectifiedFactDisplayShape = ObjectifiedFactDisplayShape_Default;

		private const MandatoryDotPlacement MandatoryDotPlacement_Default = MandatoryDotPlacement.RoleBoxEnd;
		private static MandatoryDotPlacement myCurrentMandatoryDotPlacement = MandatoryDotPlacement_Default;
		private MandatoryDotPlacement myMandatoryDotPlacement = MandatoryDotPlacement_Default;

		private const RoleNameDisplay RoleNameDisplay_Default = RoleNameDisplay.On;
		private static RoleNameDisplay myCurrentRoleNameDisplay = RoleNameDisplay_Default;
		private RoleNameDisplay myRoleNameDisplay = RoleNameDisplay_Default;

		private const bool DisplayShadows_Default = true;
		private static bool myCurrentDisplayShadows = DisplayShadows_Default;
		private bool myDisplayShadows = DisplayShadows_Default;

		private const PortableDataType DefaultDataType_Default = PortableDataType.Unspecified;
		private static PortableDataType myCurrentDefaultDataType = DefaultDataType_Default;
		private PortableDataType myDefaultDataType = DefaultDataType_Default;

		private const ExternalConstraintRoleBarDisplay ExternalConstraintRoleBarDisplay_Default = ExternalConstraintRoleBarDisplay.SplitRoles;
		private static ExternalConstraintRoleBarDisplay myCurrentExternalConstraintRoleBarDisplay = ExternalConstraintRoleBarDisplay_Default;
		private ExternalConstraintRoleBarDisplay myExternalConstraintRoleBarDisplay = ExternalConstraintRoleBarDisplay_Default;

		private const PrimaryDeleteBehavior PrimaryDeleteBehavior_Default = PrimaryDeleteBehavior.DeleteShape;
		private static PrimaryDeleteBehavior myCurrentPrimaryDeleteBehavior = PrimaryDeleteBehavior_Default;
		private PrimaryDeleteBehavior myPrimaryDeleteBehavior = PrimaryDeleteBehavior_Default;

		private const FinalShapeDeleteBehavior FinalShapeDeleteBehavior_Default = FinalShapeDeleteBehavior.Prompt;
		private static FinalShapeDeleteBehavior myCurrentFinalShapeDeleteBehavior = FinalShapeDeleteBehavior_Default;
		private FinalShapeDeleteBehavior myFinalShapeDeleteBehavior = FinalShapeDeleteBehavior_Default;

		private const bool CombineMandatoryAndUniqueVerbalization_Default = true;
		private static bool myCurrentCombineMandatoryAndUniqueVerbalization = CombineMandatoryAndUniqueVerbalization_Default;
		private bool myCombineMandatoryAndUniqueVerbalization = CombineMandatoryAndUniqueVerbalization_Default;

		private const bool ShowDefaultConstraintVerbalization_Default = true;
		private static bool myCurrentShowDefaultConstraintVerbalization = ShowDefaultConstraintVerbalization_Default;
		private bool myShowDefaultConstraintVerbalization = ShowDefaultConstraintVerbalization_Default;

		private const bool VerbalizeFactTypesWithObjectType_Default = true;
		private static bool myCurrentVerbalizeFactTypesWithObjectType = VerbalizeFactTypesWithObjectType_Default;
		private bool myVerbalizeFactTypesWithObjectType = VerbalizeFactTypesWithObjectType_Default;

		private const HyperlinkTargetWindow VerbalizationHyperlinkTarget_Default = HyperlinkTargetWindow.DiagramSpyWindow;
		private static HyperlinkTargetWindow myCurrentVerbalizationHyperlinkTarget = VerbalizationHyperlinkTarget_Default;
		private HyperlinkTargetWindow myVerbalizationHyperlinkTarget = VerbalizationHyperlinkTarget_Default;

		private const ObjectTypeNameVerbalizationStyle VerbalizationObjectTypeNameDisplay_Default = ObjectTypeNameVerbalizationStyle.AsIs;
		private static ObjectTypeNameVerbalizationStyle myCurrentVerbalizationObjectTypeNameDisplay = VerbalizationObjectTypeNameDisplay_Default;
		private ObjectTypeNameVerbalizationStyle myVerbalizationObjectTypeNameDisplay = VerbalizationObjectTypeNameDisplay_Default;

		private const string VerbalizationRemoveObjectTypeNameCharactersOnSeparate_Default = ".:_";
		private static string myCurrentVerbalizationRemoveObjectTypeNameCharactersOnSeparate = VerbalizationRemoveObjectTypeNameCharactersOnSeparate_Default;
		private string myVerbalizationRemoveObjectTypeNameCharactersOnSeparate = VerbalizationRemoveObjectTypeNameCharactersOnSeparate_Default;

		private const string CustomVerbalizationSnippets_Default = "";
		private static string myCurrentCustomVerbalizationSnippets = CustomVerbalizationSnippets_Default;
		private string myCustomVerbalizationSnippets = CustomVerbalizationSnippets_Default;

		private const PreferredInternalUniquenessConstraintDisplay PreferredInternalUniquenessConstraintDisplay_Default = PreferredInternalUniquenessConstraintDisplay.MultipleObjectifiedInternalConstraints;
		private static PreferredInternalUniquenessConstraintDisplay myCurrentPreferredInternalUniquenessConstraintDisplay = PreferredInternalUniquenessConstraintDisplay_Default;
		private PreferredInternalUniquenessConstraintDisplay myPreferredInternalUniquenessConstraintDisplay = PreferredInternalUniquenessConstraintDisplay_Default;

		private const ReadingDirectionIndicatorDisplay ReadingDirectionIndicatorDisplay_Default = ReadingDirectionIndicatorDisplay.Separated;
		private static ReadingDirectionIndicatorDisplay myCurrentReadingDirectionIndicatorDisplay = ReadingDirectionIndicatorDisplay_Default;
		private ReadingDirectionIndicatorDisplay myReadingDirectionIndicatorDisplay = ReadingDirectionIndicatorDisplay_Default;

		private const bool ShowDebugCommands_Default = 
#if DEBUG
			true;
#else
			false;
#endif
		private static bool myCurrentShowDebugCommands = ShowDebugCommands_Default;
		private bool myShowDebugCommands = ShowDebugCommands_Default;

		private const EntityRelationshipBinaryMultiplicityDisplay EntityRelationshipBinaryMultiplicityDisplay_Default = EntityRelationshipBinaryMultiplicityDisplay.Off;
		private static EntityRelationshipBinaryMultiplicityDisplay myCurrentEntityRelationshipBinaryMultiplicityDisplay = EntityRelationshipBinaryMultiplicityDisplay_Default;
		private EntityRelationshipBinaryMultiplicityDisplay myEntityRelationshipBinaryMultiplicityDisplay = EntityRelationshipBinaryMultiplicityDisplay_Default;

		private const bool DisplayDefinitionTooltips_Default = false;
		private static bool myCurrentDisplayDefinitionTooltips = DisplayDefinitionTooltips_Default;
		private bool myDisplayDefinitionTooltips = DisplayDefinitionTooltips_Default;

		private const bool DelayActivateModelBrowserLabelEdits_Default = false;
		private static bool myCurrentDelayActivateModelBrowserLabelEdits = DelayActivateModelBrowserLabelEdits_Default;
		private bool myDelayActivateModelBrowserLabelEdits = DelayActivateModelBrowserLabelEdits_Default;
		#endregion // Member variables
		#region Custom Loading
		/// <summary>
		/// Load the cached settings into the current instance
		/// </summary>
		public void Initialize()
		{
			OnActivate(null);
		}
		/// <summary>
		/// Apply changes for a locally created options page
		/// </summary>
		public void ApplyChanges()
		{
			SaveSettingsToStorage();
			OnApply(null);
		}
		#endregion // Custom Loading
		#region Base overrides
		/// <summary>
		/// Set the current values of the static properties
		/// to match the cached settings
		/// </summary>
		public override void LoadSettingsFromStorage()
		{
			base.LoadSettingsFromStorage();
			myCurrentObjectTypeDisplayShape = myObjectTypeDisplayShape;
			myCurrentObjectifiedFactDisplayShape = myObjectifiedFactDisplayShape;
			myCurrentMandatoryDotPlacement = myMandatoryDotPlacement;
			myCurrentRoleNameDisplay = myRoleNameDisplay;
			myCurrentDisplayShadows = myDisplayShadows;
			myCurrentDefaultDataType = myDefaultDataType;
			myCurrentExternalConstraintRoleBarDisplay = myExternalConstraintRoleBarDisplay;
			myCurrentPrimaryDeleteBehavior = myPrimaryDeleteBehavior;
			myCurrentFinalShapeDeleteBehavior = myFinalShapeDeleteBehavior;
			myCurrentCombineMandatoryAndUniqueVerbalization = myCombineMandatoryAndUniqueVerbalization;
			myCurrentShowDefaultConstraintVerbalization = myShowDefaultConstraintVerbalization;
			myCurrentVerbalizeFactTypesWithObjectType = myVerbalizeFactTypesWithObjectType;
			myCurrentVerbalizationHyperlinkTarget = myVerbalizationHyperlinkTarget;
			myCurrentVerbalizationObjectTypeNameDisplay = myVerbalizationObjectTypeNameDisplay;
			myCurrentVerbalizationRemoveObjectTypeNameCharactersOnSeparate = myVerbalizationRemoveObjectTypeNameCharactersOnSeparate;
			myCurrentCustomVerbalizationSnippets = myCustomVerbalizationSnippets;
			myCurrentPreferredInternalUniquenessConstraintDisplay = myPreferredInternalUniquenessConstraintDisplay;
			myCurrentReadingDirectionIndicatorDisplay = myReadingDirectionIndicatorDisplay;
			myCurrentShowDebugCommands = myShowDebugCommands;
			myCurrentEntityRelationshipBinaryMultiplicityDisplay = myEntityRelationshipBinaryMultiplicityDisplay;
			myCurrentDisplayDefinitionTooltips = myDisplayDefinitionTooltips;
			myCurrentDelayActivateModelBrowserLabelEdits = myDelayActivateModelBrowserLabelEdits;
		}
		/// <summary>
		/// Set local values for the current settings to determine later if the
		/// settings have changed in the OnApply method.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnActivate(CancelEventArgs e)
		{
			myObjectTypeDisplayShape = myCurrentObjectTypeDisplayShape;
			myObjectifiedFactDisplayShape = myCurrentObjectifiedFactDisplayShape;
			myMandatoryDotPlacement = myCurrentMandatoryDotPlacement;
			myRoleNameDisplay = myCurrentRoleNameDisplay;
			myDisplayShadows = myCurrentDisplayShadows;
			myDefaultDataType = myCurrentDefaultDataType;
			myExternalConstraintRoleBarDisplay = myCurrentExternalConstraintRoleBarDisplay;
			myPrimaryDeleteBehavior = myCurrentPrimaryDeleteBehavior;
			myFinalShapeDeleteBehavior = myCurrentFinalShapeDeleteBehavior;
			myCombineMandatoryAndUniqueVerbalization = myCurrentCombineMandatoryAndUniqueVerbalization;
			myShowDefaultConstraintVerbalization = myCurrentShowDefaultConstraintVerbalization;
			myVerbalizeFactTypesWithObjectType = myCurrentVerbalizeFactTypesWithObjectType;
			myVerbalizationHyperlinkTarget = myCurrentVerbalizationHyperlinkTarget;
			myVerbalizationObjectTypeNameDisplay = myCurrentVerbalizationObjectTypeNameDisplay;
			myVerbalizationRemoveObjectTypeNameCharactersOnSeparate = myCurrentVerbalizationRemoveObjectTypeNameCharactersOnSeparate;
			myCustomVerbalizationSnippets = myCurrentCustomVerbalizationSnippets;
			myPreferredInternalUniquenessConstraintDisplay = myCurrentPreferredInternalUniquenessConstraintDisplay;
			myReadingDirectionIndicatorDisplay = myCurrentReadingDirectionIndicatorDisplay;
			myShowDebugCommands = myCurrentShowDebugCommands;
			myEntityRelationshipBinaryMultiplicityDisplay = myCurrentEntityRelationshipBinaryMultiplicityDisplay;
			myDisplayDefinitionTooltips = myCurrentDisplayDefinitionTooltips;
			myDelayActivateModelBrowserLabelEdits = myCurrentDelayActivateModelBrowserLabelEdits;
		}

		/// <summary>
		/// Invalidate each loaded ORM diagram to force a redraw of the shapes
		/// </summary>
		/// <param name="e"></param>
		protected override void OnApply(DialogPage.PageApplyEventArgs e)
		{
			bool updateVerbalizer =
				myCurrentCombineMandatoryAndUniqueVerbalization != myCombineMandatoryAndUniqueVerbalization ||
				myCurrentShowDefaultConstraintVerbalization != myShowDefaultConstraintVerbalization ||
				myCurrentVerbalizeFactTypesWithObjectType != myVerbalizeFactTypesWithObjectType ||
				myCurrentCustomVerbalizationSnippets != myCustomVerbalizationSnippets ||
				myCurrentVerbalizationObjectTypeNameDisplay != myVerbalizationObjectTypeNameDisplay ||
				myCurrentVerbalizationRemoveObjectTypeNameCharactersOnSeparate != myVerbalizationRemoveObjectTypeNameCharactersOnSeparate;
			// Get out early if none of the displayed settings have changed
			if (myCurrentMandatoryDotPlacement == myMandatoryDotPlacement &&
				myCurrentObjectifiedFactDisplayShape == myObjectifiedFactDisplayShape &&
				myCurrentObjectTypeDisplayShape == myObjectTypeDisplayShape &&
				myCurrentRoleNameDisplay == myRoleNameDisplay &&
				myCurrentDisplayShadows == myDisplayShadows &&
				myCurrentExternalConstraintRoleBarDisplay == myExternalConstraintRoleBarDisplay &&
				myCurrentPreferredInternalUniquenessConstraintDisplay == myPreferredInternalUniquenessConstraintDisplay &&
				myCurrentReadingDirectionIndicatorDisplay == myReadingDirectionIndicatorDisplay &&
				myCurrentEntityRelationshipBinaryMultiplicityDisplay == myEntityRelationshipBinaryMultiplicityDisplay)
			{
				// Non-displayed setting, don't notify
				myCurrentDefaultDataType = myDefaultDataType;
				myCurrentPrimaryDeleteBehavior = myPrimaryDeleteBehavior;
				myCurrentFinalShapeDeleteBehavior = myFinalShapeDeleteBehavior;
				myCurrentCombineMandatoryAndUniqueVerbalization = myCombineMandatoryAndUniqueVerbalization;
				myCurrentShowDefaultConstraintVerbalization = myShowDefaultConstraintVerbalization;
				myCurrentVerbalizeFactTypesWithObjectType = myVerbalizeFactTypesWithObjectType;
				myCurrentVerbalizationHyperlinkTarget = myVerbalizationHyperlinkTarget;
				myCurrentVerbalizationObjectTypeNameDisplay = myVerbalizationObjectTypeNameDisplay;
				myCurrentVerbalizationRemoveObjectTypeNameCharactersOnSeparate = myVerbalizationRemoveObjectTypeNameCharactersOnSeparate;
				myCurrentCustomVerbalizationSnippets = myCustomVerbalizationSnippets;
				myCurrentShowDebugCommands = myShowDebugCommands;
				myCurrentDisplayDefinitionTooltips = myDisplayDefinitionTooltips;
				myCurrentDelayActivateModelBrowserLabelEdits = myDelayActivateModelBrowserLabelEdits;
				if (updateVerbalizer)
				{
					ORMDesignerPackage.VerbalizationWindowGlobalSettingsChanged();
				}
				return;
			}

			// See if facts need resizing
			bool resizeFactShapes = myCurrentExternalConstraintRoleBarDisplay != myExternalConstraintRoleBarDisplay;
			bool updateRoleNames = myCurrentRoleNameDisplay != myRoleNameDisplay;
			bool resizeReadingShapes = myCurrentReadingDirectionIndicatorDisplay != myReadingDirectionIndicatorDisplay;

			// Set the new options
			myCurrentMandatoryDotPlacement = myMandatoryDotPlacement;
			myCurrentObjectifiedFactDisplayShape = myObjectifiedFactDisplayShape;
			myCurrentObjectTypeDisplayShape = myObjectTypeDisplayShape;
			myCurrentRoleNameDisplay = myRoleNameDisplay;
			myCurrentDisplayShadows = myDisplayShadows;
			myCurrentExternalConstraintRoleBarDisplay = myExternalConstraintRoleBarDisplay;
			myCurrentDefaultDataType = myDefaultDataType;
			myCurrentPrimaryDeleteBehavior = myPrimaryDeleteBehavior;
			myCurrentFinalShapeDeleteBehavior = myFinalShapeDeleteBehavior;
			myCurrentCombineMandatoryAndUniqueVerbalization = myCombineMandatoryAndUniqueVerbalization;
			myCurrentShowDefaultConstraintVerbalization = myShowDefaultConstraintVerbalization;
			myCurrentVerbalizeFactTypesWithObjectType = myVerbalizeFactTypesWithObjectType;
			myCurrentVerbalizationHyperlinkTarget = myVerbalizationHyperlinkTarget;
			myCurrentVerbalizationObjectTypeNameDisplay = myVerbalizationObjectTypeNameDisplay;
			myCurrentVerbalizationRemoveObjectTypeNameCharactersOnSeparate = myVerbalizationRemoveObjectTypeNameCharactersOnSeparate;
			myCurrentCustomVerbalizationSnippets = myCustomVerbalizationSnippets;
			myCurrentPreferredInternalUniquenessConstraintDisplay = myPreferredInternalUniquenessConstraintDisplay;
			myCurrentReadingDirectionIndicatorDisplay = myReadingDirectionIndicatorDisplay;
			myCurrentShowDebugCommands = myShowDebugCommands;
			myCurrentEntityRelationshipBinaryMultiplicityDisplay = myEntityRelationshipBinaryMultiplicityDisplay;
			myCurrentDisplayDefinitionTooltips = myDisplayDefinitionTooltips;
			myCurrentDelayActivateModelBrowserLabelEdits = myDelayActivateModelBrowserLabelEdits;

			// Walk all the documents and invalidate ORM diagrams if the options have changed
			NotifySettingsChange(
				Site,
				delegate(ORMDesignerDocData docData)
			{
				ReadOnlyCollection<ORMDiagram> diagrams = docData.Store.ElementDirectory.FindElements<ORMDiagram>();
				int diagramCount = diagrams.Count;
				for (int i = 0; i < diagramCount; ++i)
				{
					ORMDiagram diagram = diagrams[i];
					using (Transaction t = diagram.Store.TransactionManager.BeginTransaction(ResourceStrings.OptionsPageChangeTransactionName))
					{
						Store store = diagram.Store;
						foreach (BinaryLinkShape link in store.ElementDirectory.FindElements<BinaryLinkShape>(true))
						{
							link.RecalculateRoute();
						}
						if (resizeFactShapes)
						{
							foreach (FactTypeShape factShape in store.ElementDirectory.FindElements<FactTypeShape>(true))
							{
								factShape.AutoResize();
							}
						}
						if (updateRoleNames)
						{
							foreach (Role role in store.ElementDirectory.FindElements<Role>(true))
							{
								RoleNameShape.SetRoleNameDisplay(role.FactType);
							}
						}
						if (resizeReadingShapes)
						{
							foreach (ReadingShape readingShape in store.ElementDirectory.FindElements<ReadingShape>(true))
							{
								readingShape.InvalidateDisplayText();
							}
						}
						if (t.HasPendingChanges)
						{
							t.Commit();
						}
					}
					diagram.Invalidate(true);
				}
			});
			if (updateVerbalizer)
			{
				ORMDesignerPackage.VerbalizationWindowGlobalSettingsChanged();
			}
		}
		#endregion // Base overrides
		#region Change Notification Functions
		/// <summary>
		/// Walk all running ORMDesigner documents and callback to the
		/// notification delegate. Used to notify settings changes from
		/// the options page or other change sources (like the FontAndColors
		/// settings).
		/// </summary>
		/// <param name="serviceProvider">IServiceProvider</param>
		/// <param name="changeCallback">Delegate callback</param>
		public static void NotifySettingsChange(IServiceProvider serviceProvider, NotifyDocument changeCallback)
		{
			// Fire the change callback for all loaded ORM diagrams
			foreach (ORMDesignerDocData docData in FrameworkShellUtility.GetLoadedDocuments<ORMDesignerDocData>(serviceProvider))
			{
				changeCallback(docData);
			}
		}
		#endregion Change Notification Functions
		#region Accessor properties
		/// <summary>
		/// Object Type Shape option
		/// </summary>
		[DefaultValue(ObjectTypeDisplayShape_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryAppearanceId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyObjectTypeShapeDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyObjectTypeShapeDisplayNameId)]
		public ObjectTypeDisplayShape ObjectTypeDisplayShape
		{
			get { return myObjectTypeDisplayShape; }
			set { myObjectTypeDisplayShape = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for ObjectTypeDisplayShape
		/// </summary>
		public static ObjectTypeDisplayShape CurrentObjectTypeDisplayShape
		{
			get { return myCurrentObjectTypeDisplayShape; }
		}

		/// <summary>
		/// Objectified Shape option
		/// </summary>
		[DefaultValue(ObjectifiedFactDisplayShape_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryAppearanceId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyObjectifiedShapeDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyObjectifiedShapeDisplayNameId)]
		public ObjectifiedFactDisplayShape ObjectifiedFactDisplayShape
		{
			get { return myObjectifiedFactDisplayShape; }
			set { myObjectifiedFactDisplayShape = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for ObjectifiedFactShape
		/// </summary>
		public static ObjectifiedFactDisplayShape CurrentObjectifiedFactDisplayShape
		{
			get { return myCurrentObjectifiedFactDisplayShape;  }
		}

		/// <summary>
		/// Objectified Shape option
		/// </summary>
		[DefaultValue(MandatoryDotPlacement_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryAppearanceId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyMandatoryDotDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyMandatoryDotDisplayNameId)]
		public MandatoryDotPlacement MandatoryDotPlacement
		{
			get { return myMandatoryDotPlacement; }
			set { myMandatoryDotPlacement = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for MandatoryDotPlacement
		/// </summary>
		public static MandatoryDotPlacement CurrentMandatoryDotPlacement
		{
			get { return myCurrentMandatoryDotPlacement; }
		}

		/// <summary>
		/// Display of role names
		/// </summary>
		[DefaultValue(RoleNameDisplay_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryAppearanceId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyRoleNameDisplayDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyRoleNameDisplayDisplayNameId)]
		public RoleNameDisplay RoleNameDisplay
		{
			get { return myRoleNameDisplay; }
			set { myRoleNameDisplay = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for RoleNameDisplay
		/// </summary>
		public static RoleNameDisplay CurrentRoleNameDisplay
		{
			get { return myCurrentRoleNameDisplay; }
		}

		/// <summary>
		/// Display of shape shadow
		/// </summary>
		[DefaultValue(DisplayShadows_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryAppearanceId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyDisplayShadowsDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyDisplayShadowsDisplayNameId)]
		public bool DisplayShadows
		{
			get { return myDisplayShadows; }
			set { myDisplayShadows = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for DisplayShadows
		/// </summary>
		public static bool CurrentDisplayShadows
		{
			get { return myCurrentDisplayShadows; }
		}

		/// <summary>
		/// Default data type for value types
		/// </summary>
		[DefaultValue(DefaultDataType_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryDataTypesId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyDataTypeDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyDataTypeDisplayNameId)]
		public PortableDataType DefaultDataType
		{
			get { return myDefaultDataType; }
			set
			{
				// UNDONE: Figure out how to get this value out of the list
				if (value != PortableDataType.UserDefined)
				{
					myDefaultDataType = value;
				}
			}
		}

		/// <summary>
		/// Current VS session-wide setting default data type
		/// when a new value type is added
		/// </summary>
		public static PortableDataType CurrentDefaultDataType
		{
			get { return myCurrentDefaultDataType; }
		}

		/// <summary>
		/// Display of external constraint bars
		/// </summary>
		[DefaultValue(ExternalConstraintRoleBarDisplay_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryAppearanceId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyExternalConstraintRoleBarDisplayDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyExternalConstraintRoleBarDisplayNameId)]
		public ExternalConstraintRoleBarDisplay ExternalConstraintRoleBarDisplay
		{
			get { return myExternalConstraintRoleBarDisplay; }
			set { myExternalConstraintRoleBarDisplay = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for ExternalConstraintRoleBarDisplay
		/// </summary>
		public static ExternalConstraintRoleBarDisplay CurrentExternalConstraintRoleBarDisplay
		{
			get { return myCurrentExternalConstraintRoleBarDisplay; }
		}

		/// <summary>
		/// Behavior of Delete and Control-Delete keys
		/// </summary>
		[DefaultValue(PrimaryDeleteBehavior_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryDeleteBehaviorId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyPrimaryDeleteBehaviorDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyPrimaryDeleteBehaviorDisplayNameId)]
		public PrimaryDeleteBehavior PrimaryDeleteBehavior
		{
			get { return myPrimaryDeleteBehavior; }
			set { myPrimaryDeleteBehavior = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for PrimaryDeleteBehavior
		/// </summary>
		public static PrimaryDeleteBehavior CurrentPrimaryDeleteBehavior
		{
			get { return myCurrentPrimaryDeleteBehavior; }
		}

		/// <summary>
		/// Behavior of final shape deletion
		/// </summary>
		[DefaultValue(FinalShapeDeleteBehavior_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryDeleteBehaviorId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyFinalShapeDeleteBehaviorDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyFinalShapeDeleteBehaviorDisplayNameId)]
		public FinalShapeDeleteBehavior FinalShapeDeleteBehavior
		{
			get { return myFinalShapeDeleteBehavior; }
			set { myFinalShapeDeleteBehavior = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for PrimaryDeleteBehavior
		/// </summary>
		public static FinalShapeDeleteBehavior CurrentFinalShapeDeleteBehavior
		{
			get { return myCurrentFinalShapeDeleteBehavior; }
		}

		/// <summary>
		/// Current setting for CombineMandatoryAndUniqueVerbalization
		/// </summary>
		[DefaultValue(CombineMandatoryAndUniqueVerbalization_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryVerbalizationBehaviorId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyCombineMandatoryAndUniqueVerbalizationDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyCombineMandatoryAndUniqueVerbalizationDisplayNameId)]
		public bool CombineMandatoryAndUniqueVerbalization
		{
			get { return myCombineMandatoryAndUniqueVerbalization; }
			set { myCombineMandatoryAndUniqueVerbalization = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for CombineMandatoryAndUniqueVerbalization
		/// </summary>
		public static bool CurrentCombineMandatoryAndUniqueVerbalization
		{
			get { return myCurrentCombineMandatoryAndUniqueVerbalization; }
		}

		/// <summary>
		/// Current setting for ShowDefaultConstraintVerbalization
		/// </summary>
		[DefaultValue(ShowDefaultConstraintVerbalization_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryVerbalizationBehaviorId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyShowDefaultConstraintVerbalizationDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyShowDefaultConstraintVerbalizationDisplayNameId)]
		public bool ShowDefaultConstraintVerbalization
		{
			get { return myShowDefaultConstraintVerbalization; }
			set { myShowDefaultConstraintVerbalization = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for ShowDefaultConstraintVerbalization
		/// </summary>
		public static bool CurrentShowDefaultConstraintVerbalization
		{
			get { return myCurrentShowDefaultConstraintVerbalization; }
		}

		/// <summary>
		/// Current setting for VerbalizeFactTypesWithObjectType
		/// </summary>
		[DefaultValue(VerbalizeFactTypesWithObjectType_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryVerbalizationBehaviorId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyVerbalizeFactTypesWithObjectTypeDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyVerbalizeFactTypesWithObjectTypeDisplayNameId)]
		public bool VerbalizeFactTypesWithObjectType
		{
			get { return myVerbalizeFactTypesWithObjectType; }
			set { myVerbalizeFactTypesWithObjectType = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for VerbalizeFactTypesWithObjectType
		/// </summary>
		public static bool CurrentVerbalizeFactTypesWithObjectType
		{
			get { return myCurrentVerbalizeFactTypesWithObjectType; }
		}

		/// <summary>
		/// Current setting for VerbalizationHyperlinkTarget
		/// </summary>
		[DefaultValue(VerbalizationHyperlinkTarget_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryVerbalizationBehaviorId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyVerbalizationHyperlinkTargetDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyVerbalizationHyperlinkTargetDisplayNameId)]
		public HyperlinkTargetWindow VerbalizationHyperlinkTarget
		{
			get { return myVerbalizationHyperlinkTarget; }
			set { myVerbalizationHyperlinkTarget = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for VerbalizationHyperlinkTarget
		/// </summary>
		public static HyperlinkTargetWindow CurrentVerbalizationHyperlinkTarget
		{
			get { return myCurrentVerbalizationHyperlinkTarget; }
		}

		/// <summary>
		/// Current setting for VerbalizationObjectTypeNameDisplay
		/// </summary>
		[DefaultValue(VerbalizationObjectTypeNameDisplay_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryVerbalizationBehaviorId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyVerbalizationObjectTypeNameDisplayDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyVerbalizationObjectTypeNameDisplayDisplayNameId)]
		public ObjectTypeNameVerbalizationStyle VerbalizationObjectTypeNameDisplay
		{
			get { return myVerbalizationObjectTypeNameDisplay; }
			set { myVerbalizationObjectTypeNameDisplay = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for VerbalizationObjectTypeNameDisplay
		/// </summary>
		public static ObjectTypeNameVerbalizationStyle CurrentVerbalizationObjectTypeNameDisplay
		{
			get { return myCurrentVerbalizationObjectTypeNameDisplay; }
		}

		/// <summary>
		/// Current setting for VerbalizationRemoveObjectTypeNameCharactersOnSeparate
		/// </summary>
		[DefaultValue(VerbalizationRemoveObjectTypeNameCharactersOnSeparate_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryVerbalizationBehaviorId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyVerbalizationRemoveObjectTypeNameCharactersOnSeparateDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyVerbalizationRemoveObjectTypeNameCharactersOnSeparateDisplayNameId)]
		public string VerbalizationRemoveObjectTypeNameCharactersOnSeparate
		{
			get { return myVerbalizationRemoveObjectTypeNameCharactersOnSeparate; }
			set { myVerbalizationRemoveObjectTypeNameCharactersOnSeparate = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for VerbalizationRemoveObjectTypeNameCharactersOnSeparate
		/// </summary>
		public static string CurrentVerbalizationRemoveObjectTypeNameCharactersOnSeparate
		{
			get { return myCurrentVerbalizationRemoveObjectTypeNameCharactersOnSeparate; }
		}

		/// <summary>
		/// Current setting for CustomVerbalizationSnippets
		/// </summary>
		[DefaultValue(CustomVerbalizationSnippets_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryVerbalizationBehaviorId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyCustomVerbalizationSnippetsDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyCustomVerbalizationSnippetsDisplayNameId)]
		[Editor(typeof(CustomVerbalizationSnippetsEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(CustomVerbalizationSnippetsConverter))]
		public string CustomVerbalizationSnippets
		{
			get { return myCustomVerbalizationSnippets; }
			set { myCustomVerbalizationSnippets = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for CustomVerbalizationSnippets
		/// </summary>
		public static string CurrentCustomVerbalizationSnippets
		{
			get { return myCurrentCustomVerbalizationSnippets; }
		}

		/// <summary>
		/// Current setting for PreferredInternalUniquenessConstraintDisplay
		/// </summary>
		[DefaultValue(PreferredInternalUniquenessConstraintDisplay_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryAppearanceId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyPreferredInternalUniquenessConstraintDisplayDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyPreferredInternalUniquenessConstraintDisplayDisplayNameId)]
		public PreferredInternalUniquenessConstraintDisplay PreferredInternalUniquenessConstraintDisplay
		{
			get { return myPreferredInternalUniquenessConstraintDisplay; }
			set { myPreferredInternalUniquenessConstraintDisplay = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for PreferredInternalUniquenessConstraintDisplay
		/// </summary>
		public static PreferredInternalUniquenessConstraintDisplay CurrentPreferredInternalUniquenessConstraintDisplay
		{
			get { return myCurrentPreferredInternalUniquenessConstraintDisplay; }
		}
		/// <summary>
		/// Current setting for ReadingDirectionIndicatorDisplay
		/// </summary>
		[DefaultValue(ReadingDirectionIndicatorDisplay_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryAppearanceId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyReadingDirectionIndicatorDisplayDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyReadingDirectionIndicatorDisplayDisplayNameId)]
		public ReadingDirectionIndicatorDisplay ReadingDirectionIndicatorDisplay
		{
			get { return myReadingDirectionIndicatorDisplay; }
			set { myReadingDirectionIndicatorDisplay = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for ReadingDirectionIndicatorDisplay
		/// </summary>
		public static ReadingDirectionIndicatorDisplay CurrentReadingDirectionIndicatorDisplay
		{
			get { return myCurrentReadingDirectionIndicatorDisplay; }
		}
		/// <summary>
		/// Current setting for ShowDebugCommands
		/// </summary>
		[DefaultValue(ShowDebugCommands_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryDiagnosticsId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyShowDebugCommandsDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyShowDebugCommandsDisplayNameId)]
		public bool ShowDebugCommands
		{
			get { return myShowDebugCommands; }
			set { myShowDebugCommands = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for ShowDebugCommands
		/// </summary>
		public static bool CurrentShowDebugCommands
		{
			get { return myCurrentShowDebugCommands; }
		}

		/// <summary>
		/// Entity Relationship Binary Multiplicity Display option
		/// </summary>
		[DefaultValue(EntityRelationshipBinaryMultiplicityDisplay_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryEntityRelationshipLearningModeId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyEntityRelationshipBinaryMultiplicityDisplayDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyEntityRelationshipBinaryMultiplicityDisplayDisplayNameId)]
		public EntityRelationshipBinaryMultiplicityDisplay EntityRelationshipBinaryMultiplicityDisplay
		{
			get { return myEntityRelationshipBinaryMultiplicityDisplay; }
			set { myEntityRelationshipBinaryMultiplicityDisplay = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for EntityRelationshipBinaryMultiplicityDisplay
		/// </summary>
		public static EntityRelationshipBinaryMultiplicityDisplay CurrentEntityRelationshipBinaryMultiplicityDisplay
		{
			get { return myCurrentEntityRelationshipBinaryMultiplicityDisplay; }
		}

		/// <summary>
		/// Display Definition Tooltip option
		/// </summary>
		[DefaultValue(DisplayDefinitionTooltips_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryAppearanceId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyDisplayDefinitionTooltipsDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyDisplayDefinitionTooltipsDisplayNameId)]
		public bool DisplayDefinitionTooltips
		{
			get { return myDisplayDefinitionTooltips; }
			set { myDisplayDefinitionTooltips = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for DisplayDefinitionTooltips
		/// </summary>
		public static bool CurrentDisplayDefinitionTooltips
		{
			get { return myCurrentDisplayDefinitionTooltips; }
		}

		/// <summary>
		/// Display Model Browser Delayed Activation option
		/// </summary>
		[DefaultValue(DelayActivateModelBrowserLabelEdits_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryModelBrowserBehaviorId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyDelayActivateModelBrowserLabelEditsDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyDelayActivateModelBrowserLabelEditsDisplayNameId)]
		public bool DelayActivateModelBrowserLabelEdits
		{
			get { return myDelayActivateModelBrowserLabelEdits; }
			set { myDelayActivateModelBrowserLabelEdits = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for DelayActivateModelBrowserLabelEdits
		/// </summary>
		public static bool CurrentDelayActivateModelBrowserLabelEdits
		{
			get { return myCurrentDelayActivateModelBrowserLabelEdits; }
		}
		#endregion // Accessor properties
		#region Custom dropdown for CustomVerbalizationSnippets option
		/// <summary>
		/// Provide context for the AvailableVerbalizationSnippetsPicker
		/// </summary>
		private sealed class CustomVerbalizationSnippetsEditor : AvailableVerbalizationSnippetsPicker<CustomVerbalizationSnippetsEditor>
		{
			/// <summary>
			/// Provide the root directory for verbalization snippets. Individual packages
			/// have directories below this.
			/// </summary>
			protected sealed override string VerbalizationDirectory
			{
				get
				{
					return ORMDesignerPackage.VerbalizationDirectory;
				}
			}
			/// <summary>
			/// Enumerate all registered snippets providers
			/// </summary>
			protected sealed override IEnumerable<IVerbalizationSnippetsProvider> SnippetsProviders
			{
				get
				{
					foreach (Type metaModelType in ORMDesignerPackage.ExtensionLoader.AvailableDomainModels)
					{
						object[] providers = metaModelType.GetCustomAttributes(typeof(VerbalizationSnippetsProviderAttribute), false);
						if (providers.Length != 0) // Single use non-inheritable attribute, there will only be one
						{
							IVerbalizationSnippetsProvider provider = ((VerbalizationSnippetsProviderAttribute)providers[0]).CreateSnippetsProvider(metaModelType);
							if (provider != null)
							{
								yield return provider;
							}
						}
					}
				}
			}
			/// <summary>
			/// Enumerate all registered snippets providers
			/// </summary>
			protected sealed override IEnumerable<IVerbalizationTargetProvider> TargetProviders
			{
				get
				{
					foreach (Type metaModelType in ORMDesignerPackage.ExtensionLoader.AvailableDomainModels)
					{
						object[] providers = metaModelType.GetCustomAttributes(typeof(VerbalizationTargetProviderAttribute), false);
						if (providers.Length != 0) // Single use non-inheritable attribute, there will only be one
						{
							IVerbalizationTargetProvider provider = ((VerbalizationTargetProviderAttribute)providers[0]).CreateTargetProvider(metaModelType);
							if (provider != null)
							{
								yield return provider;
							}
						}
					}
				}
			}
			/// <summary>
			/// Get the format string for the dropdown
			/// </summary>
			protected sealed override string LanguageFormatString
			{
				get
				{
					return ResourceStrings.OptionsPagePropertyCustomVerbalizationSnippetsDisplayValueLanguageFormat;
				}
			}
		}
		#endregion // Custom dropdown for CustomVerbalizationSnippets option
		#region Type converter for CustomVerbalizationSnippets option
		/// <summary>
		/// Stop live editing of the CustomVerbalizationSnippets property when shown
		/// in the OptionsPage dialog. Also display the 'Default' string if all defaults
		/// should be used, and the 'Custom' string if there are any custom settings.
		/// </summary>
		private sealed class CustomVerbalizationSnippetsConverter : StringConverter
		{
			public sealed override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				if (context != null && context.Instance is OptionsPage && sourceType == typeof(string))
				{
					return false;
				}
				return base.CanConvertFrom(context, sourceType);
			}
			public sealed override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				object retVal = base.ConvertTo(context, culture, value, destinationType);
				if (context != null && context.Instance is OptionsPage && destinationType == typeof(string))
				{
					retVal = string.IsNullOrEmpty((string)retVal) ? ResourceStrings.OptionsPagePropertyCustomVerbalizationSnippetsDisplayValueDefault : ResourceStrings.OptionsPagePropertyCustomVerbalizationSnippetsDisplayValueCustom;
				}
				return retVal;
			}
		}
		#endregion // Type converter for CustomVerbalizationSnippets option
	}
}
