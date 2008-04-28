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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
#endregion

namespace Neumont.Tools.ORM.Shell
{
	partial class FactEditorLanguageService
	{
		#region ParsedFactType class
		/// <summary>
		/// Created from fact text in the fact editor and contains
		/// properties for all the role players and colorization in the fact text
		/// </summary>
		private class ParsedFactType
		{
			#region Regex properties
			private static Regex myObjectTypeRegex;
			private static Regex ObjectTypeRegex
			{
				get
				{
					Regex retVal = myObjectTypeRegex;
					if (retVal == null)
					{
						System.Threading.Interlocked.CompareExchange<Regex>(
							ref myObjectTypeRegex,
							new Regex(
								@"(?n)(?<object>((\[(?<objectName>[\w\p{Pd}]+(\s+[\w\p{Pd}]+)*)(?<refModeWithParens>(\((?<refMode>.*?)?\))?)?\])|((?<objectName>(?<=^|\s)\p{Lu}[\w\p{Pd}]*)(?<refModeWithParens>(\((?<refMode>.*?)?\))?)?)))",
								RegexOptions.Compiled),
							null);
						retVal = myObjectTypeRegex;
					}
					return retVal;
				}
			}
			#endregion // Regex properties
			#region FactObjectCollection class
			/// <summary>
			/// A collection of role players in a facttype
			/// </summary>
			private class ParsedFactTypeRolePlayerCollection : Collection<ParsedFactTypeRolePlayer>
			{
				/// <summary>
				/// Creates and adds a new FactObject to the collection
				/// </summary>
				/// <returns>The object created</returns>
				public ParsedFactTypeRolePlayer NewRolePlayer()
				{
					ParsedFactTypeRolePlayer retVal = new ParsedFactTypeRolePlayer();
					base.Items.Add(retVal);
					return retVal;
				}

				/// <summary>
				/// Adds a range of ParsedFactTypeRolePlayers to the collection
				/// </summary>
				public void AddRange(IEnumerable<ParsedFactTypeRolePlayer> values)
				{
					IList<ParsedFactTypeRolePlayer> items = base.Items;
					foreach (ParsedFactTypeRolePlayer value in values)
					{
						items.Add(value);
					}
				}
			}
			#endregion // FactObjectCollection class
			#region Public Static Members
			/// <summary>
			/// Examines the line text marks object types, predicates, and reference modes
			/// </summary>
			/// <param name="factText">The source of the line text to examine</param>
			/// <returns><see cref="ParsedFactType"/></returns>
			public static ParsedFactType ParseLine(string factText)
			{
				return ParseFactFromLine(factText);
			}

			/// <summary>
			/// Parse the fact into a collection of role players with quantifiers
			/// </summary>
			/// <returns><see cref="ParsedFactType"/></returns>
			private static ParsedFactType ParseFactFromLine(string factText)
			{
				ParsedFactType instance = new ParsedFactType(factText);
				// Setup the default colorization to predicate text
				// don't setup the last element (the cursor position)
				uint predicateText = (uint)FactEditorColorizableItem.PredicateText;
				for (int i = 0; i < instance.ColorAttributes.Length - 1; ++i)
				{
					instance.ColorAttributes[i] = predicateText;
				}

				// FACT-QUANTIFIERS: This is the new quantifier pattern entered with (note the "." as the delimiter):
				// [Person] drives .at most one. [Car]
				//			string logicalOne = "one";
				//			string logicalQuantifierPattern = String.Format(@"\.(?<quantifier>(?<logicalName>({0}|{1}|{2}))\s+(?<logicalFreq>(\d+|{3})))\.", FactQuantifier.LogicalExactly, FactQuantifier.LogicalAtLeast, FactQuantifier.LogicalAtMost, logicalOne);
				//			Regex regExQuantifier = new Regex(logicalQuantifierPattern);
				//			MatchCollection quantifiers = regExQuantifier.Matches(factText);

				// FACT-QUANTIFIERS: This is the old quantifier Pattern
				// mandatory both sides, one to one {..;1:1}
				// functional role pattern {.-;n:1}
				//			string quantifierPattern = @"([^{]+)?(?<multiplicityWhole>({(?<mandatoryRoles>(\.|-)+)?;(?<multiplicity>(.+))?}))";
				//			Regex regExMultiplicity = new Regex(quantifierPattern);
				//			Match multiplicity = regExMultiplicity.Match(factText);


				#region Old Multiplicity matches, eg. {.-;n:1}
				// Grab the multiplicity values
				//			char[] mandatoryValues = null;
				//			string multiplicityValues = null; 
				//			if (multiplicity.Success)
				//			{
				//				mandatoryValues = multiplicity.Groups["mandatoryRoles"].Value.ToCharArray();
				//				multiplicityValues = multiplicity.Groups["multiplicity"].Value;
				//				factText = factText.Replace(multiplicity.Groups["multiplicityWhole"].Value, "");
				//			}
				#endregion // Old Multiplicity matches, eg. {.-;n:1}

				Regex regExObjectType = ObjectTypeRegex;
				MatchCollection objectTypes = regExObjectType.Matches(factText);
				ParsedFactTypeRolePlayerCollection rolePlayers = new ParsedFactTypeRolePlayerCollection();
				// Loop the objects. Inside this loop, loop the quantifiers collection.
				// Remove quantifiers that are positioned before the object.
				int counter = -1;
				int position = 0;
				StringBuilder builderFact = new StringBuilder();
				int nrObjects = objectTypes.Count;
				for (int i = 0; i < nrObjects; ++i)
				{
					Match m = objectTypes[i];
					int index = m.Index;
					ParsedFactTypeRolePlayer newObject = rolePlayers.NewRolePlayer();
					Group objectGroup = m.Groups["object"];
					Group objectNameGroup = m.Groups["objectName"];
					Group refModeGroup = m.Groups["refMode"];
					Group refModeWithParensGroup = m.Groups["refModeWithParens"];
					newObject.Name = objectNameGroup.Value;
					newObject.RefMode = refModeGroup.Value;
					newObject.RefModeHasParenthesis = refModeWithParensGroup.Value.Length > 0;
					newObject.Position = ++counter;

					// set the colorization for the object
					int objColorStart = objectNameGroup.Index;
					int objColorEnd = objectNameGroup.Index + objectNameGroup.Length - 1;

					// get the previous "[", only if it contains "["
					bool objectContainsBracket = objectGroup.Value.IndexOf('[') > -1 && objectGroup.Value.IndexOf(']') > -1;
					if (objectContainsBracket)
					{
						instance.ColorAttributes[objectGroup.Index] = (uint)FactEditorColorizableItem.Delimiter;
					}
					for (int j = objColorStart; j <= objColorEnd; ++j)
					{
						instance.ColorAttributes[j] = (uint)FactEditorColorizableItem.ObjectName;
					}
					// get the following "]"
					if (objectContainsBracket)
					{
						instance.ColorAttributes[objectGroup.Index + objectGroup.Length - 1] = (uint)FactEditorColorizableItem.Delimiter;
					}

					// set the colorization for the ref mode
					if (newObject.RefMode.Length > 0)
					{
						int refColorStart = refModeGroup.Index;
						int refColorEnd = refModeGroup.Index + refModeGroup.Length - 1;
						for (int j = refColorStart; j <= refColorEnd; ++j)
						{
							instance.ColorAttributes[j] = (uint)FactEditorColorizableItem.ReferenceModeName;
						}

					}

					// colorize the parens for ref modes
					if (refModeWithParensGroup.Length > 0)
					{
						// get the previous "("
						instance.ColorAttributes[refModeWithParensGroup.Index] = (uint)FactEditorColorizableItem.Delimiter;
						// get the following ")"
						instance.ColorAttributes[refModeWithParensGroup.Index + refModeWithParensGroup.Length - 1] = (uint)FactEditorColorizableItem.Delimiter;
					}

					// get from position to index of current object
					builderFact.AppendFormat("{0}{{{1}}}", factText.Substring(position, index - position), i);
					// add any "suffix" predicate to the last object
					int nextPredicatePos = m.Index + m.Length;
					if (i == nrObjects - 1)
					{
						if (nextPredicatePos < factText.Length)
						{
							builderFact.Append(factText.Substring(nextPredicatePos, factText.Length - nextPredicatePos));
						}
					}
					position = nextPredicatePos;

					#region Logical Quantifiers
					// FACT-QUANTIFIERS: This is for the old multiplicity code, e.g. {.-;m:n}, where "."
					// indicates a mandatory role
					//				if (i <= mandatoryValues.GetUpperBound(0))
					//				{
					//					// Is mandatory
					//					if (mandatoryValues[i] == '.')
					//					{
					//						newObject.RoleQuantifiers.NewFactQuantifier("mandatory", 1);
					//					}
					//				}

					// FACT-QUANTIFIERS: This is for the new quantifier implementation using the "." delimiters:
					// [Person] drives .at most one. [Car]
					//				foreach (Match quantifier in quantifiers)
					//				{
					//					if (quantifier.Index < index)
					//					{
					//						try
					//						{
					//							int frequency = 0;
					//							string freqString = quantifier.Groups["logicalFreq"].Value;
					//
					//							if (freqString == logicalOne)
					//							{
					//								frequency = 1;
					//							}
					//							else
					//							{
					//								frequency = int.Parse(freqString);
					//							}
					//							newObject.RoleQuantifiers.NewFactQuantifier(quantifier.Groups["logicalName"].Value, frequency);
					//						}
					//						catch (FormatException ex)
					//						{
					//							//NOTDONE: Create a task item for a quantifier sans a numeric frequency.
					//							string err = ex.Message;
					//						}
					//					}
					//				}
					#endregion // Logical Quantifiers
				}
				#region Commented Out
				// FACT-QUANTIFIERS: This is for the new quantifier implementation using the "." delimiters.
				// get a copy of the fact to work with
				//			string tempFactText = builderFact.ToString();
				// remove all quantifiers from the fact
				//			foreach (Match quantifier in quantifiers)
				//			{
				//				tempFactText = tempFactText.Replace(quantifier.Value, "");
				//
				//				// setup colorization for quantifiers
				//				int qColorStart = quantifier.Index;
				//				int qColorEnd = quantifier.Index + quantifier.Length - 1;
				//				for (int j = qColorStart; j <= qColorEnd; ++j)
				//				{
				//					instance.ColorAttributes[j] = (uint)FactEditorColorizableItem.Quantifier;
				//				}
				//			}
				#endregion Commented Out
				ParsedFactTypeRolePlayerCollection objects = instance.myRolePlayers;
				//Reading text is in format: {0} <blank> {1}
				instance.ReadingText = builderFact.ToString();
				//Checks if the fact is a binary fact type with a forward AND reverse reading, denoted by the presence of the "/"
				//Then it splits the text into representations of two seperate readings
				if (nrObjects == 2 && instance.ReadingText.Contains("/"))
				{
					string[] splitReadings = instance.ReadingText.Split(new char[] { '/' }, 2, StringSplitOptions.RemoveEmptyEntries);
					if (splitReadings.Length == 2)
					{
						instance.ReadingText = splitReadings[0].Trim() + " {1}";
						instance.ReverseReadingText = "{0} " + splitReadings[1].Trim();
					}
				}
				instance.myRolePlayers.AddRange(rolePlayers);
				return instance;
			}
			#endregion // Public Static Members
			#region Member Variables
			private string myReadingText;
			private string myReverseReadingText;
			private ParsedFactTypeRolePlayerCollection myRolePlayers;
			private uint[] myColorAttributes;
			private string myOriginalFactText;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Create a new <see cref="ParsedFactType"/> from a structured fact in the fact editor
			/// </summary>
			/// <param name="originalFactText"></param>
			private ParsedFactType(string originalFactText)
			{
				myOriginalFactText = originalFactText;
				myRolePlayers = new ParsedFactTypeRolePlayerCollection();
				// this must be 1 element longer than the fact text, see comment in FactColorizer.ColorizeLine
				myColorAttributes = new uint[myOriginalFactText.Length + 1];
			}
			#endregion // Constructor
			#region Accessor Properties
			/// <summary>
			/// The reading text used for fact readings, e.g. {0} has {1}
			/// </summary>
			public string ReadingText
			{
				get { return myReadingText; }
				set { myReadingText = value; }
			}
			/// <summary>
			/// The reverse reading text used only for binary fact readings, e.g. {0} is of {1}
			/// </summary>
			public string ReverseReadingText
			{
				get { return myReverseReadingText; }
				set { myReverseReadingText = value; }
			}

			/// <summary>
			/// All of the role players parsed out of the original fact text
			/// </summary>
			public IList<ParsedFactTypeRolePlayer> RolePlayers
			{
				get { return myRolePlayers; }
			}

			/// <summary>
			/// An array of properties for colorization
			/// </summary>
			public uint[] ColorAttributes
			{
				get { return myColorAttributes; }
				set { myColorAttributes = value; }
			}

			/// <summary>
			/// The original fact text
			/// </summary>
			public string OriginalFactText
			{
				get { return myOriginalFactText; }
				set { myOriginalFactText = value; }
			}
			#endregion // Accessor Properties
		}
		#endregion // ParsedFactType class
		#region ParsedFactTypeRolePlayer class
		/// <summary>
		/// An object found in the Fact Editor's structured text
		/// which includes the object name, ref mode, quantifiers,
		/// and position in the fact
		/// </summary>
		private class ParsedFactTypeRolePlayer
		{
			private string myName;
			private string myRefMode;
			private bool myRefModeHasParenthesis;
			// TODO: Uncomment
			//		private FactQuantifierCollection myRoleQuantifiers;
			private int myPositionNr;

			/// <summary>
			/// Construct a new FactObject. This initialized the RoleQuantifiers collection
			/// </summary>
			public ParsedFactTypeRolePlayer()
			{
				//			myRoleQuantifiers = new FactQuantifierCollection();
			}
			/// <summary>
			/// The name of the object
			/// </summary>
			public string Name
			{
				get { return myName; }
				set { myName = value; }
			}
			/// <summary>
			/// The object's reference mode
			/// </summary>
			public string RefMode
			{
				get { return myRefMode; }
				set { myRefMode = value; }
			}
			/// <summary>
			/// True if parenthesis were found in the fact (even if the
			/// reference mode is blank)
			/// </summary>
			public bool RefModeHasParenthesis
			{
				get { return myRefModeHasParenthesis; }
				set { myRefModeHasParenthesis = value; }
			}
			//		/// <summary>
			//		/// Quantifiers which indicate mandatory or uniqueness
			//		/// </summary>
			//		public FactQuantifierCollection RoleQuantifiers
			//		{
			//			get { return myRoleQuantifiers; }
			//		}
			/// <summary>
			/// The position of this object in the parsed fact
			/// </summary>
			public int Position
			{
				get { return myPositionNr; }
				set { myPositionNr = value; }
			}
		}
		#endregion // ParsedFactTypeRolePlayer class
		#region Helper classes
		// TODO: Use FactQuantifier when parser can handle it more gracefully
		//	/// <summary>
		//	/// Types of quantifiers available.
		//	/// This is currently not used
		//	/// </summary>
		//	public enum LogicalQuantifierType 
		//	{ 
		//		/// <summary>
		//		/// AtLeast Quantifier
		//		/// </summary>
		//		AtLeast,
		//		/// <summary>
		//		/// AtMost Quantifier
		//		/// </summary>
		//		AtMost,
		//		/// <summary>
		//		/// Exactly Quantifier
		//		/// </summary>
		//		Exactly,
		//		/// <summary>
		//		/// Mandatory Quantifier
		//		/// </summary>
		//		Mandatory
		//	}
		//
		//	
		//	/// <summary>
		//	/// Quantifiers which are attached to role players, such as mandatory or at least one
		//	/// </summary>
		//	public class FactQuantifier
		//	{
		//		private string myName;
		//		private int myFrequency;
		//		private LogicalQuantifierType myQuantifierType;
		//
		//		//UNDONE: Localize these strings
		////		/// <summary>
		////		/// A string representing a logical quantifier literal
		////		/// </summary>
		////		public static readonly string LogicalExactly = "exactly";
		////		/// <summary>
		////		/// A string representing a logical quantifier literal
		////		/// </summary>
		////		public static readonly string LogicalAtLeast = @"at\sleast";
		////		/// <summary>
		////		/// A string representing a logical quantifier literal
		////		/// </summary>
		////		public static readonly string LogicalAtMost = @"at\smost";
		////		/// <summary>
		////		/// A string representing a logical quantifier literal
		////		/// </summary>
		////		public static readonly string LogicalMandatory = @"mandatory";
		//
		//		/// <summary>
		//		/// Create a new quantifier based on its name, e.g. "exactly" or "at least"
		//		/// </summary>
		//		/// <param name="name"></param>
		//		/// <param name="frequency"></param>
		//		public FactQuantifier(string name, int frequency)
		//		{
		//			Name = name;
		//			myFrequency = frequency;
		//		}
		//		/// <summary>
		//		/// Gets or sets the name of this quantifier
		//		/// </summary>
		//		public string Name
		//		{
		//			get { return myName; }
		//			set 
		//			{ 
		//				myName = value;
		//				// TODO: set the quantifier type to the right type if the name is changed
		//				// replace spaces to match the constant tokens
		////				string noSpaces = myName.Replace(" ", @"\s");
		////				if (noSpaces == LogicalExactly)
		////				{
		////					this.QuantifierType = LogicalQuantifierType.Exactly;
		////				}
		////				else if (noSpaces == LogicalAtLeast)
		////				{
		////					this.QuantifierType = LogicalQuantifierType.AtLeast;
		////				}
		////				else if (noSpaces == LogicalAtMost)
		////				{
		////					this.QuantifierType = LogicalQuantifierType.AtMost;
		////				}
		////				else if (noSpaces == LogicalMandatory)
		////				{
		////					this.QuantifierType = LogicalQuantifierType.Mandatory;
		////				}
		//			}
		//		}
		//		/// <summary>
		//		/// Gets or sets the frequency value. "One" maps to the number 1.
		//		/// </summary>
		//		public int Frequency
		//		{
		//			get { return myFrequency; }
		//			set { myFrequency = value; }
		//		}
		//		/// <summary>
		//		/// The type of quantifier: mandatory, at least one, etc.
		//		/// </summary>
		//		public LogicalQuantifierType QuantifierType
		//		{
		//			get { return myQuantifierType; }
		//			set { myQuantifierType = value; }
		//		}
		//	}
		//
		//	/// <summary>
		//	/// A collection of quantifiers for an object
		//	/// </summary>
		//	public class FactQuantifierCollection : System.Collections.CollectionBase
		//	{
		//		/// <summary>
		//		/// An indexer to get and set an item in the collection
		//		/// </summary>
		//		/// <param name="index"></param>
		//		/// <returns></returns>
		//		public FactQuantifier this[int index]
		//		{
		//			get
		//			{
		//				return (FactQuantifier)List[index];
		//			}
		//			set
		//			{
		//				List[index] = value;
		//			}
		//		}
		//
		//		/// <summary>
		//		/// Shortcut to create and add an new quantifier to the collection
		//		/// </summary>
		//		/// <param name="name"></param>
		//		/// <param name="frequency"></param>
		//		/// <returns></returns>
		//		public FactQuantifier NewFactQuantifier(string name, int frequency)
		//		{
		//			FactQuantifier fq = new FactQuantifier(name, frequency);
		//			this.Add(fq);
		//
		//			return fq;
		//		}
		//
		//		/// <summary>
		//		/// Adds a quantifier to the collection
		//		/// </summary>
		//		/// <param name="value"></param>
		//		/// <returns></returns>
		//		public int Add(FactQuantifier value)
		//		{
		//			return ((this as IList<FactObject>).Add(value));
		//		}
		//
		//		/// <summary>
		//		/// Finds the index of an item in the collection
		//		/// </summary>
		//		/// <param name="value"></param>
		//		/// <returns></returns>
		//		public int IndexOf(FactQuantifier value)
		//		{
		//			return ((this as IList<FactObject>).IndexOf(value));
		//		}
		//
		//		/// <summary>
		//		/// Inserts a quantifier at the given index
		//		/// </summary>
		//		/// <param name="index"></param>
		//		/// <param name="value"></param>
		//		public void Insert(int index, FactQuantifier value)
		//		{
		//			(this as IList<FactObject>).Insert(index, value);
		//		}
		//
		//		/// <summary>
		//		/// Removes a quantifier from the collection
		//		/// </summary>
		//		/// <param name="value"></param>
		//		public void Remove(FactQuantifier value)
		//		{
		//			(this as IList<FactObject>).Remove(value);
		//		}
		//
		//		/// <summary>
		//		/// Find if a quantifier is in the collection
		//		/// </summary>
		//		/// <param name="value"></param>
		//		/// <returns></returns>
		//		public bool Contains(FactQuantifier value)
		//		{
		//			return ((this as IList<FactObject>).Contains(value));
		//		}
		//	}
		#endregion // Helper classes
	}
}
