#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Collections;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.TextManager.Interop;
using Northface.Tools.ORM.Shell;
using Northface.Tools.ORM.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#endregion

namespace Northface.Tools.ORM.FactEditor
{
	/// <summary>
	/// A completion set manager for fact editing
	/// </summary>
	public class FactCompletionSet : IVsCompletionSet
	{
		private ORMDesignerPackage myPackage;
		private IVsTextView myTextView;
		private List<ObjectType> myObjectEntries;
		private ORMDesignerDocData myCurrentDocument;
		private ImageList myImageList;
		private IComparer<ObjectType> myComparer;

		/// <summary>
		/// Construct a completion set
		/// </summary>
		/// <param name="package"></param>
		/// <param name="textView"></param>
		public FactCompletionSet(ORMDesignerPackage package, IVsTextView textView)
		{
			myPackage = package;
			myTextView = textView;

			IServiceProvider serviceProvider = (IServiceProvider)myPackage;
			IMonitorSelectionService monitor = (IMonitorSelectionService)serviceProvider.GetService(typeof(IMonitorSelectionService));
			monitor.DocumentWindowChanged += new MonitorSelectionEventHandler(DocumentWindowChangedEvent);
			CurrentDocument = monitor.CurrentDocument as ORMDesignerDocData;
			
			// initialize the comparer used for sorting
			myComparer = new ObjectTypeNameComparer();

			// create image list for intellisense
			InitializeImageList();

			// Create the list of object types in the model
			LoadModelElements();
		}

		private void InitializeImageList()
		{
			myImageList = new ImageList();
			myImageList.ImageSize = new Size(16, 16);
			myImageList.TransparentColor = System.Drawing.Color.Lime;
			myImageList.ImageStream = ResourceStrings.FactEditorIntellisenseImageList;
		}

		private void LoadModelElements()
		{
			IList objectList = myCurrentDocument.Store.ElementDirectory.GetElements(ObjectType.MetaClassGuid);
			IList factList = myCurrentDocument.Store.ElementDirectory.GetElements(FactType.MetaClassGuid);			
			myObjectEntries = new List<ObjectType>();
			foreach (ObjectType ot in objectList)
			{
				myObjectEntries.Add(ot);
			}
			myObjectEntries.Sort(myComparer);
		}

		/// <summary>
		/// Clear out the completion flags
		/// </summary>
		/// <param name="completionStatusFlags"></param>
		/// <returns></returns>
		public int Reset(out int completionStatusFlags)
		{
			completionStatusFlags = (int)UpdateCompletionFlags.UCS_NAMESCHANGED;
			return NativeMethods.S_OK;
		}

		#region IVsCompletionSet Members

		void IVsCompletionSet.Dismiss()
		{
			Dismiss();
		}
		/// <summary>
		/// Implements IVsCompletionSet.Dismiss
		/// </summary>
		protected void Dismiss()
		{
		}

		int IVsCompletionSet.GetBestMatch(string pszSoFar, int iLength, out int piIndex, out uint pdwFlags)
		{
			return GetBestMatch(pszSoFar, iLength, out piIndex, out pdwFlags);
		}
		/// <summary>
		/// Implements IVsCompletionSet.GetBestMatch
		/// Determine the index of the closest matching completion, given 
		/// what has been typed so far.  Note that this is only called if
		/// CSF_CUSTOMMATCHING is set in this completion set's flags.  If
		/// *pdwFlags is set to contain one of the GBM_* flags the default 
		/// matching in the view uses case sensitive comparison.
		/// </summary>
		/// <param name="pszSoFar"></param>
		/// <param name="iLength"></param>
		/// <param name="piIndex"></param>
		/// <param name="pdwFlags"></param>
		/// <returns></returns>
		protected int GetBestMatch(string pszSoFar, int iLength, out int piIndex, out uint pdwFlags)
		{
			Debug.Assert(false); // Only called if UpdateCompletionFlags.CSF_CUSTOMMATCHING is set
			piIndex = 0;
			pdwFlags = 0;
			return NativeMethods.E_NOTIMPL;
		}

		int IVsCompletionSet.GetCount()
		{
			return GetCount();
		}
		/// <summary>
		/// Implements IVsCompletionSet.GetCount
		/// Gets the number of available completions
		/// </summary>
		/// <returns></returns>
		protected int GetCount()
		{
			return myObjectEntries.Count;
		}

		int IVsCompletionSet.GetDescriptionText(int iIndex, out string pbstrDescription)
		{
			return GetDescriptionText(iIndex, out pbstrDescription);
		}
		/// <summary>
		/// Implements IVsCompletionSet.GetDescriptionText. To get this method called GetFlags()
		/// must include UpdateCompletionFlags.CSF_HAVEDESCRIPTIONS
		/// </summary>
		/// <param name="iIndex"></param>
		/// <param name="pbstrDescription"></param>
		/// <returns></returns>
		protected int GetDescriptionText(int iIndex, out string pbstrDescription)
		{
			int hr = NativeMethods.S_OK;
			if (iIndex >= myObjectEntries.Count)
			{
				hr = NativeMethods.E_INVALIDARG;
				pbstrDescription = "";
			}
			else
			{
				// TODO: What should we use for the description text?
				pbstrDescription = "";
			}
			return hr;
		}

		int IVsCompletionSet.GetDisplayText(int iIndex, out string ppszText, int[] piGlyph)
		{
			return GetDisplayText(iIndex, out ppszText, piGlyph);
		}
		/// <summary>
		/// Implements IVsCompletionSet.GetDisplayText
		/// Get the text of a completion, as it is to be displayed in the list.
		/// The pointer returned should be maintained by the completion set
		/// object, and remain valid until final release, or until updated
		/// through IVsTextView::UpdateCompletionStatus
		/// </summary>
		/// <param name="iIndex"></param>
		/// <param name="ppszText"></param>
		/// <param name="piGlyph"></param>
		/// <returns></returns>
		protected int GetDisplayText(int iIndex, out string ppszText, int[] piGlyph)
		{
			int hr = NativeMethods.S_OK;
			if (iIndex >= myObjectEntries.Count)
			{
				hr = NativeMethods.E_INVALIDARG;
				ppszText = "";
			}
			else
			{
				// is it objectified?
				FactType nestedFact = myObjectEntries[iIndex].NestedFactType;
				if (nestedFact != null)
				{
					ppszText = nestedFact.NestingType.Name;
				}
				else
				{
					ppszText = myObjectEntries[iIndex].Name;
				}

				// set the image for this item
				if (piGlyph != null)
				{
					if (nestedFact != null)
					{
						// This is an objectified fact
						piGlyph[0] = 2;
					}
					else if (myObjectEntries[iIndex].IsValueType)
					{
						piGlyph[0] = 1;
					}
					else
					{
						// this is just an entity type
						piGlyph[0] = 0;
					}
				}
			}

			return hr;
		}

		uint IVsCompletionSet.GetFlags()
		{
			return GetFlags();
		}
		/// <summary>
		/// Implements IVsCompletionSet.GetFlags
		/// Flags indicating specific behaviors of this completion set (CSF_* in textmgr.idl)
		/// </summary>
		/// <returns></returns>
		protected uint GetFlags()
		{
			return (uint)UpdateCompletionFlags.CSF_HAVEDESCRIPTIONS;
		}

		int IVsCompletionSet.GetImageList(out IntPtr phImages)
		{
			if (myImageList.HandleCreated)
			{
				phImages = myImageList.Handle;
			}
			else
			{
				phImages = IntPtr.Zero;
			}
			return NativeMethods.S_OK;
		}

		int IVsCompletionSet.GetInitialExtent(out int piLine, out int piStartCol, out int piEndCol)
		{
			Debug.Assert(false); // Only called if UpdateCompletionFlags.CSF_INITIALEXTENTKNOWN is set
			piLine = piStartCol = piEndCol = 0;

			int hr = NativeMethods.S_OK;

			// get the current line where the cursor is
			hr = myTextView.GetCaretPos(out piLine, out piStartCol);
			if (NativeMethods.Succeeded(hr))
			{
				piEndCol = piStartCol;
			}
			return hr;
		}

		int IVsCompletionSet.OnCommit(string pszSoFar, int iIndex, int fSelected, ushort cCommit, out string pbstrCompleteWord)
		{
			return OnCommit(pszSoFar, iIndex, fSelected, cCommit, out pbstrCompleteWord);
		}
		/// <summary>
		/// Implements IVsCompletionSet.OnCommit
		/// </summary>
		/// <param name="pszSoFar"></param>
		/// <param name="iIndex"></param>
		/// <param name="fSelected"></param>
		/// <param name="cCommit"></param>
		/// <param name="pbstrCompleteWord"></param>
		/// <returns></returns>
		protected int OnCommit(string pszSoFar, int iIndex, int fSelected, ushort cCommit, out string pbstrCompleteWord)
		{
			Debug.Assert(false); // Only called if UpdateCompletionFlags.CSF_CUSTOMCOMMIT is set
			pbstrCompleteWord = pszSoFar;
			return NativeMethods.S_OK;
		}

		#endregion

		#region Properties
		private ORMDesignerDocData CurrentDocument
		{
			set
			{
				if (myCurrentDocument != null)
				{
					if (value != null && object.ReferenceEquals(myCurrentDocument, value))
					{
						return;
					}
					DetachEventHandlers(myCurrentDocument.Store);
				}
				myCurrentDocument = value;
				if (value != null)
				{
					AttachEventHandlers(myCurrentDocument.Store);
				}
			}
		}
#endregion

		#region Event Handlers
		private void DocumentWindowChangedEvent(object sender, MonitorSelectionEventArgs e)
		{
			CurrentDocument = ((IMonitorSelectionService)sender).CurrentDocument as ORMDesignerDocData;
		}

		private void AttachEventHandlers(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			MetaClassInfo classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasReading.MetaRelationshipGuid);

			// Track ObjectType changes
			classInfo = dataDirectory.FindMetaRelationship(ModelHasObjectType.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(ObjectTypeAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(ObjectTypeRemovedEvent));

			classInfo = dataDirectory.FindMetaClass(ObjectType.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Add(classInfo, new ElementAttributeChangedEventHandler(ObjectTypeChangedEvent));
		}

		private void DetachEventHandlers(Store store)
		{
			if (store == null || store.Disposed)
			{
				return; // Bail out
			}
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			MetaClassInfo classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasReading.MetaRelationshipGuid);

			// Track ObjectType changes
			classInfo = dataDirectory.FindMetaRelationship(ModelHasObjectType.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(ObjectTypeAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(ObjectTypeRemovedEvent));

			classInfo = dataDirectory.FindMetaClass(ObjectType.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Remove(classInfo, new ElementAttributeChangedEventHandler(ObjectTypeChangedEvent));
		}

		private void ObjectTypeAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ModelHasObjectType link = e.ModelElement as ModelHasObjectType;
			ObjectType objectType = link.ObjectTypeCollection;
			Debug.Assert(objectType != null);

			// Add the object to our list of valid objects, BinarySearch saves an extra Sort
			int newIndex = myObjectEntries.BinarySearch(objectType, myComparer);
			if (newIndex < 0)
			{
				myObjectEntries.Insert(~newIndex, objectType);
			}
			else
			{
				myObjectEntries[newIndex] = objectType;
			}
		}

		private void ObjectTypeRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ModelHasObjectType link = e.ModelElement as ModelHasObjectType;
			ObjectType objectType = link.ObjectTypeCollection;
			Debug.Assert(objectType != null);

			// Remove the object from our list of valid objects if it exists
			if (myObjectEntries.Contains(objectType))
			{
				myObjectEntries.Remove(objectType);
			}
		}
		private void ObjectTypeChangedEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			ObjectType objectType = e.ModelElement as ObjectType;
			Debug.Assert(objectType != null);

			Guid attributeId = e.MetaAttribute.Id;
			if (attributeId == ObjectType.NameMetaAttributeGuid)
			{
				string oldName = (string)e.OldValue;
				string newName = (string)e.NewValue;
				// UNDONE: We could do this a little better. Find the old index,
				// compare the new string to the before/after elements, and RemoveAt
				// followed by a BinarySearch and Insert if needed.
				myObjectEntries.Remove(objectType);
				int newIndex = myObjectEntries.BinarySearch(objectType, myComparer);
				Debug.Assert(newIndex < 0);
				myObjectEntries.Insert(~newIndex, objectType);
			}
		}
		#endregion // Event Handlers

		#region ObjectTypeNameComparer private class
		/// <summary>
		/// Compare ObjectType objects by their name
		/// </summary>
		private class ObjectTypeNameComparer : IComparer<ObjectType>
		{
			/// <summary>
			/// Default constructor
			/// </summary>
			public ObjectTypeNameComparer()
			{
			}

			#region IComparer<ObjectType> Members

			int IComparer<ObjectType>.Compare(ObjectType x, ObjectType y)
			{
				return x.Name.CompareTo(y.Name);
			}

			bool IComparer<ObjectType>.Equals(ObjectType x, ObjectType y)
			{
				return object.ReferenceEquals(x, y);
			}

			int IComparer<ObjectType>.GetHashCode(ObjectType obj)
			{
				return obj.GetHashCode();
			}

			#endregion // IComparer<ObjectType> Members
		}
		#endregion // ObjectTypeNameComparer private class
	}
}
