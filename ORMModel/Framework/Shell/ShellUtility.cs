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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Modeling.Shell;

namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	/// <summary>
	/// Contains general-purpose utility methods.
	/// </summary>
	[CLSCompliant(false)]
	public static class FrameworkShellUtility
	{
		#region GetLoadedDocuments method
		/// <summary>
		/// Iterate all loaded documents of the given <typeparam name="DocumentType"/>
		/// </summary>
		public static IEnumerable<DocumentType> GetLoadedDocuments<DocumentType>(IServiceProvider serviceProvider) where DocumentType : class
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
						DocumentType docData = Marshal.GetObjectForIUnknown(punkDocData) as DocumentType;
						if (docData != null)
						{
							yield return docData;
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
		#endregion // GetLoadedDocuments method
		#region ResolveLoadedStore method
		private static WeakReference myLatestResolvedStore;
		/// <summary>
		/// Efficiently locate a loaded <see cref="Store"/> from the loaded
		/// documents based on the identifier for that store.
		/// </summary>
		/// <param name="serviceProvider">The service provided used to get
		/// loaded documents.</param>
		/// <param name="storeId">The store identifier to locate.</param>
		/// <returns>The resolved Store, or <see langword="null"/>
		/// if the store could not be found.</returns>
		public static Store ResolveLoadedStore<DocumentType>(IServiceProvider serviceProvider, Guid storeId) where DocumentType : ModelingDocData
		{
			Store retVal = null;
			WeakReference cachedStoreRef = myLatestResolvedStore;
			if (cachedStoreRef != null &&
				null != (retVal = (Store)cachedStoreRef.Target))
			{
				if (retVal.Id == storeId)
				{
					return retVal;
				}
				retVal = null;
			}
			foreach (DocumentType docData in GetLoadedDocuments<DocumentType>(serviceProvider))
			{
				Store testStore = docData.Store;
				if (testStore.Id == storeId)
				{
					retVal = testStore;
					if (cachedStoreRef == null)
					{
						myLatestResolvedStore = new WeakReference(retVal);
					}
					else
					{
						cachedStoreRef.Target = retVal;
					}
					break;
				}
			}
			return retVal;
		}
		#endregion // ResolveLoadedStore method
	}
}
