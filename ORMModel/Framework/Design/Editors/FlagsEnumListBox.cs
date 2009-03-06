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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ORMSolutions.ORMArchitect.Framework.Design
{
	/// <summary>
	/// A <see cref="Control"/> for editing <see cref="Enum"/> values that have the <see cref="FlagsAttribute"/>
	/// applied to them.
	/// </summary>
	/// <remarks>
	/// Directly altering the collection returned by <see cref="CheckedListBox.Items"/> or <see cref="ListBox.Items"/>
	/// is not supported; the results if this is done are undefined.
	/// </remarks>
	public class FlagsEnumListBox : CheckedListBox
	{
		/// <summary>
		/// Initializes a new instance of <see cref="FlagsEnumListBox"/>.
		/// </summary>
		public FlagsEnumListBox()
			: base()
		{
		}

		#region Unsupported methods and properties
		/// <summary>
		/// This property is not relevant for this class, and will always throw a <see cref="NotSupportedException"/>.
		/// </summary>
		/// <exception cref="NotSupportedException"/>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new object Items
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}
		/// <summary>
		/// This method is not relevant for this class, and will always throw a <see cref="NotSupportedException"/>.
		/// </summary>
		/// <exception cref="NotSupportedException"/>
		/// <seealso cref="ListBox.AddItemsCore"/>
		[Obsolete]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected sealed override void AddItemsCore(object[] value)
		{
			throw new NotSupportedException();
		}
		/// <summary>
		/// This method is not relevant for this class, and will always throw a <see cref="NotSupportedException"/>.
		/// </summary>
		/// <exception cref="NotSupportedException"/>
		/// <seealso cref="ListBox.SetItemCore"/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected sealed override void SetItemCore(int index, object value)
		{
			throw new NotSupportedException();
		}
		/// <summary>
		/// This method is not relevant for this class, and will always throw a <see cref="NotSupportedException"/>.
		/// </summary>
		/// <exception cref="NotSupportedException"/>
		/// <seealso cref="ListBox.SetItemsCore"/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected sealed override void SetItemsCore(System.Collections.IList value)
		{
			throw new NotSupportedException();
		}
		#endregion // Unsupported methods and properties

		#region Update tracking support
		private int _updateCount;
		private new void BeginUpdate()
		{
			this._updateCount++;
			base.BeginUpdate();
		}
		private new void EndUpdate()
		{
			base.EndUpdate();
			this._updateCount--;
		}
		private bool IsUpdating
		{
			get
			{
				return this._updateCount > 0;
			}
		}
		#endregion // Update tracking support

		private Type _enumType;
		private ulong _value;
		/// <summary>
		/// Gets or sets the <see cref="Enum"/> value being edited.
		/// </summary>
		public object Value
		{
			get
			{
				Type enumType = this._enumType;
				return (enumType != null) ? Enum.ToObject(enumType, this._value) : null;
			}
			set
			{
				if (value != null)
				{
					Type enumType = value.GetType();
					if (!enumType.IsEnum || !enumType.IsDefined(typeof(FlagsAttribute), false))
					{
						throw new InvalidEnumArgumentException();
					}
					this._enumType = enumType;
					this._value = Convert.ToUInt64(value);
				}
				else
				{
					this._enumType = null;
					this._value = 0;
				}
				this.PopulateItems();
			}
		}

		private CheckState GetCheckState(object enumValue)
		{
			ulong requestedValue = Convert.ToUInt64(enumValue);
			ulong actualValue = requestedValue & this._value;
			return (actualValue == requestedValue) ? CheckState.Checked :
				(actualValue != 0) ? CheckState.Indeterminate : CheckState.Unchecked;
		}


		private void PopulateItems()
		{
			try
			{
				this.BeginUpdate();
				ObjectCollection items = base.Items;
				items.Clear();
				Type enumType = this._enumType;
				if (enumType != null)
				{
					foreach (object enumValue in Enum.GetValues(enumType))
					{
						items.Add(enumValue, this.GetCheckState(enumValue));
					}
				}
			}
			finally
			{
				this.EndUpdate();
			}
		}

		/// <summary>See <see cref="CheckedListBox.RefreshItems"/>.</summary>
		protected override void RefreshItems()
		{
			try
			{
				this.BeginUpdate();
				base.RefreshItems();
				this.RefreshCheckStates();
			}
			finally
			{
				this.EndUpdate();
			}
		}

		private void RefreshCheckStates()
		{
			try
			{
				this.BeginUpdate();
				ObjectCollection items = base.Items;
				int itemsCount = items.Count;
				for (int i = 0; i < itemsCount; i++)
				{
					CheckState newCheckState = this.GetCheckState(items[i]);
					if (newCheckState != base.GetItemCheckState(i))
					{
						base.SetItemCheckState(i, newCheckState);
						this.RefreshItem(i);
					}
				}
			}
			finally
			{
				this.EndUpdate();
			}
		}

		/// <summary>See <see cref="CheckedListBox.OnItemCheck"/>.</summary>
		protected override void OnItemCheck(ItemCheckEventArgs ice)
		{
			CheckState currentValue = ice.CurrentValue;
			CheckState newValue = ice.NewValue;
			if (newValue == currentValue)
			{
				return;
			}
			if (this.IsUpdating)
			{
				// If we're doing this during our own update, let the user get the notification...
				base.OnItemCheck(ice);
				
				// NOTE: If the user changes other checks states from their event handler, it could potentially
				// mess things up. However, in that case, they're on their own.

				// ... but don't let them change what the new value is supposed to be.
				ice.NewValue = newValue;
				return;
			}

			if (newValue == CheckState.Indeterminate)
			{
				// Don't allow the CheckState to be changed to Indeterminate
				ice.NewValue = currentValue;
				return;
			}

			base.OnItemCheck(ice);

			// We need to check again in case any user-registered handlers changed the new value
			newValue = ice.NewValue;
			if (newValue == currentValue)
			{
				return;
			}

			ulong itemValue = Convert.ToUInt64(base.Items[ice.Index]);
			switch (newValue)
			{
				case CheckState.Unchecked:
					this._value &= ~itemValue;
					break;
				case CheckState.Checked:
					this._value |= itemValue;
					break;
				case CheckState.Indeterminate:
					// Don't allow the CheckState to be changed to Indeterminate
					ice.NewValue = currentValue;
					return;
			}
			this.RefreshCheckStates();
		}
	}
}
