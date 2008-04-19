using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using System.IO;
using System.Xml.Xsl;
using Neumont.Tools.Modeling;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.CustomProperties
{
	public delegate void NameChangedHandler(object sender, NameChangedEventArgs e);
	public partial class CustomPropertiesManager : Form
	{
		#region Fields
		private static XslCompiledTransform _upgradeMachineFileTransform;
		private static readonly object LockObject = new object();
		private static string _filePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Neumont\ORM\GroupsAndDefinitions.xml";
		private static XmlNamespaceManager _namespaceManager;
		private static XmlDocument _loadedDoc;
		private Store _store;
		private const int _groupLevel = 1;
		private const int _definitionLevel = 2;
		private TreeNode _machineNode;
		private TreeNode _modelNode;
		#endregion
		#region Constructors
		public CustomPropertiesManager()
		{
			InitializeComponent();
		}
		#endregion
		#region Machine document management
		internal static XmlDocument LoadedDocument
		{
			get
			{
				XmlDocument retVal = _loadedDoc;
				if (retVal == null)
				{
					EnsureMachineDocument();
					retVal = _loadedDoc;
				}
				return retVal;
			}
		}

		internal static XmlNamespaceManager NamespaceManager
		{
			get
			{
				XmlNamespaceManager retVal = _namespaceManager;
				if (retVal == null)
				{
					EnsureMachineDocument();
					retVal = _namespaceManager;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Load the xml document off the machine if it hasn't been already.
		/// And create the document if it doesn't exist on the machine.
		/// </summary>
		private static void EnsureMachineDocument()
		{
			if (_loadedDoc == null)
			{
				lock (LockObject)
				{
					if (_loadedDoc == null)
					{
						XmlDocument newDoc = null;
						XmlNamespaceManager newNamespaceManager;
						if (File.Exists(_filePath))
						{
							using (FileStream stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
							{
								Stream loadFromStream = null;
								XmlReaderSettings readerSettings = new XmlReaderSettings();
								readerSettings.CloseInput = false;
								using (XmlReader reader = XmlReader.Create(stream, readerSettings))
								{
									if (reader.MoveToContent() == XmlNodeType.Element)
									{
										if (reader.Name == "CustomPropertyGroups")
										{
											if (reader.NamespaceURI == CustomPropertiesDomainModel.XmlNamespace)
											{
												stream.Position = 0;
												loadFromStream = stream;
											}
											else
											{
												// Attempt to upgrade the file to the current namespace. The
												// machine file itself has never exactly matched the file format
												// for the .orm model of the schema, but the I/O is custom and
												// we have never changed it--beyond writing a different default
												// namespace. The upgrade transform moves all elements to the
												// the current default namespace.
												stream.Position = 0;
												XslCompiledTransform transform = GetUpgradeMachineFileTransform();
												using (XmlReader upgradeReader = XmlReader.Create(stream))
												{
													loadFromStream = new MemoryStream();
													GetUpgradeMachineFileTransform().Transform(upgradeReader, null, loadFromStream);
												}
												loadFromStream.Position = 0;
											}
										}
									}
								}
								using (loadFromStream)
								{
									newDoc = new XmlDocument();
									newDoc.Load(loadFromStream);
								}
							}
						}
						if (newDoc == null)
						{
							newDoc = new XmlDocument();
							newDoc.AppendChild(newDoc.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
							XmlNode newGroups = newDoc.CreateNode(XmlNodeType.Element, "CustomPropertyGroups", CustomPropertiesDomainModel.XmlNamespace);
							newDoc.AppendChild(newGroups);
						}
						newNamespaceManager = new XmlNamespaceManager(newDoc.NameTable);
						newNamespaceManager.AddNamespace("def", CustomPropertiesDomainModel.XmlNamespace);
						newNamespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
						_loadedDoc = newDoc;
						_namespaceManager = newNamespaceManager;
					}
				}
			}
		}
		#endregion // Machine document management
		#region Event Handlers
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		private void btnSave_Click(object sender, EventArgs e)
		{
			ClearHighLights(tvCustomProperties.Nodes);

			XmlNodeList definitions = _loadedDoc.SelectNodes("//def:CustomPropertyDefinition", _namespaceManager);
			string error = null;
			foreach (XmlNode def in definitions)
			{
				string groupName = def.ParentNode.Attributes["name"].Value;
				if (!DefinitionEditor.ValidateDefaultValue(def))
				{
					HighlightTreeNode(tvCustomProperties.Nodes, def);
					error += string.Format("Definition named \"{0}\" in group \"{1}\" does not have a default value that conforms to the data type."
						+ Environment.NewLine, def.Attributes["name"].Value, groupName);
				}
				if (def.SelectNodes("def:ORMTypes/def:ORMType", _namespaceManager).Count == 0)
				{
					HighlightTreeNode(tvCustomProperties.Nodes, def);
					error += string.Format("Definition named \"{0}\" in group \"{1}\" does not have any model elements specified for it to show up on."
						+ Environment.NewLine, def.Attributes["name"].Value, groupName);
				}
				if ((def.Attributes["name"] == null || string.IsNullOrEmpty(def.Attributes["name"].Value)))
				{
					HighlightTreeNode(tvCustomProperties.Nodes, def);
					error += string.Format("Group named \"{0}\" has a definition without a name." + Environment.NewLine, groupName);
				}
			}

			XmlNodeList groups = _loadedDoc.SelectNodes("//def:CustomPropertyGroup", _namespaceManager);
			foreach (XmlNode group in groups)
			{
				if (group.Attributes["name"] == null || string.IsNullOrEmpty(group.Attributes["name"].Value))
				{
					HighlightTreeNode(tvCustomProperties.Nodes, group);
					error += "There is a group without a name." + Environment.NewLine;
					break;
				}
			}

			Dictionary<CustomPropertyGroup, List<CustomPropertyDefinition>> groupsAndDefs = GetGroupsAndDefsFromStore(_store);
			foreach (CustomPropertyGroup group in groupsAndDefs.Keys)
			{
				if (string.IsNullOrEmpty(group.Name))
				{
					HighlightTreeNode(tvCustomProperties.Nodes, group);
					if (error.IndexOf("There is a group without a name.") == -1)
					{
						error += "There is a group without a name." + Environment.NewLine;
					}
				}
				foreach (CustomPropertyDefinition def in groupsAndDefs[group])
				{
					if (!DefinitionEditor.ValidateDefaultValue(def.DataType, (string)def.DefaultValue, def.CustomEnumValue))
					{
						HighlightTreeNode(tvCustomProperties.Nodes, def);
						error += string.Format("Definition named \"{0}\" in group \"{1}\" does not have a default value that conforms to the data type."
							+ Environment.NewLine, def.Name, group.Name);
					}
					if (def.ORMTypes == ORMTypes.None)
					{
						HighlightTreeNode(tvCustomProperties.Nodes, def);
						error += string.Format("Definition named \"{0}\" in group \"{1}\" does not have any model elements specified for it to show up on."
							+ Environment.NewLine, def.Name, group.Name);
					}
					if (string.IsNullOrEmpty(def.Name))
					{
						HighlightTreeNode(tvCustomProperties.Nodes, def);
						error += string.Format("Group named \"{0}\" has a definition without a name." + Environment.NewLine, group.Name);
					}
				}
			}

			if (error != null)
			{
				MessageBox.Show(error, "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!File.Exists(Path.GetDirectoryName(_filePath)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
			}

			_loadedDoc.Save(_filePath);

			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void editor_NameChanged(object sender, NameChangedEventArgs e)
		{
			tvCustomProperties.SelectedNode.Text = e.NewName;
			//If this node was a group and default node make sure to append the "default" text after the group name
			if (tvCustomProperties.SelectedNode.Tag is XmlNode && tvCustomProperties.SelectedNode.Level == _groupLevel)
			{
				XmlNode xmlGroup = tvCustomProperties.SelectedNode.Tag as XmlNode;
				if (xmlGroup.Attributes["isDefault"] != null && xmlGroup.Attributes["isDefault"].Value == "true")
				{
					tvCustomProperties.SelectedNode.Text += " (Default)";
				}
			}
		}
		private void tsbAddGroup_Click(object sender, EventArgs e)
		{
			TreeNode rootNode = GetRootNode();

			if (rootNode == _machineNode)
			{
				XmlNode node = _loadedDoc.SelectSingleNode("//def:CustomPropertyGroups", _namespaceManager);
				XmlNode newGroup = _loadedDoc.CreateNode(XmlNodeType.Element, "CustomPropertyGroup", CustomPropertiesDomainModel.XmlNamespace);
				XmlAttribute nameAttrib = _loadedDoc.CreateAttribute("name");
				XmlAttribute descAttrib = _loadedDoc.CreateAttribute("description");
				XmlAttribute idAttrib = _loadedDoc.CreateAttribute("id");

				node.AppendChild(newGroup);
				newGroup.Attributes.Append(nameAttrib);
				newGroup.Attributes.Append(descAttrib);
				newGroup.Attributes.Append(idAttrib);
				idAttrib.Value = Guid.NewGuid().ToString();

				nameAttrib.Value = PickNewGroupName();
				TreeNode tNode = rootNode.Nodes.Add(nameAttrib.Value);
				tNode.Tag = newGroup;
				tvCustomProperties.SelectedNode = tNode;
			}
			else if (rootNode == _modelNode)
			{
				CustomPropertyGroup grp = new CustomPropertyGroup(_store);
				grp.Name = PickNewGroupName();
				TreeNode newGroupNode = rootNode.Nodes.Add(grp.Name);
				newGroupNode.Tag = grp;
				tvCustomProperties.SelectedNode = newGroupNode;
			}
		}
		private void tsbAddDefinition_Click(object sender, EventArgs e)
		{
			TreeNode rootNode = GetRootNode();

			TreeNode groupNode;
			groupNode = tvCustomProperties.SelectedNode.Level == _groupLevel ?
				tvCustomProperties.SelectedNode : tvCustomProperties.SelectedNode.Parent;
			if (rootNode == _machineNode)
			{
				XmlNode groupXmlNode;

				groupXmlNode = groupNode.Tag as XmlNode;
				CustomPropertyGroup groupObject = groupNode.Tag as CustomPropertyGroup;

				XmlNode newProperty = _loadedDoc.CreateNode(XmlNodeType.Element, "CustomPropertyDefinition", CustomPropertiesDomainModel.XmlNamespace);
				XmlNode newOrmTypes = _loadedDoc.CreateElement("ORMTypes", CustomPropertiesDomainModel.XmlNamespace);
				newProperty.AppendChild(newOrmTypes);
				groupXmlNode.AppendChild(newProperty);

				XmlAttribute nameAttrib = _loadedDoc.CreateAttribute("name");
				XmlAttribute categoryAttrib = _loadedDoc.CreateAttribute("category");
				XmlAttribute dataTypeAttrib = _loadedDoc.CreateAttribute("dataType");
				XmlAttribute descAttrib = _loadedDoc.CreateAttribute("description");

				newProperty.Attributes.Append(nameAttrib);
				newProperty.Attributes.Append(categoryAttrib);
				newProperty.Attributes.Append(dataTypeAttrib);
				newProperty.Attributes.Append(descAttrib);

				if (groupXmlNode != null)
				{
					nameAttrib.Value = PickNewDefinitionName(groupXmlNode);
				}
				else if (groupObject != null)
				{
					nameAttrib.Value = PickNewDefinitionName(groupObject);
				}

				dataTypeAttrib.Value = "string";

				TreeNode newPropertyNode = groupNode.Nodes.Add(nameAttrib.Value);
				newPropertyNode.Tag = newProperty;
				tvCustomProperties.SelectedNode = newPropertyNode;
			}
			else if (rootNode == _modelNode)
			{
				CustomPropertyGroup grp = groupNode.Tag as CustomPropertyGroup;
				CustomPropertyDefinition def = new CustomPropertyDefinition(_store);
				def.CustomPropertyGroup = grp;
				def.Name = PickNewDefinitionName(grp);
				TreeNode newDefNode = groupNode.Nodes.Add(def.Name);
				newDefNode.Tag = def;
				tvCustomProperties.SelectedNode = newDefNode;

			}
		}
		private void tsbAddGroupToMachine_Click(object sender, EventArgs e)
		{
			//Grab the model group that we are adding to the machine.
			TreeNode modelGroupNode = null;
			switch (tvCustomProperties.SelectedNode.Level)
			{
				case _groupLevel:
					modelGroupNode = tvCustomProperties.SelectedNode;
					break;
				case _definitionLevel:
					modelGroupNode = tvCustomProperties.SelectedNode.Parent;
					break;
			}

			CustomPropertyGroup cpg = modelGroupNode.Tag as CustomPropertyGroup;

			//Create the XML for the Group and it's attributes.
			XmlNode groupsNode = _loadedDoc.SelectSingleNode("//def:CustomPropertyGroups", _namespaceManager);
			XmlNode newGroup = _loadedDoc.CreateNode(XmlNodeType.Element, "CustomPropertyGroup", CustomPropertiesDomainModel.XmlNamespace);
			XmlAttribute nameAttrib = _loadedDoc.CreateAttribute("name");
			XmlAttribute descAttrib = _loadedDoc.CreateAttribute("description");
			XmlAttribute idAttrib = _loadedDoc.CreateAttribute("id");

			groupsNode.AppendChild(newGroup);
			newGroup.Attributes.Append(nameAttrib);
			newGroup.Attributes.Append(descAttrib);
			newGroup.Attributes.Append(idAttrib);

			//Assign the values from the model group.
			nameAttrib.Value = cpg.Name;
			descAttrib.Value = cpg.Description;
			idAttrib.Value = cpg.Id.ToString();

			//Create the tree node for the group and set the tag
			TreeNode tNode = _machineNode.Nodes.Add(nameAttrib.Value);
			tNode.Tag = newGroup;
			tvCustomProperties.SelectedNode = tNode;

			//Add all of the definitions for the group we just added to the machine.
			foreach (TreeNode nd in modelGroupNode.Nodes)
			{
				CustomPropertyDefinition newDef = nd.Tag as CustomPropertyDefinition;

				//Create the XML objects for the definition.
				XmlNode newProperty = _loadedDoc.CreateNode(XmlNodeType.Element, "CustomPropertyDefinition", CustomPropertiesDomainModel.XmlNamespace);
				XmlNode newOrmTypes = _loadedDoc.CreateElement("ORMTypes", CustomPropertiesDomainModel.XmlNamespace);
				newProperty.AppendChild(newOrmTypes);
				newGroup.AppendChild(newProperty);

				XmlAttribute defNameAttrib = _loadedDoc.CreateAttribute("name");
				XmlAttribute categoryAttrib = _loadedDoc.CreateAttribute("category");
				XmlAttribute dataTypeAttrib = _loadedDoc.CreateAttribute("dataType");
				XmlAttribute defDescAttrib = _loadedDoc.CreateAttribute("description");

				newProperty.Attributes.Append(defNameAttrib);
				newProperty.Attributes.Append(categoryAttrib);
				newProperty.Attributes.Append(dataTypeAttrib);
				newProperty.Attributes.Append(defDescAttrib);

				//Assign them their values from the model object.
				defNameAttrib.Value = newDef.Name;
				categoryAttrib.Value = newDef.Category;
				defDescAttrib.Value = newDef.Description;
				dataTypeAttrib.Value = newDef.DataType.ToString();

				//Probably a better way to do this but since the newDef.ORMTypes property is a bit field
				//we need to find out each enumeration that has been specified for the object and add it to the xml.
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.AllConstraints, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.EntityType, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.EqualityConstraint, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.ExclusionConstraint, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.FactType, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.FrequencyConstraint, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.MandatoryConstraint, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.RingConstraint, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.Role, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.SubsetConstraint, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.SubtypeFact, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.UniquenessConstraint, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.ValueConstraint, newOrmTypes);
				AddORMTypeToGroupIfNeeded(newDef.ORMTypes, ORMTypes.ValueType, newOrmTypes);

				//Add the new definition to the TreeView and set it's tag.
				TreeNode newPropertyNode = tNode.Nodes.Add(defNameAttrib.Value);
				newPropertyNode.Tag = newProperty;
			}
		}
		private void tsbAddGroupToModel_Click(object sender, EventArgs e)
		{
			TreeNode groupTreeNode = null;
			switch (tvCustomProperties.SelectedNode.Level)
			{
				case _groupLevel:
					groupTreeNode = tvCustomProperties.SelectedNode;
					break;
				case _definitionLevel:
					groupTreeNode = tvCustomProperties.SelectedNode.Parent;
					break;
			}

			XmlNode groupXMLNode = groupTreeNode.Tag as XmlNode;
			AddGroupToModel(_store, groupXMLNode, this, null);
		}
		private void tsbDefaultToggle_Click(object sender, EventArgs e)
		{
			TreeNode groupTreeNode;
			XmlNode groupNode;
			if (tvCustomProperties.SelectedNode.Level == _groupLevel)
			{
				groupTreeNode = tvCustomProperties.SelectedNode;
				groupNode = groupTreeNode.Tag as XmlNode;
			}
			else
			{
				groupTreeNode = tvCustomProperties.SelectedNode.Parent;
				groupNode = groupTreeNode.Tag as XmlNode;
			}

			if (groupNode.Attributes["isDefault"] == null)
			{
				XmlAttribute attrib = _loadedDoc.CreateAttribute("isDefault");
				attrib.Value = "true";
				groupNode.Attributes.Append(attrib);
				groupTreeNode.Text = groupNode.Attributes["name"].Value + " (Default)";
			}
			else if (groupNode.Attributes["isDefault"].Value == "true")
			{
				groupNode.Attributes.Remove(groupNode.Attributes["isDefault"]);
				groupTreeNode.Text = groupNode.Attributes["name"].Value;
			}
			else
			{
				groupNode.Attributes["isDefault"].Value = "true";
				groupTreeNode.Text = groupNode.Attributes["name"].Value + " (Default)";
			}
		}
		private void tsbDelete_Click(object sender, EventArgs e)
		{
			string message = "Are you sure you want to remove the ";
			message += tvCustomProperties.SelectedNode.Level == _groupLevel ? "group " : "definition ";
			XmlNode node = tvCustomProperties.SelectedNode.Tag as XmlNode;
			ModelElement elem = tvCustomProperties.SelectedNode.Tag as ModelElement;
			string name = null;
			if (node != null)
			{
				name = node.Attributes["name"].Value;
			}
			if (elem != null)
			{
				if (elem is CustomPropertyGroup)
				{
					name = ((CustomPropertyGroup)elem).Name;
				}
				else
				{
					name = ((CustomPropertyDefinition)elem).Name;
				}
			}
			message += "named \"" + name + "\"?";

			DialogResult rslt = MessageBox.Show(message, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			if (rslt == DialogResult.Yes)
			{
				TreeNode rootNode = null;
				switch (tvCustomProperties.SelectedNode.Level)
				{
					case _groupLevel:
						rootNode = tvCustomProperties.SelectedNode.Parent;
						break;
					case _definitionLevel:
						rootNode = tvCustomProperties.SelectedNode.Parent.Parent;
						break;
				}
				if (rootNode == _machineNode)
				{
					node.ParentNode.RemoveChild(node);
				}
				else
				{
					elem.Delete();
				}
				tvCustomProperties.SelectedNode.Remove();
			}
		}
		private void tvCustomProperties_AfterSelect(object sender, TreeViewEventArgs e)
		{
			groupEditor1.Visible = false;
			definitionEditor1.Visible = false;

			TreeNode rootNode = null;
			int nodeLevel = e.Node.Level;
			switch (nodeLevel)
			{
				case _groupLevel:
					rootNode = e.Node.Parent;
					groupEditor1.Visible = true;
					if (e.Node.Tag is XmlNode)
					{
						groupEditor1.GroupNode = e.Node.Tag as XmlNode;
					}
					if (e.Node.Tag is CustomPropertyGroup)
					{
						groupEditor1.GroupObject = e.Node.Tag as CustomPropertyGroup;
					}
					break;
				case _definitionLevel:
					rootNode = e.Node.Parent.Parent;
					definitionEditor1.Visible = true;
					if (e.Node.Tag is XmlNode)
					{
						definitionEditor1.DefinitionNode = e.Node.Tag as XmlNode;
					}
					if (e.Node.Tag is CustomPropertyDefinition)
					{
						definitionEditor1.DefinitionObject = e.Node.Tag as CustomPropertyDefinition;
					}
					break;
			}

			tsbAddDefinition.Enabled = nodeLevel != 0;
			tsbDelete.Enabled = nodeLevel != 0;
			tsbAddGroupToModel.Enabled = rootNode == _machineNode;
			tsbDefaultToggle.Enabled = rootNode == _machineNode;
			tsbAddGroupToMachine.Enabled = rootNode == _modelNode;
		}
		private void tvCustomProperties_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete && tsbDelete.Enabled)
			{
				tsbDelete.PerformClick();
			}
		}
		#endregion
		#region Methods
		/// <summary>
		/// Loads the form and shows the custom groups that have been defined on the machine
		/// and in the specified model.
		/// </summary>
		/// <param name="store">The store for the current model.</param>
		/// <param name="serviceProvider">The service provider to use to display the form.</param>
		public static void ShowCustomGroups(Store store, IServiceProvider serviceProvider)
		{
			if (store == null)
			{
				return;
			}
			if (serviceProvider == null)
			{
				serviceProvider = store;
			}
			using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.CustomPropertiesManagerTransactionName))
			{
				CustomPropertiesManager mgr = new CustomPropertiesManager();
				mgr._store = store;

				mgr.tvCustomProperties.Nodes.Clear();
				mgr._machineNode = mgr.tvCustomProperties.Nodes.Add("Machine");
				mgr._modelNode = mgr.tvCustomProperties.Nodes.Add("Model");

				EnsureMachineDocument();

				XmlNodeList groups = _loadedDoc.SelectNodes("//def:CustomPropertyGroup", _namespaceManager);
				foreach (XmlNode group in groups)
				{
					TreeNode newGroupNode = mgr._machineNode.Nodes.Add(group.Attributes["name"].InnerText);
					newGroupNode.Tag = group;

					if (group.Attributes["isDefault"] != null && group.Attributes["isDefault"].Value == "true")
					{
						newGroupNode.Text += " (Default)";
					}

					XmlNodeList defs = group.SelectNodes("def:CustomPropertyDefinition", _namespaceManager);
					foreach (XmlNode def in defs)
					{
						TreeNode defNode = newGroupNode.Nodes.Add(def.Attributes["name"].InnerText);
						defNode.Tag = def;
					}
				}

				Dictionary<CustomPropertyGroup, List<CustomPropertyDefinition>> groupsAndDefs = GetGroupsAndDefsFromStore(store);
				foreach (CustomPropertyGroup grp in groupsAndDefs.Keys)
				{
					TreeNode groupNode = mgr._modelNode.Nodes.Add(grp.Name);
					groupNode.Tag = grp;
					foreach (CustomPropertyDefinition def in groupsAndDefs[grp])
					{
						TreeNode defNode = groupNode.Nodes.Add(def.Name);
						defNode.Tag = def;
					}
				}

				ApplyDefaultGroups(store, mgr, null);

				bool saveChanges = false;
				IWindowsFormsEditorService windowsFormsEditorService = serviceProvider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
				IUIService uiService;
				if (windowsFormsEditorService != null)
				{
					saveChanges = (windowsFormsEditorService.ShowDialog(mgr) == DialogResult.OK);
				}
				else if ((uiService = serviceProvider.GetService(typeof(IUIService)) as IUIService) != null)
				{
					saveChanges = (uiService.ShowDialog(mgr) == DialogResult.OK);
				}
				
				if (saveChanges)
				{
					t.Commit();
				}
				else
				{
					t.Rollback();
				}
			}
		}
		/// <summary>
		/// This method grabs and compiles the XSLT transform that upgrades the machine file transform to the current namespace.
		/// </summary>
		/// <returns>The compiled XSLT tranform.</returns>
		private static XslCompiledTransform GetUpgradeMachineFileTransform()
		{
			XslCompiledTransform retVal = _upgradeMachineFileTransform;
			if (retVal == null)
			{
				lock (LockObject)
				{
					retVal = _upgradeMachineFileTransform;
					if (retVal == null)
					{
						retVal = new XslCompiledTransform();
						Type resourceType = typeof(CustomPropertiesManager);
						using (Stream transformStream = resourceType.Assembly.GetManifestResourceStream(resourceType, "UpgradeCurrentNamespace.xslt"))
						{
							using (XmlReader reader = XmlReader.Create(transformStream))
							{
								retVal.Load(reader, XsltSettings.TrustedXslt, new XmlUrlResolver());
							}
						}
						_upgradeMachineFileTransform = retVal;
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Looks through all tree nodes recursively until it finds the one with 
		/// the tag specified and highlights it's back color.
		/// </summary>
		/// <param name="treeNodes">The node collection to start looking in.</param>
		/// <param name="tag">The tag to look for.</param>
		private void HighlightTreeNode(TreeNodeCollection treeNodes, object tag)
		{
			foreach (TreeNode tNode in treeNodes)
			{
				if (tNode.Tag == tag)
				{
					tNode.BackColor = Color.LightPink;
					//Expand all the nodes leading up to this node.
					TreeNode tempNode = tNode;
					while ((tempNode = tempNode.Parent) != null && !tempNode.IsExpanded)
					{
						tempNode.Expand();
					}
					return;
				}
				//If this node has nodes keep looking for the one to highlight.
				if (tNode.Nodes.Count > 0)
				{
					HighlightTreeNode(tNode.Nodes, tag);
				}
			}
		}
		/// <summary>
		/// Removes the highlighting recursively from all nodes in the tree view.
		/// </summary>
		/// <param name="treeNodes">The node collection to start at.</param>
		private void ClearHighLights(TreeNodeCollection treeNodes)
		{
			foreach (TreeNode node in treeNodes)
			{
				node.BackColor = Color.Empty;
				ClearHighLights(node.Nodes);
			}
		}
		/// <summary>
		/// Gets a new name for a group.
		/// </summary>
		/// <returns>The name for the new group.</returns>
		private string PickNewGroupName()
		{
			List<string> names = new List<string>();
			XmlNodeList groupNodes = _loadedDoc.SelectNodes("//def:CustomPropertyGroup", _namespaceManager);
			foreach (XmlNode node in groupNodes)
			{
				names.Add(node.Attributes["name"].Value);
			}

			List<CustomPropertyGroup> groups = GetGroupsFromStore(_store);
			foreach (CustomPropertyGroup grp in groups)
			{
				names.Add(grp.Name);
			}

			return "CustomPropertyGroup" + LookForNewNumber(names.ToArray(), "CustomPropertyGroup");
		}
		/// <summary>
		/// Gets a new name for a definition that is in the Machine
		/// </summary>
		/// <param name="group">The group that this definition will belong to.</param>
		/// <returns></returns>
		private string PickNewDefinitionName(XmlNode group)
		{
			XmlNodeList defs = group.SelectNodes("def:CustomPropertyDefinition", _namespaceManager);
			List<string> names = new List<string>();
			foreach (XmlNode def in defs)
			{
				names.Add(def.Attributes["name"].Value);
			}
			return PickNewDefinitionName(names.ToArray());
		}
		/// <summary>
		/// Gets a new name for a definition that is in the Model.
		/// </summary>
		/// <param name="group">The group that this definition will belong to.</param>
		/// <returns>The name for the new definition.</returns>
		private string PickNewDefinitionName(CustomPropertyGroup group)
		{
			LinkedElementCollection<CustomPropertyDefinition> defs = group.CustomPropertyDefinitionCollection;
			List<string> names = new List<string>();
			foreach (CustomPropertyDefinition def in defs)
			{
				names.Add(def.Name);
			}
			return PickNewDefinitionName(names.ToArray());
		}
		/// <summary>
		/// Returns the new definition name based on the list of names passed in.
		/// </summary>
		/// <param name="names">The list of currently existing definition names.</param>
		/// <returns>A definition name that is currently available.</returns>
		private string PickNewDefinitionName(string[] names)
		{
			return "CustomPropertyDefinition" + LookForNewNumber(names, "CustomPropertyDefinition");
		}
		/// <summary>
		/// Looks through the list of names for any that match the base name + numbers at the end. 
		/// If it finds one that matches it will find the largest and smallest numbers
		/// at the end of the name and return the lowest number available. For instance 
		/// if the names list contains "abc2, abc3, abc4" this would see that "abc1" 
		/// is free and return "1" as it's value.
		/// </summary>
		/// <param name="names">The list of names to look through.</param>
		/// <param name="baseName">The base name to look for.</param>
		/// <returns></returns>
		private int LookForNewNumber(string[] names, string baseName)
		{
			string regularExp = baseName + @"\d+";
			Regex reg = new Regex(regularExp);
			int smallest = int.MaxValue;
			int largest = 0;
			foreach (string name in names)
			{
				if (reg.IsMatch(name))
				{
					int nodeNumber = int.Parse(name.Substring(baseName.Length, name.Length - baseName.Length));
					if (nodeNumber < smallest)
					{
						smallest = nodeNumber;
					}
					if (nodeNumber > largest)
					{
						largest = nodeNumber;
					}
				}
			}
			if (largest == 0 || smallest > 1)
			{
				return 1;
			}
			else
			{
				return ++largest;
			}
		}
		/// <summary>
		/// Used when adding a CustomPropertyGroup to the machine. Looks to see if the specified typeToLookFor
		/// is in the typesValue and if so adds the XML to the types node.
		/// </summary>
		/// <param name="typesValue">The existing types already specified.</param>
		/// <param name="typeToLookFor">The type we're looking for.</param>
		/// <param name="ORMTypesNode">The types node to add the type to if needed.</param>
		private void AddORMTypeToGroupIfNeeded(ORMTypes typesValue, ORMTypes typeToLookFor, XmlNode ORMTypesNode)
		{
			if ((typesValue & typeToLookFor) == typeToLookFor)
			{
				XmlNode newOrmType = _loadedDoc.CreateElement("ORMType", CustomPropertiesDomainModel.XmlNamespace);
				XmlAttribute nameAttrib = CustomPropertiesManager.LoadedDocument.CreateAttribute("name");
				nameAttrib.Value = typeToLookFor.ToString();
				ORMTypesNode.AppendChild(newOrmType);
				newOrmType.Attributes.Append(nameAttrib);
			}
		}
		/// <summary>
		/// Gets all of the groups and their definitions from the store.
		/// </summary>
		/// <returns>All of the groups and their definitions.</returns>
		private static Dictionary<CustomPropertyGroup, List<CustomPropertyDefinition>> GetGroupsAndDefsFromStore(Store store)
		{
			ReadOnlyCollection<ModelElement> mdlDefs = store.ElementDirectory.FindElements(CustomPropertyDefinition.DomainClassId);
			Dictionary<CustomPropertyGroup, List<CustomPropertyDefinition>> groupsAndDefs = new Dictionary<CustomPropertyGroup, List<CustomPropertyDefinition>>();
			foreach (ModelElement elemnt in mdlDefs)
			{
				CustomPropertyDefinition def = elemnt as CustomPropertyDefinition;
				if (!groupsAndDefs.ContainsKey(def.CustomPropertyGroup))
				{
					List<CustomPropertyDefinition> defs = new List<CustomPropertyDefinition>();
					defs.Add(def);
					groupsAndDefs.Add(def.CustomPropertyGroup, defs);
				}
				else
				{
					groupsAndDefs[def.CustomPropertyGroup].Add(def);
				}
			}
			return groupsAndDefs;
		}
		/// <summary>
		/// Gets all of the groups from the store.
		/// </summary>
		/// <returns>All of the groups.</returns>
		private static List<CustomPropertyGroup> GetGroupsFromStore(Store store)
		{
			ReadOnlyCollection<ModelElement> mdlDefs = store.ElementDirectory.FindElements(CustomPropertyGroup.DomainClassId);
			List<CustomPropertyGroup> groups = new List<CustomPropertyGroup>();
			foreach (CustomPropertyGroup grp in mdlDefs)
			{
				groups.Add(grp);
			}
			return groups;
		}
		/// <summary>
		/// Gets the root node of the currently selected TreeNode.
		/// </summary>
		/// <returns>The root tree node.</returns>
		private TreeNode GetRootNode()
		{
			TreeNode rootNode = null;
			switch (tvCustomProperties.SelectedNode.Level)
			{
				case _groupLevel:
					rootNode = tvCustomProperties.SelectedNode.Parent;
					break;
				case _definitionLevel:
					rootNode = tvCustomProperties.SelectedNode.Parent.Parent;
					break;
				case 0:
					rootNode = tvCustomProperties.SelectedNode;
					break;
			}
			return rootNode;
		}
		/// <summary>
		/// Goes through all of the machine level groups that are marked as default 
		/// and adds them to the model if needed.
		/// </summary>
		/// <param name="store">The current store</param>
		/// <param name="activeForm">An active form</param>
		/// <param name="notifyAdded">Used during deserialization fixup</param>
		private static void ApplyDefaultGroups(Store store, CustomPropertiesManager activeForm, INotifyElementAdded notifyAdded)
		{
			XmlNodeList defaultGroups = _loadedDoc.SelectNodes("//def:CustomPropertyGroup[@isDefault='true']", _namespaceManager);
			Dictionary<CustomPropertyGroup, List<CustomPropertyDefinition>> groupsAndDefs = GetGroupsAndDefsFromStore(store);
			foreach (XmlNode xmlGroup in defaultGroups)
			{
				string groupName = xmlGroup.Attributes["name"].Value;
				if (!ListHasGroupName(groupsAndDefs, groupName))
				{
					AddGroupToModel(store, xmlGroup, activeForm, notifyAdded);
				}
			}
		}
		/// <summary>
		/// Looks through the list of groups to see if one already exists with the specified name.
		/// </summary>
		/// <param name="groups">The list of groups and definitions.</param>
		/// <param name="name">The name to look for.</param>
		/// <returns>True if the list has a group with the specified name.</returns>
		private static bool ListHasGroupName(Dictionary<CustomPropertyGroup, List<CustomPropertyDefinition>> groups, string name)
		{
			foreach (CustomPropertyGroup group in groups.Keys)
			{
				if (group.Name == name)
				{
					return true; 
				}
			}
			return false;
		}
		/// <summary>
		/// Adds the specified machine level group to the model.
		/// </summary>
		/// <param name="store">Target store</param>
		/// <param name="groupXMLNode">The group to add to the model.</param>
		/// <param name="activeForm">The current form</param>
		/// <param name="notifyAdded">Used during deserialization fixup</param>
		private static void AddGroupToModel(Store store, XmlNode groupXMLNode, CustomPropertiesManager activeForm, INotifyElementAdded notifyAdded)
		{
			CustomPropertyGroup grp = new CustomPropertyGroup(store);
			grp.Name = groupXMLNode.Attributes["name"].Value;
			grp.Description = groupXMLNode.Attributes["description"].Value;

			if (notifyAdded != null)
			{
				notifyAdded.ElementAdded(grp, false);
			}

			TreeNode newGroupTreeNode = null;
			if (activeForm != null)
			{
				newGroupTreeNode = activeForm._modelNode.Nodes.Add(grp.Name);
				newGroupTreeNode.Tag = grp;
				activeForm.tvCustomProperties.SelectedNode = newGroupTreeNode;
			}

			XmlNodeList xmlDefinitions = groupXMLNode.SelectNodes("def:CustomPropertyDefinition", _namespaceManager);
			foreach (XmlNode xmlDef in xmlDefinitions)
			{
				CustomPropertyDefinition def = new CustomPropertyDefinition(store);
				def.CustomPropertyGroup = grp;
				def.Name = xmlDef.Attributes["name"].Value;
				def.Category = xmlDef.Attributes["category"].Value;
				def.Description = xmlDef.Attributes["description"].Value;
				def.DataType = (CustomPropertyDataType)Enum.Parse(typeof(CustomPropertyDataType), xmlDef.Attributes["dataType"].Value, true);
				if (xmlDef.Attributes["defaultValue"] != null)
				{
					def.DefaultValue = xmlDef.Attributes["defaultValue"].Value;
				}
				XmlNodeList types = xmlDef.SelectNodes("def:ORMTypes/def:ORMType", _namespaceManager);
				foreach (XmlNode ormType in types)
				{
					def.ORMTypes = def.ORMTypes | (ORMTypes)Enum.Parse(typeof(ORMTypes), ormType.Attributes["name"].Value, true);
				}

				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(def, true);
				}

				if (newGroupTreeNode != null)
				{
					newGroupTreeNode.Nodes.Add(def.Name).Tag = def;
				}
			}
		}
		#endregion
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds the implicit FactConstraint elements.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new DefaultMachinePropertiesFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Adds default properties
		/// </summary>
		private sealed class DefaultMachinePropertiesFixupListener : DeserializationFixupListener<ORMModel>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public DefaultMachinePropertiesFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Move settings from the machine file into the model if they are not there already 
			/// </summary>
			protected override void ProcessElement(ORMModel element, Store store, INotifyElementAdded notifyAdded)
			{
				EnsureMachineDocument();
				ApplyDefaultGroups(store, null, notifyAdded);
			}
		}
		#endregion // Deserialization Fixup
	}
}