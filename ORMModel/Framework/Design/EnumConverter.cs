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
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Neumont.Tools.Modeling.Design
{
	/// <summary>
	/// Supports localized display names for <see cref="Enum"/>s, and ensures that the current value is always shown
	/// in editor dropdowns if <see cref="Enum.IsDefined"/> returns <see langword="true"/> for it.
	/// </summary>
	/// <typeparam name="TEnum">
	/// The type of the <see cref="Enum"/> that this <see cref="EnumConverter{TEnum,TResourceManagerSource}"/> is for.
	/// </typeparam>
	/// <typeparam name="TResourceManagerSource">
	/// The type from which the <see cref="ResourceManager"/> should be obtained.
	/// </typeparam>
	/// <remarks>
	/// <see cref="ResourceAccessor{TResourceManagerSource}"/> is used to obtain the <see cref="ResourceManager"/> for <typeparamref name="TResourceManagerSource"/>.
	/// </remarks>
	[CLSCompliant(false)] // IConvertible is not CLS-compliant
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class EnumConverter<TEnum, TResourceManagerSource> : EnumConverter
		where TEnum : struct, IFormattable, IComparable, IConvertible // This is as close as we can get to 'System.Enum'
	{
		#region EnumValueInfo struct
		[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
		private struct EnumValueInfo : IEquatable<EnumValueInfo>
		{
			private static readonly string ResourceNamePrefix = EnumType.Name + Type.Delimiter;
			public EnumValueInfo(FieldInfo field)
			{
				this.InvariantName = (this.Value = (TEnum)field.GetValue(null)).ToString();
				this.ResourceName = ResourceNamePrefix + field.Name;
				object[] browsableArray = field.GetCustomAttributes(typeof(BrowsableAttribute), false);
				this.Browsable = (browsableArray.Length <= 0) || ((BrowsableAttribute)browsableArray[0]).Browsable;
			}
			public readonly TEnum Value;
			public readonly string InvariantName;
			public readonly string ResourceName;
			public readonly bool Browsable;

			public override int GetHashCode()
			{
				return this.Value.GetHashCode();
			}
			public override bool Equals(object obj)
			{
				return obj is EnumValueInfo && this.Value.Equals(((EnumValueInfo)obj).Value);
			}
			public bool Equals(EnumValueInfo other)
			{
				return this.Value.Equals(other.Value);
			}
			public static bool operator ==(EnumValueInfo left, EnumValueInfo right)
			{
				return left.Value.Equals(right.Value);
			}
			public static bool operator !=(EnumValueInfo left, EnumValueInfo right)
			{
				return !left.Value.Equals(right.Value);
			}
		}
		#endregion // EnumValueInfo struct

		#region LocalizedNameDictionary class
		private sealed class LocalizedNameDictionary
		{
			private readonly Dictionary<string, TEnum> ValuesByName = new Dictionary<string, TEnum>(DefinedValuesCount, StringComparer.Ordinal);
			private readonly Dictionary<TEnum, string> NamesByValue = new Dictionary<TEnum, string>(DefinedValuesCount);

			public bool TryGetName(TEnum value, out string name)
			{
				return NamesByValue.TryGetValue(value, out name);
			}
			public bool TryGetValue(string name, out TEnum value)
			{
				return ValuesByName.TryGetValue(name, out value);
			}
			public void Add(string name, TEnum value)
			{
				ValuesByName.Add(name, value);
				NamesByValue.Add(value, name);
			}
		}
		#endregion // LocalizedNameDictionary class

		#region Static helper methods
		static EnumConverter()
		{
			Type enumType = EnumType = typeof(TEnum);
			if (!enumType.IsEnum)
			{
				// For whatever reason, C# doesn't let us use System.Enum as a class-type constraint,
				// and ECMA-334 §25.1.5 instead demonstrates checking Type.IsEnum in a static constructor.
				throw new InvalidEnumArgumentException();
			}

			IsFlags = enumType.IsDefined(typeof(FlagsAttribute), false);

			LocalizedNamesByCultureName = new Dictionary<string, LocalizedNameDictionary>(StringComparer.Ordinal);

			FieldInfo[] fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding | BindingFlags.DeclaredOnly);
			int definedValuesCount = DefinedValuesCount = fields.Length;
			Dictionary<TEnum, EnumValueInfo> valueInfoDictionary = ValueInfoDictionary = new Dictionary<TEnum, EnumValueInfo>(definedValuesCount);
			Dictionary<string, TEnum> invariantNameDictionary = InvariantNameDictionary = new Dictionary<string, TEnum>(definedValuesCount, StringComparer.Ordinal);
			for (int i = 0; i < fields.Length; i++)
			{
				EnumValueInfo valueInfo = new EnumValueInfo(fields[i]);
				TEnum value = valueInfo.Value;
				valueInfoDictionary[value] = valueInfo;
				invariantNameDictionary[valueInfo.InvariantName] = value;
			}
		}

		private static readonly object LockObject = new object();
		private static new readonly Type EnumType;
		private static readonly bool IsFlags;
		private static readonly int DefinedValuesCount;
		private static readonly Dictionary<TEnum, EnumValueInfo> ValueInfoDictionary;
		private static readonly Dictionary<string, TEnum> InvariantNameDictionary;
		private static readonly Dictionary<string, LocalizedNameDictionary> LocalizedNamesByCultureName;

		private const string FlagsNameSeparator = ", ";
		private static readonly string[] FlagsNameSeparatorArray = new string[] { FlagsNameSeparator };

		private static EnumValueInfo? GetValueInfo(TEnum value)
		{
			EnumValueInfo valueInfo;
			return (ValueInfoDictionary.TryGetValue(value, out valueInfo)) ? (EnumValueInfo?)valueInfo : null;
		}
		private static EnumValueInfo? GetValueInfo(string invariantName)
		{
			TEnum? value = GetValue(invariantName);
			return (value.HasValue) ? GetValueInfo(value.Value) : null;
		}
		private static TEnum? GetValue(string invariantName)
		{
			TEnum value;
			return (InvariantNameDictionary.TryGetValue(invariantName, out value)) ? (TEnum?)value : null;
		}

		private static string GetLocalizedNameFromValue(TEnum value, CultureInfo culture)
		{
			EnumValueInfo? nullableValueInfo = GetValueInfo(value);
			if (nullableValueInfo.HasValue)
			{
				return GetLocalizedNameFromValueInternal(nullableValueInfo.Value, culture);
			}
			else
			{
				string invariantName = value.ToString();
				if (IsFlags)
				{
					// We let Enum do the hard work getting the correct Invariant names, and then we replace them with the localized versions				
					string[] invariantNames = invariantName.Split(FlagsNameSeparatorArray, StringSplitOptions.RemoveEmptyEntries);
					if (invariantNames.Length > 1)
					{
						System.Text.StringBuilder sb = new System.Text.StringBuilder(invariantName.Length * 3);
						int invariantNamesLengthMinusOne = invariantNames.Length - 1;
						for (int i = 0; i < invariantNames.Length; i++)
						{
							string currentInvariantName = invariantNames[i];
							nullableValueInfo = GetValueInfo(currentInvariantName);
							sb.Append(nullableValueInfo.HasValue ? GetLocalizedNameFromValueInternal(nullableValueInfo.Value, culture) : currentInvariantName);
							if (i < invariantNamesLengthMinusOne)
							{
								sb.Append(FlagsNameSeparator);
							}
						}
						return sb.ToString();
					}
				}
				return invariantName;
			}
		}
		private static string GetLocalizedNameFromValueInternal(EnumValueInfo valueInfo, CultureInfo culture)
		{
			TEnum value = valueInfo.Value;
			string invariantName = valueInfo.InvariantName;
			Dictionary<string, LocalizedNameDictionary> localizedNamesByCultureName = LocalizedNamesByCultureName;

			CultureInfo originalCulture = CultureInfo.CurrentCulture;

			// Yes, we want reference equality here, not value equality. We only want to do this if we were passed
			// the actual CurrentCulture object, not just a culture that happens to be equal to it.
			if (culture == originalCulture)
			{
				// Normally, TypeConverters do actual conversion, not resource lookups, so they are usually passed
				// CurrentCulture, rather than CurrentUICulture (which will often be a neutral culture that throws
				// if used for anything other than resource lookup). Since we ARE actually doing resource lookups,
				// it is reasonable to try the CurrentUICulture first instead of the CurrentCulture, if that is
				// indeed what was passed to us.
				culture = CultureInfo.CurrentUICulture;

				// However, if the CurrentCulture and the CurrentUICulture are equal, we can just null out
				// originalCulture so that we don't try to do lookups against that culture twice.
				if (culture.Equals(originalCulture))
				{
					originalCulture = null;
				}
			}
			else
			{
				originalCulture = null;
			}

		L_ProcessCulture:
			string cultureName = culture.Name;
			
			LocalizedNameDictionary localizedNameDictionary;
			bool dictionaryAlreadyExists = localizedNamesByCultureName.TryGetValue(cultureName, out localizedNameDictionary);
			string localizedName;
			if (dictionaryAlreadyExists && localizedNameDictionary.TryGetName(value, out localizedName))
			{
				return localizedName;
			}
			localizedName = ResourceAccessor<TResourceManagerSource>.ResourceManager.GetString(valueInfo.ResourceName, culture);
			bool localizedNameIsNullOrEmpty = string.IsNullOrEmpty(localizedName);
			if (localizedNameIsNullOrEmpty || localizedName == invariantName)
			{
				if (localizedNameIsNullOrEmpty && originalCulture != null)
				{
					// We didn't find a resource (or we found an empty one, see next comment) for
					// CurrentUICulture, so go back and try the culture that we were originally passed.
					culture = originalCulture;
					originalCulture = null;
					goto L_ProcessCulture;
				}
				
				// Don't bother saving the Invariant name in the LocalizedNamesDictionary,
				// since we always check the InvariantNameDictionary first.
				// Also, we don't allow empty strings as the name, so if that is what the
				// resource contained, we just ignore it.
				return invariantName;
			}
			else
			{
				if (!dictionaryAlreadyExists)
				{
					// Make sure we don't blow away any other dictionaries for this culture that have been
					// created in the mean time.
					lock (LockObject)
					{
						if (!localizedNamesByCultureName.TryGetValue(cultureName, out localizedNameDictionary))
						{
							localizedNamesByCultureName[cultureName] = localizedNameDictionary = new LocalizedNameDictionary();
						}
					}
				}
				localizedNameDictionary.Add(localizedName, value);
				return localizedName;
			}
		}
		private static TEnum? GetValueFromLocalizedName(string localizedName, CultureInfo culture)
		{
			TEnum value;
			if (InvariantNameDictionary.TryGetValue(localizedName, out value))
			{
				// This could potentially return the wrong value if the Invariant name of one value is exactly the same as
				// the localized name of a different value, but we probably don't need to worry about that actually happening.
				return value;
			}
			else
			{
				// See GetLocalizedNameFromValueInternal for explanations
				Dictionary<string, LocalizedNameDictionary> localizedNamesByCultureName = LocalizedNamesByCultureName;
				CultureInfo originalCulture = null;
				if (culture == CultureInfo.CurrentCulture)
				{
					originalCulture = culture;
					culture = CultureInfo.CurrentUICulture;
				}

			L_ProcessCulture:
				LocalizedNameDictionary localizedNameDictionary;
				if (localizedNamesByCultureName.TryGetValue(culture.Name, out localizedNameDictionary))
				{
					if (localizedNameDictionary.TryGetValue(localizedName, out value))
					{
						return value;
					}
				}
				if (originalCulture != null)
				{
					culture = originalCulture;
					originalCulture = null;
					goto L_ProcessCulture;
				}
			}
			return null;
		}
		#endregion // Static helper methods

		/// <summary>
		/// Initializes a new instance of <see cref="EnumConverter{TEnum,TResourceManagerSource}"/>.
		/// </summary>
		public EnumConverter()
			: base(EnumType)
		{
		}

		/// <summary>See <see cref="EnumConverter.ConvertFrom"/>.</summary>
		public sealed override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string stringValue = value as string;
			if ((object)stringValue != null && culture != null && !culture.Equals(CultureInfo.InvariantCulture))
			{
				TEnum? nullableValue = GetValueFromLocalizedName(stringValue, culture);
				if (nullableValue.HasValue)
				{
					return nullableValue.Value;
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>See <see cref="EnumConverter.ConvertTo"/>.</summary>
		public sealed override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && culture != null && !culture.Equals(CultureInfo.InvariantCulture) && value is TEnum)
			{
				return GetLocalizedNameFromValue((TEnum)value, culture);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		#region GetStandardValues method
		/// <summary>
		/// Obtains the <see cref="TypeConverter.StandardValuesCollection"/> from <see cref="EnumConverter.GetStandardValues"/>,
		/// and adds the current value to the beginning of the <see cref="TypeConverter.StandardValuesCollection"/>, if necessary.
		/// </summary>
		/// <remarks>
		/// The current value will be added to the beginning of the <see cref="TypeConverter.StandardValuesCollection"/> returned
		/// by <see cref="EnumConverter.GetStandardValues"/> if the following conditions are met:
		/// <list type="bullet">
		/// <item><description><paramref name="context"/> is not <see langword="null"/></description></item>
		/// <item><description><see cref="ITypeDescriptorContext.Instance"/> is not <see langword="null"/>.</description></item>
		/// <item><description><see cref="ITypeDescriptorContext.PropertyDescriptor"/> is not <see langword="null"/>.</description></item>
		/// <item><description><see cref="PropertyDescriptor.GetValue"/> returns a value of type <typeparamref name="TEnum"/>.</description></item>
		/// <item><description><see cref="Enum.IsDefined"/> returns <see langword="true"/> for that value.</description></item>
		/// <item><description>The <see cref="TypeConverter.StandardValuesCollection"/> returned by <see cref="EnumConverter.GetStandardValues"/> does not already contain that value.</description></item>
		/// </list>
		/// </remarks>
		/// <seealso cref="TypeConverter.GetStandardValues(ITypeDescriptorContext)"/>
		public sealed override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			StandardValuesCollection standardValuesCollection = base.GetStandardValues(context);
			object component;
			PropertyDescriptor propertyDescriptor;
			if (context != null && (component = context.Instance) != null && (propertyDescriptor = context.PropertyDescriptor) != null)
			{
				TEnum? valueNullable = propertyDescriptor.GetValue(component) as TEnum?;
				TEnum value;
				EnumValueInfo? valueInfoNullable;
				if (valueNullable.HasValue && (valueInfoNullable = GetValueInfo(value = valueNullable.Value)).HasValue)
				{
					// Since we found an EnumValueInfo for the value, we know that Enum.IsDefined would return true
					if (!valueInfoNullable.Value.Browsable)
					{
						// Since the value is not browsable, it will not be in the StandardValuesCollection we retrieved from EnumConverter,
						// so we have to add it.
						TEnum[] newStandardValues = new TEnum[standardValuesCollection.Count + 1];
						newStandardValues[0] = value;
						standardValuesCollection.CopyTo(newStandardValues, 1);
						return new StandardValuesCollection(newStandardValues);
					}
				}
			}
			return standardValuesCollection;
		}
		#endregion // GetStandardValues method
	}
}
