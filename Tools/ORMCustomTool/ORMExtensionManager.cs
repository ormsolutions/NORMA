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
using System.IO;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	/// <summary>
	/// Extensions are additional named requirements for a given document format.
	/// The extensions are treated as required content within a document format.
	/// UNDONE: In the longer run, individual generators will be asked to ensure that
	/// a given set of extensions are present in a returned document. For now, we
	/// are not treating the document retrieved from the ORM designer as a true generator,
	/// so we're just hacking this in.
	/// </summary>
	internal class ORMExtensionManager
	{
		private string[] myLoadedExtensions;
		private EnvDTE.Document myDocument;
		private EnvDTE.ProjectItem myProjectItem;
		private Stream myStream;
		public ORMExtensionManager(EnvDTE.Document document, Stream stream)
		{
			myDocument = document;
			myStream = stream;
		}
		public ORMExtensionManager(EnvDTE.ProjectItem projectItem)
		{
			myProjectItem = projectItem;
		}
		/// <summary>
		/// Return a sorted array of loaded extensions
		/// </summary>
		public string[] GetLoadedExtensions()
		{
			string[] retVal = myLoadedExtensions;
			if (retVal == null)
			{
				if (myDocument == null && myProjectItem != null)
				{
					myDocument = myProjectItem.Document;
				}

				object documentExtensionManager;
				MethodInfo methodInfo;
				if (null != myDocument &&
					null != (documentExtensionManager = myDocument.Object("ORMExtensionManager")) &&
					null != (methodInfo = documentExtensionManager.GetType().GetMethod("GetLoadedExtensions", Type.EmptyTypes)))
				{
					retVal = methodInfo.Invoke(documentExtensionManager, null) as string[];
					myDocument = null; // No longer needed
				}
				Stream stream = null;
				if (null == retVal)
				{
					// First used the passed in stream
					stream = myStream;

					// Next use an open text document. Note that this will already have provided
					// an extension manager if this is an open ORM designer
					if (stream == null && myDocument != null)
					{
						EnvDTE.TextDocument textDoc = myDocument.Object("TextDocument") as EnvDTE.TextDocument;
						if (textDoc != null)
						{
							// Note that the stream will be closed with the default reader settings of the XmlReader below
							stream = new MemoryStream(Encoding.UTF8.GetBytes(textDoc.StartPoint.CreateEditPoint().GetText(textDoc.EndPoint)), false);
						}
					}

					// Try the file directly from the project item
					if (stream == null && myProjectItem != null)
					{
						// Note that the stream will be closed with the default reader settings of the XmlReader below
						stream = new FileStream(myProjectItem.get_FileNames(0), FileMode.Open);
					}
				}
				if (stream != null)
				{
					// Attempt to open the stream as an Xml file to
					// get the required extensions from the Xml
					string[] namespaces = null;
					int namespaceCount = 0;
					try
					{
						XmlReaderSettings readerSettings = new XmlReaderSettings();
						readerSettings.CloseInput = true;
						using (XmlReader reader = XmlReader.Create(stream, readerSettings))
						{
							reader.MoveToContent();
							if (reader.NodeType == XmlNodeType.Element)
							{
								int attributeCount = reader.AttributeCount;
								if (attributeCount != 0)
								{
									namespaces = new string[attributeCount];
									if (reader.MoveToFirstAttribute())
									{
										do
										{
											if (reader.Prefix == "xmlns" || reader.Name == "xmlns")
											{
												// Note that some of these are standard, not extensions, but it
												// isn't worth the trouble to add extra ORM knowledge here to figure it out
												string value = reader.Value;
												if (!string.IsNullOrEmpty(value))
												{
													namespaces[namespaceCount] = reader.Value;
													++namespaceCount;
												}
											}
										} while (reader.MoveToNextAttribute());
									}
								}
							}
						}
					}
					catch (XmlException)
					{
						// Nothing to do
					}
					finally
					{
						if (myStream != null)
						{
							myStream.Seek(0, SeekOrigin.Begin);
							myStream = null;
						}
					}
					if (namespaceCount != 0)
					{
						if (namespaceCount != namespaces.Length)
						{
							Array.Resize(ref namespaces, namespaceCount);
						}
						retVal = namespaces;
					}
				}
				if (retVal == null)
				{
					retVal = new string[0];
				}
				else
				{
					Array.Sort<string>(retVal);
				}
				myLoadedExtensions = retVal;
			}
			return retVal;
		}
		/// <summary>
		/// Ensure that the current project item has the required extensions loaded.
		/// This is only called after verifying that the current project item does
		/// not satisfy the requirements.
		/// </summary>
		/// <param name="projectItem">The <see cref="EnvDTE.ProjectItem"/> to modify</param>
		/// <param name="extensions">An <see cref="T:ICollection{System.String}"/> of additional required extensions</param>
		public static bool EnsureExtensions(EnvDTE.ProjectItem projectItem, ICollection<string> extensions)
		{
			ServiceProvider provider = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)projectItem.DTE);
			// UNDONE: Localize message strings in here
			if ((int)DialogResult.No == VsShellUtilities.ShowMessageBox(
				provider,
				"Additional extensions are required to support the chosen generators. Would you like to load the required extensions now?",
				"ORM Generator Selection",
				OLEMSGICON.OLEMSGICON_QUERY,
				OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
				OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST))
			{
				return false;
			}
			EnvDTE.Document document = projectItem.Document;
			bool secondPass = false;
			bool tryDocument = true;
			while (tryDocument)
			{
				object documentExtensionManager;
				MethodInfo methodInfo;
				if (null != document &&
					null != (documentExtensionManager = document.Object("ORMExtensionManager")) &&
					null != (methodInfo = documentExtensionManager.GetType().GetMethod("EnsureExtensions", new Type[] { typeof(string[]) })))
				{
					string[] extensionsArray = new string[extensions.Count];
					extensions.CopyTo(extensionsArray, 0);
					methodInfo.Invoke(documentExtensionManager, new object[] { extensionsArray });
					return true;
				}

				if (secondPass)
				{
					return false;
				}
				tryDocument = false;
				secondPass = true;

				// UNDONE: Localize message strings in here
				if ((int)DialogResult.No == VsShellUtilities.ShowMessageBox(
					provider,
					"The .ORM file must be open in the default designer to add extensions. Would you like to open or reopen the document now?",
					"ORM Generator Selection",
					OLEMSGICON.OLEMSGICON_QUERY,
					OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
					OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST))
				{
					return false;
				}

				if (document != null)
				{
					document.Close(EnvDTE.vsSaveChanges.vsSaveChangesPrompt);
					document = projectItem.Document;
				}
				if (document == null)
				{
					projectItem.Open(Guid.Empty.ToString("B")).Visible = true;
					document = projectItem.Document;
					tryDocument = true;
				}
			}
			return false;
		}
	}
}
