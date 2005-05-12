#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Northface.Tools.ORM.Shell;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.ShapeModel;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Text.RegularExpressions;

#endregion

namespace Northface.Tools.ORM.FactEditor
{
	/// <summary>
	/// Use to draw a FactLine on the model.
	/// </summary>
	public struct FactSaver
	{
		private ORMDesignerDocView myCurrentDocView;
		private ORMDesignerDocData myCurrentDocument;
		private FactLine myFactLine;
		private FactType myEditFact;
		private const string RefModeGroupName = "refmode";
		private static Regex ReferenceModeRegEx = new Regex(@"\((?<refmode>[^\(\)]+)\)", RegexOptions.Singleline | RegexOptions.Compiled);

		/// <summary>
		/// Add facts to the model
		/// </summary>
		/// <param name="docView">Curent document view</param>
		/// <param name="factLine">Parsed line</param>
		/// <param name="fact">Editing fact</param>
		public static void AddFact(ORMDesignerDocView docView, FactLine factLine, FactType fact)
		{
			(new FactSaver(docView, factLine, fact)).AddFactToModel();
		}
		private FactSaver(ORMDesignerDocView docView, FactLine factLine, FactType fact)
		{
			myCurrentDocView = docView;
			myCurrentDocument = docView.DocData as ORMDesignerDocData;
			myFactLine = factLine;
			myEditFact = fact;
		}

		private void AddFactToModel()
		{
			ORMModel myModel = null;
			Store store = myCurrentDocument.Store;
			myModel = (ORMModel)store.ElementDirectory.GetElements(ORMModel.MetaClassGuid)[0];

			if (null != myModel)
			{

				ORMDesignerDocView docView = (ORMDesignerDocView)myCurrentDocument.DocViews[0];
				ORMDiagram diagram = docView.Diagram as ORMDiagram;
				Dictionary<ModelElement, bool> modelElements = new Dictionary<ModelElement, bool>();
				bool factChanged = false;

				// We've got a model, now lets start a transaction to add our fact to the model.
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.InterpretFactEditorLineTransactionName))
				{
					IDictionary topLevelTransactionContextInfo = t.TopLevelTransaction.Context.ContextInfo;
					RoleMoveableCollection roles = null;
					RoleMoveableCollection factRoles = null;
					ReadingOrder readOrd;
					Reading read = null;
					FactType factType;
					if (myEditFact == null)
					{
						factType = FactType.CreateFactType(store);
						readOrd = ReadingOrder.CreateReadingOrder(store);
					}
					else
					{
						factType = myEditFact;
						readOrd = FactType.FindMatchingReadingOrder(factType);
						read = readOrd.PrimaryReading;
					}
					roles = readOrd.RoleCollection;
					roles.Clear();

					factRoles = factType.RoleCollection;
					factRoles.Clear();

					foreach (FactTokenMark mark in myFactLine.Marks)
					{
						// Check to see if the mark is an ObjectType (either entity or value).
						switch (mark.TokenType)
						{
							case FactTokenType.EntityType:
							case FactTokenType.ValueType:
							{
								ObjectType objType = null;
								string preObjName = myFactLine.LineText.Substring(mark.nStart, mark.nEnd - mark.nStart + 1);
								Match m = ReferenceModeRegEx.Match(preObjName);
								string refModeText = m.Groups[RefModeGroupName].Value;
								string objNameSansRef = preObjName.Replace(string.Concat("(", refModeText, ")"), "");


								// figure out if there are any objects on the diagram
								bool isEmptyElement = true;

								LocatedElement existingElement = myModel.ObjectTypesDictionary.GetElement(objNameSansRef);
								isEmptyElement = existingElement.IsEmpty;

								if (isEmptyElement)
								{
									objType = ObjectType.CreateObjectType(store);
									factChanged = true;
								}
								else
								{
									objType = (ObjectType)existingElement.FirstElement;
								}

									// If our mark is known to be a Value type object...
								if (isEmptyElement)
								{
									objType.Model = myModel;
									if (mark.TokenType == FactTokenType.ValueType)
									{
										objType.IsValueType = true;
										modelElements.Add(objType, true);
									}
									else
									{
										objType.Name = objNameSansRef;
										modelElements.Add(objType, true);
										IList<ReferenceMode> modes = ReferenceMode.FindReferenceModesByName(refModeText, myModel);
										if (modes.Count == 0)
										{
											objType.ReferenceModeString = refModeText;
										}
										else
										{
											// UNDONE: Consider giving a warning here (after the transaction is
											// completed, no UI during transactions) that the reference mode is ambiguous.
											objType.ReferenceMode = modes[0];
										}
									}
								}
								else
								{
									objType.Name = objNameSansRef;
									modelElements.Add(objType, true);
									bool convertingToValueType = false;
									bool entityWasCollapsed = false;

									// get a presentation element to work with for determine if the ref mode is expanded or collapsed
									ShapeModel.ObjectTypeShape objShape = (myCurrentDocView.Diagram as ORMDiagram).FindShapeForElement<ShapeModel.ObjectTypeShape>(objType);

									// convert this object to a entity type if it was a value type and if we are now adding a ref mode
									if (refModeText.Length > 0 && objType.IsValueType)
									{
										objType.IsValueType = false;
										// Note: Don't change the ExpandRefMode property to false here. This is
										// a user setting and we should respect the current value.
									}
									else if (refModeText.Length == 0 && !objType.IsValueType)
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
									if (objType.NestedFactType == null)
									{
										IList<ReferenceMode> modes = ReferenceMode.FindReferenceModesByName(refModeText, myModel);
										if (modes.Count == 0)
										{
											// Add a "Delete" object to the transaction bucket to enable agressive RefMode deletion
											if (convertingToValueType && entityWasCollapsed)
											{
												topLevelTransactionContextInfo[ObjectType.DeleteReferenceModeValueType] = null;
											}
											objType.ReferenceModeString = refModeText;
											
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
											objType.ReferenceMode = modes[0];
										}

										// The object was an entity, but is now a value type
										if (convertingToValueType)
										{
											objType.IsValueType = true;
										}
									}
								}
								objType.Name = objNameSansRef;

								if (null != factRoles)
								{
									Role role = Role.CreateRole(store);
									role.Name = "Role1";
									role.RolePlayer = objType;
									factRoles.Add(role);
								}
								break;
							}
						}
					}

					int currentRole = 0;
					StringBuilder reading = new StringBuilder();
					// Loop the predicate marks
					bool inPredicate = false;
					bool firstPredicate = true;
					foreach (FactTokenMark mark in myFactLine.Marks)
					{
						if (mark.TokenType == FactTokenType.Predicate)
						{
							string innerText = myFactLine.LineText.Substring(mark.nStart, mark.nEnd - mark.nStart + 1);
							if (inPredicate)
							{
								reading.AppendFormat(" {0}", innerText);
							}
							else
							{
								if (firstPredicate)
								{
									reading.AppendFormat("{{{0}}} {1}", currentRole++, innerText);
									firstPredicate = false;
								}
								else
								{
									reading.AppendFormat(" {0}", innerText);
								}
								inPredicate = true;
							}
						}
						else
						{
							if (inPredicate)
							{
								reading.AppendFormat(" {{{0}}}", currentRole++);
								inPredicate = false;
							}
						}
					}

					if (null != factType)
					{
						if (factType.Model == null)
						{
							factType.Model = myModel;
						}
						modelElements.Add(factType, true);
					}

					roles.Clear();
					for(int i=0;i<factRoles.Count;++i)
					{
						roles.Add(factRoles[i]);
					}

					if (myEditFact == null)
					{
						factType.ReadingOrderCollection.Add(readOrd);
						read = Reading.CreateReading(store);
						readOrd.ReadingCollection.Add(read);
					}
					read.Text = reading.ToString();

					// Commit the changes to the model.
					t.Commit();
				}

				if (factChanged)
				{
					using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.InterpretFactEditorLineTransactionName))
					{
						// New stuff for autolayout
						Dictionary<ShapeElement, bool> shapeElements = new Dictionary<ShapeElement, bool>();
						foreach (KeyValuePair<ModelElement, bool> modelElement in modelElements)
						{
							ShapeElement shapeElement = diagram.FindShapeForElement(modelElement.Key);
							if (shapeElement != null)
							{
								shapeElements.Add(shapeElement, modelElement.Value);
							}
						}

						docView.Diagram.AutoLayoutChildShapes(shapeElements);
						t.Commit();
					}
				}
			}
		}
	}
}