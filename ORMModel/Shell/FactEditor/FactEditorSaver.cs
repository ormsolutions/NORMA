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
using System.Text;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	partial class FactEditorLanguageService
	{
		/// <summary>
		/// Use to draw a FactLine on the model.
		/// </summary>
		private struct FactSaver
		{
			private IORMDesignerView myCurrentDocView;
			private ORMDesignerDocData myCurrentDocument;
			private ReadingOrder mySelectedReadingOrder;
			private IList<RoleBase> mySelectedRoleOrder;
			private FactType myEditFactType;
			private ParsedFactType myParsedFactType;

			/// <summary>
			/// Add facts to the model
			/// </summary>
			/// <param name="docData">Curent document data</param>
			/// <param name="docView">Curent document view</param>
			/// <param name="selectedFactType">Editing fact</param>
			/// <param name="selectedReadingOrder">Non-binding currently selected reading order</param>
			/// <param name="selectedRoleOrder">Non-binding currently selected role order that does not match and existing reading order.</param>
			/// <param name="parsedFactType">Parsed FactType</param>
			/// <returns>The <see cref="ReadingOrder"/> that was created or modified</returns>
			public static ReadingOrder IntegrateParsedFactType(ORMDesignerDocData docData, IORMDesignerView docView, FactType selectedFactType, ReadingOrder selectedReadingOrder, IList<RoleBase> selectedRoleOrder, ParsedFactType parsedFactType)
			{
				return (new FactSaver(docData, docView, selectedFactType, selectedReadingOrder, selectedRoleOrder, parsedFactType)).Go();
			}

			private FactSaver(ORMDesignerDocData docData, IORMDesignerView docView, FactType selectedFactType, ReadingOrder selectedReadingOrder, IList<RoleBase> selectedRoleOrder, ParsedFactType parsedFactType)
			{
				myCurrentDocView = docView;
				myCurrentDocument = docData;
				mySelectedReadingOrder = selectedReadingOrder;
				mySelectedRoleOrder = selectedRoleOrder;
				myEditFactType = selectedFactType;
				myParsedFactType = parsedFactType;
			}
			#region NewElementTracker class
			/// <summary>
			/// A helper class to track new shapes, provides a instance
			/// for adding new element filters.
			/// </summary>
			private class NewElementTracker : IEnumerable<KeyValuePair<ModelElement, bool>>
			{
				private bool myBlockAllElements;
				#region Constructor
				private Dictionary<ModelElement, bool> myElements;
				/// <summary>
				/// Create a tracker
				/// </summary>
				public NewElementTracker()
				{
					myElements = new Dictionary<ModelElement, bool>(HashCodeComparer<ModelElement>.Instance);
					myBlockAllElements = true;
				}
				#endregion // Constructor
				#region Tracking Methods
				/// <summary>
				/// <see cref="IORMToolServices.AutomatedElementFilter"/> callback
				/// </summary>
				public AutomatedElementDirective BlockElement(ModelElement element)
				{
					return myBlockAllElements || !myElements.ContainsKey(element) ? AutomatedElementDirective.Ignore : AutomatedElementDirective.None;
				}
				/// <summary>
				/// Were any elements tracked?
				/// </summary>
				public bool HasElements
				{
					get
					{
						return myElements.Count != 0;
					}
				}
				/// <summary>
				/// Add the element to the set of tracked elements
				/// </summary>
				public void Add(ModelElement element, bool forceShapeCreation)
				{
					Dictionary<ModelElement, bool> elements = myElements;
					bool existingForce;
					if (elements.TryGetValue(element, out existingForce))
					{
						if (forceShapeCreation && !existingForce)
						{
							elements[element] = forceShapeCreation;
						}
					}
					else
					{
						elements.Add(element, forceShapeCreation);
					}
				}
				/// <summary>
				/// Set to true to disable all element creation. This lets us
				/// create only the elements we want to when we want to, and not
				/// before or after. Short circuits lots of unnecessary processing.
				/// Defaults to true.
				/// </summary>
				public bool BlockAllElements
				{
					get
					{
						return myBlockAllElements;
					}
					set
					{
						myBlockAllElements = value;
					}
				}
				#endregion // Tracking Methods
				#region IEnumerable<ModelElement> Implementation
				IEnumerator<KeyValuePair<ModelElement, bool>> IEnumerable<KeyValuePair<ModelElement, bool>>.GetEnumerator()
				{
					return ((IEnumerable<KeyValuePair<ModelElement, bool>>)myElements).GetEnumerator();
				}
				IEnumerator IEnumerable.GetEnumerator()
				{
					return ((IEnumerable)myElements).GetEnumerator();
				}
				#endregion // IEnumerable<ModelElement> Implementation
			}
			#endregion // NewElementTracker class
			private ReadingOrder Go()
			{
				Store store = myCurrentDocument.Store;
				FactType startingFactType = myEditFactType;
				ORMModel model = (myEditFactType != null) ? myEditFactType.Model : store.ElementDirectory.FindElements<ORMModel>()[0];
				FactType currentFactType = null;
				ReadingOrder retVal = null;

				if (null != model)
				{
					IORMDesignerView docView = myCurrentDocView;
					ORMDiagram diagram = (docView != null) ? docView.CurrentDiagram as ORMDiagram : null;
					LayoutManager layoutManager = (diagram != null) ? new LayoutManager(diagram, (diagram.Store as IORMToolServices).GetLayoutEngine(typeof(ORMRadialLayoutEngine))) : null;
					NodeShape rightOfShape = null;

					IORMToolServices toolServices = (IORMToolServices)store;
					NewElementTracker tracker = new NewElementTracker();
					toolServices.AutomatedElementFilter += tracker.BlockElement;
					try
					{

						// We've got a model, now lets start a transaction to add our FactType and associated ObjectType elements to the model.
						using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.InterpretFactEditorLineTransactionName))
						{
							Dictionary<object, object> topLevelTransactionContextInfo = t.TopLevelTransaction.Context.ContextInfo;
							topLevelTransactionContextInfo[ORMBaseShape.PlaceAllChildShapes] = null;
							Dictionary<string, ObjectType> newlyCreatedObjectTypes = null;

							#region Determine all object types
							IList<ParsedFactTypeRolePlayer> rolePlayers = myParsedFactType.RolePlayers;
							int newFactArity = rolePlayers.Count;
							ObjectType[] objectTypes = new ObjectType[newFactArity];
							for (int i = 0; i < newFactArity; ++i)
							{
								ParsedFactTypeRolePlayer currentRolePlayer = rolePlayers[i];
								string objectName = currentRolePlayer.Name;
								bool forceValueType = (currentRolePlayer.RefMode.Length == 0 && currentRolePlayer.RefModeHasParenthesis);
								ObjectType currentObjectType;

								// Get the object if it already exists.
								LocatedElement existingElement = model.ObjectTypesDictionary.GetElement(objectName);

								// If the object was not found, create a new one.
								// Or if the object exists and the names are different, create a new one
								if (existingElement.IsEmpty)
								{
									currentObjectType = new ObjectType(store);
									currentObjectType.Name = objectName;
									currentObjectType.Model = model;
									(newlyCreatedObjectTypes ?? (newlyCreatedObjectTypes = new Dictionary<string, ObjectType>())).Add(objectName, currentObjectType);
									tracker.Add(currentObjectType, true);
									string currentRefModeName;

									// If the object DOES NOT already exist AND it's a value type
									if (forceValueType)
									{
										currentObjectType.IsValueType = true;
									}
									// If the object DOES NOT already exist AND it's an entity type
									else if ((currentRefModeName = currentRolePlayer.RefMode).Length != 0)
									{
										ReferenceMode singleMode = ReferenceMode.GetReferenceModeForDecoratedName(currentRefModeName, model, true);
										if (singleMode != null)
										{
											currentObjectType.ReferenceMode = singleMode;
										}
										else
										{
											currentObjectType.ReferenceModeString = currentRefModeName;
										}
									}
								} // Otherwise, use the existing object.
								else
								{
									if (newlyCreatedObjectTypes == null || !newlyCreatedObjectTypes.TryGetValue(objectName, out currentObjectType))
									{
										currentObjectType = existingElement.FirstElement as ObjectType;
									}
									bool convertingToValueType = false;
									string refModeText = currentRolePlayer.RefMode;
									int refModeLength = refModeText.Length;
									bool currentObjectIsValueType = currentObjectType.IsValueType;

									// get a presentation element to work with for determine if the ref mode is expanded or collapsed
									if (diagram != null)
									{
										FactType nestedFactType = currentObjectType.NestedFactType;
										ShapeElement foundShape;
										if (nestedFactType != null)
										{
											foundShape = diagram.FindShapeForElement<FactTypeShape>(nestedFactType);
											if (foundShape != null)
											{
												rightOfShape = (i == 0) ? (NodeShape)foundShape : null;
												layoutManager.AddShape(foundShape, true);
											}
											else
											{
												tracker.Add(nestedFactType, true);
											}
										}
										else
										{
											foundShape = diagram.FindShapeForElement<ObjectTypeShape>(currentObjectType);
											if (foundShape != null)
											{
												rightOfShape = (i == 0) ? (NodeShape)foundShape : null;
												layoutManager.AddShape(foundShape, true);
											}
											else
											{
												tracker.Add(currentObjectType, true);
											}
										}
										LinkedElementCollection<Role> playedRoles = currentObjectType.PlayedRoleCollection;
										int playedRoleCount = playedRoles.Count;
										for (int j = 0; j < playedRoleCount; ++j)
										{
											FactType factType = playedRoles[j].FactType;
											foundShape = diagram.FindShapeForElement<FactTypeShape>(factType);
											if (foundShape != null)
											{
												layoutManager.AddShape(foundShape, true);
											}
											else
											{
												tracker.Add(factType, false);
											}
										}
									}

									// convert this object to a entity type if it was a value type and if we are now adding a ref mode
									if (refModeLength > 0 && currentObjectIsValueType)
									{
										currentObjectType.IsValueType = false;
										// Note: Don't change the ExpandRefMode property to false here. This is
										// a user setting and we should respect the current value.
									}
									else if (refModeLength == 0 && !currentObjectIsValueType && currentRolePlayer.RefModeHasParenthesis)
									{
										// Set the flag to indicate we are converting an entity to a value type
										// so we need to kill the RefMode
										convertingToValueType = true;
									}

									// UNDONE: make sure we don't make an objectification a value type.
									// Use squiggles with markers/regions to block the commit of invalid operations

									if (currentObjectType.NestedFactType == null)
									{
										// if convertingToValueType, then we know we have parens and 0 length ref mode - force value type
										// if we have a ref mode and it's different than the old one, change it
										if (convertingToValueType || (!currentObjectType.IsValueType && refModeLength > 0 && !(currentObjectType.ReferenceModeDecoratedString == refModeText || currentObjectType.ReferenceModeString == refModeText)))
										{
											ReferenceMode singleMode = ReferenceMode.GetReferenceModeForDecoratedName(refModeText, model, true);
											if (singleMode == null)
											{
												// Add a "Delete" object to the transaction bucket to enable agressive RefMode deletion
												if (convertingToValueType)
												{
													topLevelTransactionContextInfo[ObjectType.DeleteReferenceModeValueType] = null;
												}
												currentObjectType.ReferenceModeString = refModeText;

												// remove the "Delete" object from the transaction bucket
												if (convertingToValueType)
												{
													topLevelTransactionContextInfo.Remove(ObjectType.DeleteReferenceModeValueType);
												}
											}
											else
											{
												currentObjectType.ReferenceMode = singleMode;
											}

											// The object was an entity, but is now a value type
											if (convertingToValueType)
											{
												currentObjectType.IsValueType = true;
											}
										}
										// ELSE:
										// if we have a ref mode and it's the same ignore it.
										// if ref mode is blank and !convertingToValueType, then we need to ignore the ref mode all together


									} // end of if (currentObject.NestedFactType == null)
								} // end of use existing object
								objectTypes[i] = currentObjectType;
							} // end foreach (ParsedFactTypeRolePlayer o in myParsedFactType.RolePlayers)
							#endregion // Determine all object types

							// Make sure that we aren't trying to use the objectifying type
							// of the active fact as one of the objects.
							if (startingFactType != null)
							{
								startingFactType = ObjectificationCheck(startingFactType);
							}

							// Handle an empty forward direction in the parse
							string readingText = myParsedFactType.ReadingText;
							string reverseReadingText = (newFactArity == 2) ? myParsedFactType.ReverseReadingText : null;
							bool deleteReverseReading = false;
							if (string.IsNullOrEmpty(readingText) &&
								!(string.IsNullOrEmpty(reverseReadingText)))
							{
								// Get an explicit reading order for reversing a reading so
								// where the role players are not changed. Without the explicit
								// order, ring fact types will not reverse correctly.
								readingText = reverseReadingText;
								reverseReadingText = null;
								ReadingOrder selectedReadingOrder;
								if (mySelectedRoleOrder == null &&
									null != (selectedReadingOrder = mySelectedReadingOrder))
								{
									IList<RoleBase> originalRoles = selectedReadingOrder.RoleCollection;
									if (originalRoles.Count == newFactArity)
									{
										int i = 0;
										for (; i < newFactArity; ++i)
										{
											if (originalRoles[i].Role.RolePlayer != objectTypes[i])
											{
												break;
											}
										}
										if (i == newFactArity)
										{
											RoleBase[] reversedRoles = new RoleBase[newFactArity];
											originalRoles.CopyTo(reversedRoles, 0);
											Array.Reverse(reversedRoles);
											mySelectedRoleOrder = reversedRoles;
											mySelectedReadingOrder = null;
										}
									}
								}
								Array.Reverse(objectTypes);
								deleteReverseReading = true;
							}
							else
							{
								// The reverse text is set to empty instead of null to indicate 'specified with no data'
								// as opposed to 'not specified'. If the user enters A r/ B, then we have an empty
								// instead of a null.
								deleteReverseReading = reverseReadingText != null && reverseReadingText.Length == 0;
							}

							// Add a FactType unless there is no information other than ObjectTypes
							if (startingFactType != null ||
								!string.IsNullOrEmpty(reverseReadingText) ||
								Reading.ReplaceFields(readingText, "").Trim().Length != 0)
							{
								RoleBase[] matchedRoles = new RoleBase[newFactArity];

								// Get the facttype if it exists, otherwise create a new one.
								bool resetReadingOrders = false;
								if (startingFactType == null)
								{
									currentFactType = new FactType(store);
									tracker.Add(currentFactType, true);
									currentFactType.Model = model;
									for (int i = 0; i < newFactArity; ++i)
									{
										Role role = new Role(store);
										role.FactType = currentFactType;
										role.RolePlayer = objectTypes[i];
										matchedRoles[i] = role;
									}
									resetReadingOrders = true;
								}
								else
								{
									currentFactType = startingFactType;

									// Match roles in current FactType to object types. If one
									// ObjectType is used multiple times, fallback on the selected
									// reading orders to determine the best role order
									IList<RoleBase> selectedRolesList = mySelectedReadingOrder != null ? mySelectedReadingOrder.RoleCollection : mySelectedRoleOrder;
									int selectedRoleCount = selectedRolesList.Count;
									int unmatchedRoleCount = newFactArity;
									int unusedRoleCount = selectedRoleCount;
									RoleBase[] selectedRoles = new RoleBase[selectedRoleCount];
									selectedRolesList.CopyTo(selectedRoles, 0);
									for (int i = 0; i < newFactArity; ++i)
									{
										ObjectType matchObjectType = objectTypes[i];
										RoleBase matchRole = null;
										for (int j = 0; j < selectedRoleCount; ++j)
										{
											RoleBase testRole = selectedRoles[j];
											if (testRole != null && testRole.Role.RolePlayer == matchObjectType)
											{
												matchRole = testRole;
												selectedRoles[j] = null;
												--unusedRoleCount;
												break;
											}
										}
										if (matchRole != null)
										{
											matchedRoles[i] = matchRole;
											--unmatchedRoleCount;
										}
									}
									if (unmatchedRoleCount != 0)
									{
										if (selectedRoleCount == newFactArity)
										{
											// Keep the FactType basically intact if the
											// null matched roles line up positionally with the
											// unused selected roles.
											int i = 0;
											for (; i < newFactArity; ++i)
											{
												// First pass, see if they line up
												if ((matchedRoles[i] == null) == (selectedRoles[i] == null))
												{
													break;
												}
											}
											if (i == newFactArity) // All missing slots align with selected order
											{
												for (i = 0; i < newFactArity; ++i)
												{
													if (matchedRoles[i] == null)
													{
														RoleBase selectedRole = selectedRoles[i];
														matchedRoles[i] = selectedRole;
														selectedRoles[i] = null;
														--unusedRoleCount;
														--unmatchedRoleCount;
														// Clear the value constraint (the type has changed),
														// but leave other constraints attached
														Role role = selectedRole.Role;
														role.ValueConstraint = null;
														role.RolePlayer = objectTypes[i];
													}
												}
											}
										}
										if (unusedRoleCount != 0)
										{
											// Remove any ConstraintRoleSequence attached to a matchedRole
											// that includes a role on this FactType that is not part of the
											// current matchedRoles set.
											for (int i = 0; i < newFactArity; ++i)
											{
												RoleBase roleBase = matchedRoles[i];
												if (roleBase != null)
												{
													Role role = roleBase.Role;
													LinkedElementCollection<ConstraintRoleSequence> sequences = role.ConstraintRoleSequenceCollection;
													int sequenceCount = sequences.Count;
													for (int j = sequenceCount - 1; j >= 0; --j)
													{
														ConstraintRoleSequence sequence = sequences[j];
														bool unmatchedRole = false;
														bool sequenceIntersectsForeignFactType = false;
														LinkedElementCollection<Role> constraintRoles = sequence.RoleCollection;
														int constraintRoleCount = constraintRoles.Count;
														if (constraintRoleCount > 1)
														{
															for (int k = 0; k < constraintRoleCount; ++k)
															{
																Role constraintRole = constraintRoles[k];
																if (constraintRole != role)
																{
																	if (constraintRole.FactType != currentFactType)
																	{
																		sequenceIntersectsForeignFactType = true;
																	}
																	else
																	{
																		int m = 0;
																		for (; m < newFactArity; ++m)
																		{
																			if (m != i)
																			{
																				RoleBase testRole = matchedRoles[m];
																				if (testRole.Role == constraintRole)
																				{
																					break;
																				}
																			}
																		}
																		if (m == newFactArity)
																		{
																			unmatchedRole = true;
																			break;
																		}
																	}
																}
															}
															if (unmatchedRole)
															{
																if (sequenceIntersectsForeignFactType)
																{
																	for (int k = constraintRoleCount - 1; k >= 0; --k)
																	{
																		if (constraintRoles[k].FactType == currentFactType)
																		{
																			constraintRoles.RemoveAt(k);
																		}
																	}
																}
																else
																{
																	// Easier case, get rid of the full sequence
																	sequence.Delete();
																}
															}
														}
													}
												}
											}
										}
										if (unmatchedRoleCount != 0)
										{
											int lastUnusedRoleIndex = 0;
											for (int i = 0; i < newFactArity; ++i)
											{
												if (matchedRoles[i] == null)
												{
													// UNDONE: Consider using the FactType.InsertAfterRoleKey
													// and FactType.InsertBeforeRoleKey context information
													// to modify display of inserted roles.

													// Use an existing role if we have it
													if (unusedRoleCount != 0)
													{
														for (int j = lastUnusedRoleIndex; j < selectedRoleCount; ++j)
														{
															RoleBase availableRole = selectedRoles[j];
															if (availableRole != null)
															{
																Role role = availableRole.Role;
																// Clean constraint relationships on this role
																// We're essentially using it like a new role.
																role.ConstraintRoleSequenceCollection.Clear();
																role.ValueConstraint = null;
																role.RolePlayer = objectTypes[i];
																role.Name = "";
																matchedRoles[i] = availableRole;
																selectedRoles[j] = null;
																--unusedRoleCount;
																lastUnusedRoleIndex = j + 1;
															}
														}
													}
													else
													{
														Role role = new Role(store);
														role.FactType = currentFactType;
														role.RolePlayer = objectTypes[i];
														matchedRoles[i] = role;
													}
												}
											}
											resetReadingOrders = true;
										}
									}
									if (unusedRoleCount != 0)
									{
										resetReadingOrders = true;
										for (int i = 0; i < selectedRoleCount; ++i)
										{
											RoleBase currentRole = selectedRoles[i];
											if (currentRole != null)
											{
												// Deleting the role will automatically take
												// care of the other relationships that reference it
												currentRole.Delete();
											}
										}
									}
								}

								// Get a current reading to set the text for
								if (!string.IsNullOrEmpty(readingText))
								{
									if (resetReadingOrders)
									{
										// There is no way to preserve any meaningingful information
										// in the existing reading orders and readings (if any). However,
										// we don't want to clear the collection outright because there
										// may be shapes attached to the reading orders that will lose
										// position if we clear the full collection. Make sure we
										// keep a reading order at all times.
										LinkedElementCollection<ReadingOrder> orders = currentFactType.ReadingOrderCollection;
										ReadingOrder newOrder = new ReadingOrder(store);
										newOrder.RoleCollection.AddRange(matchedRoles);
										int startingOrdersCount = orders.Count;
										if (startingOrdersCount == 0)
										{
											orders.Add(newOrder);
										}
										else
										{
											orders.Insert(0, newOrder);
											orders.RemoveRange(1, startingOrdersCount);
										}
										newOrder.ReadingText = readingText;
										retVal = newOrder;
									}
									else
									{
										retVal = SetPrimaryReading(store, currentFactType, matchedRoles, readingText);
										if (deleteReverseReading)
										{
											Array.Reverse(matchedRoles);
											ReadingOrder order = currentFactType.FindMatchingReadingOrder(matchedRoles);
											if (order != null)
											{
												order.Delete();
											}
											// Array.Reverse(matchedRoles); No need to reverse back, the
											// final use of matchedRoles won't happen on this code path
										}
									}

									if (!string.IsNullOrEmpty(reverseReadingText))
									{
										Array.Reverse(matchedRoles);
										SetPrimaryReading(store, currentFactType, matchedRoles, reverseReadingText);
									}
								}
							}

							if (t.HasPendingChanges)
							{
								tracker.BlockAllElements = false;
								if (diagram != null && tracker.HasElements)
								{
									AutoLayout(diagram, layoutManager, tracker, rightOfShape);
								}
								tracker.BlockAllElements = true;
								t.Commit();
							}
						} // end transaction
					}
					finally
					{
						toolServices.AutomatedElementFilter -= tracker.BlockElement;
					}
				}
				return retVal;
			}
			private ReadingOrder SetPrimaryReading(Store store, FactType factType, RoleBase[] roleOrder, string readingText)
			{
				ReadingOrder currentOrder = factType.FindMatchingReadingOrder(roleOrder);
				if (currentOrder == null)
				{
					currentOrder = new ReadingOrder(store);
					currentOrder.RoleCollection.AddRange(roleOrder);
					currentOrder.FactType = factType;
				}
				currentOrder.ReadingText = readingText;
				return currentOrder;
			}
			private static void AutoLayout(ORMDiagram diagram, LayoutManager layoutManager, IEnumerable<KeyValuePair<ModelElement, bool>> newlyCreatedElements, NodeShape rightOfShape)
			{
				// New stuff for autolayout
				foreach (KeyValuePair<ModelElement, bool> keyPair in newlyCreatedElements)
				{
					ModelElement modelElement = keyPair.Key;
					ShapeElement shapeElement;
					if (null != (shapeElement = diagram.FindShapeForElement(modelElement)))
					{
						layoutManager.AddShape(shapeElement, true);
					}
					else if (keyPair.Value)
					{
						diagram.PlaceORMElementOnDiagram(
							null,
							modelElement,
							PointD.Empty,
							ORMPlacementOption.None,
							delegate(ModelElement shapeForElement, ShapeElement newShape)
							{
								if (shapeForElement == modelElement)
								{
									layoutManager.AddShape(newShape, false);
								}
							},
							null);
					}
				}
				layoutManager.Layout(false, rightOfShape);
			}

			private FactType ObjectificationCheck(FactType startingFact)
			{
				ObjectType nestingType = startingFact.NestingType;
				if (startingFact.NestingType != null)
				{
					string nestingName = nestingType.Name;
					foreach (ParsedFactTypeRolePlayer factObject in myParsedFactType.RolePlayers)
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
}
