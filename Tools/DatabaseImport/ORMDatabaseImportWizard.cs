using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
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

namespace Neumont.Tools.ORM.DatabaseImport
{
	/// <summary>
	/// Wizard interface used when loading a DcilREFromDB template
	/// </summary>	
	public class ORMDatabaseImportWizard : IWizard
	{
		#region Member variables
		private bool myAddToProject = true;
		#endregion // Member variables
		#region IWizard Members
		void IWizard.BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
		{
		}

		void IWizard.ProjectFinishedGenerating(EnvDTE.Project project)
		{
		}

		void IWizard.ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
		{
			

		}

		void IWizard.RunFinished()
		{
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

		void IWizard.RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
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
					//string schemaName = dbConn.Database;
					//if (dbConn is System.Data.SqlClient.SqlConnection)
					//{
						
					//}
					DataProvider provider = manager.GetDataProvider(dataConn.Provider);
					string invariantName = provider.GetProperty("InvariantName") as string;

					IList<string> schemaList = DcilSchema.GetAvailableSchemaNames(dbConn, invariantName);
					string selectedSchema = null;
					switch (schemaList.Count)
					{
						case 0:
							break;
						case 1:
							selectedSchema = schemaList[0];
							break;
						default:
							{
								selectedSchema = SchemaSelector.SelectSchema(serviceProvider, schemaList);
							}
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
					}
				}
			}			
		}

		bool IWizard.ShouldAddProjectItem(string filePath)
		{
			return myAddToProject;
		}

		#endregion
	}
}
