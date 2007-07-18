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
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.RelationalModels.ConceptualDatabase;
using Neumont.Tools.Modeling;
using Neumont.Tools.ORMToORMAbstractionBridge;
using Neumont.Tools.ORMAbstraction;
using Neumont.Tools.ORM.ObjectModel;
using System.Diagnostics;

namespace Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge
{
	partial class AssimilationMapping
	{
		#region Extension property provider callback
		/// <summary>
		/// An <see cref="ORMPropertyProvisioning"/> callback for adding extender properties to a <see cref="FactType"/>
		/// </summary>
		public static void PopulateAssimilationMappingExtensionProperties(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
		{
			if (null != GetAssimilationFromFactType((FactType)extendableElement))
			{
				properties.Add(AssimilationMappingPropertyDescriptor.Instance);
			}
		}
		#endregion // Extension property provider callback
		#region Static helper functions
		/// <summary>
		/// Given a <see cref="ConceptTypeAssimilatesConceptType"/> relationship, determine the current
		/// <see cref="AssimilationAbsorptionChoice"/> for that assimilation
		/// </summary>
		public static AssimilationAbsorptionChoice GetAbsorptionChoiceFromAssimilation(ConceptTypeAssimilatesConceptType assimilation)
		{
			AssimilationMapping mapping;
			return (null != assimilation && null != (mapping = AssimilationMappingCustomizesAssimilation.GetAssimilationMapping(assimilation))) ?
				mapping.AbsorptionChoice :
				AssimilationAbsorptionChoice.Absorb;
		}
		/// <summary>
		/// Given a <see cref="FactType"/>, return an associated <see cref="ConceptTypeAssimilatesConceptType"/>
		/// </summary>
		private static ConceptTypeAssimilatesConceptType GetAssimilationFromFactType(FactType factType)
		{
			if (factType != null)
			{
				foreach (ConceptTypeChildHasPathFactType pathLink in ConceptTypeChildHasPathFactType.GetLinksToConceptTypeChild(factType))
				{
					// UNDONE: Is there any guarantee that there is only one of these?
					ConceptTypeAssimilatesConceptType assimilation = pathLink.ConceptTypeChild as ConceptTypeAssimilatesConceptType;
					if (assimilation != null)
					{
						return assimilation;
					}
				}
			}
			return null;
		}
		#endregion // Static helper functions
		#region AssimilationMappingPropertyDescriptor class
		private sealed class AssimilationMappingPropertyDescriptor : PropertyDescriptor
		{
			public static readonly AssimilationMappingPropertyDescriptor Instance = new AssimilationMappingPropertyDescriptor();
			private AssimilationMappingPropertyDescriptor()
				: base("AssmilationMappingPropertyEditor", null)
			{
			}
			public static ConceptTypeAssimilatesConceptType GetAssimilationFromComponent(object component)
			{
				return GetAssimilationFromFactType(Neumont.Tools.ORM.ObjectModel.Design.ORMEditorUtility.ResolveContextFactType(component));
			}
			public sealed override bool CanResetValue(object component)
			{
				return true;
			}
			public sealed override bool ShouldSerializeValue(object component)
			{
				return false;
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.AbsorptionChoicePropertyDisplayName;
				}
			}
			public sealed override string Category
			{
				get
				{
					return ResourceStrings.AbsorptionChoicePropertyCategory;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.AbsorptionChoicePropertyDescription;
				}
			}
			public sealed override object GetValue(object component)
			{
				return GetAbsorptionChoiceFromAssimilation(GetAssimilationFromComponent(component));
			}
			public sealed override Type ComponentType
			{
				get
				{
					return typeof(FactType);
				}
			}
			public sealed override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
			public sealed override Type PropertyType
			{
				get
				{
					return typeof(AssimilationAbsorptionChoice);
				}
			}
			public sealed override void ResetValue(object component)
			{
				SetValue(component, AssimilationAbsorptionChoice.Absorb);
			}
			public sealed override void SetValue(object component, object value)
			{
				ConceptTypeAssimilatesConceptType assimilation = GetAssimilationFromComponent(component);
				if (assimilation != null)
				{
					AssimilationMapping mapping = AssimilationMappingCustomizesAssimilation.GetAssimilationMapping(assimilation);
					AssimilationAbsorptionChoice newChoice = (AssimilationAbsorptionChoice)value;
					Store store = assimilation.Store;
					using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.AbsorptionChoicePropertyTransactionName))
					{
						if (mapping != null)
						{
							mapping.AbsorptionChoice = newChoice;
						}
						else if (newChoice != AssimilationAbsorptionChoice.Absorb)
						{
							MappingCustomizationModel model = null;
							foreach (MappingCustomizationModel findModel in store.ElementDirectory.FindElements<MappingCustomizationModel>())
							{
								model = findModel;
								break;
							}
							if (model == null)
							{
								model = new MappingCustomizationModel(store);
							}
							mapping = new AssimilationMapping(store, new PropertyAssignment(AssimilationMapping.AbsorptionChoiceDomainPropertyId, newChoice));
							mapping.Model = model;
							mapping.Assimilation = assimilation;
						}
						if (t.HasPendingChanges)
						{
							t.Commit();
						}
					}
				}
			}
		}
		#endregion // AssimilationMappingPropertyDescriptor class
	}
}
