using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagnostics;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class Note
	{
		#region NoteChangeRule class

		/// <summary>
		/// Enforces Change Rules
		/// </summary>
		[RuleOn(typeof(Note))]
		private class NoteChangeRule : ChangeRule
		{
			/// <summary>
			/// Handle custom storage attributes on Note
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				// If what was changed was the note's Text property,
				if (attributeGuid == Note.TextMetaAttributeGuid)
				{
					// cache the value.
					string newText = (string)e.NewValue;
					// Get the note
					Note note = e.ModelElement as Note;
					// and if the text is blank, 
					if (string.IsNullOrEmpty(newText))
					{
						// get rid of the note.
						note.Remove();
					}
				}
			}
		}
		#endregion // NoteChangeRule class
	}
}
