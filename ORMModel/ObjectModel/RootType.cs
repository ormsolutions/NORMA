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
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class RootType
	{
		/// <summary>
		/// Return a simple name instead of a name decorated with the type (the
		/// default for a ModelElement). This is the easiest way to display
		/// clean names in the property grid when we reference properties.
		/// </summary>
		public override string ToString()
		{
			return Name;
		}

		#region CustomStorage handlers
		/// <summary>
		/// Standard override. All custom storage properties are derived, not
		/// stored. Actual changes are handled in ObjectTypeChangeRule.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <param name="newValue">object</param>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == NoteTextMetaAttributeGuid)
			{
				// Handled by RootTypeChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == NoteTextMetaAttributeGuid)
			{
				Note currentNote = Note;
				return (currentNote != null) ? currentNote.Text : "";
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. Defer to GetValueForCustomStoredAttribute.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		protected override object GetOldValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			return GetValueForCustomStoredAttribute(attribute);
		}
		#endregion // CustomStorage handlers
		#region RootTypeChangeRule class
		/// <summary>
		/// Enforces Change Rules
		/// </summary>
		[RuleOn(typeof(RootType))]
		private class RootTypeChangeRule : ChangeRule
		{
			/// <summary>
			/// Handle custom storage attributes on RootType
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				// If what was changed was the NoteText,
				if (attributeGuid == RootType.NoteTextMetaAttributeGuid)
				{
					// cache the text.
					string newText = (string)e.NewValue;
					RootType rootType = e.ModelElement as RootType;
					// Get the note if it exists
					Note note = rootType.Note;
					if (note != null)
					{
						// and try to set the text to the cached value.
						note.Text = newText;
					}
					else if (!string.IsNullOrEmpty(newText))
					{
						// Otherwise, create the note and set the text,
						note = Note.CreateNote(rootType.Store);
						note.Text = newText;
						// then attach the note to the RootType.
						rootType.Note = note;
					}
				}
			}
		}
		#endregion // RootTypeChangeRule class
	}
}
