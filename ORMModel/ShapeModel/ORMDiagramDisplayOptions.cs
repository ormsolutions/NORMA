#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	/// <summary>
	/// Determine when a reading direction indicator is displayed for
	/// readings on binary fact types. These options are cumulative,
	/// so any higher display option turns on all other displays.
	/// </summary>
	[TypeConverter(typeof(EnumConverter<ReadingDirectionIndicatorDisplay, ORMDiagram>))]
	[System.CLSCompliant(true)]
	public enum ReadingDirectionIndicatorDisplay
	{
		/// <summary>
		/// Display indicator for reverse readings only.
		/// </summary>
		Reversed,
		/// <summary>
		/// (Deprecated value, split readings are not supported.)
		/// If the forward and reverse readings are split into
		/// two shapes, then display an indicator for both of them.
		/// </summary>
		[Obsolete]
		[Browsable(false)]
		Separated,
		/// <summary>
		/// Display indicator if the fact type is rotated, even if the reading order is top-down.
		/// </summary>
		Rotated,
		/// <summary>
		/// Always display a reading direction indicator.
		/// </summary>
		Always,
	}
	partial class ORMDiagramDisplayOptions
	{
		/// <summary>
		/// Base override, show options with <see cref="ORMDiagram"/> selection
		/// </summary>
		public override bool AppliesToDiagramClass(DomainClassInfo diagramClassInfo)
		{
			return typeof(ORMDiagram).IsAssignableFrom(diagramClassInfo.ImplementationClass);
		}
		/// <summary>
		/// Pull in initial new file settings from the options page.
		/// </summary>
		public override void InitializeForNewFile()
		{
			this.DisplayRoleNames = OptionsPage.CurrentRoleNameDisplay;
			this.DisplayReverseReadings = OptionsPage.CurrentReverseReadingDisplay;
			this.DisplayReadingDirection = OptionsPage.CurrentReadingDirectionIndicatorDisplay;
		}
	}
	/// <summary>
	/// Mark a domain property displayed with an <see cref="ORMDiagram"/> as either a local
	/// or global display property, allowing these to be displayed as nested properties.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ORMDiagramDisplayOptionAttribute : Attribute
	{
		private bool myIsGlobal;
		/// <summary>
		/// Create a new display option attribute for a local display option.
		/// </summary>
		public ORMDiagramDisplayOptionAttribute()
		{
		}
		/// <summary>
		/// Create a new display option attribute for a global or local display option.
		/// </summary>
		/// <param name="isGlobal">The option is global.</param>
		public ORMDiagramDisplayOptionAttribute(bool isGlobal)
		{
			myIsGlobal = true;
		}
		/// <summary>
		/// Standard override. Does the attribute have a default state?
		/// </summary>
		public override bool IsDefaultAttribute()
		{
			return myIsGlobal == false;
		}
		/// <summary>
		/// Standard override
		/// </summary>
		public override bool Equals(object obj)
		{
			ORMDiagramDisplayOptionAttribute other;
			return null != (other = obj as ORMDiagramDisplayOptionAttribute) &&
				other.myIsGlobal == myIsGlobal;
		}
		/// <summary>
		/// Standard override
		/// </summary>
		public override int GetHashCode()
		{
			return myIsGlobal.GetHashCode();
		}
		/// <summary>
		/// This domain property is a global display option. If false, this is
		/// a local display option.
		/// </summary>
		public bool IsGlobal
		{
			get
			{
				return myIsGlobal;
			}
		}
	}
}
