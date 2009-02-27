#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Matthew Curland. All rights reserved.                        *
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
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.Modeling.Shell;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Drawing;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Modeling.Shell;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region ElementGrouping class
	partial class ElementGrouping : IAnswerSurveyQuestion<SurveyElementType>, IAnswerSurveyQuestion<SurveyErrorState>, IAnswerSurveyQuestion<SurveyQuestionGlyph>, ISurveyNode, ISurveyNodeDropTarget
	{
		#region Survey Fields
		/// <summary>
		/// The expansion key for a group of elements
		/// </summary>
		public static readonly object SurveyExpansionKey = new object();
		#endregion // Survey Fields
		#region IAnswerSurveyQuestion<SurveyElementType> Implementation
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyElementType}.AskQuestion"/>
		/// </summary>
		protected static int AskElementTypeQuestion(object contextElement)
		{
			return (int)SurveyElementType.Grouping;
		}
		int IAnswerSurveyQuestion<SurveyElementType>.AskQuestion(object contextElement)
		{
			return AskElementTypeQuestion(contextElement);
		}
		#endregion // IAnswerSurveyQuestion<SurveyElementType> Implementation
		#region IAnswerSurveyQuestion<SurveyErrorState> Implementation
		int IAnswerSurveyQuestion<SurveyErrorState>.AskQuestion(object contextElement)
		{
			return AskErrorQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyErrorState}.AskQuestion"/>
		/// </summary>
		protected int AskErrorQuestion(object contextElement)
		{
			ElementGroupingSet groupingSet;
			ORMModel model;
			return (null == (groupingSet = GroupingSet) ||
				null == (model = groupingSet.Model)) ?
				-1 :
				(int)(ModelError.HasErrors(this, ModelErrorUses.DisplayPrimary, model.ModelErrorDisplayFilter) ? SurveyErrorState.HasError : SurveyErrorState.NoError);
		}
		#endregion // IAnswerSurveyQuestion<SurveyErrorState> Implementation
		#region ISurveyNode Implementation
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected new object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(ElementGrouping), this);
				return retVal;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>		
		protected static new object SurveyNodeExpansionKey
		{
			get
			{
				return SurveyExpansionKey;
			}
		}
		object ISurveyNode.SurveyNodeExpansionKey
		{
			get
			{
				return SurveyNodeExpansionKey;
			}
		}
		#endregion // ISurveyNode Implementation
		#region ISurveyNodeDropTarget Implementation
		/// <summary>
		/// Implements <see cref="ISurveyNodeDropTarget.OnDragEvent"/>
		/// </summary>
		void OnDragEvent(object contextElement, DragEventType eventType, DragEventArgs args)
		{
			LinkedElementCollection<ElementGroupingType> types = null;
			switch (eventType)
			{
				case DragEventType.Enter:
					foreach (ModelElement element in ExtractElements(args.Data))
					{
						ModelElement normalizedElement = EditorUtility.ResolveContextInstance(element, false) as ModelElement;
						// UNDONE: NestedGrouping
						if (null == normalizedElement && !(normalizedElement is ElementGrouping) && !(normalizedElement is ElementGroupingType))
						{
							continue;
						}
						if (GroupingMembershipInclusion.AddAllowed == GetElementInclusion(normalizedElement, types ?? (types = GroupingTypeCollection)))
						{
							args.Effect = DragDropEffects.Copy;
							return;
						}
					}
					break;
				case DragEventType.Drop:
					using (Transaction t = Store.TransactionManager.BeginTransaction(ResourceStrings.ElementGroupingAddElementTransactionName))
					{
						foreach (ModelElement element in ExtractElements(args.Data))
						{
							ModelElement normalizedElement = EditorUtility.ResolveContextInstance(element, false) as ModelElement;
							// UNDONE: NestedGrouping
							if (null != normalizedElement && !(normalizedElement is ElementGrouping) && !(normalizedElement is ElementGroupingType))
							{
								if (GroupingMembershipInclusion.AddAllowed == GetElementInclusion(normalizedElement, types ?? (types = GroupingTypeCollection)))
								{
									GroupingElementExclusion exclusion = GroupingElementExclusion.GetLink(this, normalizedElement);
									if (exclusion != null)
									{
										// Delete the exclusion. A rule will automatically determine
										// if this turns into a new inclusion or a contradiction.
										exclusion.Delete();
									}
									else
									{
										new GroupingElementInclusion(this, normalizedElement);
									}
								}
							}
						}
						t.Commit();
					}
					break;
			}
		}
		private IEnumerable<ModelElement> ExtractElements(IDataObject data)
		{
			if (data != null)
			{
				if (data.GetDataPresent(typeof(ElementGroupPrototype)))
				{
					ElementGroupPrototype groupPrototype = (ElementGroupPrototype)data.GetData(typeof(ElementGroupPrototype));
					if (groupPrototype != null)
					{
						IElementDirectory elementDir = Store.ElementDirectory;
						foreach (ProtoElement proto in groupPrototype.RootProtoElements)
						{
							ModelElement element;
							if (null != (element = elementDir.FindElement(proto.ElementId)))
							{
								yield return element;
							}
						}
					}
				}
				else
				{
					Store store = Store;
					DomainDataDirectory dataDirectory = store.DomainDataDirectory;
					foreach (string possibleClassName in data.GetFormats(false))
					{
						DomainClassInfo classInfo;
						DomainClassInfo elementClassInfo;
						ModelElement element;
						if (null != (classInfo = dataDirectory.FindDomainClass(possibleClassName)) &&
							null != (element = data.GetData(possibleClassName )as ModelElement) &&
							element.Store == store &&
							((elementClassInfo = element.GetDomainClass()) == classInfo ||
							elementClassInfo.IsDerivedFrom(classInfo)))
						{
							yield return element;
						}
					}
				}
			}
		}
		void ISurveyNodeDropTarget.OnDragEvent(object contextElement, DragEventType eventType, DragEventArgs args)
		{
			OnDragEvent(contextElement, eventType, args);
		}
		#endregion // ISurveyNodeDropTarget Implementation
		#region IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
		int IAnswerSurveyQuestion<SurveyQuestionGlyph>.AskQuestion(object contextElement)
		{
			return AskGlyphQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyQuestionGlyph}.AskQuestion"/>
		/// </summary>
		protected int AskGlyphQuestion(object contextElement)
		{
			return (int)SurveyQuestionGlyph.Grouping;
		}
		#endregion // IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
	}
	#endregion // ElementGrouping class
	#region ElementGroupingType class
	partial class ElementGroupingType : IAnswerSurveyQuestion<SurveyGroupingChildType>, IAnswerSurveyDynamicQuestion<SurveyGroupingTypeGlyph>, ISurveyNode, ISurveyNodeContext
	{
		#region ISurveyNode Implementation
		/// <summary>
		/// Implements <see cref="ISurveyNode.IsSurveyNameEditable"/>
		/// </summary>
		protected static bool IsSurveyNameEditable
		{
			get
			{
				return false;
			}
		}
		bool ISurveyNode.IsSurveyNameEditable
		{
			get
			{
				return IsSurveyNameEditable;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyName"/>
		/// </summary>
		protected string SurveyName
		{
			get
			{
				return DomainTypeDescriptor.GetDisplayName(this.GetType());
			}
		}
		string ISurveyNode.SurveyName
		{
			get
			{
				return SurveyName;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.EditableSurveyName"/>
		/// </summary>
		protected static string EditableSurveyName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}
		string ISurveyNode.EditableSurveyName
		{
			get
			{
				return EditableSurveyName;
			}
			set
			{
				EditableSurveyName = value;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(ElementGroupingType), this);
				return retVal;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>		
		protected static object SurveyNodeExpansionKey
		{
			get
			{
				return null;
			}
		}
		object ISurveyNode.SurveyNodeExpansionKey
		{
			get
			{
				return SurveyNodeExpansionKey;
			}
		}
		#endregion // ISurveyNode Implementation
		#region IAnswerSurveyQuestion<SurveyGroupingChildType> Implementation
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyGroupingChildType}.AskQuestion"/>
		/// </summary>
		protected static int AskGroupingChildTypeQuestion(object contextElement)
		{
			return (int)SurveyGroupingChildType.GroupingType;
		}
		int IAnswerSurveyQuestion<SurveyGroupingChildType>.AskQuestion(object contextElement)
		{
			return AskGroupingChildTypeQuestion(contextElement);
		}
		#endregion // IAnswerSurveyQuestion<SurveyGroupingChildType> Implementation
		#region IAnswerSurveyDynamicQuestion<SurveyGroupingTypeGlyph> Implementation
		int IAnswerSurveyDynamicQuestion<SurveyGroupingTypeGlyph>.AskQuestion(SurveyGroupingTypeGlyph answerValues, object contextElement)
		{
			return answerValues.GetGroupingTypeIndex(this);
		}
		#endregion // IAnswerSurveyDynamicQuestion<SurveyGroupingTypeGlyph> Implementation

		#region ISurveyNodeContext Implementation
		/// <summary>
		/// <see cref="ElementGroupingType"/> instances are displayed inside the parent Grouping
		/// </summary>
		protected object SurveyNodeContext
		{
			get
			{
				return Grouping;
			}
		}
		object ISurveyNodeContext.SurveyNodeContext
		{
			get
			{ 
				return SurveyNodeContext;
			}
		}
		#endregion // ISurveyNodeContext Implementation
	}
	#endregion // ElementGroupingType class
	#region GroupingElementRelationship class
	partial class GroupingElementRelationship : ISurveyNodeReference, IAnswerSurveyQuestion<SurveyGroupingChildType>
	{
		#region ISurveyNodeReference Implementation
		/// <summary>
		/// Implements <see cref="IElementReference.ReferencedElement"/>
		/// </summary>
		protected object ReferencedElement
		{
			get
			{
				return Element;
			}
		}
		object IElementReference.ReferencedElement
		{
			get
			{
				return ReferencedElement;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.SurveyNodeReferenceReason"/>
		/// </summary>
		protected object SurveyNodeReferenceReason
		{
			get
			{
				return this;
			}
		}
		object ISurveyNodeReference.SurveyNodeReferenceReason
		{
			get
			{
				return SurveyNodeReferenceReason;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.SurveyNodeReferenceOptions"/>
		/// </summary>
		protected static SurveyNodeReferenceOptions SurveyNodeReferenceOptions
		{
			get
			{
				return SurveyNodeReferenceOptions.FilterReferencedAnswers | SurveyNodeReferenceOptions.SelectSelf;
			}
		}
		SurveyNodeReferenceOptions ISurveyNodeReference.SurveyNodeReferenceOptions
		{
			get
			{
				return SurveyNodeReferenceOptions;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.UseSurveyNodeReferenceAnswer"/>
		/// </summary>
		protected static bool UseSurveyNodeReferenceAnswer(Type questionType, ISurveyDynamicValues dynamicValues, int answer)
		{
			return questionType != typeof(SurveyErrorState);
		}
		bool ISurveyNodeReference.UseSurveyNodeReferenceAnswer(Type questionType, ISurveyDynamicValues dynamicValues, int answer)
		{
			return UseSurveyNodeReferenceAnswer(questionType, dynamicValues, answer);
		}
		#endregion // ISurveyNodeReference Implementation
		#region IAnswerSurveyQuestion<SurveyGroupingChildType> Implementation
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyGroupingChildType}.AskQuestion"/>
		/// </summary>
		protected static int AskGroupingChildTypeQuestion(object contextElement)
		{
			return (int)SurveyGroupingChildType.ReferencedElement;
		}
		int IAnswerSurveyQuestion<SurveyGroupingChildType>.AskQuestion(object contextElement)
		{
			return AskGroupingChildTypeQuestion(contextElement);
		}
		#endregion // IAnswerSurveyQuestion<SurveyGroupingChildType> Implementation
	}
	#endregion // GroupingElementRelationship class
	#region GroupingElementInclusion class
	partial class GroupingElementInclusion : IAnswerSurveyQuestion<SurveyGroupingReferenceType>
	{
		#region IAnswerSurveyQuestion<SurveyGroupingReferenceType> Implementation
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyGroupingReferenceType}.AskQuestion"/>
		/// </summary>
		protected static int AskGroupingReferenceTypeQuestion(object contextElement)
		{
			return (int)SurveyGroupingReferenceType.Inclusion;
		}
		int IAnswerSurveyQuestion<SurveyGroupingReferenceType>.AskQuestion(object contextElement)
		{
			return AskGroupingReferenceTypeQuestion(contextElement);
		}
		#endregion // IAnswerSurveyQuestion<SurveyGroupingReferenceType> Implementation
	}
	#endregion // GroupingElementInclusion class
	#region GroupingElementExclusion class
	partial class GroupingElementExclusion : IAnswerSurveyQuestion<SurveyGroupingReferenceType>
	{
		#region IAnswerSurveyQuestion<SurveyGroupingReferenceType> Implementation
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyGroupingReferenceType}.AskQuestion"/>
		/// </summary>
		protected static int AskGroupingReferenceTypeQuestion(object contextElement)
		{
			return (int)SurveyGroupingReferenceType.Exclusion;
		}
		int IAnswerSurveyQuestion<SurveyGroupingReferenceType>.AskQuestion(object contextElement)
		{
			return AskGroupingReferenceTypeQuestion(contextElement);
		}
		#endregion // IAnswerSurveyQuestion<SurveyGroupingReferenceType> Implementation
	}
	#endregion // GroupingElementExclusion class
	#region ElementGroupingContainsElementGrouping class
	partial class ElementGroupingContainsElementGrouping : ISurveyNodeReference, IAnswerSurveyQuestion<SurveyGroupingChildType>
	{
		#region ISurveyNodeReference Implementation
		/// <summary>
		/// Implements <see cref="IElementReference.ReferencedElement"/>
		/// </summary>
		protected object ReferencedElement
		{
			get
			{
				return ChildGrouping;
			}
		}
		object IElementReference.ReferencedElement
		{
			get
			{
				return ReferencedElement;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.SurveyNodeReferenceReason"/>
		/// </summary>
		protected object SurveyNodeReferenceReason
		{
			get
			{
				return this;
			}
		}
		object ISurveyNodeReference.SurveyNodeReferenceReason
		{
			get
			{
				return SurveyNodeReferenceReason;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.SurveyNodeReferenceOptions"/>
		/// </summary>
		protected static SurveyNodeReferenceOptions SurveyNodeReferenceOptions
		{
			get
			{
				return SurveyNodeReferenceOptions.FilterReferencedAnswers | SurveyNodeReferenceOptions.InlineExpansion;
			}
		}
		SurveyNodeReferenceOptions ISurveyNodeReference.SurveyNodeReferenceOptions
		{
			get
			{
				return SurveyNodeReferenceOptions;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.UseSurveyNodeReferenceAnswer"/>
		/// </summary>
		protected static bool UseSurveyNodeReferenceAnswer(Type questionType, ISurveyDynamicValues dynamicValues, int answer)
		{
			return questionType != typeof(SurveyErrorState);
		}
		bool ISurveyNodeReference.UseSurveyNodeReferenceAnswer(Type questionType, ISurveyDynamicValues dynamicValues, int answer)
		{
			return UseSurveyNodeReferenceAnswer(questionType, dynamicValues, answer);
		}
		#endregion // ISurveyNodeReference Implementation
		#region IAnswerSurveyQuestion<SurveyGroupingChildType> Implementation
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyGroupingChildType}.AskQuestion"/>
		/// </summary>
		protected static int AskGroupingChildTypeQuestion(object contextElement)
		{
			return (int)SurveyGroupingChildType.NestedGrouping;
		}
		int IAnswerSurveyQuestion<SurveyGroupingChildType>.AskQuestion(object contextElement)
		{
			return AskGroupingChildTypeQuestion(contextElement);
		}
		#endregion // IAnswerSurveyQuestion<SurveyGroupingChildType> Implementation
	}
	#endregion // ElementGroupingContainsElementGrouping class
	#region ElementGroupingIncludesElementGrouping class
	partial class ElementGroupingIncludesElementGrouping : IAnswerSurveyQuestion<SurveyGroupingReferenceType>
	{
		#region IAnswerSurveyQuestion<SurveyGroupingReferenceType> Implementation
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyGroupingReferenceType}.AskQuestion"/>
		/// </summary>
		protected static int AskGroupingReferenceTypeQuestion(object contextElement)
		{
			return (int)SurveyGroupingReferenceType.Inclusion;
		}
		int IAnswerSurveyQuestion<SurveyGroupingReferenceType>.AskQuestion(object contextElement)
		{
			return AskGroupingReferenceTypeQuestion(contextElement);
		}
		#endregion // IAnswerSurveyQuestion<SurveyGroupingReferenceType> Implementation
	}
	#endregion // ElementGroupingIncludesElementGrouping class
	#region ElementGroupingExcludesElementGrouping class
	partial class ElementGroupingExcludesElementGrouping : IAnswerSurveyQuestion<SurveyGroupingReferenceType>
	{
		#region IAnswerSurveyQuestion<SurveyGroupingReferenceType> Implementation
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyGroupingReferenceType}.AskQuestion"/>
		/// </summary>
		protected static int AskGroupingReferenceTypeQuestion(object contextElement)
		{
			return (int)SurveyGroupingReferenceType.Exclusion;
		}
		int IAnswerSurveyQuestion<SurveyGroupingReferenceType>.AskQuestion(object contextElement)
		{
			return AskGroupingReferenceTypeQuestion(contextElement);
		}
		#endregion // IAnswerSurveyQuestion<SurveyGroupingReferenceType> Implementation
	}
	#endregion // ElementGroupingExcludesElementGrouping class
	#region GroupingMembershipContradictionErrorIsForElement class
	partial class GroupingMembershipContradictionErrorIsForElement : ISurveyNodeReference, IAnswerSurveyQuestion<SurveyGroupingChildType>, IAnswerSurveyQuestion<SurveyGroupingReferenceType>
	{
		#region ISurveyNodeReference Implementation
		/// <summary>
		/// Implements <see cref="IElementReference.ReferencedElement"/>
		/// </summary>
		protected object ReferencedElement
		{
			get
			{
				return Element;
			}
		}
		object IElementReference.ReferencedElement
		{
			get
			{
				return ReferencedElement;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.SurveyNodeReferenceReason"/>
		/// </summary>
		protected object SurveyNodeReferenceReason
		{
			get
			{
				return this;
			}
		}
		object ISurveyNodeReference.SurveyNodeReferenceReason
		{
			get
			{
				return SurveyNodeReferenceReason;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.SurveyNodeReferenceOptions"/>
		/// </summary>
		protected static SurveyNodeReferenceOptions SurveyNodeReferenceOptions
		{
			get
			{
				return SurveyNodeReferenceOptions.FilterReferencedAnswers | SurveyNodeReferenceOptions.SelectSelf;
			}
		}
		SurveyNodeReferenceOptions ISurveyNodeReference.SurveyNodeReferenceOptions
		{
			get
			{
				return SurveyNodeReferenceOptions;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.UseSurveyNodeReferenceAnswer"/>
		/// </summary>
		protected static bool UseSurveyNodeReferenceAnswer(Type questionType, ISurveyDynamicValues dynamicValues, int answer)
		{
			return questionType != typeof(SurveyErrorState);
		}
		bool ISurveyNodeReference.UseSurveyNodeReferenceAnswer(Type questionType, ISurveyDynamicValues dynamicValues, int answer)
		{
			return UseSurveyNodeReferenceAnswer(questionType, dynamicValues, answer);
		}
		#endregion // ISurveyNodeReference Implementation
		#region IAnswerSurveyQuestion<SurveyGroupingChildType> Implementation
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyGroupingChildType}.AskQuestion"/>
		/// </summary>
		protected static int AskGroupingChildTypeQuestion(object contextElement)
		{
			return (int)SurveyGroupingChildType.ReferencedElement;
		}
		int IAnswerSurveyQuestion<SurveyGroupingChildType>.AskQuestion(object contextElement)
		{
			return AskGroupingChildTypeQuestion(contextElement);
		}
		#endregion // IAnswerSurveyQuestion<SurveyGroupingChildType> Implementation
		#region IAnswerSurveyQuestion<SurveyGroupingReferenceType> Implementation
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyGroupingReferenceType}.AskQuestion"/>
		/// </summary>
		protected static int AskGroupingReferenceTypeQuestion(object contextElement)
		{
			return (int)SurveyGroupingReferenceType.Contradiction;
		}
		int IAnswerSurveyQuestion<SurveyGroupingReferenceType>.AskQuestion(object contextElement)
		{
			return AskGroupingReferenceTypeQuestion(contextElement);
		}
		#endregion // IAnswerSurveyQuestion<SurveyGroupingReferenceType> Implementation
	}
	#endregion // GroupingMembershipContradictionErrorIsForElement class
	#region SurveyGroupingTypeGlyph class
	/// <summary>
	/// Provide a dynamic set of survey values for the set
	/// of non-abstract <see cref="ElementGroupingType"/>
	/// </summary>
	public sealed class SurveyGroupingTypeGlyph : ISurveyDynamicValues
	{
		#region Member Variables
		private Type[] myGroupingTypes;
		private ImageList myImageList;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Required constructor to use generated survey code with dynamic values
		/// </summary>
		public SurveyGroupingTypeGlyph(Store store)
		{
			DomainClassInfo domainClass = store.DomainDataDirectory.GetDomainClass(typeof(ElementGroupingType));
			ReadOnlyCollection<DomainClassInfo> possibleGroupingTypes = store.DomainDataDirectory.FindDomainClass(ElementGroupingType.DomainClassId).AllDescendants;
			int possibleGroupingTypeCount = possibleGroupingTypes.Count;
			int concreteGroupingTypeCount = 0;
			for (int i = 0; i < possibleGroupingTypeCount; ++i)
			{
				if (!possibleGroupingTypes[i].ImplementationClass.IsAbstract)
				{
					++concreteGroupingTypeCount;
				}
			}
			Type[] groupingTypes = new Type[concreteGroupingTypeCount];
			Image[] images = new Image[concreteGroupingTypeCount];
			int imageCount = 0;
			for (int i = 0; i < possibleGroupingTypeCount; ++i)
			{
				Type diagramType = possibleGroupingTypes[i].ImplementationClass;
				if (!diagramType.IsAbstract)
				{
					Image image = null;
					object[] attributes = diagramType.GetCustomAttributes(typeof(ElementGroupingTypeDisplayAttribute), false);
					if (attributes.Length > 0)
					{
						image = ((ElementGroupingTypeDisplayAttribute)attributes[0]).BrowserImage;
					}
					if (image != null)
					{
						images[imageCount] = image;
						groupingTypes[imageCount] = diagramType;
						++imageCount;
					}
				}
			}
			if (imageCount < concreteGroupingTypeCount)
			{
				Array.Resize<Type>(ref groupingTypes, imageCount);
				Array.Resize<Image>(ref images, imageCount);
			}
			myGroupingTypes = groupingTypes;
			ImageList imageList = new ImageList();
			imageList.ColorDepth = ColorDepth.Depth32Bit;
			imageList.ImageSize = new Size(16, 16);
			imageList.Images.AddRange(images);
			myImageList = imageList;
		}
		#endregion // Constructor
		#region Public Members
		/// <summary>
		/// Get the index of the <see cref="ElementGroupingType"/>'s glyph
		/// </summary>
		public int GetGroupingTypeIndex(ElementGroupingType groupingTypeInstance)
		{
			return Array.IndexOf<Type>(myGroupingTypes, groupingTypeInstance.GetType());
		}
		/// <summary>
		/// Get the index of a <see cref="DomainClassInfo"/> that is a descendent of the
		/// <see cref="DomainClassInfo"/> for <see cref="ElementGroupingType"/>
		/// </summary>
		public int GetGroupingTypeIndex(DomainClassInfo groupingTypeClassInfo)
		{
			return Array.IndexOf<Type>(myGroupingTypes, groupingTypeClassInfo.ImplementationClass);
		}
		/// <summary>
		/// Get the imagelist for the loaded grouping types
		/// </summary>
		public ImageList GroupingTypeImages
		{
			get
			{
				return myImageList;
			}
		}
		/// <summary>
		/// Retrieve the <see cref="SurveyGroupingTypeGlyph"/> instance for the specified <see cref="Store"/>
		/// </summary>
		public static SurveyGroupingTypeGlyph GetStoreInstance(Store store)
		{
			foreach (ISurveyQuestionTypeInfo<Store> questionInfo in ((ISurveyQuestionProvider<Store>)store.GetDomainModel<ORMCoreDomainModel>()).GetSurveyQuestions(store, ElementGrouping.SurveyExpansionKey))
			{
				if (questionInfo.QuestionType == typeof(SurveyGroupingTypeGlyph))
				{
					return (SurveyGroupingTypeGlyph)questionInfo.DynamicQuestionValues;
				}
			}
			Debug.Fail("SurveyGroupingTypeGlyph should be created on demand by an ORMCoreDomainModel");
			return null;
		}
		#endregion // Public Members
		#region ISurveyDynamicValues Implementation
		int ISurveyDynamicValues.ValueCount
		{
			get
			{
				return myGroupingTypes.Length;
			}
		}
		string ISurveyDynamicValues.GetValueName(int value)
		{
			// The text values are not used
			return "";
		}
		#endregion // ISurveyDynamicValues Implementation
	}
	#endregion // SurveyGroupingTypeGlyph class
	#region FreeFormCommand Providers
	partial class ORMCoreDomainModel
	{
		#region FreeForm Commands for ElementGrouping
		private static IFreeFormCommandProvider<Store> myElementGroupingCommands;
		private static IFreeFormCommandProvider<Store> FreeFormElementGroupingCommands
		{
			get
			{
				IFreeFormCommandProvider<Store> retVal = myElementGroupingCommands;
				if (retVal == null)
				{
					retVal = new ElementGroupingCommandProvider();
					myElementGroupingCommands = retVal;
					retVal = myElementGroupingCommands; // Get the current field, but not worth synchronizing
				}
				return retVal;
			}
		}
		private sealed class ElementGroupingCommandProvider : IFreeFormCommandProvider<Store>
		{
			#region IFreeFormCommandProvider Implementation
			int IFreeFormCommandProvider<Store>.GetFreeFormCommandCount(Store context, IFreeFormCommandProvider<Store> targetProvider)
			{
				return 1;
			}
			void IFreeFormCommandProvider<Store>.OnFreeFormCommandStatus(Store context, IFreeFormCommandProvider<Store> targetProvider, MenuCommand command, int commandIndex)
			{
				switch (commandIndex)
				{
					case 0:
						command.Visible = true;
						command.Enabled = true;
						((DynamicStatusMenuCommand)command).Text = ResourceStrings.ElementGroupingAddGroupTransactionName;
						break;
				}
			}
			void IFreeFormCommandProvider<Store>.OnFreeFormCommandExecute(Store context, IFreeFormCommandProvider<Store> targetProvider, int commandIndex)
			{
				ElementGrouping grouping;
				using (Transaction t = context.TransactionManager.BeginTransaction(ResourceStrings.ElementGroupingAddGroupTransactionName))
				{
					grouping = new ElementGrouping(context);
					ReadOnlyCollection<ElementGroupingSet> groupingSets = context.ElementDirectory.FindElements<ElementGroupingSet>();
					grouping.GroupingSet = (groupingSets.Count == 0) ? new ElementGroupingSet(context) : groupingSets[0];
					t.Commit();
				}
				((IORMToolServices)context).NavigateTo(grouping, NavigateToWindow.ModelBrowser);
			}
			#endregion // IFreeFormCommandProvider Implementation
		}
		#endregion // FreeForm Commands for ElementGrouping
	}
	#endregion // FreeFormCommand Providers
}
