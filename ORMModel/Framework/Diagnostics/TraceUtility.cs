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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms.Design;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace Neumont.Tools.Modeling.Diagnostics
{
	/// <summary>
	/// Utility class used for trace diagnostics
	/// </summary>
	public static class TraceUtility
	{
		#region TraceRule methods
		/// <summary>
		/// Trace switch for determining which changes are made by which rules. To enable, modify the devenv.exe.config
		/// files with the following information, or explicity set the TraceLevel on the switch while debugging. The command
		/// numbers generated here will correspond to the command values shown in the transaction log viewer.
		///<system.diagnostics>
		///    <switches>
		///        <add name="Neumont.Tools.Modeling.Diagnostics.TraceRules" value="Verbose"/>
		///    </switches>
		///</system.diagnostics>
		/// </summary>
		public static readonly TraceSwitch TraceRulesSwitch = new TraceSwitch("Neumont.Tools.Modeling.Diagnostics.TraceRules", "Switch determining if rules are traced", "Off");
		#region Dynamic Microsoft.VisualStudio.Modeling.Transaction.CommandCount implementation
		private delegate int TransactionCommandCountDelegate(Transaction @this);
		/// <summary>
		/// Microsoft.VisualStudio.Modeling.ModelCommand is internal
		/// </summary>
		private static readonly TransactionCommandCountDelegate TransactionCommandCount = CreateTransactionCommandCount();
		private static TransactionCommandCountDelegate CreateTransactionCommandCount()
		{
			Type transactionType = typeof(Transaction);
			Assembly modelingAssembly = transactionType.Assembly;
			string privateTypeBaseName = transactionType.Namespace + Type.Delimiter;
			Type modelCommandType;
			Type modelCommandListType;
			PropertyInfo commandsProperty;
			MethodInfo getCommandsMethod;
			if (null == (modelCommandType = modelingAssembly.GetType(privateTypeBaseName + "ModelCommand", false)) ||
				null == (modelCommandListType = typeof(List<>).MakeGenericType(modelCommandType)) ||
				null == (commandsProperty = transactionType.GetProperty("Commands", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) ||
				commandsProperty.PropertyType != modelCommandListType ||
				null == (getCommandsMethod = commandsProperty.GetGetMethod(true)))
			{
				// The structure of the internal dll implementation has changed, il generation will fail
				return null;
			}

			// Approximate method being written (assuming Transaction context):
			// int CommandCount()
			// {
			//     return Commands.Count;
			// }
			DynamicMethod dynamicMethod = new DynamicMethod(
				"TransactionCommandCount",
				typeof(int),
				new Type[] { transactionType },
				transactionType,
				true);
			// ILGenerator tends to be rather aggressive with capacity checks, so we'll ask for more than the required 12 bytes
			// to avoid a resize to an even larger buffer.
			ILGenerator il = dynamicMethod.GetILGenerator(16);
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Call, getCommandsMethod);

			// Cache the loop count and return the value
			il.Emit(OpCodes.Call, modelCommandListType.GetProperty("Count").GetGetMethod());
			il.Emit(OpCodes.Ret);
			return (TransactionCommandCountDelegate)dynamicMethod.CreateDelegate(typeof(TransactionCommandCountDelegate));
		}
		#endregion // Dynamic Microsoft.VisualStudio.Modeling.Transaction.CommandCount implementation
		private static long LastTransactionSequenceNumber = -1;
		/// <summary>
		/// Call at the beginning of a rule
		/// </summary>
		/// <param name="store">The <see cref="Store"/> associated with the change</param>
		/// <param name="ruleName">The full name of the rule class that is about to execute.</param>
		[Conditional("TRACE")]
		public static void TraceRuleStart(Store store, string ruleName)
		{
			if (TraceRulesSwitch.TraceVerbose)
			{
				Transaction transaction = store.TransactionManager.CurrentTransaction;
				long sequenceNumber = transaction.TopLevelTransaction.SequenceNumber;
				if (LastTransactionSequenceNumber != sequenceNumber)
				{
					LastTransactionSequenceNumber = sequenceNumber;
					Trace.IndentLevel = 0;
				}
				Trace.Write("->R ");
				Trace.Write(ruleName);
				Trace.Write(", command count = ");
				Trace.WriteLine(TransactionCommandCount(transaction).ToString());
				++Trace.IndentLevel;
			}
		}
		/// <summary>
		/// Call at the end of a rule
		/// </summary>
		/// <param name="store">The <see cref="Store"/> associated with the change</param>
		/// <param name="ruleName">The full name of the rule class that has completed executing.</param>
		[Conditional("TRACE")]
		public static void TraceRuleEnd(Store store, string ruleName)
		{
			if (TraceRulesSwitch.TraceVerbose)
			{
				Transaction transaction = store.TransactionManager.CurrentTransaction;
				int indentLevel = Trace.IndentLevel;
				if (indentLevel > 0)
				{
					Trace.IndentLevel = indentLevel - 1;
				}
				Trace.Write("<-R ");
				Trace.Write(ruleName);
				Trace.Write(", command count = ");
				Trace.WriteLine(TransactionCommandCount(transaction).ToString());
			}
		}
		/// <summary>
		/// Call at the beginning of a callback delegate, such as those used
		/// with the delayed validation pattern
		/// </summary>
		/// <param name="store">The <see cref="Store"/> associated with the change</param>
		/// <param name="method">A delegate bound to the method to trace</param>
		[Conditional("TRACE")]
		public static void TraceDelegateStart(Store store, Delegate method)
		{
			if (TraceRulesSwitch.TraceVerbose)
			{
				Transaction transaction = store.TransactionManager.CurrentTransaction;
				long sequenceNumber = transaction.TopLevelTransaction.SequenceNumber;
				if (LastTransactionSequenceNumber != sequenceNumber)
				{
					LastTransactionSequenceNumber = sequenceNumber;
					Trace.IndentLevel = 0;
				}
				Trace.Write("->D ");
				MethodInfo methodInfo = method.Method;
				Trace.Write(methodInfo.ReflectedType.FullName);
				Trace.Write(".");
				Trace.Write(methodInfo.Name);
				Trace.Write(", command count = ");
				Trace.WriteLine(TransactionCommandCount(transaction).ToString());
				++Trace.IndentLevel;
			}
		}
		/// <summary>
		/// Call at the end of a callback delegate, such as those used
		/// with the delayed validation pattern
		/// </summary>
		/// <param name="store">The <see cref="Store"/> associated with the change</param>
		/// <param name="method">A delegate bound to the method to trace</param>
		[Conditional("TRACE")]
		public static void TraceDelegateEnd(Store store, Delegate method)
		{
			if (TraceRulesSwitch.TraceVerbose)
			{
				Transaction transaction = store.TransactionManager.CurrentTransaction;
				int indentLevel = Trace.IndentLevel;
				if (indentLevel > 0)
				{
					Trace.IndentLevel = indentLevel - 1;
				}
				Trace.Write("<-D ");
				MethodInfo methodInfo = method.Method;
				Trace.Write(methodInfo.ReflectedType.FullName);
				Trace.Write(".");
				Trace.Write(methodInfo.Name);
				Trace.Write(", command count = ");
				Trace.WriteLine(TransactionCommandCount(transaction).ToString());
			}
		}
		#endregion // TraceRule methods
	}
}