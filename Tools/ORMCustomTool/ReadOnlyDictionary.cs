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

namespace Neumont.Tools.ORM.ORMCustomTool
{
	public sealed partial class ORMCustomTool
	{
		private sealed class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
		{
			private readonly IDictionary<TKey, TValue> _backingDictionary;
			public ReadOnlyDictionary(IDictionary<TKey, TValue> backingDictionary)
			{
				this._backingDictionary = backingDictionary;
			}

			public void Add(TKey key, TValue value)
			{
				throw new NotSupportedException();
			}

			public bool ContainsKey(TKey key)
			{
				return this._backingDictionary.ContainsKey(key);
			}

			public ICollection<TKey> Keys
			{
				get { return this._backingDictionary.Keys; }
			}

			public bool Remove(TKey key)
			{
				throw new NotSupportedException();
			}

			public bool TryGetValue(TKey key, out TValue value)
			{
				return this._backingDictionary.TryGetValue(key, out value);
			}

			public ICollection<TValue> Values
			{
				get { return this._backingDictionary.Values; }
			}

			public TValue this[TKey key]
			{
				get
				{
					return this._backingDictionary[key];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			public void Add(KeyValuePair<TKey, TValue> item)
			{
				throw new NotSupportedException();
			}

			public void Clear()
			{
				throw new NotSupportedException();
			}

			public bool Contains(KeyValuePair<TKey, TValue> item)
			{
				return this._backingDictionary.Contains(item);
			}

			public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
			{
				this._backingDictionary.CopyTo(array, arrayIndex);
			}

			public int Count
			{
				get { return this._backingDictionary.Count; }
			}

			public bool IsReadOnly
			{
				get { return true; }
			}

			public bool Remove(KeyValuePair<TKey, TValue> item)
			{
				throw new NotSupportedException();
			}

			public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
			{
				return this._backingDictionary.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return ((System.Collections.IEnumerable)this._backingDictionary).GetEnumerator();
			}
		}
	}
}
