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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.Modeling;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Text.RegularExpressions;

namespace Neumont.Tools.ORM.Shell.FactEditor
{
	/// <summary>
	/// Use to draw a FactLine on the model.
	/// </summary>
	public struct FactSaver
	{
		private ORMDesignerDocView myCurrentDocView;
		private ORMDesignerDocData myCurrentDocument;
		private FactType myEditFact;
		private ParsedFact myParsedFact;

		/// <summary>
		/// Add facts to the model
		/// </summary>
		/// <param name="docView">Curent document view</param>
		/// <param name="parsedFact">Parsed fact</param>
		/// <param name="fact">Editing fact</param>
		[CLSCompliant(false)]
		public static void AddFact(ORMDesignerDocView docView, ParsedFact parsedFact, FactType fact)
		{
			(new FactSaver(docView, parsedFact, fact)).AddFactToModel();
		}

		private FactSaver(ORMDesignerDocView docView, ParsedFact parsedFact, FactType fact)
		{
			myCurrentDocView = docView;
			myCurrentDocument = docView.DocData as ORMDesignerDocData;
			myEditFact = fact;
			myParsedFact = parsedFact;
		}

		private void AddFactToModel()
		{
			ORMModel myModel = null;
			Store store = myCurrentDocument.Store;
			myModel = store.ElementDirectory.FindElements<ORMModel>()[0];

			if (null != myModel)
			{
				ORMDesignerDocView docView = (ORMDesignerDocView)myCurrentDocument.DocViews[0];
				ORMDiagram diagram = docView.CurrentDiagram as ORMDiagram;
				LayoutManager layoutManager = new LayoutManager(diagram as ORMDiagram, (diagram.Store as IORMToolServices).GetLayoutEngine(typeof(ORMRadialLayoutEngine)));
				List<ModelElement> newlyCreatedElements = new List<ModelElement>();

				// We've got a model, now lets start a transaction to add our fact to the model.
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.InterpretFactEditorLineTransactionName))
				{
					Dictionary<object, object> topLevelTransactionContextInfo = t.TopLevelTransaction.Context.ContextInfo;
					topLevelTransactionContextInfo[ORMBaseShape.PlaceAllChildShapes] = null;
					LinkedElementCollection<RoleBase> factRoles = null;
					ReadingOrder readOrd;
					Reading primaryReading = null;
					FactType startingFact = myEditFact;
					Dictionary<string, ObjectType> newlyCreatedObjects = new Dictionary<string, ObjectType>();

					// Make sure that we aren't trying to use the objectifying type
					// of the active fact as one of the objects.
					if (startingFact != null)
					{
						ObjectType nestingType = startingFact.NestingType;
						if (nestingType != null)
						{
							string nestingName = nestingType.Name;
							foreach (FactObject factObject in myParsedFact.FactObjects)
							{
								if (nestingName == factObject.Name)
								{
									startingFact = null;
									break;
								}
							}
						}
					}

					FactType currentFact;
					// Get the fact if it exists, otherwise create a new one.
					if (startingFact == null)
					{
						currentFact = new FactType(store);
						newlyCreatedElements.Add(currentFact);
						readOrd = new ReadingOrder(store);
						// assign the fact to this reading order to setup the reference
						// between role collectionsreadOrd.FactType = currentFact;
						readOrd.FactType = currentFact;
						currentFact.Model = myModel;
					}
					else
					{
						currentFact = startingFact;
						readOrd = currentFact.FindMatchingReadingOrder(currentFact.RoleCollection);
						primaryReading = readOrd.PrimaryReading;
					}


					factRoles = currentFact.RoleCollection;
					factRoles.Clear();

					// Loop the FactObjectCollection and create new objects or update existing objects
					// (this means assign refmodes and change between entity/value types.
					foreach (FactObject o in myParsedFact.FactObjects)
					{
						string objectName = o.Name;
						bool isEmptyElement = true;
						bool forceValueType = (o.RefMode.Length == 0 && o.RefModeHasParenthesis);
						ObjectType currentObject = null;

						// Get the object if it already exists.
						LocatedElement existingElement = myModel.ObjectTypesDictionary.GetElement(objectName);
						isEmptyElement = existingElement.IsEmpty;

						// If the object was not found, create a new one.
						// Or if the object exists and the names are different, create a new one
						if (isEmptyElement)
						{
							currentObject = new ObjectType(store);
							currentObject.Name = objectName;
							currentObject.Model = myModel;
							newlyCreatedObjects.Add(objectName, currentObject);
							newlyCreatedElements.Add(currentObject);

							// If the object DOES NOT already exist AND it's a value type
							if (forceValueType)
							{
								currentObject.IsValueType = true;
							}
							// If the object DOES NOT already exist AND it's an entity type
							else if (o.RefMode.Length > 0)
							{
								IList<ReferenceMode> modes = ReferenceMode.FindReferenceModesByName(o.RefMode, myModel);
								if (modes.Count == 0)
								{
									currentObject.ReferenceModeString = o.RefMode;
								}
								else
								{
									// UNDONE: Consider giving a warning here (after the transaction is
									// completed, no UI during transactions) that the reference mode is ambiguous.
									currentObject.ReferenceMode = modes[0];
								}
							}
						} // Otherwise, use the existing object.
						else
						{
							if (newlyCreatedObjects.ContainsKey(objectName))
							{
								currentObject = newlyCreatedObjects[objectName];
							}
							else
							{
								currentObject = existingElement.FirstElement as ObjectType;
							}
							bool convertingToValueType = false;
							bool entityWasCollapsed = false;
							string refModeText = o.RefMode;
							int refModeLength = refModeText.Length;
							bool currentObjectIsValueType = currentObject.IsValueType;

							// get a presentation element to work with for determine if the ref mode is expanded or collapsed
							ShapeModel.ObjectTypeShape objShape = (myCurrentDocView.CurrentDiagram as ORMDiagram).FindShapeForElement<ShapeModel.ObjectTypeShape>(currentObject);
							layoutManager.AddShape(objShape, true);

							// convert this object to a entity type if it was a value type and if we are now adding a ref mode
							if (refModeLength > 0 && currentObjectIsValueType)
							{
								currentObject.IsValueType = false;
								// Note: Don't change the ExpandRefMode property to false here. This is
								// a user setting and we should respect the current value.
							}
							else if (refModeLength == 0 && !currentObjectIsValueType && o.RefModeHasParenthesis)
							{
								// Set the flag to indicate we are converting an entity to a value type
								// so we need to kill the RefMode
								convertingToValueType = true;

								// Set the flag to indicate if the old entity type was collapsed. If it
								// was, then we are ok to agressively delete the ref mode
								if (objShape != null)
								{
									entityWasCollapsed = !objShape.ExpandRefMode;
								}
							}

							// UNDONE: make sure we don't set a RefMode on an objectified fact; don't make
							// the objectification a value type either. Use squiggles with markers/regions
							// to block the commit of invalid operations

							if (currentObject.NestedFactType == null)
							{
								// if convertingToValueType, then we know we have parens and 0 length ref mode - force value type
								// if we have a ref mode and it's different than the old one, change it
								if (convertingToValueType || (!currentObject.IsValueType && refModeLength > 0 && currentObject.ReferenceModeString != refModeText))
								{
									IList<ReferenceMode> modes = ReferenceMode.FindReferenceModesByName(refModeText, myModel);
									if (modes.Count == 0)
									{
										// Add a "Delete" object to the transaction bucket to enable agressive RefMode deletion
										if (convertingToValueType && entityWasCollapsed)
										{
											topLevelTransactionContextInfo[ObjectType.DeleteReferenceModeValueType] = null;
										}
										currentObject.ReferenceModeString = refModeText;

										// remove the "Delete" object from the transaction bucket
										if (convertingToValueType && entityWasCollapsed)
										{
											topLevelTransactionContextInfo.Remove(ObjectType.DeleteReferenceModeValueType);
										}
									}
									else
									{
										// UNDONE: Consider giving a warning here (after the transaction is
										// completed, no UI during transactions) that the reference mode is ambiguous.
										currentObject.ReferenceMode = modes[0];
									}

									// The object was an entity, but is now a value type
									if (convertingToValueType)
									{
										currentObject.IsValueType = true;
									}
								}
								// ELSE:
								// if we have a ref mode and it's the same ignore it.
								// if ref mode is blank and !convertingToValueType, then we need to ignore the ref mode all together


							} // end of if (currentObject.NestedFactType == null)
						} // end of use existing object

						// Add this object to the fact role collection, default the role name to the object name
						Role role = new Role(store);
						role.RolePlayer = currentObject;
						factRoles.Add(role);
					} // end foreach (FactObject o in myParsedFact.FactObjects)

					// If we're creating a new fact, add the reading to the reading collection
					if (startingFact == null)
					{
						primaryReading = new Reading(store);
						primaryReading.ReadingOrder = readOrd;
					}
					primaryReading.Text = myParsedFact.ReadingText;

					// FACT-QUANTIFIERS: This is for the new quantifier implementation using the "." delimiters
					// Apply a mandatory dot to the opposite role
//					for (int i = 0; i < factRoles.Count; i++)
//					{
//						Role currentRole = factRoles[i];
//						FactObject currentObject = myParsedFact.FactObjects[i];
//
//						foreach (FactQuantifier fq in currentObject.RoleQuantifiers)
//						{
//							// AtLeast means MANDATORY ONLY
//							if (fq.QuantifierType == LogicalQuantifierType.AtLeast)
//							{
//								// apply the mandatory dot to the opposite role
//								Role oppositeRole = currentRole.OppositeRole;
//								if (oppositeRole != null)
//								{
//									oppositeRole.IsMandatory = true;
//								}
//								
//							}
//							else if (fq.QuantifierType == LogicalQuantifierType.Exactly)
//							{
//								currentRole.Multiplicity = RoleMultiplicity.ExactlyOne;
//							}
//							else if (fq.QuantifierType == LogicalQuantifierType.AtMost)
//							{
//								currentRole.Multiplicity = RoleMultiplicity.ZeroToOne;
//							}
//						}
//					}

					// Commit the changes to the model.
					t.Commit();
				} // end transaction

				#region Autolayout
				// Only perform autlayout if new objects have been created AND there are objects
				// in the modelElements collection.
				if (newlyCreatedElements.Count > 0)
				{
					// Create a new transaction to perform autolayout (You cannot do this inside the same transaction)
					using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.InterpretFactEditorLineTransactionName))
					{
						// New stuff for autolayout
						foreach (ModelElement modelElement in newlyCreatedElements)
						{
							ShapeElement shapeElement = diagram.FindShapeForElement(modelElement);
							if (shapeElement != null)
							{
								layoutManager.AddShape(shapeElement, false);
							}
						}
						layoutManager.Layout();

						t.Commit();
					}
				}
				#endregion // Autolayout
			}
		}
	}
}
