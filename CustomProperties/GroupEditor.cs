using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Neumont.Tools.ORM.CustomProperties
{
	public partial class GroupEditor : UserControl
	{
		#region Fields
		private XmlNode _groupNode;
		private CustomPropertyGroup _groupObject;
		private bool _loadingGroup;
		#endregion
		#region Events
		/// <summary>
		/// Raised when the name of the group has been changed.
		/// </summary>
		public event NameChangedHandler NameChanged;
		#endregion
		#region Constructors
		public GroupEditor()
		{
			InitializeComponent();
		}
		#endregion
		#region Properties
		public XmlNode GroupNode
		{
			get
			{
				return _groupNode;
			}
			set
			{
				_groupNode = value;
				_groupObject = null;
				PopulateBoxes();
			}
		}
		internal CustomPropertyGroup GroupObject
		{
			get
			{
				return _groupObject;
			}
			set
			{
				_groupObject = value;
				_groupNode = null;
				PopulateBoxes();
			}
		}
		#endregion
		#region Event Handlers
		private void tbxTextChanged(object sender, EventArgs e)
		{
			TextBox tbox = sender as TextBox;
			string attribName = (string)tbox.Tag;
			UpdateAttribute(attribName, tbox.Text);
		}
		private void btnEditDescription_Click(object sender, EventArgs e)
		{
			string desc = tbxDescription.Text;
			if (EditCustomEnumOrDescription.EditDescription(ref desc))
			{
				tbxDescription.Text = desc;
			}
		}
		#endregion
		#region Methods
		private void PopulateBoxes()
		{
			_loadingGroup = true;

			if (_groupNode != null)
			{
				tbxName.Text = _groupNode.Attributes["name"].InnerText;
				tbxDescription.Text = _groupNode.Attributes["description"] == null ? string.Empty : _groupNode.Attributes["description"].InnerText;
			}

			if (_groupObject != null)
			{
				tbxName.Text = _groupObject.Name;
				tbxDescription.Text = _groupObject.Description;
			}

			_loadingGroup = false;
		}
		private void UpdateAttribute(string attributeName, string newValue)
		{
			if (_loadingGroup)
			{
				return;
			}
			if (_groupNode != null)
			{
				_groupNode.Attributes[attributeName].Value = newValue;
			}
			if (_groupObject != null)
			{
				switch (attributeName)
				{
					case "name":
						_groupObject.Name = newValue;
						break;
					case "description":
						_groupObject.Description = newValue;
						break;
				}
			}
			if (attributeName == "name" && NameChanged != null)
			{
				NameChanged(this, new NameChangedEventArgs(newValue));
			}
		}
		#endregion
	}
}
