using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.ORM.Shell;
using CategoryAttribute = System.ComponentModel.CategoryAttribute;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
namespace Neumont.Tools.ORM.Shell
{
	#region Shape enums
	/// <summary>
	/// Valid shapes for object types
	/// </summary>
	public enum ObjectTypeShape
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
	public enum ObjectifiedFactShape
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
	#endregion
	#region NotifyDocument Delegate
	/// <summary>
	/// Used as a callback delegate for the OptionsPage.NotifySettingsChange
	/// method. Allow other global notification methods to walk all open
	/// documents.
	/// </summary>
	/// <param name="docData">A running ORMDesignerDocData to modify</param>
	public delegate void NotifyDocument(ORMDesignerDocData docData);
	#endregion // NotifyDocument Delegate
	/// <summary>
	/// Options dialog for ORM designers
	/// see https://svn.neumont.edu/projects/orm2/wiki/HowToAddOptionPageOptions for adding options
	/// </summary>
	[Guid("B4ABD9FD-CE79-4B26-8D36-F345CB53B525")]
	public class OptionsPage : DialogPage
	{
		#region Localized PropertyDescriptor attribute classes
		private sealed class LocalizedCategoryAttribute : CategoryAttribute
		{
			public LocalizedCategoryAttribute(string category) : base(category)
			{
			}
			protected override string GetLocalizedString(string value)
			{
				return ResourceStrings.GetOptionsPageString(value);
			}
		}
		private sealed class LocalizedDescriptionAttribute : DescriptionAttribute
		{
			public LocalizedDescriptionAttribute(string description) : base(description)
			{
			}

			public override string Description
			{
				get { return ResourceStrings.GetOptionsPageString(base.Description); }
			}
		}
		private sealed class LocalizedDisplayNameAttribute : DisplayNameAttribute
		{
			public LocalizedDisplayNameAttribute(string displayName) : base(displayName)
			{
			}
			public override string DisplayName
			{
				get { return ResourceStrings.GetOptionsPageString(base.DisplayName); }
			}
		}
		#endregion // Localized PropertyDescriptor attribute classes
		#region Member variables
		// If more settings are added, add a corresponding check in the OnApply override below
		private const ObjectTypeShape ObjectTypeShape_Default = ObjectTypeShape.SoftRectangle;
		private static ObjectTypeShape myCurrentObjectTypeShape = ObjectTypeShape_Default;
		private ObjectTypeShape myObjectTypeShape = ObjectTypeShape_Default;

		private const ObjectifiedFactShape ObjectifiedFactShape_Default = ObjectifiedFactShape.SoftRectangle;
		private static ObjectifiedFactShape myCurrentObjectifiedFactShape = ObjectifiedFactShape_Default;
		private ObjectifiedFactShape myObjectifiedFactShape = ObjectifiedFactShape_Default;

		private const MandatoryDotPlacement MandatoryDotPlacement_Default = MandatoryDotPlacement.RoleBoxEnd;
		private static MandatoryDotPlacement myCurrentMandatoryDotPlacement = MandatoryDotPlacement_Default;
		private MandatoryDotPlacement myMandatoryDotPlacement = MandatoryDotPlacement_Default;

		private const RoleNameDisplay RoleNameDisplay_Default = RoleNameDisplay.On;
		private static RoleNameDisplay myCurrentRoleNameDisplay = RoleNameDisplay_Default;
		private RoleNameDisplay myRoleNameDisplay = RoleNameDisplay_Default;

		private const PortableDataType DefaultDataType_Default = PortableDataType.Unspecified;
		private static PortableDataType myCurrentDefaultDataType = DefaultDataType_Default;
		private PortableDataType myDefaultDataType = DefaultDataType_Default;
		#endregion // Member variables
		#region Base overrides
		/// <summary>
		/// Set the current values of the static properties
		/// to match the cached settings
		/// </summary>
		public override void LoadSettingsFromStorage()
		{
			base.LoadSettingsFromStorage();
			myCurrentObjectTypeShape = myObjectTypeShape;
			myCurrentObjectifiedFactShape = myObjectifiedFactShape;
			myCurrentMandatoryDotPlacement = myMandatoryDotPlacement;
			myCurrentRoleNameDisplay = myRoleNameDisplay;
			myCurrentDefaultDataType = myDefaultDataType;
		}
		/// <summary>
		/// Set local values for the current settings to determine later if the
		/// settings have changed in the OnApply method.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnActivate(CancelEventArgs e)
		{
			myObjectTypeShape = myCurrentObjectTypeShape;
			myObjectifiedFactShape = myCurrentObjectifiedFactShape;
			myMandatoryDotPlacement = myCurrentMandatoryDotPlacement;
			myRoleNameDisplay = myCurrentRoleNameDisplay;
			myDefaultDataType = myCurrentDefaultDataType;
		}

		/// <summary>
		/// Invalidate each loaded ORM diagram to force a redraw of the shapes
		/// </summary>
		/// <param name="e"></param>
		protected override void OnApply(DialogPage.PageApplyEventArgs e)
		{
			// Get out early if none of the settings have changed
			if (myCurrentMandatoryDotPlacement == myMandatoryDotPlacement &&
				myCurrentObjectifiedFactShape == myObjectifiedFactShape &&
				myCurrentObjectTypeShape == myObjectTypeShape &&
				myCurrentRoleNameDisplay == myRoleNameDisplay)
			{
				// Non-displayed setting, don't notify
				myCurrentDefaultDataType = myDefaultDataType;
				return;
			}

			// Set the new options
			myCurrentMandatoryDotPlacement = myMandatoryDotPlacement;
			myCurrentObjectifiedFactShape = myObjectifiedFactShape;
			myCurrentObjectTypeShape = myObjectTypeShape;
			myCurrentRoleNameDisplay = myRoleNameDisplay;
			myCurrentDefaultDataType = myDefaultDataType;

			// Walk all the documents and invalidate ORM diagrams if the options have changed
			NotifySettingsChange(Site, FixupDocument);
		}
		/// <summary>
		/// Modify the specified document layout when options are changed
		/// </summary>
		/// <param name="docData"></param>
		private void FixupDocument(ORMDesignerDocData docData)
		{
			IList diagrams = docData.Store.ElementDirectory.GetElements(ORMDiagram.MetaClassGuid);
			int diagramCount = diagrams.Count;
			for (int i = 0; i < diagramCount; ++i)
			{
				ORMDiagram diagram = (ORMDiagram)diagrams[i];
				using (Transaction t = diagram.Store.TransactionManager.BeginTransaction(ResourceStrings.OptionsPageChangeTransactionName))
				{
					Store store = diagram.Store;
					foreach (BinaryLinkShape link in store.ElementDirectory.GetElements(BinaryLinkShape.MetaClassGuid, true))
					{
						link.RipUp();
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
				diagram.Invalidate(true);
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
			// Walk all the documents and invalidate ORM diagrams if the options have changed
			IVsRunningDocumentTable docTable = (IVsRunningDocumentTable)serviceProvider.GetService(typeof(IVsRunningDocumentTable));
			IEnumRunningDocuments docIter;
			ErrorHandler.ThrowOnFailure(docTable.GetRunningDocumentsEnum(out docIter));
			int hrIter;
			uint[] currentDocs = new uint[1];
			uint fetched = 0;
			do
			{
				ErrorHandler.ThrowOnFailure(hrIter = docIter.Next(1, currentDocs, out fetched));
				if (hrIter == 0)
				{
					uint grfRDTFlags;
					uint dwReadLocks;
					uint dwEditLocks;
					string bstrMkDocument;
					IVsHierarchy pHier;
					uint itemId;
					IntPtr punkDocData = IntPtr.Zero;
					ErrorHandler.ThrowOnFailure(docTable.GetDocumentInfo(
						currentDocs[0],
						out grfRDTFlags,
						out dwReadLocks,
						out dwEditLocks,
						out bstrMkDocument,
						out pHier,
						out itemId,
						out punkDocData));
					try
					{
						ORMDesignerDocData docData = Marshal.GetObjectForIUnknown(punkDocData) as ORMDesignerDocData;
						if (docData != null)
						{
							changeCallback(docData);
						}
					}
					finally
					{
						if (punkDocData != IntPtr.Zero)
						{
							Marshal.Release(punkDocData);
						}
					}
				}
			} while (fetched != 0);
		}
		#endregion Change Notification Functions
		#region Accessor properties
		/// <summary>
		/// Object Type Shape option
		/// </summary>
		[DefaultValue(ObjectTypeShape_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryAppearanceId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyObjectTypeShapeDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyObjectTypeShapeDisplayNameId)]
		public ObjectTypeShape ObjectTypeShape
		{
			get { return myObjectTypeShape; }
			set { myObjectTypeShape = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for ObjectTypeShape
		/// </summary>
		public static ObjectTypeShape CurrentObjectTypeShape
		{
			get { return myCurrentObjectTypeShape; }
		}

		/// <summary>
		/// Objectified Shape option
		/// </summary>
		[DefaultValue(ObjectifiedFactShape_Default)]
		[LocalizedCategory(ResourceStrings.OptionsPageCategoryAppearanceId)]
		[LocalizedDescription(ResourceStrings.OptionsPagePropertyObjectifiedShapeDescriptionId)]
		[LocalizedDisplayName(ResourceStrings.OptionsPagePropertyObjectifiedShapeDisplayNameId)]
		public ObjectifiedFactShape ObjectifiedFactShape
		{
			get { return myObjectifiedFactShape; }
			set { myObjectifiedFactShape = value; }
		}

		/// <summary>
		/// Current VS session-wide setting for ObjectifiedFactShape
		/// </summary>
		public static ObjectifiedFactShape CurrentObjectifiedFactShape
		{
			get { return myCurrentObjectifiedFactShape;  }
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
		#endregion // Accessor properties
	}
}