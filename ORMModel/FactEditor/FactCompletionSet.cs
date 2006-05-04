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

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.TextManager.Interop;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ObjectModel.Editors;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#endregion

namespace Neumont.Tools.ORM.FactEditor
{
	/// <summary>
	/// A completion set manager for fact editing
	/// </summary>
	public class FactCompletionSet : IVsCompletionSet
	{
		private ORMDesignerPackage myPackage;
		private IVsTextView myTextView;
		private List<ObjectType> myObjectEntries;
		private ORMDesignerDocView myCurrentDocView;
		private ImageList myImageList;
		private IComparer<ObjectType> myComparer;
		private FactType myEditFact;

		/// <summary>
		/// Construct a completion set
		/// </summary>
		/// <param name="package"></param>
		/// <param name="textView"></param>
		[CLSCompliant(false)]
		public FactCompletionSet(ORMDesignerPackage package, IVsTextView textView)
		{
			myTextView = textView;
			myPackage = package;
			IServiceProvider serviceProvider = (IServiceProvider)myPackage;
			IMonitorSelectionService monitor = (IMonitorSelectionService)serviceProvider.GetService(typeof(IMonitorSelectionService));
			monitor.DocumentWindowChanged += new EventHandler<MonitorSelectionEventArgs>(DocumentWindowChangedEvent);
			monitor.SelectionChanged += new EventHandler<MonitorSelectionEventArgs>(SelectionChangedEvent);
			CurrentDocumentView = monitor.CurrentDocumentView as ORMDesignerDocView;
			
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
			if (myCurrentDocView != null)
			{
				IList objectList = (myCurrentDocView.DocData as ModelingDocData).Store.ElementDirectory.GetElements(ObjectType.MetaClassGuid);
				myObjectEntries = new List<ObjectType>();
				foreach (ObjectType ot in objectList)
				{
					myObjectEntries.Add(ot);
				}
				myObjectEntries.Sort(myComparer);
			}
		}

		/// <summary>
		/// Clear out the completion flags
		/// </summary>
		/// <param name="completionStatusFlags"></param>
		/// <returns></returns>
		public static int Reset(out int completionStatusFlags)
		{
			completionStatusFlags = (int)UpdateCompletionFlags.UCS_NAMESCHANGED;
			return VSConstants.S_OK;
		}
		#region IVsCompletionSet Members

		void IVsCompletionSet.Dismiss()
		{
			Dismiss();
		}
		/// <summary>
		/// Implements IVsCompletionSet.Dismiss
		/// </summary>
		protected static void Dismiss()
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
		[CLSCompliant(false)]
		protected static int GetBestMatch(string pszSoFar, int iLength, out int piIndex, out uint pdwFlags)
		{
			Debug.Assert(false); // Only called if UpdateCompletionFlags.CSF_CUSTOMMATCHING is set
			piIndex = 0;
			pdwFlags = 0;
			return VSConstants.E_NOTIMPL;
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
			int hr = VSConstants.S_OK;
			if (iIndex >= myObjectEntries.Count)
			{
				hr = VSConstants.E_INVALIDARG;
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
			int hr = VSConstants.S_OK;
			ppszText = null;
			if (iIndex >= myObjectEntries.Count || iIndex < 0)
			{
				hr = VSConstants.E_INVALIDARG;
				return hr;
			}
			else if (myObjectEntries[iIndex] != null)
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
		[CLSCompliant(false)]
		protected static uint GetFlags()
		{
			return (uint)UpdateCompletionFlags.CSF_HAVEDESCRIPTIONS;
		}

		/// <summary>
		/// Implements IVsCompletionSet.GetImageList
		/// </summary>
		protected int GetImageList(out IntPtr phImages)
		{
			if (myImageList.HandleCreated)
			{
				phImages = myImageList.Handle;
			}
			else
			{
				phImages = IntPtr.Zero;
			}
			return VSConstants.S_OK;
		}
		int IVsCompletionSet.GetImageList(out IntPtr phImages)
		{
			return GetImageList(out phImages);
		}
		/// <summary>
		/// Implements IVsCompletionSet.GetInitialExtent
		/// </summary>
		protected int GetInitialExtent(out int piLine, out int piStartCol, out int piEndCol)
		{
			Debug.Assert(false); // Only called if UpdateCompletionFlags.CSF_INITIALEXTENTKNOWN is set
			piLine = piStartCol = piEndCol = 0;

			int hr = VSConstants.S_OK;

			// get the current line where the cursor is
			hr = myTextView.GetCaretPos(out piLine, out piStartCol);
			if (ErrorHandler.Succeeded(hr))
			{
				piEndCol = piStartCol;
			}
			return hr;
		}
		int IVsCompletionSet.GetInitialExtent(out int piLine, out int piStartCol, out int piEndCol)
		{
			return GetInitialExtent(out piLine, out piStartCol, out piEndCol);
		}

		/// <summary>
		/// Implements IVsCompletionSet.OnCommit
		/// </summary>
		[CLSCompliant(false)]
		protected static int OnCommit(string pszSoFar, int iIndex, int fSelected, ushort cCommit, out string pbstrCompleteWord)
		{
			Debug.Assert(false); // Only called if UpdateCompletionFlags.CSF_CUSTOMCOMMIT is set
			pbstrCompleteWord = pszSoFar;
			return VSConstants.S_OK;
		}
		int IVsCompletionSet.OnCommit(string pszSoFar, int iIndex, int fSelected, ushort cCommit, out string pbstrCompleteWord)
		{
			return OnCommit(pszSoFar, iIndex, fSelected, cCommit, out pbstrCompleteWord);
		}

		#endregion // IVsCompletionSet Members

		#region Properties
		/// <summary>
		/// Get the FactCompletionSet's current DocData.
		/// </summary>
		[CLSCompliant(false)]
		public ORMDesignerDocView CurrentDocumentView
		{
			get
			{
				return myCurrentDocView;
			}
			private set
			{
				ORMDesignerDocView oldView = myCurrentDocView;
				if (oldView != null)
				{
					if (value != null)
					{
						if (object.ReferenceEquals(oldView, value))
						{
							return;
						}
						else if (object.ReferenceEquals(oldView.DocData, value.DocData))
						{
							myCurrentDocView = value;
							return;
						}
					}
					ModelingDocData docData = oldView.DocData as ModelingDocData;
					if (docData != null)
					{
						DetachEventHandlers(docData.Store);
					}
				}
				myCurrentDocView = value;
				if (value != null)
				{
					AttachEventHandlers((value.DocData as ModelingDocData).Store);
				}
			}
		}
		/// <summary>
		/// Get or set the selected fact we are editing.
		/// </summary>
		public FactType EditFact
		{
			get
			{
				return myEditFact;
			}
		}

		/// <summary>
		/// Gets the number of objects in the completion set dropdown
		/// </summary>		
		public int ObjectCount
		{
			get { return myObjectEntries.Count; }
		}
		#endregion

		#region Event Handlers
		private void DocumentWindowChangedEvent(object sender, MonitorSelectionEventArgs e)
		{
			CurrentDocumentView = ((IMonitorSelectionService)sender).CurrentDocumentView as ORMDesignerDocView;
			LoadModelElements();
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
			// we don't want to process Removed objects because it will cause an exception
			// with the undo stack
			if (objectType.IsRemoved)
			{
				return;
			}

			Guid attributeId = e.MetaAttribute.Id;
			if (attributeId == ObjectType.NameMetaAttributeGuid)
			{
				// UNDONE: We could do this a little better. Find the old index,
				// compare the new string to the before/after elements, and RemoveAt
				// followed by a BinarySearch and Insert if needed.
				myObjectEntries.Remove(objectType);
				int newIndex = myObjectEntries.BinarySearch(objectType, myComparer);
				Debug.Assert(newIndex < 0);
				myObjectEntries.Insert(~newIndex, objectType);
			}
		}
		private static readonly Regex regCountPlaces = new Regex(@"{(?<placeHolderNr>\d+)}", RegexOptions.Compiled);
		void SelectionChangedEvent(object sender, MonitorSelectionEventArgs e)
		{
			IVsTextLines textLines = null;
			ErrorHandler.ThrowOnFailure(myTextView.GetBuffer(out textLines));

			FactType fact = null;
			IORMSelectionContainer selectionContainer = e.NewValue as IORMSelectionContainer;
			if (selectionContainer != null)
			{
				ModelingWindowPane pane = selectionContainer as ModelingWindowPane;
				if (pane != null)
				{
					ICollection selectedObjects = pane.GetSelectedComponents();
					foreach (object o in selectedObjects)
					{
						FactType testFact = EditorUtility.ResolveContextFactType(o);
						// Handle selection of multiple elements as long as
						// they all resolve to the same fact
						if (fact == null)
						{
							fact = testFact;
						}
						else if (!object.ReferenceEquals(testFact, fact))
						{
							fact = null;
							break;
						}
					}
				}
			}
			Reading reading = null;
			ReadingOrder readingOrder = null;
			if (fact != null)
			{
				readingOrder = FactType.FindMatchingReadingOrder(fact);
				if (readingOrder != null)
				{
					reading = readingOrder.PrimaryReading;
				}
			}
			string fullReading = "";
			myEditFact = fact;
			if (reading != null)
			{
				RoleBaseMoveableCollection roles = readingOrder.RoleCollection;
				int roleCount = roles.Count;
				fullReading = regCountPlaces.Replace(
					reading.Text,
					delegate(Match m)
					{
						string retval = null;
						string matchText = m.Value;
						int rolePosition = int.Parse(matchText.Substring(1, matchText.Length - 2), CultureInfo.InvariantCulture);
						if (roleCount > rolePosition)
						{
							ObjectType player = roles[rolePosition].Role.RolePlayer;
							if (player != null)
							{
								string refModeString = player.ReferenceModeString;
								if ((refModeString == null || refModeString.Length == 0) && !player.IsValueType)
								{
									retval = string.Format(CultureInfo.InvariantCulture, "[{0}]", player.Name);
								}
								else
								{
									retval = string.Format(CultureInfo.InvariantCulture, "[{0}({1})]", player.Name, refModeString);
								}
							}
							else
							{
								retval = ResourceStrings.ModelReadingEditorMissingRolePlayerText;
							}
						}
						else
						{
							retval = ResourceStrings.ModelReadingEditorMissingRolePlayerText;
						}
						return retval;
					});
			}

			IntPtr initialText = Marshal.StringToBSTR(fullReading);
			int lineLength;
			ErrorHandler.ThrowOnFailure(textLines.GetLengthOfLine(0, out lineLength));
			ErrorHandler.ThrowOnFailure(textLines.ReplaceLines(0, 0, 0, lineLength, initialText, fullReading.Length, null));
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
			#endregion // IComparer<ObjectType> Members
		}
		#endregion // ObjectTypeNameComparer private class
	}
}
