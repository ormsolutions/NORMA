#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
* Copyright � ORM Solutions, LLC. All rights reserved.                        *
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

// Turn this on to block hiding of shapes implied by an objectification pattern
//#define SHOW_IMPLIED_SHAPES
// Turn this on to show a fact shape instead of a subtype link for all SubtypeFacts
//#define SHOW_FACTSHAPE_FOR_SUBTYPE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ORMSolutions.ORMArchitect.Core.Load;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Framework.Diagrams.Design;
using ORMSolutions.ORMArchitect.Framework.Shell;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	/// <summary>
	/// A callback delegate to use during shape placement. Used with <see cref="M:ORMDiagram.PlaceElementOnDiagram"/>
	/// and <see cref="M:ORMDiagram.FixupRelatedLinks"/>
	/// </summary>
	/// <param name="element">The placed element</param>
	/// <param name="newShape">The newly created shape element</param>
	public delegate void FixupNewShape(ModelElement element, ShapeElement newShape);
	/// <summary>
	/// Implement this interface on any shape (generally a link shape) that is auto-created
	/// and also selectable. If this is set, then the element is filtered out of any drag/drop
	/// or copy of shapes.
	/// </summary>
	public interface IAutoCreatedSelectableShape
	{
	}
	[DiagramMenuDisplay(DiagramMenuDisplayOptions.Required | DiagramMenuDisplayOptions.AllowMultiple, typeof(ORMDiagram), "Diagram.MenuDisplayName", "Diagram.TabImage", "Diagram.BrowserImage", NestedDiagramInitializerTypeName="DiagramInitializer")]
	public partial class ORMDiagram : IProxyDisplayProvider, IMergeElements, IStickyObjectDiagram, IInvalidateDisplay, IORMExtendableElement, IVerbalizeCustomChildren
	{
		#region DiagramInitializer class
		/// <summary>
		/// Perform custom initialization for newly loaded or freshly created diagram.
		/// </summary>
		private class DiagramInitializer : IDiagramInitialization
		{
			#region IDiagramInitialization Implementation
			bool IDiagramInitialization.CreateRequiredDiagrams(Store store)
			{
				// Use the default implementation: create a new diagram of the given type
				// and call InitializeDiagram.
				return false;
			}
			void IDiagramInitialization.InitializeDiagram(Diagram diagram)
			{
				if (null == diagram.Subject)
				{
					Store store = diagram.Store;
					ReadOnlyCollection<ORMModel> models = store.ElementDirectory.FindElements<ORMModel>(false);
					if (models.Count != 0)
					{
						diagram.Associate(models[0]);
					}
				}
				ORMDiagram ormDiagram = (ORMDiagram)diagram;
				if (ormDiagram.AutoPopulateShapes)
				{
					ormDiagram.AutoPopulateShapes = false;
					ORMDesignerCommandManager.AutoLayoutDiagram(diagram, diagram.NestedChildShapes, true);
				}
			}
			#endregion // IDiagramInitialization Implementation
		}
		#endregion // DiagramInitializer class
		#region Constructors
		/// <summary>Constructor.</summary>
		/// <param name="store"><see cref="Store"/> where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public ORMDiagram(Store store, params PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartition : null, propertyAssignments)
		{
			// This constructor calls our other constructor which takes a Partition.
			// All work should be done there rather than here.
		}
		/// <summary>Constructor.</summary>
		/// <param name="partition"><see cref="Partition"/> where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public ORMDiagram(Partition partition, params PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
			//turned snap to grid off because we are aligning the facttypes based
			//on the center of the roles. Since the center of the roles is not necessarily
			//going to be located in alignment on the grid we had to turn this off so facttypes
			//would get properly aligned with other objects.
			base.SnapToGrid = false;
			base.Name = ResourceStrings.DiagramCommandNewPage.Replace("&", "");
		}
		#endregion // Constructors
		#region Auto-invalidate tracking, IInvalidateDisplay implementation
		/// <summary>
		/// Implements <see cref="IInvalidateDisplay.InvalidateRequired()"/>
		/// Call to automatically invalidate the shape during events.
		/// Invalidates during the original event sequence as well as undo and redo.
		/// </summary>
		protected void InvalidateRequired()
		{
			InvalidateRequired(false);
		}
		void IInvalidateDisplay.InvalidateRequired()
		{
			InvalidateRequired();
		}
		/// <summary>
		/// Implements <see cref="IInvalidateDisplay.InvalidateRequired(bool)"/>
		/// Call to automatically invalidate the shape during events.
		/// Invalidates during the original event sequence as well as undo and redo.
		/// </summary>
		/// <param name="refreshBitmap">Value to forward to the Invalidate method's refreshBitmap property during event playback</param>
		protected void InvalidateRequired(bool refreshBitmap)
		{
			long? newValue = ORMShapeDomainModel.GetNewUpdateCounterValue(this, refreshBitmap);
			if (newValue.HasValue)
			{
				UpdateCounter = newValue.Value;
			}
		}
		void IInvalidateDisplay.InvalidateRequired(bool refreshBitmap)
		{
			InvalidateRequired(refreshBitmap);
		}
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for <see cref="ORMBaseShape"/>s.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public static void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(UpdateCounterDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(UpdateRequiredEvent), action);
		}
		private static void UpdateRequiredEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			ORMDiagram diagram = (ORMDiagram)e.ModelElement;
			if (!diagram.IsDeleted)
			{
				diagram.Invalidate(Math.Abs(unchecked((long)e.OldValue - (long)e.NewValue)) != 1L);
			}
		}
		#endregion // Auto-invalidate tracking, IInvalidateDisplay implementation
		#region DragDrop overrides
		/// <summary>
		/// Check to see if <see cref="DiagramDragEventArgs.Data">dragged object</see> is a type that can be dropped on the <see cref="Diagram"/>,
		/// if so change <see cref="DiagramDragEventArgs.Effect"/>.
		/// </summary>
		public override void OnDragOver(DiagramDragEventArgs e)
		{
			IDataObject data = e.Data;
			string[] dataFormats = data.GetFormats();
			if (Array.IndexOf(dataFormats, typeof(ObjectType).FullName) >= 0 ||
				Array.IndexOf(dataFormats, typeof(FactType).FullName) >= 0 ||
				Array.IndexOf(dataFormats, typeof(SetComparisonConstraint).FullName) >= 0 ||
				Array.IndexOf(dataFormats, typeof(SetConstraint).FullName) >= 0 ||
				Array.IndexOf(dataFormats, typeof(ModelNote).FullName) >= 0)
			{
				e.Effect = DragDropEffects.All;
				e.Handled = true;
			}
			else if (Array.IndexOf(dataFormats, typeof(ShapeFreeDataObjectSourceStore).FullName) > 0 &&
				(data.GetData(typeof(ShapeFreeDataObjectSourceStore)) as Store) != Store)
			{
				// Allow shape-free members to be dragged across stores. This does not create
				// shapes, it just duplicates all of the closure elements. This includes
				// ElementGrouping in the core model, but is also available for extensions.
				// The extra data object must be added as a signal when the data object for
				// a shape-free element is created.
				e.Effect = DragDropEffects.All;
				e.Handled = true;
			}
			if (!e.Handled)
			{
				IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
				if (extenders != null)
				{
					for (int i = 0; i < extenders.Length && !e.Handled; ++i)
					{
						extenders[i].OnDragOver(this, e);
					}
				}
			}
			base.OnDragOver(e);
		}
		/// <summary>
		/// Check to see if dragged object is a type that can be dropped on the diagram, if so allow it to be
		/// dropped by calling <see cref="PlaceORMElementOnDiagram"/> to support direct additions of elements from
		/// non-diagram sources, then deferring to the base to do a drag-drop from another element.
		/// </summary>
		public override void OnDragDrop(DiagramDragEventArgs e)
		{
			IDataObject dataObject = e.Data;
			if (dataObject == null)
			{
				return;
			}
			Store store = Store;
			Guid startTransactionId = store.UndoManager.TopmostUndoableTransaction;
			if (PlaceORMElementOnDiagram(dataObject, null, e.MousePosition, ORMPlacementOption.CreateNewShape, null, null))
			{
				e.Effect = DragDropEffects.All;
				e.Handled = true;
			}
			if (!e.Handled)
			{
				IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
				if (extenders != null)
				{
					for (int i = 0; i < extenders.Length && !e.Handled; ++i)
					{
						extenders[i].OnDragDrop(this, e);
					}
				}
			}
			base.OnDragDrop(e);

			// Check for missing extensions
			Dictionary<object, object> propertyBag = store.PropertyBag;
			object missingExtensionsObject;
			Guid[] missingExtensions;
			if (propertyBag.TryGetValue("ORMDiagram.MergeMissingExtensions", out missingExtensionsObject) &&
				null != (missingExtensions = missingExtensionsObject as Guid[]))
			{
				propertyBag.Remove("ORMDiagram.MergeMissingExtensions");
				if (null != (missingExtensions = CopyMergeUtility.GetNonIgnoredMissingExtensions(store, missingExtensions)))
				{
					// Ask the user what should be done in this case
					ExtensionLoader loader = ORMDesignerPackage.ExtensionLoader;
					// Note that we know the extensions are available or they would not elements
					// available in the other store.
					int missingExtensionCount = missingExtensions.Length;
					string[] missingExtensionNamepaces = new string[missingExtensionCount];
					ExtensionModelBinding[] missingExtensionBindings = new ExtensionModelBinding[missingExtensionCount];
					string[] displayNames = new string[missingExtensionCount];
					for (int i = 0; i < missingExtensionCount; ++i)
					{
						displayNames[i] = DomainTypeDescriptor.GetDisplayName((missingExtensionBindings[i] = loader.GetExtensionDomainModel(missingExtensionNamepaces[i] = loader.MapExtensionDomainModelToName(missingExtensions[i])).Value).Type);
					}
					switch ((DialogResult)VsShellUtilities.ShowMessageBox(
						((IORMToolServices)store).ServiceProvider,
						string.Format(
							CultureInfo.CurrentCulture,
							ResourceStrings.CopyMergeExtensionRequiredMessage,
							string.Join(ResourceStrings.CopyMergeExtensionRequiredMessageJoinSeparator, displayNames)),
						ResourceStrings.PackageOfficialName,
						OLEMSGICON.OLEMSGICON_QUERY,
						OLEMSGBUTTON.OLEMSGBUTTON_YESNOCANCEL,
						OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST))
					{
						case DialogResult.Yes:
							VSDiagramView diagramView;
							ORMDesignerDocData docData;
							if (null != (diagramView = ActiveDiagramView as VSDiagramView) &&
								null != (docData = diagramView.DocData as ORMDesignerDocData))
							{
								ExtensionLoader extensionLoader = ORMDesignerPackage.ExtensionLoader;
								Dictionary<string, ExtensionModelBinding> bindings = new Dictionary<string, ExtensionModelBinding>();
								for (int i = 0; i < missingExtensionCount; ++i)
								{
									bindings[missingExtensionNamepaces[i]] = missingExtensionBindings[i];
								}
								extensionLoader.AddRequiredExtensions(store, ref bindings);

								if (store.UndoManager.TopmostUndoableTransaction != startTransactionId)
								{
									store.UndoManager.Undo();
								}

								Stream currentStream = null;
								Stream newStream = null;
								try
								{
									MemoryStream fallbackStream = new MemoryStream();
									(new ORMSerializationEngine(store)).Save(fallbackStream);
									fallbackStream.Position = 0;
									currentStream = fallbackStream;

									newStream = ExtensionLoader.CleanupStream(currentStream, extensionLoader.StandardDomainModels, bindings != null ? bindings.Values : null, null);
									docData.ReloadFromStream(newStream, currentStream);
								}
								finally
								{
									if (currentStream != null)
									{
										currentStream.Dispose();
									}
									if (newStream != null)
									{
										newStream.Dispose();
									}
								}
							}
							break;
						case DialogResult.No:
							// Cache the modified missing extensions
							CopyMergeUtility.CacheIgnoredMissingExtensions(store, missingExtensions);
							break;
					}
				}
			}
		}
		/// <summary>
		/// Place a new shape for an existing element onto this diagram
		/// </summary>
		/// <param name="dataObject">The dataObject containing the element to place. If this is set, elementToPlace must be null.</param>
		/// <param name="elementToPlace">The the element to place. If this is set, dataObject must be null.</param>
		/// <param name="elementPosition">An initial position for the element</param>
		/// <param name="placementOption">Controls the actions by this method</param>
		/// <param name="beforeStandardFixupCallback">A <see cref="FixupNewShape"/> callback used to configure the shape before standard processing is applied</param>
		/// <param name="afterStandardFixupCallback">A <see cref="FixupNewShape"/> callback used to configure the shape after standard processing is applied</param>
		/// <returns>true if the element was placed</returns>
		public bool PlaceORMElementOnDiagram(IDataObject dataObject, ModelElement elementToPlace, PointD elementPosition, ORMPlacementOption placementOption, FixupNewShape beforeStandardFixupCallback, FixupNewShape afterStandardFixupCallback)
		{
			Debug.Assert((dataObject == null) ^ (elementToPlace == null), "Pass in dataObject or elementToPlace");
			bool retVal = false;
			ObjectType objectType = null;
			FactType factType = null;
			SetComparisonConstraint setComparisonConstraint = null;
			SetConstraint setConstraint = null;
			ModelNote modelNote = null;
			ModelElement element = null;
			LinkedElementCollection<FactType> verifyFactTypeList = null;
			Store store = Store;
			IList<ModelElement> closureElements = null;
			Store sourceStore;
			if (null != (objectType = (dataObject == null) ? elementToPlace as ObjectType : dataObject.GetData(typeof(ObjectType)) as ObjectType))
			{
				factType = objectType.NestedFactType;
				if (factType != null)
				{
					objectType = null;
					element = factType;
				}
				else
				{
					element = objectType;
				}
			}
			else if (null != (factType = (dataObject == null) ? elementToPlace as FactType : dataObject.GetData(typeof(FactType)) as FactType))
			{
				// SubtypeFacts are not placed, they appear as links between placed shapes and are fixed up at that point
				if (!(factType is SubtypeFact))
				{
					element = factType;
				}
			}
			else if (null != (setComparisonConstraint = (dataObject == null) ? elementToPlace as SetComparisonConstraint : dataObject.GetData(typeof(SetComparisonConstraint)) as SetComparisonConstraint))
			{
				verifyFactTypeList = setComparisonConstraint.FactTypeCollection;
				element = setComparisonConstraint;
			}
			else if (null != (setConstraint = (dataObject == null) ? elementToPlace as SetConstraint : dataObject.GetData(typeof(SetConstraint)) as SetConstraint))
			{
				verifyFactTypeList = setConstraint.FactTypeCollection;
				element = setConstraint;
			}
			else if (null != (modelNote = (dataObject == null) ? elementToPlace as ModelNote : dataObject.GetData(typeof(ModelNote)) as ModelNote))
			{
				element = modelNote;
			}

			if (element != null)
			{
				closureElements = (verifyFactTypeList == null || VerifyCorrespondingFactTypes(verifyFactTypeList, null)) ? new ModelElement[] { element } : null;
			}
			else if (dataObject != null &&
				null != (sourceStore = dataObject.GetData(typeof(ShapeFreeDataObjectSourceStore)) as Store) &&
				store != sourceStore)
			{
				// Support cross-store drag drop of specially tagged elements with no associated shape.
				// For the core model this is the ElementGrouping type, so there will always be something here.
				// This list is retrieved from the source store in case the target store does not have the
				// extension loaded for the shape free element.
				List<ModelElement> shapeFreeData = null;
				IShapeFreeDataObjectProvider[] shapeFreeProviders = ((IFrameworkServices)sourceStore).GetTypedDomainModelProviders<IShapeFreeDataObjectProvider>();
				for (int i = 0; i < shapeFreeProviders.Length; ++i)
				{
					Type[] dataObjectTypes = shapeFreeProviders[i].ShapeFreeDataObjectTypes;
					for (int j = 0; j < dataObjectTypes.Length; ++j)
					{
						Type testType = dataObjectTypes[j];
						ModelElement testElement;
						if (null != (testElement = dataObject.GetData(testType) as ModelElement) &&
							store != testElement.Store)
						{
							if (shapeFreeData == null)
							{
								closureElements = shapeFreeData = new List<ModelElement>();
								closureElements.Add(element = testElement);
							}
							else
							{
								shapeFreeData.Add(testElement);
							}
						}
					}
				}
			}

			if (closureElements != null)
			{
				sourceStore = element.Store;
				IDictionary<Guid, IClosureElement> copyClosure = null;
				ICopyClosureManager closureManager = null;
				bool crossStoreCopy;
				if ((crossStoreCopy = store != sourceStore) &&
					(null == (closureManager = ((IFrameworkServices)sourceStore).CopyClosureManager) ||
					null == (copyClosure = closureManager.GetCopyClosure(closureElements, crossStoreCopy ? store : null))))
				{
					return false;
				}
				retVal = true;
				bool storeChange = false;

				using (Transaction transaction = store.TransactionManager.BeginTransaction(ResourceStrings.DropShapeTransactionName))
				{
					bool clearContext;
					if (clearContext = !elementPosition.IsEmpty)
					{
						DropTargetContext.Set(transaction.TopLevelTransaction, Id, elementPosition, null);
					}
					Dictionary<object, object> topLevelContextInfo = null;
					object placementKey;
					bool tightenPlacementKey = false;
					switch (placementOption)
					{
						case ORMPlacementOption.AllowMultipleShapes:
							placementKey = MultiShapeUtility.AllowMultipleShapes;
							break;
						case ORMPlacementOption.CreateNewShape:
							placementKey = MultiShapeUtility.ForceMultipleShapes;
							tightenPlacementKey = true;
							break;
						default:
							placementKey = null;
							break;
					}
					if (placementKey != null)
					{
						(topLevelContextInfo = transaction.TopLevelTransaction.Context.ContextInfo).Add(placementKey, null);
					}
					IDictionary<Guid, ModelElement> integratedElements = null;
					if (crossStoreCopy)
					{
						// Integrate elements and translate to the target store
						ORMModel model = (ORMModel)ModelElement;
						CopyClosureIntegrationResult integrationResult = closureManager.IntegrateCopyClosure(copyClosure, sourceStore, store, new ModelElement[] { model }, true);
						integratedElements = integrationResult.CopiedElements;

						// Track missing references for a message after integration is complete
						Guid[] missingExtensions = integrationResult.MissingExtensionModels;
						if (missingExtensions != null)
						{
							store.PropertyBag["ORMDiagram.MergeMissingExtensions"] = missingExtensions;
						}

						// Translate each closure element to the equivalent element in the other store
						for (int i = 0, count = closureElements.Count; i < count; ++i)
						{
							ModelElement closureElement = closureElements[i];
							if (!integratedElements.TryGetValue(closureElement.Id, out closureElement))
							{
								return false; // Transaction rolls back on dispose
							}
							closureElements[i] = closureElement;
							if (i == 0)
							{
								element = closureElement;
							}
						}

						// Add some context information for view fixup
						ElementGroup elementGroup = new ElementGroup(store);
						elementGroup.Add(element);
						elementGroup.MarkAsRoot(element);
						DesignSurfaceMergeContext.Set(transaction, model, elementGroup);
					}
					ShapeElement shapeElement = FixUpLocalDiagram(element);
					if (clearContext)
					{
						DropTargetContext.Remove(transaction.TopLevelTransaction);
						NodeShape nodeShape;
						if (null != (nodeShape = shapeElement as NodeShape) &&
							nodeShape.Location.IsEmpty)
						{
							// Backup plan if the location doesn't take. This can
							// happen if an element is being automatically created
							// during the drop of another shape element, such as
							// during cross-model drops. In this case, the design
							// surface merge context is set, this shape is not
							// considered root element, and the drop target context
							// location is ignored by ShapeElement.PlaceChildShapeUsingContext
							nodeShape.Location = elementPosition;
						}
					}
					if (shapeElement != null)
					{
						if (tightenPlacementKey)
						{
							// We only want to force create the top level element, not its child elements.
							topLevelContextInfo.Remove(placementKey);
							topLevelContextInfo.Add(placementKey = MultiShapeUtility.AllowMultipleShapes, null);
						}
						// Perform preliminary fixup
						if (null != beforeStandardFixupCallback)
						{
							beforeStandardFixupCallback(element, shapeElement);
						}
						if (factType != null)
						{
							FixupFactType(crossStoreCopy ? (FactType)element : factType, shapeElement as FactTypeShape, false);
						}
						else if (objectType != null)
						{
							FixupObjectType(crossStoreCopy ? (ObjectType)element : objectType, shapeElement as ObjectTypeShape, false);
						}
						else if (setConstraint != null)
						{
							FixupConstraint(crossStoreCopy ? (SetConstraint)element : setConstraint, (ExternalConstraintShape)shapeElement);
						}
						else if (setComparisonConstraint != null)
						{
							FixupConstraint(crossStoreCopy ? (SetComparisonConstraint)element : setComparisonConstraint, (ExternalConstraintShape)shapeElement);
						}
						else if (modelNote != null)
						{
							FixupModelNote(crossStoreCopy ? (ModelNote)element : modelNote, (ModelNoteShape)shapeElement);
						}
						
						// Perform additional fixup
						if (null != afterStandardFixupCallback)
						{
							afterStandardFixupCallback(element, shapeElement);
						}
					}
					if (placementKey != null)
					{
						topLevelContextInfo.Remove(placementKey);
					}
					if (transaction.HasPendingChanges)
					{
						transaction.Commit();
						storeChange = true;
					}
				}
				if (!storeChange && placementOption == ORMPlacementOption.SelectIfNotPlaced)
				{
					DiagramView selectOnView = ActiveDiagramView;
					ShapeElement shape = null;

					foreach (ShapeElement existingShape in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(this, element))
					{
						shape = existingShape;
						break;
					}

					if (selectOnView != null && shape != null)
					{
						selectOnView.Selection.Set(new DiagramItem(shape));
						selectOnView.DiagramClientView.EnsureVisible(new ShapeElement[] { shape });
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Place a new shape for an existing element onto this diagram
		/// </summary>
		/// <param name="elementToPlace">The the element to place.</param>
		/// <param name="elementPosition">An initial position for the element</param>
		/// <param name="placementOption">Controls the actions by this method</param>
		/// <param name="fixupShapeCallback">A <see cref="FixupNewShape"/> callback used to configure the shape</param>
		/// <returns>true if the element was placed</returns>
		public bool PlaceElementOnDiagram(ModelElement elementToPlace, PointD elementPosition, ORMPlacementOption placementOption, FixupNewShape fixupShapeCallback)
		{
			bool retVal = false;
			if (elementToPlace != null)
			{
				using (Transaction transaction = Store.TransactionManager.BeginTransaction(ResourceStrings.DropShapeTransactionName))
				{
					bool clearContext;
					if (clearContext = !elementPosition.IsEmpty)
					{
						DropTargetContext.Set(transaction.TopLevelTransaction, Id, elementPosition, null);
					}
					Dictionary<object, object> topLevelContextInfo = null;
					object placementKey;
					bool tightenPlacementKey = false;
					switch (placementOption)
					{
						case ORMPlacementOption.AllowMultipleShapes:
							placementKey = MultiShapeUtility.AllowMultipleShapes;
							break;
						case ORMPlacementOption.CreateNewShape:
							placementKey = MultiShapeUtility.ForceMultipleShapes;
							tightenPlacementKey = true;
							break;
						default:
							placementKey = null;
							break;
					}
					if (placementKey != null)
					{
						(topLevelContextInfo = transaction.TopLevelTransaction.Context.ContextInfo).Add(placementKey, null);
					}
					ShapeElement shapeElement = FixUpLocalDiagram(elementToPlace);
					if (clearContext)
					{
						DropTargetContext.Remove(transaction.TopLevelTransaction);
					}
					if (shapeElement != null && fixupShapeCallback != null)
					{
						if (tightenPlacementKey)
						{
							// We only want to force create the top level element, not its child elements.
							topLevelContextInfo.Remove(placementKey);
							topLevelContextInfo.Add(placementKey = MultiShapeUtility.AllowMultipleShapes, null);
						}
						fixupShapeCallback(elementToPlace, shapeElement);
					}
					if (placementKey != null)
					{
						topLevelContextInfo.Remove(placementKey);
					}
					if (transaction.HasPendingChanges)
					{
						transaction.Commit();
						retVal = true;
					}
				}
				if (!retVal && placementOption == ORMPlacementOption.SelectIfNotPlaced)
				{
					DiagramView selectOnView = ActiveDiagramView;
					ShapeElement shape = null;

					foreach (ShapeElement existingShape in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(this, elementToPlace))
					{
						shape = existingShape;
						break;
					}

					if (selectOnView != null && shape != null)
					{
						selectOnView.Selection.Set(new DiagramItem(shape));
						selectOnView.DiagramClientView.EnsureVisible(new ShapeElement[] { shape });
						retVal = true;
					}
				}
			}
			return retVal;
		}
		private void FixupFactType(FactType factType, FactTypeShape factTypeShape, bool childShapesMerged)
		{
			if (factTypeShape == null)
			{
				return;
			}
			bool duplicateShape = false;
			Objectification objectification = factType.Objectification;
			ObjectType nestingType = (objectification != null) ? objectification.NestingType : null;
			bool lookForNonDisplayedRelatedTypes = false;
			bool haveNonDisplayedRelatedTypes = false;
			if (childShapesMerged && IsMergingExternalStore)
			{
				// Override this setting if we're merging from an external store, which
				// can produce a merge of a shape that does not have all of the elements
				// from the current store.
				childShapesMerged = false;
			}
			foreach (FactTypeShape testShape in MultiShapeUtility.FindAllShapesForElement<FactTypeShape>(factTypeShape.Diagram, factType))
			{
				if (testShape != factTypeShape)
				{
					duplicateShape = true;
					if (nestingType != null)
					{
						if (lookForNonDisplayedRelatedTypes)
						{
							if (haveNonDisplayedRelatedTypes = testShape.DisplayRelatedTypes != RelatedTypesDisplay.AttachAllTypes)
							{
								break;
							}
						}
						else if (nestingType.IsSubtypeOrSupertype &&
							factTypeShape.DisplayRelatedTypes != RelatedTypesDisplay.AttachAllTypes)
						{
							lookForNonDisplayedRelatedTypes = true;
							if (haveNonDisplayedRelatedTypes = testShape.DisplayRelatedTypes != RelatedTypesDisplay.AttachAllTypes)
							{
								break;
							}
						}
						else
						{
							break;
						}
					}
					else
					{
						break;
					}
				}
			}

			// Make sure the role name shape visibility is correct. Visibility
			// settings do not survive a merge operation, so we need to check
			// this explicitly regardless of merge state.
			factTypeShape.UpdateRoleNameDisplay();
			
			// Handle other shapes related to roles.
			LinkedElementCollection<RoleBase> roleCollection = factType.RoleCollection;
			int roleCount = roleCollection.Count;
			int? unaryRoleIndex = FactType.GetUnaryRoleIndex(roleCollection);
			Role unaryRole = unaryRoleIndex.HasValue ? roleCollection[unaryRoleIndex.Value].Role : null;
			bool impliedFactType = factType.ImpliedByObjectification != null;
			for (int i = 0; i < roleCount; ++i)
			{
				RoleBase roleBase = roleCollection[i];
				Role role = roleBase.Role;

				if (!duplicateShape)
				{
					// Pick up role players
					ReadOnlyCollection<ElementLink> rolePlayerLinks = DomainRoleInfo.GetElementLinks<ElementLink>(role, ObjectTypePlaysRole.PlayedRoleDomainRoleId);
					if (roleBase != role)
					{
						Dictionary<object, object> topLevelContextInfo = role.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
						try
						{
							topLevelContextInfo[ORMDiagram.CreatingRolePlayerProxyLinkKey] = null;
							FixupRelatedLinks(rolePlayerLinks);
						}
						finally
						{
							topLevelContextInfo.Remove(ORMDiagram.CreatingRolePlayerProxyLinkKey);
						}
					}
					else
					{
						FixupRelatedLinks(rolePlayerLinks);
					}

					// Pick up attached constraints
					FixupRelatedLinks(DomainRoleInfo.GetElementLinks<ElementLink>(role, FactSetConstraint.FactTypeDomainRoleId));
					FixupRelatedLinks(DomainRoleInfo.GetElementLinks<ElementLink>(role, FactSetComparisonConstraint.FactTypeDomainRoleId));
				}

				// Get the role value constraint and the link to it.
				RoleHasValueConstraint valueConstraintLink = RoleHasValueConstraint.GetLinkToValueConstraint(role);
				UnaryRoleCardinalityConstraint unaryRoleCardinality = (unaryRole == role) ? unaryRole.Cardinality : null;
				if (!childShapesMerged)
				{
					if (valueConstraintLink != null)
					{
						FixUpLocalDiagram(factTypeShape as ShapeElement, valueConstraintLink.ValueConstraint);
					}

					if (unaryRoleCardinality != null)
					{
						FixUpLocalDiagram(factTypeShape as ShapeElement, unaryRoleCardinality);
					}
				}
				// Role player links are not part of the merge hierarchy, add them for both
				// merge and non-merge cases.
				if (valueConstraintLink != null)
				{
					FixUpLocalDiagram(valueConstraintLink);
				}
			}
			if (!childShapesMerged)
			{
				LinkedElementCollection<ReadingOrder> orders = factType.ReadingOrderCollection;
				if (orders.Count != 0)
				{
					FixUpLocalDiagram(factTypeShape as ShapeElement, orders[0]);
				}
			}
			if (!duplicateShape)
			{
				FixupRelatedLinks(DomainRoleInfo.GetElementLinks<ElementLink>(factType, ModelNoteReferencesFactType.ElementDomainRoleId));
			}
			if (objectification != null && !objectification.IsImplied)
			{
				if (!childShapesMerged)
				{
					ValueConstraint valueConstraint = nestingType.FindValueConstraint(false);
					ObjectTypeCardinalityConstraint cardinalityConstraint = nestingType.Cardinality;
					if (factTypeShape.DisplayAsObjectType)
					{
						if (valueConstraint != null)
						{
							FixUpLocalDiagram(factTypeShape, valueConstraint);
						}
						if (cardinalityConstraint != null)
						{
							FixUpLocalDiagram(factTypeShape, cardinalityConstraint);
						}
					}
					else
					{
						// If this creates an objectified fact type name shape it will fix up
						// its own children.
						FixUpLocalDiagram(factTypeShape as ShapeElement, nestingType);
					}
				}
				if (!duplicateShape || haveNonDisplayedRelatedTypes)
				{
					FixupObjectTypeLinks(nestingType, false);
				}
			}
		}
		private void FixupObjectType(ObjectType objectType, ObjectTypeShape objectTypeShape, bool childShapesMerged)
		{
			if (objectTypeShape == null)
			{
				return;
			}
			bool duplicateShape = false;
			bool lookForNonDisplayedRelatedTypes = false;
			bool haveNonDisplayedRelatedTypes = false;
			foreach (ObjectTypeShape testShape in MultiShapeUtility.FindAllShapesForElement<ObjectTypeShape>(objectTypeShape.Diagram, objectType))
			{
				if (testShape != objectTypeShape)
				{
					duplicateShape = true;
					if (lookForNonDisplayedRelatedTypes)
					{
						if (haveNonDisplayedRelatedTypes = testShape.DisplayRelatedTypes != RelatedTypesDisplay.AttachAllTypes)
						{
							break;
						}
					}
					else if (objectType.IsSubtypeOrSupertype &&
						objectTypeShape.DisplayRelatedTypes != RelatedTypesDisplay.AttachAllTypes)
					{
						lookForNonDisplayedRelatedTypes = true;
						if (haveNonDisplayedRelatedTypes = testShape.DisplayRelatedTypes != RelatedTypesDisplay.AttachAllTypes)
						{
							break;
						}
					}
					else
					{
						break;
					}
				}
			}
			if (!duplicateShape || haveNonDisplayedRelatedTypes)
			{
				FixupObjectTypeLinks(objectType, haveNonDisplayedRelatedTypes);
			}
			if (!childShapesMerged || IsMergingExternalStore)
			{
				// If the shape comes from the local store the source shape should always be
				// in sync. However, if the shape comes from an external store, then it may not
				// have child elements displayed on shapes in this store, so we need to check.
				ValueConstraint valueConstraint;
				ObjectTypeCardinalityConstraint cardinalityConstraint;
				if (null != (valueConstraint = objectType.FindValueConstraint(false)))
				{
					FixUpLocalDiagram(objectTypeShape as ShapeElement, valueConstraint);
				}
				if (null != (cardinalityConstraint = objectType.Cardinality))
				{
					FixUpLocalDiagram(objectTypeShape as ShapeElement, cardinalityConstraint);
				}
			}
		}
		/// <summary>
		/// Helper function for FixupFactType and FixupObjectType
		/// </summary>
		private void FixupObjectTypeLinks(ObjectType objectType, bool supertypeLinksOnly)
		{
			ReadOnlyCollection<ObjectTypePlaysRole> rolePlayerLinks = DomainRoleInfo.GetElementLinks<ObjectTypePlaysRole>(objectType, ObjectTypePlaysRole.RolePlayerDomainRoleId);
			int linksCount = rolePlayerLinks.Count;
			for (int i = 0; i < linksCount; ++i)
			{
				ObjectTypePlaysRole link = rolePlayerLinks[i];
				Role role = link.PlayedRole;
				SubtypeMetaRole subRole;
				SupertypeMetaRole superRole;
				FactType subtypeFact = null;
				if (null != (subRole = role as SubtypeMetaRole))
				{
					subtypeFact = role.FactType;
				}
				else if (supertypeLinksOnly)
				{
					continue;
				}
				else if (null != (superRole = role as SupertypeMetaRole))
				{
					subtypeFact = role.FactType;
				}
				if (subtypeFact != null)
				{
					FixUpLocalDiagram(subtypeFact);
				}
				else
				{
					FixUpLocalDiagram(link);
				}
			}
			if (!supertypeLinksOnly)
			{
				FixupRelatedLinks(DomainRoleInfo.GetElementLinks<ElementLink>(objectType, ModelNoteReferencesObjectType.ElementDomainRoleId));
			}
		}
		private void FixupConstraint(IConstraint constraint, ExternalConstraintShape constraintShape)
		{
			Debug.Assert(constraint is SetComparisonConstraint || constraint is SetConstraint,
				"Only use FixupConstraint for a SetConstraint or SetComparisonConstraint.");

			ModelElement constraintElement = constraint as ModelElement;
			bool duplicateShape = false;
			foreach (ExternalConstraintShape testShape in MultiShapeUtility.FindAllShapesForElement<ExternalConstraintShape>(constraintShape.Diagram, constraintElement))
			{
				if (testShape != constraintShape)
				{
					duplicateShape = true;
					break;
				}
			}

			if (!duplicateShape)
			{
				FixupRelatedLinks(
					DomainRoleInfo.GetElementLinks<ElementLink>(
						constraintElement,
						constraint is SetComparisonConstraint ?
							FactSetComparisonConstraint.SetComparisonConstraintDomainRoleId :
							(0 != (constraint.RoleSequenceStyles & RoleSequenceStyles.ConnectIndividualRoles)) ?
								ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId :
								FactSetConstraint.SetConstraintDomainRoleId),
					delegate(ModelElement link, ShapeElement newShape)
					{
						ExternalConstraintLink linkShape = newShape as ExternalConstraintLink;
						if (linkShape != null)
						{
							FactTypeShape shape = linkShape.AssociatedFactTypeShape as FactTypeShape;
							if (shape != null)
							{
								shape.ConstraintShapeSetChanged(constraint);
							}
						}
					});
			}
		}
		private void FixupModelNote(ModelNote noteElement, ModelNoteShape noteShape)
		{
			bool duplicateShape = false;
			foreach (ModelNoteShape testShape in MultiShapeUtility.FindAllShapesForElement<ModelNoteShape>(noteShape.Diagram, noteElement))
			{
				if (testShape != noteShape)
				{
					duplicateShape = true;
					break;
				}
			}

			if (!duplicateShape)
			{
				FixupRelatedLinks(DomainRoleInfo.GetElementLinks<ElementLink>(noteElement, ModelNoteReferencesModelElement.NoteDomainRoleId));
			}
		}
		/// <summary>
		/// Fixes up the local diagram for each of the links
		/// </summary>
		/// <param name="links">The links</param>
		public void FixupRelatedLinks(ReadOnlyCollection<ElementLink> links)
		{
			FixupRelatedLinks(links, null);
		}
		/// <summary>
		/// Fixes up the local diagram for each of the links
		/// </summary>
		/// <param name="links">The links</param>
		/// <param name="afterFixup">A <see cref="FixupNewShape"/> callback that fires after link fixup is complete</param>
		public void FixupRelatedLinks(ReadOnlyCollection<ElementLink> links, FixupNewShape afterFixup)
		{
			int linksCount = links.Count;
			for (int i = 0; i < linksCount; ++i)
			{
				ElementLink link;
				ShapeElement newChildShape = FixUpLocalDiagram(link = links[i]);
				if (afterFixup != null)
				{
					afterFixup(link, newChildShape);
				}
			}
		}
		/// <summary>
		/// Do the same work as <see cref="Diagram.FixUpDiagram"/> for just
		/// this diagram.  Uses this model as the parent.
		/// </summary>
		/// <param name="newChild">The new element to add</param>
		/// <returns>A newly created child shape</returns>
		public ShapeElement FixUpLocalDiagram(ModelElement newChild)
		{
			return FixUpLocalDiagram(this as ShapeElement, newChild);
		}
		/// <summary>
		/// Do the same work as <see cref="Diagram.FixUpDiagram"/> for just
		/// this diagram
		/// </summary>
		/// <param name="existingParent">A model element with a shape on this diagram</param>
		/// <param name="newChild">The new element to add</param>
		/// <returns>All newly created child shapes for the element</returns>
		public IList<ShapeElement> FixUpLocalDiagram(ModelElement existingParent, ModelElement newChild)
		{
			List<ShapeElement> allChildShapes = new List<ShapeElement>();

			if (existingParent == null || existingParent == ModelElement)
			{
				//fix up using this diagram as the parent
				ShapeElement newChildShape;
				if ((newChildShape = FixUpLocalDiagram(newChild)) != null)
				{
					allChildShapes.Add(newChildShape);
				}
				return allChildShapes;
			}

			//fix up for each shape associated with the model element
			foreach (ShapeElement parentShape in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(this, existingParent))
			{
				ShapeElement newChildShape;
				if ((newChildShape = FixUpLocalDiagram(parentShape, newChild)) != null)
				{
					allChildShapes.Add(newChildShape);
				}
			}

			return allChildShapes;
		}
		/// <summary>
		/// Do the same work as <see cref="Diagram.FixUpDiagram"/> for just
		/// this diagram
		/// </summary>
		/// <param name="existingParent">A shape element on this diagram</param>
		/// <param name="newChild">The new element to add</param>
		/// <returns>A newly created child shape for the element</returns>
		public ShapeElement FixUpLocalDiagram(ShapeElement existingParent, ModelElement newChild)
		{
			if (existingParent == null || existingParent == ModelElement)
			{
				//use this diagram as the parent
				existingParent = this;
			}

			ShapeElement newChildShape;
			if ((newChildShape = existingParent.FixUpChildShapes(newChild)) != null &&
				newChildShape.Diagram == this)
			{
				FixUpDiagramSelection(newChildShape);
				return newChildShape;
			}

			return null;
		}
		#endregion // DragDrop overrides
		#region Toolbox filter strings
		/// <summary>
		/// The filter string used for simple actions
		/// </summary>
		public const string ORMDiagramDefaultFilterString = ORMShapeToolboxHelper.ToolboxFilterString;

		/// <summary>
		/// The filter string used to create an external constraint. Very similar to a
		/// normal action, except the external constraint connector is activated on completion
		/// of the action.
		/// </summary>
		public const string ORMDiagramExternalConstraintFilterString = "ORMDiagramExternalConstraintFilterString";
		/// <summary>
		/// The filter string used to connect role sequences to external constraints
		/// </summary>
		public const string ORMDiagramConnectExternalConstraintFilterString = ORMShapeToolboxHelper.ExternalConstraintConnectorFilterString;
		/// <summary>
		/// The filter string used to create subtype relationships between object types
		/// </summary>
		public const string ORMDiagramCreateSubtypeFilterString = ORMShapeToolboxHelper.SubtypeConnectorFilterString;
		/// <summary>
		/// The filter string used to create an internal constraint. Very similar to a
		/// normal action, except the internal constraint connector is activated on completion
		/// of the action.
		/// </summary>
		public const string ORMDiagramInternalUniquenessConstraintFilterString = "ORMDiagramInternalUniquenessConstraintFilterString";
		/// <summary>
		/// The filter string used to connect role sequences to internal uniqueness constraints
		/// </summary>
		public const string ORMDiagramConnectInternalUniquenessConstraintFilterString = "ORMDiagramConnectInternalUniquenessConstraintFilterString";
		/// <summary>
		/// The filter string used to connect a role to its role player object type
		/// </summary>
		public const string ORMDiagramConnectRoleFilterString = ORMShapeToolboxHelper.RoleConnectorFilterString;
		/// <summary>
		/// The filter string used to create a model note. Very similar to a
		/// normal action, except the model note property editor is activated on
		/// completion of the action.
		/// </summary>
		public const string ORMDiagramModelNoteFilterString = "ORMDiagramModelNoteFilterString";
		/// <summary>
		/// The filter string used to associate a model note with other model element
		/// </summary>
		public const string ORMDiagramConnectModelNoteFilterString = ORMShapeToolboxHelper.ModelNoteConnectorFilterString;
		#endregion // Toolbox filter strings
		#region IStickyObjectDiagram Implementation
		/// <summary>
		/// The StickyObject associated with this diagram.  
		/// </summary>
		[NonSerialized]
		private IStickyObject mySticky;
		/// <summary>
		/// The StickyObject associated with this diagram.  
		/// Implements <see cref="IStickyObjectDiagram.StickyObject"/>
		/// </summary>
		public IStickyObject StickyObject
		{
			get
			{
				return mySticky;
			}
			set
			{
				// Need to account for: going from null to ShapeElement, ShapeElement to null, ShapeElement to ShapeElement
				IStickyObject currentStickyShape;
				IStickyObject incomingStickyShape;

				currentStickyShape = mySticky;
				incomingStickyShape = value;
				if (incomingStickyShape == currentStickyShape)
				{
					if (currentStickyShape != null)
					{
						currentStickyShape.StickyRedraw();
					}
				}
				else
				{
					if (currentStickyShape != null)
					{
						mySticky = null;
						// If the previous StickyObject was a ShapeElement, invalidate it so that it can redraw.
						// This is because a sticky ShapeElement should give a visual indicator that it's active.
						currentStickyShape.StickyRedraw();
					}

					if (incomingStickyShape != null)
					{
						mySticky = value;
						mySticky.StickyInitialize();
					}
				}
			}
		}
		#endregion // IStickyObjectDiagram Implementation
		#region View Fixup Methods
		/// <summary>
		/// Used by <see cref="ShouldAddShapeForElement"/> to map an
		/// element that is not directly displayed shaped one with displayed
		/// elements. The default representation maps a <see cref="Role"/> to
		/// its parent <see cref="FactType"/> and an objectified <see cref="ObjectType"/>
		/// to the objectifying <see cref="FactType"/>.
		/// </summary>
		/// <param name="element">The element to be displayed or redirected.</param>
		/// <returns>The input element, or displayed element.</returns>
		protected virtual ModelElement RedirectToDisplayedElement(ModelElement element)
		{
			Role role;
			ObjectType objectType;
			Objectification objectification;
			if (null != (role = element as Role))
			{
				element = role.FactType;
			}
			else if (null != (objectType = element as ObjectType) &&
				null != (objectification = objectType.Objectification))
			{
				element = objectification.NestedFactType;
			}
			return element;
		}
		/// <summary>
		/// Customizable test to determine if shapes should be
		/// created for elements in a given partition. Allows a
		/// diagram and the elements it is displaying to be in
		/// different partitions. Implementations of this callback
		/// should use the <see cref="Partition.AlternateId"/> property
		/// of the provided partition instead of the <see cref="Partition"/>
		/// instance so that drag-drop from others models is supported.
		/// </summary>
		[NonSerialized]
		public Predicate<Partition> ForeignPartitionTest = null;
		/// <summary>
		/// Check if the element has valid ownership for appearing
		/// on this diagram. Allows ORM elements to be used in structures
		/// outside the core model without appearing on a diagram.
		/// </summary>
		/// <param name="element">An element that may be added to the diagram.</param>
		/// <returns><see langword="true"/> if the element has correct ownership.</returns>
		protected bool VerifyElementOwnership(ModelElement element)
		{
			// Use the partition id instead of the partition itself so that dragging
			// from other models is supported.
			Predicate<Partition> partitionTest = ForeignPartitionTest;
			return (partitionTest != null ?
					partitionTest(element.Partition) :
					element.Partition.AlternateId == this.Partition.AlternateId); // &&
				//!(element is IHasAlternateOwner);
			// UNDONE: AlternateOwner Currently there are no alternate owners generating
			// in the same partition. The current implementation is incomplete because
			// the hierarchy context also needs to check whether the element should be
			// traversed when determining the hierarchy. Filtering at this point in the
			// process is much too late.
		}
		/// <summary>
		/// Called as a result of the FixUpDiagram calls
		/// with the diagram as the first element
		/// </summary>
		/// <param name="element">Added element</param>
		/// <returns>True for items displayed directly on the
		/// surface. Nesting object types are not displayed</returns>
		protected override bool ShouldAddShapeForElement(ModelElement element)
		{
			if (!VerifyElementOwnership(element))
			{
				return false;
			}
			ElementLink link = element as ElementLink;
			SubtypeFact subtypeFact = null;
			ModelElement element1 = null;
			ModelElement element2 = null;
			if (link != null)
			{
				element1 = RedirectToDisplayedElement(DomainRoleInfo.GetSourceRolePlayer(link));
				element2 = RedirectToDisplayedElement(DomainRoleInfo.GetTargetRolePlayer(link));
			}
			else if ((subtypeFact = element as SubtypeFact) != null)
			{
				element1 = RedirectToDisplayedElement(subtypeFact.Subtype);
				element2 = RedirectToDisplayedElement(subtypeFact.Supertype);
			}

			bool isLink = link != null || subtypeFact != null;
			if (isLink && (element1 == null || element2 == null || !ElementHasShape(element1) || !ElementHasShape(element2)))
			{
				return false;
			}
			else if (!isLink && !this.AutoPopulateShapes)
			{
				// Note that this used to be the following, but we can't rely on ActiveDiagramView
				// to be null when the diagram is visible in an inactive window.
				// MSBUG ActiveDiagramView should be null if the containing window is not active.
				//else if (!isLink && (!this.AutoPopulateShapes && this.ActiveDiagramView == null))
				DiagramView activeDiagramView = this.ActiveDiagramView;
				Store store = Store;
				TransactionManager transactionManager = store.TransactionManager;
				if (activeDiagramView == null)
				{
					if (!(transactionManager.InTransaction && AutomatedElementDirective.NeverIgnore == ((IFrameworkServices)store).GetAutomatedElementDirective(element)))
					{
						return false;
					}
				}
				else
				{
					// If the diagram has an active view on it, then we
					// need to make sure that the view belongs to the currently
					// active document. Otherwise, with multiple windows open
					// on the document and different diagrams open in each document,
					// dropping on one document will also add the shape to the
					// diagram in the non-active window.
					IServiceProvider serviceProvider;
					IMonitorSelectionService selectionService;
					object selectionContainer;
					IORMDesignerView currentView;
					Guid diagramDropTargetId;
					IORMToolServices toolServices;
					AutomatedElementDirective directive;
					if (transactionManager.InTransaction &&
						(AutomatedElementDirective.Ignore == (directive = (toolServices = (IORMToolServices)store).GetAutomatedElementDirective(element)) ||
						(directive != AutomatedElementDirective.NeverIgnore &&
						((null == (serviceProvider = toolServices.ServiceProvider) ||
						null == (selectionService = (IMonitorSelectionService)serviceProvider.GetService(typeof(IMonitorSelectionService))) ||
						(null == (currentView = (selectionContainer = selectionService.CurrentSelectionContainer) as IORMDesignerView) &&
						null == (currentView = selectionService.CurrentDocumentView as IORMDesignerView)) ||
						currentView.CurrentDesigner != activeDiagramView ||
						(selectionContainer != currentView && selectionContainer is IORMSelectionContainer)) ||
						((diagramDropTargetId = DropTargetContext.GetTargetDiagramId(transactionManager.CurrentTransaction.TopLevelTransaction)) != Guid.Empty &&
						diagramDropTargetId != this.Id)))))
					{
						return false;
					}
				}
			}
			ObjectType objType;
			FactType factType;
			ObjectTypePlaysRole objectTypePlaysRole;
			SetConstraint setConstraint;
			ExclusionConstraint exclusionConstraint;
			MandatoryConstraint mandatoryConstraint;
			ConstraintRoleSequenceHasRole constraintRole;
			ModelNoteReferencesModelElement noteReference;
			if (null != (factType = element as FactType))
			{
				return ShouldDisplayFactType(factType);
			}
			else if (null != (objectTypePlaysRole = element as ObjectTypePlaysRole))
			{
#if SHOW_IMPLIED_SHAPES
#if !SHOW_FACTSHAPE_FOR_SUBTYPE
				FactType fact = objectTypePlaysRole.PlayedRoleCollection.FactType;
				if (fact is SubtypeFact)
				{
					return false;
				}
#endif // !SHOW_FACTSHAPE_FOR_SUBTYPE
#elif SHOW_FACTSHAPE_FOR_SUBTYPE
				FactType fact = objectTypePlaysRole.PlayedRoleCollection.FactType;
				if (fact.ImpliedByObjectification != null)
				{
					return false;
				}
#else
				FactType fact = objectTypePlaysRole.PlayedRole.FactType;
				if (fact is SubtypeFact)
				{
					return false;
				}
				else if (fact.ImpliedByObjectification != null)
				{
					return true;
				}
#endif
				return ShouldDisplayPartOfReferenceMode(objectTypePlaysRole);
			}
			else if (null != (mandatoryConstraint = element as MandatoryConstraint))
			{
				return !mandatoryConstraint.IsSimple && !mandatoryConstraint.IsImplied;
			}
			else if (null != (setConstraint = element as SetConstraint))
			{
				return !setConstraint.Constraint.ConstraintIsInternal;
			}
			else if (null != (exclusionConstraint = element as ExclusionConstraint))
			{
				return exclusionConstraint.ExclusiveOrMandatoryConstraint == null;
			}
			else if (null != (noteReference = element as ModelNoteReferencesModelElement))
			{
				SetConstraint referencedSetConstraint = noteReference.Element as SetConstraint;
				// Note that note references to internal constraint cannot be added with the current UI, but
				// are valid in the object model. Don't try to link them.
				return referencedSetConstraint == null || !referencedSetConstraint.Constraint.ConstraintIsInternal;
			}
			else if (null != (constraintRole = element as ConstraintRoleSequenceHasRole))
			{
				return null != (setConstraint = constraintRole.ConstraintRoleSequence as SetConstraint) &&
					0 != (((IConstraint)setConstraint).RoleSequenceStyles & RoleSequenceStyles.ConnectIndividualRoles);
			}
			else if (element is SetComparisonConstraint ||
					 element is RoleHasValueConstraint ||
					 element is FactConstraint ||
					 element is ModelNote)
			{
				return true;
			}
			else if (null != (objType = element as ObjectType))
			{
				return ShouldDisplayObjectType(objType);
			}
			IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
			if (extenders != null)
			{
				for (int i = 0; i < extenders.Length; ++i)
				{
					if (extenders[i].ShouldAddShapeForElement(this, element))
					{
						return true;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Determine if an ObjectType element should be displayed on
		/// the diagram.
		/// </summary>
		/// <param name="objectType">The element to test</param>
		/// <returns>true to display, false to not display</returns>
		public bool ShouldDisplayObjectType(ObjectType objectType)
		{
			// We don't ever display a nesting ObjectType, even if the Objectification is not drawn.
			// This also applies to Implicit Boolean ValueTypes (those that are part of a binarized unary).
			if (objectType.NestedFactType == null && !objectType.IsImplicitBooleanValue)
			{
				return ShouldDisplayPartOfReferenceMode(objectType);
			}
			return false;
		}
		/// <summary>
		/// Determine if a FactType element should be displayed on
		/// the diagram.
		/// </summary>
		/// <param name="factType">The element to test</param>
		/// <returns>true to display, false to not display</returns>
		public bool ShouldDisplayFactType(FactType factType)
		{
			if (factType is SubtypeFact)
			{
				return true;
			}
#if !SHOW_IMPLIED_SHAPES
			else if (factType.ImpliedByObjectification != null)
			{
				return true;
			}
#endif // !SHOW_IMPLIED_SHAPES
			return ShouldDisplayPartOfReferenceMode(factType);
		}
		/// <summary>
		/// Function to determine if a fact type, which may be participating
		/// in a reference mode pattern, should be displayed.
		/// </summary>
		private bool ShouldDisplayPartOfReferenceMode(FactType factType)
		{
			foreach (UniquenessConstraint constraint in factType.GetInternalConstraints<UniquenessConstraint>())
			{
				ObjectType entity = constraint.PreferredIdentifierFor;
				// We only consider this to be a collapsible ref mode if its roleplayer is a value type
				LinkedElementCollection<Role> constraintRoles;
				ObjectType rolePlayer;
				Role role;
				RoleProxy proxy;
				FactType impliedFact;
				if (entity != null &&
					1 == (constraintRoles = constraint.RoleCollection).Count &&
					null != (rolePlayer = (role = constraintRoles[0]).RolePlayer) &&
					rolePlayer.IsValueType &&
					(null == (proxy = role.Proxy) ||
					!(null != (impliedFact = proxy.FactType) &&
					impliedFact.ImpliedByObjectification == entity.Objectification)))
				{
					return !ShouldCollapseReferenceMode(entity);
				}
			}
			return true;
		}
		/// <summary>
		/// Function to determine if a role player link, which may be participating
		/// in a reference mode pattern, should be displayed. Defers to test for
		/// the corresponding fact type.
		/// </summary>
		private bool ShouldDisplayPartOfReferenceMode(ObjectTypePlaysRole objectTypePlaysRole)
		{
			Role role = objectTypePlaysRole.PlayedRole;
			FactType factType = role.FactType;
			return (factType != null) ? ShouldDisplayPartOfReferenceMode(factType) : true;
		}
		/// <summary>
		/// Function to determine if an object type, which may be participating
		/// as the value type in the reference mode pattern, should be displayed. The
		/// object type needs to be displayed if any of the reference modes using the
		/// value type has a true ExpandRefMode property or if the object type is
		/// a role player for any other visible role.
		/// </summary>
		private bool ShouldDisplayPartOfReferenceMode(ObjectType objectType)
		{
			if (objectType.IsValueType)
			{
				LinkedElementCollection<Role> playedRoles = objectType.PlayedRoleCollection;
				int playedRoleCount = playedRoles.Count;
				if (playedRoleCount > 0)
				{
					bool partOfCollapsedRefMode = false;
					for (int i = 0; i < playedRoleCount; ++i)
					{
						FactType factType = playedRoles[i].FactType;
						if (factType != null)
						{
							if (ShouldDisplayPartOfReferenceMode(factType))
							{
								partOfCollapsedRefMode = false;
								break;
							}
							else
							{
								partOfCollapsedRefMode = true;
								// Keep going. We may be part of a
								// non-collapsed relationship as well, in
								// which case we need to be visible.
							}
						}
					}
					if (partOfCollapsedRefMode)
					{
						return false;
					}
				}
			}
			return true;
		}
		/// <summary>
		/// Test if the reference mode should be collapsed. Helper function for
		/// ShouldDisplayPartOfReferenceMode implementations.
		/// </summary>
		/// <param name="objectType"></param>
		/// <returns>True if the object type has a collapsed reference mode</returns>
		private bool ShouldCollapseReferenceMode(ObjectType objectType)
		{
			if (!objectType.HasReferenceMode)
			{
				return false;
			}
			bool hasShape = false;
			foreach (ORMBaseShape pel in MultiShapeUtility.FindAllShapesForElement<ORMBaseShape>(this, objectType))
			{
				ObjectTypeShape objectTypeShape;
				ObjectifiedFactTypeNameShape objectifiedFactTypeNameShape;
				if (null != (objectTypeShape = pel as ObjectTypeShape))
				{
					hasShape = true;
					if (objectTypeShape.ExpandRefMode)
					{
						return false;
					}
				}
				else if (null != (objectifiedFactTypeNameShape = pel as ObjectifiedFactTypeNameShape))
				{
					hasShape = true;
					if (objectifiedFactTypeNameShape.ExpandRefMode)
					{
						return false;
					}
				}
			}
			FactType factType;
			if (null != (factType = objectType.NestedFactType))
			{

				foreach (FactTypeShape factTypeShape in MultiShapeUtility.FindAllShapesForElement<FactTypeShape>(this, factType))
				{
					if (factTypeShape.DisplayAsObjectType)
					{
						hasShape = true;
						if (factTypeShape.ExpandRefMode)
						{
							return false;
						}
					}
				}
			}
			return hasShape; // Don't collapse if a shape can't be found.
		}
#if SHOW_FACTSHAPE_FOR_SUBTYPE
		/// <summary>See <see cref="ORMDiagramBase.CreateChildShape"/>.</summary>
		protected override ShapeElement CreateChildShape(ModelElement element)
		{
			if (element is SubtypeFact)
			{
				return new FactTypeShape(this.Partition);
			}
			return base.CreateChildShape(element);
		}
#endif // SHOW_FACTSHAPE_FOR_SUBTYPE
		/// <summary>
		/// Defer to <see cref="IConfigureAsChildShape.ConfiguringAsChildOf"/> on the child shape
		/// </summary>
		/// <param name="child">The child being configured</param>
		/// <param name="createdDuringViewFixup">Whether this shape was created as part of a view fixup</param>
		protected override void OnChildConfiguring(ShapeElement child, bool createdDuringViewFixup)
		{
			IConfigureAsChildShape baseShape;
			if (null != (baseShape = child as IConfigureAsChildShape))
			{
				baseShape.ConfiguringAsChildOf(this, createdDuringViewFixup);
			}
		}
		/// <summary>
		/// Auto shape placement performance when AutoPopulateShapes is turned on (such
		/// as when importing a model with no shape information) is dreadful. Don't auto-place
		/// in this condition.
		/// </summary>
		public override bool ShouldAutoPlaceChildShapes
		{
			get
			{
				return false;	//!AutoPopulateShapes || (Partition != Store.DefaultPartition);
			}
		}
		/// <summary>
		/// Locate an existing shape on this diagram corresponding to this element
		/// </summary>
		/// <param name="element">The element to search</param>
		/// <returns>An existing shape, or null if not found</returns>
		public ShapeElement FindShapeForElement(ModelElement element)
		{
			return FindShapeForElement<ShapeElement>(element, false);
		}
		/// <summary>
		/// Locate an existing shape on this diagram corresponding to this element
		/// </summary>
		/// <param name="element">The element to search</param>
		/// <param name="filterDeleting">Do not return an element where the <see cref="ModelElement.IsDeleting"/> property is true</param>
		/// <returns>An existing shape, or null if not found</returns>
		public ShapeElement FindShapeForElement(ModelElement element, bool filterDeleting)
		{
			return FindShapeForElement<ShapeElement>(element, filterDeleting);
		}
		/// <summary>
		/// Locate an existing typed shape on this diagram corresponding to this element
		/// </summary>
		/// <typeparam name="TShape">The type of the shape to return</typeparam>
		/// <param name="element">The element to search</param>
		/// <returns>An existing shape, or null if not found</returns>
		public TShape FindShapeForElement<TShape>(ModelElement element) where TShape : ShapeElement
		{
			return FindShapeForElement<TShape>(element, false);
		}
		/// <summary>
		/// Locate an existing typed shape on this diagram corresponding to this element
		/// </summary>
		/// <typeparam name="TShape">The type of the shape to return</typeparam>
		/// <param name="element">The element to search</param>
		/// <param name="filterDeleting">Do not return an element where the <see cref="ModelElement.IsDeleting"/> property is true</param>
		/// <returns>An existing shape, or null if not found</returns>
		public TShape FindShapeForElement<TShape>(ModelElement element, bool filterDeleting) where TShape : ShapeElement
		{
			if (element != null)
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(element))
				{
					TShape shape = pel as TShape;
					if (shape != null && shape.Diagram == this && (!filterDeleting || !shape.IsDeleting))
					{
						return shape;
					}
				}
			}
			return null;
		}
		/// <summary>
		/// Determines if an element has a shape on this diagram.
		/// </summary>
		/// <param name="element">The element to check</param>
		/// <returns>true if a shape exists on this diagram for the element</returns>
		public bool ElementHasShape(ModelElement element)
		{
			return ElementHasShape(element, false);
		}
		/// <summary>
		/// Determines if an element has a shape on this diagram.
		/// </summary>
		/// <param name="element">The element to check</param>
		/// <param name="filterDeleting">Do not return an element where the <see cref="ModelElement.IsDeleting"/> property is true</param>
		/// <returns>true if a shape exists on this diagram for the element</returns>
		public bool ElementHasShape(ModelElement element, bool filterDeleting)
		{
			foreach (ShapeElement shapeElement in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(this, element, filterDeleting))
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Setup our routing style.
		/// </summary>
		public override void OnInitialize()
		{
			base.OnInitialize();
			this.RoutingStyle = VGRoutingStyle.VGRouteNone;
		}
		/// <summary>See <see cref="ShapeElement.FixUpChildShapes"/>.</summary>
		public override ShapeElement FixUpChildShapes(ModelElement childElement)
		{
			ShapeElement retVal = null;
			ObjectTypePlaysRole rolePlayerLink = childElement as ObjectTypePlaysRole;
			if (null != rolePlayerLink)
			{
				// Add custom handling for role player link, which is the only backing element
				// that can have link shapes of different types displayed simultaneously for the
				// same backing element.
				Dictionary<object, object> contextInfo = childElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
				object key = ORMDiagram.CreatingRolePlayerProxyLinkKey;
				if (contextInfo.ContainsKey(key))
				{
					// Limit to proxy role player link fixup only
					if (rolePlayerLink.PlayedRole.Proxy != null)
					{
						retVal = MultiShapeUtility.FixUpChildShapes(this, childElement, typeof(RolePlayerProxyLink));
					}
				}
				else
				{
					retVal = MultiShapeUtility.FixUpChildShapes(this, childElement, typeof(RolePlayerLink));
					if (rolePlayerLink.PlayedRole.Proxy != null &&
						!contextInfo.ContainsKey(ORMDiagram.CreatingRolePlayerLinkKey))
					{
						ShapeElement alternateRetVal;
						try
						{
							contextInfo[key] = null;
							alternateRetVal = MultiShapeUtility.FixUpChildShapes(this, childElement, typeof(RolePlayerProxyLink));
							retVal = retVal ?? alternateRetVal;
						}
						finally
						{
							contextInfo.Remove(key);
						}
					}
				}
			}
			else
			{
				retVal = MultiShapeUtility.FixUpChildShapes(this, childElement, null);
			}
			return retVal;
		}
		#endregion // View Fixup Methods
		#region Customize appearance
		/// <summary>
		/// The Brush to use when drawing the background of a sticky object.
		/// </summary>
		public static readonly StyleSetResourceId StickyBackgroundResource = new StyleSetResourceId("ORMArchitect", "StickyBackgroundResource");
		/// <summary>
		/// The Brush to use when drawing the foreground of a sticky object.
		/// </summary>
		public static readonly StyleSetResourceId StickyForegroundResource = new StyleSetResourceId("ORMArchitect", "StickyForegroundResource");
		/// <summary>
		/// The brush or pen used to draw a link decorator as sticky
		/// </summary>
		public static readonly StyleSetResourceId StickyConnectionLineDecoratorResource = new StyleSetResourceId("ORMArchitect", "StickyConnectionLineDecorator");
		/// <summary>
		/// The brush or pen used to draw a link decorator as active. Generally corresponds to the role picker color
		/// </summary>
		public static readonly StyleSetResourceId ActiveConnectionLineDecoratorResource = new StyleSetResourceId("ORMArchitect", "ActiveConnectionLineDecorator");
		/// <summary>
		/// The brush used to draw a link as active. Generally corresponds to the role picker color.
		/// </summary>
		public static readonly StyleSetResourceId ActiveBackgroundResource = new StyleSetResourceId("ORMArchitect", "ActiveBackgroundResource");
		/// <summary>
		/// The brush used to draw the background for an item with errors.
		/// </summary>
		public static readonly StyleSetResourceId ErrorBackgroundResource = new StyleSetResourceId("ORMArchitect", "ErrorBackgroundResource");
		/// <summary>
		/// The brush used to draw the background for an item with errors when the shape is highlighted.
		/// </summary>
		public static readonly StyleSetResourceId HighlightedErrorBackgroundResource = new StyleSetResourceId("ORMArchitect", "HighlightedErrorBackgroundResource");
		/// <summary>
		/// A transparent brush.
		/// </summary>
		public static readonly StyleSetResourceId TransparentBrushResource = new StyleSetResourceId("ORMArchitect", "TransparentBrushResource");

		/// <summary>
		/// Standard override to populate the style set for the shape type
		/// </summary>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);

			IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
			Color stickyBackColor = colorService.GetBackColor(ORMDesignerColor.ActiveConstraint);
			Color stickyForeColor = colorService.GetForeColor(ORMDesignerColor.ActiveConstraint);
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = stickyBackColor;
			classStyleSet.AddBrush(StickyBackgroundResource, DiagramBrushes.ShapeBackgroundSelected, brushSettings);

			brushSettings.Color = stickyForeColor;
			classStyleSet.AddBrush(StickyForegroundResource, DiagramBrushes.ShapeText, brushSettings);

			PenSettings penSettings = new PenSettings();
			penSettings.Color = stickyBackColor;
			penSettings.Width = 1.0F / 72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.Alignment = PenAlignment.Center;
			classStyleSet.AddPen(StickyBackgroundResource, DiagramPens.ShapeHighlightOutline, penSettings);

			penSettings.Color = stickyForeColor;
			classStyleSet.AddPen(StickyForegroundResource, DiagramPens.ShapeHighlightOutline, penSettings);
		}
		/// <summary>
		/// Drop the grid size to make positioning easier.
		/// </summary>
		public override double DefaultGridSize
		{
			get
			{
				return .05;
			}
		}
		private const double FineGridSize = 1d / (72 * 8); // 1/8 point, small enough so smooth out just about any line
		/// <summary>
		/// Adjust grid size based on zoom of active view
		/// </summary>
		public override double GridSize
		{
			get
			{
				double gridSize = base.GridSize;
				DiagramView diagramView;
				DiagramClientView clientView;
				double zoomFactor;
				if (null != (diagramView = ActiveDiagramView) &&
					null != (clientView = diagramView.DiagramClientView) &&
					(zoomFactor = clientView.ZoomFactor) > 1 &&
					!VGConstants.FuzzEqual(zoomFactor, 1, VGConstants.FuzzGeneral))
				{
					// Linearly adjust to fine grid size at max zoom
					gridSize += (zoomFactor - 1) * (FineGridSize - gridSize) / (clientView.MaximumZoom - 1);
				}
				return gridSize;
			}
			set
			{
				base.GridSize = value;
			}
		}
		#endregion // Customize appearance
		#region Toolbox support
		[NonSerialized]
		private Dictionary<string, ElementPrototypeToolboxAction> myPrototypedToolboxActions;
		/// <summary>
		/// Enable our toolbox actions. Additional filters recognized in this
		/// routine are added in ORMDesignerPackage.CreateToolboxItems.
		/// </summary>
		public override void OnViewMouseEnter(DiagramPointEventArgs pointArgs)
		{
			DiagramView activeView = ActiveDiagramView;
			MouseAction action = null;

			if (activeView != null)
			{
				if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramConnectExternalConstraintFilterString))
				{
					action = ExternalConstraintConnectAction;
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramExternalConstraintFilterString))
				{
					action = GetExternalConstraintAction(activeView.Toolbox.GetSelectedToolboxItem() as ModelingToolboxItem);
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramConnectInternalUniquenessConstraintFilterString))
				{
					action = InternalUniquenessConstraintConnectAction;
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramInternalUniquenessConstraintFilterString))
				{
					action = GetInternalUniquenessConstraintAction(activeView.Toolbox.GetSelectedToolboxItem() as ModelingToolboxItem);
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramConnectRoleFilterString))
				{
					action = RoleConnectAction;
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramCreateSubtypeFilterString))
				{
					action = SubtypeConnectAction;
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramModelNoteFilterString))
				{
					action = GetModelNoteAction(activeView.Toolbox.GetSelectedToolboxItem() as ModelingToolboxItem);
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramConnectModelNoteFilterString))
				{
					action = ModelNoteConnectAction;
				}
				else
				{
					IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
					if (extenders != null)
					{
						for (int i = 0; i < extenders.Length && action == null; ++i)
						{
							action = extenders[i].GetMouseAction(this, activeView);
						}
					}
					ModelingToolboxItem currentItem;
					ElementGroupPrototype prototype;
					if (action == null &&
						activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramDefaultFilterString) &&
						null != (currentItem = activeView.Toolbox.GetSelectedToolboxItem() as ModelingToolboxItem) &&
						null != (prototype = currentItem.Prototype))
					{
						Dictionary<string, ElementPrototypeToolboxAction> actionCache = myPrototypedToolboxActions;
						ElementPrototypeToolboxAction prototypeAction = null;
						if (actionCache == null)
						{
							myPrototypedToolboxActions = actionCache = new Dictionary<string, ElementPrototypeToolboxAction>();
						}
						if (!actionCache.TryGetValue(currentItem.Id, out prototypeAction))
						{
							prototypeAction = new ElementPrototypeToolboxAction(this, prototype);
							actionCache[currentItem.Id] = prototypeAction;
						}
						action = prototypeAction;
					}
				}
			}

			DiagramClientView clientView = pointArgs.DiagramClientView;
			IToolboxService toolboxService;
			if (clientView.ActiveMouseAction != action &&
				// UNDONE: We should not need the following line because the current mouse action
				// should correspond to the current toolbox action. However, Toolbox.SetSelectedToolboxItem
				// always crashes, so there is no way to reset the action when we explicitly chain.
				// The result of not doing this is that moving the mouse off and back on the diagram
				// during a chained mouse action cancels the action.
				// See corresponding code in ExternalConstraintConnectAction.ChainMouseAction and
				// InternalUniquenessConstraintConnectAction.ChainMouseAction.
				(action != null || (null != activeView && null != (toolboxService = activeView.Toolbox) && null != toolboxService.GetSelectedToolboxItem())))
			{
				ToolboxUtility.ActivateMouseAction(action, clientView, ((IORMToolServices)this.Store).ServiceProvider);
			}
		}
		/// <summary>
		/// Select the given item on the default tab
		/// </summary>
		/// <param name="activeView">DiagramView</param>
		/// <param name="itemId">Name of the item id</param>
		[Conditional("FALSE")] // The end result of this does nothing, block the calls but keep the concept in the code
		public static void SelectToolboxItem(DiagramView activeView, string itemId)
		{
			SelectToolboxItem(activeView, itemId, ResourceStrings.ToolboxDefaultTabName);
		}
		/// <summary>
		/// Select the given item on the specified toolbox tab
		/// </summary>
		/// <param name="activeView">DiagramView</param>
		/// <param name="itemId">Name of the item id</param>
		/// <param name="tabName">The tab name to select</param>
		[Conditional("FALSE")] // The end result of this does nothing, block the calls but keep the concept in the code
		public static void SelectToolboxItem(DiagramView activeView, string itemId, string tabName)
		{
			IToolboxService toolbox = activeView.Toolbox;
			if (toolbox != null)
			{
				// Select the connector action on the toolbox
				ToolboxItemCollection items = toolbox.GetToolboxItems(tabName);
				foreach (ToolboxItem item in items)
				{
					ModelingToolboxItem modelingItem = item as ModelingToolboxItem;
					if (modelingItem != null && modelingItem.Id == itemId)
					{
						// UNDONE: See comments on side effect in ORMDiagram.OnViewMouseEnter
						//toolbox.SetSelectedToolboxItem(item); // UNDONE: MSBUG Gives 'Value does not fall within expected range' error message, not sure why
						break;
					}
				}
			}
		}
		#region Standard toolbox action
#if VISUALSTUDIO_10_0
		[NonSerialized]
		private ToolboxAction myStandardToolboxAction;
		/// <summary>
		/// A replacement for the standard toolbox action.
		/// Fixes issue with toolbox action not deactivating.
		/// </summary>
		public new ToolboxAction ToolboxAction
		{
			get
			{
				ToolboxAction retVal = myStandardToolboxAction;
				if (retVal == null)
				{
					myStandardToolboxAction = retVal = new StandardToolboxAction(this); 
				}
				return retVal;
			}
		}
		/// <summary>
		/// Modify the standard toolbox action in VS2010 to deactivate
		/// after a click is completed. Otherwise, we remain in a hover
		/// state and do not cancel the drag tool
		/// </summary>
		private class StandardToolboxAction : ToolboxAction
		{
			/// <summary>
			/// Constructor for <see cref="StandardToolboxAction"/>
			/// </summary>
			public StandardToolboxAction(Diagram diagram)
				: base(diagram)
			{
			}
			/// <summary>
			/// Make sure we deactivate on click
			/// </summary>
			protected override void OnClicked(MouseActionEventArgs e)
			{
				base.OnClicked(e);
				if (IsActive)
				{
					Cancel(e.DiagramClientView);
				}
			}
		}
#endif // VISUALSTUDIO_10_0
		#endregion // Standard toolbox action
		#region External constraint action
		[NonSerialized]
		private ExternalConstraintConnectAction myExternalConstraintConnectAction;
		/// <summary>
		/// The connect action used to connect an external constraint to its role sequences
		/// </summary>
		public ExternalConstraintConnectAction ExternalConstraintConnectAction
		{
			get
			{
				if (myExternalConstraintConnectAction == null)
				{
					myExternalConstraintConnectAction = new ExternalConstraintConnectAction(this);
				}
				return myExternalConstraintConnectAction;
			}
		}
		/// <summary>
		/// Create the action used to connect an external constraint to its role sequences
		/// </summary>
		/// <returns>ExternalConstraintConnectAction instance</returns>
		protected virtual ExternalConstraintConnectAction CreateExternalConstraintConnectAction()
		{
			return new ExternalConstraintConnectAction(this);
		}
		/// <summary>
		/// The action used to drop an external constraint from the toolbox
		/// </summary>
		public ExternalConstraintAction GetExternalConstraintAction(ModelingToolboxItem toolboxItem)
		{
			Dictionary<string, ElementPrototypeToolboxAction> actionCache = myPrototypedToolboxActions;
			ElementPrototypeToolboxAction prototypeAction = null;
			ExternalConstraintAction constraintAction = null;
			string itemId = toolboxItem.Id;
			if (actionCache == null)
			{
				myPrototypedToolboxActions = actionCache = new Dictionary<string, ElementPrototypeToolboxAction>();
			}
			if (actionCache.TryGetValue(itemId, out prototypeAction))
			{
				constraintAction = (ExternalConstraintAction)prototypeAction;
			}
			else
			{
				constraintAction = CreateExternalConstraintAction(toolboxItem.Prototype);
				constraintAction.AfterMouseActionDeactivated += delegate(object sender, DiagramEventArgs e)
				{
					ExternalConstraintAction action = sender as ExternalConstraintAction;
					if (action.ActionCompleted)
					{
						ExternalConstraintShape addedShape = action.AddedConstraintShape;
						Debug.Assert(addedShape != null); // ActionCompleted should be false otherwise
						ExternalConstraintConnectAction.ChainMouseAction(addedShape, e.DiagramClientView);
					}
				};
				actionCache[itemId] = constraintAction;
			}
			return constraintAction;
		}
		/// <summary>
		/// Create the action used to add an external constraint from the toolbox
		/// </summary>
		/// <param name="prototype">The prototype associated with this action.</param>
		/// <returns>ExternalConstraintAction instance</returns>
		protected virtual ExternalConstraintAction CreateExternalConstraintAction(ElementGroupPrototype prototype)
		{
			return new ExternalConstraintAction(this, prototype);
		}
		#endregion // External constraint action
		#region Internal uniqueness constraint action
		[NonSerialized]
		private InternalUniquenessConstraintAction myInternalUniquenessConstraintAction;
		[NonSerialized]
		private InternalUniquenessConstraintConnectAction myInternalUniquenessConstraintConnectAction;
		/// <summary>
		/// The connect action used to connect an internal uniqueness constraint
		/// to its roles.
		/// </summary>
		public InternalUniquenessConstraintConnectAction InternalUniquenessConstraintConnectAction
		{
			get
			{
				if (myInternalUniquenessConstraintConnectAction == null)
				{
					myInternalUniquenessConstraintConnectAction = CreateInternalUniquenessConstraintConnectAction();
				}
				return myInternalUniquenessConstraintConnectAction;
			}
		}
		/// <summary>
		/// Create the connect action used to add an internal uniqueness constraint from the toolbox
		/// </summary>
		/// <returns>InternalUniquenssConstraintAction instance</returns>
		protected virtual InternalUniquenessConstraintConnectAction CreateInternalUniquenessConstraintConnectAction()
		{
			return new InternalUniquenessConstraintConnectAction(this);
		}
		/// <summary>
		/// The action used to add an internal uniqueness constraint from the toolbox
		/// </summary>
		public InternalUniquenessConstraintAction GetInternalUniquenessConstraintAction(ModelingToolboxItem toolboxItem)
		{
			InternalUniquenessConstraintAction constraintAction = myInternalUniquenessConstraintAction;
			ElementGroupPrototype prototype;
			if (constraintAction == null &&
				null != (prototype = toolboxItem.Prototype))
			{
				myInternalUniquenessConstraintAction = constraintAction = CreateInternalUniquenessConstraintAction(prototype);
				constraintAction.AfterMouseActionDeactivated += delegate(object sender, DiagramEventArgs e)
				{
					InternalUniquenessConstraintAction action = sender as InternalUniquenessConstraintAction;
					if (action.ActionCompleted)
					{
						UniquenessConstraint constraint = action.AddedConstraint;
						FactTypeShape addedToShape = action.DropTargetShape;
						DiagramClientView view = e.DiagramClientView;
						Debug.Assert(constraint != null); // ActionCompleted should be false otherwise
						view.Selection.Set(addedToShape.GetDiagramItem(constraint));
						InternalUniquenessConstraintConnectAction.ChainMouseAction(addedToShape, constraint, view);
					}
				};
			}
			return constraintAction;
		}
		/// <summary>
		/// Create the connect action used to connect internal uniqueness constrant roles
		/// </summary>
		/// <param name="prototype">The prototype associated with this action.</param>
		/// <returns>InternalUniquenssConstraintAction instance</returns>
		protected virtual InternalUniquenessConstraintAction CreateInternalUniquenessConstraintAction(ElementGroupPrototype prototype)
		{
			return new InternalUniquenessConstraintAction(this, prototype);
		}
		#endregion Internal uniqueness constraint action
		#region Role drag action
		[NonSerialized]
		private RoleDragPendingAction myRoleDragPendingAction;
		[NonSerialized]
		private RoleConnectAction myRoleConnectAction;
		/// <summary>
		/// The drag action used by a role box to begin dragging.
		/// The default implementation chains to a RoleConnectAction
		/// when dragging begins.
		/// </summary>
		public RoleDragPendingAction RoleDragPendingAction
		{
			get
			{
				RoleDragPendingAction retVal = myRoleDragPendingAction;
				if (retVal == null)
				{
					myRoleDragPendingAction = retVal = CreateRoleDragPendingAction();
				}
				return retVal;
			}
		}
		/// <summary>
		/// Create the drag action used for the RoleDragPendingAction property
		/// </summary>
		/// <returns>RoleDragPendingAction instance</returns>
		protected virtual RoleDragPendingAction CreateRoleDragPendingAction()
		{
			return new RoleDragPendingAction(this);
		}
		/// <summary>
		/// The connect action used to connect a role and
		/// its role player (an object type)
		/// </summary>
		public RoleConnectAction RoleConnectAction
		{
			get
			{
				RoleConnectAction retVal = myRoleConnectAction;
				if (retVal == null)
				{
					myRoleConnectAction = retVal = CreateRoleConnectAction();
				}
				return retVal;
			}
		}
		/// <summary>
		/// Create the connect action used to connect roles to their role players
		/// </summary>
		/// <returns>RoleConnectAction instance</returns>
		protected virtual RoleConnectAction CreateRoleConnectAction()
		{
			return new RoleConnectAction(this);
		}
		#endregion // Role drag action
		#region Subtype create action
		[NonSerialized]
		private SubtypeConnectAction mySubtypeConnectAction;
		/// <summary>
		/// The connect action used to connect a base type to a derived type
		/// </summary>
		public SubtypeConnectAction SubtypeConnectAction
		{
			get
			{
				SubtypeConnectAction retVal = mySubtypeConnectAction;
				if (retVal == null)
				{
					mySubtypeConnectAction = retVal = CreateSubtypeConnectAction();
				}
				return retVal;
			}
		}
		/// <summary>
		/// Create the connect action used to connect roles to their role players
		/// </summary>
		/// <returns>SubtypeConnectAction instance</returns>
		protected virtual SubtypeConnectAction CreateSubtypeConnectAction()
		{
			return new SubtypeConnectAction(this);
		}
		#endregion // Subtype create action
		#region ModelNote create action
		[NonSerialized]
		private ModelNoteAction myModelNoteAction;
		/// <summary>
		/// The action used to drop a model note from the toolbox
		/// </summary>
		public ModelNoteAction GetModelNoteAction(ModelingToolboxItem toolboxItem)
		{
			ModelNoteAction noteAction = myModelNoteAction;
			if (noteAction == null)
			{
				myModelNoteAction = noteAction = CreateModelNoteAction(toolboxItem.Prototype);
				noteAction.AfterMouseActionDeactivated += delegate(object sender, DiagramEventArgs e)
				{
					ModelNoteAction action = sender as ModelNoteAction;
					if (action.ActionCompleted)
					{
						ModelNoteShape addedShape = action.AddedNoteShape;
						Debug.Assert(addedShape != null); // ActionCompleted should be false otherwise
						Store store = Store;
						EditorUtility.ActivatePropertyEditor(
							(store as IORMToolServices).ServiceProvider,
							DomainTypeDescriptor.CreatePropertyDescriptor(addedShape.ModelElement, Note.TextDomainPropertyId),
							true);
					}
				};
			}
			return noteAction;
		}
		/// <summary>
		/// Create the action used to add an external constraint from the toolbox
		/// </summary>
		/// <param name="prototype">The prototype associated with this action.</param>
		/// <returns>ExternalConstraintAction instance</returns>
		protected virtual ModelNoteAction CreateModelNoteAction(ElementGroupPrototype prototype)
		{
			return new ModelNoteAction(this, prototype);
		}
		#endregion // ModelNote connect action
		#region ModelNote connect action
		[NonSerialized]
		private ModelNoteConnectAction myModelNoteConnectAction;
		/// <summary>
		/// The connect action used to connect a note to a referenced element
		/// </summary>
		public ModelNoteConnectAction ModelNoteConnectAction
		{
			get
			{
				ModelNoteConnectAction retVal = myModelNoteConnectAction;
				if (retVal == null)
				{
					myModelNoteConnectAction = retVal = CreateModelNoteConnectAction();
				}
				return retVal;
			}
		}
		/// <summary>
		/// Create the connect action used to connect a note to a referenced element
		/// </summary>
		/// <returns>ModelNoteConnectAction instance</returns>
		protected virtual ModelNoteConnectAction CreateModelNoteConnectAction()
		{
			return new ModelNoteConnectAction(this);
		}
		#endregion // ModelNote connect action
		#endregion // Toolbox support
		#region Other base overrides
		/// <summary>
		/// Clean up disposable members (connection actions)
		/// </summary>
		/// <param name="disposing">Do stuff if true</param>
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					// Use a somewhat paranoid pattern here to protect against reentrancy
					IDisposable disposeMe;
					disposeMe = myExternalConstraintConnectAction as IDisposable;
					myExternalConstraintConnectAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}

					disposeMe = myInternalUniquenessConstraintAction as IDisposable;
					myInternalUniquenessConstraintAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}

					disposeMe = myInternalUniquenessConstraintConnectAction as IDisposable;
					myInternalUniquenessConstraintConnectAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}

					disposeMe = myRoleDragPendingAction as IDisposable;
					myRoleDragPendingAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}

					disposeMe = myRoleConnectAction as IDisposable;
					myRoleConnectAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}

					disposeMe = mySubtypeConnectAction as IDisposable;
					mySubtypeConnectAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}

					Dictionary<string, ElementPrototypeToolboxAction> defaultActions = myPrototypedToolboxActions;
					if (defaultActions != null)
					{
						myPrototypedToolboxActions = null;
						foreach (ElementPrototypeToolboxAction action in defaultActions.Values)
						{
							if (null != (disposeMe = action as IDisposable))
							{
								disposeMe.Dispose();
							}
						}
					}
					Store store = Utility.ValidateStore(Store);
					if (store != null)
					{
						IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
						if (extenders != null)
						{
							for (int i = 0; i < extenders.Length; ++i)
							{
								extenders[i].ExtendedShapeDisposed(this);
							}
						}
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		/// <summary>
		/// Set the base font based on the font and color settings.
		/// UNDONE: This affects the size right now, but not the font name
		/// </summary>
		protected override Font BaseFontFromEnvironment
		{
			get
			{
				return (this.Store as ObjectModel.IORMToolServices).FontAndColorService.GetFont(ORMDesignerColorCategory.Editor);
			}
		}
		/// <summary>
		/// Stop all auto shape selection on transaction commit except when
		/// the item is being dropped.
		/// </summary>
		public override IList FixUpDiagramSelection(ShapeElement newChildShape)
		{
			ISelectionContainerFilter selectionFilter;
			if (DropTargetContext.HasDropTargetContext(Store.TransactionManager.CurrentTransaction.TopLevelTransaction) &&
				(null == (selectionFilter = newChildShape as ISelectionContainerFilter) ||
				selectionFilter.IncludeInSelectionContainer))
			{
				return base.FixUpDiagramSelection(newChildShape);
			}
			return null;
		}
		#endregion // Other base overrides
		#region Selection Rules
		/// <summary>
		/// A class to limit the selection of link shapes when
		/// the connected shapes are also selected. This is specifically
		/// targeted at subtype connectors, which otherwise become
		/// the primary selection in a lasso select of the subtype and
		/// supertype shapes.
		/// </summary>
		private class LimitLinkShapeSelectionRules : DiagramSelectionRules
		{
			private ORMDiagram myDiagram;
			private DiagramSelectionRules myDeferToRules;
			/// <summary>
			/// Create a new <see cref="LimitLinkShapeSelectionRules"/> object
			/// </summary>
			/// <param name="diagram">The context diagram</param>
			/// <param name="deferToRules">The base rules used for initial collection modification.</param>
			public LimitLinkShapeSelectionRules(ORMDiagram diagram, DiagramSelectionRules deferToRules)
			{
				myDiagram = diagram;
				myDeferToRules = deferToRules;
			}
			/// <summary>
			/// Limit selection
			/// </summary>
			public override bool GetCompliantSelection(SelectedShapesCollection currentSelection, DiagramItemCollection proposedItemsToAdd, DiagramItemCollection proposedItemsToRemove, DiagramItem primaryItem)
			{
				bool retVal = myDeferToRules.GetCompliantSelection(currentSelection, proposedItemsToAdd, proposedItemsToRemove, primaryItem);
				int addCount;
				if (retVal &&
					0 != (addCount = proposedItemsToAdd.Count))
				{
					Dictionary<ShapeElement, object> endpointTracker = null;
					Dictionary<LinkShape, int> linkIndices = null; // Positive means in add collection and included, MaxValue indicates in current selection, ~index (negative value) indicates remove required
					Dictionary<LinkShape, DiagramItem> currentLinks = null; // Selected shapes collection is not indexed, track the DiagramItem for current items that need to be removed.
					bool firstSet = true;
					int linkIndex = 0;
					LinkShape linkShape;
					ShapeElement selectedShape;
					object tracked;
					LinkedNode<DiagramItem> trackedNode;
					DiagramItem trackedItem;
					ICollection[] itemCollections = new ICollection[] { proposedItemsToAdd, currentSelection };
					foreach (ICollection itemSet in itemCollections)
					{
						foreach (DiagramItem item in itemSet)
						{
							IProxyConnectorShape connectorShape;
							if (item.SubField == null &&
								null != (linkShape = item.Shape as LinkShape))
							{
								if (linkIndices == null)
								{
									linkIndices = new Dictionary<LinkShape, int>();
								}
								if (firstSet)
								{
									linkIndices[linkShape] = linkIndex;
								}
								else
								{
									linkIndices[linkShape] = int.MaxValue;
									(currentLinks ?? (currentLinks = new Dictionary<LinkShape,DiagramItem>()))[linkShape] = item;
								}
								foreach (NodeShape nodeShape in LinkConnectsToNode.GetNodes(linkShape))
								{
									selectedShape = (null != (connectorShape = nodeShape as IProxyConnectorShape)) ? connectorShape.ProxyConnectorShapeFor : nodeShape;
									if ((endpointTracker ?? (endpointTracker = new Dictionary<ShapeElement,object>())).TryGetValue(selectedShape, out tracked))
									{
										if (null != (trackedItem = tracked as DiagramItem))
										{
											trackedNode = new LinkedNode<DiagramItem>(trackedItem);
											trackedNode.SetNext(new LinkedNode<DiagramItem>(item), ref trackedNode);
											endpointTracker[selectedShape] = trackedNode;
										}
										else
										{
											trackedNode = (LinkedNode<DiagramItem>)tracked;
											trackedNode.GetTail().SetNext(new LinkedNode<DiagramItem>(item), ref trackedNode);
										}
									}
									else
									{
										endpointTracker.Add(selectedShape, item);
									}
								}
							}
							++linkIndex;
						}
						firstSet = false;
					}
					if (null != endpointTracker)
					{
						// Do no further processing to track removed links
						foreach (DiagramItem item in proposedItemsToRemove)
						{
							if (item.SubField == null &&
								null != (linkShape = item.Shape as LinkShape) &&
								linkIndices.ContainsKey(linkShape))
							{
								linkIndices.Remove(linkShape);
							}
						}

						// Walk both collections to see if link endpoints need to be removed
						int removedCount = 0;
						foreach (ICollection itemSet in itemCollections)
						{
							foreach (DiagramItem item in itemSet)
							{
								if (item.SubField == null &&
									null != (selectedShape = item.Shape) &&
									endpointTracker.TryGetValue(selectedShape, out tracked))
								{
									if (null != (trackedItem = tracked as DiagramItem))
									{
										linkShape = (LinkShape)trackedItem.Shape;
										if (linkIndices.TryGetValue(linkShape, out linkIndex) &&
											linkIndex >= 0)
										{
											++removedCount;
											linkIndices[linkShape] = ~linkIndex;
										}
									}
									else
									{
										trackedNode = (LinkedNode<DiagramItem>)tracked;
										while (null != trackedNode)
										{
											trackedItem = trackedNode.Value;
											// Same as code for single item above
											linkShape = (LinkShape)trackedItem.Shape;
											if (linkIndices.TryGetValue(linkShape, out linkIndex) &&
												linkIndex >= 0)
											{
												++removedCount;
												linkIndices[linkShape] = ~linkIndex;
											}

											trackedNode = trackedNode.Next;
										}
									}
								}
							}
						}
						if (removedCount != 0)
						{
							List<int> removedIndices = null;
							foreach (KeyValuePair<LinkShape, int> pair in linkIndices)
							{
								int removeIndex = pair.Value;
								if (removeIndex < 0)
								{
									if (removeIndex == int.MinValue)
									{
										trackedItem = currentLinks[pair.Key];
										if (!proposedItemsToRemove.Contains(trackedItem))
										{
											proposedItemsToRemove.Add(trackedItem);
										}
									}
									else
									{
										(removedIndices ?? (removedIndices = new List<int>())).Add(removeIndex);
									}
								}
							}
							if (null != removedIndices)
							{
								int removeCount = removedIndices.Count;
								if (removeCount > 1)
								{
									removedIndices.Sort();
								}
								for (int i = 0; i < removeCount; ++i)
								{
									proposedItemsToAdd.RemoveAt(~removedIndices[i]);
								}
							}
						}
					}
					retVal = proposedItemsToAdd.Count != 0 || proposedItemsToRemove.Count != 0;
				}
				return retVal;
			}
		}
		private DiagramSelectionRules mySelectionRules;
		/// <summary>
		/// Modify selection algorithm to automatically deselect connector
		/// lines when either of the endpoint shapes are also selected.
		/// </summary>
		public override DiagramSelectionRules SelectionRules
		{
			get
			{
				DiagramSelectionRules retVal = mySelectionRules;
				if (retVal == null)
				{
					mySelectionRules = retVal = new LimitLinkShapeSelectionRules(this, base.SelectionRules);
				}
				return retVal;
			}
		}
		#endregion // Selection Rules
		#region Accessibility Properties
		/// <summary>
		/// Return the class name as the accessible name
		/// </summary>
		public override string AccessibleName
		{
			get
			{
				return ResourceStrings.ORMDiagramAccessibleName;
			}
		}
		/// <summary>
		/// Return the component name as the accessible value
		/// </summary>
		public override string AccessibleValue
		{
			get
			{
				return this.Name;
			}
		}
		#endregion // Accessibility Properties
		#region Utility Methods
		/// <summary>
		/// Modify the luminosity for a given color. This
		/// duplicates the algorithm in ShapeElement.GetShapeLuminosity,
		/// which is not available because it is only run for shape elements
		/// in the DiagramClientView.HighlightedShapes collection.
		/// </summary>
		/// <param name="startColor">The original color</param>
		/// <returns>The modified color</returns>
		public static Color ModifyLuminosity(Color startColor)
		{
			HslColor hslColor = HslColor.FromRgbColor(startColor);
			hslColor.Luminosity = ModifyLuminosity(hslColor.Luminosity);
			return hslColor.ToRgbColor();
		}
		/// <summary>
		/// Modify a specific luminosity. 
		/// </summary>
		/// <param name="startLuminosity">Beginning luminosity value</param>
		/// <returns>modified luminosity</returns>
		public static int ModifyLuminosity(int startLuminosity)
		{
			// Base framework algorithm for reference
			//const int luminosityCheck = 160;
			//const int luminosityDelta = 40;
			//const double luminosityFactor = 0.9;
			//return (startLuminosity >= luminosityCheck) ?
			//	(int)(startLuminosity * luminosityFactor) :
			//	(startLuminosity + luminosityDelta);

			// Use a sliding scale to brighten/darken colors
			const int maxLuminosity = 255;
			const int luminosityCheck = 160;
			const int luminosityFixedDelta = 60;
			const int luminosityIncrementalDelta = 30;
			const double luminosityFixedFactor = 0.93;
			const double luminosityIncrementalFactor = -0.06;
			return (startLuminosity >= luminosityCheck) ?
				(int)(startLuminosity * (luminosityFixedFactor + (luminosityIncrementalFactor * (double)(maxLuminosity - startLuminosity) / (maxLuminosity - luminosityCheck)))) :
				(startLuminosity + luminosityFixedDelta + (int)((double)(luminosityCheck - startLuminosity) / luminosityCheck * luminosityIncrementalDelta));
		}
		#endregion // Utility Methods
		#region IProxyDisplayProvider Implementation
		/// <summary>
		/// Implements IProxyDisplayProvider.ElementDisplayedAs
		/// </summary>
		protected object ElementDisplayedAs(ModelElement element, ModelError forError)
		{
			ObjectType objectElement;
			ExclusionConstraint exclusionConstraint;
			SetConstraint setConstraint;
			FactType factType;
			if (null != (objectElement = element as ObjectType))
			{
				if (!ShouldDisplayObjectType(objectElement) &&
					!(forError is ObjectTypeDuplicateNameError))
				{
					FactType nestedFact = objectElement.NestedFactType;
					if (nestedFact != null)
					{
						return nestedFact;
					}
					// Otherwise, every fact type we're a role player for is
					// part of a collapsed reference mode. Grab the first fact, and
					// find the corresponding object type.
					foreach (ConstraintRoleSequence constraintSequence in objectElement.PlayedRoleCollection[0].ConstraintRoleSequenceCollection)
					{
						UniquenessConstraint iuc = constraintSequence as UniquenessConstraint;
						if (iuc != null && iuc.IsInternal)
						{
							ObjectType displayedType = iuc.PreferredIdentifierFor;
							if (displayedType != null)
							{
								return displayedType;
							}
						}
					}
				}
			}
			else if (null != (factType = element as FactType))
			{
				// For an implied FactType, select the associated role on the
				// nesting FactType.
				Objectification objectification;
				if (null != (objectification = factType.ImpliedByObjectification))
				{
					foreach (RoleBase roleBase in factType.RoleCollection)
					{
						RoleProxy proxy;
						ObjectifiedUnaryRole objectifiedUnaryRole;
						if (null != (proxy = roleBase as RoleProxy))
						{
							return proxy.TargetRole;
						}
						else if (null != (objectifiedUnaryRole = roleBase as ObjectifiedUnaryRole))
						{
							return objectifiedUnaryRole.TargetRole;
						}
					}
				}
			}
			else if (null != (exclusionConstraint = element as ExclusionConstraint))
			{
				MandatoryConstraint mandatoryConstraint = exclusionConstraint.ExclusiveOrMandatoryConstraint;
				if (mandatoryConstraint != null)
				{
					return mandatoryConstraint;
				}
			}
			else if (null != (setConstraint = element as SetConstraint))
			{
				// Internal constraints are displayed with the FactType
				LinkedElementCollection<FactType> factTypes;
				if ((setConstraint as IConstraint).ConstraintIsInternal &&
					1 == (factTypes = setConstraint.FactTypeCollection).Count)
				{
					return factTypes[0];
				}
			}
			return null;
		}
		object IProxyDisplayProvider.ElementDisplayedAs(ModelElement element, ModelError forError)
		{
			return ElementDisplayedAs(element, forError);
		}
		#endregion // IProxyDisplayProvider Implementation
		#region IMergeElements implementation
		/// <summary>
		/// Implements <see cref="IMergeElements.MergeRelate"/>. Allows
		/// duplication of shapes across diagrams in the same model.
		/// </summary>
		protected new void MergeRelate(ModelElement sourceElement, ElementGroup elementGroup)
		{
			ShapeElement shape = sourceElement as ShapeElement;
			if (shape != null && shape.ParentShape == null)
			{
				NestedChildShapes.Add(shape);
				MergeRelateShape(shape);
			}
		}
		/// <summary>
		/// Complete the merge of a top-level shape element into the diagram. Called immediately
		/// after the <paramref name="shape"/> element is added to the <see cref="ShapeElement.NestedChildShapes"/> collection.
		/// </summary>
		/// <param name="shape">The newly merged <see cref="ShapeElement"/></param>
		protected virtual void MergeRelateShape(ShapeElement shape)
		{
			object AllowMultipleShapes;
			Dictionary<object, object> topLevelContextInfo;
			bool containedAllowMultipleShapes;
			if (!(containedAllowMultipleShapes = (topLevelContextInfo = Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
			{
				topLevelContextInfo.Add(AllowMultipleShapes, null);
			}

			ModelElement element = shape.ModelElement;
			FactType factType;
			ObjectType objectType;
			SetConstraint setConstraint;
			SetComparisonConstraint setComparisonConstraint;
			ModelNote modelNote;
			if (null != (factType = element as FactType))
			{
				FixupFactType(factType, shape as FactTypeShape, true);
			}
			else if (null != (objectType = element as ObjectType))
			{
				FixupObjectType(objectType, shape as ObjectTypeShape, true);
			}
			else if (null != (setConstraint = element as SetConstraint))
			{
				FixupConstraint(setConstraint, (ExternalConstraintShape)shape);
			}
			else if (null != (setComparisonConstraint = element as SetComparisonConstraint))
			{
				FixupConstraint(setComparisonConstraint, (ExternalConstraintShape)shape);
			}
			else if (null != (modelNote = element as ModelNote))
			{
				FixupModelNote(modelNote, (ModelNoteShape)shape);
			}

			if (!containedAllowMultipleShapes)
			{
				topLevelContextInfo.Remove(AllowMultipleShapes);
			}
		}
		void IMergeElements.MergeRelate(ModelElement sourceElement, ElementGroup elementGroup)
		{
			MergeRelate(sourceElement, elementGroup);
		}
		/// <summary>
		/// Make sure that all elements in <paramref name="verifyFactTypes"/>
		/// have a corresponding shape on the diagram or a pending shape in
		/// the <paramref name="elementGroupPrototype"/>
		/// </summary>
		/// <param name="verifyFactTypes">An <see cref="IList{FactType}"/> to verify</param>
		/// <param name="elementGroupPrototype">The <see cref="ElementGroupPrototype"/> that is being merged</param>
		/// <returns><see langword="true"/> if all <see cref="FactType"/>s are accounted for.</returns>
		private bool VerifyCorrespondingFactTypes(IList<FactType> verifyFactTypes, ElementGroupPrototype elementGroupPrototype)
		{
			int factCount = verifyFactTypes.Count;
			if (factCount == 0)
			{
				// Nothing required, trivial success
				return true;
			}
			bool searchedPrototypes = elementGroupPrototype == null;
			FactType[] verifyFactTypes_Editable = null;
			IElementDirectory sourceElementDirectory = null;
			ReadOnlyCollection<ProtoElement> rootElements = null;
			Store targetStore = Store;
			Store sourceStore = verifyFactTypes[0].Store;
			bool foreignSource = targetStore != sourceStore;
			IEquivalentElementTracker elementTracker = null;
			bool newTracker = true;
			if (foreignSource &&
				elementGroupPrototype != null)
			{
				object trackerCache;
				newTracker = !elementGroupPrototype.TargetContext.ContextInfo.TryGetValue("EquivalentElementTracker", out trackerCache) ||
					null == (elementTracker = trackerCache as IEquivalentElementTracker);
			}
			bool retVal = true;
			for (int i = 0; i < factCount; ++i)
			{
				FactType verifySourceFactType = verifyFactTypes[i];
				if (verifySourceFactType == null)
				{
					// This can be nulled out below
					continue;
				}
				FactType verifyTargetFactType = foreignSource ? CopyMergeUtility.GetEquivalentElement(verifySourceFactType, targetStore, elementTracker ?? (elementTracker = CopyMergeUtility.CreateEquivalentElementTracker())) : verifySourceFactType;
				if (null == verifyTargetFactType ||
					null == FindShapeForElement(verifyTargetFactType))
				{
					SubtypeFact subtypeFact = verifySourceFactType as SubtypeFact;
					if (subtypeFact != null)
					{
						// Subtypes links are not directly prototyped. If the shape does not yet it exist,
						// then it will be created automatically if both the subtype and supertype already
						// exist are or available in the prototypes.
						ObjectType sourceSupertype = subtypeFact.Supertype;
						ObjectType sourceSubtype = subtypeFact.Subtype;
						ObjectType targetObjectType;
						bool haveSupertypeRepresentation = foreignSource ?
							(null != (targetObjectType = CopyMergeUtility.GetEquivalentElement(sourceSupertype, targetStore, elementTracker)) && FindShapeForElement(targetObjectType) != null) :
							(FindShapeForElement(sourceSupertype) != null);
						bool haveSubtypeRepresentation = foreignSource ?
							(null != (targetObjectType = CopyMergeUtility.GetEquivalentElement(sourceSubtype, targetStore, elementTracker)) && FindShapeForElement(targetObjectType) != null) :
							(FindShapeForElement(sourceSubtype) != null);
						if ((!haveSupertypeRepresentation || !haveSubtypeRepresentation) &&
							elementGroupPrototype != null)
						{
							if (rootElements == null)
							{
								sourceElementDirectory = sourceStore.ElementDirectory;
								rootElements = elementGroupPrototype.RootProtoElements;
							}
							foreach (ProtoElement protoElement in rootElements)
							{
								PresentationElement testPel;
								FactType testFactType;
								ObjectType testObjectType;
								if (null != (testPel = sourceElementDirectory.FindElement(protoElement.ElementId) as PresentationElement))
								{
									ModelElement testElement = testPel.ModelElement;
									if (null != (testObjectType = testElement as ObjectType) ||
										(null != (testFactType = testElement as FactType) &&
										null != (testObjectType = testFactType.NestingType)))
									{
										if (!haveSupertypeRepresentation)
										{
											if (testObjectType == sourceSupertype)
											{
												haveSupertypeRepresentation = true;
												if (haveSubtypeRepresentation)
												{
													break;
												}
											}
										}
										if (!haveSubtypeRepresentation)
										{
											if (testObjectType == sourceSubtype)
											{
												haveSubtypeRepresentation = true;
												if (haveSupertypeRepresentation)
												{
													break;
												}
											}
										}
									}
								}
							}
						}
						if (haveSupertypeRepresentation && haveSubtypeRepresentation)
						{
							continue;
						}
						retVal = false;
						break;
					}
					if (searchedPrototypes)
					{
						retVal = false;
						break;
					}
					searchedPrototypes = true;
					// See which prototype facts are being added. Create an editable list so
					// we can walk this once only.
					if (rootElements == null)
					{
						sourceElementDirectory = sourceStore.ElementDirectory;
						rootElements = elementGroupPrototype.RootProtoElements;
					}
					foreach (ProtoElement protoElement in rootElements)
					{
						PresentationElement testPel;
						FactType testFactType;
						if (null != (testPel = sourceElementDirectory.FindElement(protoElement.ElementId) as PresentationElement) &&
							null != (testFactType = testPel.ModelElement as FactType))
						{
							int verifyIndex = verifyFactTypes.IndexOf(testFactType);
							if (verifyIndex != -1)
							{
								if (verifyFactTypes_Editable == null)
								{
									verifyFactTypes_Editable = new FactType[factCount];
									verifyFactTypes.CopyTo(verifyFactTypes_Editable, 0);
									for (int k = 0; k < i; ++k)
									{
										verifyFactTypes_Editable[k] = null;
									}
									verifyFactTypes = verifyFactTypes_Editable;
								}
								verifyFactTypes_Editable[verifyIndex] = null;
							}
						}
					}
					if (verifyFactTypes[i] != null)
					{
						retVal = false;
						break;
					}
				}
			}
			if (newTracker &&
				elementTracker != null &&
				elementGroupPrototype != null)
			{
				elementGroupPrototype.TargetContext.ContextInfo["EquivalentElementTracker"] = elementTracker;
			}
			return retVal;
		}
		/// <summary>
		/// Extend CanMerge to allow duplication of shapes across diagrams in the same model
		/// </summary>
		protected override bool CanMerge(ProtoElementBase rootElement, ElementGroupPrototype elementGroupPrototype)
		{
			Store store = Store;
			object storeIdObject;
			if (Partition == store.DefaultPartition &&
				elementGroupPrototype.SourceContext.ContextInfo.TryGetValue("SourceStore", out storeIdObject) &&
				storeIdObject != null &&
				storeIdObject is Guid)
			{
				Guid sourceStoreId = (Guid)storeIdObject;
				PresentationElement pel;
				ModelElement element;
				Store sourceStore = sourceStoreId == store.Id ? store : FrameworkShellUtility.ResolveLoadedStore<ORMDesignerDocData>(((IORMToolServices)store).ServiceProvider, sourceStoreId);
				if (sourceStore != null &&
					null != (pel = sourceStore.ElementDirectory.FindElement(rootElement.ElementId) as PresentationElement) &&
					null != (element = pel.ModelElement) &&
					(ShouldAddShapeForElement(element)))
				{
					return CanMergeElement(element, elementGroupPrototype);
				}
			}
			return base.CanMerge(rootElement, elementGroupPrototype);
		}
		/// <summary>
		/// Provide element-specific testing to determine if an element can be merged or not.
		/// This provides a stronger test than <see cref="ShouldAddShapeForElement"/>, which does
		/// not consider pending shapes representing by the <paramref name="elementGroupPrototype"/>
		/// </summary>
		/// <param name="element">A <see cref="ModelElement"/> that does not currently have a shape on the
		/// diagram and has already passed the <see cref="ShouldAddShapeForElement"/> method.</param>
		/// <param name="elementGroupPrototype">The <see cref="ElementGroupPrototype"/> that is being merged</param>
		/// <returns><see langword="true"/> to allow merge.</returns>
		protected virtual bool CanMergeElement(ModelElement element, ElementGroupPrototype elementGroupPrototype)
		{
			bool retVal = true;
			SetConstraint setConstraint;
			SetComparisonConstraint setComparisonConstraint;
			if (null != (setConstraint = element as SetConstraint))
			{
				retVal = VerifyCorrespondingFactTypes(setConstraint.FactTypeCollection, elementGroupPrototype);
			}
			else if (null != (setComparisonConstraint = element as SetComparisonConstraint))
			{
				retVal = VerifyCorrespondingFactTypes(setComparisonConstraint.FactTypeCollection, elementGroupPrototype);
			}
			return retVal;
		}
		/// <summary>
		/// Helper function to verify merge requirements.
		/// </summary>
		private bool IsMergingExternalStore
		{
			get
			{
				Store store;
				object storeIdObject;
				Transaction t;
				return (null != (store = Store) &&
					null != (t = store.TransactionManager.CurrentTransaction) &&
					t.TopLevelTransaction.Context.ContextInfo.TryGetValue("SourceStore", out storeIdObject) &&
					storeIdObject != null &&
					storeIdObject is Guid) ?
						(((Guid)storeIdObject) != store.Id) :
						false;
			}
		}
		[NonSerialized]
		private ORMDesignerElementOperations myElementOperations;
		/// <summary>
		/// Modified element operations to allow duplication of shapes across diagrams
		/// in the same model.
		/// </summary>
		public override DesignSurfaceElementOperations ElementOperations
		{
			get
			{
				ORMDesignerElementOperations elementOperations = myElementOperations;
				if (elementOperations == null)
				{
#if VISUALSTUDIO_10_0
					myElementOperations = elementOperations = new ORMDesignerElementOperations(this);
#else
					myElementOperations = elementOperations = new ORMDesignerElementOperations(Store);
#endif
				}
				return elementOperations;
			}
		}
		/// <summary>
		/// Reconstitute parts of a merged shape that are not duplicated as
		/// part of the copy closure.
		/// </summary>
		/// <param name="mergedShape">The new shape being merged</param>
		/// <param name="prototypeShape">The shape the new shape is based on</param>
		/// <param name="identityMap">A map from elements in the native <see cref="Store"/>
		/// for <paramref name="prototypeShape"/> to elements in the <see cref="Store"/> for
		/// the <paramref name="mergedShape"/>. Not set if the Store is the same for both shapes.</param>
		protected virtual void ReconstituteMergedShape(PresentationElement mergedShape, PresentationElement prototypeShape, IDictionary<Guid, ModelElement> identityMap)
		{
			FactTypeShape factTypeShape;
			if (null != (factTypeShape = mergedShape as FactTypeShape))
			{
				// Role order display is not included in the copy because
				// of the direct links to the object model. We need to reattach
				// to existing roles, not create new ones as part of the prototype.
				FactTypeShape protoFactTypeShape = (FactTypeShape)prototypeShape;
				LinkedElementCollection<RoleBase> protoDisplayRoles = protoFactTypeShape.RoleDisplayOrderCollection;
				int displayRoleCount = protoDisplayRoles.Count;
				if (displayRoleCount != 0)
				{
					LinkedElementCollection<RoleBase> displayRoles = factTypeShape.RoleDisplayOrderCollection;
					for (int j = 0; j < displayRoleCount; ++j)
					{
						displayRoles.Add(identityMap == null ? protoDisplayRoles[j] : (RoleBase)identityMap[protoDisplayRoles[j].Id]);
					}
				}
			}

			// Merged shapes do not go through any other code paths that
			// would automatically add them to the current selection, so
			// we do it here. This means that dropped elements will be
			// selected after the drop. Note that there is no guarantee of
			// ordering or of which element will be primary when the merge
			// is complete.
			ShapeElement shape;
			if (null != (shape = mergedShape as ShapeElement))
			{
				FixUpDiagramSelection(shape);
			}
		}
		#region ORMDesignerElementOperations class
		/// <summary>
		/// Support duplication of shapes across diagrams in the same model
		/// </summary>
		protected class ORMDesignerElementOperations : DesignSurfaceElementOperations
		{
#if VISUALSTUDIO_10_0
			/// <summary>
			/// Create custom operations to allow copying of shapes between
			/// diagrams in the same model
			/// </summary>
			/// <param name="diagram">The context <see cref="Diagram"/></param>
			public ORMDesignerElementOperations(Diagram diagram)
				: base((IServiceProvider)diagram.Store, diagram)
			{
			}
#else
			/// <summary>
			/// Create custom operations to allow copying of shapes between
			/// diagrams in the same model
			/// </summary>
			/// <param name="store">The context <see cref="Store"/></param>
			public ORMDesignerElementOperations(Store store)
				: base((IServiceProvider)store, store)
			{
			}
#endif
			/// <summary>
			/// Mark all non-parented shape elements as root elements before propagating element group
			/// </summary>
			protected override void PropagateElementGroupContextToTransaction(ModelElement targetElement, ElementGroup elementGroup, Transaction t)
			{
				ReadOnlyCollection<ModelElement> rootElements = elementGroup.RootElements;
				ReadOnlyCollection<ModelElement> elements = elementGroup.ModelElements;
				int elementCount = elements.Count;
				for (int i = 0; i < elementCount; ++i)
				{
					ShapeElement shape = elements[i] as ShapeElement;
					if (shape != null && shape.ParentShape == null && !rootElements.Contains(shape))
					{
						elementGroup.MarkAsRoot(shape);
					}
				}
				base.PropagateElementGroupContextToTransaction(targetElement, elementGroup, t);
			}
			/// <summary>
			/// Presort elements before copying. The element order is persistent through the
			/// copy operation. Presorting the elements makes merging much easier at the drop target.
			/// Sorting can be modified by overriding the <see cref="CopyOrderComparer"/> property,
			/// which uses the <see cref="CompareElementsForCopy"/> method as the default sort.
			/// </summary>
			public override void Copy(IDataObject data, ICollection<ModelElement> elements, ClosureType closureType, PointF sourcePosition)
			{
				int elementCount = elements.Count;
				if (elementCount > 1)
				{
					int remainingCount = elementCount;
					foreach (ModelElement mel in elements)
					{
						if (!FilterCopiedElement(mel))
						{
							--remainingCount;
						}
					}
					if (remainingCount != 0)
					{
						Comparison<ModelElement> comparer = CopyOrderComparer;
						if (comparer != null || remainingCount != elementCount)
						{
							ModelElement[] modifiedElements = new ModelElement[remainingCount];
							if (remainingCount == elementCount)
							{
								elements.CopyTo(modifiedElements, 0);
							}
							else
							{
								int i = -1;
								foreach (ModelElement mel in elements)
								{
									if (FilterCopiedElement(mel))
									{
										modifiedElements[++i] = mel;
									}
								}
							}
							if (comparer != null)
							{
								Array.Sort<ModelElement>(modifiedElements, comparer);
							}
							elements = modifiedElements;
						}
					}
				}
				base.Copy(data, elements, closureType, sourcePosition);
			}
			/// <summary>
			/// Use as a compare routine to presort copied elements. Override and
			/// return null for no sort. The default compare is available in the
			/// <see cref="CompareElementsForCopy"/> method.
			/// </summary>
			protected virtual Comparison<ModelElement> CopyOrderComparer
			{
				get
				{
					// UNDONE: Wire the CopyOrderComparer into the ShapeExtension mechanism
					return new Comparison<ModelElement>(CompareElementsForCopy);
				}
			}
			/// <summary>
			/// Remove elements from a set of copied elements
			/// </summary>
			/// <param name="element"><see cref="ModelElement">Element</see> to filter.</param>
			/// <returns>Return <see langword="true"/> to include the <paramref name="element"/>.</returns>
			protected virtual bool FilterCopiedElement(ModelElement element)
			{
				return !(element is IAutoCreatedSelectableShape);
			}
			/// <summary>
			/// Reorder elements so that <see cref="ExternalConstraintShape"/> elements
			/// are copied last. Used by the <see cref="CopyOrderComparer"/> and <see cref="Copy"/> methods.
			/// </summary>
			protected static int CompareElementsForCopy(ModelElement element1, ModelElement element2)
			{
				if (element1 == element2)
				{
					return 0;
				}
				int retVal = 0;
				ExternalConstraintShape constraintShape1 = element1 as ExternalConstraintShape;
				ExternalConstraintShape constraintShape2 = element2 as ExternalConstraintShape;
				if (constraintShape1 == null)
				{
					if (constraintShape2 != null)
					{
						retVal = -1;
					}
				}
				else if (constraintShape2 == null)
				{
					retVal = 1;
				}
				return retVal;
			}
			/// <summary>
			/// Support shape merging by reattaching model elements to the shapes
			/// before an attempt is made to merge the shapes into the diagram.
			/// </summary>
			protected override void OnElementsReconstituted(MergeElementGroupEventArgs e)
			{
				ORMDiagram diagram;
				Store targetStore;
				if (null != (diagram = e.TargetElement as ORMDiagram) &&
					diagram.Partition == (targetStore = diagram.Store).DefaultPartition)
				{
					ElementGroupPrototype groupPrototype = e.ElementGroupPrototype;
					object sourceStoreIdObject;
					Guid sourceStoreId;
					Store sourceStore;
					if (groupPrototype.SourceContext.ContextInfo.TryGetValue("SourceStore", out sourceStoreIdObject) &&
						sourceStoreIdObject is Guid &&
						null != (sourceStore = (targetStore.Id == (sourceStoreId = (Guid)sourceStoreIdObject) ? targetStore : FrameworkShellUtility.ResolveLoadedStore<ORMDesignerDocData>(((IORMToolServices)targetStore).ServiceProvider, sourceStoreId))))
					{
						ElementGroup group = e.ElementGroup;
						ReadOnlyCollection<ModelElement> elements = group.ModelElements;
						ReadOnlyCollection<ProtoElement> protoElements = groupPrototype.ProtoElements;
						IElementDirectory elementDirectory = sourceStore.ElementDirectory;
						int elementCount = elements.Count;
						Debug.Assert(elementCount == protoElements.Count);
						if (sourceStore == targetStore)
						{
							for (int i = 0; i < elementCount; ++i)
							{
								PresentationElement pel;
								PresentationElement protoShape;
								ModelElement backingElement;
								if (null != (pel = elements[i] as PresentationElement) &&
									null != (protoShape = elementDirectory.FindElement(protoElements[i].ElementId) as PresentationElement) &&
									null != (backingElement = protoShape.ModelElement))
								{
									pel.Associate(backingElement);
									// Defer to an overridable callback for additional shape-specific fixup
									diagram.ReconstituteMergedShape(pel, protoShape, null);
								}
							}
						}
						else
						{
							object copiedElementsObject;
							IDictionary<Guid, ModelElement> copiedElements;
							if (groupPrototype.TargetContext.ContextInfo.TryGetValue("BackingElementIntegratedCopyClosure", out copiedElementsObject) &&
								null != (copiedElements = copiedElementsObject as IDictionary<Guid, ModelElement>))
							{
								for (int i = 0; i < elementCount; ++i)
								{
									PresentationElement pel;
									PresentationElement protoShape;
									ModelElement sourceBackingElement;
									if (null != (pel = elements[i] as PresentationElement) &&
										null != (protoShape = elementDirectory.FindElement(protoElements[i].ElementId) as PresentationElement) &&
										null != (sourceBackingElement = protoShape.ModelElement) &&
										copiedElements.TryGetValue(sourceBackingElement.Id, out sourceBackingElement))
									{
										pel.Associate(sourceBackingElement);
										// Defer to an overridable callback for additional shape-specific fixup
										diagram.ReconstituteMergedShape(pel, protoShape, copiedElements);
									}
								}
							}
						}
					}
				}
				base.OnElementsReconstituted(e);
			}
			/// <summary>
			/// For an external store, create an element copy closure in a
			/// separate nested transaction so that all elements are properly
			/// constructed before any presentation elements are created.
			/// </summary>
			protected override void OnMerging(MergeElementGroupEventArgs e)
			{
				if (!e.MergeCompleted && e.ElementGroup == null)
				{
					ORMDiagram diagram;
					Store targetStore;
					if (null != (diagram = e.TargetElement as ORMDiagram) &&
						diagram.Partition == (targetStore = diagram.Store).DefaultPartition)
					{
						ElementGroupPrototype groupPrototype = e.ElementGroupPrototype;
						object sourceStoreIdObject;
						Guid sourceStoreId;
						Store sourceStore;
						IDictionary<object, object> sourceContextInfo;
						IDictionary<object, object> targetContextInfo = groupPrototype.TargetContext.ContextInfo;
						if (!targetContextInfo.ContainsKey("BackingElementIntegratedCopyClosure") &&
							(sourceContextInfo = groupPrototype.SourceContext.ContextInfo).TryGetValue("SourceStore", out sourceStoreIdObject) &&
							sourceStoreIdObject is Guid &&
							null != (sourceStore = (targetStore.Id == (sourceStoreId = (Guid)sourceStoreIdObject) ? null : FrameworkShellUtility.ResolveLoadedStore<ORMDesignerDocData>(((IORMToolServices)targetStore).ServiceProvider, sourceStoreId))))
						{
							IDictionary<Guid, IClosureElement> closure;
							object closureObject;
							ICopyClosureManager closureManager = ((IFrameworkServices)sourceStore).CopyClosureManager;
							if (!sourceContextInfo.TryGetValue("BackingElementCopyClosure", out closureObject) ||
								null == (closure = closureObject as IDictionary<Guid, IClosureElement>))
							{
								ReadOnlyCollection<ProtoElement> protoElements = groupPrototype.ProtoElements;
								IElementDirectory elementDirectory = sourceStore.ElementDirectory;
								int elementCount = protoElements.Count;
								List<ModelElement> sourceBackingElements = new List<ModelElement>(elementCount);
								for (int i = 0; i < elementCount; ++i)
								{
									PresentationElement protoShape;
									ModelElement sourceBackingElement;
									if (null != (protoShape = elementDirectory.FindElement(protoElements[i].ElementId) as PresentationElement) &&
										null != (sourceBackingElement = protoShape.ModelElement))
									{
										sourceBackingElements.Add(sourceBackingElement);
									}
								}
								closure = closureManager.GetCopyClosure(sourceBackingElements, targetStore);
								sourceContextInfo["BackingElementCopyClosure"] = closure;
							}
							CopyClosureIntegrationResult integrationResult = closureManager.IntegrateCopyClosure(closure, sourceStore, targetStore, new ModelElement[] { diagram.ModelElement }, true);
							targetContextInfo["BackingElementIntegratedCopyClosure"] = integrationResult.CopiedElements;
							Guid[] missingExtensions = integrationResult.MissingExtensionModels;
							if (missingExtensions != null)
							{
								targetStore.PropertyBag["ORMDiagram.MergeMissingExtensions"] = missingExtensions;
							}
						}
					}
				}
				base.OnMerging(e);
			}
		}
		#endregion // ORMDesignerElementOperations class
		#endregion // IMergeElements implementation
		#region ShapeExtension support
		/// <summary>
		/// Add loaded extension attributes to the standard toolbox items attributes
		/// </summary>
		public override ICollection TargetToolboxItemFilterAttributes
		{
			get
			{
				ICollection baseAttributes = base.TargetToolboxItemFilterAttributes;
				IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
				if (extenders != null)
				{
					ICollection[] extenderAttributeSets = null;
					int extenderCount = extenders.Length;
					int extenderAttributeCount = 0;
					for (int i = 0; i < extenderCount; ++i)
					{
						ICollection extenderAttributes = extenders[i].GetToolboxFilterAttributes() as ICollection;
						int attributeCount;
						if (extenderAttributes != null &&
							0 != (attributeCount = extenderAttributes.Count))
						{
							extenderAttributeCount += attributeCount;
							(extenderAttributeSets ?? (extenderAttributeSets = new ICollection[extenderCount]))[i] = extenderAttributes;
						}
					}
					if (extenderAttributeCount != 0)
					{
						int baseCount = (baseAttributes != null) ? baseAttributes.Count : 0;
						ToolboxItemFilterAttribute[] allAttributes = new ToolboxItemFilterAttribute[baseCount + extenderAttributeCount];
						baseAttributes.CopyTo(allAttributes, 0);
						int copyToIndex = baseCount;
						for (int i = 0; i < extenderCount; ++i)
						{
							ICollection extenderCollection = extenderAttributeSets[i];
							if (extenderCollection != null)
							{
								extenderCollection.CopyTo(allAttributes, copyToIndex);
								copyToIndex += extenderCollection.Count;
							}
						}
						return allAttributes;
					}
				}
				return baseAttributes;
			}
		}
		/// <summary>
		/// Defer child shape creation to <see cref="IShapeExtender{ORMDiagram}"/> implementations as needed
		/// </summary>
		protected override ShapeElement CreateChildShape(ModelElement element)
		{
			ShapeElement retVal = base.CreateChildShape(element);
			if (retVal == null)
			{
				IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
				if (extenders != null)
				{
					for (int i = 0; i < extenders.Length && retVal == null; ++i)
					{
						retVal = extenders[i].CreateChildShape(this, element);
					}
				}
			}
			return retVal;
		}
		#endregion // ShapeExtension support
		#region IORMExtendableElement implementation
		LinkedElementCollection<ModelError> IORMExtendableElement.ExtensionModelErrorCollection
		{
			get
			{
				return null;
			}
		}
		#endregion // IORMExtendableElement implementation
		#region IVerbalizeCustomChildren Implementation
		/// <summary>
		/// Implements <see cref="IVerbalizeCustomChildren.GetCustomChildVerbalizations"/>.
		/// Explicitly verbalizes extension elements
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign)
		{
			foreach (ModelElement extensionElement in ExtensionCollection)
			{
				IVerbalize verbalizeExtension = extensionElement as IVerbalize;
				if (verbalizeExtension != null)
				{
					yield return CustomChildVerbalizer.VerbalizeInstance(verbalizeExtension);
				}
			}
		}
		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign)
		{
			return GetCustomChildVerbalizations(filter, verbalizationOptions, sign);
		}
		#endregion // IVerbalizeCustomChildren Implementation
	}
	#region ORMShapeDomainModel toolbox initialization
	[ModelingToolboxItemProvider("ToolboxInitializer")]
	partial class ORMShapeDomainModel
	{
		private sealed class ToolboxInitializer : IModelingToolboxItemProvider
		{
			#region IModelingToolboxItemProvider Implementation
			IList<ModelingToolboxItem> IModelingToolboxItemProvider.CreateToolboxItems(IServiceProvider serviceProvider)
			{
				IList<ModelingToolboxItem> items;
				FrameworkDomainModel.InitializingToolboxItems = true;
				try
				{
					items = new ORMShapeToolboxHelper(serviceProvider).CreateToolboxItems();
				}
				finally
				{
					FrameworkDomainModel.InitializingToolboxItems = false;
				}

				// Add additional filter strings. These are not easily specified in the .dsl file, so we
				// do it here.
				IDictionary<string, int> itemIndexDictionary = ToolboxHelperUtility.CreateIdentifierToIndexMap(items);

				ToolboxItemFilterAttribute attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramInternalUniquenessConstraintFilterString, ToolboxItemFilterType.Allow);
				ToolboxHelperUtility.AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxInternalUniquenessConstraintItemId, attribute);

				attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramConnectInternalUniquenessConstraintFilterString, ToolboxItemFilterType.Allow);
				ToolboxHelperUtility.AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxInternalUniquenessConstraintConnectorItemId, attribute);

				attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramModelNoteFilterString, ToolboxItemFilterType.Allow);
				ToolboxHelperUtility.AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxModelNoteItemId, attribute);

				attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramExternalConstraintFilterString, ToolboxItemFilterType.Allow);
				string[] itemIds = new string[] {
					ResourceStrings.ToolboxEqualityConstraintItemId,
					ResourceStrings.ToolboxExclusionConstraintItemId,
					ResourceStrings.ToolboxExclusiveOrConstraintItemId,
					ResourceStrings.ToolboxExternalUniquenessConstraintItemId,
					ResourceStrings.ToolboxInclusiveOrConstraintItemId,
					ResourceStrings.ToolboxRingConstraintItemId,
					ResourceStrings.ToolboxValueComparisonConstraintItemId,
					ResourceStrings.ToolboxSubsetConstraintItemId,
					ResourceStrings.ToolboxFrequencyConstraintItemId};
				for (int i = 0; i < itemIds.Length; ++i)
				{
					ToolboxHelperUtility.AddFilterAttribute(items, itemIndexDictionary, itemIds[i], attribute);
				}
				return items;
			}
			int IModelingToolboxItemProvider.ToolboxItemPositionOffset
			{
				get
				{
					return 0;
				}
			}
			#endregion // IModelingToolboxItemProvider Implementation
		}
	}
	#endregion // ORMShapeDomainModel toolbox initialization
	#region ORMDiagramDynamicColor enum
	/// <summary>
	/// Specify the color role for a dynamic shape color
	/// </summary>
	[TypeConverter(typeof(EnumConverter<ORMDiagramDynamicColor, ORMDiagram>))]
	[ResourceAccessorCategory(typeof(ORMDiagram), "ORMDiagramDynamicColor.Category")]
	public enum ORMDiagramDynamicColor
	{
		/// <summary>
		/// Get the background color
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.Background.Description")]
		Background,
		/// <summary>
		/// Get the foreground color
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.ForegroundGraphics.Description")]
		ForegroundGraphics,
		/// <summary>
		/// Shape foreground text color
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.ForegroundText.Description")]
		ForegroundText,
		/// <summary>
		/// The outline color for a shape
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.Outline.Description")]
		Outline,
		/// <summary>
		/// Constraint color (alethic modality)
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.Constraint.Description")]
		Constraint,
		/// <summary>
		/// Deontic constraint color
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.DeonticConstraint.Description")]
		DeonticConstraint,
		/// <summary>
		/// Role player connector color
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.RolePlayerConnector.Description")]
		RolePlayerConnector,
		/// <summary>
		/// Shape floating text color
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.FloatingText.Description")]
		FloatingText,
	}
	#endregion // ORMDiagramDynamicColor enum
	#region IDynamicColorSetConsumer implementation
	partial class ORMShapeDomainModel : IDynamicColorSetConsumer
	{
		#region IDynamicColorSetConsumer Implementation
		/// <summary>
		/// Implements <see cref="IDynamicColorSetConsumer.GetDynamicColorSet"/>
		/// </summary>
		protected static Type GetDynamicColorSet(Type renderingType)
		{
			if (renderingType == typeof(ORMDiagram))
			{
				return typeof(ORMDiagramDynamicColor);
			}
			return null;
		}
		Type IDynamicColorSetConsumer.GetDynamicColorSet(Type renderingType)
		{
			return GetDynamicColorSet(renderingType);
		}
		#endregion // IDynamicColorSetConsumer implementation
	}
	#endregion // IDynamicColorSetConsumer implementation
	#region INotifyCultureChange implementation
	partial class ORMShapeDomainModel : INotifyCultureChange
	{
		/// <summary>
		/// Implements <see cref="INotifyCultureChange.CultureChanged"/>
		/// </summary>
		protected void CultureChanged()
		{
			// Note that the calling code will have an open transaction
			// Culture changes can affect value constraint shapes
			foreach (ValueConstraintShape constraintShape in Store.ElementDirectory.FindElements<ValueConstraintShape>(true))
			{
				constraintShape.InvalidateDisplayText();
			}
		}
		void INotifyCultureChange.CultureChanged()
		{
			CultureChanged();
		}
	}
	#endregion // INotifyCultureChange implementation
	#region IRegisterSignalChanges interface
	partial class ORMShapeDomainModel : IRegisterSignalChanges
	{
		#region IRegisterSignalChanges Implementation
		/// <summary>
		/// Implements <see cref="IRegisterSignalChanges.GetSignalPropertyChanges"/>
		/// </summary>
		protected IEnumerable<KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>> GetSignalPropertyChanges()
		{
			// The core diagram processing creates a number of signal changes
			// that are meaningless outside of the transaction. They also
			// create a meaningless change for the EdgePoints property on a
			// link shape. These are all registered here because the core doesn't
			// know about this method.
			yield return new KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>(Diagram.DoLineRoutingDomainPropertyId, null);
			yield return new KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>(Diagram.DoResizeParentDomainPropertyId, null);
			yield return new KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>(Diagram.DoShapeAnchoringDomainPropertyId, null);
			yield return new KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>(Diagram.DoViewFixupDomainPropertyId, null);
			yield return new KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>(LinkShape.EdgePointsDomainPropertyId, delegate(ElementPropertyChangedEventArgs e)
			{
				object oldVal = e.OldValue;
				object newVal = e.NewValue;
				if (oldVal == null || newVal == null)
				{
					return false;
				}
				EdgePointCollection oldPoints = (EdgePointCollection)oldVal;
				EdgePointCollection newPoints = (EdgePointCollection)newVal;
				int pointCount = oldPoints.Count;
				if (pointCount != newPoints.Count)
				{
					return false;
				}
				for (int i = 0; i < pointCount; ++i)
				{
					EdgePoint oldPoint = oldPoints[i];
					EdgePoint newPoint = newPoints[i];
					if (oldPoint.Flag != newPoint.Flag ||
						oldPoint.Point != newPoint.Point)
					{
						return false;
					}
				}
				return true;
			});

			// Our UpdateCounter properties are also signals that should not trigger a user-visible change.
			yield return new KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>(ORMBaseShape.UpdateCounterDomainPropertyId, null);
			yield return new KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>(ORMBaseBinaryLinkShape.UpdateCounterDomainPropertyId, null);
			yield return new KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>(FactTypeShape.RoleNameVisibilityChangedDomainPropertyId, null);
		}
		IEnumerable<KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>> IRegisterSignalChanges.GetSignalPropertyChanges()
		{
			return GetSignalPropertyChanges();
		}
		#endregion // IRegisterSignalChanges Implementation
	}
	#endregion // IRegisterSignalChanges interface
	#region ORMPlacementOption enum
	/// <summary>
	/// Controls the actions taken when placing a <see cref="ModelElement"/> on a <see cref="Diagram"/>.
	/// </summary>
	[Serializable]
	public enum ORMPlacementOption
	{
		/// <summary>
		/// No special placement actions should be taken.
		/// </summary>
		None = 0,
		/// <summary>
		/// Select a <see cref="ShapeElement"/> for the <see cref="ModelElement"/> on the <see cref="Diagram"/> if one already exists.
		/// </summary>
		SelectIfNotPlaced = 1,
		/// <summary>
		/// Allow addition of a new <see cref="ShapeElement"/> for the <see cref="ModelElement"/>.
		/// </summary>
		AllowMultipleShapes = 2,
		/// <summary>
		/// Force create of a new <see cref="ShapeElement"/> for the <see cref="ModelElement"/>
		/// </summary>
		CreateNewShape = 3,
	}
	#endregion // ORMPlacementOption enum
	#region ORMDiagramBase class
	partial class ORMDiagramBase
	{
		/// <summary>
		/// Set during view fixup to create a <see cref="RolePlayerProxyLink"/> instead
		/// of a <see cref="RolePlayerLink"/> when binding connectors to an <see cref="ObjectTypePlaysRole"/>
		/// relationship.
		/// </summary>
		public static readonly object CreatingRolePlayerProxyLinkKey = new object();
		/// <summary>
		/// Set during view fixup to always create a <see cref="RolePlayerLink"/> and not
		/// attempt to create any <see cref="RolePlayerProxyLink"/> for a role player.
		/// </summary>
		public static readonly object CreatingRolePlayerLinkKey = new object();
		private NodeShape CreateShapeForObjectType(ObjectType newElement)
		{
			return FactTypeShape.ShouldDrawObjectification(newElement.NestedFactType) ? (NodeShape)new ObjectifiedFactTypeNameShape(this.Partition) : new ObjectTypeShape(this.Partition);
		}
		private LinkShape CreateConnectorForObjectTypePlaysRole(ObjectTypePlaysRole newElement)
		{
			return this.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey(CreatingRolePlayerProxyLinkKey) ? (LinkShape)new RolePlayerProxyLink(this.Partition) : new RolePlayerLink(this.Partition);
		}
		private long GetUpdateCounterValue()
		{
			return ORMShapeDomainModel.GetCurrentUpdateCounterValue(this);
		}
		private void SetUpdateCounterValue(long newValue)
		{
			// Nothing to do, we're just trying to create a transaction log entry
		}
	}
	#endregion // ORMDiagramBase class
	#region ZOrderLayer class
	/// <summary>
	/// Diagram shapes zorder is determined automatically by the relative
	/// creation order of the shapes and connectors. By default, this means
	/// that connectors display above the shapes they are connected to.
	/// However, if a connector is attached to another shape, then it may
	/// display behind the other shape, giving an inconsistent (or possibly
	/// incorrent) display. The ZOrderLayer values provide an offset to
	/// add to the explicit ZOrder so that different shape types always
	/// layer in a consistent fashion.
	/// </summary>
	/// <remarks>These values are intentionally static properties instead
	/// of constants so that extensions get updated values without recompilation.</remarks>
	public static class ZOrderLayer
	{
		/// <summary>
		/// Put note connectors behind all other shapes
		/// </summary>
		public static readonly double NoteConnectors = 0d;
		/// <summary>
		/// The minimum zorder order range used by native shapes.
		/// </summary>
		public static readonly double FirstLayerBeginning = NoteConnectors;
		/// <summary>
		/// Place object type shapes at the bottom
		/// </summary>
		public static readonly double ObjectTypeShapes = 100000d;
		/// <summary>
		/// Place fact types shapes over object type shapes. The fact type
		/// shapes are generally smaller, so they can remain selectable
		/// in cases of overlap.
		/// </summary>
		public static readonly double FactTypeShapes = 200000d;
		/// <summary>
		/// Place external constraint shapes above base shapes to keep them selectable.
		/// </summary>
		public static readonly double ExternalConstraintShapes = 300000d;
		/// <summary>
		/// Place note shapes, which have a transparent backgroun by default,
		/// above other shapes.
		/// </summary>
		public static readonly double NoteShapes = 400000d;
		/// <summary>
		/// Subtype connectors go over all shapes. These lines are drawn wider
		/// than other connectors, so placing them before other connector types
		/// keeps them below the other conector types.
		/// </summary>
		public static readonly double SubtypeConnectors = 500000d;
		/// <summary>
		/// Role player connectors draw over subtype connectors and below
		/// constraint connectors.
		/// </summary>
		public static readonly double RolePlayerConnectors = 600000d;
		/// <summary>
		/// Draw the lightweight constraint connector lines above other connector types.
		/// </summary>
		public static readonly double ConstraintConnectors = 700000d;
		/// <summary>
		/// Show transparent value constraint shapes above intersecting connectors.
		/// </summary>
		public static readonly double ValueConstraintShapes = 800000d;
		/// <summary>
		/// Place floating text shapes (reading, objectified fact type name, and role name)
		/// above other shapes.
		/// </summary>
		public static readonly double FloatingTextShapes = 900000d;
		/// <summary>
		/// The end of the maximum zorder order range used by native shapes.
		/// </summary>
		public static readonly double MaximumLayerEnd = 1000000d;
	}
	#endregion // ZOrderLayer class
}
