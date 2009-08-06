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
	[InstalledProductRegistration(true, null, null, null, LanguageIndependentName="Natural ORM Architect")]
	[ProvideLoadKey("Standard", "1.0", "Natural Object-Role Modeling Architect for Visual Studio", "ORM Solutions, LLC", PackageResources.Id.PackageLoadKey)]
	#endregion // Attributes
	public sealed class ORMDesignerPackage : ModelingPackage, IVsInstalledProduct, IVsToolWindowFactory
	{
		#region Constants
		private const string REGISTRYROOT_PACKAGE = @"ORM Solutions\Natural ORM Architect";
		private const string REGISTRYROOT_EXTENSIONS = REGISTRYROOT_PACKAGE + @"\Extensions\";
		private const string REGISTRYROOT_DESIGNERSETTINGS = REGISTRYROOT_PACKAGE + @"\DesignerSettings\";
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
		private IDictionary<string, ORMExtensionType> myAvailableExtensions;
		private IDictionary<Guid, string> myExtensionIdToExtensionNameMap;
		private IDictionary<Guid, Type> myStandardDomainModelsMap;
		private IDictionary<string, ToolboxProviderInfo> myToolboxProviderInfoMap;
		private string[] myAutoLoadExtensions;
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
						package.myDesignerSettings = retVal = new ORMDesignerSettings(package, REGISTRYROOT_DESIGNERSETTINGS);
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
						packageRegistryRoot = userRegistryRoot.OpenSubKey(REGISTRYROOT_PACKAGE, RegistryKeyPermissionCheck.ReadWriteSubTree);
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
							packageRegistryRoot = userRegistryRoot.CreateSubKey(REGISTRYROOT_PACKAGE, RegistryKeyPermissionCheck.ReadWriteSubTree);
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
		/// Generate a collection of autoload extension namespaces
		/// </summary>
		private static string[] GetAutoLoadExtensions()
		{
			string[] retVal = null;
			ORMDesignerPackage package = mySingleton;
			if (package != null)
			{
				retVal = package.myAutoLoadExtensions;
				if (retVal == null)
				{
					ICollection<ORMExtensionType> availableExtensions = GetAvailableCustomExtensions().Values;
					int autoLoadNamespacesCount = 0;
					foreach (ORMExtensionType testType in availableExtensions)
					{
						if (testType.IsAutoLoad)
						{
							++autoLoadNamespacesCount;
						}
					}
					if (autoLoadNamespacesCount != 0)
					{
						retVal = new string[autoLoadNamespacesCount];
						int currentIndex = 0;
						foreach (ORMExtensionType testType in availableExtensions)
						{
							if (testType.IsAutoLoad)
							{
								retVal[currentIndex] = testType.NamespaceUri;
								if (++currentIndex == autoLoadNamespacesCount)
								{
									break;
								}
							}
						}
					}
					else
					{
						retVal = new string[0];
					}
					package.myAutoLoadExtensions = retVal;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Retrieves the <see cref="DomainModel"/> for a specific extension namespace.
		/// </summary>
		/// <remarks>If a <see cref="DomainModel"/> cannot be found for a namespace, <see langword="null"/> is returned.</remarks>
		public static ORMExtensionType? GetExtensionDomainModel(string extensionNamespace)
		{
			ORMExtensionType extensionType;
			return GetAvailableCustomExtensions().TryGetValue(extensionNamespace, out extensionType) ? new ORMExtensionType?(extensionType) : null;
		}
		/// <summary>
		/// Loads and returns the extension <see cref="DomainModel"/> <see cref="Type"/> for <paramref name="extensionNamespace"/>.
		/// </summary>
		private static Type LoadExtension(string extensionNamespace, RegistryKey hkeyExtensions, out bool isSecondary, out bool isAutoLoad)
		{
			isSecondary = false;
			isAutoLoad = false;
			using (RegistryKey hkeyExtension = hkeyExtensions.OpenSubKey(extensionNamespace, RegistryKeyPermissionCheck.ReadSubTree))
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

						object valueObject = hkeyExtension.GetValue("SecondaryNamespace");
						isSecondary = valueObject != null && ((int)valueObject) == 1;
						valueObject = hkeyExtension.GetValue("AutoLoadNamespace");
						isAutoLoad = valueObject != null && ((int)valueObject) == 1;

						string assemblyValue = hkeyExtension.GetValue("Assembly") as string;
						string codeBaseValue = hkeyExtension.GetValue("CodeBase") as string;
						if (string.IsNullOrEmpty(assemblyValue) && string.IsNullOrEmpty(codeBaseValue))
						{
							// Extension is registered in this assembly
							return typeof(ORMDesignerPackage).Assembly.GetType(extensionTypeString, true, false);
						}
						else
						{

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
		/// <returns>See <see cref="IEnumerable{Type}"/></returns>
		public static IEnumerable<Type> GetAvailableDomainModels()
		{
			foreach (Type standardType in GetStandardDomainModelsMap().Values)
			{
				yield return standardType;
			}
			foreach (ORMExtensionType extension in GetAvailableCustomExtensions().Values)
			{
				yield return extension.Type;
			}
		}
		/// <summary>
		/// Get the standard models that are always loaded with the tool
		/// </summary>
		public static ICollection<Type> StandardDomainModels
		{
			get
			{
				return GetStandardDomainModelsMap().Values;
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
					ORMExtensionType? extensionType = GetExtensionDomainModel(myExtensionNamespaceUri);
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
					using (RegistryKey applicationRegistryRoot = package.ApplicationRegistryRoot)
					{
						LoadExtensionToolboxProviders(applicationRegistryRoot, retVal);
					}
					using (RegistryKey userRegistryRoot = package.UserRegistryRoot)
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
			using (RegistryKey hkeyExtensions = rootKey.OpenSubKey(REGISTRYROOT_EXTENSIONS, RegistryKeyPermissionCheck.ReadSubTree))
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
		/// <summary>
		/// Get a dictionary containing all standard domain models
		/// keyed of the domain model identifier.
		/// </summary>
		private static IDictionary<Guid, Type> GetStandardDomainModelsMap()
		{
			IDictionary<Guid, Type> retVal = null;
			ORMDesignerPackage package = mySingleton;
			if (package != null)
			{
				retVal = package.myStandardDomainModelsMap;
				if (retVal == null)
				{
					// Any model change here that has toolbox information requires a corresponding change in GetToolboxProviderInfoMap
					retVal = new Dictionary<Guid, Type>(6);
					retVal.Add(FrameworkDomainModel.DomainModelId, typeof(FrameworkDomainModel));
					retVal.Add(ORMCoreDomainModel.DomainModelId, typeof(ORMCoreDomainModel));
					retVal.Add(CoreDesignSurfaceDomainModel.DomainModelId, typeof(CoreDesignSurfaceDomainModel));
					retVal.Add(ORMShapeDomainModel.DomainModelId, typeof(ORMShapeDomainModel));
					// UNDONE: Temporary until the report validation is moved into a separate dll. See https://projects.neumont.edu/orm2/ticket/315
					retVal.Add(ObjectModel.Verbalization.HtmlReport.DomainModelId, typeof(ObjectModel.Verbalization.HtmlReport));
					retVal.Add(ORMSolutions.ORMArchitect.Framework.Shell.DiagramSurvey.DomainModelId, typeof(ORMSolutions.ORMArchitect.Framework.Shell.DiagramSurvey));
					package.myStandardDomainModelsMap = retVal;
				}
			}
			return retVal;
		}
		/// <summary>
		/// This method cycles through the registered Custom Extensions.
		/// It then returns an IList of ORMExtensionType. containing all the Types of the Custom Extensions.
		/// </summary>
		/// <returns>An IList of registered ORMExtensionTypes.</returns>
		public static IDictionary<string, ORMExtensionType> GetAvailableCustomExtensions()
		{
			IDictionary<string, ORMExtensionType> retVal = null;
			ORMDesignerPackage package = mySingleton;
			if (package != null)
			{
				retVal = package.myAvailableExtensions;
				if (retVal == null)
				{
					retVal = new Dictionary<string, ORMExtensionType>();

					// Check for CustomExtensions in the ApplicationRegistryRoot and the UserRegistryRoot keys
					using (RegistryKey applicationRegistryRoot = package.ApplicationRegistryRoot)
					{
						LoadAvailableCustomExtensions(applicationRegistryRoot, retVal);
					}
					using (RegistryKey userRegistryRoot = package.UserRegistryRoot)
					{
						LoadAvailableCustomExtensions(userRegistryRoot, retVal);
					}
					package.myAvailableExtensions = retVal;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Helper method for <see cref="GetAvailableCustomExtensions"/>
		/// </summary>
		private static void LoadAvailableCustomExtensions(RegistryKey rootKey, IDictionary<string, ORMExtensionType> extensionMap)
		{
			using (RegistryKey hkeyExtensions = rootKey.OpenSubKey(REGISTRYROOT_EXTENSIONS, RegistryKeyPermissionCheck.ReadSubTree))
			{
				if (hkeyExtensions != null)
				{
					string[] extensionNamespaces = hkeyExtensions.GetSubKeyNames();
					foreach (string extensionNamespace in extensionNamespaces)
					{
						Type t;
						ORMExtensionType extensionType;
						bool isSecondary;
						bool isAutoLoad;
						if (null != (t = LoadExtension(extensionNamespace, hkeyExtensions, out isSecondary, out isAutoLoad)) &&
							(extensionType = new ORMExtensionType(extensionNamespace, t, isSecondary, isAutoLoad)).IsValidExtension)
						{
							// If there is a duplicate, let the user settings win
							extensionMap[extensionNamespace] = extensionType;
						}
					}
				}
			}
		}
		/// <summary>
		/// Get the domain model name corresponding to an extension domain model identifier.
		/// Returns <see langword="null"/> if the domain model is not an available extension.
		/// </summary>
		public static string MapExtensionDomainModelToName(Guid domainModelId)
		{
			string retVal = null;
			GetExtensionIdToExtensionNameMap().TryGetValue(domainModelId, out retVal);
			return retVal;
		}
		/// <summary>
		/// Generate a map from an extension domain model id to the identifying extension name
		/// </summary>
		private static IDictionary<Guid, string> GetExtensionIdToExtensionNameMap()
		{
			IDictionary<Guid, string> retVal = null;
			ORMDesignerPackage package = mySingleton;
			if (package != null)
			{
				retVal = package.myExtensionIdToExtensionNameMap;
				if (retVal == null)
				{
					IDictionary<string, ORMExtensionType> availableExtensions = GetAvailableCustomExtensions();
					retVal = new Dictionary<Guid, string>(availableExtensions.Count);
					foreach (KeyValuePair<string, ORMExtensionType> pair in availableExtensions)
					{
						retVal.Add(pair.Value.DomainModelId, pair.Key);
					}
					package.myExtensionIdToExtensionNameMap = retVal;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Extend the set of required extensions to include any dependencies
		/// </summary>
		/// <param name="extensions">Currently loaded extensions. May be created if null to add auto-load extensions.</param>
		public static void VerifyRequiredExtensions(ref Dictionary<string, ORMExtensionType> extensions)
		{
			if (mySingleton == null)
			{
				return;
			}
			IDictionary<string, ORMExtensionType> availableExtensions = GetAvailableCustomExtensions();
			string[] autoLoadExtensions = GetAutoLoadExtensions();

			// First get all autoload extensions
			for (int i = 0; i < autoLoadExtensions.Length; ++i)
			{
				string extensionNamespace = autoLoadExtensions[i];
				if (extensions == null)
				{
					extensions = new Dictionary<string, ORMExtensionType>();
					extensions[extensionNamespace] = availableExtensions[extensionNamespace];
				}
				else if (!extensions.ContainsKey(extensionNamespace))
				{
					extensions[extensionNamespace] = availableExtensions[extensionNamespace];
				}
			}
			if (extensions == null)
			{
				return;
			}

			IDictionary<Guid, string> idToExtensionNameMap = GetExtensionIdToExtensionNameMap();
			IDictionary<Guid, Type> standardModelsMap = GetStandardDomainModelsMap();

			// Get a starting keyset we can iterate so the enumerators don't cry foul
			ICollection<string> startKeysCollection = extensions.Keys;
			int startKeysCount = startKeysCollection.Count;
			if (startKeysCount == 0)
			{
				return;
			}
			string[] startKeys = new string[startKeysCount];
			startKeysCollection.CopyTo(startKeys, 0);

			// Recursively verify dependencies for each starting element
			for (int i = 0; i < startKeys.Length; ++i)
			{
				VerifyExtensions(startKeys[i], extensions, availableExtensions, idToExtensionNameMap, standardModelsMap);
			}
		}
		/// <summary>
		/// Recursively add additional extension models. Helper function for <see cref="VerifyRequiredExtensions"/>
		/// </summary>
		private static void VerifyExtensions(string extensionNamespace, IDictionary<string, ORMExtensionType> targetExtensions, IDictionary<string, ORMExtensionType> availableExtensions, IDictionary<Guid, string> extensionModelMap, IDictionary<Guid, Type> standardModelMap)
		{
			ORMExtensionType extension = availableExtensions[extensionNamespace];
			ICollection<Guid> recurseExtensions = extension.ExtendsDomainModelIds;
			if (recurseExtensions.Count != 0)
			{
				foreach (Guid recurseExtensionId in recurseExtensions)
				{
					string recurseExtensionNamespace;
					if (extensionModelMap.TryGetValue(recurseExtensionId, out recurseExtensionNamespace) &&
						!standardModelMap.ContainsKey(recurseExtensionId) &&
						!targetExtensions.ContainsKey(recurseExtensionNamespace))
					{
						targetExtensions.Add(recurseExtensionNamespace, availableExtensions[recurseExtensionNamespace]);
						VerifyExtensions(recurseExtensionNamespace, targetExtensions, availableExtensions, extensionModelMap, standardModelMap);
					}
				}
			}
		}
		#endregion // Extension DomainModels
	}
}
