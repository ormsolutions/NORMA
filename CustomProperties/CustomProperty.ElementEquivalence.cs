#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.CustomProperties
{
	#region CustomPropertyGroup class
	partial class CustomPropertyGroup : IElementEquivalence
	{
		/// <summary>
		/// Map groups by name
		/// </summary>
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			if (null != foreignStore.FindDomainModel(CustomPropertiesDomainModel.DomainModelId))
			{
				string testName = Name;
				foreach (CustomPropertyGroup otherGroup in foreignStore.ElementDirectory.FindElements<CustomPropertyGroup>(false))
				{
					if (otherGroup.Name == testName)
					{
						elementTracker.AddEquivalentElement(this, otherGroup);
						return true;
					}
				}
			}
			return false;
		}
	}
	#endregion // CustomPropertyGroup class
	#region CustomPropertyDefinition class
	partial class CustomPropertyDefinition : IElementEquivalence
	{
		/// <summary>
		/// Map groups by name
		/// </summary>
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			CustomPropertyGroup otherGroup;
			if (null != (otherGroup = CopyMergeUtility.GetEquivalentElement(CustomPropertyGroup, foreignStore, elementTracker)))
			{
				string testName = Name;
				foreach (CustomPropertyDefinition otherDefinition in otherGroup.CustomPropertyDefinitionCollection)
				{
					if (otherDefinition.Name == testName)
					{
						elementTracker.AddEquivalentElement(this, otherDefinition);
						return true;
					}
				}
			}
			return false;
		}
	}
	#endregion // CustomPropertyDefinition class
	#region CustomProperty class
	partial class CustomProperty : IElementEquivalence
	{
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			CustomPropertyDefinition definition;
			CustomPropertyDefinition otherDefinition;
			ORMModelElement extendedElement;
			ORMModelElement otherExtendedElement;
			if (null != (definition = CustomPropertyDefinition) &&
				null != (otherDefinition = CopyMergeUtility.GetEquivalentElement(definition, foreignStore, elementTracker)) &&
				null != (extendedElement = ORMModelElementHasExtensionElement.GetExtendedElement(this)) &&
				null != (otherExtendedElement = CopyMergeUtility.GetEquivalentElement(extendedElement, foreignStore, elementTracker)))
			{
				// UNDONE: COPYMERGE This will not work for custom properties on a model,
				// but these are not currently selectable and all other contained elements should work correctly.
				foreach (ModelElement element in otherExtendedElement.ExtensionCollection)
				{
					CustomProperty otherProperty = element as CustomProperty;
					if (otherProperty != null &&
						otherProperty.CustomPropertyDefinition == otherDefinition)
					{
						elementTracker.AddEquivalentElement(this, otherProperty);
						return true;
					}
				}
			}
			return false;
		}
	}
	#endregion // CustomProperty class
}
