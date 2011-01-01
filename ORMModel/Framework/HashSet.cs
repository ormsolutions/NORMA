#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace ORMSolutions.ORMArchitect.Framework
{
	#region KeyProvider

	#region IKeyProvider interface
	/// <summary>
	/// Supports getting the key of type <typeparamref name="TValue"/> for a value
	/// of type <typeparamref name="TValue"/>.
	/// </summary>
	/// <typeparam name="TKey">
	/// The type of the key.
	/// </typeparam>
	/// <typeparam name="TValue">
	/// The type of the value.
	/// </typeparam>
	public interface IKeyProvider<TKey, TValue>
	{
		/// <summary>
		/// Retrieves the key of type <typeparamref name="TKey"/> for the instance of
		/// <typeparamref name="TValue"/> specified by <paramref name="value"/>.
		/// </summary>
		/// <param name="value">
		/// The value of type <typeparamref name="TValue"/> for which the key should be retrieved.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="value"/> is <see langword="null"/>.
		/// </exception>
		TKey GetKey(TValue value);
	}
	#endregion // IKeyProvider interface

	#region IKeyed interface
	/// <summary>
	/// Supports getting the key of type <typeparamref name="TKey"/> for instances
	/// of types that implement this interface.
	/// </summary>
	/// <typeparam name="TKey">
	/// The type of the key.
	/// </typeparam>
	public interface IKeyed<TKey>
	{
		/// <summary>
		/// Retrieves the key of type <typeparamref name="TKey"/> for this instance.
		/// </summary>
		TKey GetKey();
	}
	#endregion // IKeyed interface

	#region KeyProvisioning delegate
	/// <summary>
	/// Retrieves the key of type <typeparamref name="TKey"/> for the instance of
	/// <typeparamref name="TValue"/> specified by <paramref name="value"/>.
	/// </summary>
	/// <typeparam name="TKey">
	/// The type of the key.
	/// </typeparam>
	/// <typeparam name="TValue">
	/// The type of the value.
	/// </typeparam>
	/// <param name="value">
	/// The value of type <typeparamref name="TValue"/> for which the key should be retrieved.
	/// </param>
	/// <exception cref="ArgumentNullException">
	/// <paramref name="value"/> is <see langword="null"/>.
	/// </exception>
	[Serializable]
	public delegate TKey KeyProvisioning<TKey, TValue>(TValue value);
	#endregion // KeyProvisioning delegate

	#region KeyProvider class
	/// <summary>
	/// Supports <see cref="KeyProvider{TKey,TValue}"/>.
	/// </summary>
	public static class KeyProvider
	{
		/// <summary>
		/// Creates an <see cref="IKeyProvider{TKey,TValue}"/> that uses the
		/// <see cref="KeyProvisioning{TKey,TValue}"/> specified by <paramref name="provisioning"/>.
		/// </summary>
		/// <typeparam name="TKey">
		/// The type of the key.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// The type of the value.
		/// </typeparam>
		/// <param name="provisioning">
		/// The <see cref="KeyProvisioning{TKey,TValue}"/> from which a <see cref="IKeyProvider{TKey,TValue}"/>
		/// should be created.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="provisioning"/> is <see langword="null"/>.
		/// </exception>
		public static IKeyProvider<TKey, TValue> CreateProvider<TKey, TValue>(KeyProvisioning<TKey, TValue> provisioning)
		{
			if ((object)provisioning == null)
			{
				throw new ArgumentNullException("provisioning");
			}
			return new DelegateKeyProvider<TKey, TValue>(provisioning);
		}

		// This method should only be called from KeyProvider<TKey, TValue>.Default
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static IKeyProvider<TKey, TValue> CreateProvider<TKey, TValue>()
		{
			// UNDONE: A KeyProviderAttribute that specifies the IKeyProvider implementation to use would also probably be a good thing to support.

			Type valueType = typeof(TValue);
			Type keyedType = typeof(IKeyed<TKey>);
			Type keyType = typeof(TKey);
			Type underlyingValueType;

			// First, try using a provider that defers to TValue's IKeyed<TKey> implementation.
			if (keyedType.IsAssignableFrom(valueType))
			{
				return (IKeyProvider<TKey, TValue>)Activator.CreateInstance(typeof(GenericKeyProvider<,>).MakeGenericType(keyType, valueType));
			}
			// Second, if TValue is Nullable...
			else if ((underlyingValueType = Nullable.GetUnderlyingType(valueType)) != null)
			{
				// ... try using a provider that defers to the underlying type's IKeyed<Key> implementation.
				if (keyedType.IsAssignableFrom(underlyingValueType))
				{
					return (IKeyProvider<TKey, TValue>)Activator.CreateInstance(typeof(NullableGenericKeyProvider<,>).MakeGenericType(keyType, underlyingValueType));
				}
				// If the underlying type is the same as the key type, use a provider that just returns the value.
				else if (keyType == underlyingValueType)
				{
					return (IKeyProvider<TKey, TValue>)Activator.CreateInstance(typeof(NullableSelfKeyProvider<>).MakeGenericType(keyType));
				}
			}

			// Third, if TKey and TValue are the same type, or if TKey is object, use a provider that just returns the value.
			if (keyType == valueType || keyType == typeof(object))
			{
				return (IKeyProvider<TKey, TValue>)Activator.CreateInstance(typeof(SelfKeyProvider<TValue>));
			}
			// Fourth, if TKey is Int32 or ValueType, use a provider that returns the HashCode of the value.
			else if (keyType == typeof(int) || keyType == typeof(ValueType))
			{
				return (IKeyProvider<TKey, TValue>)Activator.CreateInstance(typeof(HashCodeKeyProvider<TValue>));
			}

			// If we were unable to find anything to return, throw
			// UNDONE: Localize this
			throw new InvalidOperationException("Attempted to create an IKeyProvider for an unsupported TKey/TValue pair.");
		}

		#region Private KeyProvider implementation classes
		#region GenericKeyProvider class
		/// <summary>
		/// <see cref="IKeyProvider{TKey,TValue}"/> implementation that defers to the <see cref="IKeyed{TKey}"/>
		/// implementation of <typeparamref name="TValue"/>.
		/// </summary>
		[Serializable]
		private sealed class GenericKeyProvider<TKey, TValue> : KeyProvider<TKey, TValue>
			where TValue : IKeyed<TKey>
		{
			public GenericKeyProvider()
				: base()
			{
			}
			public sealed override TKey GetKey(TValue value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				return value.GetKey();
			}
		}
		#endregion // GenericKeyProvider class
		#region NullableGenericKeyProvider class
		/// <summary>
		/// <see cref="IKeyProvider{TKey,TValue}"/> implementation that defers to the <see cref="IKeyed{TKey}"/>
		/// implementation of <typeparamref name="TValue"/>.
		/// </summary>
		[Serializable]
		private sealed class NullableGenericKeyProvider<TKey, TValue> : KeyProvider<TKey, TValue?>
			where TValue : struct, IKeyed<TKey>
		{
			public NullableGenericKeyProvider()
				: base()
			{
			}
			public sealed override TKey GetKey(TValue? value)
			{
				if (!value.HasValue)
				{
					throw new ArgumentNullException("value");
				}
				return value.Value.GetKey();
			}
		}
		#endregion // NullableGenericKeyProvider class
		#region SelfKeyProvider class
		/// <summary>
		/// <see cref="IKeyProvider{TKeyAndValue,TKeyAndValue}"/> and <see cref="IKeyProvider{Object,TKeyAndValue}"/>
		/// implementation that returns the value passed in.
		/// </summary>
		[Serializable]
		private sealed class SelfKeyProvider<TKeyAndValue> : KeyProvider<TKeyAndValue, TKeyAndValue>, IKeyProvider<object, TKeyAndValue>
		{
			public SelfKeyProvider()
				: base()
			{
			}
			public sealed override TKeyAndValue GetKey(TKeyAndValue value)
			{
				return value;
			}
			object IKeyProvider<object, TKeyAndValue>.GetKey(TKeyAndValue value)
			{
				return value;
			}
		}
		#endregion // SelfKeyProvider class
		#region NullableSelfKeyProvider class
		/// <summary>
		/// <see cref="IKeyProvider{TKeyAndValue,TKeyAndValue}"/> implementation that returns the value passed in.
		/// </summary>
		[Serializable]
		private sealed class NullableSelfKeyProvider<TKeyAndValue> : KeyProvider<TKeyAndValue, TKeyAndValue?>
			where TKeyAndValue : struct
		{
			public NullableSelfKeyProvider()
				: base()
			{
			}
			public sealed override TKeyAndValue GetKey(TKeyAndValue? value)
			{
				if (!value.HasValue)
				{
					throw new ArgumentNullException("value");
				}
				return value.Value;
			}
		}
		#endregion // NullableSelfKeyProvider class
		#region HashCodeKeyProvider class
		/// <summary>
		/// <see cref="IKeyProvider{Int32,TValue}"/> and <see cref="IKeyProvider{ValueType,TValue}"/> implementation
		/// that returns the result of calling <see cref="object.GetHashCode"/> on the value passed in.
		/// </summary>
		[Serializable]
		private sealed class HashCodeKeyProvider<TValue> : KeyProvider<int, TValue>, IKeyProvider<ValueType, TValue>
		{
			public HashCodeKeyProvider()
				: base()
			{
			}
			public sealed override int GetKey(TValue value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				return value.GetHashCode();
			}
			ValueType IKeyProvider<ValueType, TValue>.GetKey(TValue value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				return value.GetHashCode();
			}
		}
		#endregion // HashCodeKeyProvider class
		#region DelegateKeyProvider class
		/// <summary>
		/// <see cref="IKeyProvider{TKey,TValue}"/> implementation that defers to the <see cref="KeyProvisioning{TKey,TValue}"/>
		/// provided when it is initialized.
		/// </summary>
		[Serializable]
		private sealed class DelegateKeyProvider<TKey, TValue> : KeyProvider<TKey, TValue>
		{
			public DelegateKeyProvider(KeyProvisioning<TKey, TValue> provisioning)
			{
				// The CreateProvider method already checked provisioning for null
				this._provisioning = provisioning;
			}
			private readonly KeyProvisioning<TKey, TValue> _provisioning;
			public sealed override TKey GetKey(TValue value)
			{
				return this._provisioning(value);
			}
		}
		#endregion // DelegateKeyProvider class
		#endregion // Private KeyProvider implementation classes
	}
	/// <summary>
	/// Provides a base class for implementations of the <see cref="IKeyProvider{TKey,TValue}"/> interface.
	/// </summary>
	/// <typeparam name="TKey">
	/// The type of the key.
	/// </typeparam>
	/// <typeparam name="TValue">
	/// The type of the value.
	/// </typeparam>
	[Serializable]
	public abstract class KeyProvider<TKey, TValue> : IKeyProvider<TKey, TValue>
	{
		/// <summary>
		/// Retrieves the key of type <typeparamref name="TKey"/> for the instance of
		/// <typeparamref name="TValue"/> specified by <paramref name="value"/>.
		/// </summary>
		/// <param name="value">
		/// The value of type <typeparamref name="TValue"/> for which the key should be retrieved.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="value"/> is <see langword="null"/>.
		/// </exception>
		public abstract TKey GetKey(TValue value);

		private static IKeyProvider<TKey, TValue> defaultProvider;
		// UNDONE: Document the default IKeyProvider implementations that are tried.
		/// <summary>
		/// Returns a default implementation of <see cref="IKeyProvider{TKey,TValue}"/> for the
		/// types specified by <typeparamref name="TKey"/> and <typeparamref name="TValue"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// An <see cref="IKeyProvider{TKey,TValue}"/> could not be created for the types specified by
		/// <typeparamref name="TKey"/> and <typeparamref name="TValue"/>.
		/// </exception>
		public static IKeyProvider<TKey, TValue> Default
		{
			get
			{
				return defaultProvider ?? (defaultProvider = KeyProvider.CreateProvider<TKey, TValue>());
			}
		}
	}
	#endregion // KeyProvider class

	#endregion // KeyProvider

	#region HashSet class

	#region HashSet base class
	/// <summary>
	/// This class supports <see cref="HashSet{TKey,TValue}"/> and is not intended to be used directly from your code.
	/// </summary>
	[Serializable]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class HashSet
	{
		// This class exists to prevent a separate copy of the precomputed primes array from being stored for each
		// closed constructed type instantiation of HashSet<TKey, TValue>.

		/*protected AND*/ internal HashSet()
			: base()
		{
		}

		#region GetNewCapacity method

		#region PrecomputedPrimes array
		private const int MaxPrecomputedPrime = 7199369;
		private static readonly int[] PrecomputedPrimes = new int[] { 3, 7, 11, 17, 23, 29, 37, 47,
			59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919, 1103, 1327, 1597,
			1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591, 17519, 21023,
			25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437, 187751,
			225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
			1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, MaxPrecomputedPrime };
		#endregion // PrecomputedPrimes array

		private byte _primesIndex;

		/*protected AND*/ internal int GetNewCapacity(int minCapacity, bool isInitialCapacity)
		{
			int newPrime;

			// First try the precomputed primes
			if (minCapacity <= MaxPrecomputedPrime)
			{
				int[] precomputedPrimes = PrecomputedPrimes;
				for (byte i = this._primesIndex; i < precomputedPrimes.Length; i++)
				{
					newPrime = precomputedPrimes[i];
					if (newPrime >= minCapacity)
					{
						this._primesIndex = i;
						return newPrime;
					}
				}
			}

			// If we have run out of precomputed primes, we need to calculate a new one
			// If this isn't the initial capacity, try for 150% of the requested capacity, so that we won't have to resize for a while
			for (newPrime = (isInitialCapacity ? minCapacity : (int)(minCapacity * 1.5)) | 1; newPrime < int.MaxValue; newPrime += 2)
			{
				int maxPossibleDivisor = (int)Math.Sqrt(newPrime);
				for (int possibleDivisor = 3; possibleDivisor <= maxPossibleDivisor; possibleDivisor += 2)
				{
					if (newPrime % possibleDivisor == 0)
					{
						goto L_ContinueOuterLoop;
					}
				}
				return newPrime;

			L_ContinueOuterLoop:
				continue;
			}

			// If we haven't found a prime yet, just return the minimum capacity
			return minCapacity;
		}
		#endregion // GetNewCapacity method

		#region DebugView class
		[Serializable]
		[EditorBrowsable(EditorBrowsableState.Never)]
		/*protected AND*/ internal sealed class DebugView<TKey, TValue>
		{
			private readonly HashSet<TKey, TValue> _hashSet;
			public DebugView(HashSet<TKey, TValue> hashSet)
			{
				this._hashSet = hashSet;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public KeyValuePair<TKey, TValue>[] Items
			{
				get
				{
					HashSet<TKey, TValue> hashSet = this._hashSet;
					KeyValuePair<TKey, TValue>[] items = new KeyValuePair<TKey, TValue>[hashSet.Count];
					hashSet.CopyTo(items, 0);
					return items;
				}
			}
		}
		#endregion // DebugView class

		#region KeyCollectionDebugView class
		[Serializable]
		[EditorBrowsable(EditorBrowsableState.Never)]
		/*protected AND*/ internal sealed class KeyCollectionDebugView<TKey, TValue>
		{
			private readonly HashSet<TKey, TValue>.KeyCollection _keyCollection;
			public KeyCollectionDebugView(HashSet<TKey, TValue>.KeyCollection keyCollection)
			{
				this._keyCollection = keyCollection;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public TKey[] Items
			{
				get
				{
					return this._keyCollection.ToArray();
				}
			}
		}
		#endregion // KeyCollectionDebugView class
	}
	#endregion // HashSet base class

	/// <summary>
	/// Represents a <c>set</c> of <typeparamref name="TValue"/> instances.
	/// </summary>
	/// <typeparam name="TKey">
	/// The type of the keys in the <see cref="HashSet{TKey,TValue}"/>.
	/// </typeparam>
	/// <typeparam name="TValue">
	/// The type of the values in the <see cref="HashSet{TKey,TValue}"/>.
	/// </typeparam>
	/// <remarks>
	/// <para>
	/// The <see cref="HashSet{TKey,TValue}"/> generic class provides a mapping from a <c>bag</c> of <typeparamref name="TKey"/>
	/// instances to a <c>set</c> of <typeparamref name="TValue"/> instances. Each addition to the <see cref="HashSet{TKey,TValue}"/>
	/// consists of a <typeparamref name="TValue"/> instance, from which a <typeparamref name="TKey"/> instance is obtained via the
	/// <see cref="IKeyProvider{TKey,TValue}.GetKey"/> method of the <see cref="IKeyProvider{TKey,TValue}"/> implementation specified
	/// when the <see cref="HashSet{TKey,TValue}"/> is initialized. The <typeparamref name="TKey"/> instance is not stored; rather, it
	/// is obtained as needed. Retrieving a <typeparamref name="TValue"/> instance by using its associated <typeparamref name="TKey"/>
	/// instance is very fast, close to <c>O(1)</c>, because the <see cref="HashSet{TKey,TValue}"/> class is implemented as a hash table.
	/// </para>
	/// <note>
	/// The speed of retrieval depends on the quality of the hashing and equality algorithms used by the <see cref="IEqualityComparer{TKey}"/>
	/// implementation and the quality of the key retrieval algorithm used by the <see cref="IKeyProvider{TKey,TValue}"/> impelementation.
	/// </note>
	/// <para>
	/// Instances of <see cref="HashSet{TKey,TValue}"/> require an <see cref="IEqualityComparer{TKey}"/> implementation to determine whether
	/// <typeparamref name="TKey"/> instances are equal (via <see cref="IEqualityComparer{TKey}.Equals"/>), and to obtain a hash
	/// value for <typeparamref name="TKey"/> instances (via <see cref="IEqualityComparer{TKey}.GetHashCode"/>). Instances of
	/// <see cref="HashSet{TKey,TValue}"/> also require an <see cref="IEqualityComparer{TValue}"/> implementation to determine whether
	/// <typeparamref name="TValue"/> instances are equal (via <see cref="IEqualityComparer{TValue}.Equals"/>). You can specify an
	/// implementation of <see cref="IEqualityComparer{T}"/> for keys and values by using a constructor that accepts a
	/// <see cref="IEqualityComparer{TKey}"/> and/or <see cref="IEqualityComparer{TValue}"/> parameters, respectively; if you do not specify
	/// an implementation, the default generic equality comparer <see cref="EqualityComparer{T}.Default"/> is used. The default equality comparer
	/// will use the <see cref="IEquatable{T}"/> implementation of the type being compared, if possible.
	/// </para>
	/// <para>
	/// As long as a <typeparamref name="TValue"/> instance is contained in the <see cref="HashSet{TKey,TValue}"/>, the <typeparamref name="TKey"/>
	/// instance (returned by the <see cref="IKeyProvider{TKey,TValue}.GetKey"/> implementation) for that <typeparamref name="TValue"/> instance
	/// must not change in any way that affects the hash value or equality comparison result returned by the the <see cref="IEqualityComparer{TKey}"/>
	/// implementation for that <typeparamref name="TKey"/> instance.
	/// </para>
	/// <para>
	/// Every <typeparamref name="TValue"/> instance in the <see cref="HashSet{TKey,TValue}"/> must be unique according to the
	/// <see cref="IEqualityComparer{TValue}.Equals"/> method of the <see cref="IEqualityComparer{TValue}"/> implementation. The associated
	/// <typeparamref name="TKey"/> instances are not required to be unique (that is, more than one <typeparamref name="TValue"/> instance
	/// can return the same <typeparamref name="TKey"/> instance), however, using unique <typeparamref name="TKey"/> instances will generally
	/// improve performance. <typeparamref name="TValue"/> instances are not allowed to be <see langword="null"/>. An
	/// <see cref="ArgumentNullException"/> will be thrown by any method of this class that accepts a <typeparamref name="TValue"/> instance
	/// via a parameter if <see langword="null"/> is specified for that parameter. <typeparamref name="TKey"/> instances are allowed to be
	/// <see langword="null"/> only if the <see cref="IEqualityComparer{TKey}"/> implementation supports <see langword="null"/> being
	/// passed to the <see cref="IEqualityComparer{TKey}.Equals"/> and <see cref="IEqualityComparer{TKey}.GetHashCode"/> methods.
	/// </para>
	/// <para>
	/// For purposes of enumeration, each <typeparamref name="TValue"/> instance in the <see cref="HashSet{TKey,TValue}"/> is returned in an
	/// undefined order. However, the order of items will be consistent between the <see cref="IEnumerator{T}"/> implementations and the
	/// <see cref="KeyCollection"/>.
	/// </para>
	/// </remarks>
	/// <threadsafety static="true" instance="false">
	/// Instances of <see cref="HashSet{TKey,TValue}"/> can support multiple readers concurrently, as long as the collection is not modified.
	/// Even so, enumerating through a collection is intrinsically not a thread-safe procedure. In the rare case where an enumeration contends
	/// with write accesses, the collection must be locked during the entire enumeration. To allow the collection to be accessed by multiple
	/// threads for reading and writing, you must implement your own synchronization.
	/// </threadsafety>
	/// <seealso cref="IKeyProvider{TKey,TValue}"/>
	/// <seealso cref="IEqualityComparer{T}"/>
	/// <seealso cref="EqualityComparer{T}.Default"/>
	[Serializable]
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(DebugView<,>))]
	[EditorBrowsable(EditorBrowsableState.Always)]
	public sealed class HashSet<TKey, TValue> : HashSet, IDictionary<TKey, TValue>, ICollection<TValue>, IDictionary, ICollection
	{
		#region Entry class
		[Serializable]
		private sealed class Entry
		{
			public Entry(TValue value, int hash)
				: base()
			{
				this.Value = value;
				this.Hash = hash;
			}
			public TValue Value;
			public int Hash;
			public Entry NextEntry;
		}
		#endregion // Entry class

		#region Constructors
		/// <summary>
		/// Initializes a new instance of <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <param name="keyProvider">
		/// The <see cref="IKeyProvider{TKey,TValue}"/> used to obtain a <typeparamref name="TKey"/> instance for each <typeparamref name="TValue"/> instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="keyProvider"/> is <see langword="null"/>.
		/// </exception>
		public HashSet(IKeyProvider<TKey, TValue> keyProvider)
			: this(0, keyProvider)
		{
		}
		/// <summary>
		/// Initializes a new instance of <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <param name="initialCapacity">
		/// The minimum initial capacity of the <see cref="HashSet{TKey,TValue}"/>.
		/// </param>
		/// <param name="keyProvider">
		/// The <see cref="IKeyProvider{TKey,TValue}"/> used to obtain a <typeparamref name="TKey"/> instance for each <typeparamref name="TValue"/> instance.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="initialCapacity"/> is less than 0.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="keyProvider"/> is <see langword="null"/>.
		/// </exception>
		public HashSet(int initialCapacity, IKeyProvider<TKey, TValue> keyProvider)
			: this(initialCapacity, keyProvider, EqualityComparer<TKey>.Default)
		{
		}
		/// <summary>
		/// Initializes a new instance of <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <param name="keyProvider">
		/// The <see cref="IKeyProvider{TKey,TValue}"/> used to obtain a <typeparamref name="TKey"/> instance for each <typeparamref name="TValue"/> instance.
		/// </param>
		/// <param name="keyEqualityComparer">
		/// The <see cref="IEqualityComparer{TKey}"/> to use with <typeparamref name="TKey"/> instances.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="keyProvider"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="keyEqualityComparer"/> is <see langword="null"/>.
		/// </exception>
		public HashSet(IKeyProvider<TKey, TValue> keyProvider, IEqualityComparer<TKey> keyEqualityComparer)
			: this(0, keyProvider, keyEqualityComparer, EqualityComparer<TValue>.Default)
		{
		}
		/// <summary>
		/// Initializes a new instance of <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <param name="initialCapacity">
		/// The minimum initial capacity of the <see cref="HashSet{TKey,TValue}"/>.
		/// </param>
		/// <param name="keyProvider">
		/// The <see cref="IKeyProvider{TKey,TValue}"/> used to obtain a <typeparamref name="TKey"/> instance for each <typeparamref name="TValue"/> instance.
		/// </param>
		/// <param name="keyEqualityComparer">
		/// The <see cref="IEqualityComparer{TKey}"/> to use with <typeparamref name="TKey"/> instances.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="initialCapacity"/> is less than 0.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="keyProvider"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="keyEqualityComparer"/> is <see langword="null"/>.
		/// </exception>
		public HashSet(int initialCapacity, IKeyProvider<TKey, TValue> keyProvider, IEqualityComparer<TKey> keyEqualityComparer)
			: this(initialCapacity, keyProvider, keyEqualityComparer, EqualityComparer<TValue>.Default)
		{
		}
		/// <summary>
		/// Initializes a new instance of <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <param name="keyProvider">
		/// The <see cref="IKeyProvider{TKey,TValue}"/> used to obtain a <typeparamref name="TKey"/> instance for each <typeparamref name="TValue"/> instance.
		/// </param>
		/// <param name="keyEqualityComparer">
		/// The <see cref="IEqualityComparer{TKey}"/> to use with <typeparamref name="TKey"/> instances.
		/// </param>
		/// <param name="valueEqualityComparer">
		/// The <see cref="IEqualityComparer{TValue}"/> to use with <typeparamref name="TValue"/> instances.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="keyProvider"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="keyEqualityComparer"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="valueEqualityComparer"/> is <see langword="null"/>.
		/// </exception>
		public HashSet(IKeyProvider<TKey, TValue> keyProvider, IEqualityComparer<TKey> keyEqualityComparer, IEqualityComparer<TValue> valueEqualityComparer)
			: this(0, keyProvider, keyEqualityComparer, valueEqualityComparer)
		{
		}
		/// <summary>
		/// Initializes a new instance of <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <param name="initialCapacity">
		/// The minimum initial capacity of the <see cref="HashSet{TKey,TValue}"/>.
		/// </param>
		/// <param name="keyProvider">
		/// The <see cref="IKeyProvider{TKey,TValue}"/> used to obtain a <typeparamref name="TKey"/> instance for each <typeparamref name="TValue"/> instance.
		/// </param>
		/// <param name="keyEqualityComparer">
		/// The <see cref="IEqualityComparer{TKey}"/> to use with <typeparamref name="TKey"/> instances.
		/// </param>
		/// <param name="valueEqualityComparer">
		/// The <see cref="IEqualityComparer{TValue}"/> to use with <typeparamref name="TValue"/> instances.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="initialCapacity"/> is less than 0.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="keyProvider"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="keyEqualityComparer"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="valueEqualityComparer"/> is <see langword="null"/>.
		/// </exception>
		public HashSet(int initialCapacity, IKeyProvider<TKey, TValue> keyProvider, IEqualityComparer<TKey> keyEqualityComparer, IEqualityComparer<TValue> valueEqualityComparer)
			: base()
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentOutOfRangeException("initialCapacity");
			}
			if (keyProvider == null)
			{
				throw new ArgumentNullException("keyProvider");
			}
			if (keyEqualityComparer == null)
			{
				throw new ArgumentNullException("keyEqualityComparer");
			}
			if (valueEqualityComparer == null)
			{
				throw new ArgumentNullException("valueEqualityComparer");
			}

			this._keyProvider = keyProvider;
			this._keyComparer = keyEqualityComparer;
			this._valueComparer = valueEqualityComparer;

			this._entries = (initialCapacity > 0) ? new Entry[base.GetNewCapacity(initialCapacity, true)] : EmptyArray;
		}
		#endregion // Constructors

		private static readonly Entry[] EmptyArray = new Entry[0];

		private int _size;
		private Entry[] _entries;

		#region EnsureCapacity and ProcessEntryForResize methods
		private void EnsureCapacity(int minCapacity)
		{
			Entry[] oldEntries = this._entries;
			if (minCapacity > oldEntries.Length)
			{
				this._entries = new Entry[base.GetNewCapacity(minCapacity, false)];
				this._size = 0;
				
				for (int i = 0; i < oldEntries.Length; i++)
				{
					this.ProcessEntryForResize(oldEntries[i]);
				}
			}
		}

		private void ProcessEntryForResize(Entry entry)
		{
			if (entry == null)
			{
				return;
			}
			this.ProcessEntryForResize(entry.NextEntry);
			entry.NextEntry = null;
			this.AddEntry(entry, DuplicateKeyBehavior.Ignore);
		}
		#endregion // EnsureCapacity and ProcessEntryForResize methods

		/*
		 * Throwing Considerations:
		 * 
		 * Options: throw, return default/empty
		 * 
		 * Expects single value, multiple values encountered
		 * Expects value, no value encountered
		 * 
		 * Null values? Always throw
		 * Null keys? Let the IKeyProvider throw if it can't handle nulls
		 * 
		 */

		#region Add methods
		/// <summary>
		/// Adds a new <typeparamref name="TValue"/> instance to this <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <param name="value">
		/// The <typeparamref name="TValue"/> instance to be added.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if no other <typeparamref name="TValue"/> instance in this <see cref="HashSet{TKey,TValue}"/>
		/// is associated with the same <typeparamref name="TKey"/> instance as <paramref name="value"/>; otherwise, <see langword="false"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="value"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="value"/> is already contained within this <see cref="HashSet{TKey,TValue}"/>.
		/// </exception>
		public bool Add(TValue value)
		{
			return this.AddCore(value, DuplicateKeyBehavior.Check);
		}
		void ICollection<TValue>.Add(TValue value)
		{
			this.AddCore(value, DuplicateKeyBehavior.Ignore);
		}
		private bool AddCore(TValue value, DuplicateKeyBehavior duplicateKeyBehavior)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.EnsureCapacity(this._size + 1);
			return this.AddEntry(new Entry(value, this._keyComparer.GetHashCode(this._keyProvider.GetKey(value))), duplicateKeyBehavior);
		}
		[Serializable]
		private enum DuplicateKeyBehavior : byte
		{
			Ignore = 0,
			Check = 1,
			Throw = 2
		}
		private bool AddEntry(Entry newEntry, DuplicateKeyBehavior duplicateKeyBehavior)
		{
			Entry[] entries = this._entries;
			int hash = newEntry.Hash;
			int index = Math.Abs(hash % entries.Length);

			Entry existingEntry = entries[index];
			if (existingEntry != null)
			{
				IKeyProvider<TKey, TValue> keyProvider = this._keyProvider;
				IEqualityComparer<TKey> keyComparer = this._keyComparer;
				IEqualityComparer<TValue> valueComparer = this._valueComparer;
				TValue newValue = newEntry.Value;
				TKey newKey = keyProvider.GetKey(newValue);
				bool duplicateKey = false;
				while (true)
				{
					if (existingEntry.Hash == hash)
					{
						TValue existingValue = existingEntry.Value;
						if (valueComparer.Equals(existingValue, newValue))
						{
							throw DuplicateValueException(newValue);
						}
						if (duplicateKeyBehavior != DuplicateKeyBehavior.Ignore && !duplicateKey && keyComparer.Equals(keyProvider.GetKey(existingValue), newKey))
						{
							if (duplicateKeyBehavior == DuplicateKeyBehavior.Throw)
							{
								throw DuplicateKeyAddException(newKey);
							}
							duplicateKey = true;
						}
					}
					if (existingEntry.NextEntry == null)
					{
						break;
					}
					existingEntry = existingEntry.NextEntry;
				}
				existingEntry.NextEntry = newEntry;
				this._size++;
				return !duplicateKey;
			}
			else
			{
				entries[index] = newEntry;
				this._size++;
				return true;
			}
		}
		private void AddKeyValuePair(TKey key, TValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.EnsureGivenKeyMatchesProvider(key, value, false);
			this.EnsureCapacity(this._size + 1);
			this.AddEntry(new Entry(value, this._keyComparer.GetHashCode(key)), DuplicateKeyBehavior.Throw);
		}
		void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
		{
			this.AddKeyValuePair(key, value);
		}
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			this.AddKeyValuePair(item.Key, item.Value);
		}
		void IDictionary.Add(object key, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!IsKeyTypeCompatible(key))
			{
				throw KeyTypeMismatchException(key);
			}
			if (!(value is TValue))
			{
				throw ValueTypeMismatchException(value);
			}
			this.AddKeyValuePair((TKey)key, (TValue)value);
		}
		#endregion // Add methods

		#region ReplaceOrAdd method
		private void ReplaceOrAdd(TKey key, TValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.EnsureGivenKeyMatchesProvider(key, value, false);

			Entry[] entries = this._entries;
			IEqualityComparer<TKey> keyComparer = this._keyComparer;
			int hash = keyComparer.GetHashCode(key);
			if (entries.Length <= 0)
			{
				entries = this._entries = new Entry[base.GetNewCapacity(1, true)];
			}
			int index = Math.Abs(hash % entries.Length);

			Entry existingEntry = entries[index];
			if (existingEntry != null)
			{
				IKeyProvider<TKey, TValue> keyProvider = this._keyProvider;
				Entry matchingEntry = null;
				while (true)
				{
					if (existingEntry.Hash == hash && keyComparer.Equals(keyProvider.GetKey(existingEntry.Value), key))
					{
						if (matchingEntry != null)
						{
							throw DuplicateKeyGetRemoveOrReplaceException(key);
						}
						matchingEntry = existingEntry;
					}
					if (existingEntry.NextEntry == null)
					{
						break;
					}
					existingEntry = existingEntry.NextEntry;
				}
				if (matchingEntry != null)
				{
					matchingEntry.Value = value;
				}
				else
				{
					existingEntry.NextEntry = new Entry(value, hash);
					// Don't call EnsureCapacity until after we have added the new Entry,
					// since it could cause the location where it needs to be added to change.
					this.EnsureCapacity(++this._size);
				}
			}
			else
			{
				entries[index] = new Entry(value, hash);
				// Don't call EnsureCapacity until after we have added the new Entry,
				// since it could cause the location where it needs to be added to change.
				this.EnsureCapacity(++this._size);
			}
		}
		#endregion // ReplaceOrAdd method

		#region Remove methods
		/// <summary>
		/// Removes all <typeparamref name="TValue"/> instances associated with the <typeparamref name="TKey"/> instance
		/// specified by <paramref name="key"/> from this <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance for which all associated <typeparamref name="TValue"/> instances
		/// should be removed.
		/// </param>
		/// <returns>
		/// The number of <typeparamref name="TValue"/> instances removed.
		/// </returns>
		public int RemoveAll(TKey key)
		{
			return this.RemoveKey(key, false);
		}
		/// <summary>
		/// Removes the single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/> instance
		/// specified by <paramref name="key"/> from this <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance for which the single associated <typeparamref name="TValue"/> instance
		/// should be removed.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the single <typeparamref name="TValue"/> instance was successfully found and removed;
		/// otherwise, <see langword="false"/>.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Multiple <typeparamref name="TValue"/> instances are associated with the <typeparamref name="TKey"/> instance
		/// specified by <paramref name="key"/>.
		/// </exception>
		public bool RemoveSingle(TKey key)
		{
			return this.RemoveKey(key, true) > 0;
		}
		private int RemoveKey(TKey key, bool throwOnDuplicates)
		{
			int entriesRemoved = 0;
			if (this._size > 0)
			{
				Entry[] entries = this._entries;
				IKeyProvider<TKey, TValue> keyProvider = this._keyProvider;
				IEqualityComparer<TKey> keyComparer = this._keyComparer;
				int hash = keyComparer.GetHashCode(key);
				int index = Math.Abs(hash % entries.Length);
				for (Entry previousEntry = null, entry = entries[index]; entry != null; previousEntry = entry, entry = entry.NextEntry)
				{
					if (entry.Hash == hash && keyComparer.Equals(keyProvider.GetKey(entry.Value), key))
					{
						if (throwOnDuplicates && entriesRemoved > 0)
						{
							throw DuplicateKeyGetRemoveOrReplaceException(key);
						}
						if (previousEntry == null)
						{
							entries[index] = entry.NextEntry;
						}
						else
						{
							previousEntry.NextEntry = entry.NextEntry;
						}
						entriesRemoved++;
					}
				}
				if (entriesRemoved > 0)
				{
					this._size -= entriesRemoved;
				}
			}
			return entriesRemoved;
		}
		/// <summary>
		/// Removes the <typeparamref name="TValue"/> instance specified by <paramref name="value"/> from this
		/// <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <param name="value">
		/// The <typeparamref name="TValue"/> instance to be removed.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the <typeparamref name="TValue"/> instance specified by <paramref name="value"/>
		/// was successfully found and removed; otherwise, <see langword="false"/>.
		/// </returns>
		public bool Remove(TValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this._size > 0)
			{
				Entry[] entries = this._entries;
				IEqualityComparer<TValue> valueComparer = this._valueComparer;
				int hash = this._keyComparer.GetHashCode(this._keyProvider.GetKey(value));
				int index = Math.Abs(hash % entries.Length);
				for (Entry previousEntry = null, entry = entries[index]; entry != null; previousEntry = entry, entry = entry.NextEntry)
				{
					if (entry.Hash == hash && valueComparer.Equals(entry.Value, value))
					{
						if (previousEntry == null)
						{
							entries[index] = entry.NextEntry;
						}
						else
						{
							previousEntry.NextEntry = entry.NextEntry;
						}
						this._size--;
						return true;
					}
				}
			}
			return false;
		}
		bool IDictionary<TKey, TValue>.Remove(TKey key)
		{
			return this.RemoveSingle(key);
		}
		void IDictionary.Remove(object key)
		{
			if (IsKeyTypeCompatible(key))
			{
				this.RemoveSingle((TKey)key);
			}
		}
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			TValue value = item.Value;
			this.EnsureGivenKeyMatchesProvider(item.Key, value, true);
			return this.Remove(value);
		}
		#endregion // Remove methods

		#region GetValues method and variants
		/// <summary>
		/// Gets an <see cref="IList{TValue}"/> containing the <typeparamref name="TValue"/> instances
		/// associated with the <typeparamref name="TKey"/> instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance for which the associated <typeparamref name="TValue"/>
		/// instances should be retrieved.
		/// </param>
		/// <returns>
		/// An <see cref="IList{TValue}"/> containing the <typeparamref name="TValue"/> instances
		/// associated with the <typeparamref name="TKey"/> instance specified by <paramref name="key"/>.
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// There are no <typeparamref name="TValue"/> instances associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </exception>
		public IList<TValue> GetValues(TKey key)
		{
			// Throw if missing
			IList<TValue> values;
			if (!this.TryGetValues(key, out values))
			{
				throw KeyNotFoundException();
			}
			return values;
		}

		/// <summary>
		/// Gets an <see cref="IList{TValue}"/> containing the <typeparamref name="TValue"/> instances
		/// associated with the <typeparamref name="TKey"/> instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance for which the associated <typeparamref name="TValue"/>
		/// instances should be retrieved.
		/// </param>
		/// <param name="values">
		/// An <see cref="IList{TValue}"/> containing the <typeparamref name="TValue"/> instances
		/// associated with the <typeparamref name="TKey"/> instance specified by <paramref name="key"/>.
		/// If no associated <typeparamref name="TValue"/> instances are found, the <see cref="IList{TValue}"/>
		/// will be empty.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if at least one <typeparamref name="TValue"/> instance associated with
		/// the <typeparamref name="TKey"/> instance specified by <paramref name="key"/> was found; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		public bool TryGetValues(TKey key, out IList<TValue> values)
		{
			// Empty collection if missing
			values = this.FindValues(key);
			return (values.Count > 0);
		}
		/// <summary>
		/// Gets an <see cref="IList{TValue}"/> containing the <typeparamref name="TValue"/> instances
		/// associated with the <typeparamref name="TKey"/> instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance for which the associated <typeparamref name="TValue"/>
		/// instances should be retrieved.
		/// </param>
		/// <returns>
		/// An <see cref="IList{TValue}"/> containing the <typeparamref name="TValue"/> instances
		/// associated with the <typeparamref name="TKey"/> instance specified by <paramref name="key"/>.
		/// If no associated <typeparamref name="TValue"/> instances are found, the <see cref="IList{TValue}"/>
		/// will be empty.
		/// </returns>
		public IList<TValue> FindValues(TKey key)
		{
			// Empty collection if missing
			List<TValue> values = new List<TValue>(1);
			if (this._size > 0)
			{
				Entry[] entries = this._entries;
				IKeyProvider<TKey, TValue> keyProvider = this._keyProvider;
				IEqualityComparer<TKey> keyComparer = this._keyComparer;
				int hash = keyComparer.GetHashCode(key);
				for (Entry entry = entries[Math.Abs(hash % entries.Length)]; entry != null; entry = entry.NextEntry)
				{
					TValue value;
					if (entry.Hash == hash && keyComparer.Equals(keyProvider.GetKey(value = entry.Value), key))
					{
						values.Add(value);
					}
				}
			}
			return values;
		}
		#endregion // GetValues method and variants

		#region GetValue and GetAnyValue methods and variants
		/// <summary>
		/// Gets the single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance associated with the single <typeparamref name="TValue"/>
		/// instance that should be retrieved.
		/// </param>
		/// <returns>
		/// The single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// There is no <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Multiple <typeparamref name="TValue"/> instances are associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </exception>
		public TValue GetValue(TKey key)
		{
			// Throw if missing, throw if duplicate keys found
			TValue value = default(TValue);
			if (!this.TryGetValue(key, ref value, true))
			{
				throw KeyNotFoundException();
			}
			return value;
		}
		/// <summary>
		/// Gets a single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance for which a single <typeparamref name="TValue"/> instance
		/// should be retrieved.
		/// </param>
		/// <returns>
		/// A single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// There are no <typeparamref name="TValue"/> instances associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </exception>
		public TValue GetAnyValue(TKey key)
		{
			// Throw if missing
			TValue value = default(TValue);
			if (!this.TryGetValue(key, ref value, false))
			{
				throw KeyNotFoundException();
			}
			return value;
		}
		/// <summary>
		/// Gets the single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance associated with the single <typeparamref name="TValue"/>
		/// instance that should be retrieved.
		/// </param>
		/// <param name="value">
		/// The single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>, or the default value for <typeparamref name="TValue"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the single <typeparamref name="TValue"/> instance associated with
		/// the <typeparamref name="TKey"/> instance specified by <paramref name="key"/> was found; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Multiple <typeparamref name="TValue"/> instances are associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </exception>
		public bool TryGetValue(TKey key, out TValue value)
		{
			// Default if missing, throw if duplicate keys found
			value = default(TValue);
			return this.TryGetValue(key, ref value, true);
		}
		/// <summary>
		/// Gets a single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance for which a single <typeparamref name="TValue"/> instance
		/// should be retrieved.
		/// </param>
		/// <param name="value">
		/// A single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>, or the default value for <typeparamref name="TValue"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if a <typeparamref name="TValue"/> instance associated with the
		/// <typeparamref name="TKey"/> instance specified by <paramref name="key"/> was found; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		public bool TryGetAnyValue(TKey key, out TValue value)
		{
			// Default if missing
			value = default(TValue);
			return this.TryGetValue(key, ref value, false);
		}
		/// <summary>
		/// Gets the single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance associated with the single <typeparamref name="TValue"/>
		/// instance that should be retrieved.
		/// </param>
		/// <returns>
		/// The single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>, or the default value for <typeparamref name="TValue"/>.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Multiple <typeparamref name="TValue"/> instances are associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </exception>
		public TValue FindValue(TKey key)
		{
			return this.FindValue(key, default(TValue));
		}
		/// <summary>
		/// Gets the single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance associated with the single <typeparamref name="TValue"/>
		/// instance that should be retrieved.
		/// </param>
		/// <param name="defaultValue">
		/// The <typeparamref name="TValue"/> instance that should be returned if no <typeparamref name="TValue"/>
		/// instance associated with the <typeparamref name="TKey"/> instance specified by <paramref name="key"/>
		/// can be found.
		/// </param>
		/// <returns>
		/// The single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>, or the <typeparamref name="TValue"/> instance specified
		/// by <paramref name="defaultValue"/>.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Multiple <typeparamref name="TValue"/> instances are associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </exception>
		public TValue FindValue(TKey key, TValue defaultValue)
		{
			// Throw if duplicate keys found
			this.TryGetValue(key, ref defaultValue, true);
			return defaultValue;
		}
		/// <summary>
		/// Gets a single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance for which a single <typeparamref name="TValue"/> instance
		/// should be retrieved.
		/// </param>
		/// <returns>
		/// A single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>, or the default value for <typeparamref name="TValue"/>.
		/// </returns>
		public TValue FindAnyValue(TKey key)
		{
			return this.FindAnyValue(key, default(TValue));
		}
		/// <summary>
		/// Gets a single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance for which a single <typeparamref name="TValue"/> instance
		/// should be retrieved.
		/// </param>
		/// <param name="defaultValue">
		/// The <typeparamref name="TValue"/> instance that should be returned if no <typeparamref name="TValue"/>
		/// instance associated with the <typeparamref name="TKey"/> instance specified by <paramref name="key"/>
		/// can be found.
		/// </param>
		/// <returns>
		/// A single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>, or the <typeparamref name="TValue"/> instance specified
		/// by <paramref name="defaultValue"/>.
		/// </returns>
		public TValue FindAnyValue(TKey key, TValue defaultValue)
		{
			this.TryGetValue(key, ref defaultValue, false);
			return defaultValue;
		}
		private bool TryGetValue(TKey key, ref TValue value, bool checkForDuplicates)
		{
			if (this._size <= 0)
			{
				return false;
			}
			Entry[] entries = this._entries;
			IKeyProvider<TKey, TValue> keyProvider = this._keyProvider;
			IEqualityComparer<TKey> keyComparer = this._keyComparer;
			int hash = keyComparer.GetHashCode(key);
			bool foundKey = false;
			for (Entry entry = entries[Math.Abs(hash % entries.Length)]; entry != null; entry = entry.NextEntry)
			{
				if (entry.Hash == hash && keyComparer.Equals(keyProvider.GetKey(entry.Value), key))
				{
					if (checkForDuplicates && foundKey)
					{
						throw DuplicateKeyGetRemoveOrReplaceException(key);
					}
					value = entry.Value;
					if (!checkForDuplicates)
					{
						return true;
					}
					foundKey = true;
				}
			}
			return foundKey;
		}
		#endregion // GetValue and GetAnyValue methods and variants

		#region Contains methods
		/// <summary>
		/// Determines whether this <see cref="HashSet{TKey,TValue}"/> contains at least one
		/// <typeparamref name="TValue"/> instance that is associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance for which an attempt to locate an associated
		/// <typeparamref name="TValue"/> instance should be made.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if this <see cref="HashSet{TKey,TValue}"/> contains at least one
		/// <typeparamref name="TValue"/> instance that is associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </returns>
		public bool ContainsKey(TKey key)
		{
			Entry[] entries = this._entries;
			if (this._size > 0)
			{
				IKeyProvider<TKey, TValue> keyProvider = this._keyProvider;
				IEqualityComparer<TKey> keyComparer = this._keyComparer;
				int hash = keyComparer.GetHashCode(key);
				for (Entry entry = entries[Math.Abs(hash % entries.Length)]; entry != null; entry = entry.NextEntry)
				{
					if (entry.Hash == hash && keyComparer.Equals(keyProvider.GetKey(entry.Value), key))
					{
						return true;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Determines whether this <see cref="HashSet{TKey,TValue}"/> contains the <typeparamref name="TValue"/>
		/// instance specified by <paramref name="value"/>.
		/// </summary>
		/// <param name="value">
		/// The <typeparamref name="TValue"/> instance to locate in this <see cref="HashSet{TKey,TValue}"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if this <see cref="HashSet{TKey,TValue}"/> contains the <typeparamref name="TValue"/>
		/// instance specified by <paramref name="value"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="value"/> is <see langword="null"/>.
		/// </exception>
		public bool Contains(TValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Entry[] entries = this._entries;
			if (this._size > 0)
			{
				IEqualityComparer<TValue> valueComparer = this._valueComparer;
				int hash = this._keyComparer.GetHashCode(this._keyProvider.GetKey(value));
				for (Entry entry = entries[Math.Abs(hash % entries.Length)]; entry != null; entry = entry.NextEntry)
				{
					if (entry.Hash == hash && valueComparer.Equals(entry.Value, value))
					{
						return true;
					}
				}
			}
			return false;
		}
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			TValue value = item.Value;
			this.EnsureGivenKeyMatchesProvider(item.Key, value, true);
			return this.Contains(value);
		}
		bool IDictionary.Contains(object key)
		{
			return IsKeyTypeCompatible(key) && this.ContainsKey((TKey)key);
		}
		#endregion // Contains methods

		#region Clear method
		/// <summary>
		/// Removes all <typeparamref name="TValue"/> instances from this <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		public void Clear()
		{
			this._size = 0;
			Array.Clear(this._entries, 0, this._entries.Length);
		}
		#endregion // Clear method

		#region CopyTo and ToArray methods
		/// <summary>
		/// Copies the contents of this <see cref="HashSet{TKey,TValue}"/> to a new <typeparamref name="TValue"/>
		/// array and returns it.
		/// </summary>
		/// <returns>
		/// A new <typeparamref name="TValue"/> array containing a copy of the contents of this
		/// <see cref="HashSet{TKey,TValue}"/>.
		/// </returns>
		public TValue[] ToArray()
		{
			TValue[] array = new TValue[this._size];
			this.CopyToInternal(array, 0);
			return array;
		}
		/// <summary>
		/// Copies the contents of this <see cref="HashSet{TKey,TValue}"/> to the <typeparamref name="TValue"/>
		/// array specified by <paramref name="array"/>.
		/// </summary>
		/// <param name="array">
		/// The <typeparamref name="TValue"/> array to which the contents of this <see cref="HashSet{TKey,TValue}"/>
		/// should be copied.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="array"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The number of elements in this <see cref="HashSet{TKey,TValue}"/> is greater than the available space in
		/// the destination <paramref name="array"/>.
		/// </exception>
		public void CopyTo(TValue[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Length < this._size)
			{
				throw ArrayTooSmallException();
			}
			this.CopyToInternal(array, 0);
		}
		/// <summary>
		/// Copies the contents of this <see cref="HashSet{TKey,TValue}"/> to the <typeparamref name="TValue"/>
		/// array specified by <paramref name="array"/>, starting at the array index specified by <paramref name="arrayIndex"/>.
		/// </summary>
		/// <param name="array">
		/// The <typeparamref name="TValue"/> array to which the contents of this <see cref="HashSet{TKey,TValue}"/>
		/// should be copied.
		/// </param>
		/// <param name="arrayIndex">
		/// The zero-based index in <paramref name="array"/> at which copying should begin.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="array"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="arrayIndex"/> is less than 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="arrayIndex"/> is greater than or equal to the length of <paramref name="array"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The number of elements in this <see cref="HashSet{TKey,TValue}"/> is greater than the available space from
		/// <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
		/// </exception>
		public void CopyTo(TValue[] array, int arrayIndex)
		{
			this.VerifyArrayForCopy(array, arrayIndex, true);
			this.CopyToInternal(array, arrayIndex);
		}
		private void CopyToInternal(TValue[] array, int arrayIndex)
		{
			Entry[] entries = this._entries;
			for (int i = 0; i < entries.Length; i++)
			{
				Entry currentEntry = entries[i];
				if (currentEntry != null)
				{
					do
					{
						array[arrayIndex++] = currentEntry.Value;
					}
					while ((currentEntry = currentEntry.NextEntry) != null);
				}
			}
		}
		/// <summary>
		/// Copies the contents of this <see cref="HashSet{TKey,TValue}"/> to the <see cref="KeyValuePair{TKey,TValue}"/>
		/// array specified by <paramref name="array"/>, starting at the array index specified by <paramref name="arrayIndex"/>.
		/// </summary>
		/// <param name="array">
		/// The <see cref="KeyValuePair{TKey,TValue}"/> array to which the contents of this <see cref="HashSet{TKey,TValue}"/>
		/// should be copied.
		/// </param>
		/// <param name="arrayIndex">
		/// The zero-based index in <paramref name="array"/> at which copying should begin.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="array"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="arrayIndex"/> is less than 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="arrayIndex"/> is greater than or equal to the length of <paramref name="array"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The number of elements in this <see cref="HashSet{TKey,TValue}"/> is greater than the available space from
		/// <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
		/// </exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.VerifyArrayForCopy(array, arrayIndex, true);
			this.CopyToInternal(array, arrayIndex);
		}
		private void CopyToInternal(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			IKeyProvider<TKey, TValue> keyProvider = this._keyProvider;
			Entry[] entries = this._entries;
			for (int i = 0; i < entries.Length; i++)
			{
				Entry currentEntry = entries[i];
				if (currentEntry != null)
				{
					do
					{
						TValue currentValue = currentEntry.Value;
						array[arrayIndex++] = new KeyValuePair<TKey, TValue>(keyProvider.GetKey(currentValue), currentValue);
					}
					while ((currentEntry = currentEntry.NextEntry) != null);
				}
			}
		}
		/// <summary>
		/// Copies the contents of this <see cref="HashSet{TKey,TValue}"/> to the <see cref="DictionaryEntry"/>
		/// array specified by <paramref name="array"/>, starting at the array index specified by <paramref name="arrayIndex"/>.
		/// </summary>
		/// <param name="array">
		/// The <see cref="DictionaryEntry"/> array to which the contents of this <see cref="HashSet{TKey,TValue}"/>
		/// should be copied.
		/// </param>
		/// <param name="arrayIndex">
		/// The zero-based index in <paramref name="array"/> at which copying should begin.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="array"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="arrayIndex"/> is less than 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="arrayIndex"/> is greater than or equal to the length of <paramref name="array"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The number of elements in this <see cref="HashSet{TKey,TValue}"/> is greater than the available space from
		/// <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
		/// </exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void CopyTo(DictionaryEntry[] array, int arrayIndex)
		{
			this.VerifyArrayForCopy(array, arrayIndex, true);
			this.CopyToInternal(array, arrayIndex);
		}
		private void CopyToInternal(DictionaryEntry[] array, int arrayIndex)
		{
			IKeyProvider<TKey, TValue> keyProvider = this._keyProvider;
			Entry[] entries = this._entries;
			for (int i = 0; i < entries.Length; i++)
			{
				Entry currentEntry = entries[i];
				if (currentEntry != null)
				{
					do
					{
						TValue currentValue = currentEntry.Value;
						array[arrayIndex++] = new DictionaryEntry(keyProvider.GetKey(currentValue), currentValue);
					}
					while ((currentEntry = currentEntry.NextEntry) != null);
				}
			}
		}
		void ICollection.CopyTo(Array array, int index)
		{
			this.VerifyArrayForCopy(array, index, false);

			TValue[] valueArray = array as TValue[];
			if (valueArray != null)
			{
				this.CopyToInternal(valueArray, index);
				return;
			}

			KeyValuePair<TKey, TValue>[] keyValuePairArray = array as KeyValuePair<TKey, TValue>[];
			if (keyValuePairArray != null)
			{
				this.CopyToInternal(keyValuePairArray, index);
				return;
			}

			DictionaryEntry[] dictionaryEntryArray = array as DictionaryEntry[];
			if (dictionaryEntryArray != null)
			{
				this.CopyToInternal(dictionaryEntryArray, index);
				return;
			}

			TKey[] keyArray = array as TKey[];
			if (keyArray != null)
			{
				this.Keys.CopyTo(keyArray, index);
				return;
			}

			Type elementType = array.GetType().GetElementType();

			if (elementType.IsAssignableFrom(typeof(TValue)))
			{
				CopyFromEnumerator(this.GetEnumerator(), array, index);
				return;
			}

			if (elementType.IsAssignableFrom(typeof(KeyValuePair<TKey, TValue>)))
			{
				CopyFromEnumerator(this.GetKeyValuePairEnumerator(), array, index);
				return;
			}

			if (elementType.IsAssignableFrom(typeof(DictionaryEntry)))
			{
				CopyFromEnumerator(this.GetDictionaryEnumerator(), array, index);
				return;
			}

			if (elementType.IsAssignableFrom(typeof(TKey)))
			{
				CopyFromEnumerator(this.GetKeyEnumerator(), array, index);
				return;
			}

			throw new ArrayTypeMismatchException();
		}
		private static void CopyFromEnumerator<TEnumerator>(TEnumerator enumerator, Array array, int index)
			where TEnumerator : struct, IEnumerator
		{
			while (enumerator.MoveNext())
			{
				array.SetValue(enumerator.Current, index++);
			}
		}
		#endregion // CopyTo and ToArray methods


		#region Exception helper methods
		private static ArgumentException DuplicateValueException(TValue duplicateValue)
		{
			// UNDONE: Localize this
			return new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The value '{0}' has already been added to this collection.", duplicateValue), "value");
		}
		private static ArgumentException DuplicateKeyAddException(TKey duplicateKey)
		{
			// UNDONE: Localize this
			return new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The key '{0}' has already been added to this collection.", duplicateKey), "key");
		}
		private static ArgumentException DuplicateKeyGetRemoveOrReplaceException(TKey duplicateKey)
		{
			// UNDONE: Localize this
			return new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Multiple values for key '{0}' exist in this collection.", duplicateKey), "key");
		}
		private static NotSupportedException MutatingKeyCollectionNotSupportedException()
		{
			// UNDONE: Localize this
			// This corresponds to "NotSupported_KeyCollectionSet" in mscorlib.resources.
			return new NotSupportedException("Mutating a key collection derived from a dictionary is not allowed.");
		}
		private static ArgumentException ArrayTooSmallException()
		{
			// UNDONE: Localize this
			return new ArgumentException("The specified array is not large enough to contain all of the values in this collection.", "array");
		}
		private static KeyNotFoundException KeyNotFoundException()
		{
			return new KeyNotFoundException();
		}
		private static ArgumentException ValueTypeMismatchException(object value)
		{
			// UNDONE: Localize this
			// This corresponds to "Arg_WrongType" in mscorlib.resources.
			return new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The value '{0}' is not of type '{1}' and cannot be used in this generic collection.", value, typeof(TValue)), "value");
		}
		private static ArgumentException KeyTypeMismatchException(object key)
		{
			// UNDONE: Localize this
			return new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The key '{0}' is not of type '{1}' and cannot be used in this generic collection.", key, typeof(TKey)), "key");
		}
		private static bool IsKeyTypeCompatible(object key)
		{
			Type keyType;
			// Check that is is the correct type, or it is null and not a ValueType, or it is null and a ValueType and Nullable<>
			return (key is TKey) || (key == null && (!(keyType = typeof(TKey)).IsValueType || (keyType.IsGenericType && keyType.GetGenericTypeDefinition() == typeof(Nullable<>))));
		}
		private void EnsureGivenKeyMatchesProvider(TKey key, TValue value, bool paramIsItem)
		{
			if (!this._keyComparer.Equals(this._keyProvider.GetKey(value), key))
			{
				// UNDONE: Localize this
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The given key '{0}' does not match the key returned by the IKeyProvider for value '{1}'.", key, value), paramIsItem ? "item" : "key");
			}
		}
		private void VerifyArrayForCopy(Array array, int arrayIndex, bool indexIsArrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				// While the 'arrayIndex > array.Length' check above should technically be '>=' instead,
				// the more relaxed check is needed for compatibility with Dictionary<TKey,TValue>
				throw new ArgumentOutOfRangeException(indexIsArrayIndex ? "arrayIndex" : "index");
			}
			if (array.Length - arrayIndex < this._size)
			{
				throw ArrayTooSmallException();
			}
		}
		#endregion // Exception helper methods


		#region Properties

		#region Count property
		/// <summary>
		/// Gets the number of <typeparamref name="TValue"/> instances in this <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <seealso cref="ICollection{TValue}.Count"/>
		public int Count
		{
			get
			{
				return this._size;
			}
		}
		#endregion // Count property

		#region IsReadOnly property
		/// <summary>
		/// Gets a value indicating whether this <see cref="HashSet{TKey,TValue}"/> is read-only.
		/// </summary>
		/// <remarks>
		/// This property always returns <see langword="false"/>.
		/// </remarks>
		/// <seealso cref="ICollection{TValue}.IsReadOnly"/>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}
		#endregion // IsReadOnly property

		#region KeyProvider property
		private readonly IKeyProvider<TKey, TValue> _keyProvider;
		/// <summary>
		/// Gets the <see cref="IKeyProvider{TKey,TValue}"/> implementation provided to this
		/// <see cref="HashSet{TKey,TValue}"/> when it was initialized.
		/// </summary>
		public IKeyProvider<TKey, TValue> KeyProvider
		{
			get
			{
				return this._keyProvider;
			}
		}
		#endregion // KeyProvider property

		#region KeyComparer property
		private readonly IEqualityComparer<TKey> _keyComparer;
		/// <summary>
		/// Gets the <see cref="IEqualityComparer{TKey}"/> implementation provided to this
		/// <see cref="HashSet{TKey,TValue}"/> when it was initialized.
		/// </summary>
		public IEqualityComparer<TKey> KeyComparer
		{
			get
			{
				return this._keyComparer;
			}
		}
		#endregion // KeyComparer property

		#region ValueComparer property
		private readonly IEqualityComparer<TValue> _valueComparer;
		/// <summary>
		/// Gets the <see cref="IEqualityComparer{TValue}"/> implementation provided to this
		/// <see cref="HashSet{TKey,TValue}"/> when it was initialized.
		/// </summary>
		public IEqualityComparer<TValue> ValueComparer
		{
			get
			{
				return this._valueComparer;
			}
		}
		#endregion // ValueComparer property

		#region Indexer
		/// <summary>
		/// Gets or sets the single <typeparamref name="TValue"/> instance associated with the
		/// <typeparamref name="TKey"/> instance specified by <paramref name="key"/>.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="TKey"/> instance associated with the single <typeparamref name="TValue"/>
		/// instance that should be retrieved or assigned.
		/// </param>
		/// <returns>
		/// The single <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="value"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		/// There is no <typeparamref name="TValue"/> instance associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The <typeparamref name="TKey"/> instance specified by <paramref name="key"/> does not match the
		/// <typeparamref name="TKey"/> instance returned by the <see cref="IKeyProvider{TKey,TValue}"/> for
		/// <paramref name="value"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Multiple <typeparamref name="TValue"/> instances are associated with the <typeparamref name="TKey"/>
		/// instance specified by <paramref name="key"/>.
		/// </exception>
		public TValue this[TKey key]
		{
			get
			{
				return this.GetValue(key);
			}
			set
			{
				this.ReplaceOrAdd(key, value);
			}
		}
		object IDictionary.this[object key]
		{
			get
			{
				if (IsKeyTypeCompatible(key))
				{
					TValue value = default(TValue);
					if (this.TryGetValue((TKey)key, out value))
					{
						return value;
					}
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!IsKeyTypeCompatible(key))
				{
					throw KeyTypeMismatchException(key);
				}
				if (!(value is TValue))
				{
					throw ValueTypeMismatchException(value);
				}
				this.ReplaceOrAdd((TKey)key, (TValue)value);
			}
		}
		#endregion // Indexer

		#region Keys properties

		#region KeyCollection struct
		/// <summary>
		/// An <see cref="ICollection{TKey}"/> of the keys in a <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		[Serializable]
		[StructLayout(LayoutKind.Auto)]
		[DebuggerDisplay("Count = {Count}")]
		[DebuggerTypeProxy(typeof(KeyCollectionDebugView<,>))]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public struct KeyCollection : ICollection<TKey>, ICollection
		{
			private readonly HashSet<TKey, TValue> _hashSet;
			internal KeyCollection(HashSet<TKey, TValue> hashSet)
			{
				this._hashSet = hashSet;
			}

			void ICollection<TKey>.Add(TKey item)
			{
				throw MutatingKeyCollectionNotSupportedException();
			}
			void ICollection<TKey>.Clear()
			{
				throw MutatingKeyCollectionNotSupportedException();
			}
			bool ICollection<TKey>.Remove(TKey item)
			{
				throw MutatingKeyCollectionNotSupportedException();
			}

			/// <summary>
			/// Determines whether the <see cref="HashSet{TKey,TValue}"/> contains at least one
			/// <typeparamref name="TValue"/> instance that is associated with the <typeparamref name="TKey"/>
			/// instance specified by <paramref name="item"/>.
			/// </summary>
			/// <param name="item">
			/// The <typeparamref name="TKey"/> instance for which an attempt to locate an associated
			/// <typeparamref name="TValue"/> instance should be made.
			/// </param>
			/// <returns>
			/// <see langword="true"/> if this <see cref="HashSet{TKey,TValue}"/> contains at least one
			/// <typeparamref name="TValue"/> instance that is associated with the <typeparamref name="TKey"/>
			/// instance specified by <paramref name="item"/>.
			/// </returns>
			/// <remarks>
			/// This method is equivalent to <see cref="HashSet{TKey,TValue}.ContainsKey"/>.
			/// </remarks>
			public bool Contains(TKey item)
			{
				return this._hashSet.ContainsKey(item);
			}

			/// <summary>
			/// Copies the keys for the contents of this <see cref="HashSet{TKey,TValue}"/> to a new <typeparamref name="TKey"/>
			/// array and returns it.
			/// </summary>
			/// <returns>
			/// A new <typeparamref name="TKey"/> array containing a copy of the keys for the contents of this
			/// <see cref="HashSet{TKey,TValue}"/>.
			/// </returns>
			public TKey[] ToArray()
			{
				TKey[] array = new TKey[this._hashSet._size];
				this.CopyToInternal(array, 0);
				return array;
			}
			/// <summary>
			/// Copies the <typeparamref name="TKey"/> instances of the <see cref="HashSet{TKey,TValue}"/> to the
			/// <typeparamref name="TKey"/> array specified by <paramref name="array"/>, starting at the array index
			/// specified by <paramref name="arrayIndex"/>.
			/// </summary>
			/// <param name="array">
			/// The <typeparamref name="TKey"/> array to which the contents of this <see cref="HashSet{TKey,TValue}"/>
			/// should be copied.
			/// </param>
			/// <param name="arrayIndex">
			/// The zero-based index in <paramref name="array"/> at which copying should begin.
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// <paramref name="array"/> is <see langword="null"/>.
			/// </exception>
			/// <exception cref="ArgumentOutOfRangeException">
			/// <paramref name="arrayIndex"/> is less than 0.
			/// </exception>
			/// <exception cref="ArgumentOutOfRangeException">
			/// <paramref name="arrayIndex"/> is greater than or equal to the length of <paramref name="array"/>.
			/// </exception>
			/// <exception cref="ArgumentException">
			/// The number of elements in the <see cref="HashSet{TKey,TValue}"/> is greater than the available space from
			/// <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
			/// </exception>
			public void CopyTo(TKey[] array, int arrayIndex)
			{
				this._hashSet.VerifyArrayForCopy(array, arrayIndex, true);
				this.CopyToInternal(array, arrayIndex);
			}
			private void CopyToInternal(TKey[] array, int arrayIndex)
			{
				IKeyProvider<TKey, TValue> keyProvider = this._hashSet._keyProvider;
				Entry[] entries = this._hashSet._entries;
				for (int i = 0; i < entries.Length; i++)
				{
					Entry currentEntry = entries[i];
					if (currentEntry != null)
					{
						do
						{
							array[arrayIndex++] = keyProvider.GetKey(currentEntry.Value);
						}
						while ((currentEntry = currentEntry.NextEntry) != null);
					}
				}
			}
			void ICollection.CopyTo(Array array, int index)
			{
				TKey[] keyArray = array as TKey[];
				if (keyArray != null)
				{
					this.CopyTo(keyArray, index);
				}
				else
				{
					this._hashSet.VerifyArrayForCopy(array, index, false);
					IKeyProvider<TKey, TValue> keyProvider = this._hashSet._keyProvider;
					Entry[] entries = this._hashSet._entries;
					for (int i = 0; i < entries.Length; i++)
					{
						Entry currentEntry = entries[i];
						if (currentEntry != null)
						{
							do
							{
								array.SetValue(keyProvider.GetKey(currentEntry.Value), index++);
							}
							while ((currentEntry = currentEntry.NextEntry) != null);
						}
					}
				}
			}

			/// <summary>
			/// Gets the number of <typeparamref name="TKey"/> instances in the <see cref="HashSet{TKey,TValue}"/>.
			/// </summary>
			/// <remarks>
			/// This method is equivalent to <see cref="HashSet{TKey,TValue}.Count"/>.
			/// </remarks>
			public int Count
			{
				get
				{
					return this._hashSet._size;
				}
			}

			/// <summary>
			/// Gets a value indicating whether this <see cref="KeyCollection"/> is read-only.
			/// </summary>
			/// <remarks>
			/// This property always returns <see langword="true"/>.
			/// </remarks>
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			object ICollection.SyncRoot
			{
				get
				{
					return this._hashSet;
				}
			}

			#region GetEnumerator methods
			/// <summary>
			/// Returns an <see cref="Enumerator.KeyEnumerator"/> that iterates through the <typeparamref name="TKey"/>
			/// instances in the <see cref="HashSet{TKey,TValue}"/>.
			/// </summary>
			/// <returns>
			/// An <see cref="Enumerator.KeyEnumerator"/> that iterates through the <typeparamref name="TKey"/>
			/// instances in the <see cref="HashSet{TKey,TValue}"/>.
			/// </returns>
			/// <remarks>
			/// This method is equivalent to <see cref="HashSet{TKey,TValue}.GetKeyEnumerator"/>.
			/// </remarks>
			public Enumerator.KeyEnumerator GetEnumerator()
			{
				return this._hashSet.GetKeyEnumerator();
			}
			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				return this._hashSet.GetKeyEnumerator();
			}
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._hashSet.GetKeyEnumerator();
			}
			#endregion // GetEnumerator methods
		}
		#endregion // KeyCollection struct

		/// <summary>
		/// Gets a <see cref="KeyCollection"/> containing the <typeparamref name="TKey"/> instances from
		/// this <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <remarks>
		/// Because the <typeparamref name="TKey"/> instances are not stored, the <see cref="KeyCollection"/>
		/// will derive them as needed.
		/// </remarks>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public KeyCollection Keys
		{
			get
			{
				return new KeyCollection(this);
			}
		}
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				return new KeyCollection(this);
			}
		}
		ICollection IDictionary.Keys
		{
			get
			{
				return new KeyCollection(this);
			}
		}
		#endregion // Keys properties

		#region Values properties
		/// <summary>
		/// Gets this <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public HashSet<TKey, TValue> Values
		{
			get
			{
				return this;
			}
		}
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				return this;
			}
		}
		ICollection IDictionary.Values
		{
			get
			{
				return this;
			}
		}
		#endregion // Values properties

		#region ICollection and IDictionary properties
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}
		#endregion // ICollection and IDictionary properties

		#endregion // Properties


		#region GetEnumerator methods and variants

		#region Enumerator structs
		/// <summary>
		/// Enumerates the <typeparamref name="TValue"/> instances of a <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		[Serializable]
		[StructLayout(LayoutKind.Auto)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public struct Enumerator : IEnumerator<TValue>, IEnumerable<TValue>
		{
			private readonly HashSet<TKey, TValue> _hashSet;
			private int _index;
			private Entry _currentEntry;

			internal Enumerator(HashSet<TKey, TValue> hashSet)
			{
				this._hashSet = hashSet;
				this._index = -1;
				this._currentEntry = null;
			}

			#region Reset method
			/// <summary>
			/// Sets this <see cref="Enumerator"/> to its initial position,
			/// which is before the first element in the collection.
			/// </summary>
			/// <seealso cref="IEnumerator.Reset"/>
			public void Reset()
			{
				this._index = -1;
				this._currentEntry = null;
			}
			#endregion // Reset method

			#region MoveNext method
			/// <summary>
			/// Advances this <see cref="Enumerator"/> to the next element of the collection.
			/// </summary>
			/// <seealso cref="IEnumerator.MoveNext"/>
			public bool MoveNext()
			{
				Entry currentEntry = this._currentEntry;
				if (currentEntry != null && (currentEntry = currentEntry.NextEntry) != null)
				{
					this._currentEntry = currentEntry;
					return true;
				}
				else
				{
					Entry[] entries = this._hashSet._entries;
					for (int i = this._index + 1; i < entries.Length; i++)
					{
						if ((currentEntry = entries[i]) != null)
						{
							this._currentEntry = currentEntry;
							this._index = i;
							return true;
						}
					}
				}
				this._currentEntry = null;
				return false;
			}
			#endregion // MoveNext method

			#region Current properties
			/// <summary>
			/// Gets the <typeparamref name="TValue"/> instance at the current position of this <see cref="Enumerator"/>.
			/// </summary>
			public TValue Current
			{
				get
				{
					Entry currentEntry = this._currentEntry;
					if (currentEntry == null)
					{
						throw new InvalidOperationException();
					}
					return currentEntry.Value;
				}
			}
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}
			#endregion // Current properties

			#region Dispose method
			/// <summary>Does nothing.</summary>
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void Dispose()
			{
				// Do nothing
			}
			#endregion // Dispose method

			#region GetEnumerator methods
			/// <summary>
			/// Returns an <see cref="Enumerator"/> that iterates through the <typeparamref name="TValue"/>
			/// instances in the same <see cref="HashSet{TKey,TValue}"/> as this <see cref="Enumerator"/>.
			/// </summary>
			/// <returns>
			/// An <see cref="Enumerator"/> that iterates through the <typeparamref name="TValue"/>
			/// instances in the same <see cref="HashSet{TKey,TValue}"/> as this <see cref="Enumerator"/>.
			/// </returns>
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			public Enumerator GetEnumerator()
			{
				Enumerator enumerator = this;
				enumerator.Reset();
				return enumerator;
			}
			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				return this.GetEnumerator();
			}
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
			#endregion // GetEnumerator methods

			#region KeyEnumerator struct
			/// <summary>
			/// Enumerates the <typeparamref name="TKey"/> instances of a <see cref="HashSet{TKey,TValue}"/>.
			/// </summary>
			[Serializable]
			[StructLayout(LayoutKind.Auto)]
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			public struct KeyEnumerator : IEnumerator<TKey>, IEnumerable<TKey>
			{
				private Enumerator _enumerator;
				internal KeyEnumerator(HashSet<TKey, TValue> hashSet)
				{
					this._enumerator = new Enumerator(hashSet);
				}
				/// <summary>
				/// Gets the <typeparamref name="TKey"/> instance at the current position of this <see cref="KeyEnumerator"/>.
				/// </summary>
				public TKey Current
				{
					get
					{
						return this._enumerator._hashSet._keyProvider.GetKey(this._enumerator.Current);
					}
				}
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}
				/// <summary>Does nothing.</summary>
				[EditorBrowsable(EditorBrowsableState.Never)]
				public void Dispose()
				{
					// Do nothing
				}
				/// <summary>
				/// Advances this <see cref="KeyEnumerator"/> to the next element of the collection.
				/// </summary>
				/// <seealso cref="IEnumerator.MoveNext"/>
				public bool MoveNext()
				{
					return this._enumerator.MoveNext();
				}
				/// <summary>
				/// Sets this <see cref="KeyEnumerator"/> to its initial position,
				/// which is before the first element in the collection.
				/// </summary>
				/// <seealso cref="IEnumerator.Reset"/>
				public void Reset()
				{
					this._enumerator.Reset();
				}
				#region GetEnumerator methods
				/// <summary>
				/// Returns a <see cref="KeyEnumerator"/> that iterates through the <typeparamref name="TKey"/>
				/// instances in the same <see cref="HashSet{TKey,TValue}"/> as this <see cref="KeyEnumerator"/>.
				/// </summary>
				/// <returns>
				/// A <see cref="KeyEnumerator"/> that iterates through the <typeparamref name="TKey"/>
				/// instances in the same <see cref="HashSet{TKey,TValue}"/> as this <see cref="KeyEnumerator"/>.
				/// </returns>
				[EditorBrowsable(EditorBrowsableState.Advanced)]
				public KeyEnumerator GetEnumerator()
				{
					KeyEnumerator enumerator = this;
					enumerator._enumerator.Reset();
					return enumerator;
				}
				IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
				{
					return this.GetEnumerator();
				}
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}
				#endregion // GetEnumerator methods
			}
			#endregion // KeyEnumerator struct

			#region KeyValuePairEnumerator struct
			/// <summary>
			/// Enumerates the <see cref="KeyValuePair{TKey,TValue}"/> instances of a <see cref="HashSet{TKey,TValue}"/>.
			/// </summary>
			[Serializable]
			[StructLayout(LayoutKind.Auto)]
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			public struct KeyValuePairEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerator<DictionaryEntry>, IEnumerable<DictionaryEntry>, IDictionaryEnumerator
			{
				private Enumerator _enumerator;
				internal KeyValuePairEnumerator(HashSet<TKey, TValue> hashSet)
				{
					this._enumerator = new Enumerator(hashSet);
				}
				/// <summary>
				/// Gets the <see cref="KeyValuePair{TKey,TValue}"/> instance at the current position of this <see cref="KeyValuePairEnumerator"/>.
				/// </summary>
				public KeyValuePair<TKey, TValue> Current
				{
					get
					{
						return new KeyValuePair<TKey, TValue>(this._enumerator._hashSet._keyProvider.GetKey(this._enumerator.Current), this._enumerator.Current);
					}
				}
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}
				/// <summary>
				/// Gets the <see cref="DictionaryEntry"/> instance at the current position of this <see cref="KeyValuePairEnumerator"/>.
				/// </summary>
				[EditorBrowsable(EditorBrowsableState.Advanced)]
				public DictionaryEntry Entry
				{
					get
					{
						return new DictionaryEntry(this._enumerator._hashSet._keyProvider.GetKey(this._enumerator.Current), this._enumerator.Current);
					}
				}
				DictionaryEntry IEnumerator<DictionaryEntry>.Current
				{
					get
					{
						return this.Entry;
					}
				}
				/// <summary>
				/// Gets the <typeparamref name="TKey"/> instance at the current position of this <see cref="KeyValuePairEnumerator"/>.
				/// </summary>
				[EditorBrowsable(EditorBrowsableState.Advanced)]
				public TKey Key
				{
					get
					{
						return this._enumerator._hashSet._keyProvider.GetKey(this._enumerator.Current);
					}
				}
				/// <summary>
				/// Gets the <typeparamref name="TValue"/> instance at the current position of this <see cref="KeyValuePairEnumerator"/>.
				/// </summary>
				[EditorBrowsable(EditorBrowsableState.Advanced)]
				public TValue Value
				{
					get
					{
						return this._enumerator.Current;
					}
				}
				object IDictionaryEnumerator.Key
				{
					get
					{
						return this.Key;
					}
				}
				object IDictionaryEnumerator.Value
				{
					get
					{
						return this.Value;
					}
				}
				/// <summary>Does nothing.</summary>
				[EditorBrowsable(EditorBrowsableState.Never)]
				public void Dispose()
				{
					// Do nothing
				}
				/// <summary>
				/// Advances this <see cref="KeyValuePairEnumerator"/> to the next element of the collection.
				/// </summary>
				/// <seealso cref="IEnumerator.MoveNext"/>
				public bool MoveNext()
				{
					return this._enumerator.MoveNext();
				}
				/// <summary>
				/// Sets this <see cref="KeyValuePairEnumerator"/> to its initial position,
				/// which is before the first element in the collection.
				/// </summary>
				/// <seealso cref="IEnumerator.Reset"/>
				public void Reset()
				{
					this._enumerator.Reset();
				}
				#region GetEnumerator methods
				/// <summary>
				/// Returns a <see cref="KeyValuePairEnumerator"/> that iterates through the <see cref="KeyValuePair{TKey,TValue}"/>
				/// instances in the same <see cref="HashSet{TKey,TValue}"/> as this <see cref="KeyValuePairEnumerator"/>.
				/// </summary>
				/// <returns>
				/// A <see cref="KeyValuePairEnumerator"/> that iterates through the <see cref="KeyValuePair{TKey,TValue}"/>
				/// instances in the same <see cref="HashSet{TKey,TValue}"/> as this <see cref="KeyValuePairEnumerator"/>.
				/// </returns>
				[EditorBrowsable(EditorBrowsableState.Advanced)]
				public KeyValuePairEnumerator GetEnumerator()
				{
					KeyValuePairEnumerator enumerator = this;
					enumerator._enumerator.Reset();
					return enumerator;
				}
				IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
				{
					return this.GetEnumerator();
				}
				IEnumerator<DictionaryEntry> IEnumerable<DictionaryEntry>.GetEnumerator()
				{
					return this.GetEnumerator();
				}
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}
				#endregion // GetEnumerator methods
			}
			#endregion // KeyValuePairEnumerator struct

			#region DictionaryEnumerator struct
			/// <summary>
			/// Enumerates the <see cref="DictionaryEntry"/> instances of a <see cref="HashSet{TKey,TValue}"/>.
			/// </summary>
			[Serializable]
			[StructLayout(LayoutKind.Auto)]
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			public struct DictionaryEnumerator : IEnumerator<DictionaryEntry>, IEnumerable<DictionaryEntry>, IDictionaryEnumerator
			{
				private Enumerator _enumerator;
				internal DictionaryEnumerator(HashSet<TKey, TValue> hashSet)
				{
					this._enumerator = new Enumerator(hashSet);
				}
				/// <summary>
				/// Gets the <see cref="DictionaryEntry"/> instance at the current position of this <see cref="DictionaryEnumerator"/>.
				/// </summary>
				public DictionaryEntry Current
				{
					get
					{
						return new DictionaryEntry(this._enumerator._hashSet._keyProvider.GetKey(this._enumerator.Current), this._enumerator.Current);
					}
				}
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}
				DictionaryEntry IDictionaryEnumerator.Entry
				{
					get
					{
						return this.Current;
					}
				}
				object IDictionaryEnumerator.Key
				{
					get
					{
						return this._enumerator._hashSet._keyProvider.GetKey(this._enumerator.Current);
					}
				}
				object IDictionaryEnumerator.Value
				{
					get
					{
						return this._enumerator.Current;
					}
				}
				/// <summary>Does nothing.</summary>
				[EditorBrowsable(EditorBrowsableState.Never)]
				public void Dispose()
				{
					// Do nothing
				}
				/// <summary>
				/// Advances this <see cref="DictionaryEnumerator"/> to the next element of the collection.
				/// </summary>
				/// <seealso cref="IEnumerator.MoveNext"/>
				public bool MoveNext()
				{
					return this._enumerator.MoveNext();
				}
				/// <summary>
				/// Sets this <see cref="DictionaryEnumerator"/> to its initial position,
				/// which is before the first element in the collection.
				/// </summary>
				/// <seealso cref="IEnumerator.Reset"/>
				public void Reset()
				{
					this._enumerator.Reset();
				}
				#region GetEnumerator methods
				/// <summary>
				/// Returns a <see cref="DictionaryEnumerator"/> that iterates through the <see cref="DictionaryEntry"/>
				/// instances in the same <see cref="HashSet{TKey,TValue}"/> as this <see cref="DictionaryEnumerator"/>.
				/// </summary>
				/// <returns>
				/// A <see cref="DictionaryEnumerator"/> that iterates through the <see cref="DictionaryEntry"/>
				/// instances in the same <see cref="HashSet{TKey,TValue}"/> as this <see cref="DictionaryEnumerator"/>.
				/// </returns>
				[EditorBrowsable(EditorBrowsableState.Advanced)]
				public DictionaryEnumerator GetEnumerator()
				{
					DictionaryEnumerator enumerator = this;
					enumerator._enumerator.Reset();
					return enumerator;
				}
				IEnumerator<DictionaryEntry> IEnumerable<DictionaryEntry>.GetEnumerator()
				{
					return this.GetEnumerator();
				}
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}
				#endregion // GetEnumerator methods
			}
			#endregion // DictionaryEnumerator struct
		}

		#endregion // Enumerator structs

		/// <summary>
		/// Returns an <see cref="Enumerator"/> that iterates through the <typeparamref name="TValue"/>
		/// instances in this <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <returns>
		/// An <see cref="Enumerator"/> that iterates through the <typeparamref name="TValue"/>
		/// instances in this <see cref="HashSet{TKey,TValue}"/>.
		/// </returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}
		IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
		{
			return new Enumerator(this);
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}
		/// <summary>
		/// Returns an <see cref="Enumerator.KeyEnumerator"/> that iterates through the <typeparamref name="TKey"/>
		/// instances in this <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <returns>
		/// An <see cref="Enumerator.KeyEnumerator"/> that iterates through the <typeparamref name="TKey"/>
		/// instances in this <see cref="HashSet{TKey,TValue}"/>.
		/// </returns>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public Enumerator.KeyEnumerator GetKeyEnumerator()
		{
			return new Enumerator.KeyEnumerator(this);
		}
		/// <summary>
		/// Returns an <see cref="Enumerator.KeyValuePairEnumerator"/> that iterates through the <see cref="KeyValuePair{TKey,TValue}"/>
		/// instances in this <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <returns>
		/// An <see cref="Enumerator.KeyValuePairEnumerator"/> that iterates through the <see cref="KeyValuePair{TKey,TValue}"/>
		/// instances in this <see cref="HashSet{TKey,TValue}"/>.
		/// </returns>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public Enumerator.KeyValuePairEnumerator GetKeyValuePairEnumerator()
		{
			return new Enumerator.KeyValuePairEnumerator(this);
		}
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new Enumerator.KeyValuePairEnumerator(this);
		}
		/// <summary>
		/// Returns an <see cref="Enumerator.DictionaryEnumerator"/> that iterates through the <see cref="DictionaryEntry"/>
		/// instances in this <see cref="HashSet{TKey,TValue}"/>.
		/// </summary>
		/// <returns>
		/// An <see cref="Enumerator.DictionaryEnumerator"/> that iterates through the <see cref="DictionaryEntry"/>
		/// instances in this <see cref="HashSet{TKey,TValue}"/>.
		/// </returns>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public Enumerator.DictionaryEnumerator GetDictionaryEnumerator()
		{
			return new Enumerator.DictionaryEnumerator(this);
		}
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new Enumerator.DictionaryEnumerator(this);
		}
		#endregion // GetEnumerator methods and variants
	}
	#endregion // HashSet class
}
