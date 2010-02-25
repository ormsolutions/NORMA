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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;

namespace ORMSolutions.ORMArchitect.Framework
{
	/// <summary>
	/// Contains general-purpose utility methods.
	/// </summary>
	public static class Utility
	{
		#region GetCombinedHashCode methods
		/// <summary>
		/// Combines multiple hash codes (as returned from implementations of <see cref="Object.GetHashCode"/>)
		/// together in a way that results in an <see cref="Int32"/> value suitable for use as a hash code.
		/// </summary>
		/// <param name="hashCode1">
		/// The first hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode2">
		/// The second hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <returns>
		/// The <see cref="Int32"/> value suitable for use as a hash code that results from combining the hash codes specified by
		/// <paramref name="hashCode1"/> and <paramref name="hashCode2"/>.
		/// </returns>
		public static int GetCombinedHashCode(int hashCode1, int hashCode2)
		{
			return unchecked((int)((uint)hashCode1 ^
				(((uint)hashCode2 >> 1) | ((uint)hashCode2 << (32 - 1)))));
		}
		/// <summary>
		/// Combines multiple hash codes (as returned from implementations of <see cref="Object.GetHashCode"/>)
		/// together in a way that results in an <see cref="Int32"/> value suitable for use as a hash code.
		/// </summary>
		/// <param name="hashCode1">
		/// The first hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode2">
		/// The second hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode3">
		/// The third hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <returns>
		/// The <see cref="Int32"/> value suitable for use as a hash code that results from combining the hash codes specified by
		/// <paramref name="hashCode1"/>, <paramref name="hashCode2"/>, and <paramref name="hashCode3"/>.
		/// </returns>
		public static int GetCombinedHashCode(int hashCode1, int hashCode2, int hashCode3)
		{
			return unchecked((int)((uint)hashCode1 ^
				(((uint)hashCode2 >> 1) | ((uint)hashCode2 << (32 - 1))) ^
				(((uint)hashCode3 >> 2) | ((uint)hashCode3 << (32 - 2)))));
		}
		/// <summary>
		/// Combines multiple hash codes (as returned from implementations of <see cref="Object.GetHashCode"/>)
		/// together in a way that results in an <see cref="Int32"/> value suitable for use as a hash code.
		/// </summary>
		/// <param name="hashCode1">
		/// The first hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode2">
		/// The second hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode3">
		/// The third hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode4">
		/// The fourth hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <returns>
		/// The <see cref="Int32"/> value suitable for use as a hash code that results from combining the hash codes specified by
		/// <paramref name="hashCode1"/>, <paramref name="hashCode2"/>, <paramref name="hashCode3"/>, and <paramref name="hashCode4"/>.
		/// </returns>
		public static int GetCombinedHashCode(int hashCode1, int hashCode2, int hashCode3, int hashCode4)
		{
			return unchecked((int)((uint)hashCode1 ^
				(((uint)hashCode2 >> 1) | ((uint)hashCode2 << (32 - 1))) ^
				(((uint)hashCode3 >> 2) | ((uint)hashCode3 << (32 - 2))) ^
				(((uint)hashCode4 >> 3) | ((uint)hashCode4 << (32 - 3)))));
		}
		/// <summary>
		/// Combines multiple hash codes (as returned from implementations of <see cref="Object.GetHashCode"/>)
		/// together in a way that results in an <see cref="Int32"/> value suitable for use as a hash code.
		/// </summary>
		/// <param name="hashCode1">
		/// The first hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode2">
		/// The second hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode3">
		/// The third hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode4">
		/// The fourth hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode5">
		/// The fifth hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <returns>
		/// The <see cref="Int32"/> value suitable for use as a hash code that results from combining the hash codes specified by
		/// <paramref name="hashCode1"/>, <paramref name="hashCode2"/>, <paramref name="hashCode3"/>, <paramref name="hashCode4"/>, and
		/// <paramref name="hashCode5"/>.
		/// </returns>
		public static int GetCombinedHashCode(int hashCode1, int hashCode2, int hashCode3, int hashCode4, int hashCode5)
		{
			return unchecked((int)((uint)hashCode1 ^
				(((uint)hashCode2 >> 1) | ((uint)hashCode2 << (32 - 1))) ^
				(((uint)hashCode3 >> 2) | ((uint)hashCode3 << (32 - 2))) ^
				(((uint)hashCode4 >> 3) | ((uint)hashCode4 << (32 - 3))) ^
				(((uint)hashCode5 >> 4) | ((uint)hashCode4 << (32 - 4)))));
		}
		/// <summary>
		/// Combines multiple hash codes (as returned from implementations of <see cref="Object.GetHashCode"/>)
		/// together in a way that results in an <see cref="Int32"/> value suitable for use as a hash code.
		/// </summary>
		/// <param name="hashCodes">
		/// The hash codes (<see cref="Int32"/> values returned from implementations of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <returns>
		/// The <see cref="Int32"/> value suitable for use as a hash code that results from combining the specified <paramref name="hashCodes"/>.
		/// </returns>
		public static int GetCombinedHashCode(params int[] hashCodes)
		{
			unchecked
			{
				if (hashCodes == null || hashCodes.Length <= 0)
				{
					return 0;
				}
				uint hashCode = (uint)hashCodes[0];
				for (int i = 1; i < hashCodes.Length; i++)
				{
					hashCode ^= (uint)RotateRight(hashCodes[i], i);
				}
				return (int)hashCode;
			}
		}
		#endregion // GetCombinedHashCode methods
		#region RotateRight method
		/// <summary>
		/// Returns the result of rotating the bits in the <see cref="Int32"/> value specified by <paramref name="value"/>
		/// the number of places specified by <paramref name="places"/>.
		/// </summary>
		/// <param name="value">
		/// The <see cref="Int32"/> value for which the bits should be rotated.
		/// </param>
		/// <param name="places">
		/// The <see cref="Int32"/> value specifying the number of places to rotate the bits in <paramref name="value"/>.
		/// </param>
		/// <returns>
		/// The result of rotating the bits in the <see cref="Int32"/> value specified by <paramref name="value"/>
		/// the number of places specified by <paramref name="places"/>.
		/// </returns>
		/// <remarks>
		/// The rotation is performed without sign extension. That is, the most significant bit is not treated specially.
		/// </remarks>
		public static int RotateRight(int value, int places)
		{
			// NOTE: If changes are made to this method, they will most likely also need to be made to the inlined
			// versions of it in the various GetCombinedHashCode overloads.

			// The IL that is generated for this method does not actually contain any casts. The casts
			// below simply instruct the compiler to perform the operations without sign extension.
			return unchecked((int)(((uint)value >> places) | ((uint)value << (32 - places))));
		}
		#endregion // RotateRight method
		#region IsDescendantOrSelf methods
		/// <summary>
		/// Determines if the <see cref="DomainRoleInfo"/> specified by <paramref name="domainRole"/> is
		/// or derives from the <see cref="DomainRoleInfo"/> with the <see cref="DomainObjectInfo.Id"/>
		/// specified by <paramref name="desiredDomainRoleId"/>.
		/// </summary>
		/// <param name="domainRole">
		/// The <see cref="DomainRoleInfo"/> to be evaluated as to whether it is or derives from the
		/// <see cref="DomainRoleInfo"/> with the <see cref="DomainObjectInfo.Id"/> specified by
		/// <paramref name="desiredDomainRoleId"/>
		/// </param>
		/// <param name="desiredDomainRoleId">
		/// The <see cref="DomainObjectInfo.Id"/> of the <see cref="DomainRoleInfo"/> against which
		/// <paramref name="domainRole"/> should be evaluated.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="domainRole"/> is <see langword="null"/>.
		/// </exception>
		public static bool IsDescendantOrSelf(DomainRoleInfo domainRole, Guid desiredDomainRoleId)
		{
			if (domainRole == null)
			{
				throw new ArgumentNullException("domainRole");
			}

			do
			{
				if (domainRole.Id == desiredDomainRoleId)
				{
					return true;
				}
				domainRole = domainRole.BaseDomainRole;
			}
			while (domainRole != null);

			return false;
		}
		/// <summary>
		/// Determines if the <see cref="DomainRoleInfo"/> specified by <paramref name="domainRole"/> is
		/// or dervies from one of the <see cref="DomainRoleInfo"/>s with the <see cref="DomainObjectInfo.Id"/>s
		/// specified by <paramref name="desiredDomainRoleIds"/>.
		/// </summary>
		/// <param name="domainRole">
		/// The <see cref="DomainRoleInfo"/> to be evaluated as to whether it is or derives from one of
		/// the <see cref="DomainRoleInfo"/>s with the <see cref="DomainObjectInfo.Id"/>s specified by
		/// <paramref name="desiredDomainRoleIds"/>
		/// </param>
		/// <param name="desiredDomainRoleIds">
		/// The <see cref="DomainObjectInfo.Id"/>s of the <see cref="DomainRoleInfo"/>s against which
		/// <paramref name="domainRole"/> should be evaluated.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="domainRole"/> is <see langword="null"/>.
		/// </exception>
		public static bool IsDescendantOrSelf(DomainRoleInfo domainRole, params Guid[] desiredDomainRoleIds)
		{
			if (domainRole == null)
			{
				throw new ArgumentNullException("domainRole");
			}
			if (desiredDomainRoleIds == null || desiredDomainRoleIds.Length <= 0)
			{
				return false;
			}

			Array.Sort<Guid>(desiredDomainRoleIds);
			do
			{
				if (Array.BinarySearch<Guid>(desiredDomainRoleIds, domainRole.Id) >= 0)
				{
					return true;
				}
				domainRole = domainRole.BaseDomainRole;
			}
			while (domainRole != null);

			return false;
		}
		#endregion // IsDescendantOrSelf methods
		#region EnumerateDomainModels method
		/// <summary>
		/// Enumerate the provided domain models, filtering on the specifies type
		/// </summary>
		/// <typeparam name="T">The type of interface to test support for on the element</typeparam>
		/// <param name="domainModels">An enumeration of domain models</param>
		/// <returns>Enumerable set with element type of <typeparamref name="T"/></returns>
		public static IEnumerable<T> EnumerateDomainModels<T>(IEnumerable<DomainModel> domainModels) where T : class
		{
			foreach (DomainModel domainModel in domainModels)
			{
				T typedModel = domainModel as T;
				if (typedModel != null)
				{
					yield return typedModel;
				}
			}
		}
		#endregion // EnumerateDomainModels method
		#region GetTypedDomainModels method
		/// <summary>
		/// Return all domain models that support the provided interface. Use instead of
		/// <see cref="EnumerateDomainModels"/> to cache the results.
		/// </summary>
		/// <typeparam name="T">The type of interface to test support for on the element</typeparam>
		/// <param name="domainModels">An enumeration of domain models</param>
		/// <returns>An array with element type <typeparamref name="T"/></returns>
		public static T[] GetTypedDomainModels<T>(IEnumerable<DomainModel> domainModels) where T : class
		{
			int count = 0;
			foreach (DomainModel domainModel in domainModels)
			{
				if (domainModel is T)
				{
					++count;
				}
			}
			if (count == 0)
			{
				return null;
			}
			T[] retVal = new T[count];
			int index = 0;
			foreach (DomainModel domainModel in domainModels)
			{
				T typedModel = domainModel as T;
				if (typedModel != null)
				{
					retVal[index] = typedModel;
					if (++index == count)
					{
						break;
					}
				}
			}
			return retVal;
		}
		#endregion // GetTypedDomainModels method
		#region GetLocalizedEnumName method
		/// <summary>
		/// Retrieve a localized name for a single value of type <typeparamref name="EnumType"/>
		/// </summary>
		/// <typeparam name="EnumType">The type of an <see cref="Enum"/></typeparam>
		/// <param name="value">A value from the <typeparamref name="EnumType"/> enumeration.</param>
		/// <returns>A <see cref="String"/> corresponding to the localized enum name</returns>
		public static string GetLocalizedEnumName<EnumType>(EnumType value) where EnumType : struct
		{
			return TypeDescriptor.GetConverter(typeof(EnumType)).ConvertToString(value);
		}
		/// <summary>
		/// Retrieve a localized names for a single value of type <paramref name="enumType"/>
		/// </summary>
		/// <param name="enumType">The type of an <see cref="Enum"/></param>
		/// <param name="value">A value from the <paramref name="enumType"/> enumeration.</param>
		/// <returns>A <see cref="String"/> corresponding to the localized enum name</returns>
		public static string GetLocalizedEnumName(Type enumType, object value)
		{
			return TypeDescriptor.GetConverter(enumType).ConvertToString(value);
		}
		#endregion // GetLocalizedEnumName method
		#region GetLocalizedEnumNames method
		/// <summary>
		/// Retrieve localized names for all values in an <paramref name="enumType"/>
		/// </summary>
		/// <param name="enumType">A <see cref="Type"/> to retrieve values for.</param>
		/// <param name="isSequential">Set to <see langword="true"/> if the enum is sequential and starts
		/// with the 0 value. If this is set, then the returned array will be sorted by the enum value.</param>
		/// <returns>A <see cref="String"/> array of enum names</returns>
		public static string[] GetLocalizedEnumNames(Type enumType, bool isSequential)
		{
			Array values = Enum.GetValues(enumType);
			TypeConverter converter = TypeDescriptor.GetConverter(enumType);
			int valueCount = values.Length;
			string[] retVal = new string[valueCount];
			for (int i = 0; i < valueCount; ++i)
			{
				object value = values.GetValue(i);
				retVal[isSequential ? (int)value : i] = converter.ConvertToString(value);
			}
			return retVal;
		}
		#endregion // GetLocalizedEnumNames method
		#region EnumerableContains method
		/// <summary>
		/// Helper method to see if an <see cref="IEnumerable{T}"/> contains
		/// a specified item.
		/// </summary>
		/// <typeparam name="T">Any type</typeparam>
		/// <param name="enumerable">The items to search</param>
		/// <param name="item">The item to search for</param>
		/// <returns>true of the <paramref name="enumerable"/> contains the <paramref name="item"/></returns>
		public static bool EnumerableContains<T>(IEnumerable<T> enumerable, T item)
		{
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			foreach (T testItem in enumerable)
			{
				if (comparer.Equals(item, testItem))
				{
					return true;
				}
			}
			return false;
		}
		#endregion // EnumerableContains
		#region EnumerableTrueForAll method
		/// <summary>
		/// Helper method to see if the items in an <see cref="IEnumerable{T}"/>
		/// are all true for the specified <paramref name="match"/>
		/// </summary>
		/// <typeparam name="T">Any type</typeparam>
		/// <param name="enumerable">The items to verify</param>
		/// <param name="match">The callback to verify with</param>
		/// <returns>true if the <paramref name="match"/> returns true for all items in <paramref name="enumerable"/></returns>
		public static bool EnumerableTrueForAll<T>(IEnumerable<T> enumerable, Predicate<T> match)
		{
			foreach (T testItem in enumerable)
			{
				if (!match(testItem))
				{
					return false;
				}
			}
			return true;
		}
		#endregion // EnumerableTrueForAll method
		#region EnumerableTrueCount method
		/// <summary>
		/// Helper method to count the number of items in an <see cref="IEnumerable{T}"/>
		/// are all true for the specified <paramref name="match"/>
		/// </summary>
		/// <typeparam name="T">Any type</typeparam>
		/// <param name="enumerable">The items to verify</param>
		/// <param name="match">The callback to verify with</param>
		/// <returns>The count of items for which <paramref name="match"/> returns true</returns>
		public static int EnumerableTrueCount<T>(IEnumerable<T> enumerable, Predicate<T> match)
		{
			int retVal = 0;
			foreach (T testItem in enumerable)
			{
				if (match(testItem))
				{
					++retVal;
				}
			}
			return retVal;
		}
		#endregion // EnumerableTrueCount method
		#region GetBaseInterfaceMethodDelegate method
		/// <summary>
		/// Return a delegate that can be used to call the implementation of a private
		/// method on a base class.
		/// </summary>
		/// <typeparam name="T">The type of a delegate. The first parameter of the delegate
		/// is assumed to be a 'this' parameter with a type of a base class (not the type of
		/// the class attempting to forward to a base class implementation). The signature
		/// of the delegate is also used to verify a matching signature on the method name.</typeparam>
		/// <param name="interfaceType">The type of the interface to call a method on.</param>
		/// <param name="methodName">The name of the method on the interface</param>
		/// <returns>A delegate that can be used to forward the current method call.</returns>
		public static T GetBaseInterfaceMethodDelegate<T>(Type interfaceType, string methodName)
			where T : class
		{
			Type delegateType = typeof(T);
			MethodInfo invokeMethod = delegateType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
			ParameterInfo[] invokeParameters = invokeMethod.GetParameters();
			Type baseType = invokeParameters[0].ParameterType;
			InterfaceMapping mapping = baseType.GetInterfaceMap(interfaceType);
			MethodInfo[] interfaceMethods = mapping.InterfaceMethods;
			MethodInfo[] targetMethods = mapping.TargetMethods;
			for (int i = 0; i < interfaceMethods.Length; ++i)
			{
				MethodInfo interfaceMethod = interfaceMethods[i];
				if (interfaceMethod.Name == methodName)
				{
					if (interfaceMethod.ReturnType == invokeMethod.ReturnType)
					{
						bool signatureMatch = true;
						ParameterInfo[] targetParameters = interfaceMethod.GetParameters();
						if (targetParameters.Length == invokeParameters.Length - 1)
						{
							for (int j = 0; j < targetParameters.Length; ++j)
							{
								if (targetParameters[j].ParameterType != invokeParameters[j + 1].ParameterType)
								{
									signatureMatch = false;
									break;
								}
							}
						}
						if (signatureMatch)
						{
							return System.Delegate.CreateDelegate(delegateType, mapping.TargetMethods[i]) as T;
						}
					}
				}
			}
			return null;
		}
		#endregion // GetBaseInterfaceMethodDelegate method
		#region IsCriticalException method
		/// <summary>
		/// Determine if a provided <see cref="Exception"/> is a critical
		/// system failure, or if can be safely ignored.
		/// </summary>
		public static bool IsCriticalException(Exception ex)
		{
			if (((ex is StackOverflowException)) || ((ex is OutOfMemoryException) || (ex is System.Threading.ThreadAbortException)))
			{
				return true;
			}
			Exception inner = ex.InnerException;
			if (inner != null)
			{
				return IsCriticalException(inner);
			}
			return false;
		}
		#endregion // IsCriticalException methods
		#region IsValidStore method
		/// <summary>
		/// Helper method to determine if a <see cref="Store"/> is current
		/// valid, or if it is currently in a torn down state.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> to validate</param>
		/// <returns><see cref="Store"/> or <see langword="null"/></returns>
		public static Store ValidateStore(Store store)
		{
			return store != null && !store.Disposed && !store.ShuttingDown ? store : null;
		}
		#endregion // IsValidStore method
		#region GetOwnerWindow method
		/// <summary>
		/// Get an appropriate dialog owner window for the specified <see cref="IServiceProvider"/>
		/// </summary>
		public static IWin32Window GetDialogOwnerWindow(IServiceProvider serviceProvider)
		{
			return new DialogOwnerWindow(serviceProvider);
		}
		private sealed class DialogOwnerWindow : IWin32Window
		{
			private readonly IntPtr myHandle;
			[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
			private static extern IntPtr GetDesktopWindow();
			public DialogOwnerWindow(IServiceProvider serviceProvider)
			{
				IVsUIShell shell = (IVsUIShell)serviceProvider.GetService(typeof(IVsUIShell));
				if (shell == null || ErrorHandler.Failed(shell.GetDialogOwnerHwnd(out myHandle)) || myHandle == IntPtr.Zero)
				{
					myHandle = GetDesktopWindow();
				}
			}
			public IntPtr Handle
			{
				get
				{
					return myHandle;
				}
			}
		}
		#endregion // GetOwnerWindow method
		#region UpperCaseFirstLetter method
		private static Regex myUpperFirstRegex;
		/// <summary>
		/// Upper case the first character of a string if it is lower case
		/// </summary>
		public static string UpperCaseFirstLetter(string value)
		{
			Regex upperFirst = myUpperFirstRegex;
			if (upperFirst == null)
			{
				System.Threading.Interlocked.CompareExchange<Regex>(
					ref myUpperFirstRegex,
					new Regex(
						@"^\p{Ll}",
						RegexOptions.Compiled),
					null);
				upperFirst = myUpperFirstRegex;
			}
			return upperFirst.Replace(
				value,
				delegate(Match m)
				{
					return m.Value.ToUpper();
				});
		}
		#endregion // UpperCaseFirstLetter method
	}
	#region LinkedNode class
	/// <summary>
	/// A simple class for creating a node in a linked list without
	/// using a containing LinkedList{} class.
	/// </summary>
	/// <remarks>The impetus for creating this class is that LinkedList{} is
	/// too hard to modify during iteration, and LinkedListNode{} requires a
	/// containing LinkedList.</remarks>
	public sealed class LinkedNode<T> : IEnumerable<T>
	{
		#region Member Variables
		private T myValue;
		private LinkedNode<T> myNext;
		private LinkedNode<T> myPrev;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a new LinkedNode. The new node must be attached to the
		/// list with the <see cref="SetNext"/> or <see cref="Detach"/> methods.
		/// </summary>
		/// <param name="value">The element of type <typeparamref name="T"/> to create a node for.</param>
		public LinkedNode(T value)
		{
			myValue = value;
		}
		#endregion // Constructor
		#region Linked List Integration
		/// <summary>
		/// Set the next element
		/// </summary>
		/// <param name="next">Next element. If next has a previous element, then the head of the next element is inserted.</param>
		/// <param name="head">Reference to head node</param>
		public void SetNext(LinkedNode<T> next, ref LinkedNode<T> head)
		{
			Debug.Assert(next != null);
			if (next.myPrev != null)
			{
				next.myPrev.SetNext(GetHead(), ref head);
				return;
			}
			if (myNext != null)
			{
				myNext.myPrev = next.GetTail();
			}
			if (myPrev == null)
			{
				head = this;
			}
			myNext = next;
			next.myPrev = this;
		}
		/// <summary>
		/// The value passed to the constructor or set directly
		/// </summary>
		public T Value
		{
			get
			{
				return myValue;
			}
			set
			{
				myValue = value;
			}
		}
		/// <summary>
		/// Get the next node
		/// </summary>
		public LinkedNode<T> Next
		{
			get
			{
				return myNext;
			}
		}
		/// <summary>
		/// Get the previous node
		/// </summary>
		public LinkedNode<T> Previous
		{
			get
			{
				return myPrev;
			}
		}
		/// <summary>
		/// Get the head element in the linked list
		/// </summary>
		public LinkedNode<T> GetHead()
		{
			LinkedNode<T> retVal = this;
			LinkedNode<T> prev;
			while (null != (prev = retVal.myPrev))
			{
				retVal = prev;
			}
			return retVal;
		}
		/// <summary>
		/// Get the tail element in the linked list
		/// </summary>
		public LinkedNode<T> GetTail()
		{
			LinkedNode<T> retVal = this;
			LinkedNode<T> next;
			while (null != (next = retVal.myNext))
			{
				retVal = next;
			}
			return retVal;
		}
		/// <summary>
		/// Detach the current node
		/// </summary>
		/// <param name="headNode"></param>
		public void Detach(ref LinkedNode<T> headNode)
		{
			if (myPrev == null)
			{
				headNode = myNext;
			}
			else
			{
				myPrev.myNext = myNext;
			}
			if (myNext != null)
			{
				myNext.myPrev = myPrev;
			}
			myNext = null;
			myPrev = null;
		}
		#endregion // Linked List Integration
		#region IEnumerable<T> Implementation
		/// <summary>
		/// Iterate all values from this node on down, including this node
		/// </summary>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			LinkedNode<T> node = this;
			while (node != null)
			{
				yield return node.Value;
				node = node.Next;
			}
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return (this as IEnumerable<T>).GetEnumerator();
		}
		#endregion // IEnumerable<T> Implementation
	}
	#endregion // LinkedNode class
	#region BitTracker struct
	/// <summary>
	/// Simple helper structure for tracking a variable number of bits.
	/// Can be used in place of a boolean array.
	/// </summary>
	public struct BitTracker
	{
		private int myLeadBits;
		private int[] myMoreBits;
		private int myBitCount;
		/// <summary>
		/// Create a bit tracker
		/// </summary>
		/// <param name="bitCount">The number of bits to tracker</param>
		public BitTracker(int bitCount)
		{
			myLeadBits = 0;
			int requiresMoreInts = (bitCount - 1) / 32;
			myMoreBits = requiresMoreInts > 0 ? new int[requiresMoreInts] : null;
			myBitCount = bitCount; // Keep so that we can resize and reset
		}
		/// <summary>
		/// Extend the the number of bits available to the bit tracker.
		/// </summary>
		public void Resize(int bitCount)
		{
			int oldBitCount = myBitCount;
			myBitCount = bitCount;
			if (bitCount > oldBitCount) // Don't bother to shrink capacity or reset memory for fewer bits
			{
				int requiresMoreInts = (bitCount - 1) / 32;
				if (requiresMoreInts > 0)
				{
					int[] currentBits = myMoreBits;
					if (currentBits == null)
					{
						myMoreBits = new int[requiresMoreInts];
					}
					else if (currentBits.Length < requiresMoreInts)
					{
						// Zero out the remainder of the current and following blocks
						if (oldBitCount == 0)
						{
							myLeadBits = 0;
						}
						else if (oldBitCount < 32)
						{
							myLeadBits = MaskBits(myLeadBits, oldBitCount);
						}
						int oldOverflowBlock = (oldBitCount - 1) / 32 - 1;
						int oldBitIndex = oldBitCount % 32;
						if (oldBitIndex != 0)
						{
							currentBits[oldOverflowBlock] = MaskBits(currentBits[oldOverflowBlock], oldBitIndex);
						}
						for (int i = oldOverflowBlock + 1; i < currentBits.Length; ++i)
						{
							currentBits[i] = 0;
						}
						Array.Resize<int>(ref myMoreBits, requiresMoreInts);
					}
					else
					{
						// Zero out remaining bits
						int oldOverflowBlock = -1;
						if (oldBitCount > 32)
						{
							oldOverflowBlock = (oldBitCount - 1) / 32 - 1;
							int oldBitIndex = oldBitCount % 32;
							if (oldBitIndex != 0)
							{
								currentBits[oldOverflowBlock] = MaskBits(currentBits[oldOverflowBlock], oldBitIndex);
							}
						}
						else
						{
							myLeadBits = MaskBits(myLeadBits, oldBitCount);
						}
						for (int i = oldOverflowBlock + 1; i < requiresMoreInts; ++i)
						{
							currentBits[i] = 0;
						}
					}
				}
				else if (oldBitCount == 0)
				{
					myLeadBits = 0;
				}
				else if (oldBitCount < 32)
				{
					myLeadBits = MaskBits(myLeadBits, oldBitCount);
				}
			}
		}
		/// <summary>
		/// Get the first <paramref name="bitCount"/> bits from the
		/// <paramref name="bits"/> value.
		/// </summary>
		private static int MaskBits(int bits, int bitCount)
		{
			if (bitCount == 32)
			{
				return bits;
			}
			else if (bitCount == 0)
			{
				return 0;
			}
			int killBits = 0;
			int killBit = 1 << bitCount;
			for (int i = bitCount; i < 32; ++i)
			{
				killBits |= killBit;
				killBit <<= 1;
			}
			return bits & ~killBits;
		}
		/// <summary>
		/// Reset all bits to false without changing the bit count
		/// </summary>
		public void Reset()
		{
			myLeadBits = 0;
			int[] otherBits = myMoreBits;
			if (otherBits != null)
			{
				int blockCount = (myBitCount - 1) / 32 - 1;
				for (int i = 0; i < blockCount; ++i)
				{
					otherBits[i] = 0;
				}
			}
		}
		/// <summary>
		/// Reset all values to false with a new number of bits.
		/// </summary>
		/// <param name="bitCount">The new number of bits to support.</param>
		public void Reset(int bitCount)
		{
			int oldBitCount = myBitCount;
			myBitCount = bitCount;
			if (bitCount > 32)
			{
				if (bitCount > oldBitCount)
				{
					myLeadBits = 0;
					int requiresMoreInts = (bitCount - 1) / 32;
					int[] currentBits = myMoreBits;
					if (currentBits == null || currentBits.Length < requiresMoreInts)
					{
						myMoreBits = new int[requiresMoreInts];
					}
					else
					{
						for (int i = 0; i < requiresMoreInts; ++i)
						{
							currentBits[i] = 0;
						}
					}
				}
				else
				{
					Reset();
				}
			}
			else
			{
				myLeadBits = 0;
			}
		}
		/// <summary>
		/// Get the current number of supported bits.
		/// </summary>
		public int Count
		{
			get
			{
				return myBitCount;
			}
		}
		/// <summary>
		/// Get a bit value
		/// </summary>
		/// <param name="index">The index of the bit to set. Must be less than the bitCount specified in the constructor.</param>
		public bool this[int index]
		{
			get
			{
				if (index < 0 || index >= myBitCount)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return index < 32 ? (0 != (myLeadBits & (1 << index))) : (0 != (myMoreBits[index / 32 - 1] & (1 << (index % 32))));
			}
			set
			{
				if (index < 0 || index >= myBitCount)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index < 32)
				{
					index = 1 << index;
					if (value)
					{
						myLeadBits |= index;
					}
					else
					{
						myLeadBits &= ~index;
					}
				}
				else
				{
					int moreBitsIndex = index / 32 - 1;
					index = 1 << (index % 32);
					if (value)
					{
						myMoreBits[moreBitsIndex] |= index;
					}
					else
					{
						myMoreBits[moreBitsIndex] &= ~index;
					}
				}
			}
		}
	}
	#endregion // BitTracker struct
}
