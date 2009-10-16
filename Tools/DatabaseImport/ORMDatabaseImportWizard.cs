#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.Data;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.VisualStudio.Data;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;

using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using ServiceProvider = Microsoft.VisualStudio.Shell.ServiceProvider;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace ORMSolutions.ORMArchitect.DatabaseImport
{
	/// <summary>
	/// Wizard interface used when loading a DcilREFromDB template
	/// </summary>	
	[CLSCompliant(false)]
	[TemplateWizardDisallowUserTemplatesSecurity(true)]
	public class ORMDatabaseImportWizard : IWizard
	{
		#region Member variables
		private bool myAddToProject = false;
		#endregion // Member variables
		#region IWizard Members
		/// <summary>
		/// Implements <see cref="IWizard.BeforeOpeningFile"/>
		/// </summary>
		protected void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
		{
		}
		void IWizard.BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
		{
			BeforeOpeningFile(projectItem);
		}

		/// <summary>
		/// Implements <see cref="IWizard.ProjectFinishedGenerating"/>
		/// </summary>
		protected void ProjectFinishedGenerating(EnvDTE.Project project)
		{
		}
		void IWizard.ProjectFinishedGenerating(EnvDTE.Project project)
		{
			ProjectFinishedGenerating(project);
		}

		/// <summary>
		/// Implements <see cref="IWizard.ProjectItemFinishedGenerating"/>
		/// </summary>
		protected void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
		{
		}
		void IWizard.ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
		{
			ProjectItemFinishedGenerating(projectItem);
		}

		/// <summary>
		/// Implements <see cref="IWizard.RunFinished"/>
		/// </summary>
		protected void RunFinished()
		{
		}
		void IWizard.RunFinished()
		{
			RunFinished();
		}

		/// <summary>
		/// Method use to get the type of Service specified T from the Provider
		/// </summary>		
		/// <param name="provider">The Provider to get the service from</param>	
		public static T GetService<T>(IServiceProvider provider)
			where T : class
		{
			Type serviceType = typeof(T);
			return ((provider != null) ? provider.GetService(serviceType) as T : null) ?? Package.GetGlobalService(serviceType) as T;
		}

		/// <summary>
		/// Implements <see cref="IWizard.RunStarted"/>
		/// </summary>
		protected void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			IOleServiceProvider oleServiceProvider = automationObject as IOleServiceProvider;
			ServiceProvider serviceProvider = (oleServiceProvider != null) ? new ServiceProvider(oleServiceProvider) : null;

			DataConnectionDialogFactory dialogFactory = GetService<DataConnectionDialogFactory>(serviceProvider);
			Debug.Assert(dialogFactory != null, "Can't get DataConnectionDialogFactory!");

			System.Data.IDbConnection dbConn;
			DataConnectionDialog dialog = dialogFactory.CreateConnectionDialog();
			dialog.AddAllSources();
			DataConnection dataConn = dialog.ShowDialog(false);
			if (dataConn != null)
			{
				if ((dbConn = dataConn.ConnectionSupport.ProviderObject as System.Data.IDbConnection) == null)
				{
					// show error
					return;
				}

				DataProviderManager manager = GetService<DataProviderManager>(serviceProvider);
				if (manager != null)
				{
					DataProvider provider = manager.GetDataProvider(dataConn.Provider);
					string invariantName = provider.GetProperty("InvariantName") as string;

					IList<string> schemaList = DcilSchema.GetAvailableSchemaNames(dbConn, invariantName);
					string selectedSchema = null;
					switch (schemaList.Count)
					{
						case 1:
							selectedSchema = schemaList[0];
							break;
						default:
                            // Allow this for an empty list
							selectedSchema = SchemaSelector.SelectSchema(serviceProvider, schemaList);
							break;
					}

					if (!string.IsNullOrEmpty(selectedSchema))
					{
						DcilSchema schema = DcilSchema.FromSchemaName(selectedSchema, dbConn, invariantName);

						StringBuilder stringBuilder = new StringBuilder();
						string replacementString = null;
						using (MemoryStream ms = new MemoryStream())
						{
							DcilSchema.Serialize(schema, ms);
							ms.Seek(0, SeekOrigin.Begin);
							StreamReader sr = new StreamReader(ms);
							replacementString = sr.ReadToEnd();
						}


						replacementsDictionary.Add("$DcilFile$", replacementString);
                        myAddToProject = true;
                    }
				}
			}
		}
		void IWizard.RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			RunStarted(automationObject, replacementsDictionary, runKind, customParams);
		}

		/// <summary>
		/// Implements <see cref="IWizard.ShouldAddProjectItem"/>
		/// </summary>
		protected bool ShouldAddProjectItem(string filePath)
		{
			return myAddToProject;
		}
		bool IWizard.ShouldAddProjectItem(string filePath)
		{
			return ShouldAddProjectItem(filePath);
		}
		#endregion
	}
	// Following code is lifted directly from ORMModel/Shell/ORMGeneralTemplateWizard.cs

	/// <summary>
	/// Works around Visual Studio bugs in order to allow <c>.vstemplate</c> files
	/// to be used for the "<c>General</c>" <c>ProjectType</c>.
	/// </summary>
	[CLSCompliant(false)]
	public class ORMDatabaseImportGeneralProjectWizard : ORMDatabaseImportWizard, IWizard
	{
		private static bool? TemplateWizardIsOrcasOrLater;

		/// <summary>
		/// Prevents the pre-Orcas version of <c>Microsoft.VisualStudio.TemplateWizard.Wizard</c> from crashing in <c>OpenNotedItems</c>.
		/// </summary>
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
		protected new void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			base.RunStarted(automationObject, replacementsDictionary, runKind, customParams);
			if (!base.ShouldAddProjectItem(null))
			{
				return;
			}
			const int OrcasMajorVersionNumber = 9;
			// UNDONE: See assert where this is used const string TemplateWizardAssemblyName = "Microsoft.VisualStudio.TemplateWizard";
			const string SuppressOpeningItems = "$__suppress_opening_items__$";

			bool? templateWizardIsOrcasOrLater = TemplateWizardIsOrcasOrLater;

			// If we haven't yet determined whether we are being called by the pre-Orcas version of TemplateWizard or not, do so now.
			if (!templateWizardIsOrcasOrLater.HasValue)
			{
				System.Reflection.Assembly callingAssembly = System.Reflection.Assembly.GetCallingAssembly();

				// UNDONE: This is asserting at this point, figure out how to determine if we're Whidbey or Orcas
				//Debug.Assert(callingAssembly != null && callingAssembly.FullName.StartsWith(TemplateWizardAssemblyName, StringComparison.Ordinal));

				// If for any reason we can't tell what version we're running under, we assume it is pre-Orcas.
				templateWizardIsOrcasOrLater = TemplateWizardIsOrcasOrLater =
					(callingAssembly != null && (callingAssembly.GetName().Version.Major >= OrcasMajorVersionNumber));
			}

			if (templateWizardIsOrcasOrLater.GetValueOrDefault())
			{
				// The Orcas version of TemplateWizard correctly checks Project.FullName for null / empty string before
				// trying to create a Uri from it. Therefore, this workaround is only needed for pre-Orcas versions.
				return;
			}

			Debug.Assert(runKind == WizardRunKind.AsNewItem);

			if (replacementsDictionary == null)
			{
				// If we don't have a replacements dictionary, we can't do anything anyway, so just bail out.
				return;
			}

			if (!replacementsDictionary.ContainsKey(SuppressOpeningItems))
			{
				// The value that we add doesn't really matter (only the key does),
				// but just in case they start doing anything with it, we'll use "true".
				replacementsDictionary.Add(SuppressOpeningItems, "true");
			}
		}
		void IWizard.RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			RunStarted(automationObject, replacementsDictionary, runKind, customParams);
		}

		/// <summary>
		/// Ensures that only the file name portion of the new <see cref="ProjectItem"/>'s path is used as the
		/// title for the document window.
		/// </summary>
		protected new void ProjectItemFinishedGenerating(ProjectItem projectItem)
		{
			base.ProjectItemFinishedGenerating(projectItem);

			// UNDONE: This may or may not be needed for Orcas. This needs to be tested, and if it is not needed,
			// the following code block should be uncommented.
			//Debug.Assert(TemplateWizardIsOrcasOrLater.HasValue, "RunStarted should have been called before ProjectItemFinishedGenerating is called.");
			//if (TemplateWizardIsOrcasOrLater.GetValueOrDefault())
			//{
			//    return;
			//}

			if (projectItem == null)
			{
				throw new ArgumentNullException("projectItem");
			}

			DTE application = projectItem.DTE;
			if (application == null)
			{
				// If we can't get the DTE, just bail out.
				return;
			}

			Document document = projectItem.Document;
			if (document == null)
			{
				// At the point this is called, the ProjectItem seems to not have the Document associated with it yet.
				// In that case, we need to look through the documents to find it. We'll try ActiveDocument first...
				document = application.ActiveDocument;
				if (document != null && document.ProjectItem != projectItem)
				{
					document = null;
					Documents documents = application.Documents;
					if (documents != null)
					{
						foreach (Document potentialDocument in documents)
						{
							if (potentialDocument.ProjectItem == projectItem)
							{
								document = potentialDocument;
								break;
							}
						}
					}
				}
			}

			if (document != null)
			{
				IOleServiceProvider oleServiceProvider = application as IOleServiceProvider;
				if (oleServiceProvider == null)
				{
					// If we can't get an IOleServiceProvider, just bail out.
					return;
				}

				IVsUIHierarchy uiHierarchy;
				uint itemId;
				IVsWindowFrame windowFrame;
				bool documentIsOpen = VsShellUtilities.IsDocumentOpen(new ServiceProvider(oleServiceProvider), document.FullName, Guid.Empty, out uiHierarchy, out itemId, out windowFrame);
				Debug.Assert(documentIsOpen);

				if (windowFrame != null)
				{
					object pVar;
					ErrorHandler.ThrowOnFailure(uiHierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_Caption, out pVar));
					string caption = pVar as string;
					if (!string.IsNullOrEmpty(caption))
					{
						try
						{
							caption = Path.GetFileName(caption);
						}
						catch (ArgumentException)
						{
							// Ignore any ArgumentException (which can occur if the caption contains characters invalid in a path).
							return;
						}
						// Set the new caption on the backend document.
						ErrorHandler.ThrowOnFailure(uiHierarchy.SetProperty(itemId, (int)__VSHPROPID.VSHPROPID_Caption, caption));
						// Set the new caption on the window.
						ErrorHandler.ThrowOnFailure(windowFrame.SetProperty((int)__VSFPROPID.VSFPROPID_OwnerCaption, caption));
					}
				}
			}
		}
		void IWizard.ProjectItemFinishedGenerating(ProjectItem projectItem)
		{
			ProjectItemFinishedGenerating(projectItem);
		}
	}
}
