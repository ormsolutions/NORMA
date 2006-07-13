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
//NOTICE: if you toggel HIDENEWMODELBROWSER on/off make sure to change it on in ORMCommandSet.cs as well
//#define HIDENEWMODELBROWSER
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.Win32;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.ORM.Shell.FactEditor;

using OleInterop = Microsoft.VisualStudio.OLE.Interop;
using DomainModel = Microsoft.VisualStudio.Modeling.DomainModel;

namespace Neumont.Tools.ORM.Shell
{
	#region Attributes
	/// <summary>
	/// Entry point for the ORMPackage package. An instance of this class is created by the VS
	/// shell whenever one of our services is required.
	/// </summary>
	[Guid("EFDDC549-1646-4451-8A51-E5A5E94D647C")]
	[CLSCompliant(false)]

	// IMPORTANT: Changes to anything in this region must also be made to "NORMAVSPackageRegistry.wxi" in the "Setup" project.

	// "ORM Designer" and "General" correspond and must be in sync with ORMDesignerUI.rc
	[ProvideOptionPage(typeof(OptionsPage), "ORM Designer", "General", 105, 106, false)]
	[ProvideEditorFactory(typeof(ORMDesignerEditorFactory), 108, TrustLevel=__VSEDITORTRUSTLEVEL.ETL_AlwaysTrusted)]
	// The following ProvideEditorExtension properties are for {General, Misc, Solution} Items, in that order
	[ProvideEditorExtension(typeof(ORMDesignerEditorFactory), ".orm", 0x32, NameResourceID=107, TemplateDir=@"C:\Program Files\Neumont\ORM Architect for Visual Studio\ORMProjectItems\", ProjectGuid="2150E333-8FDC-42A3-9474-1A3956D46DE8")]
	[ProvideEditorExtension(typeof(ORMDesignerEditorFactory), ".orm", 0x32, NameResourceID=107, TemplateDir=@"C:\Program Files\Neumont\ORM Architect for Visual Studio\ORMProjectItems\", ProjectGuid="A2FE74E1-B743-11d0-AE1A-00A0C90FFFC3")]
	[ProvideEditorExtension(typeof(ORMDesignerEditorFactory), ".orm", 0x32, NameResourceID=107, TemplateDir=@"C:\Program Files\Neumont\ORM Architect for Visual Studio\ORMProjectItems\", ProjectGuid="D1DCDB85-C5E8-11d2-BFCA-00C04F990235")]
	[ProvideEditorExtension(typeof(ORMDesignerEditorFactory), ".xml", 0x10)]
	[ProvideService(typeof(ORMDesignerFontsAndColors), ServiceName="OrmDesignerFontAndColorProvider")]
	[ProvideLanguageService(typeof(FactLanguageService), "ORM Fact Editor", 109, ShowCompletion=true, ShowSmartIndent=false, RequestStockColors=false, ShowHotURLs=false, DefaultToNonHotURLs=false, DefaultToInsertSpaces=false, ShowDropDownOptions=false, SingleCodeWindowOnly=true, EnableAdvancedMembersOption=false, SupportCopyPasteOfHTML=true)]
	[ProvideToolWindow(typeof(ORMDesignerPackage.FactEditorToolWindowShim), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMReferenceModeEditorToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMSamplePopulationToolWindow), Style = VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMReadingEditorToolWindow), Style = VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMVerbalizationToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMBrowserToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.SolutionExplorer)]
#if !HIDENEWMODELBROWSER
	[ProvideToolWindow(typeof(NewORMModelBrowser), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
#endif
	[ProvideToolWindow(typeof(ORMNotesToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindowVisibility(typeof(ORMDesignerPackage.FactEditorToolWindowShim), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMReferenceModeEditorToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMSamplePopulationToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMReadingEditorToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMVerbalizationToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMBrowserToolWindow), ORMDesignerEditorFactory.GuidString)]
#if !HIDENEWBROWSERWINDOW
	[ProvideToolWindowVisibility(typeof(NewORMModelBrowser), ORMDesignerEditorFactory.GuidString)]
#endif
	[ProvideToolWindowVisibility(typeof(ORMNotesToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideMenuResource(1000, 1)]
	[ProvideToolboxItems(1, true)]
	[ProvideToolboxFormat("Microsoft.VisualStudio.Modeling.ElementGroupPrototype")]
	[DefaultRegistryRoot(@"SOFTWARE\Microsoft\VisualStudio\8.0Exp")]
	[PackageRegistration(UseManagedResourcesOnly=false, RegisterUsing=RegistrationMethod.CodeBase, SatellitePath=@"C:\Program Files\Neumont\ORM Architect for Visual Studio\bin\")]
	[InstalledProductRegistration(true, null, null, null, LanguageIndependentName="Neumont ORM Architect")]
	[ProvideLoadKey("Standard", "1.0", "Neumont ORM Architect for Visual Studio", "Neumont University", 150)]
	#endregion // Attributes
	public sealed class ORMDesignerPackage : ModelingPackage, IVsInstalledProduct
	{
		#region FactEditorToolWindow Shim
		// HACK: This exists only so that the ProvideToolWindowAttribute can pull the GUID off of it.
		[Guid(FactGuidList.FactEditorToolWindowGuidString)]
		private static class FactEditorToolWindowShim { }
		#endregion // FactEditorToolWindow Shim

		#region Constants
		private const string REGISTRYROOT_PACKAGE = @"Neumont\ORM Architect";
		private const string REGISTRYROOT_EXTENSIONS = REGISTRYROOT_PACKAGE + @"\Extensions\";
		private const string REGISTRYVALUE_SETTINGSPATH = "SettingsPath";
		private const string REGISTRYVALUE_CONVERTERSDIR = "ConvertersDir";
		private const string REGISTRYVALUE_VERBALIZATIONDIR = "VerbalizationDir";
		#endregion

		#region Member variables
		/// <summary>
		/// The commands supported by this package
		/// </summary>
		private object myCommandSet;
		private IVsWindowFrame myFactEditorToolWindow;
		private ORMDesignerFontsAndColors myFontAndColorService;
		private ORMDesignerSettings myDesignerSettings;
		private string myVerbalizationDirectory;
		private static ORMDesignerPackage mySingleton;
		#endregion
		#region Construction/destruction
		/// <summary>
		/// Class constructor.
		/// </summary>
		public ORMDesignerPackage()
		{
			Debug.Assert(mySingleton == null); // Should only be loaded once per IDE session
			mySingleton = this;
			// System.ComponentModel.TypeDescriptor is failing to find our TypeDescriptorProvider classes.
			// This fixes it.
			// UNDONE: This is first getting called with the OptionsPage on the call stack
			AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblyOnce;
		}
		private static Assembly ResolveAssemblyOnce(object sender, ResolveEventArgs args)
		{
			Assembly assembly = typeof(ORMDesignerPackage).Assembly;
			if (string.Equals(args.Name, assembly.FullName, StringComparison.Ordinal))
			{
				AppDomain.CurrentDomain.AssemblyResolve -= ResolveAssemblyOnce;
				return assembly;
			}
			return null;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the singleton command set create for this package.
		/// </summary>
		public static object CommandSet
		{
			get
			{
				ORMDesignerPackage package = mySingleton;
				return (package != null) ? package.myCommandSet : null;
			}
		}
		/// <summary>
		/// Gets the singleton font and color service for this package
		/// </summary>
		public static ORMDesignerFontsAndColors FontAndColorService
		{
			get
			{
				ORMDesignerPackage package = mySingleton;
				return (package != null) ? package.myFontAndColorService : null;
			}
		}
		/// <summary>
		/// Get the designer settings for this package
		/// </summary>
		public static ORMDesignerSettings DesignerSettings
		{
			get
			{
				ORMDesignerPackage package = mySingleton;
				if (package != null)
				{
					ORMDesignerSettings retVal = package.myDesignerSettings;
					if (retVal == null)
					{
						RegistryKey applicationRegistryRoot = null;
						RegistryKey normaRegistryRoot = null;
						try
						{
							applicationRegistryRoot = package.ApplicationRegistryRoot;
							normaRegistryRoot = applicationRegistryRoot.OpenSubKey(REGISTRYROOT_PACKAGE, RegistryKeyPermissionCheck.ReadSubTree);
							string settingsPath = (string)normaRegistryRoot.GetValue(REGISTRYVALUE_SETTINGSPATH, String.Empty);
							string xmlConvertersDir = (string)normaRegistryRoot.GetValue(REGISTRYVALUE_CONVERTERSDIR, String.Empty);
							retVal = new ORMDesignerSettings(package, settingsPath, xmlConvertersDir);
							package.myDesignerSettings = retVal;
						}
						finally
						{
							if (applicationRegistryRoot != null)
							{
								applicationRegistryRoot.Close();
							}
							if (normaRegistryRoot != null)
							{
								normaRegistryRoot.Close();
							}
						}
					}
					return retVal;
				}
				return null;
			}
		}
		/// <summary>
		/// Get the designer settings for this package
		/// </summary>
		public static string VerbalizationDirectory
		{
			get
			{
				ORMDesignerPackage package = mySingleton;
				if (package != null)
				{
					string retVal = package.myVerbalizationDirectory;
					if (retVal == null)
					{
						RegistryKey applicationRegistryRoot = null;
						RegistryKey normaRegistryRoot = null;
						try
						{
							applicationRegistryRoot = package.ApplicationRegistryRoot;
							normaRegistryRoot = applicationRegistryRoot.OpenSubKey(REGISTRYROOT_PACKAGE, RegistryKeyPermissionCheck.ReadSubTree);
							string settingsPath = (string)normaRegistryRoot.GetValue(REGISTRYVALUE_SETTINGSPATH, String.Empty);
							retVal = (string)normaRegistryRoot.GetValue(REGISTRYVALUE_VERBALIZATIONDIR, String.Empty);
							package.myVerbalizationDirectory = retVal;
						}
						finally
						{
							if (applicationRegistryRoot != null)
							{
								applicationRegistryRoot.Close();
							}
							if (normaRegistryRoot != null)
							{
								normaRegistryRoot.Close();
							}
						}
					}
					return retVal;
				}
				return null;
			}
		}
		/// <summary>
		/// For use by unit tests. Also used by ModelElementLocator.
		/// Private to discourage use outside of unit testing,
		/// may only be accessed through reflection.
		/// </summary>
		private static ORMDesignerPackage Singleton
		{
			get
			{
				return mySingleton;
			}
		}
		#endregion // Properties
		#region Base overrides
		/// <summary>
		/// This is called by the package base class when our package is loaded. When devenv is run
		/// with the "/setup" command line switch it is not able to do a lot of the normal things,
		/// such as creating output windows and tool windows. Under normal circumstances our package
		/// isn't loaded when run with this switch. However, our package will be loaded when items 
		/// are added to the toolbox, even when run with "/setup". To be safe we'll check for "setup"
		/// and we don't do anything interesting in MgdSetSite if we find it. 
		/// </summary>
		protected sealed override void Initialize()
		{
			base.Initialize();

			// register the class designer editor factory
			RegisterEditorFactory(new ORMDesignerEditorFactory(this));

			if (!SetupMode)
			{
				IServiceContainer service = (IServiceContainer)this;
				myFontAndColorService = new ORMDesignerFontsAndColors(this);
				service.AddService(typeof(ORMDesignerFontsAndColors), myFontAndColorService, true);
				service.AddService(typeof(FactLanguageService), new FactLanguageService(this), true);
				
				// setup commands
				myCommandSet = ORMDesignerDocView.CreateCommandSet(this);

				// Create tool windows
#if !HIDENEWMODELBROWSER
				AddToolWindow(typeof(NewORMModelBrowser));
#endif
				AddToolWindow(typeof(ORMBrowserToolWindow));
				AddToolWindow(typeof(ORMReadingEditorToolWindow));
				AddToolWindow(typeof(ORMReferenceModeEditorToolWindow));
				AddToolWindow(typeof(ORMSamplePopulationToolWindow));
				AddToolWindow(typeof(ORMVerbalizationToolWindow));
				AddToolWindow(typeof(ORMNotesToolWindow));
				EnsureFactEditorToolWindow();
				
				// Make sure our options are loaded from the registry
				GetDialogPage(typeof(OptionsPage));

				base.SetupDynamicToolbox();
			}

		}
		/// <summary>
		/// This is called by the package base class when our package gets unloaded.
		/// </summary>
		protected sealed override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IServiceContainer service = (IServiceContainer)this;
				service.RemoveService(typeof(ORMDesignerFontsAndColors), true);

				if (myFactEditorToolWindow != null)
				{
					myFactEditorToolWindow.CloseFrame(0);
				}

				service.RemoveService(typeof(FactLanguageService), true);
				// dispose of any private objects here
			}
			base.Dispose(disposing);
		}
		/// <summary>
		/// Retrieve toolbox items. Called during devenv /setup or
		/// toolbox refresh. Uses standard prototype settings (mostly
		/// created in ORMDiagram.InitializeToolboxItem) and adds additional
		/// filter strings as required.
		/// </summary>
		/// <seealso cref="ModelingPackage.CreateToolboxItems"/>
		protected sealed override IList<ModelingToolboxItem> CreateToolboxItems()
		{
			IList<ModelingToolboxItem> items;
			ORMCoreModel.InitializingToolboxItems = false;
			try
			{
				items = new ORMShapeModelToolboxHelper(this).CreateToolboxItems();
			}
			finally
			{
				ORMCoreModel.InitializingToolboxItems = true;
			}

			// Build up a dictionary of items so we can add filter strings. This is
			// much easier than trying to maintain all of the filter strings at the ims level,
			// which would require elements with different filter string sets to be placed on different
			// ims elements.
			int itemCount = items.Count;
			Dictionary<string, int> itemIndexDictionary = new Dictionary<string, int>(itemCount);
			for (int i = 0; i < itemCount; i++)
			{
				itemIndexDictionary[items[i].Id] = i;
			}

			ToolboxItemFilterAttribute attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramInternalUniquenessConstraintFilterString, ToolboxItemFilterType.Allow);
			AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxInternalUniquenessConstraintItemId, attribute);

			attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramConnectInternalUniquenessConstraintFilterString, ToolboxItemFilterType.Allow);
			AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxInternalUniquenessConstraintConnectorItemId, attribute);

			attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramConnectExternalConstraintFilterString, ToolboxItemFilterType.Allow);
			AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxExternalConstraintConnectorItemId, attribute);

			attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramConnectRoleFilterString, ToolboxItemFilterType.Allow);
			AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxRoleConnectorItemId, attribute);

			attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramCreateSubtypeFilterString, ToolboxItemFilterType.Allow);
			AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxSubtypeConnectorItemId, attribute);

			attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramExternalConstraintFilterString, ToolboxItemFilterType.Allow);
			string[] itemIds = {
				ResourceStrings.ToolboxEqualityConstraintItemId,
				ResourceStrings.ToolboxExclusionConstraintItemId,
				ResourceStrings.ToolboxExclusiveOrConstraintItemId,
				ResourceStrings.ToolboxExternalUniquenessConstraintItemId,
				ResourceStrings.ToolboxInclusiveOrConstraintItemId,
				ResourceStrings.ToolboxRingConstraintItemId,
				ResourceStrings.ToolboxSubsetConstraintItemId,
				ResourceStrings.ToolboxFrequencyConstraintItemId
			};
			int idCount = itemIds.Length;
			for (int i = 0; i < idCount; ++i)
			{
				AddFilterAttribute(items, itemIndexDictionary, itemIds[i], attribute);
			}
			return items;
		}
		/// <summary>
		/// Add a filter string to the specified ModelingToolboxItem
		/// </summary>
		/// <param name="items">An array of existing items</param>
		/// <param name="itemIndexDictionary">A dictionary mapping from the item name
		/// to an index in the items array</param>
		/// <param name="itemId">The name of the item to modify</param>
		/// <param name="attribute">The filter property to add</param>
		private static void AddFilterAttribute(IList<ModelingToolboxItem> items, Dictionary<string, int> itemIndexDictionary, string itemId, ToolboxItemFilterAttribute attribute)
		{
			int itemIndex;
			if (itemIndexDictionary.TryGetValue(itemId, out itemIndex))
			{
				ModelingToolboxItem itemBase = items[itemIndex];
				System.Collections.ICollection baseFilters = itemBase.Filter;
				int baseFilterCount = baseFilters.Count;
				ToolboxItemFilterAttribute[] newFilters = new ToolboxItemFilterAttribute[baseFilterCount + 1];
				baseFilters.CopyTo(newFilters, 0);
				newFilters[baseFilterCount] = attribute;
				itemBase.Filter = newFilters;
				// UNDONE: 2006-06 DSL Tools port: Why was this recreating the ModelingToolboxItem rather than just setting Filter?
				/*items[itemIndex] = new ModelingToolboxItem(
					itemBase.Id,
					itemBase.Position,
					itemBase.DisplayName ?? String.Empty,
					itemBase.Bitmap,
					itemBase.TabNameId,
					itemBase.TabName ?? String.Empty,
					itemBase.ContextSensitiveHelpKeyword ?? String.Empty,
					itemBase.Description,
					itemBase.Prototype,
					newFilters);*/
			}
		}
		#endregion // Base overrides
		#region FactEditorToolWindow Creation
		private IVsWindowFrame EnsureFactEditorToolWindow()
		{
			IVsWindowFrame frame = myFactEditorToolWindow;
			if (frame == null)
			{
				myFactEditorToolWindow = frame = AddFactEditorToolWindow();
			}
			return frame;
		}
		private IVsWindowFrame AddFactEditorToolWindow()
		{
			ILocalRegistry3 locReg = (ILocalRegistry3)this.GetService(typeof(ILocalRegistry));
			IntPtr pBuf = IntPtr.Zero;
			Guid iid = typeof(IVsTextLines).GUID;
			ErrorHandler.ThrowOnFailure(locReg.CreateInstance(
				typeof(VsTextBufferClass).GUID,
				null,
				ref iid,
				(uint)OleInterop.CLSCTX.CLSCTX_INPROC_SERVER,
				out pBuf));

			IVsTextLines lines = null;
			OleInterop.IObjectWithSite objectWithSite = null;
			try
			{
				// Get an object to tie to the IDE
				lines = (IVsTextLines)Marshal.GetObjectForIUnknown(pBuf);
				objectWithSite = lines as OleInterop.IObjectWithSite;
				objectWithSite.SetSite(this);
			}
			finally
			{
				if (pBuf != IntPtr.Zero)
				{
					Marshal.Release(pBuf);
				}
			}

			// assign our language service to the buffer
			Guid langService = typeof(FactLanguageService).GUID;
			ErrorHandler.ThrowOnFailure(lines.SetLanguageServiceID(ref langService));

			// Create a std code view (text)
			IntPtr srpCodeWin = IntPtr.Zero;
			iid = typeof(IVsCodeWindow).GUID;

			// create code view (does CoCreateInstance if not in shell's registry)
			ErrorHandler.ThrowOnFailure(locReg.CreateInstance(
				typeof(VsCodeWindowClass).GUID,
				null,
				ref iid,
				(uint)OleInterop.CLSCTX.CLSCTX_INPROC_SERVER,
				out srpCodeWin));

			IVsCodeWindow codeWindow = null;
			try
			{
				// Get an object to tie to the IDE
				codeWindow = (IVsCodeWindow)Marshal.GetObjectForIUnknown(srpCodeWin);
			}
			finally
			{
				if (srpCodeWin != IntPtr.Zero)
				{
					Marshal.Release(srpCodeWin);
				}
			}

			ErrorHandler.ThrowOnFailure(codeWindow.SetBuffer(lines));

			IVsWindowFrame windowFrame;
			IVsUIShell shell = (IVsUIShell)GetService(typeof(IVsUIShell));
			Guid emptyGuid = new Guid();
			Guid factEditorToolWindowGuid = FactGuidList.FactEditorToolWindowGuid;
			// CreateToolWindow ARGS
			// 0 - toolwindow.flags (initnew)
			// 1 - 0 (the tool window ID)
			// 2- IVsWindowPane
			// 3- guid null
			// 4- persistent slot (same nr as the guid attr on tool window class)
			// 5- guid null
			// 6- ole service provider (null)
			// 7- tool window.windowTitle
			// 8- int[] for position (empty array)
			// 9- out IVsWindowFrame
			ErrorHandler.ThrowOnFailure(shell.CreateToolWindow(
				(uint)__VSCREATETOOLWIN.CTW_fInitNew, // tool window flags, default to init new
				0,
				(IVsWindowPane)codeWindow,
				ref emptyGuid,
				ref factEditorToolWindowGuid,
				ref emptyGuid,
				null,
				ResourceStrings.FactEditorToolWindowCaption,
				null,
				out windowFrame));

			return windowFrame;
		}
		#endregion FactEditorToolWindow Creation
		#region IVsInstalledProduct Members

		[Obsolete("Visual Studio 2005 no longer calls this method.", true)]
		int IVsInstalledProduct.IdBmpSplash(out uint pIdBmp)
		{
			pIdBmp = (uint)UIntPtr.Zero;
			return VSConstants.E_NOTIMPL;
		}

		int IVsInstalledProduct.IdIcoLogoForAboutbox(out uint pIdIco)
		{
			// UNDONE: replace hard-coded ID for AboutBox icon
			pIdIco = 110;
			return VSConstants.S_OK;
		}

		int IVsInstalledProduct.OfficialName(out string pbstrName)
		{
			pbstrName = ResourceStrings.PackageOfficialName;
			return VSConstants.S_OK;
		}

		int IVsInstalledProduct.ProductDetails(out string pbstrProductDetails)
		{
			pbstrProductDetails = ResourceStrings.PackageProductDetails;
			return VSConstants.S_OK;
		}

		int IVsInstalledProduct.ProductID(out string pbstrPID)
		{
			pbstrPID = null;
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(false);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				AssemblyInformationalVersionAttribute informationalVersion = customAttributes[i] as AssemblyInformationalVersionAttribute;
				if (informationalVersion != null)
				{
					pbstrPID = informationalVersion.InformationalVersion;
					break;
				}
			}
			return VSConstants.S_OK;
		}

		#endregion
		#region Tool Window properties
		/// <summary>
		/// Browser tool window.
		/// </summary>
		public static ORMBrowserToolWindow BrowserWindow
		{
			get
			{
				return (ORMBrowserToolWindow)mySingleton.GetToolWindow(typeof(ORMBrowserToolWindow), true);
			}
		}
		/// <summary>
		/// New ORMBrowser tool window
		/// </summary>
		public static NewORMModelBrowser NewORMModelBrowserWindow
		{
			get
			{
				return (NewORMModelBrowser)mySingleton.GetToolWindow(typeof(NewORMModelBrowser), true);
			}
		}

		/// <summary>
		/// Reading editor tool window.
		/// </summary>
		public static ORMReadingEditorToolWindow ReadingEditorWindow
		{
			get
			{
				return (ORMReadingEditorToolWindow)mySingleton.GetToolWindow(typeof(ORMReadingEditorToolWindow), true);
			}
		}

		/// <summary>
		/// Notes tool window.
		/// </summary>
		public static ORMNotesToolWindow NotesWindow
		{
			get
			{
				return (ORMNotesToolWindow)mySingleton.GetToolWindow(typeof(ORMNotesToolWindow), true);
			}
		}
		/// <summary>
		/// The reference mode editor window.
		/// </summary>
		public static ORMReferenceModeEditorToolWindow ReferenceModeEditorWindow
		{
			get
			{
				return (ORMReferenceModeEditorToolWindow)mySingleton.GetToolWindow(typeof(ORMReferenceModeEditorToolWindow), true);
			}
		}
		/// <summary>
		/// The sample population editor window.
		/// </summary>
		public static ORMSamplePopulationToolWindow SamplePopulationEditorWindow
		{
			get
			{
				return (ORMSamplePopulationToolWindow)mySingleton.GetToolWindow(typeof(ORMSamplePopulationToolWindow), true);
			}
		}

		/// <summary>
		/// Fact editor tool window.
		/// </summary>
		public static IVsWindowFrame FactEditorWindow
		{
			get
			{
				return mySingleton.EnsureFactEditorToolWindow();
			}
		}
		private static bool myRetrievingVerbalizationWindow;
		/// <summary>
		/// Verbalization output tool window.
		/// </summary>
		public static ORMVerbalizationToolWindow VerbalizationWindow
		{
			get
			{
				if (myRetrievingVerbalizationWindow)
				{
					// Handles error condition where the verbalization window retrieves
					// itself during initialization. Occurs attempting to get the initial
					// state of the positive/negative verbalization buttons.
					return null;
				}
				try
				{
					myRetrievingVerbalizationWindow = true;
					return (ORMVerbalizationToolWindow)mySingleton.GetToolWindow(typeof(ORMVerbalizationToolWindow), true);
				}
				finally
				{
					myRetrievingVerbalizationWindow = false;
				}
			}
		}
		/// <summary>
		/// Called if the verbalization window settings change in the options dialog.
		/// Does nothing if the window has not been created.
		/// </summary>
		public static void VerbalizationWindowSettingsChanged()
		{
			if (mySingleton != null)
			{
				ORMVerbalizationToolWindow window = (ORMVerbalizationToolWindow)mySingleton.GetToolWindow(typeof(ORMVerbalizationToolWindow), false);
				if (window != null)
				{
					window.SettingsChanged();
				}
			}
		}
		#endregion

		#region Extension DomainModels
		/// <summary>
		/// Retrieves the <see cref="DomainModel"/> for a specific extension namespace.
		/// </summary>
		/// <remarks>If a <see cref="DomainModel"/> cannot be found for a namespace, <see langword="null"/> is returned.</remarks>
		public static Type GetExtensionSubStore(string extensionNamespace)
		{
			RegistryKey applicationRegistryRoot = null;
			RegistryKey userRegistryRoot = null;
			try
			{
				applicationRegistryRoot = mySingleton.ApplicationRegistryRoot;

				// Try to get the extension from application (all users), otherwise get it from per-user
				return
					LoadExtension(extensionNamespace, applicationRegistryRoot)
					??
					LoadExtension(extensionNamespace, userRegistryRoot = mySingleton.UserRegistryRoot);
			}
			finally
			{
				if (applicationRegistryRoot != null)
				{
					applicationRegistryRoot.Close();
				}
				if (userRegistryRoot != null)
				{
					userRegistryRoot.Close();
				}
			}
		}
		/// <summary>
		/// Adds the extension <see cref="DomainModel"/> <see cref="Type"/>s from the <paramref name="extensionPath"/>
		/// under <paramref name="hkeyBase"/> to the <see cref="ICollection{Type}"/> <paramref name="extensionSubStoreTypes"/>.
		/// </summary>
		private static Type LoadExtension(string extensionNamespace, RegistryKey hkeyBase)
		{
			using (RegistryKey hkeyExtension = hkeyBase.OpenSubKey(REGISTRYROOT_EXTENSIONS + extensionNamespace, RegistryKeyPermissionCheck.ReadSubTree))
			{
				if (hkeyExtension != null)
				{
				// Execution is returned to this point if the user elects to retry a failed extension load
				L_Retry:
					try
					{
						string extensionTypeString = hkeyExtension.GetValue("Class") as string;
						if (string.IsNullOrEmpty(extensionTypeString))
						{
							// If we don't have an extension type name, just go on to the next registered extension
							return null;
						}

						AssemblyName extensionAssemblyName;
						string extensionAssemblyNameString = hkeyExtension.GetValue("Assembly") as string;
						if (!string.IsNullOrEmpty(extensionAssemblyNameString))
						{
							extensionAssemblyName = new AssemblyName(extensionAssemblyNameString);
						}
						else
						{
							extensionAssemblyName = new AssemblyName();
						}
						extensionAssemblyName.CodeBase = hkeyExtension.GetValue("CodeBase") as string;

						Type extensionType = Assembly.Load(extensionAssemblyName).GetType(extensionTypeString, true, false);

						if (extensionType.IsSubclassOf(typeof(DomainModel)))
						{
							return extensionType;
						}
					}
					catch (Exception ex)
					{
						// An Exception can occur for a number of reasons, such as the user not having the correct
						// registry or file permissions, or the referenced assmebly or file not existing or being corrupt

						string message = string.Format(System.Globalization.CultureInfo.CurrentCulture, ResourceStrings.ExtensionLoadFailureMessage, Environment.NewLine, extensionNamespace, ex);
						int result = VsShellUtilities.ShowMessageBox(Singleton, message, ResourceStrings.ExtensionLoadFailureTitle, OLEMSGICON.OLEMSGICON_WARNING, OLEMSGBUTTON.OLEMSGBUTTON_ABORTRETRYIGNORE, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_THIRD);
						if (result == (int)DialogResult.Retry)
						{
							goto L_Retry;
						}
						else if (result != (int)DialogResult.Ignore)
						{
							// If a debugger is already attached, Launch() has no effect, so we can always safely call it
							Debugger.Launch();
							Debugger.Break();
							throw;
						}
					}
				}
				return null;
			}
		}
		/// <summary>
		/// Enumerate all models available to the ORM designer
		/// </summary>
		/// <returns>IEnumerable&lt;Type&gt;</returns>
		public static IEnumerable<Type> GetAvailableDomainModels()
		{
			yield return typeof(Neumont.Tools.ORM.ObjectModel.ORMCoreModel);
			yield return typeof(Neumont.Tools.ORM.ShapeModel.ORMShapeModel);
			foreach (ORMExtensionType extension in GetAvailableCustomExtensions())
			{
				yield return extension.Type;
			}
		}
		/// <summary>
		/// This method cycles through the registered Custom Extensions.
		/// It then returns an IList of ORMExtensionType. containing all the Types of the Custome Extensions.
		/// </summary>
		/// <returns>An IList of registered ORMExtensionTypes.</returns>
		public static IList<ORMExtensionType> GetAvailableCustomExtensions()
		{
			List<ORMExtensionType> extensions = new List<ORMExtensionType>();

			// Here we check for CustomExtensions in the ApplicationRegistryRoot.
			using (RegistryKey applicationRegistryRoot = mySingleton.ApplicationRegistryRoot)
			{
				using (RegistryKey hkeyExtensions = applicationRegistryRoot.OpenSubKey(REGISTRYROOT_EXTENSIONS, RegistryKeyPermissionCheck.ReadSubTree))
				{
					if (hkeyExtensions != null)
					{
						string[] extensionNamespaces = hkeyExtensions.GetSubKeyNames();
						foreach (String extensionNamespace in extensionNamespaces)
						{
							extensions.Add(new ORMExtensionType(extensionNamespace, LoadExtension(extensionNamespace, applicationRegistryRoot)));
						}
					}
				}
			}

			// Here we check for CustomExtensions in the UserRegistryRoot.
			using (RegistryKey userRegistryRoot = mySingleton.UserRegistryRoot)
			{
				using (RegistryKey hkeyExtensions = userRegistryRoot.OpenSubKey(REGISTRYROOT_EXTENSIONS, RegistryKeyPermissionCheck.ReadSubTree))
				{
					if (hkeyExtensions != null)
					{
						string[] extensionNamespaces = hkeyExtensions.GetSubKeyNames();
						foreach (String extensionNamespace in extensionNamespaces)
						{
							extensions.Add(new ORMExtensionType(extensionNamespace, LoadExtension(extensionNamespace, userRegistryRoot)));
						}
					}
				}
			}
			return extensions;
		}
		#endregion // Extension DomainModels
	}
}
