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

// Turn this on to always refresh the toolbox for development builds. This incurs significant
// startup costs during debugging sessions, but should be turned on temporarily if toolbox items
// are currently under development
#define ALWAYS_REFRESH_EXP_TOOLBOX
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
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Diagrams.Design;
using ORMSolutions.ORMArchitect.Core.Load;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;

namespace ORMSolutions.ORMArchitect.Core.Shell
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
	[ProvideEditorFactory(typeof(ORMFactEditorEditorFactory), PackageResources.Id.FactEditorName, TrustLevel = __VSEDITORTRUSTLEVEL.ETL_AlwaysTrusted)]
	[ProvideService(typeof(ORMDesignerFontsAndColors), ServiceName = "OrmDesignerFontAndColorProvider")]
	[ProvideToolWindow(typeof(FactEditorToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMReferenceModeEditorToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMSamplePopulationToolWindow), Style = VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMReadingEditorToolWindow), Style = VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMVerbalizationToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMModelBrowserToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.SolutionExplorer)]
	[ProvideToolWindow(typeof(ORMDescriptionToolWindow), Style=VsDockStyle.Tabbed, Transient=true, Orientation=ToolWindowOrientation.Right, Window=ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMNotesToolWindow), Style = VsDockStyle.Tabbed, Transient = true, Orientation = ToolWindowOrientation.Right, Window = ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMContextWindow), Style = VsDockStyle.Tabbed, Transient = true, Orientation = ToolWindowOrientation.Right, Window = ToolWindowGuids.Outputwindow)]
	[ProvideToolWindow(typeof(ORMDiagramSpyWindow), Style = VsDockStyle.Tabbed, Transient = true, Orientation = ToolWindowOrientation.Right, Window = ToolWindowGuids.Outputwindow)]
	[ProvideToolWindowVisibility(typeof(FactEditorToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMReferenceModeEditorToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMSamplePopulationToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMReadingEditorToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMVerbalizationToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMModelBrowserToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMDescriptionToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMNotesToolWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMContextWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideToolWindowVisibility(typeof(ORMDiagramSpyWindow), ORMDesignerEditorFactory.GuidString)]
	[ProvideMenuResource(PackageResources.Id.CTMenu, 1)]
	[ProvideService(typeof(FactEditorLanguageService), ServiceName = FactEditorLanguageService.LanguageName)]
	[ProvideLanguageService(typeof(FactEditorLanguageService),
							FactEditorLanguageService.LanguageName,
							PackageResources.Id.FactEditorName,  // resource ID of localized language name
							CodeSense = true,                    // Supports IntelliSense
							QuickInfo = true,                    // Supports Quick info
							RequestStockColors = false,          // Supplies custom colors
							EnableCommenting = false,            // Supports commenting out code
							EnableAsyncCompletion = true,        // Supports background parsing
							EnableLineNumbers = false,
							ShowCompletion = true,
							ShowMatchingBrace = true,
							SupportCopyPasteOfHTML = true,
							ShowSmartIndent = false,
							EnableAdvancedMembersOption = false,
							ShowDropDownOptions = false
							)]
	[ProvideToolboxItems(1, true)]
	[ProvideToolboxFormat("Microsoft.VisualStudio.Modeling.ElementGroupPrototype")]
	[PackageRegistration(UseManagedResourcesOnly=true, RegisterUsing=RegistrationMethod.Assembly)]
#if !VISUALSTUDIO_10_0
	// This gives build warnings in VS2010, but there does not appear to be an alternative. The
	// settings for this attribute are maintained by hand in the pkgdef file.
	[InstalledProductRegistration(true, null, null, null, LanguageIndependentName = "Natural ORM Architect")]
#endif
	[ProvideLoadKey("Standard", "1.0", "Natural Object-Role Modeling Architect for Visual Studio", "ORM Solutions, LLC", PackageResources.Id.PackageLoadKey)]
	#endregion // Attributes
	public sealed class ORMDesignerPackage : ModelingPackage, IVsInstalledProduct, IVsToolWindowFactory
	{
		#region Constants
		private const string REGISTRYROOT_PACKAGE_USER = @"ORM Solutions\Natural ORM Architect";
#if VISUALSTUDIO_10_0
		// Key relative to the root local-machine key
		private const string REGISTRYROOT_PACKAGE_SETTINGS = @"Software\ORM Solutions\Natural ORM Architect for Visual Studio 2010\Designer";
#else
		// Key relative to the VS-provided application registry root
		private const string REGISTRYROOT_PACKAGE_SETTINGS = @"ORM Solutions\Natural ORM Architect";
#endif
		private const string REGISTRYKEY_EXTENSIONS = @"Extensions";
		private const string REGISTRYKEY_DESIGNERSETTINGS = @"DesignerSettings";
		private const string REGISTRYVALUE_VERBALIZATIONDIR = "VerbalizationDir";
		private const string REGISTRYVALUE_TOOLBOXREVISION_OBSOLETESINGLEVALUE = "ToolboxRevision";
		private const string REGISTRYKEY_TOOLBOXREVISIONS = "ToolboxRevisions";
		#endregion
		#region Member variables
		/// <summary>
		/// The commands supported by this package
		/// </summary>
		private CommandSet myCommandSet;
		private ORMDesignerFontsAndColors myFontAndColorService;
		private ORMDesignerSettings myDesignerSettings;
		private string myVerbalizationDirectory;
		private IDictionary<string, ToolboxProviderInfo> myToolboxProviderInfoMap;
		private ExtensionLoader myExtensionLoader;
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
		#region Properties
		/// <summary>
		/// Gets the singleton command set create for this package.
		/// </summary>
		public static CommandSet CommandSet
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
						package.myDesignerSettings = retVal = new ORMDesignerSettings(package, REGISTRYKEY_DESIGNERSETTINGS);
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
						using (RegistryKey key = package.PackageSettingsRegistryRoot)
						{
							if (key != null)
							{
								package.myVerbalizationDirectory = retVal = (string)key.GetValue(REGISTRYVALUE_VERBALIZATIONDIR, String.Empty);
							}
						}
					}
					return retVal;
				}
				return null;
			}
		}
		/// <summary>
		/// Retrieve the registry root for settings installed in the product directory.
		/// </summary>
		public RegistryKey PackageSettingsRegistryRoot
		{
			get
			{

#if VISUALSTUDIO_10_0
				return Registry.LocalMachine.OpenSubKey(REGISTRYROOT_PACKAGE_SETTINGS);
#else
				using (RegistryKey rootKey = ApplicationRegistryRoot)
				{
					if (rootKey != null)
					{
						return rootKey.OpenSubKey(REGISTRYROOT_PACKAGE_SETTINGS, RegistryKeyPermissionCheck.ReadSubTree);
					}
				}
				return null;
#endif
			}
		}
		/// <summary>
		/// Get the user registry root as a writable registry key.
		/// </summary>
		public RegistryKey PackageUserRegistryRoot
		{
			get
			{
				using (RegistryKey rootKey = UserRegistryRoot)
				{
					if (rootKey != null)
					{
						return rootKey.OpenSubKey(REGISTRYROOT_PACKAGE_USER, true);
					}
				}
				return null;
			}
		}
		/// <summary>
		/// For use by unit tests. Also used by ModelElementLocator.
		/// Private to discourage use outside of unit testing,
		/// may only be accessed through reflection.
		/// </summary>
		internal static ORMDesignerPackage Singleton
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

				FactEditorLanguageService managedLanguageService = new FactEditorLanguageService();
				managedLanguageService.SetSite(this);
				((IServiceContainer)this).AddService(typeof(FactEditorLanguageService), managedLanguageService, true);

				// setup commands
				(myCommandSet = ORMDesignerDocView.CreateCommandSet(this)).Initialize();

				// Create managed tool windows
				AddToolWindow(typeof(ORMModelBrowserToolWindow));
				AddToolWindow(typeof(ORMReadingEditorToolWindow));
				AddToolWindow(typeof(ORMReferenceModeEditorToolWindow));
				AddToolWindow(typeof(ORMSamplePopulationToolWindow));
				AddToolWindow(typeof(ORMVerbalizationToolWindow));
				AddToolWindow(typeof(ORMDescriptionToolWindow));
				AddToolWindow(typeof(ORMNotesToolWindow));
				AddToolWindow(typeof(ORMContextWindow));
				AddToolWindow(typeof(ORMDiagramSpyWindow));

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
#if ALWAYS_REFRESH_EXP_TOOLBOX
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
#endif // ALWAYS_REFRESH_EXP_TOOLBOX
					RegistryKey userRegistryRoot = null;
					RegistryKey packageRegistryRoot = null;
					RegistryKey toolboxRevisionsKey = null;
					IDictionary<string, ToolboxProviderInfo> providers = GetToolboxProviderInfoMap();
					bool refreshRequired = false;
					try
					{
						userRegistryRoot = this.UserRegistryRoot;
						// Note that we could do this twice, once to verify in read mode and a second time to write the
						// values back out, but it isn't work the extra hassle to do this twice simply to avoid a write
						// permission check on a user registry key.
						packageRegistryRoot = userRegistryRoot.OpenSubKey(REGISTRYROOT_PACKAGE_USER, RegistryKeyPermissionCheck.ReadWriteSubTree);
						bool hadRegistryRoot = packageRegistryRoot != null;
						bool hadRevisionsKey = false;

						// If the toolbox has the correct values for all domain models that provide toolbox items,
						// then we don't do anything.
						if (hadRegistryRoot)
						{
							// Remove obsolete data
							packageRegistryRoot.DeleteValue(REGISTRYVALUE_TOOLBOXREVISION_OBSOLETESINGLEVALUE, false);

							// Get new key information
							toolboxRevisionsKey = packageRegistryRoot.OpenSubKey(REGISTRYKEY_TOOLBOXREVISIONS, RegistryKeyPermissionCheck.ReadWriteSubTree);
							hadRevisionsKey = toolboxRevisionsKey != null;
						}
						if (!hadRegistryRoot)
						{
							packageRegistryRoot = userRegistryRoot.CreateSubKey(REGISTRYROOT_PACKAGE_USER, RegistryKeyPermissionCheck.ReadWriteSubTree);
						}
						if (!hadRevisionsKey)
						{
							toolboxRevisionsKey = packageRegistryRoot.CreateSubKey(REGISTRYKEY_TOOLBOXREVISIONS, RegistryKeyPermissionCheck.ReadWriteSubTree);
						}

						string[] valueNames = hadRevisionsKey ? toolboxRevisionsKey.GetValueNames() : null;
						int matchIndex;
						int hitCount = 0;
						foreach (KeyValuePair<string, ToolboxProviderInfo> providerInfo in providers)
						{
							int assemblyRevision = providerInfo.Value.ExpectedRevisionNumber;
							string typeFullName = providerInfo.Key;
							if (hadRevisionsKey && -1 != (matchIndex = Array.IndexOf<string>(valueNames, typeFullName)))
							{
								string valueName = valueNames[matchIndex];
								valueNames[matchIndex] = null;
								++hitCount;
								if (assemblyRevision != 0)
								{
									int? revision = toolboxRevisionsKey.GetValue(valueName, null) as int?;
									if (revision.HasValue)
									{
										if (assemblyRevision != revision.Value)
										{
											refreshRequired = true;
											toolboxRevisionsKey.SetValue(valueName, assemblyRevision, RegistryValueKind.DWord);
										}
									}
									else
									{
										// Wrong value kind, delete and readd the value
										refreshRequired = true;
										toolboxRevisionsKey.DeleteValue(valueName);
										toolboxRevisionsKey.SetValue(valueName, assemblyRevision, RegistryValueKind.DWord);
									}
								}
								else
								{
									refreshRequired = true;
									toolboxRevisionsKey.DeleteValue(valueName);
								}
							}
							else
							{
								refreshRequired = true;
								if (assemblyRevision != 0)
								{
									toolboxRevisionsKey.SetValue(typeFullName, assemblyRevision, RegistryValueKind.DWord);
								}
							}
						}
						if (hadRevisionsKey && hitCount != valueNames.Length)
						{
							refreshRequired = true;
							for (int i = 0; i < valueNames.Length; ++i)
							{
								string removeValue = valueNames[i];
								if (removeValue != null)
								{
									toolboxRevisionsKey.DeleteValue(removeValue, false);
								}
							}
						}
					}
					catch (System.Security.SecurityException ex)
					{
						Debug.Fail("A security exception occurred while trying to write the current toolbox format revision number to the user registry. " +
							"You can safely continue execution of the program.", ex.ToString());
						// Swallow the exception, since it won't actually cause a problem. The next time the package is loaded, we'll just initialize the toolbox again.
					}
					finally
					{
						if (userRegistryRoot != null)
						{
							if (packageRegistryRoot != null)
							{
								if (toolboxRevisionsKey != null)
								{
									toolboxRevisionsKey.Close();
								}
								packageRegistryRoot.Close();
							}
							userRegistryRoot.Close();
						}
					}

					if (refreshRequired)
					{
						// Since the toolbox has either not been set up before, or is from an older or newer revision, call SetupDynamicToolbox.
						// This might not be necessary if it is from a newer revision, but we do it anyway to be safe.
						base.SetupDynamicToolbox();
					}
#if ALWAYS_REFRESH_EXP_TOOLBOX
				}
			}
			else if (!string.IsNullOrEmpty(optionValue))
			{
				// If any non-empty root suffix was specified as a command line parameter, call SetupDynamicToolbox.
				base.SetupDynamicToolbox();
			}
#endif // ALWAYS_REFRESH_EXP_TOOLBOX
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
				service.RemoveService(typeof(FactEditorLanguageService), true);
				// dispose of any private objects here
			}
			base.Dispose(disposing);
		}
		/// <summary>
		/// Retrieve toolbox items. Called during devenv /setup or
		/// toolbox refresh.
		/// </summary>
		protected sealed override IList<ModelingToolboxItem> CreateToolboxItems()
		{
			IList<ModelingToolboxItem> items = null;
			foreach (ToolboxProviderInfo providerInfo in GetToolboxProviderInfoMap().Values)
			{
				Type providerType = providerInfo.GetResolvedType();
				if (providerType != null)
				{
					object[] attributes = providerType.GetCustomAttributes(typeof(ModelingToolboxItemProviderAttribute), false);
					int attributeCount;
					if (attributes != null &&
						0 != (attributeCount = attributes.Length))
					{
						for (int i = 0; i < attributes.Length; ++i)
						{
							IList<ModelingToolboxItem> localItems = ((ModelingToolboxItemProviderAttribute)attributes[i]).CreateToolboxItems(this, providerType);
							int localItemCount;
							if (localItems != null &&
								0 != (localItemCount = localItems.Count))
							{
								if (items == null)
								{
									items = localItems;
								}
								else
								{
									if (items.IsReadOnly)
									{
										items = new List<ModelingToolboxItem>(items);
									}
									for (int j = 0; j < localItemCount; ++j)
									{
										items.Add(localItems[j]);
									}
								}
							}
						}
					}
				}
			}
			return items;
		}
		#endregion // Base overrides
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
		/// Description tool window.
		/// </summary>
		public static ORMDescriptionToolWindow InformalDescriptionWindow
		{
			get
			{
				return (ORMDescriptionToolWindow)mySingleton.GetToolWindow(typeof(ORMDescriptionToolWindow), true);
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
		public static ORMContextWindow ContextWindow
		{
			get
			{
				return (ORMContextWindow)mySingleton.GetToolWindow(typeof(ORMContextWindow), true);
			}
		}

		/// <summary>
		/// Gets the diagram spy tool window.
		/// </summary>
		public static ORMDiagramSpyWindow DiagramSpyWindow
		{
			get
			{
				return (ORMDiagramSpyWindow)mySingleton.GetToolWindow(typeof(ORMDiagramSpyWindow), true);
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
		/// <summary>
		/// Defer to the FactEditorLanguageService to create the tool window on demand
		/// </summary>
		private IVsWindowFrame EnsureFactEditorToolWindow()
		{
			return ((FactEditorLanguageService)this.GetService(typeof(FactEditorLanguageService))).FactEditorToolWindow;
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
		#region IVsToolWindowFactory Implementation
		private delegate int ForwardCreateToolWindowDelegate(ModelingPackage @this, ref Guid rguidPersistenceSlot, uint dwToolWindowId);
		private static ForwardCreateToolWindowDelegate myForwardCreateToolWindow;
		private static ForwardCreateToolWindowDelegate ForwardCreateToolWindow
		{
			get
			{
				ForwardCreateToolWindowDelegate retVal = myForwardCreateToolWindow;
				if (retVal == null)
				{
					System.Threading.Interlocked.CompareExchange<ForwardCreateToolWindowDelegate>(
						ref myForwardCreateToolWindow,
						Utility.GetBaseInterfaceMethodDelegate<ForwardCreateToolWindowDelegate>(typeof(IVsToolWindowFactory), "CreateToolWindow"),
						null);
					retVal = myForwardCreateToolWindow;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Shadow the toolwindow creation in the base Package code. Enables creation
		/// of tool windows that are non derived from the managed framework helpers.
		/// </summary>
		int IVsToolWindowFactory.CreateToolWindow(ref Guid rguidPersistenceSlot, uint dwToolWindowId)
		{
			if (rguidPersistenceSlot == typeof(FactEditorToolWindow).GUID)
			{
				EnsureFactEditorToolWindow();
				return 0;
			}
			return ForwardCreateToolWindow(this, ref rguidPersistenceSlot, dwToolWindowId);
		}
		#endregion // IVsToolWindowFactory Implementation
		#region Extension DomainModels
		/// <summary>
		/// Get the <see cref="ExtensionLoader"/> for this package
		/// </summary>
		public static ExtensionLoader ExtensionLoader
		{
			get
			{
				ORMDesignerPackage package = mySingleton;
				if (package == null)
				{
					return null;
				}
				ExtensionLoader retVal = package.myExtensionLoader;
				if (retVal == null)
				{
					using (RegistryKey settingsRegistryRoot = package.PackageSettingsRegistryRoot)
					{
						using (RegistryKey userRegistryRoot = package.PackageUserRegistryRoot)
						{
							package.myExtensionLoader = retVal = new ExtensionLoader(ExtensionModelData.LoadFromRegistry(REGISTRYKEY_EXTENSIONS, settingsRegistryRoot, userRegistryRoot));
						}
					}
				}
				return retVal;
			}
		}
		private class ToolboxProviderInfo
		{
			private string myExtensionNamespaceUri;
			private int myExpectedRevisionNumber;
			private Type myResolvedType;
			/// <summary>
			/// Create toolbox provider revision information for loaded type
			/// </summary>
			/// <param name="providerType">A standard provider type</param>
			public ToolboxProviderInfo(Type providerType)
			{
				myResolvedType = providerType;
				object[] customAttributes = providerType.Assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
				int? assemblyRevision = null;
				for (int i = 0; i < customAttributes.Length; i++)
				{
					assemblyRevision = new Version(((AssemblyFileVersionAttribute)customAttributes[i]).Version).Revision;
				}
				myExpectedRevisionNumber = assemblyRevision.GetValueOrDefault(0);
			}
			/// <summary>
			/// Create toolbox provider revision information for an extension type
			/// </summary>
			/// <param name="extensionNamespaceUri">The extension identifier</param>
			/// <param name="extensionRevisionNumber">The expected revision number for the package</param>
			public ToolboxProviderInfo(string extensionNamespaceUri, int extensionRevisionNumber)
			{
				myExtensionNamespaceUri = extensionNamespaceUri;
				myExpectedRevisionNumber = extensionRevisionNumber;
			}
			/// <summary>
			/// Get the expected revision number for this toolbox provider
			/// </summary>
			public int ExpectedRevisionNumber
			{
				get
				{
					return myExpectedRevisionNumber;
				}
			}
			/// <summary>
			/// Get the resolved type for the toolbox provider <see cref="DomainModel"/>
			/// </summary>
			public Type GetResolvedType()
			{
				Type retVal = myResolvedType;
				if (retVal == null)
				{
					ExtensionModelBinding? extensionType = ExtensionLoader.GetExtensionDomainModel(myExtensionNamespaceUri);
					if (extensionType.HasValue)
					{
						myResolvedType = retVal = extensionType.Value.Type;
					}
				}
				return retVal;
			}
		}
		private static IDictionary<string, ToolboxProviderInfo> GetToolboxProviderInfoMap()
		{
			IDictionary<string, ToolboxProviderInfo> retVal = null;
			ORMDesignerPackage package = mySingleton;
			if (package != null)
			{
				retVal = package.myToolboxProviderInfoMap;
				if (retVal == null)
				{
					retVal = new Dictionary<string, ToolboxProviderInfo>();

					// Add standard types
					Type standardType = typeof(ORMShapeDomainModel);
					retVal.Add(standardType.FullName, new ToolboxProviderInfo(standardType));

					// Get revision information from the registry. The revision information is written to
					// the registry to enable toolbox maintenance without unnecessarily loading extension assemblies.
					using (RegistryKey settingsRegistryRoot = package.PackageSettingsRegistryRoot)
					{
						LoadExtensionToolboxProviders(settingsRegistryRoot, retVal);
					}
					using (RegistryKey userRegistryRoot = package.PackageUserRegistryRoot)
					{
						LoadExtensionToolboxProviders(userRegistryRoot, retVal);
					}
					package.myToolboxProviderInfoMap = retVal;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Helper method for <see cref="GetToolboxProviderInfoMap"/>
		/// </summary>
		private static void LoadExtensionToolboxProviders(RegistryKey rootKey, IDictionary<string, ToolboxProviderInfo> providerMap)
		{
			if (null == rootKey)
			{
				return;
			}
			using (RegistryKey hkeyExtensions = rootKey.OpenSubKey(REGISTRYKEY_EXTENSIONS, RegistryKeyPermissionCheck.ReadSubTree))
			{
				if (hkeyExtensions != null)
				{
					string[] extensionNamespaces = hkeyExtensions.GetSubKeyNames();
					for (int i = 0; i < extensionNamespaces.Length; ++i)
					{
						string extensionNamespace = extensionNamespaces[i];
						using (RegistryKey hkeyExtensionKey = hkeyExtensions.OpenSubKey(extensionNamespace))
						{
							int? revisionNumber = hkeyExtensionKey.GetValue("ToolboxItemProviderRevisionNumber") as int?;
							string extensionClass;
							if (revisionNumber.HasValue &&
								!string.IsNullOrEmpty(extensionClass = hkeyExtensionKey.GetValue("Class") as string))
							{
								providerMap[extensionClass] = new ToolboxProviderInfo(extensionNamespace, revisionNumber.Value);
							}
						}
					}
				}
			}
		}
		#endregion // Extension DomainModels
	}
}
