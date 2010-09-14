using System;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © ORM Solutions, LLC. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace ORMSolutions.ORMArchitect.CustomProperties
{
	#region Customize Copy Closure for CustomPropertiesDomainModel
	partial class CustomPropertiesDomainModel : ICopyClosureProvider
	{
		private void AddCopyClosureDirectives(ICopyClosureManager closureManager)
		{
			#region Closures for explicit relationships
			// Override the default embedding behavior. We only need property definitions that we're currently referencing, not the full group.
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CustomPropertyGroupContainsCustomPropertyDefinition.CustomPropertyDefinitionDomainRoleId), new DomainRoleClosureRestriction(CustomPropertyGroupContainsCustomPropertyDefinition.CustomPropertyGroupDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CustomPropertyHasCustomPropertyDefinition.CustomPropertyDomainRoleId), new DomainRoleClosureRestriction(CustomPropertyHasCustomPropertyDefinition.CustomPropertyDefinitionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModelElementHasExtensionElement.ExtendedElementDomainRoleId), new DomainRoleClosureRestriction(ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModelElementHasExtensionElement.ExtensionDomainRoleId, CustomProperty.DomainClassId, false), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			#endregion // Closures for explicit relationships
		}
		void ICopyClosureProvider.AddCopyClosureDirectives(ICopyClosureManager closureManager)
		{
			this.AddCopyClosureDirectives(closureManager);
		}
	}
	#endregion // Customize Copy Closure for CustomPropertiesDomainModel
}
