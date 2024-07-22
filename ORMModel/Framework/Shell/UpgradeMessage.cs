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
using System.Reflection;

namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	#region UpgradeMessageProviderAttribute class
	/// <summary>
	/// Provide an IVerbalizationSnippetsProvider implementation for a DomainModel
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class UpgradeMessageProviderAttribute : Attribute
	{
		private Type myProviderType;
		private string myNestedProviderName;
		/// <summary>
		/// Associate an IUpgradeMessageProvider implementation with a DomainModel-derived class
		/// </summary>
		/// <param name="providerType">A type that implements <see cref="IUpgradeMessageProvider"/>
		/// and has a parameterless constructor</param>
		public UpgradeMessageProviderAttribute(Type providerType)
		{
			myProviderType = providerType;
		}

		/// <summary>
		/// Associate an IUpgradeMessageProvider implementation with a DomainModel-derived class
		/// </summary>
		/// <param name="nestedTypeName">The name of a nested class in the DomainModel that implements
		/// the <see cref="IUpgradeMessageProvider"/> interface.</param>
		public UpgradeMessageProviderAttribute(string nestedTypeName)
		{
			myNestedProviderName = nestedTypeName;
		}

		/// <summary>
		/// Retrieve a callback for displaying upgrade messages.
		/// </summary>
		/// <param name="possibleMessageOwners">Types that should be checked for an <see cref="UpgradeMessageProviderAttribute"/></param>
		/// <param name="isMessagePending">Callback to test if a message needs to be shown.</param>
		/// <param name="messageShown">A message has been shown, mark as such.</param>
		/// <returns>A callback to initiate viewing of pending messages pending messages, or null if there are no pending messages.
		/// The <paramref name="messageShown"/> callback will be used while messages are being shownafter messages are shown.</returns>
		public static Action<IServiceProvider> GetPendingMessages(IEnumerable<Type> possibleMessageOwners, Func<Type, string, bool> isMessagePending, Action<Type, string> messageShown)
		{
			List<Tuple<Type, IUpgradeMessageProvider, List<string>>> pendingMessages = null;
			foreach (Type owner in possibleMessageOwners)
			{
				UpgradeMessageProviderAttribute attr = owner.GetCustomAttribute<UpgradeMessageProviderAttribute>(false);
				IUpgradeMessageProvider provider;
				if (attr != null && null != (provider = attr.CreateMessageProvider(owner)))
				{
					List<string> pendingNames = null;
					foreach (string name in provider.UpgradeMessageNames)
					{
						if (isMessagePending(owner, name))
						{
							(pendingNames ?? (pendingNames = new List<string>())).Add(name);
						}
					}

					if (pendingNames != null)
					{
						(pendingMessages ?? (pendingMessages = new List<Tuple<Type, IUpgradeMessageProvider, List<string>>>())).Add(new Tuple<Type, IUpgradeMessageProvider, List<string>>(owner, provider, pendingNames));
					}
				}
			}

			if (pendingMessages != null)
			{
				return (serviceProvider) =>
				{
					List<Tuple<string, Action>> messages = new List<Tuple<string, Action>>();
					foreach (Tuple<Type, IUpgradeMessageProvider, List<string>> messageInfo in pendingMessages)
					{
						Type type = messageInfo.Item1;
						foreach (string name in messageInfo.Item3)
						{
							messages.Add(new Tuple<string, Action>(messageInfo.Item2.GetUpgradeMessage(name), MessageComplete(messageShown, type, name)));
						}
					}
					UpgradeMessageDialog.ShowDialog(serviceProvider, messages);
				};
			}
			return null;
		}
		private static Action MessageComplete(Action<Type, string> messageShown, Type messageType, string messageName)
		{
			return () => messageShown(messageType, messageName);
		}
		/// <summary>
		/// Create an instance of the associated snippets provider
		/// </summary>
		/// <param name="domainModelType">The type of the associated domain model</param>
		/// <returns>IUpgradeMessageProvider implementation</returns>
		private IUpgradeMessageProvider CreateMessageProvider(Type domainModelType)
		{
			Type createType = myProviderType;
			if (createType == null)
			{
				string[] nestedTypeNames = myNestedProviderName.Split(new char[] { '.', '+' }, StringSplitOptions.RemoveEmptyEntries);
				createType = domainModelType;
				for (int i = 0; i < nestedTypeNames.Length; ++i)
				{
					createType = createType.GetNestedType(nestedTypeNames[i], BindingFlags.NonPublic | BindingFlags.Public);
				}
			}
			return createType != null ? Activator.CreateInstance(createType, true) as IUpgradeMessageProvider : null;
		}
	}
	#endregion // UpgradeMessageProviderAttribute class
}
