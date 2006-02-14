using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.Shell;
using Microsoft.VisualStudio.Modeling;
using System.Collections;

namespace Neumont.Tools.ORM.ShapeModel
{
	partial class RoleNameShape
	{
		private static AutoSizeTextField myTextField;

		/// <summary>
		/// A brush used to draw the value range text
		/// </summary>
		protected static readonly StyleSetResourceId RoleNameTextBrush = new StyleSetResourceId("Neumont", "RoleNameTextBrush");
		/// <summary>
		/// Sets up the AutoSizeTextField to be added to the ShapeFieldCollection, is only run once
		/// </summary>
		protected override void InitializeShapeFields(ShapeFieldCollection shapeFields)
		{
			RoleNameAutoSizeTextField textField = RoleNameAutoSizeTextField.CreateAutoSizeTextField();
			textField.AssociateValueWith(Store, AssociatedShapeMetaAttributeGuid, AssociatedModelMetaAttributeGuid);
			textField.DefaultFocusable = true;
			textField.FillBackground = true;
			shapeFields.Add(textField);

			// Adjust anchoring after all shape fields are added
			AnchoringBehavior anchor = textField.AnchoringBehavior;
			anchor.CenterHorizontally();
			anchor.CenterVertically();

			Debug.Assert(myTextField == null); // This should only be called once per type
			textField.DefaultTextBrushId = RoleNameTextBrush;
			TextShapeField = textField;
		}
		/// <summary>
		/// Sets up the Brush to be used to draw the object and adds it to the StyleSet
		/// </summary>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = colorService.GetForeColor(ORMDesignerColor.RoleName);
			classStyleSet.AddBrush(RoleNameTextBrush, DiagramBrushes.ShapeBackground, brushSettings);
		}
		/// <summary>
		/// Returns the guid for the role name attribute
		/// </summary>
		protected override Guid AssociatedShapeMetaAttributeGuid
		{
			get { return RoleNameShape.RoleNameMetaAttributeGuid; }
		}

		/// <summary>
		/// Gets and sets the AutoSizeTextField shape for this object
		/// </summary>
		[CLSCompliant(false)]
		protected override AutoSizeTextField TextShapeField
		{
			get
			{
				return myTextField;
			}
			set
			{
				Debug.Assert(myTextField == null);
				myTextField = value;
			}
		}

		/// <summary>
		/// Removes the RoleNameShape from the associated Role
		/// </summary>
		public static void RemoveRoleNameShapeFromRole(Role role)
		{
			foreach (PresentationElement element in role.PresentationRolePlayers)
			{
				RoleNameShape rns = element as RoleNameShape;
				if (rns != null)
				{
					rns.Remove();
				}
			}
		}

		/// <summary>
		/// Sets the isVisible property for the given Role
		/// </summary>
		private static void SetRoleNameDisplay(Role role, bool shouldDisplay, bool shouldRemove)
		{
			if (!shouldRemove)
			{
				Diagram.FixUpDiagram(role.FactType, role);
			}
			foreach (PresentationElement element in role.PresentationRolePlayers)
			{
				RoleNameShape rns = element as RoleNameShape;
				if (rns != null)
				{
					if (shouldRemove)
					{
						RemoveRoleNameShapeFromRole(role);
					}
					else
					{
						if (shouldDisplay)
						{
							rns.Show();
						}
						else
						{
							rns.Hide();
							rns.Size = SizeD.Empty;
						}
					}
					break;
				}
			}
		}
		/// <summary>
		/// Sets the isVisible for each of the Roles in the given FactType
		/// </summary>
		public static void SetRoleNameDisplay(FactType fact)
		{
			bool shouldDisplay = false;
			bool shouldRemove = false;
			foreach (PresentationElement element in fact.PresentationRolePlayers)
			{
				FactTypeShape fts = element as FactTypeShape;
				if (fts != null)
				{
					if (fts.DisplayRoleNames == DisplayRoleNames.UserDefault
						&& OptionsPage.CurrentRoleNameDisplay == RoleNameDisplay.On)
					{
						shouldDisplay = true;
					}
					else if (fts.DisplayRoleNames == DisplayRoleNames.On)
					{
						shouldDisplay = true;
					}
					else if (fts.DisplayRoleNames == DisplayRoleNames.Off)
					{
						shouldRemove = true;
					}
				}
			}
			foreach (Role role in fact.RoleCollection)
			{
				if (!string.IsNullOrEmpty(role.Name))
				{
					SetRoleNameDisplay(role, shouldDisplay, shouldRemove);
				}
			}
		}
	}

	/// <summary>
	/// Inherited AutoSizeTextField class so the display GetDisplayText could be overridden
	/// </summary>
	public class RoleNameAutoSizeTextField : AutoSizeTextField
	{
		/// <summary>
		/// Gets the text to display in the RoleNameShape and appends square brackets to beginning and end
		/// </summary>
		public override string GetDisplayText(ShapeElement parentShape)
		{
			string roleName = base.GetDisplayText(parentShape);
			return "[" + roleName + "]";
		}

		/// <summary>
		/// Returns a new instance of a RoleNameAutoSizeTextField
		/// </summary>
		public static RoleNameAutoSizeTextField CreateAutoSizeTextField()
		{
			return new RoleNameAutoSizeTextField();
		}
	}
}
