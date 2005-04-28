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
		private ORMDesignerDocData myCurrentDocument;
		private FactLine myFactLine;
		private FactType myEditFact;
		private const string RefModeGroupName = "refmode";
		private static Regex ReferenceModeRegEx = new Regex(@"\((?<refmode>[^\(\)]+)\)", RegexOptions.Singleline | RegexOptions.Compiled);

		/// <summary>
		/// Add facts to the model
		/// </summary>
		/// <param name="docData">Curent document</param>
		/// <param name="factLine">Parsed line</param>
		/// <param name="fact">Editing fact</param>
		public static void AddFact(ORMDesignerDocData docData, FactLine factLine, FactType fact)
		{
			(new FactSaver(docData, factLine, fact)).AddFactToModel();
		}
		private FactSaver(ORMDesignerDocData docData, FactLine factLine, FactType fact)
		{
			myCurrentDocument = docData;
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
								ObjectType objType;
								string preObjName = myFactLine.LineText.Substring(mark.nStart, mark.nEnd - mark.nStart + 1);
								Match m = ReferenceModeRegEx.Match(preObjName);
								string refModeText = m.Groups[RefModeGroupName].Value;
								string objNameSansRef = preObjName.Replace(string.Concat("(", refModeText, ")"), "");
								LocatedElement existingElement = myModel.ObjectTypesDictionary.GetElement(objNameSansRef);
								bool isEmptyElement = existingElement.IsEmpty;
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
									if (mark.TokenType != FactTokenType.ValueType)
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
								objType.Name = objNameSansRef;

								if (null != factRoles)
								{
									Role role = Role.CreateRole(store);
									role.Name = "Role1";
									if (isEmptyElement)
									{
										role.RolePlayer = objType;
									}
									else
									{
										role.RolePlayer = (ObjectType)existingElement.FirstElement;
									}
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
						if (factChanged)
						{
							factType.Model = myModel;
						}
						modelElements.Add(factType, true);
					}

					currentRole = 0;
					roles.Clear();
					foreach (Role r in factRoles)
					{
						roles.Add(factRoles[currentRole++]);
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