#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	#region RoleNameShape class
	public partial class RoleNameShape : ISelectionContainerFilter, IProxyDisplayProvider, IDynamicColorGeometryHost, ICustomElementDeletion
	{
		#region Member Variables
		private static AutoSizeTextField myTextField;

		/// <summary>
		/// A brush used to draw the value range text
		/// </summary>
		protected static readonly StyleSetResourceId RoleNameTextBrush = new StyleSetResourceId("ORMArchitect", "RoleNameTextBrush");
		#endregion // Member Variables
		#region Base overrides
		/// <summary>
		/// Sets the AutoSizeTextField to be added to the ShapeFieldCollection, is only run once
		/// </summary>
		/// <param name="fieldName">Non-localized name for the field</param>
		protected override AutoSizeTextField CreateAutoSizeTextField(string fieldName)
		{
			AutoSizeTextField newTextField = new RoleNameAutoSizeTextField(fieldName);
			newTextField.DefaultFocusable = true;
			newTextField.DefaultTextBrushId = RoleNameTextBrush;
			return newTextField;
		}
		/// <summary>
		/// Sets up the Brush to be used to draw the object and adds it to the StyleSet
		/// </summary>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = colorService.GetForeColor(ORMDesignerColor.RoleName);
			classStyleSet.AddBrush(RoleNameTextBrush, DiagramBrushes.ShapeBackground, brushSettings);
		}
		/// <summary>
		/// Returns the guid for the object model name property
		/// </summary>
		protected override Guid AssociatedModelDomainPropertyId
		{
			get
			{
				return Role.NameDomainPropertyId;
			}
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
		/// Place a newly added role name shape
		/// </summary>
		/// <param name="parent">Parent FactTypeShape</param>
		/// <param name="createdDuringViewFixup">Whether this shape was created as part of a view fixup</param>
		public override void PlaceAsChildOf(NodeShape parent, bool createdDuringViewFixup)
		{
			if (createdDuringViewFixup)
			{
				FactTypeShape factShape = (FactTypeShape)parent;
				double x = -0.2;
				double y = -0.2;
				FactType factType = factShape.AssociatedFactType;
				// Cascades RoleNameShapes for facts that contain more than one role
				LinkedElementCollection<RoleBase> roles = factShape.DisplayedRoleOrder;
				int roleIndex = roles.IndexOf((RoleBase)ModelElement);
				if (roleIndex != -1)
				{
					x += roleIndex * 0.15;
					y -= roleIndex * 0.15;
				}
				Location = new PointD(x, y);
			}
		}
		/// <summary>
		/// Highlight both the name shape and the corresponding role box.
		/// </summary>
		public override void OnMouseEnter(DiagramPointEventArgs e)
		{
			DiagramClientView clientView;
			FactTypeShape parentShape;
			Role role;
			if (null != (clientView = e.DiagramClientView) &&
				null != (parentShape = ParentShape as FactTypeShape) &&
				null != (role = this.ModelElement as Role))
			{
				DiagramItemCollection items = new DiagramItemCollection();
				items.Add(new DiagramItem(this));
				items.Add(parentShape.GetDiagramItem(role));
				clientView.HighlightedShapes.Set(items);
			}
			else
			{
				base.OnMouseEnter(e);
			}
		}
		/// <summary>
		/// Highlight both the name shape and the corresponding role box.
		/// </summary>
		public override void OnMouseLeave(DiagramPointEventArgs e)
		{
			DiagramClientView clientView;
			FactTypeShape parentShape;
			Role role;
			if (null != (clientView = e.DiagramClientView) &&
				null != (parentShape = ParentShape as FactTypeShape) &&
				null != (role = this.ModelElement as Role))
			{
				clientView.HighlightedShapes.Remove(new DiagramItem[]{new DiagramItem(this), parentShape.GetDiagramItem(role)});
			}
			else
			{
				base.OnMouseLeave(e);
			}
		}
		#endregion // Base overrides
		#region IDynamicColorGeometryHost Implementation
		/// <summary>
		/// Implements <see cref="IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId,Pen)"/>
		/// </summary>
		protected static Color UpdateDynamicColor(StyleSetResourceId penId, Pen pen)
		{
			return Color.Empty;
		}
		Color IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId penId, Pen pen)
		{
			return UpdateDynamicColor(penId, pen);
		}
		/// <summary>
		/// Implements <see cref="IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId,Brush)"/>
		/// </summary>
		protected Color UpdateDynamicColor(StyleSetResourceId brushId, Brush brush)
		{
			Color retVal = Color.Empty;
			SolidBrush solidBrush;
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, RoleNameShape, RoleBase>[] providers;
			RoleBase element;
			Store store;
			if (brushId == RoleNameTextBrush &&
				null != (solidBrush = brush as SolidBrush) &&
				null != (store = Utility.ValidateStore(Store)) &&
				null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, RoleNameShape, RoleBase>>()) &&
				null != (element = (RoleBase)ModelElement))
			{
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(ORMDiagramDynamicColor.FloatingText, this, element);
					if (alternateColor != Color.Empty)
					{
						retVal = solidBrush.Color;
						solidBrush.Color = alternateColor;
						break;
					}
				}
			}
			return retVal;
		}
		Color IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId brushId, Brush brush)
		{
			return UpdateDynamicColor(brushId, brush);
		}
		#endregion // IDynamicColorGeometryHost Implementation
		#region RoleNameShape specific
		/// <summary>
		/// Removes the RoleNameShape from the associated Role
		/// </summary>
		public static void RemoveRoleNameShapeFromRole(Role role)
		{
			LinkedElementCollection<PresentationElement> pels = PresentationViewsSubject.GetPresentation(role);
			int pelCount = pels.Count;
			for (int i = pelCount - 1; i >= 0; --i)
			{
				RoleNameShape pel = pels[i] as RoleNameShape;
				if (pel != null)
				{
					pel.Delete();
				}
			}
		}
		#endregion // RoleNameShape specific
		#region ISelectionContainerFilter Implementation
		/// <summary>
		/// Implements ISelectionContainerFilter.IncludeInSelectionContainer
		/// </summary>
		protected static bool IncludeInSelectionContainer
		{
			get
			{
				return false;
			}
		}
		bool ISelectionContainerFilter.IncludeInSelectionContainer
		{
			get
			{
				return IncludeInSelectionContainer;
			}
		}
		#endregion // ISelectionContainerFilter Implementation
		#region RoleNameAutoSizeTextField class
		/// <summary>
		/// Inherited AutoSizeTextField class so the display GetDisplayText could be overridden
		/// </summary>
		private sealed class RoleNameAutoSizeTextField : AutoSizeTextField
		{
			/// <summary>
			/// Create a new RoleNameAutoSizeTextField
			/// </summary>
			/// <param name="fieldName">Non-localized name for the field</param>
			public RoleNameAutoSizeTextField(string fieldName)
			    : base(fieldName)
			{
			}			
			/// <summary>
			/// Gets the text to display in the RoleNameShape and appends square brackets to beginning and end
			/// </summary>
			public sealed override string GetDisplayText(ShapeElement parentShape)
			{
				return string.Format(CultureInfo.InvariantCulture, "[{0}]", base.GetDisplayText(parentShape)); // UNDONE: Localize format string
			}
		}
		#endregion // RoleNameAutoSizeTextField class
		#region IProxyDisplayProvider Implementation
		/// <summary>
		/// Implements <see cref="IProxyDisplayProvider.ElementDisplayedAs"/>
		/// </summary>
		protected object ElementDisplayedAs(ModelElement element, ModelError forError)
		{
			Role role;
			FactType factType;
			//((Role)ModelElement).FactType == ((FactType)element).ImpliedByObjectification.NestedFactType
			if (null != (role = element as Role))
			{
				return ((FactTypeShape)ParentShape).GetDiagramItem(role);
			}
			else if (null != (factType = element as FactType))
			{
				Objectification objectification;
				if (null != (objectification = factType.ImpliedByObjectification) &&
					null != (role = (Role)ModelElement) &&
					role.FactType == objectification.NestedFactType)
				{
					return ((FactTypeShape)ParentShape).GetDiagramItem(role);
				}
			}
			return null;
		}
		object IProxyDisplayProvider.ElementDisplayedAs(ModelElement element, ModelError forError)
		{
			return ElementDisplayedAs(element, forError);
		}
		#endregion // IProxyDisplayProvider Implementation
		#region ICustomElementDeletion Implementation
		/// <summary>
		/// Implements <see cref="ICustomElementDeletion.DeleteCustomElement"/>
		/// Clear the role name on deletion instead of deleting the role itself.
		/// </summary>
		protected void DeleteCustomElement()
		{
			Role role = ModelElement as Role;
			if (role != null)
			{
				role.Name = "";
			}
		}
		void ICustomElementDeletion.DeleteCustomElement()
		{
			DeleteCustomElement();
		}
		#endregion // ICustomElementDeletion Implementation
	}
	#endregion // RoleNameShape class
	#region FactTypeShape class
	partial class FactTypeShape
	{
		#region Role name shape display helpers
		private long GetRoleNameVisibilityChangedValue()
		{
			TransactionManager tmgr = Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				// Subtract 1 so that we get a difference in the transaction log
				return unchecked(tmgr.CurrentTransaction.SequenceNumber - 1);
			}
			else
			{
				return 0L;
			}
		}
		private void SetRoleNameVisibilityChangedValue(long newValue)
		{
			// Nothing to do, we're just trying to create a transaction log entry
		}
		private void OnRoleNameVisibilityChanged()
		{
			TransactionManager tmgr = Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				RoleNameVisibilityChanged = tmgr.CurrentTransaction.SequenceNumber;
			}
		}
		private static void RoleNameVisibilityChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			FactTypeShape factTypeShape = (FactTypeShape)e.ModelElement;
			if (!factTypeShape.IsDeleted)
			{
				DisplayRoleNames display = factTypeShape.DisplayRoleNames;
				bool shouldBeVisible = display == DisplayRoleNames.On || (display == DisplayRoleNames.UserDefault && OptionsPage.CurrentRoleNameDisplay == RoleNameDisplay.On);
				foreach (ShapeElement childShape in factTypeShape.RelativeChildShapes)
				{
					RoleNameShape roleNameShape;
					if (null != (roleNameShape = childShape as RoleNameShape) &&
						(shouldBeVisible ^ roleNameShape.IsVisible))
					{
						if (shouldBeVisible)
						{
							roleNameShape.Show();
						}
						else
						{
							roleNameShape.Hide();
						}
					}
				}
			}
		}
		/// <summary>
		/// Adjust the role name display for all shapes corresponding a given fact type
		/// </summary>
		public static void UpdateRoleNameDisplay(FactType factType)
		{
			foreach (PresentationElement element in PresentationViewsSubject.GetPresentation(factType))
			{
				FactTypeShape factTypeShape;
				ORMDiagram diagram;
				if (null != (factTypeShape = element as FactTypeShape) &&
					null != (diagram = factTypeShape.Diagram as ORMDiagram))
				{
					UpdateRoleNameDisplay(factType, factTypeShape, diagram, false);
				}
			}
		}
		/// <summary>
		/// Adjust the role name display for this shape.
		/// </summary>
		public void UpdateRoleNameDisplay()
		{
			UpdateRoleNameDisplay(false);
		}
		private void UpdateRoleNameDisplay(bool immediateNotification)
		{
			FactType factType;
			ORMDiagram diagram;
			if (null != (diagram = Diagram as ORMDiagram) &&
				null != (factType = AssociatedFactType))
			{
				UpdateRoleNameDisplay(factType, this, diagram, immediateNotification);
			}
		}
		/// <summary>
		/// Set shape visibility for the given fact type, fact type shape, and diagram
		/// </summary>
		private static void UpdateRoleNameDisplay(FactType factType, FactTypeShape factTypeShape, ORMDiagram diagram, bool immediateNotification)
		{
			DisplayRoleNames display = factTypeShape.DisplayRoleNames;
			bool asObjectType = factTypeShape.DisplayAsObjectType;
			bool shouldDisplay = !asObjectType && display == DisplayRoleNames.On || (display == DisplayRoleNames.UserDefault && OptionsPage.CurrentRoleNameDisplay == RoleNameDisplay.On);
			bool shouldRemove = asObjectType || display == DisplayRoleNames.Off;
			foreach (RoleBase roleBase in factType.RoleCollection)
			{
				Role role = roleBase as Role;
				if (role != null && !string.IsNullOrEmpty(role.Name))
				{
					UpdateRoleNameDisplay(role, factType, factTypeShape, diagram, shouldDisplay, shouldRemove, immediateNotification);
				}
			}
		}
		/// <summary>
		/// Set shape visibility for a role in the given fact type, fact type shape, and diagram
		/// </summary>
		private static void UpdateRoleNameDisplay(Role role, FactType factType, FactTypeShape factTypeShape, ORMDiagram diagram, bool shouldDisplay, bool shouldRemove, bool immediateNotification)
		{
			if (!shouldRemove && diagram != null)
			{
				diagram.FixUpLocalDiagram(factType, role);
			}
			LinkedElementCollection<ShapeElement> childShapes = factTypeShape.RelativeChildShapes;
			bool notifyUpdate = false;
			for (int i = childShapes.Count - 1; i >= 0; --i)
			{
				RoleNameShape roleNameShape;
				if (null != (roleNameShape = childShapes[i] as RoleNameShape))
				{
					if (shouldRemove)
					{
						roleNameShape.Delete();
					}
					else
					{
						if (shouldDisplay)
						{
							if (!roleNameShape.IsVisible)
							{
								if (immediateNotification)
								{
									roleNameShape.Show();
								}
								else
								{
									notifyUpdate = true;
								}
							}
						}
						else if (roleNameShape.IsVisible)
						{
							if (immediateNotification)
							{
								roleNameShape.Hide();
							}
							else
							{
								notifyUpdate = true;
							}
							roleNameShape.Size = SizeD.Empty;
						}
					}
				}
			}
			if (notifyUpdate)
			{
				factTypeShape.OnRoleNameVisibilityChanged();
			}
		}
		#endregion // Role name shape display helpers
	}
	#endregion // FactTypeShape class
}
