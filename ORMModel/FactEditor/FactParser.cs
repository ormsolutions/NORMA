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
#endregion

namespace Northface.Tools.ORM.FactEditor
{
	#region IFactParser

	/// <summary>
	/// Setup a Face Parser interface to ensure line markup
	/// </summary>
	public interface IFactParser
	{
		/// <summary>
		/// Implement the parsing of line text
		/// </summary>
		/// <param name="factText">A string containing the original line text</param>
		/// <returns>HRESULT</returns>
		ParsedFact ParseLine(string factText);
	}
	#endregion

	/// <summary>
	/// The FactLine class is used to organize markings within a line for
	/// colorization. We look for object types, predicates, and reference modes
	/// </summary>
	public sealed class FactLine
	{
		private string myLineText;
		private Collection<FactTokenMark> myMarks;
		private string myError = "";

		/// <summary>
		/// Create a new FactLine
		/// </summary>
		/// <param name="line">The source line text</param>
		public FactLine(string line) 
		{
			myLineText = line;
			myMarks = new Collection<FactTokenMark>();
		}

		/// <summary>
		/// The source text for this line
		/// </summary>
		/// <value></value>
		public string LineText
		{
			get { return myLineText; }
		}

		/// <summary>
		/// The markings on the line to indicate where different types of tokens are located
		/// </summary>
		/// <value></value>
		public Collection<FactTokenMark> Marks
		{
			get { return myMarks; }
			set { myMarks = value; }
		}

		/// <summary>
		/// An error that may have been found in setting the marks
		/// </summary>
		/// <value></value>
		public string Error
		{
			get { return myError; }
			set { myError = value; }
		}

		/// <summary>
		/// Indicates whether the current FactLine state has an error
		/// </summary>
		/// <value></value>
		public bool HasError
		{
			get { return myError.Length > 0; }
		}
	}

	/// <summary>
	/// A parser which breaks down a string into marks representing object types, predicates, and reference modes
	/// </summary>
	public class FactParser : IFactParser
	{
		private const string myObjectWithBracketsPattern = @"\[(?<objectName>\w+(\s+\w+)*)(?<refModeWithParens>(\((?<refMode>.*?)?\))?)?\]";
		private const string myObjectSansBracketsPattern = @"(?<objectName>[A-Z]\w*)(?<refModeWithParens>(\((?<refMode>.*?)?\))?)?";

		/// <summary>
		/// Default constructor
		/// </summary>
		public FactParser() { }

		#region IFactParser Members

		ParsedFact IFactParser.ParseLine(string factText)
		{
			return ParseLine(factText);
		}

		/// <summary>
		/// Examines the line text marks object types, predicates, and reference modes
		/// </summary>
		/// <param name="factText">The source of the line text to examine</param>
		/// <returns>ParsedFact</returns>
		protected static ParsedFact ParseLine(string factText)
		{
			return ParseFactFromLine(factText);
		}

		/// <summary>
		/// Parse the fact into a collection of objects with quantifiers
		/// </summary>
		/// <returns>A ParsedFact</returns>
		private static ParsedFact ParseFactFromLine(string factText)
		{
			ParsedFact parsedFact = new ParsedFact(factText);
			// Setup the default colorization to predicate text
			// don't setup the last element (the cursor position)
			uint predicateText = (uint)FactEditorColorizableItem.PredicateText;
			for (int i = 0; i < parsedFact.ColorAttributes.Length - 1; ++i)
			{
				parsedFact.ColorAttributes[i] = predicateText;
			}

			string objectTypePattern = string.Format(null, "(?<object>({0}|{1}))", myObjectWithBracketsPattern, myObjectSansBracketsPattern);

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

			Regex regExObjectType = new Regex(objectTypePattern);
			MatchCollection objectTypes = regExObjectType.Matches(factText);
			FactObjectCollection factObjects = new FactObjectCollection();
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
				FactObject newObject = factObjects.NewFactObject();
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
					parsedFact.ColorAttributes[objectGroup.Index] = (uint)FactEditorColorizableItem.Delimiter;
				}
				for (int j = objColorStart; j <= objColorEnd; ++j)
				{
					parsedFact.ColorAttributes[j] = (uint)FactEditorColorizableItem.ObjectName;
				}
				// get the following "]"
				if (objectContainsBracket)
				{
					parsedFact.ColorAttributes[objectGroup.Index + objectGroup.Length - 1] = (uint)FactEditorColorizableItem.Delimiter;
				}

				// set the colorization for the ref mode
				if (newObject.RefMode.Length > 0)
				{
					int refColorStart = refModeGroup.Index;
					int refColorEnd = refModeGroup.Index + refModeGroup.Length - 1;
					for (int j = refColorStart; j <= refColorEnd; ++j)
					{
						parsedFact.ColorAttributes[j] = (uint)FactEditorColorizableItem.ReferenceModeName;
					}
					
				}

				// colorize the parens for ref modes
				if (refModeWithParensGroup.Length > 0)
				{
					// get the previous "("
					parsedFact.ColorAttributes[refModeWithParensGroup.Index] = (uint)FactEditorColorizableItem.Delimiter;
					// get the following ")"
					parsedFact.ColorAttributes[refModeWithParensGroup.Index + refModeWithParensGroup.Length - 1] = (uint)FactEditorColorizableItem.Delimiter;
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
			}
			#endregion // Logical Quantifiers

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
//					parsedFact.ColorAttributes[j] = (uint)FactEditorColorizableItem.Quantifier;
//				}
//			}

			parsedFact.ReadingText = builderFact.ToString();
			parsedFact.FactObjects.AddRange(factObjects);
			return parsedFact;
		}

		#endregion
	}

	#region Helper classes
	/// <summary>
	/// Created from fact text in the fact editor and contains
	/// properties for all the objects and colorization in the fact text
	/// </summary>
	public class ParsedFact
	{
		private string myReading;
		private FactObjectCollection myObjects;
		private uint[] myColorAttributes;
		private string myOriginalFactText;

		/// <summary>
		/// Create a new ParsedFact from a structured fact in the fact editor
		/// </summary>
		/// <param name="originalFactText"></param>
		public ParsedFact(string originalFactText)
		{
			myOriginalFactText = originalFactText;
			myObjects = new FactObjectCollection();
			// this must be 1 element longer than the fact text, see comment in FactColorizer.ColorizeLine
			myColorAttributes = new uint[myOriginalFactText.Length + 1];
		}
		/// <summary>
		/// The reading text used for fact readings, e.g. {0} has {1}
		/// </summary>
		/// <value></value>
		public string ReadingText
		{
			get { return myReading; }
			set { myReading = value; }
		}
		/// <summary>
		/// All of the objects parsed out of the original fact text
		/// </summary>
		/// <value></value>
		public FactObjectCollection FactObjects
		{
			get { return myObjects; }
		}

		/// <summary>
		/// An array of attributes for colorization
		/// </summary>
		/// <value></value>
		public uint[] ColorAttributes
		{
			get { return myColorAttributes; }
			set { myColorAttributes = value; }
		}

		/// <summary>
		/// The original fact text
		/// </summary>
		/// <value></value>
		public string OriginalFactText
		{
			get { return myOriginalFactText; }
			set { myOriginalFactText = value; }
		}
	}

	/// <summary>
	/// An object found in the Fact Editor's structured text
	/// which includes the object name, ref mode, quantifiers,
	/// and position in the fact
	/// </summary>
	public class FactObject
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
		public FactObject()
		{
//			myRoleQuantifiers = new FactQuantifierCollection();
		}
		/// <summary>
		/// The name of the object
		/// </summary>
		/// <value></value>
		public string Name
		{
			get { return myName; }
			set { myName = value; }
		}
		/// <summary>
		/// The object's reference mode
		/// </summary>
		/// <value></value>
		public string RefMode
		{
			get { return myRefMode; }
			set { myRefMode = value; }
		}
		/// <summary>
		/// True if parenthesis were found in the fact (even if the
		/// reference mode is blank)
		/// </summary>
		/// <value></value>
		public bool RefModeHasParenthesis
		{
			get { return myRefModeHasParenthesis; }
			set { myRefModeHasParenthesis = value; }
		}
//		/// <summary>
//		/// Quantifiers which indicate mandatory or uniqueness
//		/// </summary>
//		/// <value></value>
//		public FactQuantifierCollection RoleQuantifiers
//		{
//			get { return myRoleQuantifiers; }
//		}
		/// <summary>
		/// The position of this object in the parsed fact
		/// </summary>
		/// <value></value>
		public int Position
		{
			get { return myPositionNr; }
			set { myPositionNr = value; }
		}
	}

	/// <summary>
	/// A collection of objects in a fact
	/// </summary>
	public class FactObjectCollection : IList<FactObject>
	{
		private List<FactObject> myList = new List<FactObject>();
		/// <summary>
		/// Creates and adds a new FactObject to the collection
		/// </summary>
		/// <returns>The object created</returns>
		public FactObject NewFactObject()
		{
			FactObject fo = new FactObject();
			myList.Add(fo);
			return fo;
		}

		/// <summary>
		/// Adds a range of FactObjects to the collection
		/// </summary>
		/// <param name="value"></param>
		public void AddRange(FactObjectCollection value)
		{
			myList.AddRange(value);
		}

		#region IList<FactObject> Members

		int IList<FactObject>.IndexOf(FactObject item)
		{
			return this.IndexOf(item);
		}

		/// <summary>
		/// Implements IList.IndexOf
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected int IndexOf(FactObject item)
		{
			return myList.IndexOf(item);
		}

		void IList<FactObject>.Insert(int index, FactObject item)
		{
			this.Insert(index, item);
		}

		/// <summary>
		/// Implements IList.IndexOf
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		protected void Insert(int index, FactObject item)
		{
			myList.Insert(index, item);
		}

		void IList<FactObject>.RemoveAt(int index)
		{
			RemoveAt(index);
		}

		/// <summary>
		/// Implements IList.RemoveAt
		/// </summary>
		/// <param name="index"></param>
		protected void RemoveAt(int index)
		{
			myList.RemoveAt(index);
		}

		FactObject IList<FactObject>.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = value;
			}
		}

		/// <summary>
		/// Implements indexer for IList
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		protected FactObject this[int index]
		{
			get
			{
				return (FactObject)myList[index];
			}
			set
			{
				myList[index] = value;
			}
		}
		#endregion

		#region ICollection<FactObject> Members

		void ICollection<FactObject>.Add(FactObject item)
		{
			Add(item);
		}

		/// <summary>
		/// Implements ICollection.Add
		/// </summary>
		/// <param name="item"></param>
		protected void Add(FactObject item)
		{
			myList.Add(item);
		}

		void ICollection<FactObject>.Clear()
		{
			Clear();
		}

		/// <summary>
		/// Implements ICollection.Clear
		/// </summary>
		protected void Clear()
		{
			myList.Clear();
		}

		bool ICollection<FactObject>.Contains(FactObject item)
		{
			return Contains(item);
		}

		/// <summary>
		/// Implements ICollection.Contains
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected bool Contains(FactObject item)
		{
			return myList.Contains(item);
		}

		void ICollection<FactObject>.CopyTo(FactObject[] array, int arrayIndex)
		{
			CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Implements CopyTo
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		protected void CopyTo(FactObject[] array, int arrayIndex)
		{
			myList.CopyTo(array, arrayIndex);
		}

		int ICollection<FactObject>.Count
		{
			get { return this.Count; }
		}

		/// <summary>
		/// Implements ICollection.Count
		/// </summary>
		/// <value></value>
		protected int Count
		{
			get { return myList.Count; }
		}

		bool ICollection<FactObject>.IsReadOnly
		{
			get { return IsReadOnly; }
		}

		// UNDONE: List<FactObject> doesn't contain an IsReadOnly property, so we're returning false
		/// <summary>
		/// Implements ICollection.IsReadOnly
		/// </summary>
		/// <value></value>
		protected static bool IsReadOnly
		{
			get { return false; }
		}

		bool ICollection<FactObject>.Remove(FactObject item)
		{
			return Remove(item);
		}

		/// <summary>
		/// Implements ICollection.Remove
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected bool Remove(FactObject item)
		{
			return myList.Remove(item);
		}

		#endregion

		#region IEnumerable<FactObject> Members

		IEnumerator<FactObject> IEnumerable<FactObject>.GetEnumerator()
		{
			return GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		/// <summary>
		/// Implements IEnumerable.GetEnumerator()
		/// </summary>
		/// <returns></returns>
		protected IEnumerator<FactObject> GetEnumerator()
		{
			return myList.GetEnumerator();
		}

		#endregion
	}

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
//	/// Quantifiers which are attached to objects, such as mandatory or at least one
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
//		/// <value></value>
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
//		/// <value></value>
//		public int Frequency
//		{
//			get { return myFrequency; }
//			set { myFrequency = value; }
//		}
//		/// <summary>
//		/// The type of quantifier: mandatory, at least one, etc.
//		/// </summary>
//		/// <value></value>
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
	#endregion
}
