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
using System.Diagnostics;

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
		/// <param name="docData">Curent document data</param>
		/// <param name="docView">Curent document view</param>
		/// <param name="parsedFact">Parsed fact</param>
		/// <param name="fact">Editing fact</param>
		[CLSCompliant(false)]
		public static FactType AddFact(ORMDesignerDocData docData, ORMDesignerDocView docView, ParsedFact parsedFact, FactType fact)
		{
			return (new FactSaver(docData, docView, parsedFact, fact)).AddFactToModel();
		}

		private FactSaver(ORMDesignerDocData docData, ORMDesignerDocView docView, ParsedFact parsedFact, FactType fact)
		{
			myCurrentDocView = docView;
			myCurrentDocument = docData;
			myEditFact = fact;
			myParsedFact = parsedFact;
		}

		private FactType AddFactToModel()
		{
			ORMModel myModel = null;
			Store store = myCurrentDocument.Store;
			myModel = store.ElementDirectory.FindElements<ORMModel>()[0];
			FactType currentFact = null;

			if (null != myModel)
			{
				ORMDesignerDocView docView = myCurrentDocView;
				ORMDiagram diagram = (docView != null) ? docView.CurrentDiagram as ORMDiagram : null;
				LayoutManager layoutManager = (diagram != null) ? new LayoutManager(diagram, (diagram.Store as IORMToolServices).GetLayoutEngine(typeof(ORMRadialLayoutEngine))) : null;
				List<ModelElement> newlyCreatedElements = new List<ModelElement>();

				// We've got a model, now lets start a transaction to add our fact to the model.
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.InterpretFactEditorLineTransactionName))
				{
					Dictionary<object, object> topLevelTransactionContextInfo = t.TopLevelTransaction.Context.ContextInfo;
					topLevelTransactionContextInfo[ORMBaseShape.PlaceAllChildShapes] = null;
					LinkedElementCollection<RoleBase> factRoles = null;
					ReadingOrder readOrd;
					Reading editedReading = null;
					FactType startingFact = myEditFact;
					Dictionary<string, ObjectType> newlyCreatedObjects = new Dictionary<string, ObjectType>();

					// Make sure that we aren't trying to use the objectifying type
					// of the active fact as one of the objects.
					if (startingFact != null)
					{
						startingFact = ObjectificationCheck(startingFact);
					}

					// Get the fact if it exists, otherwise create a new one.
					#region Check if Fact exists
					if (startingFact == null)
					{
						currentFact = new FactType(store);
						newlyCreatedElements.Add(currentFact);
						InitializeReadingOrder(currentFact);
						currentFact.Model = myModel;
					}
					else
					{
						currentFact = startingFact;
					}
					#endregion

					int newFactArity = ((IList<FactObject>)(myParsedFact.FactObjects)).Count;

					//Shallow copy of the role collection, we will use this to update/modify the role collection according to the new fact
					factRoles = currentFact.RoleCollection;

					//checks for arity change because contraints will need to be wiped if the arity changed
					bool arityChanged = false;
					if (newFactArity != factRoles.Count)
					{
						arityChanged = true;
					}

					//if the new fact does not have the same number of roles as the original fact (will also trigger if it is a new fact and not one being editted)
					if (arityChanged)
					{
						//Adjusts role collection to have the proper number of roles and clear all constraints
						PrepRoleCollection(store, factRoles, currentFact, newFactArity, ref editedReading);
					}
					else
					{
						readOrd = null;
						readOrd = myParsedFact.ReadingToEdit.ReadingOrder;

						//If reading order exists, assign the reading to readOrd's primary reading
						if (readOrd != null)
						{
							//editedReading = readOrd.ReadingCollection[0];
							editedReading = myParsedFact.ReadingToEdit;
						}

					}

					//Counter used to cycle through the role collection and assign role players
					int factCounter = -1;

					// Loop the FactObjectCollection and create new objects or update existing objects
					// (this means assign refmodes and change between entity/value types.
					#region FactObjectCollection Loop
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
							ObjectTypeShape objShape = null;
							if (diagram != null)
							{
								objShape = diagram.FindShapeForElement<ObjectTypeShape>(currentObject);
								layoutManager.AddShape(objShape, true);
								LinkedElementCollection<Role> playedRoles = currentObject.PlayedRoleCollection;
								int playedRoleCount = playedRoles.Count;
								for (int i = 0; i < playedRoleCount; ++i)
								{
									layoutManager.AddShape(diagram.FindShapeForElement<FactTypeShape>(playedRoles[i].FactType), true);
								}
							}

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

						//Update roleplayers 
						if (editedReading == null ||
							myParsedFact.FactObjects.Count == 2)
						{
							factRoles[++factCounter].Role.RolePlayer = currentObject;
						}
						else
						{
							myParsedFact.ReadingToEdit.ReadingOrder.RoleCollection[++factCounter].Role.RolePlayer = currentObject;
						}

					} // end foreach (FactObject o in myParsedFact.FactObjects)
					if (factRoles.Count <= 0)
					{
						factRoles = myParsedFact.ReadingToEdit.ReadingOrder.RoleCollection;
					}
					#endregion

					LinkedElementCollection<ReadingOrder> readingOrders = currentFact.ReadingOrderCollection;
					CleanReadings(readingOrders, factRoles);
					// If we're creating a new fact, add the reading to the reading collection
					if (editedReading == null)
					{
						if (arityChanged && startingFact != null)
						{
							editedReading = readingOrders[0].ReadingCollection[0];
						}
						else
						{
							editedReading = new Reading(store);
							editedReading.ReadingOrder = readingOrders[0];
						}
					}
					editedReading.Text = myParsedFact.ReadingText;

					//if fact is a binary
					if (factRoles.Count == 2)
					{
						//if there is an inverse (reverse) reading
						if (!string.IsNullOrEmpty(myParsedFact.ReverseReadingText))
						{
							//if there is currently only one (forward) reading
							switch (readingOrders.Count)
							{
								case 1:
									{
										//add the inverse reading
										newlyCreatedElements.Add(currentFact);
										ReadingOrder reverseReadOrd = new ReadingOrder(store);
										Reading reverseReading = new Reading(store, new PropertyAssignment(Reading.TextDomainPropertyId, myParsedFact.ReverseReadingText));
										reverseReading.ReadingOrder = reverseReadOrd;
										LinkedElementCollection<RoleBase> newOrderRoles = reverseReadOrd.RoleCollection;
										LinkedElementCollection<RoleBase> forwardOrderRoles = editedReading.ReadingOrder.RoleCollection;
										newOrderRoles.Add(forwardOrderRoles[1]);
										newOrderRoles.Add(forwardOrderRoles[0]);
										reverseReadOrd.FactType = currentFact;
										break;
									}
								case 2:
									//edit the inverse reading
									if (readingOrders[0].ReadingText == editedReading.Text)
									{
										readingOrders[1].ReadingCollection[0].Text = myParsedFact.ReverseReadingText;
									}
									else
									{
										readingOrders[0].ReadingCollection[0].Text = myParsedFact.ReverseReadingText;
									}
									break;
							}
						}
						//if the fact had a reverse reading, but it has been deleted we need to trim the reverse
						//reading from the collection (because the count is 2 but should be 1 due to no inverse reading
						//being present)
						else if (currentFact.ReadingOrderCollection.Count == 2)
						{
							int index = 1;
							//if the parsed fact does not have the same reading order as the primary reading
							if (myParsedFact.FactObjects[0].Name != readingOrders[0].RoleCollection[0].Role.RolePlayer.Name)
							{
								index = 0;
							}
							//if the reading being deleted is the primary reading
							if (readingOrders[index].ReadingCollection.Count > 1)
							{
								readingOrders[index].ReadingCollection.RemoveAt(0);
							}
							else
							{
								readingOrders.RemoveAt(index);
							}
						}
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				} // end transaction

				#region Autolayout
				if (diagram != null)
				{
					// Only perform autlayout if new objects have been created AND there are objects
					// in the modelElements collection.
					if (newlyCreatedElements.Count != 0)
					{
						AutoLayout(store, diagram, layoutManager, newlyCreatedElements);
					}
				}
				#endregion // Autolayout
			}
			return currentFact;
		}

		private void CleanReadings(LinkedElementCollection<ReadingOrder> readingOrders, LinkedElementCollection<RoleBase> factRoles)
		{
			//deletes any readings that contain roles that are no longer in the fact because they haven't been clearing out automatically
			int factRoleCount = factRoles.Count;
			int orderCount = readingOrders.Count;
			for (int i = orderCount - 1; i >= 0; --i)
			{
				LinkedElementCollection<RoleBase> orderRoles = readingOrders[i].RoleCollection;
				for (int j = 0; j < factRoleCount; ++j)
				{
					if (!orderRoles.Contains(factRoles[j]))
					{
						readingOrders.RemoveAt(i);
					}
				}
			}
			//deletes any duplicate readings
		}


		/// <summary>
		/// Initialize the new reading order without adding any role associations to it.
		/// </summary>
		/// <param name="factType">The <see cref="FactType"/> to attach the new reading order to.</param>
		/// <returns>A newly created <see cref="ReadingOrder"/> attached to the specified <paramref name="factType"/></returns>
		private static ReadingOrder InitializeReadingOrder(FactType factType)
		{
			ReadingOrder retVal = new ReadingOrder(factType.Store);
			retVal.FactType = factType;
			return retVal;
		}

		//Reinitializes a role collection to have the same number of roles as the fact it is being edited to
		//This is used when the arity changes or when a new fact is being entered
		private void PrepRoleCollection(Store store, LinkedElementCollection<RoleBase> factRoles, FactType currentFact, int newFactArity, ref Reading primaryReading)
		{
			//Loop through all the roles in the collection and reset their contraints
			//(Note that roleplayers are not yet assigned)
			int factRoleCount = factRoles.Count;
			Debug.Assert(factRoleCount != newFactArity);
			for (int i = 0; i < factRoleCount; ++i)
			{
				Role role = factRoles[i].Role;
				role.ConstraintRoleSequenceCollection.Clear();
				role.ValueConstraint = null;
			}

			//Clears all Reading Orders/Readings except for the first one
			LinkedElementCollection<ReadingOrder> readingOrders = currentFact.ReadingOrderCollection;
			int orderCount = readingOrders.Count;
			if (orderCount > 1)
			{
				readingOrders.RemoveRange(1, orderCount - 1);
			}

			//if the new fact is larger than the original (or it is new to the model entirely)
			int arityDisparity = newFactArity - factRoleCount;
			if (arityDisparity > 0)
			{
				//creates the necessary new roles and adds them to the collection
				for (; arityDisparity != 0; --arityDisparity)
				{
					factRoles.Add(new Role(store));
				}
			}
			//if the new fact is smaller than the original fact, we need to trim roles
			else
			{
				factRoles.RemoveRange(factRoleCount + arityDisparity, -arityDisparity);
			}
			if (orderCount != 0) // defensive
			{
				LinkedElementCollection<Reading> readings = readingOrders[0].ReadingCollection;
				int readingCount = readings.Count;
				if (readingCount > 1)
				{
					readings.RemoveRange(1, readingCount - 1);
				}
			}
			primaryReading = null;
		}

		private static void AutoLayout(Store store, ORMDiagram diagram, LayoutManager layoutManager, List<ModelElement> newlyCreatedElements)
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

		private FactType ObjectificationCheck(FactType startingFact)
		{
			ObjectType nestingType = startingFact.NestingType;
			if (startingFact.NestingType != null)
			{
				string nestingName = nestingType.Name;
				foreach (FactObject factObject in myParsedFact.FactObjects)
				{
					if (nestingName == factObject.Name)
					{
						return null;
					}
				}
			}
			return startingFact;
		}

	}
}
