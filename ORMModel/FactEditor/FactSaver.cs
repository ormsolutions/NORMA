#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using Northface.Tools.ORM.Shell;
using Northface.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
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
		private static Regex ReferenceModeRegEx = new Regex(@"\((?<refmode>[^\(\)]+)\)", RegexOptions.Singleline | RegexOptions.Compiled);

		/// <summary>
		/// Add facts to the model
		/// </summary>
		/// <param name="docData">Curent document</param>
		/// <param name="factLine">Parsed line</param>
		public static void AddFact(ORMDesignerDocData docData, FactLine factLine)
		{
			(new FactSaver(docData, factLine)).AddFactToModel();
		}
		private FactSaver(ORMDesignerDocData docData, FactLine factLine)
		{
			myCurrentDocument = docData;
			myFactLine = factLine;
		}

		private void AddFactToModel()
		{
			ORMModel myModel = null;
			Store store = myCurrentDocument.Store;
			myModel = (ORMModel)store.ElementDirectory.GetElements(ORMModel.MetaClassGuid)[0];

			if (null != myModel)
			{
				// We've got a model, now lets start a transaction to add our fact to the model.
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.InterpretFactEditorLineTransactionName))
				{
					RoleMoveableCollection roles = null;
					ReadingOrder readOrd = ReadingOrder.CreateReadingOrder(store);
					// We know that each line can only contain one fact
					// so we only need to create one fact for this transaction.
					FactType factType = FactType.CreateFactType(store);
					roles = factType.RoleCollection;

					foreach (FactTokenMark mark in myFactLine.Marks)
					{
						// Check to see if the mark is an ObjectType (either entity or value).
						switch (mark.TokenType)
						{
							case FactTokenType.EntityType:
							case FactTokenType.ValueType:
							{
								ObjectType objType = ObjectType.CreateObjectType(store);
								string preObjName = myFactLine.LineText.Substring(mark.nStart, mark.nEnd - mark.nStart + 1);
								Match m = ReferenceModeRegEx.Match(preObjName);

								// If our mark is known to be a Value type object...
								if (mark.TokenType == FactTokenType.ValueType)
								{
									objType.IsValueType = true;
									objType.Name = preObjName.Replace("()", "");
									objType.Model = myModel;
								}
								else
								{
									string refModeText = m.Groups["refmode"].Value;
									objType.Name = preObjName.Replace(string.Concat("(", refModeText, ")"), "");
									objType.Model = myModel;
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
									//!myModel.ObjectTypesDictionary.GetElement("Person").IsEmpty
								}
						
								if (null != roles)
								{
									Role role = Role.CreateRole(store);
									role.Name = "Role1";
									role.RolePlayer = objType;
									roles.Add(role);
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
									reading.AppendFormat("{{0}} {1}", currentRole++, innerText);
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
								reading.AppendFormat(" {{0}}", currentRole++);
								inPredicate = false;
							}
						}
					}

					currentRole = 0;
					RoleMoveableCollection readingRoles = readOrd.RoleCollection;
					foreach (Role r in roles)
					{
						readingRoles.Add(roles[currentRole++]);
					}
					factType.ReadingOrderCollection.Add(readOrd);

					Reading read = Reading.CreateReading(store);
					readOrd.ReadingCollection.Add(read);
					read.Text = reading.ToString();

					if (null != factType)
					{
						factType.Model = myModel;
					}

					// Commit the changes to the model.
					t.Commit();
				}
			}
		}
	}
}