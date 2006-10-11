#region zlib/libpng Copyright Notice
/**************************************************************************\
* Database Intermediate Language                                           *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* This software is provided 'as-is', without any express or implied        *
* warranty. In no event will the authors be held liable for any damages    *
* arising from the use of this software.                                   *
*                                                                          *
* Permission is granted to anyone to use this software for any purpose,    *
* including commercial applications, and to alter it and redistribute it   *
* freely, subject to the following restrictions:                           *
*                                                                          *
* 1. The origin of this software must not be misrepresented; you must not  *
*    claim that you wrote the original software. If you use this software  *
*    in a product, an acknowledgment in the product documentation would be *
*    appreciated but is not required.                                      *
*                                                                          *
* 2. Altered source versions must be plainly marked as such, and must not  *
*    be misrepresented as being the original software.                     *
*                                                                          *
* 3. This notice may not be removed or altered from any source             *
*    distribution.                                                         *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Neumont.Tools.DIL.Unicode
{
	#region GeneralCategory enum
	[Serializable]
	public enum GeneralCategory : short
	{
		/// <summary>An invalid value (not part of the Unicode Standard)</summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		Invalid = 0,

		/// <summary>Letter</summary>
		L = 1 << 14,
		/// <summary>Letter, Uppercase</summary>
		Lu = L | 1 << 1,
		/// <summary>Letter, Lowercase</summary>
		Ll = L | 1 << 2,
		/// <summary>Letter, Titlecase</summary>
		Lt = L | 1 << 3,
		/// <summary>Letter, Modifier</summary>
		Lm = L | 1 << 4,
		/// <summary>Letter, Other</summary>
		Lo = L | 1 << 5,

		/// <summary>Mark</summary>
		M = 1 << 13,
		/// <summary>Mark, Nonspacing</summary>
		Mn = M | 1 << 1,
		/// <summary>Mark, Spacing Combining</summary>
		Mc = M | 1 << 2,
		/// <summary>Mark, Enclosing</summary>
		Me = M | 1 << 3,

		/// <summary>Number</summary>
		N = 1 << 12,
		/// <summary>Number, Decimal Digit</summary>
		Nd = N | 1 << 1,
		/// <summary>Number, Letter</summary>
		Nl = N | 1 << 2,
		/// <summary>Number, Other</summary>
		No = N | 1 << 3,

		/// <summary>Punctuation</summary>
		P = 1 << 11,
		/// <summary>Punctuation, Connector</summary>
		Pc = P | 1 << 1,
		/// <summary>Punctuation, Dash</summary>
		Pd = P | 1 << 2,
		/// <summary>Punctuation, Open</summary>
		Ps = P | 1 << 3,
		/// <summary>Punctuation, Close</summary>
		Pe = P | 1 << 4,
		/// <summary>Punctuation, Initial quote (may behave like Ps or Pe depending on usage)</summary>
		Pi = P | 1 << 5,
		/// <summary>Punctuation, Final quote (may behave like Ps or Pe depending on usage)</summary>
		Pf = P | 1 << 6,
		/// <summary>Punctuation, Other</summary>
		Po = P | 1 << 7,

		/// <summary>Symbol</summary>
		S = 1 << 10,
		/// <summary>Symbol, Math</summary>
		Sm = S | 1 << 1,
		/// <summary>Symbol, Currency</summary>
		Sc = S | 1 << 2,
		/// <summary>Symbol, Modifier</summary>
		Sk = S | 1 << 3,
		/// <summary>Symbol, Other</summary>
		So = S | 1 << 4,

		/// <summary>Separator</summary>
		Z = 1 << 9,
		/// <summary>Separator, Space</summary>
		Zs = Z | 1 << 1,
		/// <summary>Separator, Line</summary>
		Zl = Z | 1 << 2,
		/// <summary>Separator, Paragraph</summary>
		Zp = Z | 1 << 3,

		/// <summary>Other</summary>
		C = 1 << 8,
		/// <summary>Other, Control</summary>
		Cc = C | 1 << 1,
		/// <summary>Other, Format</summary>
		Cf = C | 1 << 2,
		/// <summary>Other, Surrogate</summary>
		Cs = C | 1 << 3,
		/// <summary>Other, Private Use</summary>
		Co = C | 1 << 4,
		/// <summary>Other, Not Assigned (no characters in the file have this property)</summary>
		Cn = C | 1 << 5,
	}
	#endregion // GeneralCategory enum

	#region CanonicalCombiningClass pseudo-enum
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public static class CanonicalCombiningClass
	{
		/// <summary>Spacing, split, enclosing, reordrant, and Tibetan subjoined</summary>
		public const byte SpacingSplitEnclosingReordrantAndTibetanSubjoined = 0;
		/// <summary>Overlays and interior</summary>
		public const byte OverlaysAndInterior = 1;
		/// <summary>Nuktas</summary>
		public const byte Nuktas = 7;
		/// <summary>Hiragana/Katakana voicing marks</summary>
		public const byte HiraganaKatakanaVoicingMarks = 8;
		/// <summary>Viramas</summary>
		public const byte Viramas = 9;
		/// <summary>Start of fixed position classes</summary>
		public const byte StartOfFixedPositionClasses = 10;
		/// <summary>End of fixed position classes</summary>
		public const byte EndOfFixedPositionClasses = 199;
		/// <summary>Below left attached</summary>
		public const byte BelowLeftAttached = 200;
		/// <summary>Below attached</summary>
		public const byte BelowAttached = 202;
		/// <summary>Below right attached</summary>
		public const byte BelowRightAttached = 204;
		/// <summary>Left attached (reordrant around single base character)</summary>
		public const byte LeftAttachedReordrantAroundSingleBaseCharacter = 208;
		/// <summary>Right attached</summary>
		public const byte RightAttached = 210;
		/// <summary>Above left atttached</summary>
		public const byte AboveLeftAttached = 212;
		/// <summary>Above attached</summary>
		public const byte AboveAttached = 214;
		/// <summary>Above right attached</summary>
		public const byte AboveRightAttached = 216;
		/// <summary>Below left</summary>
		public const byte BelowLeft = 218;
		/// <summary>Below</summary>
		public const byte Below = 220;
		/// <summary>Below right</summary>
		public const byte BelowRight = 222;
		/// <summary>Left (reordrant around single base character) </summary>
		public const byte LeftReordrantAroundSingleBaseCharacter = 224;
		/// <summary>Right</summary>
		public const byte Right = 226;
		/// <summary>Above left</summary>
		public const byte AboveLeft = 228;
		/// <summary>Above</summary>
		public const byte Above = 230;
		/// <summary>Above right</summary>
		public const byte AboveRight = 232;
		/// <summary>Double below</summary>
		public const byte DoubleBelow = 233;
		/// <summary>Double above</summary>
		public const byte DoubleAbove = 234;
		/// <summary>Below (iota subscript)</summary>
		public const byte BelowIotaSubscript = 240;
	}
	#endregion // CanonicalCombiningClass pseudo-enum

	#region BidiClass enum
	/// <summary>
	/// Please refer to UAX #9: The Bidirectional Algorithm [BIDI] for an explanation of the algorithm for
	/// Bidirectional Behavior and an explanation of the significance of these categories.
	/// </summary>
	[Serializable]
	public enum BidiClass
	{
		/// <summary>An invalid value (not part of the Unicode Standard)</summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		Invalid = 0,

		/// <summary>Left-to-Right</summary>
		L,
		/// <summary>Left-to-Right Embedding</summary>
		LRE,
		/// <summary>Left-to-Right Override</summary>
		LRO,
		/// <summary>Right-to-Left</summary>
		R,
		/// <summary>Right-to-Left Arabic</summary>
		AL,
		/// <summary>Right-to-Left Embedding</summary>
		RLE,
		/// <summary>Right-to-Left Override</summary>
		RLO,
		/// <summary>Pop Directional Format</summary>
		PDF,
		/// <summary>European Number</summary>
		EN,
		/// <summary>European Number Separator</summary>
		ES,
		/// <summary>European Number Terminator</summary>
		ET,
		/// <summary>Arabic Number</summary>
		AN,
		/// <summary>Common Number Separator</summary>
		CS,
		/// <summary>Non-Spacing Mark</summary>
		NSM,
		/// <summary>Boundary Neutral</summary>
		BN,
		/// <summary>Paragraph Separator</summary>
		B,
		/// <summary>Segment Separator</summary>
		S,
		/// <summary>Whitespace</summary>
		WS,
		/// <summary>Other Neutrals</summary>
		ON,
	}
	#endregion // BidiClass enum

	[Serializable]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public partial struct CodePoint : IComparable<CodePoint>, IEquatable<CodePoint>, IComparable
	{
		private static readonly SortedList<uint, CodePoint> codePointsByValue = new SortedList<uint, CodePoint>(18000);
		private static readonly IList<CodePoint> _codePoints = codePointsByValue.Values;
		public static IList<CodePoint> CodePoints
		{
			get
			{
				return _codePoints;
			}
		}

		private static readonly Dictionary<uint, CodePoint[]> uppercaseMappings = new Dictionary<uint, CodePoint[]>(100);
		private static readonly Dictionary<uint, CodePoint[]> lowercaseMappings = new Dictionary<uint, CodePoint[]>(100);
		private static readonly Dictionary<uint, CodePoint[]> titlecaseMappings = new Dictionary<uint, CodePoint[]>(100);

		private CodePoint(uint value, string name, GeneralCategory generalCategory, byte canonicalCombiningClass, BidiClass bidiClass, bool bidiMirrored, string unicode1Name, string isoComment, uint? simpleUppercaseMappingValue, uint? simpleLowercaseMappingValue, uint? simpleTitlecasemappingValue)
		{
			this._value = value;
			this._name = name;
			this._generalCategory = generalCategory;
			this._canonicalCombiningClass = canonicalCombiningClass;
			this._bidiClass = bidiClass;
			this._bidiMirrored = bidiMirrored;
			this._unicode1Name = unicode1Name;
			this._isoComment = isoComment;
			this._simpleUppercaseMappingValue = simpleUppercaseMappingValue;
			this._simpleLowercaseMappingValue = simpleLowercaseMappingValue;
			this._simpleTitlecaseMappingValue = simpleTitlecasemappingValue;
		}

		#region Infrastructure methods

		public override string ToString()
		{
			return Char.ConvertFromUtf32(unchecked((int)this._value));
		}
		public override int GetHashCode()
		{
			return unchecked((int)this._value);
		}
		public override bool Equals(object obj)
		{
			return obj is CodePoint && this._value == ((CodePoint)obj)._value;
		}
		public bool Equals(CodePoint other)
		{
			return this._value == other._value;
		}
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			return this._value.CompareTo(((CodePoint)obj)._value);
		}
		public int CompareTo(CodePoint other)
		{
			return this._value.CompareTo(other._value);
		}

		#region Operators
		public static bool operator ==(CodePoint x, CodePoint y)
		{
			return x._value == y._value;
		}
		public static bool operator !=(CodePoint x, CodePoint y)
		{
			return x._value != y._value;
		}
		public static bool operator >(CodePoint x, CodePoint y)
		{
			return x._value > y._value;
		}
		public static bool operator >=(CodePoint x, CodePoint y)
		{
			return x._value >= y._value;
		}
		public static bool operator <(CodePoint x, CodePoint y)
		{
			return x._value < y._value;
		}
		public static bool operator <=(CodePoint x, CodePoint y)
		{
			return x._value <= y._value;
		}
		#endregion // Operators

		#endregion // Infrastructure methods

		#region Value property
		private readonly uint _value;
		public int SignedValue
		{
			get
			{
				return unchecked((int)this._value);
			}
		}
		[CLSCompliant(false)]
		public uint Value
		{
			get
			{
				return this._value;
			}
		}
		#endregion // Value property

		#region Unicode properties

		#region Name property
		private const string NameDefault = "<reserved>";
		private readonly string _name;
		/// <summary>
		/// (1) These names match exactly the names published in the code charts of the Unicode Standard.
		/// The Hangul Syllable names are omitted from this file; see Jamo.txt.
		/// </summary>
		[Category("Miscellaneous")]
		[DefaultValue(NameDefault)]
		public string Name
		{
			get
			{
				return this._name;
			}
		}
		#endregion // Name property

		#region GeneralCategory property
		private readonly GeneralCategory _generalCategory;
		/// <summary>
		/// (2) This is a useful breakdown into various character types which can be used as a default
		/// categorization in implementations. For the property values, see General Category Values.
		/// </summary>
		[Category("Enumeration (non-binary)")]
		[DefaultValue(GeneralCategory.Cn)]
		public GeneralCategory GeneralCategory
		{
			get
			{
				return this._generalCategory;
			}
		}
		#endregion // GeneralCategory property

		#region CanonicalCombiningClass property
		private readonly byte _canonicalCombiningClass;
		/// <summary>
		/// (3) The classes used for the Canonical Ordering Algorithm in the Unicode Standard. For the property
		/// value names associated with different numeric values, see DerivedCombiningClass.txt and Canonical
		/// Combining Class Values.
		/// </summary>
		[Category("Numeric")]
		[DefaultValue(0)]
		public byte CanonicalCombiningClass
		{
			get
			{
				return this._canonicalCombiningClass;
			}
		}
		#endregion // CanonicalCombiningClass property

		#region BidiClasss property
		private readonly BidiClass _bidiClass;
		/// <summary>
		/// (4) These are the categories required by the Bidirectional Behavior Algorithm in the Unicode Standard.
		/// For the property values, see Bidi Class Values. For more information, see UAX #9: The Bidirectional Algorithm [BIDI].
		/// The default property values depend on the code point, and are given in extracted/DerivedBidiClass.txt
		/// </summary>
		[Category("Enumeration (non-binary)")]
		public BidiClass BidiClass
		{
			get
			{
				return this._bidiClass;
			}
		}
		#endregion // BidiClass property

		// 5, 6, 7, 8 omitted for the moment

		#region BidiMirrored property
		private readonly bool _bidiMirrored;
		/// <summary>
		/// (9) If the character has been identified as a "mirrored" character in bidirectional text, this field has the value "Y";
		/// otherwise "N". The list of mirrored characters is also printed in Chapter 4 of the Unicode Standard.
		/// Do not confuse this with the Bidi_Mirroring_Glyph property.
		/// </summary>
		[Category("Binary")]
		[DefaultValue(false)]
		public bool BidiMirrored
		{
			get
			{
				return this._bidiMirrored;
			}
		}
		#endregion // BidiMirrored property

		#region Unicode1Name property
		private readonly string _unicode1Name;
		/// <summary>
		/// (10) This is the old name as published in Unicode 1.0. This name is only provided when it is significantly different from the
		/// current name for the character. The value of field 10 for control characters does not always match the Unicode 1.0 names. Instead,
		/// field 10 contains ISO 6429 names for control functions, for printing in the code charts.
		/// </summary>
		[Category("Miscellaneous")]
		[DefaultValue(null)]
		public string Unicode1Name
		{
			get
			{
				return this._unicode1Name;
			}
		}
		#endregion // Unicode1Name property

		#region IsoComment property
		private readonly string _isoComment;
		/// <summary>
		/// (11) This is the ISO 10646 comment field. It appears in parentheses in the 10646 names list, or contains an asterisk to mark an Annex P note.
		/// </summary>
		[Category("Miscellaneous")]
		[DefaultValue(null)]
		public string IsoComment
		{
			get
			{
				return this._isoComment;
			}
		}
		#endregion // IsoComment property

		#region SimpleUppercaseMapping property
		private readonly uint? _simpleUppercaseMappingValue;
		/// <summary>
		/// (12) Simple uppercase mapping (single character result). If a character is part of an alphabet with case distinctions, and has a simple upper
		/// case equivalent, then the upper case equivalent is in this field. See the explanation below on case distinctions. The simple mappings have a
		/// single character result, where the full mappings may have multi-character results. For more information, see Case Mappings.
		/// Note: The simple uppercase may be omitted in the data file if the uppercase is the same as the code point itself.
		/// </summary>
		[Category("String")]
		[DefaultValue(null)]
		public CodePoint? SimpleUppercaseMapping
		{
			get
			{
				return this._simpleUppercaseMappingValue.HasValue ? (CodePoint?)CodePoint.codePointsByValue[this._simpleUppercaseMappingValue.Value] : null;
			}
		}
		#endregion // SimpleUppercaseMapping property

		#region SimpleLowercaseMapping property
		private readonly uint? _simpleLowercaseMappingValue;
		/// <summary>
		/// (13) Simple lowercase mapping (single character result). Similar to Uppercase mapping.
		/// Note: The simple lowercase may be omitted in the data file if the lowercase is the same as the code point itself.
		/// </summary>
		[Category("String")]
		[DefaultValue(null)]
		public CodePoint? SimpleLowercaseMapping
		{
			get
			{
				return this._simpleLowercaseMappingValue.HasValue ? (CodePoint?)CodePoint.codePointsByValue[this._simpleLowercaseMappingValue.Value] : null;
			}
		}
		#endregion // SimpleLowercaseMapping property

		#region SimpleTitlecaseMapping property
		private readonly uint? _simpleTitlecaseMappingValue;
		/// <summary>
		/// (14) Similar to Uppercase mapping (single character result).
		/// Note: The simple titlecase may be omitted in the data file if the titlecase is the same as the uppercase.
		/// </summary>
		[Category("String")]
		[DefaultValue(null)]
		public CodePoint? SimpleTitlecaseMapping
		{
			get
			{
				return this._simpleTitlecaseMappingValue.HasValue ? (CodePoint?)CodePoint.codePointsByValue[this._simpleTitlecaseMappingValue.Value] : null;
			}
		}
		#endregion // SimpleTitlecaseMapping property

		#endregion // Unicode properties

		#region Helper properties
		public static CodePoint[] GetUppercaseMapping(CodePoint codePoint)
		{
			CodePoint[] uppercaseMapping;
			if (!CodePoint.uppercaseMappings.TryGetValue(codePoint._value, out uppercaseMapping) && codePoint._simpleUppercaseMappingValue.HasValue)
			{
				uppercaseMapping = new CodePoint[] { CodePoint.codePointsByValue[codePoint._simpleUppercaseMappingValue.Value] };
			}
			return uppercaseMapping;
		}
		public static CodePoint[] GetLowercaseMapping(CodePoint codePoint)
		{
			CodePoint[] lowercaseMapping;
			if (!CodePoint.lowercaseMappings.TryGetValue(codePoint._value, out lowercaseMapping) && codePoint._simpleLowercaseMappingValue.HasValue)
			{
				lowercaseMapping = new CodePoint[] { CodePoint.codePointsByValue[codePoint._simpleLowercaseMappingValue.Value] };
			}
			return lowercaseMapping;
		}
		public static CodePoint[] GetTitlecaseMapping(CodePoint codePoint)
		{
			CodePoint[] titlecaseMapping;
			if (!CodePoint.titlecaseMappings.TryGetValue(codePoint._value, out titlecaseMapping) && codePoint._simpleTitlecaseMappingValue.HasValue)
			{
				titlecaseMapping = new CodePoint[] { CodePoint.codePointsByValue[codePoint._simpleTitlecaseMappingValue.Value] };
			}
			return titlecaseMapping;
		}
		#endregion // Helper properties
	}
}
