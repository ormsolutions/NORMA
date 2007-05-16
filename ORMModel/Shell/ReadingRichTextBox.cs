using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Globalization;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.Shell
{
	#region ReadingRichTextBox control
	/// <summary>
	/// A text control used for editing reading text format strings.
	/// Text will be shown as editable Rtf with provided replacement
	/// fields locked down and uneditable.
	/// </summary>
	public class ReadingRichTextBox : RichTextBox
	{
		#region LockedField structure
		/// <summary>
		/// Structure used to track locked fields in the reading text
		/// </summary>
		private struct LockedField
		{
			private int myIndex;
			private int myLength;
			private string myOriginalFieldText;
			/// <summary>
			/// Create a locked field at the given location and length
			/// </summary>
			/// <param name="index">The zero-based position of the start of the field in current text</param>
			/// <param name="length">The number of characters to lock down.</param>
			/// <param name="originalFieldText">The original field text. Used to rebuild the string.</param>
			public LockedField(int index, int length, string originalFieldText)
			{
				myIndex = index;
				myLength = length;
				myOriginalFieldText = originalFieldText;
			}
			/// <summary>
			/// Move the initial index by the given offset
			/// </summary>
			/// <param name="offset"></param>
			public void OffsetIndex(int offset)
			{
				myIndex += offset;
			}
			/// <summary>
			/// The field index. Initialized in the constructor. Can be modified with the OffsetIndex method.
			/// </summary>
			public int Index
			{
				get
				{
					return myIndex;
				}
			}
			/// <summary>
			/// The length of the field as specified in the constructor.
			/// </summary>
			public int Length
			{
				get
				{
					return myLength;
				}
			}
			/// <summary>
			/// The original field text specified in the constructor. Used to rebuild the reading text.
			/// </summary>
			public string OriginalFieldText
			{
				get
				{
					return myOriginalFieldText;
				}
			}
		}
		#endregion // LockedField structure
		#region Member variables
		private LockedField[] myLockedFields;
		private string myLastText;
		private string myLastRtf;
		private bool myAllowedProtectedEdit;
		private static Regex myDetectNonSpaceRegex;
		#endregion // Member variables
		#region Constructors
		/// <summary>
		/// Create a ReadingRichTextBox with default settings. Call the initialize method
		/// after this point to set other fields.
		/// </summary>
		public ReadingRichTextBox()
		{
			Multiline = false;
			DetectUrls = false;
		}
		#endregion // Constructors
		#region ReadingRichText specific
		/// <summary>
		/// Regular expression to determine if a reading format
		/// string contains anything other than space characters
		/// and format strings.
		/// </summary>
		private static Regex DetectNonSpaceRegex
		{
			get
			{
				Regex retVal = myDetectNonSpaceRegex;
				if (retVal == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myDetectNonSpaceRegex,
						new Regex(
							@"(?n)(^\s*(((?!((?<!\{)\{)[0-9]+(\}(?!\})))\S)+?\s*)+((?<!\{)\{)[0-9]+(\}(?!\})))|(((?<!\{)\{)[0-9]+(\}(?!\}))\s*(((?!((?<!\{)\{)[0-9]+(\}(?!\})))\S)+?\s*)+)",
							RegexOptions.Compiled),
						null);
					retVal = myDetectNonSpaceRegex;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Initialize the control
		/// </summary>
		/// <param name="readingText">The initial reading text. A format string with simple replacement fields.</param>
		/// <param name="fieldReplacements">The replacements to show instead of the replacement fields. The replacement fields will show as locked portions in the text.</param>
		/// <param name="predicateColor">The color for the normal editable text</param>
		/// <param name="replacementColor">The color for the replacement fields</param>
		public void Initialize(string readingText, string[] fieldReplacements, Color predicateColor, Color replacementColor)
		{
			StartInitialize();

			// First, lets see how many valid fields we have. A valid field
			// must be in range and must occur only once. If these conditions
			// do not hold, then those format replacements fields are invalid and
			// needs to be directly edited.
			int replacementCount = fieldReplacements.Length;
			int[] foundReplacements = new int[replacementCount];
			int fieldCount = 0;
			bool allFieldsVisited;
			allFieldsVisited = Reading.VisitFields(
				readingText,
				delegate(int index)
				{
					if (index < replacementCount)
					{
						int currentCount = foundReplacements[index];
						switch (currentCount)
						{
							case 0:
								++fieldCount;
								break;
							case 1:
								--fieldCount;
								break;
						}
						foundReplacements[index] = currentCount + 1;
						//Keep going
						return true;
					}
					else
					{
						return false;
					}
				});
			if (fieldCount != 0 && allFieldsVisited)
			{
				LockedField[] fields = new LockedField[fieldCount];
				int offsetAdjustment = 0;
				int currentField = 0;
				string modifiedString = 
					Reading.ReplaceFields(
					readingText,
					delegate(int index, Match match)
					{
						Group fieldGroup = match.Groups[Reading.ReplaceFieldsMatchFieldGroupName];
						if (index < replacementCount &&
							foundReplacements[index] == 1)
						{
							string replacement = fieldReplacements[index];
							int replacementLength = replacement.Length;
							fields[currentField] = new LockedField(fieldGroup.Index + offsetAdjustment, replacementLength, fieldGroup.Value);
							offsetAdjustment += replacementLength - fieldGroup.Length;
							++currentField;
							return replacement;
						}
						return null;
					});
				FinishInitialize(modifiedString, fields, predicateColor, replacementColor);
			}
			else
			{
				FinishInitialize(readingText, null, predicateColor, replacementColor);
			}
		}
		/// <summary>
		/// Initialize the control without an existing reading text. Creates
		/// an initial reading text with two spaces in between each field.
		/// </summary>
		/// <param name="fieldReplacements">The replacements to show instead of the replacement fields. The replacement fields will show as locked portions in the text.</param>
		/// <param name="predicateColor">The color for the normal editable text</param>
		/// <param name="replacementColor">The color for the replacement fields</param>
		public void Initialize(string[] fieldReplacements, Color predicateColor, Color replacementColor)
		{
			StartInitialize();
			int fieldCount = fieldReplacements.Length;
			LockedField[] fields = new LockedField[fieldCount];
			StringBuilder sb = new StringBuilder();
			int offsetAdjustment = 0;
			for (int i = 0; i < fieldCount; ++i)
			{
				string replacement = fieldReplacements[i];
				int replacementLength = replacement.Length;
				if (i != 0)
				{
					sb.Append("  ");
					offsetAdjustment += 2;
				}
				fields[i] = new LockedField(offsetAdjustment, replacementLength, string.Concat("{", i.ToString(CultureInfo.InvariantCulture), "}"));
				offsetAdjustment += replacementLength;
				sb.Append(replacement);
			}
			if (fieldCount == 1)
			{
				sb.Append(" ");
			}
			FinishInitialize(sb.ToString(), fields, predicateColor, replacementColor);
		}
		/// <summary>
		/// Helper function for different Initialize overloads
		/// </summary>
		private void StartInitialize()
		{
			myLockedFields = null; // Use this field as a key to indicate initialization
			if (IsHandleCreated && TextLength != 0)
			{
				Clear();
			}
		}
		/// <summary>
		/// Helper for different Initialize overloads
		/// </summary>
		/// <param name="displayText">The initial text to display</param>
		/// <param name="fields">An array LockedField elements. Can be null.</param>
		/// <param name="predicateColor">The color for the normal editable text</param>
		/// <param name="replacementColor">The color to use for the locked fields</param>
		private void FinishInitialize(string displayText, LockedField[] fields, Color predicateColor, Color replacementColor)
		{
			ForeColor = predicateColor;
			Text = displayText;
			if (fields != null)
			{
				int fieldCount = fields.Length;
				for (int i = 0; i < fieldCount; ++i)
				{
					LockedField field = fields[i];
					Select(field.Index, field.Length);
					SelectionColor = replacementColor;
					SelectionProtected = true;
				}
				myLastText = displayText;
				myLastRtf = Rtf;
				bool customSelection = false;
				if (fieldCount == 1)
				{
					// If there is no front text, then select text after spaces trailing the field
					LockedField field = fields[0];
					if (field.Index == 0)
					{
						customSelection = true;
						int fieldLength = field.Length;
						int fullLength = displayText.Length;
						int trailingLength = fullLength - fieldLength;
						if (trailingLength == 0)
						{
							Select(fieldLength, 0);
						}
						else
						{
							int trailingStart = fieldLength;
							while (trailingStart < fullLength)
							{
								if (displayText[trailingStart] != ' ')
								{
									break;
								}
								++trailingStart;
							}
							if (trailingStart == fullLength)
							{
								Select(fieldLength + 1, trailingLength - 1);
							}
							else
							{
								Select(trailingStart, fullLength - trailingStart);
							}
						}
					}
				}
				else if (fieldCount >= 2)
				{
					// If there is no front text, then select text (without leading/trailing spaces) between the first two fields
					LockedField leftField = fields[0];
					LockedField rightField = fields[1];
					if (leftField.Index == 0)
					{
						int middleStart = leftField.Length;
						int middleLength = rightField.Index - middleStart;
						customSelection = true;
						if (middleLength == 0)
						{
							Select(middleStart, 0);
						}
						else
						{
							int middleEnd = middleStart + middleLength - 1;
							while (middleStart <= middleEnd)
							{
								if (displayText[middleStart] != ' ')
								{
									break;
								}
								++middleStart;
							}
							while (middleEnd > middleStart)
							{
								if (displayText[middleEnd] != ' ')
								{
									break;
								}
								--middleEnd;
							}
							if (middleEnd < middleStart)
							{
								// Nothing but spaces
								switch (middleLength)
								{
									case 1:
										Select(middleEnd + 1, 0);
										break;
									case 2:
										Select(middleEnd, 0);
										break;
									default:
										Select(middleEnd - middleLength + 2, middleLength - 2);
										break;
								}
							}
							else
							{
								// Select stuff between the spaces
								Select(middleStart, middleEnd - middleStart + 1);
							}
						}
					}
				}
				if (!customSelection)
				{
					Select(0, 0);
				}
			}
			else // fields == null
			{
				// Freeform edit, nothing to track
				myLastRtf = null;
				myLastText = null;
				SelectAll();
			}
			ClearUndo();
			myLockedFields = fields;
			InitializeCompleted();
		}
		/// <summary>
		/// Provide an initialization callback for derived types
		/// </summary>
		protected virtual void InitializeCompleted()
		{
		}
		/// <summary>
		/// Regenerate the current form of the reading text with
		/// the displayed replacements returned to their field
		/// representations. Returned text is always trimmed.
		/// If the reading string consists of spaces and replacement
		/// fields then an empty string is returned.
		/// </summary>
		public string BuildReadingText()
		{
			LockedField[] fields = myLockedFields;
			string currentText = Text;
			if (fields == null)
			{
				return currentText.Trim();
			}
			StringBuilder sb = new StringBuilder(currentText.Length);
			int previousFieldEnd = 0;
			LockedField field;
			for (int i = 0; i < fields.Length; ++i)
			{
				field = fields[i];
				int fieldIndex = field.Index;
				if (fieldIndex > previousFieldEnd)
				{
					sb.Append(currentText, previousFieldEnd, fieldIndex - previousFieldEnd);
				}
				sb.Append(field.OriginalFieldText);
				previousFieldEnd = fieldIndex + field.Length;
			}
			int currentTextLength = currentText.Length;
			if (previousFieldEnd < currentTextLength)
			{
				sb.Append(currentText, previousFieldEnd, currentTextLength - previousFieldEnd);
			}
			currentText = sb.ToString().Trim();
			return DetectNonSpaceRegex.Match(currentText).Success ? currentText : "";
		}
		#endregion // ReadingRichText specific
		#region Base overrides
		/// <summary>
		/// Determine where the first change in a string occured. This can give
		/// different values depending on whether the string is viewed in forward
		/// or reverse perspectives.
		/// </summary>
		/// <param name="previousText">The baseline text</param>
		/// <param name="currentText">The current text</param>
		/// <param name="changeStart">The start of the change</param>
		/// <param name="alternateChangeStart">The alternate start of the change. This will no be greater than.</param>
		/// <returns>true if the change start is not ambiguous</returns>
		private static bool CalculateChangeStart(string previousText, string currentText, out int changeStart, out int alternateChangeStart)
		{
			int previousTextLength = previousText.Length;
			int currentTextLength = currentText.Length;
			int lengthDelta = currentTextLength - previousTextLength;

			// Get a change position moving from the beginning of the string
			int forwardChangeStart = 0;
			int changeRange = Math.Min(currentTextLength, previousTextLength);
			for (; forwardChangeStart < changeRange; ++forwardChangeStart)
			{
				if (currentText[forwardChangeStart] != previousText[forwardChangeStart])
				{
					break;
				}
			}

			// Get a change position moving from the end of the string
			int reverseChangeStart = 0;
			int currentTestPosition = currentTextLength - 1;
			int previousTestPosition = previousTextLength - 1;
			for (; reverseChangeStart < changeRange; ++reverseChangeStart)
			{
				if (currentText[currentTestPosition] != previousText[previousTestPosition])
				{
					break;
				}
				--currentTestPosition;
				--previousTestPosition;
			}
			reverseChangeStart = previousTextLength - reverseChangeStart + ((lengthDelta > 0) ? 0 : lengthDelta);

			// If the reverse is higher than the forward, then we deleted and added characters at the same time, use
			// the forward reading
			if (reverseChangeStart > forwardChangeStart)
			{
				reverseChangeStart = forwardChangeStart;
			}
			changeStart = forwardChangeStart;
			alternateChangeStart = reverseChangeStart;
			return forwardChangeStart == reverseChangeStart;
		}
		/// <summary>
		/// Update locked regions and fix formatting issues when the text is modified
		/// </summary>
		protected override void OnTextChanged(EventArgs e)
		{
			LockedField[] fields = myLockedFields;
			if (fields != null)
			{
				string previousText = myLastText;
				string currentText = Text;
				myLastText = currentText;
				string previousRtf = myLastRtf;
				string currentRtf = Rtf;
				myLastRtf = currentRtf;
				int lengthDelta = currentText.Length - previousText.Length;
				if (lengthDelta != 0)
				{
					int changeStart;
					int alternateChangeStart;
					bool lookForAmbiguousChange = !CalculateChangeStart(previousText, currentText, out changeStart, out alternateChangeStart);
					for (int i = 0; i < fields.Length; ++i)
					{
						int testFieldIndex = fields[i].Index;
						if (lookForAmbiguousChange)
						{
							if (testFieldIndex >= alternateChangeStart && testFieldIndex < changeStart)
							{
								if ((changeStart - alternateChangeStart) < fields[i].Length)
								{
									changeStart = alternateChangeStart;
								}
								else
								{
									// At this point, we can't tell from the text difference
									// alone what we should do so we need to revert to analyzing
									// the underlying rtf
									lookForAmbiguousChange = false;
									int rtfChangeStart;
									int rtfDummyChangeStart;
									CalculateChangeStart(previousRtf, currentRtf, out rtfDummyChangeStart, out rtfChangeStart);
									RtfWalker.VisitTextCharacters(
										Rtf,
										delegate(char character, int textIndex, int rtfIndex)
										{
											if (alternateChangeStart == textIndex)
											{
												if (rtfIndex >= rtfChangeStart)
												{
													changeStart = alternateChangeStart;
												}
												return false;
											}
											return true;
										});
								}
							}
						}
						if (testFieldIndex >= changeStart)
						{
							lookForAmbiguousChange = false;
							fields[i].OffsetIndex(lengthDelta);
							if (myAllowedProtectedEdit)
							{
								// Any text added in this position will
								// get all of the formatting characteristics of the
								// protected character. Clean up the character
								// formats and restore the selection.
								myAllowedProtectedEdit = false;
								if (lengthDelta >= 0)
								{
									// The changeStart will always be larger than alternateChangeStart,
									// so this should only happen for edits immediately before the item.
									int restoreSelStart = SelectionStart;
									int restoreSelLength = SelectionLength;
									Select(changeStart, lengthDelta);
									SelectionProtected = false;
									SelectionColor = ForeColor;
									Select(restoreSelStart, restoreSelLength);
									ClearUndo(); // Don't have the formatting changes in the undo stack
									myLastRtf = Rtf;
								}
							}
							for (int j = i + 1; j < fields.Length; ++j)
							{
								// No reason to continue the range test, any following elements will
								// also have a higher index.
								fields[j].OffsetIndex(lengthDelta);
							}
							break;
						}
					}
				}
			}
			base.OnTextChanged(e);
		}
		#endregion // Base overrides
		#region Paste Handling
		/// <summary>
		/// Handle key down for paste and insert requests
		/// </summary>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if ((e.KeyCode == Keys.V && (e.Modifiers & ~Keys.Shift) == Keys.Control) ||
				(e.KeyCode == Keys.Insert && e.Modifiers == Keys.Shift))
			{
				DoPaste();
				e.SuppressKeyPress = true;
			}
			else if (e.KeyCode == Keys.Insert && e.Modifiers == Keys.None)
			{
				// Insert mode allows overwrite of the initial protected
				// keys when we unprotect the character for normal typing.
				// Block the user from toggling insert mode on in this control.
				e.SuppressKeyPress = true;
			}
			base.OnKeyDown(e);
		}
		/// <summary>
		/// Make sure that only straight text is pasted into the control.
		/// Other text forms (especially RTF) are not allowed. We should
		/// be the only ones doing any RTF in this editor.
		/// </summary>
		private void DoPaste()
		{
			if (Clipboard.ContainsText(TextDataFormat.UnicodeText))
			{
				SelectedText = Clipboard.GetText(TextDataFormat.UnicodeText);
			}
			else if (Clipboard.ContainsText(TextDataFormat.Text))
			{
				SelectedText = Clipboard.GetText(TextDataFormat.Text);
			}
		}
		#endregion // Paste Handling
		#region Protected Region Edits
		#region NativeMethods class
		private static class NativeMethods
		{
			[StructLayout(LayoutKind.Sequential)]
			public struct NMHDR
			{
				public IntPtr hwndFrom;
				public IntPtr idFrom;
				public int code;
			}
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
			public class ENPROTECTED
			{
				public NativeMethods.NMHDR nmhdr;
				public int msg;
				public IntPtr wParam;
				public IntPtr lParam;
				public NativeMethods.CHARRANGE chrg;
				public static ENPROTECTED Create(ref Message m)
				{
					return (IntPtr.Size == sizeof(long)) ?
						ConvertFromENPROTECTED64((ENPROTECTED64)m.GetLParam(typeof(ENPROTECTED64))) :
						(ENPROTECTED)m.GetLParam(typeof(ENPROTECTED));
				}
				[StructLayout(LayoutKind.Explicit)]
				private class ENPROTECTED64
				{
					[FieldOffset(0)]
					public ulong hwndFrom;
					[FieldOffset(8)]
					public ulong idFrom;
					[FieldOffset(0x10)]
					public int code;
					[FieldOffset(0x18)]
					public int msg;
					[FieldOffset(0x1c)]
					public ulong wParam;
					[FieldOffset(0x24)]
					public ulong lParam;
					[FieldOffset(0x2c)]
					public int cpMin;
					[FieldOffset(0x30)]
					public int cpMax;
				}
				private static ENPROTECTED ConvertFromENPROTECTED64(ENPROTECTED64 es64)
				{
					ENPROTECTED retVal = new ENPROTECTED();
					retVal.nmhdr = new NativeMethods.NMHDR();
					retVal.chrg = new NativeMethods.CHARRANGE();
					retVal.nmhdr.hwndFrom = (IntPtr)es64.hwndFrom;
					retVal.nmhdr.idFrom = (IntPtr)es64.idFrom;
					retVal.nmhdr.code = es64.code;
					retVal.msg = es64.msg;
					retVal.wParam = (IntPtr)es64.wParam;
					retVal.lParam = (IntPtr)es64.wParam;
					retVal.chrg.cpMin = es64.cpMin;
					retVal.chrg.cpMax = es64.cpMax;
					return retVal;
				}
			}
			[StructLayout(LayoutKind.Sequential)]
			public class CHARRANGE
			{
				public int cpMin;
				public int cpMax;
			}
			public const int EN_PROTECTED = 0x704;
			public const int WM_KEYDOWN = 0x100;
			public const int WM_APPCOMMAND = 0x319;
			public const int FAPPCOMMAND_MASK = 0xF000;
			public const short APPCOMMAND_PASTE = 38;
			public static short GET_APPCOMMAND_LPARAM(IntPtr lParam)
			{
				return (short)(HIWORD(lParam) & ~FAPPCOMMAND_MASK);
			}
			public static int HIWORD(IntPtr n)
			{
				return HIWORD((int)((long)n));
			}
			public static int HIWORD(int n)
			{
				return ((n >> 0x10) & 0xffff);
			}
		}
		#endregion // NativeMethods class
		/// <summary>
		/// Custom window procedure.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode), SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case NativeMethods.WM_APPCOMMAND:
					if (NativeMethods.GET_APPCOMMAND_LPARAM(m.LParam) == NativeMethods.APPCOMMAND_PASTE)
					{
						DoPaste();
						m.Result = (IntPtr)1;
						return;
					}
					break;
				case 0x204E: // Reflect WM_NOTIFY
					WmReflectNotify(ref m);
					return;
			}
			base.WndProc(ref m);
		}
		private void WmReflectNotify(ref Message m)
		{
			if (m.HWnd != Handle)
			{
				base.WndProc(ref m);
			}
			else
			{
				NativeMethods.NMHDR hdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
				switch (hdr.code)
				{
					case NativeMethods.EN_PROTECTED:
						// WinForms is blocking edits at the beginning of the text
						// if the first character is locked, and between two locked
						// sections if they are adjacent to each other. If we have
						// a zero-length character range that falls at the beginning
						// of a locked field then stop the EN_PROTECTED notification
						// from reaching the base. Note that the OnProtected override
						// is especially lame: you get the notification after the
						// response to the notification is determined and after the control
						// has beeped!
						NativeMethods.ENPROTECTED protectedInfo = NativeMethods.ENPROTECTED.Create(ref m);
						NativeMethods.CHARRANGE range = protectedInfo.chrg;
						int characterIndex = range.cpMin;
						if (characterIndex == range.cpMax)
						{
							LockedField[] fields = myLockedFields;
							if (fields != null)
							{
								for (int i = 0; i < fields.Length; ++i)
								{
									int fieldIndex = fields[i].Index;
									int diff = fieldIndex - characterIndex;
									if (diff >= 0)
									{
										if (diff == 0)
										{
											if (protectedInfo.msg == NativeMethods.WM_KEYDOWN)
											{
												// If we a keydown, then we can't let through
												// a delete and we can't let through a backspace
												// if we're touching the previous field
												bool keepProtected = false;
												switch ((Keys)protectedInfo.wParam)
												{
													case Keys.Delete:
														keepProtected = true;
														break;
													case Keys.Back:
														if (i > 0)
														{
															LockedField previousField = fields[i - 1];
															// If we're touching the previous field, then don't allow
															// a backspace.
															keepProtected = (previousField.Index + previousField.Length) == fieldIndex;
														}
														break;
												}
												if (keepProtected)
												{
													break;
												}
											}
											myAllowedProtectedEdit = true;
											m.Result = (IntPtr)0;
											return;
										}
										break;
									}
								}
							}
						}
						break;
				}
				base.WndProc(ref m);
			}
		}
		#endregion // Protected Region Edits
		#region RtfWalker class
		/// <summary>
		/// A basic rich text parser used to determine the correlation between
		/// a character in the text and its position in the formatted rtf stream.
		/// Note that this has only between tested with the very limited formatting
		/// possible in this control, and has not been tested with Multiline text.
		/// </summary>
		private static class RtfWalker
		{
			/// <summary>
			/// A callback function for use with <see cref="VisitTextCharacters"/>
			/// </summary>
			/// <param name="character">The current text character</param>
			/// <param name="textIndex">The index of this character in the text string</param>
			/// <param name="rtfIndex">The index of this character in the raw Rtf string</param>
			/// <returns>true to continue iteration, false otherwise</returns>
			public delegate bool VisitCharacter(char character, int textIndex, int rtfIndex);
			/// <summary>
			/// Visit all text (non-formatting) characters in the rtf string
			/// </summary>
			/// <param name="rtf">The raw rtf string to walk.</param>
			/// <param name="visitor">The <see cref="VisitCharacter"/> callback delegate</param>
			public static void VisitTextCharacters(string rtf, VisitCharacter visitor)
			{
				int characterIndex = -1;
				int rtfLength = rtf.Length;
				int currentIndex = 1; // Skip the leading open {
				char currentCharacter;
				while (currentIndex < rtfLength)
				{
					switch (currentCharacter = rtf[currentIndex])
					{
						case '\\': // Start of a formatting block
							++currentIndex;
							switch (currentCharacter = rtf[currentIndex])
							{
								case '\\':
								case '}':
								case '{':
									if (!visitor(currentCharacter, ++characterIndex, currentIndex))
									{
										return;
									}
									++currentIndex;
									break;
								default:
									currentIndex = SkipFormattingBlock(rtf, currentIndex);
									break;
							}
							break;
						case '{': // Start of a header block
							currentIndex = SkipHeaderBlock(rtf, currentIndex);
							break;
						case '}':
							// Should only get this unescaped as the last character, we skipped the opening {
							return;
						default:
							if (!visitor(currentCharacter, ++characterIndex, currentIndex))
							{
								return;
							}
							++currentIndex;
							break;
					}
				}
			}
			/// <summary>
			/// Skip a header section
			/// </summary>
			/// <param name="rtf">The rtf string</param>
			/// <param name="currentIndex">The first character inside the block</param>
			/// <returns>The index first character after the header block</returns>
			private static int SkipHeaderBlock(string rtf, int currentIndex)
			{
				char currentCharacter;
				for (; ; )
				{
					switch (currentCharacter = rtf[currentIndex])
					{
						case '{':
							currentIndex = SkipHeaderBlock(rtf, currentIndex + 1);
							break;
						case '}':
							return currentIndex + 1;
						default:
							++currentIndex;
							break;
					}
				}
			}
			/// <summary>
			/// Skip a formatting section
			/// </summary>
			/// <param name="rtf">The rtf string</param>
			/// <param name="currentIndex">The first character inside the formatting block (after the leading \)</param>
			/// <returns>The index of the first character after the formatting section</returns>
			private static int SkipFormattingBlock(string rtf, int currentIndex)
			{
				char currentCharacter;
				for (; ; )
				{
					switch (currentCharacter = rtf[currentIndex])
					{
						case '\\':
							++currentIndex;
							switch (rtf[currentIndex])
							{
								case '\\':
								case '}':
								case '{':
									return currentIndex - 1; // Back up and let the caller reprocess this one
							}
							break;
						case ' ':
							return currentIndex + 1;
						case '{': // Start of a header block
							currentIndex = SkipHeaderBlock(rtf, currentIndex + 1);
							break;
						case '}':
							return currentIndex;
						default:
							++currentIndex;
							break;
					}
				}
			}
		}
		#endregion // RtfWalker class
	}
	#endregion // ReadingRichTextBox control
}
