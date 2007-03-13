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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling.Design;
using System.Globalization;
using System.Resources;

namespace Neumont.Tools.Modeling.Shell
{
	/// <summary>
	/// Controls the behavior of the Diagram Menu Display
	/// </summary>
	[Flags]
	public enum DiagramMenuDisplayOptions
	{
		/// <summary>
		/// Specifies that the diagram has no attributes
		/// </summary>
		None = 0,
		/// <summary>
		/// Specifies that there can be multiple diagrams of this type present.
		/// </summary>
		AllowMultiple = 1 << 0,
		/// <summary>
		/// Specifies that at least one diagram of this type must be present.
		/// </summary>
		Required = 1 << 1,
		/// <summary>
		/// Specifies that diagrams of this type cannot be renamed.
		/// </summary>
		BlockRename = 1 << 2,
	}

	/// <summary>
	/// Specifies how a diagram of this type should be displayed in the menu and what menu options should be available for it.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class DiagramMenuDisplayAttribute : System.Attribute
	{
		private DiagramMenuDisplayOptions diagramOption;
		private string nameResourceId;
		private string imageResourceId;
		private ResourceManager resourceManager;
		
		/// <summary>
		/// Retrieves the diagram options for Diagram Menu Display.
		/// </summary>
		public DiagramMenuDisplayOptions DiagramOption
		{
			get
			{
				return diagramOption;
			}
		}
				
		/// <summary>
		/// Retrieves the Display name for the Diagram Menu.
		/// </summary>
		public string DisplayName
		{
			get
			{
				ResourceManager resourceManager = this.resourceManager;
				if (resourceManager == null || string.IsNullOrEmpty(nameResourceId))
				{
					return string.Empty;
				}
				else
				{
					return resourceManager.GetString(nameResourceId, CultureInfo.CurrentUICulture);
				}
			}
		}

		/// <summary>
		/// Retrieves the Glyph for the Diagram Menu.
		/// </summary>
		public Image Image
		{
			get
			{
				ResourceManager resourceManager = this.resourceManager;
				if (resourceManager == null || string.IsNullOrEmpty(imageResourceId))
				{
					return null;
				}
				else
				{
					return resourceManager.GetObject(imageResourceId, CultureInfo.CurrentUICulture) as Image;
				}
			}
		}

		/// <summary>
		/// Determines how a Diagram Menu Item should be displayed. Supplies the Display Name and Glyph for a Diagram Menu Item
		/// </summary>
		/// <param name="diagramOption">Option that describes if the diagram is Single, Multiple, or Required</param>
		/// <param name="resourceManagerSource">The type of class that the resource manager will target</param>
		/// <param name="nameResourceId">The GUID that identifies the name of the Diagram Menu Item</param>
		/// <param name="imageResourceId">The GUID that identifies the Glyph of the Diagram Menu Item</param>
		public DiagramMenuDisplayAttribute(DiagramMenuDisplayOptions diagramOption, Type resourceManagerSource, string nameResourceId, string imageResourceId)
		{
			if (resourceManagerSource == null)
			{
				throw new ArgumentNullException("ResourceManagerSource");
			}
			this.diagramOption = diagramOption;
			this.nameResourceId = nameResourceId;
			this.imageResourceId = imageResourceId;

			resourceManager = typeof(ResourceAccessor<>).MakeGenericType(resourceManagerSource).InvokeMember("ResourceManager", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.ExactBinding | BindingFlags.DeclaredOnly, null, null, null, null, CultureInfo.InvariantCulture, null) as ResourceManager;

		}
	}
}
