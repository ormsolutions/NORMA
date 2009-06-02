using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ORMSolutions.ORMArchitect.CustomProperties
{
	public partial class DefinitionEditor : UserControl
	{
		#region Fields
		private XmlNode _definitionNode;
		private CustomPropertyDefinition _definitionObject;
		private bool _loadingDefinition;
		private bool _alteringAllConstraintsCheck;
		#endregion
		#region Events
		/// <summary>
		/// Raised when the name of the definition has been changed.
		/// </summary>
		public event NameChangedHandler NameChanged;
		#endregion
		#region Constructors
		public DefinitionEditor()
		{
			InitializeComponent();
		}
		#endregion
		#region Properties
		internal XmlNode DefinitionNode
		{
			get
			{
				return _definitionNode;
			}
			set
			{
				_definitionNode = value;
				_definitionObject = null;
				PopulateBoxes();
			}
		}
		internal CustomPropertyDefinition DefinitionObject
		{
			get
			{
				return _definitionObject;
			}
			set
			{
				_definitionObject = value;
				_definitionNode = null;
				PopulateBoxes();
			}
		}
		#endregion
		#region Event Handlers
		private void btnEditCustomEnum_Click(object sender, EventArgs e)
		{
			if (_definitionNode != null)
			{
				List<string> enums = new List<string>();
				XmlNodeList nodes = _definitionNode.SelectNodes("def:CustomEnumValues/def:CustomEnumValue", CustomPropertiesManager.NamespaceManager);
				foreach (XmlNode customEnumValue in nodes)
				{
					enums.Add(customEnumValue.Attributes["value"].Value);
				}
				if (EditCustomEnumOrDescription.EditEnum(enums))
				{
					XmlNode valuesCol = _definitionNode.SelectSingleNode("def:CustomEnumValues", CustomPropertiesManager.NamespaceManager);
					if (valuesCol == null)
					{
						valuesCol = CustomPropertiesManager.LoadedDocument.CreateElement("CustomEnumValues", CustomPropertiesDomainModel.XmlNamespace);
						_definitionNode.AppendChild(valuesCol);
					}
					else
					{
						valuesCol.RemoveAll();
					}

					tbxCustomEnum.Clear();
					tbxCustomEnum.Text = string.Join("; ", enums.ToArray());

					foreach (string newValue in enums)
					{
						XmlNode newNode = CustomPropertiesManager.LoadedDocument.CreateElement("CustomEnumValue", CustomPropertiesDomainModel.XmlNamespace);
						XmlAttribute valueAttrib = CustomPropertiesManager.LoadedDocument.CreateAttribute("value");
						newNode.Attributes.Append(valueAttrib);

						valueAttrib.Value = newValue;

						valuesCol.AppendChild(newNode);
					}
				}
			}

			if (_definitionObject != null)
			{
				List<string> values = new List<string>();
				values.AddRange(_definitionObject.CustomEnumValue.Split('\0'));
				if (EditCustomEnumOrDescription.EditEnum(values))
				{
					_definitionObject.CustomEnumValue = string.Join("\0", values.ToArray());
					tbxCustomEnum.Text = string.Join("; ", values.ToArray());
				}
			}
		}
		private void btnEditDescription_Click(object sender, EventArgs e)
		{
			string desc = tbxDescription.Text;
			if (EditCustomEnumOrDescription.EditDescription(ref desc))
			{
				tbxDescription.Text = desc;
			}
		}
		private void cmbxDataType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loadingDefinition)
			{
				return;
			}
			int index = cmbxDataType.FindString("CustomEnumeration");
			bool customEnum = cmbxDataType.SelectedIndex == index;

			tbxCustomEnum.Enabled = customEnum;
			btnEditCustomEnum.Enabled = customEnum;

			if (!customEnum)
			{
				if (_definitionNode != null)
				{
					XmlNode customEnumValues = _definitionNode.SelectSingleNode("def:CustomEnumValues", CustomPropertiesManager.NamespaceManager);
					if (customEnumValues != null)
					{
						_definitionNode.RemoveChild(customEnumValues);
					}
					tbxCustomEnum.Clear();
				}
			}

			UpdateAttribute("dataType", cmbxDataType.Text);
		}
		private void tvModelElements_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (_loadingDefinition)
			{
				return;
			}

			if ((string)e.Node.Tag == "AllConstraints")
			{
				if (!_alteringAllConstraintsCheck)
				{
					_alteringAllConstraintsCheck = true;
					foreach (TreeNode node in e.Node.Nodes)
					{
						node.Checked = e.Node.Checked;
					}
					_alteringAllConstraintsCheck = false;
				}
			}
			else if (!e.Node.Checked && ((string)e.Node.Tag).IndexOf("Constraint") > -1 && !_alteringAllConstraintsCheck)
			{
				if (e.Node.Parent.Checked)
				{
					_alteringAllConstraintsCheck = true;
					e.Node.Parent.Checked = false;
					_alteringAllConstraintsCheck = false;
				}
			}


			if (_definitionNode != null)
			{
				if (e.Node.Checked)
				{
					XmlNode newType = CustomPropertiesManager.LoadedDocument.CreateNode("element", "ORMType", CustomPropertiesDomainModel.XmlNamespace);
					XmlAttribute nameAttrib = CustomPropertiesManager.LoadedDocument.CreateAttribute("name");
					nameAttrib.Value = (string)e.Node.Tag;
					_definitionNode.SelectSingleNode("def:ORMTypes", CustomPropertiesManager.NamespaceManager).AppendChild(newType);
					newType.Attributes.Append(nameAttrib);
				}
				else
				{
					XmlNode node = _definitionNode.SelectSingleNode("def:ORMTypes/def:ORMType[@name='" + (string)e.Node.Tag + "']", CustomPropertiesManager.NamespaceManager);
					node.ParentNode.RemoveChild(node);
				}
			}
			if (_definitionObject != null)
			{
				ORMTypes typeChanged = (ORMTypes)Enum.Parse(typeof(ORMTypes), (string)e.Node.Tag);
				if (e.Node.Checked)
				{
					_definitionObject.ORMTypes = _definitionObject.ORMTypes | typeChanged;
				}
				else
				{
					_definitionObject.ORMTypes = _definitionObject.ORMTypes ^ typeChanged;
				}
			}
		}
		private void tbx_TextChanged(object sender, EventArgs e)
		{
			if (_loadingDefinition)
			{
				return;
			}

			TextBox tbox = sender as TextBox;
			string attribName = (string)tbox.Tag;
			string text = tbox.Text;
			if (attribName == "defaultValue")
			{
				chkVerbalizeDefaultValue.Enabled = text.Length != 0;
			}
			UpdateAttribute(attribName, text);
		}
		private void chkVerbalizeDefaultValue_CheckedChanged(object sender, EventArgs e)
		{
			if (_loadingDefinition)
			{
				return;
			}
			CheckBox checkbox = (CheckBox)sender;
			UpdateAttribute((string)checkbox.Tag, checkbox.Checked ? "true" : "false");
		}
		#endregion
		#region Methods
		private void PopulateBoxes()
		{
			if (_definitionNode == null && _definitionObject == null)
			{
				return;
			}
			PopulateTreeView();
			_loadingDefinition = true;

			if (_definitionNode != null)
			{
				XmlAttributeCollection attributes = _definitionNode.Attributes;
				tbxName.Text = attributes["name"].Value;
				tbxDescription.Text = attributes["description"].Value;
				cmbxDataType.SelectedIndex = cmbxDataType.FindString(attributes["dataType"].Value);

				XmlAttribute defaultValueAttribute = attributes["defaultValue"];
				if (defaultValueAttribute != null)
				{
					tbxDefaultValue.Text = defaultValueAttribute.Value;
					chkVerbalizeDefaultValue.Enabled = true;
				}
				else
				{
					tbxDefaultValue.Clear();
					chkVerbalizeDefaultValue.Enabled = false;
				}

				XmlAttribute verbalizeDefaultAttribute = attributes["verbalizeDefaultValue"];
				if (verbalizeDefaultAttribute != null)
				{
					string verbalizeDefaultText = verbalizeDefaultAttribute.Value.Trim();
					chkVerbalizeDefaultValue.Checked = verbalizeDefaultText == "true" || verbalizeDefaultText == "1";
				}
				else
				{
					chkVerbalizeDefaultValue.Checked = true;
				}

				tbxCategory.Text = attributes["category"].Value;

				tbxCustomEnum.Clear();
				XmlNodeList enums = _definitionNode.SelectNodes("def:CustomEnumValues/def:CustomEnumValue", CustomPropertiesManager.NamespaceManager);
				foreach (XmlNode en in enums)
				{
					tbxCustomEnum.Text += en.Attributes["value"].Value + "; ";
				}

				ClearCheckedItems(tvModelElements.Nodes);
				XmlNodeList ormTypes = _definitionNode.SelectNodes("def:ORMTypes/def:ORMType", CustomPropertiesManager.NamespaceManager);
				foreach (XmlNode ormType in ormTypes)
				{
					string typeName = ormType.Attributes["name"].Value;
					CheckTreeNodeItem(tvModelElements.Nodes, typeName);
				}
			}
			if (_definitionObject != null)
			{
				tbxName.Text = _definitionObject.Name;
				tbxDescription.Text = _definitionObject.Description;
				tbxCategory.Text = _definitionObject.Category;
				cmbxDataType.SelectedIndex = cmbxDataType.FindString(_definitionObject.DataType.ToString());
				object defaultValue = _definitionObject.DefaultValue;
				string defaultText = defaultValue == null ? string.Empty : defaultValue.ToString();
				tbxDefaultValue.Text = defaultText;
				chkVerbalizeDefaultValue.Checked = _definitionObject.VerbalizeDefaultValue;
				chkVerbalizeDefaultValue.Enabled = defaultText.Length != 0;
				ClearCheckedItems(tvModelElements.Nodes);
				CheckTypeIfNeeded(ORMTypes.EntityType);
				CheckTypeIfNeeded(ORMTypes.FactType);
				CheckTypeIfNeeded(ORMTypes.Role);
				CheckTypeIfNeeded(ORMTypes.AllConstraints);
				CheckTypeIfNeeded(ORMTypes.SubtypeFact);
				CheckTypeIfNeeded(ORMTypes.ValueType);
				CheckTypeIfNeeded(ORMTypes.EqualityConstraint);
				CheckTypeIfNeeded(ORMTypes.ExclusionConstraint);
				CheckTypeIfNeeded(ORMTypes.FrequencyConstraint);
				CheckTypeIfNeeded(ORMTypes.MandatoryConstraint);
				CheckTypeIfNeeded(ORMTypes.RingConstraint);
				CheckTypeIfNeeded(ORMTypes.SubsetConstraint);
				CheckTypeIfNeeded(ORMTypes.UniquenessConstraint);
				CheckTypeIfNeeded(ORMTypes.ValueConstraint);
			}

			_loadingDefinition = false;
		}
		private void PopulateTreeView()
		{
			tvModelElements.Nodes.Clear();

			tvModelElements.Nodes.Add(CreateTreeNode("Entity Type", "EntityType"));
			tvModelElements.Nodes.Add(CreateTreeNode("Value Type", "ValueType"));
			tvModelElements.Nodes.Add(CreateTreeNode("Fact Type", "FactType"));
			tvModelElements.Nodes.Add(CreateTreeNode("Subtype Fact", "SubtypeFact"));
			tvModelElements.Nodes.Add(CreateTreeNode("Role", "Role"));

			TreeNode constraintsNode = CreateTreeNode("All Constraints", "AllConstraints");
			tvModelElements.Nodes.Add(constraintsNode);

			constraintsNode.Nodes.Add(CreateTreeNode("Frequency Constraint", "FrequencyConstraint"));
			constraintsNode.Nodes.Add(CreateTreeNode("Mandatory Constraint", "MandatoryConstraint"));
			constraintsNode.Nodes.Add(CreateTreeNode("Ring Constraint", "RingConstraint"));
			constraintsNode.Nodes.Add(CreateTreeNode("Uniqueness Constraint", "UniquenessConstraint"));
			constraintsNode.Nodes.Add(CreateTreeNode("Equality Constraint", "EqualityConstraint"));
			constraintsNode.Nodes.Add(CreateTreeNode("Exclusion Constraint", "ExclusionConstraint"));
			constraintsNode.Nodes.Add(CreateTreeNode("Subset Constraint", "SubsetConstraint"));
			constraintsNode.Nodes.Add(CreateTreeNode("Value Constraint", "ValueConstraint"));
		}
		private TreeNode CreateTreeNode(string name, string tag)
		{
			TreeNode node = new TreeNode(name);
			node.Tag = tag;
			return node;
		}
		private void CheckTypeIfNeeded(ORMTypes typeToLookAt)
		{
			if ((_definitionObject.ORMTypes & typeToLookAt) == typeToLookAt)
			{
				CheckTreeNodeItem(tvModelElements.Nodes, typeToLookAt.ToString());
			}
		}
		private void ClearCheckedItems(TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				node.Checked = false;
				ClearCheckedItems(node.Nodes);
			}
		}
		private void CheckTreeNodeItem(TreeNodeCollection nodes, string typeName)
		{
			foreach (TreeNode node in nodes)
			{
				if ((string)node.Tag == typeName)
				{
					node.Checked = true;
					return;
				}
				CheckTreeNodeItem(node.Nodes, typeName);
			}
		}
		private void UpdateAttribute(string attributeName, string newValue)
		{
			if (_loadingDefinition)
			{
				return;
			}
			if (_definitionNode != null)
			{
				XmlAttribute attrib = _definitionNode.Attributes[attributeName];
				if (attrib == null)
				{
					attrib = CustomPropertiesManager.LoadedDocument.CreateAttribute(attributeName);
					_definitionNode.Attributes.Append(attrib);
				}
				_definitionNode.Attributes[attributeName].Value = newValue;
			}
			if (_definitionObject != null)
			{
				switch (attributeName)
				{
					case "name":
						_definitionObject.Name = newValue;
						break;
					case "description":
						_definitionObject.Description = newValue;
						break;
					case "defaultValue":
						_definitionObject.DefaultValue = newValue;
						break;
					case "dataType":
						_definitionObject.DataType = (CustomPropertyDataType)Enum.Parse(typeof(CustomPropertyDataType), newValue, true);
						break;
					case "category":
						_definitionObject.Category = newValue;
						break;
					case "verbalizeDefaultValue":
						_definitionObject.VerbalizeDefaultValue = newValue == "true";
						break;
				}
			}
			if (attributeName == "name" && NameChanged != null)
			{
				NameChanged(this, new NameChangedEventArgs(newValue));
			}
		}
		internal static bool ValidateDefaultValue(XmlNode definitionNode)
		{
			XmlAttribute attrib = definitionNode.Attributes["defaultValue"];
			if (attrib == null)
			{
				return true;
			}
			switch (definitionNode.Attributes["dataType"].Value)
			{
				case "string":
					return true;
				case "integer":
					int i;
					return int.TryParse(attrib.Value, out i);
				case "decimal":
					decimal d;
					return decimal.TryParse(attrib.Value, out d);
				case "datetime":
					DateTime dt;
					return DateTime.TryParse(attrib.Value, out dt);
				case "CustomEnumeration":
					XmlNodeList customEnumValues = definitionNode.SelectNodes("def:CustomEnumValues/def:CustomEnumValue", CustomPropertiesManager.NamespaceManager);
					foreach (XmlNode value in customEnumValues)
					{
						if (value.Attributes["value"].Value == attrib.Value)
						{
							return true;
						}
					}
					return false;
			}
			return false;
		}
		internal static bool ValidateDefaultValue(CustomPropertyDataType dataType, string defaultValue, string customEnum)
		{
			if (string.IsNullOrEmpty(defaultValue))
			{
				return true;
			}

			switch (dataType)
			{
				case CustomPropertyDataType.String:
					return true;
				case CustomPropertyDataType.Integer:
					int i;
					return int.TryParse(defaultValue, out i);
				case CustomPropertyDataType.Decimal:
					decimal d;
					return decimal.TryParse(defaultValue, out d);
				case CustomPropertyDataType.DateTime:
					DateTime dt;
					return DateTime.TryParse(defaultValue, out dt);
				case CustomPropertyDataType.CustomEnumeration:
					string[] values = customEnum.Split('\0');
					foreach (string value in values)
					{
						if (value == defaultValue)
						{
							return true;
						}
					}
					return false;
			}
			return false;
		}
		#endregion
	}
}
