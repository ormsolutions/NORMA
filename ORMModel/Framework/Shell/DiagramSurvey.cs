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
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace Neumont.Tools.Modeling.Shell
{
	#region DiagramSurvey class
	/// <summary>
	/// Provided as a service domain model (no elements or relationships) for displaying diagrams in the model browser
	/// </summary>
	[DomainObjectId("52222B4A-8155-43E9-8EC9-66EB056009F3")]
	public partial class DiagramSurvey : DomainModel, ISurveyNodeProvider, IModelingEventSubscriber
	{
		#region Public constants
		/// <summary>
		/// DiagramSurvey domain model Id.
		/// </summary>
		public static readonly global::System.Guid DomainModelId = new global::System.Guid(0x52222B4A, 0x8155, 0x43E9, 0x8e, 0xc9, 0x66, 0xeb, 0x05, 0x60, 0x09, 0xf3);
		#endregion // Public constants
		#region Member Variables
		private Dictionary<Diagram, DiagramNode> myDiagramToNodeMap;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Required constructor for a domain model
		/// </summary>
		/// <param name="store">The <see cref="Store"/> being populated</param>
		public DiagramSurvey(Store store)
			: base(store, new Guid("52222B4A-8155-43E9-8EC9-66EB056009F3"))
		{
		}
		#endregion // Constructor
		#region DiagramSurveyType enum
		/// <summary>
		/// Enum representing the possible survey question answers
		/// </summary>
		[TypeConverter(typeof(EnumConverter<DiagramSurveyType, IFrameworkServices>))]
		private enum DiagramSurveyType
		{
			/// <summary>
			/// Answer a diagram, or no type. Survey answers always allow a 'not-applicable' answer
			/// </summary>
			Diagram,
		}
		#endregion // DiagramSurveyType enum
		#region DiagramNode class
		/// <summary>
		/// We're showing the diagrams in the survey tree without the diagram itself
		/// knowing about it, so we cannot implement anything directly on diagram. Use
		/// a thin wrapper class to put the diagram in the tree.
		/// </summary>
		private sealed class DiagramNode : ISurveyNode, IAnswerSurveyQuestion<DiagramSurveyType>, IAnswerSurveyDynamicQuestion<DiagramGlyphSurveyType>, IRepresentModelElements
		{
			#region Member variables and constructors
			private readonly Diagram myDiagram;
			private bool myCanEditNameKnown;
			private bool myCanEditName;
			/// <summary>
			/// Create a DiagramNode for the specified Diagram
			/// </summary>
			public DiagramNode(Diagram diagram)
			{
				myDiagram = diagram;
			}
			#endregion // Member variables and constructors
			#region IAnswerSurveyQuestion<DiagramSurveyType> Implementation
			int IAnswerSurveyQuestion<DiagramSurveyType>.AskQuestion()
			{
				return (int)DiagramSurveyType.Diagram;
			}
			#endregion // IAnswerSurveyQuestion<DiagramSurveyType> Implementation
			#region IAnswerSurveyDynamicQuestion<DiagramGlyphSurveyType> Implementation
			int IAnswerSurveyDynamicQuestion<DiagramGlyphSurveyType>.AskQuestion(DiagramGlyphSurveyType answerValues)
			{
				return answerValues.GetDiagramIndex(myDiagram);
			}
			#endregion // IAnswerSurveyDynamicQuestion<DiagramGlyphSurveyType> Implementation
			#region ISurveyNode Implementation
			bool ISurveyNode.IsSurveyNameEditable
			{
				get
				{
					if (!myCanEditNameKnown)
					{
						myCanEditNameKnown = true;
						bool canEdit = true;
						object[] attributes = myDiagram.GetType().GetCustomAttributes(typeof(DiagramMenuDisplayAttribute), false);
						if (attributes.Length > 0)
						{
							// These diagrams have rules to block any name change we apply, so
							// allowing edits is simply confusing.
							canEdit = 0 == (((DiagramMenuDisplayAttribute)attributes[0]).DiagramOption & DiagramMenuDisplayOptions.BlockRename);
						}
						myCanEditName = canEdit;
					}
					return myCanEditName;
				}
			}
			string ISurveyNode.SurveyName
			{
				get
				{
					return myDiagram.Name;
				}
			}
			string ISurveyNode.EditableSurveyName
			{
				get
				{
					return myDiagram.Name;
				}
				set
				{
					Diagram diagram = myDiagram;
					new ElementPropertyDescriptor(diagram, diagram.GetDomainClass().NameDomainProperty, null).SetValue(diagram, value);
				}
			}
			object ISurveyNode.SurveyNodeDataObject
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
					return null;
				}
			}
			#endregion // ISurveyNode Implementation
			#region IRepresentModelElements Implementation
			ModelElement[] IRepresentModelElements.GetRepresentedElements()
			{
				return new ModelElement[] { myDiagram };
			}
			#endregion // IRepresentModelElements Implementation
		}
		#endregion // DiagramNode class
		#region ISurveyNodeProvider Implementation
		/// <summary>
		/// Implements <see cref="ISurveyNodeProvider.GetSurveyNodes"/>.
		/// Returns a list of all diagrams.
		/// </summary>
		protected IEnumerable<object> GetSurveyNodes(object context, object expansionKey)
		{
			if (expansionKey == null)
			{
				Dictionary<Diagram, DiagramNode> diagramToNodeMap = myDiagramToNodeMap ?? (myDiagramToNodeMap = new Dictionary<Diagram, DiagramNode>());
				Store store = Store;
				Partition defaultPartition = store.DefaultPartition;
				foreach (Diagram diagram in Store.ElementDirectory.FindElements<Diagram>(true))
				{
					if (diagram.Partition == defaultPartition)
					{
						DiagramNode node = new DiagramNode(diagram);
						diagramToNodeMap.Add(diagram, node);
						yield return node;
					}
				}
			}
		}
		IEnumerable<object> ISurveyNodeProvider.GetSurveyNodes(object context, object expansionKey)
		{
			return GetSurveyNodes(context, expansionKey);
		}
		#endregion // ISurveyNodeProvider Implementation
		#region IModelingEventSubscriber Implementation
		void IModelingEventSubscriber.ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			if (0 != (reasons & EventSubscriberReasons.SurveyQuestionEvents))
			{
				Store store = Store;
				DomainDataDirectory dataDirectory = store.DomainDataDirectory;
				DomainClassInfo classInfo = dataDirectory.FindDomainClass(Diagram.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(DiagramAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(DiagramRemovedEvent), action);
				DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(Diagram.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(DiagramRenamedEvent), action);
				if (action == EventHandlerAction.Remove)
				{
					myDiagramToNodeMap = null;
				}
			}
		}
		#endregion // IModelingEventSubscriber Implementation
		#region Event handlers
		private void DiagramAddedEvent(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			Store store;
			if (!element.IsDeleted &&
				(store = element.Store).DefaultPartition == element.Partition &&
				null != (eventNotify = (store as IFrameworkServices).NotifySurveyElementChanged))
			{
				Diagram diagram = (Diagram)element;
				DiagramNode node = new DiagramNode(diagram);
				(myDiagramToNodeMap ?? (myDiagramToNodeMap = new Dictionary<Diagram, DiagramNode>())).Add(diagram, node);
				eventNotify.ElementAdded(node, null);
			}
		}
		private void DiagramRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			Dictionary<Diagram, DiagramNode> nodeMap;
			DiagramNode node;
			Diagram diagram;
			Store store;
			if (null != (nodeMap = myDiagramToNodeMap) &&
				(store = element.Store).DefaultPartition == element.Partition &&
				null != (eventNotify = (store as IFrameworkServices).NotifySurveyElementChanged) &&
				nodeMap.TryGetValue(diagram = (Diagram)element, out node))
			{
				nodeMap.Remove(diagram);
				eventNotify.ElementDeleted(node);
			}
		}
		private void DiagramRenamedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			Dictionary<Diagram, DiagramNode> nodeMap;
			DiagramNode node;
			Store store;
			if (!element.IsDeleted &&
				null != (nodeMap = myDiagramToNodeMap) &&
				(store = element.Store).DefaultPartition == element.Partition &&
				null != (eventNotify = (store as IFrameworkServices).NotifySurveyElementChanged) &&
				nodeMap.TryGetValue((Diagram)element, out node))
			{
				eventNotify.ElementRenamed(node);
			}
		}
		#endregion // Event handlers
		#region DiagramGlyphSurveyType class
		/// <summary>
		/// Provide a dynamic set of survey values for the set
		/// of non-abstract diagram types in the store.
		/// </summary>
		private sealed class DiagramGlyphSurveyType : ISurveyDynamicValues
		{
			#region Member Variables
			private Type[] myDiagramTypes;
			private ImageList myImageList;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Required constructor to use generated survey code with dynamic values
			/// </summary>
			public DiagramGlyphSurveyType(Store store)
			{
				DomainClassInfo domainClass = store.DomainDataDirectory.GetDomainClass(typeof(Diagram));
				ReadOnlyCollection<DomainClassInfo> possibleDiagramTypes = store.DomainDataDirectory.FindDomainClass(Diagram.DomainClassId).AllDescendants;
				int possibleDiagramTypeCount = possibleDiagramTypes.Count;
				int concreteDiagramTypeCount = 0;
				for (int i = 0; i < possibleDiagramTypeCount; ++i)
				{
					if (!possibleDiagramTypes[i].ImplementationClass.IsAbstract)
					{
						++concreteDiagramTypeCount;
					}
				}
				Type[] diagramTypes = new Type[concreteDiagramTypeCount];
				Image[] images = new Image[concreteDiagramTypeCount];
				int imageCount = 0;
				for (int i = 0; i < possibleDiagramTypeCount; ++i)
				{
					Type diagramType = possibleDiagramTypes[i].ImplementationClass;
					if (!diagramType.IsAbstract)
					{
						Image image = null;
						object[] attributes = diagramType.GetCustomAttributes(typeof(DiagramMenuDisplayAttribute), false);
						if (attributes.Length > 0)
						{
							image = ((DiagramMenuDisplayAttribute)attributes[0]).BrowserImage;
						}
						if (image != null)
						{
							images[imageCount] = image;
							diagramTypes[imageCount] = diagramType;
							++imageCount;
						}
					}
				}
				if (imageCount < concreteDiagramTypeCount)
				{
					Array.Resize<Type>(ref diagramTypes, imageCount);
					Array.Resize<Image>(ref images, imageCount);
				}
				myDiagramTypes = diagramTypes;
				ImageList imageList = new ImageList();
				imageList.ColorDepth = ColorDepth.Depth32Bit;
				imageList.ImageSize = new Size(16, 16);
				imageList.Images.AddRange(images);
				myImageList = imageList;
			}
			#endregion // Constructor
			#region Public Members
			/// <summary>
			/// Get the index of the diagram's glyph
			/// </summary>
			public int GetDiagramIndex(Diagram diagram)
			{
				return Array.IndexOf<Type>(myDiagramTypes, diagram.GetType());
			}
			/// <summary>
			/// Get the imagelist for the loaded diagrams
			/// </summary>
			public ImageList DiagramImages
			{
				get
				{
					return myImageList;
				}
			}
			#endregion // Public Members
			#region ISurveyDynamicValues Implementation
			int ISurveyDynamicValues.ValueCount
			{
				get
				{
					return myDiagramTypes.Length;
				}
			}
			string ISurveyDynamicValues.GetValueName(int value)
			{
				// The text values are not used
				return "";
			}
			#endregion // ISurveyDynamicValues Implementation
		}
		#endregion // DiagramGlyphSurveyType class
	}
	#endregion // DiagramSurvey class
}
