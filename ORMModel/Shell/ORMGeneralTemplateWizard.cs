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
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;

using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	/// <summary>
	/// Works around Visual Studio bugs in order to allow <c>.vstemplate</c> files
	/// to be used for the "<c>General</c>" <c>ProjectType</c>.
	/// </summary>
	[Serializable]
	[TemplateWizardDisallowUserTemplatesSecurity(true)]
	// Given that we can't make a non-nested class 'private', 'internal' is at least better than 'public'.
	internal sealed class ORMGeneralTemplateWizard : IWizard
	{
		// This code is duplicated in Tools/DatabaseImport/ORMDatabaseImportWizard.cs (ORMDatabaseImportGeneralProjectWizard class)
		private static bool? TemplateWizardIsOrcasOrLater;

		/// <summary>
		/// Prevents the pre-Orcas version of <c>Microsoft.VisualStudio.TemplateWizard.Wizard</c> from crashing in <c>OpenNotedItems</c>.
		/// </summary>
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
		public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			const int OrcasMajorVersionNumber = 9;
			const string TemplateWizardAssemblyName = "Microsoft.VisualStudio.TemplateWizard";
			const string SuppressOpeningItems = "$__suppress_opening_items__$";

			bool? templateWizardIsOrcasOrLater = TemplateWizardIsOrcasOrLater;

			// If we haven't yet determined whether we are being called by the pre-Orcas version of TemplateWizard or not, do so now.
			if (!templateWizardIsOrcasOrLater.HasValue)
			{
				System.Reflection.Assembly callingAssembly = System.Reflection.Assembly.GetCallingAssembly();

				Debug.Assert(callingAssembly != null && callingAssembly.FullName.StartsWith(TemplateWizardAssemblyName, StringComparison.Ordinal));

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

		/// <summary>
		/// Ensures that only the file name portion of the new <see cref="ProjectItem"/>'s path is used as the
		/// title for the document window.
		/// </summary>
		public void ProjectItemFinishedGenerating(ProjectItem projectItem)
		{
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

		#region Empty methods
		public void BeforeOpeningFile(ProjectItem projectItem)
		{
			// Do nothing.
		}
		public void ProjectFinishedGenerating(Project project)
		{
			// Do nothing.
		}
		public bool ShouldAddProjectItem(string filePath)
		{
			// Do nothing.
			return true;
		}
		public void RunFinished()
		{
			// Do nothing.
		}
		#endregion // Empty methods
	}
}
