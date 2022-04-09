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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	partial class DisplayState
	{
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener automatically
		/// creates display settings instances when the store is loaded.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new DisplaySettingsFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Adds implicit display settings instances.
		/// </summary>
		private sealed class DisplaySettingsFixupListener : DeserializationFixupListener<DisplayState>, INotifyElementAdded
		{
			private bool myLoadingNewFile = false;
			/// <summary>
			/// DisplaySettingsFixupListener constructor
			/// </summary>
			public DisplaySettingsFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Empty implementation
			/// </summary>
			protected sealed override void ProcessElement(DisplayState element, Store store, INotifyElementAdded notifyAdded)
			{
				// Everything happens in PhaseCompleted
			}
			/// <summary>
			/// Directive indicates that <see cref="DisplaySetting.InitializeForNewFile"/> should
			/// be called for new DisplaySetting instances.
			/// </summary>
			protected sealed override void LoadingNewFile()
			{
				myLoadingNewFile = true;
			}
			/// <summary>
			/// Load DisplaySettings instances
			/// </summary>
			protected override void PhaseCompleted(Store store)
			{
				// There are lots of hoops we could jump through to delay create these instances, but it is much easier
				// to simply create the small number of instances than to add code for custom property descriptors to add them
				// on the fly.
				ReadOnlyCollection<DisplayState> displayStates = store.ElementDirectory.FindElements<DisplayState>(false);
				List<DomainClassInfo> settingsTypes = null;
				foreach (DomainClassInfo descendedClass in store.DomainDataDirectory.GetDomainClass(typeof(DisplaySetting)).AllDescendants)
				{
					if (!descendedClass.ImplementationClass.IsAbstract && descendedClass.LocalDescendants.Count == 0)
					{
						(settingsTypes ?? (settingsTypes = new List<DomainClassInfo>())).Add(descendedClass);
					}
				}

				DisplayState displayState = null;
				switch (displayStates.Count)
				{
					case 0:
						break;
					case 1:
						displayState = displayStates[0];
						break;
					default:
						displayState = displayStates[0];
						for (int i = displayStates.Count - 1; i >= 1; --i)
						{
							// This is a singleton, defensive code to enforce this.
							displayStates[i].Delete();
						}
						break;
				}

				if (settingsTypes != null)
				{
					int requiredTypeCount = settingsTypes.Count;
					int leftToCreate = requiredTypeCount;
					LinkedElementCollection<DisplaySetting> settings = null;
					BitTracker typeUsedTracker = new BitTracker(requiredTypeCount);
					if (displayState != null)
					{
						settings = displayState.DisplaySettings;
						for (int i = settings.Count - 1; i >= 0; --i)
						{
							DisplaySetting setting = settings[i];
							int settingTypeIndex = settingsTypes.IndexOf(setting.GetDomainClass());
							if (settingTypeIndex == -1 || typeUsedTracker[settingTypeIndex])
							{
								// Type is a duplicate or no longer a leaf setting, eliminate
								setting.Delete();
							}
							else
							{
								--leftToCreate;
								typeUsedTracker[settingTypeIndex] = true;
							}
						}
					}
					else
					{
						displayState = new DisplayState(store);
						settings = displayState.DisplaySettings;
					}

					displayState.Model = store.ElementDirectory.FindElements<ORMModel>()[0]; 

					for (int i = 0; i < settingsTypes.Count && leftToCreate != 0; ++i)
					{
						if (!typeUsedTracker[i])
						{
							DisplaySetting newSetting = (DisplaySetting)store.ElementFactory.CreateElement(settingsTypes[i]);
							settings.Add(newSetting);
							if (myLoadingNewFile)
							{
								newSetting.InitializeForNewFile();
							}
							--leftToCreate;
						}
					}
				}
				else if (displayState != null)
				{
					displayState.Delete();
				}
			}
			#region INotifyElementAdded Implementation
			void INotifyElementAdded.ElementAdded(ModelElement element)
			{
				// Just block the base from getting this, we're not recording the elements
			}
			#endregion // INotifyElementAdded Implementation
		}
		#endregion // Deserialization Fixup
		#region Property provider
		/// <summary>
		/// A <see cref="PropertyProvider"/> to attach additional properties to a <see cref="Diagram"/>
		/// </summary>
		/// <param name="element">The <see cref="ModelElement"/> to attach properties to.</param>
		/// <param name="properties">The set of <see cref="PropertyDescriptor"/> elements to extend.</param>
		public static void ProvideDiagramProperties(object element, PropertyDescriptorCollection properties)
		{
			Diagram diagram;
			Store store;
			ReadOnlyCollection<DisplayState> displayStates;
			if (null != (diagram = element as Diagram) &&
				null != (store = Utility.ValidateStore(diagram.Store)) &&
				0 != (displayStates = store.ElementDirectory.FindElements<DisplayState>(false)).Count)
			{
				DomainClassInfo domainClass = diagram.GetDomainClass();
				foreach (DisplaySetting setting in displayStates[0].DisplaySettings)
				{
					string nameSuffix = null;
					if (setting.AppliesToDiagramClass(domainClass))
					{
						foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(setting))
						{
							properties.Add(EditorUtility.ModifyPropertyDescriptorDisplay(EditorUtility.RedirectPropertyDescriptor(diagram, descriptor, domainClass.ImplementationClass), descriptor.Name + (nameSuffix ?? (nameSuffix = "_" + setting.GetType().FullName)), null, null, null));
						}
					}
				}
			}
		}
		#endregion // Property provider
	}
	partial class DisplaySetting
	{
		/// <summary>
		/// Determine if these properties should be displayed with the given diagram type.
		/// </summary>
		/// <param name="diagramClassInfo"></param>
		/// <returns>Return <see langword="true"/> if properties from this instance should be added to a given diagram type.</returns>
		public abstract bool AppliesToDiagramClass(DomainClassInfo diagramClassInfo);
		/// <summary>
		/// Called after this instance is newly created for a new file.
		/// </summary>
		public virtual void InitializeForNewFile()
		{
			// No default implementation
		}
	}
}
