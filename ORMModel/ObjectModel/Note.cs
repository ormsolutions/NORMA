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
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region INoteOwner interface
	/// <summary>
	/// An interface to implement for any Note owner
	/// </summary>
	/// <typeparam name="NoteType">The type of a DomainClass with a domain property called Text.</typeparam>
	public interface INoteOwner<NoteType> where NoteType : ModelElement
	{
		/// <summary>
		/// Return the name of the note owner
		/// </summary>
		string Name { get;}
		/// <summary>
		/// The current text value of a note
		/// </summary>
		string NoteText { get;}
		/// <summary>
		/// A property descriptor corresponding to the NoteText property
		/// </summary>
		PropertyDescriptor NoteTextPropertyDescriptor { get;}
	}
	#endregion // INoteOwner interface
	partial class Note
	{
		#region NoteChangeRule
		/// <summary>
		/// ChangeRule: typeof(Note)
		/// Handle custom storage properties on Note
		/// </summary>
		private static void NoteChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeGuid = e.DomainProperty.Id;
			// If what was changed was the note's Text property,
			if (attributeGuid == Note.TextDomainPropertyId)
			{
				// cache the value.
				string newText = (string)e.NewValue;
				// Get the note
				Note note = e.ModelElement as Note;
				// and if the text is blank, 
				if (string.IsNullOrEmpty(newText))
				{
					// get rid of the note.
					note.Delete();
				}
			}
		}
		#endregion // NoteChangeRule
	}
	partial class FactType : INoteOwner<Note>, INoteOwner<Definition>
	{
		#region INoteOwner<Note> Implementation
		/// <summary>
		/// Implements <see cref="INoteOwner{Note}.NoteTextPropertyDescriptor"/>
		/// </summary>
		protected PropertyDescriptor NoteTextPropertyDescriptor
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(this, NoteTextDomainPropertyId);
			}
		}
		PropertyDescriptor INoteOwner<Note>.NoteTextPropertyDescriptor
		{
			get
			{
				return NoteTextPropertyDescriptor;
			}
		}
		#endregion // INoteOwner<Note> Implementation
		#region INoteOwner<Definition> Implementation
		/// <summary>
		/// Implements <see cref="INoteOwner{Definition}.NoteTextPropertyDescriptor"/>
		/// </summary>
		protected PropertyDescriptor DefinitionTextPropertyDescriptor
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(this, DefinitionTextDomainPropertyId);
			}
		}
		PropertyDescriptor INoteOwner<Definition>.NoteTextPropertyDescriptor
		{
			get
			{
				return DefinitionTextPropertyDescriptor;
			}
		}
		string INoteOwner<Definition>.NoteText
		{
			get
			{
				return DefinitionText;
			}
		}
		#endregion // INoteOwner<Definition> Implementation
	}
	partial class ObjectType : INoteOwner<Note>, INoteOwner<Definition>
	{
		#region INoteOwner<Note> Implementation
		/// <summary>
		/// Implements <see cref="INoteOwner{Note}.NoteTextPropertyDescriptor"/>
		/// </summary>
		protected PropertyDescriptor NoteTextPropertyDescriptor
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(this, NoteTextDomainPropertyId);
			}
		}
		PropertyDescriptor INoteOwner<Note>.NoteTextPropertyDescriptor
		{
			get
			{
				return NoteTextPropertyDescriptor;
			}
		}
		#endregion // INoteOwner<Note> Implementation
		#region INoteOwner<Definition> Implementation
		/// <summary>
		/// Implements <see cref="INoteOwner{Definition}.NoteTextPropertyDescriptor"/>
		/// </summary>
		protected PropertyDescriptor DefinitionTextPropertyDescriptor
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(this, DefinitionTextDomainPropertyId);
			}
		}
		PropertyDescriptor INoteOwner<Definition>.NoteTextPropertyDescriptor
		{
			get
			{
				return DefinitionTextPropertyDescriptor;
			}
		}
		string INoteOwner<Definition>.NoteText
		{
			get
			{
				return DefinitionText;
			}
		}
		#endregion // INoteOwner<Definition> Implementation
	}
	partial class ORMModel : INoteOwner<Note>, INoteOwner<Definition>
	{
		#region INoteOwner<Note> Implementation
		/// <summary>
		/// Implements <see cref="INoteOwner{Note}.NoteTextPropertyDescriptor"/>
		/// </summary>
		protected PropertyDescriptor NoteTextPropertyDescriptor
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(this, NoteTextDomainPropertyId);
			}
		}
		PropertyDescriptor INoteOwner<Note>.NoteTextPropertyDescriptor
		{
			get
			{
				return NoteTextPropertyDescriptor;
			}
		}
		#endregion // INoteOwner<Note> Implementation
		#region INoteOwner<Definition> Implementation
		/// <summary>
		/// Implements <see cref="INoteOwner{Definition}.NoteTextPropertyDescriptor"/>
		/// </summary>
		protected PropertyDescriptor DefinitionTextPropertyDescriptor
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(this, DefinitionTextDomainPropertyId);
			}
		}
		PropertyDescriptor INoteOwner<Definition>.NoteTextPropertyDescriptor
		{
			get
			{
				return DefinitionTextPropertyDescriptor;
			}
		}
		string INoteOwner<Definition>.NoteText
		{
			get
			{
				return DefinitionText;
			}
		}
		#endregion // INoteOwner<Definition> Implementation
	}
	partial class ModelNote : INoteOwner<Note>
	{
		#region INoteOwner Implementation
		/// <summary>
		/// Implements <see cref="INoteOwner{Note}.Name"/>
		/// </summary>
		protected string Name
		{
			get
			{
				// UNDONE: Display an index with the NoteText property
				return TypeDescriptor.GetClassName(this);
			}
		}
		string INoteOwner<Note>.Name
		{
			get
			{
				return Name;
			}
		}
		/// <summary>
		/// Implements <see cref="INoteOwner{Note}.NoteText"/>
		/// </summary>
		protected string NoteText
		{
			get
			{
				return Text;
			}
		}
		string INoteOwner<Note>.NoteText
		{
			get
			{
				return NoteText;
			}
		}
		/// <summary>
		/// Implements <see cref="INoteOwner{Note}.NoteTextPropertyDescriptor"/>
		/// </summary>
		protected PropertyDescriptor NoteTextPropertyDescriptor
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(this, TextDomainPropertyId);
			}
		}
		PropertyDescriptor INoteOwner<Note>.NoteTextPropertyDescriptor
		{
			get
			{
				return NoteTextPropertyDescriptor;
			}
		}
		#endregion // INoteOwner Implementation
	}
}
