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

	// "ORM Designer" and "General" correspond and must be in sync with VSPackage.resx
	[ProvideOptionPage(typeof(OptionsPage), "ORM Designer", "General", PackageResources.Id.OptionsCategory, PackageResources.Id.OptionsGeneral, false)]
	[ProvideEditorFactory(typeof(ORMDesignerEditorFactory), PackageResources.Id.EditorName, TrustLevel=__VSEDITORTRUSTLEVEL.ETL_AlwaysTrusted)]
	[ProvideEditorLogicalView(typeof(ORMDesignerEditorFactory), LogicalViewID.Designer)]
	[ProvideEditorExtension(typeof(ORMDesignerEditorFactory), ".orm", 0x32)]
	[ProvideEditorExtension(typeof(ORMDesignerEditorFactory), ".xml", 0x10)]
	[ProvideService(typeof(ORMDesignerFontsAndColors), ServiceName="OrmDesignerFontAndColorProvider")]
	[ProvideLanguageService(typeof(FactLanguageService), "ORM Fact Editor", PackageResources.Id.FactEditorName, ShowCompletion=true, ShowSmartIndent=false, RequestStockColors=false, ShowHotURLs=false, DefaultToNonHotURLs=false, DefaultToInsertSpaces=false, ShowDropDownOptions=false, SingleCodeWindowOnly=true, EnableAdvancedMembersOption=false, SupportCopyPasteOfHTML=true)]
	[ProvideToolWindow(typeof(ORMDesignerPackage.FactEditorToolWindowShim), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMReferenceModeEditorToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMSamplePopulationToolWindow), Style = VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMReadingEditorToolWindow), Style = VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMVerbalizationToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMModelBrowserToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.SolutionExplorer)]
	[ProvideToolWindow(typeof(ORMNotesToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMContextWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindowVisibility(typeof(ORMDesignerPackage.FactEditorToolWindowShim), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMReferenceModeEditorToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMSamplePopulationToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMReadingEditorToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMVerbalizationToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMModelBrowserToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMNotesToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMContextWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideMenuResource(PackageResources.Id.CTMenu, 1)]
	[ProvideToolboxItems(1, true)]
	[ProvideToolboxFormat("Microsoft.VisualStudio.Modeling.ElementGroupPrototype")]
	[PackageRegistration(UseManagedResourcesOnly=true, RegisterUsing=RegistrationMethod.Assembly)]
	[InstalledProductRegistration(true, null, null, null, LanguageIndependentName="Neumont ORM Architect")]
	[ProvideLoadKey("Standard", "1.0", "Neumont ORM Architect for Visual Studio", "Neumont University", PackageResources.Id.PackageLoadKey)]
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
		private const string REGISTRYVALUE_TOOLBOXREVISION = "ToolboxRevision";
		#endregion

		#region Member variables
		/// <summary>
		/// The commands supported by this package
		/// </summary>
		private CommandSet myCommandSet;
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
		}
		#endregion

		#region Assembly Resolve Handler
		private static readonly Dictionary<string, Assembly> KnownAssemblies = GetKnownAssemblies();
		private static Dictionary<string, Assembly> GetKnownAssemblies()
		{
			Dictionary<string, Assembly> knownAssemblies = new Dictionary<string, Assembly>(1, StringComparer.Ordinal);
			Assembly packageAssembly = typeof(ORMDesignerPackage).Assembly;
			knownAssemblies[packageAssembly.FullName] = packageAssembly;
			// SECURITY: APTCA: If we ever allow partially-trusted callers, this will need to be altered so that
			// they are not added to the assembly probing path.
			AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
			{
				// This supports retrieving types from our assembly and our extension assemblies,
				// even if they aren't in the normal assembly probing path.
				Assembly knownAssembly;
				ORMDesignerPackage.KnownAssemblies.TryGetValue(e.Name, out knownAssembly);
				return knownAssembly;
			};
			return knownAssemblies;
		}
		#endregion // Assembly Resolve Handler

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
				((IServiceContainer)this).AddService(typeof(ORMDesignerFontsAndColors), myFontAndColorService = new ORMDesignerFontsAndColors(this), true);
				((IServiceContainer)this).AddService(typeof(FactLanguageService), new FactLanguageService(this), true);

				// setup commands
				(myCommandSet = ORMDesignerDocView.CreateCommandSet(this)).Initialize();

				// Create tool windows
				AddToolWindow(typeof(ORMModelBrowserToolWindow));
				AddToolWindow(typeof(ORMReadingEditorToolWindow));
				AddToolWindow(typeof(ORMReferenceModeEditorToolWindow));
				AddToolWindow(typeof(ORMSamplePopulationToolWindow));
				AddToolWindow(typeof(ORMVerbalizationToolWindow));
				AddToolWindow(typeof(ORMNotesToolWindow));
				AddToolWindow(typeof(ORMContextWindow));
				EnsureFactEditorToolWindow();

				// Make sure our options are loaded from the registry
				GetDialogPage(typeof(OptionsPage));

				InitializeToolbox();
			}

		}
		/// <summary>
		/// Checks if the toolbox needs to be initialized, and if so, calls <see cref="ModelingPackage.SetupDynamicToolbox"/>.
		/// </summary>
		private void InitializeToolbox()
		{
			IVsAppCommandLine vsAppCommandLine = (IVsAppCommandLine)base.GetService(typeof(IVsAppCommandLine));
			int present;
			string optionValue;
			if (vsAppCommandLine == null || ErrorHandler.Failed(vsAppCommandLine.GetOption("RootSuffix", out present, out optionValue)) || !Convert.ToBoolean(present))
			{
				// If we can't obtain the IVsAppCommandLine service, or our call to it fails, or no root suffix was specified, try looking at our registry root.
				IVsShell vsShell = (IVsShell)base.GetService(typeof(SVsShell));
				object registryRootObject;
				if (vsShell == null || ErrorHandler.Failed(vsShell.GetProperty((int)__VSSPROPID.VSSPROPID_VirtualRegistryRoot, out registryRootObject)) || ((string)registryRootObject).EndsWith("EXP", StringComparison.OrdinalIgnoreCase))
				{
					// If we can't get the registry root, or if it says we are running in the experimental hive, call SetupDynamicToolbox.
					base.SetupDynamicToolbox();
				}
				else
				{
					// Get the current revision number, defaulting to 0 if something goes wrong.
					int currentRevision = 0;
					object[] customAttributes = typeof(ORMDesignerPackage).Assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
					for (int i = 0; i < customAttributes.Length; i++)
					{
						AssemblyFileVersionAttribute fileVersion = customAttributes[i] as AssemblyFileVersionAttribute;
						if (fileVersion != null)
						{
							currentRevision = new Version(fileVersion.Version).Revision;
							break;
						}
					}

					RegistryKey userRegistryRoot = null;
					RegistryKey packageRegistryRoot = null;
					try
					{
						userRegistryRoot = this.UserRegistryRoot;
						packageRegistryRoot = userRegistryRoot.OpenSubKey(REGISTRYROOT_PACKAGE, RegistryKeyPermissionCheck.ReadSubTree);

						if (packageRegistryRoot != null)
						{
							// Lookup the recorded toolbox revision, and check whether it matches the current revision.
							int? toolboxRevision = packageRegistryRoot.GetValue(REGISTRYVALUE_TOOLBOXREVISION, null) as int?;
							if (toolboxRevision.HasValue && toolboxRevision.GetValueOrDefault() == currentRevision)
							{
								// If the toolbox is already from this exact revision, don't do anything.
								return;
							}
							packageRegistryRoot.Close();
						}

						// Since the toolbox has either not been set up before, or is from an older or newer revision, call SetupDynamicToolbox.
						// This might not be necessary if it is from a newer revision, but we do it anyway to be safe.
						base.SetupDynamicToolbox();

						try
						{
							// If a exception were to occur right here, Close() could get called twice for packageRegistryRoot, but that wouldn't actually hurt anything.
							packageRegistryRoot = userRegistryRoot.CreateSubKey(REGISTRYROOT_PACKAGE, RegistryKeyPermissionCheck.ReadWriteSubTree);

							// Record the current revision in the registry.
							packageRegistryRoot.SetValue(REGISTRYVALUE_TOOLBOXREVISION, currentRevision, RegistryValueKind.DWord);
						}
						catch (System.Security.SecurityException ex)
						{
							Debug.Fail("A security exception occurred while trying to write the current toolbox format revision number to the user registry. " +
								"You can safely continue execution of the program.", ex.ToString());
							// Swallow the exception, since it won't actually cause a problem. The next time the package is loaded, we'll just initialize the toolbox again.
						}
					}
					finally
					{
						if (userRegistryRoot != null)
						{
							if (packageRegistryRoot != null)
							{
								packageRegistryRoot.Close();
							}
							userRegistryRoot.Close();
						}
					}
				}
			}
			else if (!string.IsNullOrEmpty(optionValue))
			{
				// If any non-empty root suffix was specified as a command line parameter, call SetupDynamicToolbox.
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
			ORMCoreDomainModel.InitializingToolboxItems = true;
			try
			{
				items = new ORMShapeToolboxHelper(this).CreateToolboxItems();
			}
			finally
			{
				ORMCoreDomainModel.InitializingToolboxItems = false;
			}

			// Build up a dictionary of items so we can add filter strings. This is
			// much easier than trying to maintain all of the filter strings at the ims level,
			// which would require elements with different filter string sets to be placed on different
			// ims elements.
			Dictionary<string, int> itemIndexDictionary = new Dictionary<string, int>(items.Count);
			for (int i = 0; i < items.Count; i++)
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

			attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramModelNoteFilterString, ToolboxItemFilterType.Allow);
			AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxModelNoteItemId, attribute);

			attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramExternalConstraintFilterString, ToolboxItemFilterType.Allow);
			string[] itemIds = new string[] {
				ResourceStrings.ToolboxEqualityConstraintItemId,
				ResourceStrings.ToolboxExclusionConstraintItemId,
				ResourceStrings.ToolboxExclusiveOrConstraintItemId,
				ResourceStrings.ToolboxExternalUniquenessConstraintItemId,
				ResourceStrings.ToolboxInclusiveOrConstraintItemId,
				ResourceStrings.ToolboxRingConstraintItemId,
				ResourceStrings.ToolboxSubsetConstraintItemId,
				ResourceStrings.ToolboxFrequencyConstraintItemId
			};
			for (int i = 0; i < itemIds.Length; ++i)
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
			pIdBmp = 0;
			return VSConstants.E_NOTIMPL;
		}

		int IVsInstalledProduct.IdIcoLogoForAboutbox(out uint pIdIco)
		{
			pIdIco = PackageResources.Id.AboutBoxIcon;
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
			object[] customAttributes = typeof(ORMDesignerPackage).Assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
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
		/// ORMModelBrowserToolWindow singleton
		/// </summary>
		public static ORMModelBrowserToolWindow ORMModelBrowserWindow
		{
			get
			{
				return (ORMModelBrowserToolWindow)mySingleton.GetToolWindow(typeof(ORMModelBrowserToolWindow), true);
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
		/// Gets the context tool window.
		/// </summary>
		/// <value>The context tool window.</value>
		public static ORMContextWindow ContextWindow
		{
			get
			{
				return (ORMContextWindow)mySingleton.GetToolWindow(typeof(ORMContextWindow), true);
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
		private ORMVerbalizationToolWindowSettings myVerbalizationWindowSettings = new ORMVerbalizationToolWindowSettings();
		/// <summary>
		/// Retrieve the settings for the verbalization toolbar. The settings cannot be
		/// stored with the window because they are required during initialization, and
		/// retrieving them forces recursive initialization calls.
		/// </summary>
		public static ORMVerbalizationToolWindowSettings VerbalizationWindowSettings
		{
			get
			{
				return mySingleton.myVerbalizationWindowSettings;
			}
		}
		/// <summary>
		/// Verbalization output tool window.
		/// </summary>
		public static ORMVerbalizationToolWindow VerbalizationWindow
		{
			get
			{
				return (ORMVerbalizationToolWindow)mySingleton.GetToolWindow(typeof(ORMVerbalizationToolWindow), true);
			}
		}
		/// <summary>
		/// Called if the verbalization window settings change in the options dialog.
		/// Does nothing if the window has not been created.
		/// </summary>
		public static void VerbalizationWindowGlobalSettingsChanged()
		{
			if (mySingleton != null)
			{
				ORMVerbalizationToolWindow window = (ORMVerbalizationToolWindow)mySingleton.GetToolWindow(typeof(ORMVerbalizationToolWindow), false);
				if (window != null)
				{
					window.GlobalSettingsChanged();
				}
			}
		}
		#endregion

		#region Extension DomainModels
		/// <summary>
		/// Returns an <see cref="IDictionary{String,Type}"/> containing the namespaces and <see cref="DomainModel"/> types
		/// of extension <see cref="DomainModel"/>s that should automatically be loaded.
		/// </summary>
		/// <remarks>
		/// If no extensions are set to auto-load, <see langword="null"/> is returned.
		/// </remarks>
		public static IDictionary<string, Type> GetAutoLoadExtensions()
		{
			ORMDesignerPackage singleton = mySingleton;
			if (singleton == null)
			{
				return null;
			}
			List<string> autoLoadNamespaces = new List<string>();
			using (RegistryKey applicationRegistryRoot = singleton.ApplicationRegistryRoot)
			{
				GetAutoLoadExtensionNamespaces(autoLoadNamespaces, applicationRegistryRoot);
			}
			using (RegistryKey userRegistryRoot = singleton.UserRegistryRoot)
			{
				GetAutoLoadExtensionNamespaces(autoLoadNamespaces, userRegistryRoot);
			}
			int autoLoadNamespacesCount = autoLoadNamespaces.Count;
			if (autoLoadNamespacesCount > 0)
			{
				Dictionary<string, Type> autoLoadExtensions = new Dictionary<string, Type>(autoLoadNamespacesCount, StringComparer.Ordinal);
				foreach (string @namespace in autoLoadNamespaces)
				{
					if (!autoLoadExtensions.ContainsKey(@namespace))
					{
						Type type = GetExtensionDomainModel(@namespace);
						if (type != null)
						{
							autoLoadExtensions.Add(@namespace, type);
						}
					}
				}
				return autoLoadExtensions;
			}
			return null;
		}
		/// <summary>
		/// Adds the namespaces for the auto-load extensions registered in <paramref name="hkeyBase"/> to <paramref name="autoLoadNamespaces"/>.
		/// </summary>
		private static void GetAutoLoadExtensionNamespaces(List<string> autoLoadNamespaces, RegistryKey hkeyBase)
		{
			using (RegistryKey extensions = hkeyBase.OpenSubKey(REGISTRYROOT_EXTENSIONS, RegistryKeyPermissionCheck.ReadSubTree))
			{
				if (extensions != null)
				{
					string[] namespaces = extensions.GetValue("AutoLoadNamespaces") as string[];
					if (namespaces != null)
					{
						for (int i = 0; i < namespaces.Length; i++)
						{
							string @namespace = namespaces[i];
							if (!string.IsNullOrEmpty(@namespace))
							{
								autoLoadNamespaces.Add(@namespace);
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Retrieves the <see cref="DomainModel"/> for a specific extension namespace.
		/// </summary>
		/// <remarks>If a <see cref="DomainModel"/> cannot be found for a namespace, <see langword="null"/> is returned.</remarks>
		public static Type GetExtensionDomainModel(string extensionNamespace)
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
		/// Loads and returns the extension <see cref="DomainModel"/> <see cref="Type"/> for <paramref name="extensionNamespace"/>.
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
							// If we don't have an extension type name, just return null
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

						Assembly extensionAssembly = Assembly.Load(extensionAssemblyName);
						Type extensionType = extensionAssembly.GetType(extensionTypeString, true, false);

						if (extensionType.IsSubclassOf(typeof(DomainModel)))
						{
							// SECURITY: APTCA: See the comment near our AssemblyResolve handler for information regarding
							// changes that would be needed here in order to securely support partially-trusted callers.
							ORMDesignerPackage.KnownAssemblies[extensionAssembly.FullName] = extensionAssembly;
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
			yield return typeof(Neumont.Tools.ORM.ObjectModel.ORMCoreDomainModel);
			yield return typeof(Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel);
			foreach (ORMExtensionType extension in GetAvailableCustomExtensions())
			{
				Type type = extension.Type;
				if (type != null)
				{
					yield return type;
				}
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
