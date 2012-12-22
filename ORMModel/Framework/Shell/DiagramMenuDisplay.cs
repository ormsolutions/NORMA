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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Framework.Design;
using System.Globalization;
using System.Resources;

namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	#region DiagramMenuDisplayOptions enum
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
	#endregion // DiagramMenuDisplayOptions enum
	#region IDiagramInitialization interface
	/// <summary>
	/// Support custom diagram initialization when a model is first loaded. Initialization
	/// occurs after the model is fully loaded with all rules and events enabled.
	/// </summary>
	public interface IDiagramInitialization
	{
		/// <summary>
		/// Create and initialize or more diagrams. Used if <see cref="DiagramMenuDisplayOptions.Required"/>
		/// diagram option is displayed.
		/// </summary>
		/// <param name="store"></param>
		/// <returns>Return <see langword="true"/> if diagram initialization was handled and <see langword="false"/>
		/// if a diagram of this type should be created automatically with no initialization.</returns>
		bool CreateRequiredDiagrams(Store store);
		/// <summary>
		/// Perform custom initialization an existing diagram.
		/// </summary>
		/// <param name="diagram">The diagram to process. Can be cast to the type of diagram
		/// where the the <see cref="DiagramMenuDisplayAttribute"/> is specified.</param>
		void InitializeDiagram(Diagram diagram);
	}
	#endregion // IDiagramInitialization interface
	#region DiagramMenuDisplayAttribute class
	/// <summary>
	/// Specifies how a diagram of this type should be displayed in the menu and what menu options should be available for it.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class DiagramMenuDisplayAttribute : System.Attribute
	{
		private DiagramMenuDisplayOptions myDiagramOptions;
		private string myNameResourceId;
		private string myTabImageResourceId;
		private string myBrowserImageResourceId;
		private ResourceManager myResourceManager;
		private string myNestedDiagramInitializerName;
		private Type myDiagramInitializerType;
		
		/// <summary>
		/// Retrieves the diagram options for Diagram Menu Display.
		/// </summary>
		public DiagramMenuDisplayOptions DiagramOption
		{
			get
			{
				return myDiagramOptions;
			}
		}
				
		/// <summary>
		/// Retrieves the Display name for the Diagram Menu.
		/// </summary>
		public string DisplayName
		{
			get
			{
				ResourceManager resourceManager = this.myResourceManager;
				if (resourceManager == null || string.IsNullOrEmpty(myNameResourceId))
				{
					return string.Empty;
				}
				else
				{
					return resourceManager.GetString(myNameResourceId, CultureInfo.CurrentUICulture);
				}
			}
		}

		/// <summary>
		/// Retrieves the Glyph for the Diagram Menu.
		/// </summary>
		public Image TabImage
		{
			get
			{
				ResourceManager resourceManager = this.myResourceManager;
				if (resourceManager == null || string.IsNullOrEmpty(myTabImageResourceId))
				{
					return null;
				}
				else
				{
					return resourceManager.GetObject(myTabImageResourceId, CultureInfo.CurrentUICulture) as Image;
				}
			}
		}

		/// <summary>
		/// Retrieves the Glyph for the Model Browser.
		/// </summary>
		public Image BrowserImage
		{
			get
			{
				ResourceManager resourceManager = this.myResourceManager;
				if (resourceManager == null || string.IsNullOrEmpty(myBrowserImageResourceId))
				{
					return null;
				}
				else
				{
					return resourceManager.GetObject(myBrowserImageResourceId, CultureInfo.CurrentUICulture) as Image;
				}
			}
		}
		/// <summary>
		/// The name of a nested class inside the diagram where this attribute
		/// attribute. The nested class should have a parameterless constructor
		/// and implement <see cref="IDiagramInitialization"/>.
		/// </summary>
		public string NestedDiagramInitializerTypeName
		{
			get
			{
				return myNestedDiagramInitializerName;
			}
			set
			{
				myNestedDiagramInitializerName = value;
			}
		}
		/// <summary>
		/// The type of a class used for diagram initialization. The class must have
		/// a parameterless constructor and implement <see cref="IDiagramInitialization"/>.
		/// </summary>
		public Type DiagramInitializerType
		{
			get
			{
				return myDiagramInitializerType;
			}
			set
			{
				myDiagramInitializerType = value;
			}
		}
		/// <summary>
		/// Create a new diagram initializer for this type.
		/// </summary>
		/// <param name="diagramType">The type of diagram this is attached to.</param>
		/// <returns>A new diagram initializer, or <see langword="null"/> for default initialization.</returns>
		public IDiagramInitialization CreateInitializer(Type diagramType)
		{
			Type createType = myDiagramInitializerType;
			if (createType == null)
			{
				string[] nestedTypeNames = myNestedDiagramInitializerName.Split(new char[] { '.', '+' }, StringSplitOptions.RemoveEmptyEntries);
				createType = diagramType;
				for (int i = 0; i < nestedTypeNames.Length; ++i)
				{
					createType = createType.GetNestedType(nestedTypeNames[i], BindingFlags.NonPublic | BindingFlags.Public);
				}
			}
			return (createType != null) ? (IDiagramInitialization)Activator.CreateInstance(createType, true) : null;
		}
		/// <summary>
		/// Determines how a Diagram Menu Item should be displayed. Supplies the Display Name and Glyph for a Diagram Menu Item
		/// </summary>
		/// <param name="diagramOption">Option that describes if the diagram is Single, Multiple, or Required</param>
		/// <param name="resourceManagerSource">The type of class that the resource manager will target</param>
		/// <param name="nameResourceId">The resource name that identifies the name of the Diagram Menu Item</param>
		/// <param name="tabImageResourceId">The resource name that identifies the Glyph of the Diagram Menu Item</param>
		/// <param name="browserImageResourceId">The resource that identifies the Glyph of diagrams of this type in the Model Browser</param>
		public DiagramMenuDisplayAttribute(DiagramMenuDisplayOptions diagramOption, Type resourceManagerSource, string nameResourceId, string tabImageResourceId, string browserImageResourceId)
		{
			if (resourceManagerSource == null)
			{
				throw new ArgumentNullException("ResourceManagerSource");
			}
			myDiagramOptions = diagramOption;
			myNameResourceId = nameResourceId;
			myTabImageResourceId = tabImageResourceId;
			myBrowserImageResourceId = browserImageResourceId; 

			myResourceManager = typeof(ResourceAccessor<>).MakeGenericType(resourceManagerSource).InvokeMember("ResourceManager", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.ExactBinding | BindingFlags.DeclaredOnly, null, null, null, null, CultureInfo.InvariantCulture, null) as ResourceManager;
		}
	}
	#endregion // DiagramMenuDisplayAttribute class
}
