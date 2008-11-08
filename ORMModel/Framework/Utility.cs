#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © Matthew Curland. All rights reserved.                        *
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
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using System.Reflection;

namespace Neumont.Tools.Modeling
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
		#region EnumerateDomainModels methods
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
		#endregion // EnumerateDomainModels methods
		#region GetLocalizedEnumName method
		/// <summary>
		/// Retrieve localized names for all values in an <paramref name="enumType"/>
		/// </summary>
		/// <typeparam name="EnumType">The type of an <see cref="Enum"/></typeparam>
		/// <param name="value">A value from the <typeparamref name="EnumType"/> enumeration.</param>
		/// <returns>A <see cref="String"/> corresponding to the localized enum name</returns>
		public static string GetLocalizedEnumName<EnumType>(EnumType value) where EnumType : struct
		{
			return TypeDescriptor.GetConverter(typeof(EnumType)).ConvertToString(value);
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
	}
}
