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
using System.Text;
using System.Diagnostics;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.Shell;
using Microsoft.VisualStudio.Modeling;
using System.Collections;
using System.Globalization;

namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class RoleNameShape
	{
		private static AutoSizeTextField myTextField;

		/// <summary>
		/// A brush used to draw the value range text
		/// </summary>
		protected static readonly StyleSetResourceId RoleNameTextBrush = new StyleSetResourceId("Neumont", "RoleNameTextBrush");
		/// <summary>
		/// Sets the AutoSizeTextField to be added to the ShapeFieldCollection, is only run once
		/// </summary>
		protected override AutoSizeTextField CreateAutoSizeTextField()
		{
			AutoSizeTextField newTextField = new RoleNameAutoSizeTextField();
			newTextField.DefaultFocusable = true;
			newTextField.FillBackground = true;
			newTextField.DefaultTextBrushId = RoleNameTextBrush;
			return newTextField;
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
		/// <summary>
		/// Place a newly added role name shape
		/// </summary>
		/// <param name="parent">Parent FactTypeShape</param>
		public override void PlaceAsChildOf(NodeShape parent)
		{
			FactTypeShape factShape = (FactTypeShape)parent;
			double x = -0.2;
			double y = -0.2;
			FactType factType = factShape.AssociatedFactType;
			// Cascades RoleNameShapes for facts that contain more than one role
			RoleMoveableCollection roles = factShape.DisplayedRoleOrder;
			int roleIndex = roles.IndexOf((Role)ModelElement);
			if (roleIndex != -1)
			{
				x += roleIndex * 0.15;
				y -= roleIndex * 0.15;
			}
			Location = new PointD(x, y);
		}
		/// <summary>
		/// Inherited AutoSizeTextField class so the display GetDisplayText could be overridden
		/// </summary>
		private class RoleNameAutoSizeTextField : AutoSizeTextField
		{
			/// <summary>
			/// Gets the text to display in the RoleNameShape and appends square brackets to beginning and end
			/// </summary>
			public override string GetDisplayText(ShapeElement parentShape)
			{
				string roleName = base.GetDisplayText(parentShape);
				return string.Format(CultureInfo.InvariantCulture, "[{0}]", roleName); // UNDONE: Localize format string
			}
		}
	}
}
