using System;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Northface.Tools.ORM.ObjectModel;
namespace Northface.Tools.ORM.ShapeModel
{
	#region ConstraintDisplayPosition enum
	/// <summary>
	/// Determines where internal constraints are drawn
	/// on a facttype
	/// </summary>
	[CLSCompliant(true)]
	public enum ConstraintDisplayPosition
	{
		/// <summary>
		/// Draw the constraints above the role boxes
		/// </summary>
		Top,
		/// <summary>
		/// Draw the constraints below the role boxes
		/// </summary>
		Bottom,
	}
	#endregion ConstraintDisplayPosition enum
	#region FactTypeShape class
	public partial class FactTypeShape
	{
		#region Size Constants
		private const double RoleBoxHeight = 0.11;
		private const double RoleBoxWidth = 0.16;
		private const double NestedFactHorizontalMargin = 0.2;
		private const double NestedFactVerticalMargin = 0.075;
		#endregion // Size Constants
		#region RolesShapeField class
		private class RolesShapeField : ShapeField
		{
			/// <summary>
			/// Construct a default RolesShapeField (Visible, but not selectable or focusable)
			/// </summary>
			public RolesShapeField()
			{
				DefaultFocusable = false;
				DefaultSelectable = false;
				DefaultVisibility = true;
			}
			/// <summary>
			/// Find the role sub shape at this location
			/// </summary>
			/// <param name="point"></param>
			/// <param name="parentShape"></param>
			/// <param name="diagramHitTestInfo"></param>
			public override void DoHitTest(PointD point, ShapeElement parentShape, DiagramHitTestInfo diagramHitTestInfo)
			{
				RectangleD fullBounds = GetBounds(parentShape);
				if (fullBounds.Contains(point))
				{
					FactType factType = (parentShape as FactTypeShape).AssociatedFactType;
					RoleMoveableCollection roles = factType.RoleCollection;
					int roleCount = roles.Count;
					if (roleCount != 0)
					{
						int roleIndex = Math.Min((int)((point.X - fullBounds.Left) * roleCount / fullBounds.Width), roleCount - 1);
						diagramHitTestInfo.HitDiagramItem = new DiagramItem(parentShape, this, new RoleSubField(roles[roleIndex]));
					}
				}
			}
			public override double GetMinimumWidth(ShapeElement parentShape)
			{
				return FactTypeShape.RoleBoxWidth * Math.Max(1, (parentShape as FactTypeShape).AssociatedFactType.RoleCollection.Count);
			}
			public override double GetMinimumHeight(ShapeElement parentShape)
			{
				return FactTypeShape.RoleBoxHeight;
			}
			public override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
			{
				FactTypeShape parentFactShape = parentShape as FactTypeShape;
				FactType factType = parentFactShape.AssociatedFactType;
				RoleMoveableCollection roles = factType.RoleCollection;
				int roleCount = roles.Count;
				bool objectified = factType.NestingType != null;
				if (roleCount > 0 || objectified)
				{
					int highlightRoleBox = -1;
					foreach (DiagramItem item in e.View.HighlightedShapes)
					{
						if (object.ReferenceEquals(parentShape, item.Shape))
						{
							RoleSubField roleField = item.SubField as RoleSubField;
							if (roleField != null)
							{
								highlightRoleBox = roleField.RoleIndex;
								break;
							}
						}
					}
					RectangleD bounds = GetBounds(parentShape);
					Graphics g = e.Graphics;
					double offsetBy = bounds.Width / roleCount;
					float offsetByF = (float)offsetBy;
					double lastX = bounds.Left;
					StyleSet styleSet = parentShape.StyleSet;
					Pen pen = styleSet.GetPen(FactTypeShape.RoleBoxOutlinePen);
					if (objectified)
					{
						RectangleF boundsF = RectangleD.ToRectangleF(bounds);
						g.DrawRectangle(pen, boundsF.Left, boundsF.Top, boundsF.Width, boundsF.Height);
					}
					int activeRoleIndex;
					float top = (float)bounds.Top;
					float bottom = (float)bounds.Bottom;
					float height = (float)bounds.Height;
					ExternalConstraintConnectAction activeAction = ActiveExternalConstraintConnectAction;
					StringFormat stringFormat = null;
					try
					{
						for (int i = 0; i < roleCount; ++i)
						{
							float lastXF = (float)lastX;
							if ((activeAction != null) &&
								(-1 != (activeRoleIndex = activeAction.GetActiveRoleIndex(roles[i]))))
							{
								g.FillRectangle(styleSet.GetBrush((i == highlightRoleBox) ? SelectedConstraintRoleHighlightedBackgroundBrush : SelectedConstraintRoleBackgroundBrush), lastXF, top, offsetByF, height);
								if (stringFormat == null)
								{
									stringFormat = new StringFormat();
									stringFormat.LineAlignment = StringAlignment.Center;
									stringFormat.Alignment = StringAlignment.Center;
								}
								g.DrawString((activeRoleIndex + 1).ToString(), styleSet.GetFont(DiagramFonts.CommentText), styleSet.GetBrush(DiagramBrushes.CommentText),new RectangleF(lastXF, top, offsetByF, height), stringFormat);
							}
							else if (i == highlightRoleBox)
							{
								// UNDONE: The highlighted background for a full shape is drawn by adjusting
								// the luminosity. MDF modifies luminosity automatically when a color is in
								// place by directly editing the pen/brush color, then restoring it. However,
								// there is no way to get to this facility when HasHighlighting is turned off,
								// so matching the color is difficult. Turning HasHightlighting on would mean
								// the entire shape would draw highlighted, and we would have to explicitly
								// un-highlight n-1 role boxes, which would be extremely flashy. We should
								// also use this facility to adjust the color for the selected constraint so we
								// would not need to use a separate brush for the normal/highlight colors.
								g.FillRectangle(styleSet.GetBrush(DiagramBrushes.ShapeBackgroundSelectedInactive), lastXF, top, offsetByF, height);
							}

							// Draw the line between the role boxes
							if (i != 0)
							{
								g.DrawLine(pen, lastXF, top, lastXF, bottom);
							}
							lastX += offsetBy;
						}
					}
					finally
					{
						if (stringFormat != null)
						{
							stringFormat.Dispose();
						}
					}
				}
			}
		}
		#endregion // RolesShapeField class
		#region RoleSubField class
		private class RoleSubField : ShapeSubField
		{
			#region Member variables
			private Role myAssociatedRole;
			#endregion // Member variables
			#region Construction
			public RoleSubField(Role associatedRole)
			{
				Debug.Assert(associatedRole != null);
				myAssociatedRole = associatedRole;
			}
			#endregion // Construction
			#region Required ShapeSubField overrides
			/// <summary>
			/// Returns true if the fields have the same associated role
			/// </summary>
			public override bool SubFieldEquals(object obj)
			{
				RoleSubField compareTo;
				if (null != (compareTo = obj as RoleSubField))
				{
					return myAssociatedRole == compareTo.myAssociatedRole;
				}
				return false;
			}
			/// <summary>
			/// Returns the hash code for the associated role
			/// </summary>
			public override int SubFieldHashCode
			{
				get
				{
					return myAssociatedRole.GetHashCode();
				}
			}
			/// <summary>
			/// A role sub field is always selectable, return true regardless of parameters
			/// </summary>
			/// <returns>true</returns>
			public override bool GetSelectable(ShapeElement parentShape, ShapeField parentField)
			{
				return true;
			}
			/// <summary>
			/// A role sub field is always focusable, return true regardless of parameters
			/// </summary>
			/// <returns>true</returns>
			public override bool GetFocusable(ShapeElement parentShape, ShapeField parentField)
			{
				return true;
			}
			/// <summary>
			/// Returns bounds based on the size of the parent shape
			/// and the RoleIndex of this shape
			/// </summary>
			/// <param name="parentShape">The containing FactTypeShape</param>
			/// <param name="parentField">The containing shape field</param>
			/// <returns>The vertical slice for this role</returns>
			public override RectangleD GetBounds(ShapeElement parentShape, ShapeField parentField)
			{
				RectangleD retVal = parentField.GetBounds(parentShape);
				RoleMoveableCollection roles = myAssociatedRole.FactType.RoleCollection;
				retVal.Width /= roles.Count;
				int roleIndex = roles.IndexOf(myAssociatedRole);
				if (roleIndex > 0)
				{
					retVal.Offset(roleIndex * retVal.Width, 0);
				}
				return retVal;
			}
			#endregion // Required ShapeSubField
			#region Accessor functions
			/// <summary>
			/// Get the Role element associated with this sub field
			/// </summary>
			public Role AssociatedRole
			{
				get
				{
					return myAssociatedRole;
				}
			}
			/// <summary>
			/// Returns the index of the associated Role element in its
			/// containing collection.
			/// </summary>
			public int RoleIndex
			{
				get
				{
					Debug.Assert(myAssociatedRole != null && !myAssociatedRole.IsRemoved);
					return myAssociatedRole.FactType.RoleCollection.IndexOf(myAssociatedRole);
				}
			}
			#endregion // Accessor functions
		}
		#endregion // RoleSubField class
		#region Member Variables
		private static RolesShapeField myRolesShapeField = null;
		private static readonly StyleSetResourceId RoleBoxOutlinePen = new StyleSetResourceId("Northface", "RoleBoxOutlinePen");
		private static readonly StyleSetResourceId SelectedConstraintRoleBackgroundBrush = new StyleSetResourceId("Northface", "SelectedConstraintRoleBackgroundBrush");
		private static readonly StyleSetResourceId SelectedConstraintRoleHighlightedBackgroundBrush = new StyleSetResourceId("Northface", "SelectedConstraintRoleHighlightedBackgroundBrush");
		private static ExternalConstraintConnectAction myActiveExternalConstraintConnectAction;
		#endregion // Member Variables
		#region RoleSubField integration
		/// <summary>
		/// Get the role corresponding to the given subField
		/// </summary>
		/// <param name="shapeField">The containing shape field (will always be the RolesShapeField)</param>
		/// <param name="subField">A RoleSubField</param>
		/// <returns>A Role element</returns>
		public override ICollection GetSubFieldRepresentedElements(ShapeField shapeField, ShapeSubField subField)
		{
			RoleSubField roleField;
			if (null != (roleField = subField as RoleSubField))
			{
				return new ModelElement[] { roleField.AssociatedRole };
			}
			return null;
		}
		/// <summary>
		/// The roles shape field is the default and only shape field inside
		/// a FactType shape.
		/// </summary>
		public override ShapeField DefaultShapeField
		{
			get
			{
				return myRolesShapeField;
			}
		}
		#endregion // RoleSubField integration
		#region Customize appearance
		/// <summary>
		/// Set to true. Enables role highlighting
		/// </summary>
		public override bool HasSubFieldHighlighting
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Set the default size for this object. This value is basically
		/// ignored because the size is ultimately based on the contained
		/// text, but it needs to be set.
		/// </summary>
		public override SizeD DefaultSize
		{
			get
			{
				return new SizeD(.7, .35);
			}
		}
		/// <summary>
		/// Change the outline pen to a thin black line for all instances
		/// of this shape.
		/// </summary>
		/// <param name="classStyleSet">The style set to modify</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			PenSettings penSettings = new PenSettings();
			penSettings.Color = SystemColors.WindowText;
			penSettings.Width = 1.0F/72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.Alignment = PenAlignment.Center;
			classStyleSet.AddPen(RoleBoxOutlinePen, DiagramPens.ShapeOutline, penSettings);

			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = Color.Yellow;
			classStyleSet.AddBrush(SelectedConstraintRoleBackgroundBrush, DiagramBrushes.DiagramBackground, brushSettings);
			brushSettings.Color = Color.Gold;
			classStyleSet.AddBrush(SelectedConstraintRoleHighlightedBackgroundBrush, DiagramBrushes.DiagramBackground, brushSettings);
		}
		/// <summary>
		/// Use the rolebox outline pen unless we're objectified
		/// </summary>
		public override StyleSetResourceId OutlinePenId
		{
			get
			{
				return IsObjectified ? DiagramPens.ShapeOutline : RoleBoxOutlinePen;
			}
		}
		/// <summary>
		/// Create our one placeholder shape field, which fills the whole shape
		/// and contains our role boxes.
		/// </summary>
		/// <param name="shapeFields">Per-class collection of shape fields</param>
		protected override void InitializeShapeFields(ShapeFieldCollection shapeFields)
		{
			base.InitializeShapeFields(shapeFields);

			// Initialize field
			RolesShapeField field = new RolesShapeField();

			// Add all shapes before modifying anchoring behavior
			shapeFields.Add(field);

			// Modify anchoring behavior
			AnchoringBehavior anchor = field.AnchoringBehavior;
			anchor.CenterHorizontally();
			anchor.CenterVertically();
			// Do not modify set edge anchors in this case. Edge anchors
			// force the bounds of the text field to the size of the parent,
			// we want it the other way around.

			Debug.Assert(myRolesShapeField == null); // Only called once
			myRolesShapeField = field;
		}
		/// <summary>
		/// The shape field used to display roles
		/// </summary>
		protected ShapeField RolesShape
		{
			get
			{
				return myRolesShapeField;
			}
		}
		/// <summary>
		/// Highlight region surrounding the roles box if
		/// it is objectified
		/// </summary>
		/// <value>True if the fact type is nested</value>
		public override bool HasHighlighting
		{
			get
			{
				return IsObjectified;
			}
		}
		/// <summary>
		/// Set the content size to the RolesShapeField
		/// </summary>
		protected override SizeD ContentSize
		{
			get
			{
				SizeD retVal = SizeD.Empty;
				ShapeField rolesShape = RolesShape;
				if (rolesShape != null)
				{
					retVal = rolesShape.GetBounds(this).Size;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Size to ContentSize plus some margin padding if we're a nested fact type.
		/// </summary>
		public override void AutoResize()
		{
			SizeD contentSize = ContentSize;
			if (!contentSize.IsEmpty && IsObjectified)
			{
				contentSize.Width += NestedFactHorizontalMargin + NestedFactHorizontalMargin;
				contentSize.Height += NestedFactVerticalMargin + NestedFactVerticalMargin;
			}
			Size = contentSize;
		}
		/// <summary>
		/// Return different shapes for objectified versus non-objectified fact types.
		/// The actual shape is controlled by the tools options page.
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				// If the fact is objectified, get the current setting from the options
				// page for how to draw the shape
				if (IsObjectified)
				{
					ShapeGeometry useShape;
					switch (Shell.OptionsPage.CurrentObjectifiedFactShape)
					{
						case Shell.ObjectifiedFactShape.HardRectangle:
							useShape = ShapeGeometries.Rectangle;
							break;
						case Shell.ObjectifiedFactShape.SoftRectangle:
						default:
							useShape = ShapeGeometries.RoundedRectangle;
							break;
					}
					return useShape;
				}
				else
				{
					// Just draw a rectangle if the fact IS NOT objectified
					return ShapeGeometries.Rectangle;
				}
			}
		}
		/// <summary>
		/// Add a shape element linked to this parent to display the name
		/// of the objectifying type
		/// </summary>
		/// <param name="element">ModelElement of type ObjectType</param>
		/// <returns>true</returns>
		protected override bool ShouldAddShapeForElement(ModelElement element)
		{
			Debug.Assert(
					(element is ObjectType && ((ObjectType)element).NestedFactType == AssociatedFactType)
					|| (element is Reading && ((Reading)element).FactType == AssociatedFactType)
				);
			return true;
		}
		/// <summary>
		/// An object type is displayed as an ObjectTypeShape unless it is
		/// objectified, in which case we display it as an ObjectifiedFactTypeNameShape
		/// </summary>
		/// <param name="element">The element to test. Expecting an ObjectType.</param>
		/// <param name="shapeTypes">The choice of shape types</param>
		/// <returns></returns>
		protected override MetaClassInfo ChooseShape(ModelElement element, IList shapeTypes)
		{
			Guid classId = element.MetaClassId;
			if (classId == ObjectType.MetaClassGuid)
			{
				return ORMDiagram.ChooseShapeTypeForObjectType((ObjectType)element, shapeTypes);
			}
			Debug.Assert(false); // We're only expecting an ObjectType here
			return base.ChooseShape(element, shapeTypes);
		}
		/// <summary>
		/// Make an ObjectifiedFactTypeNameShape a relative child element
		/// </summary>
		/// <param name="childShape"></param>
		/// <returns></returns>
		protected override RelationshipType ChooseRelationship(ShapeElement childShape)
		{
			Debug.Assert(childShape is ObjectifiedFactTypeNameShape || childShape is ReadingShape);
			return RelationshipType.Relative;
		}
		#endregion // Customize appearance
		#region Customize property display
		#region Reusable helper class for custom property descriptor creation
		/// <summary>
		/// A helper class to enable an object to be displayed as expandable,
		/// and have one string attribute specified as an editable string.
		/// </summary>
		private abstract class ExpandableStringConverter : ExpandableObjectConverter
		{
			/// <summary>
			/// Allow conversion from a string
			/// </summary>
			/// <param name="context">ITypeDescriptorContext</param>
			/// <param name="sourceType">Type</param>
			/// <returns>true for a string type</returns>
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string);
			}
			/// <summary>
			/// Allow conversion to a string. Note that the base class
			/// handles the ConvertTo function for us.
			/// </summary>
			/// <param name="context">ITypeDescriptorContext</param>
			/// <param name="destinationType">Type</param>
			/// <returns>true for a stirng type</returns>
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(string);
			}
			/// <summary>
			/// Convert from a string to the specified string
			/// meta attribute on the context element.
			/// </summary>
			/// <param name="context">ITypeDescriptorContext</param>
			/// <param name="culture">CultureInfo</param>
			/// <param name="value">New value for the attribute</param>
			/// <returns>context.Instance for a string value, defers to base otherwise</returns>
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string stringValue = value as string;
				if (stringValue != null)
				{
					object instance = context.Instance;
					ModelElement element = ConvertContextToElement(context);
					if (element != null)
					{
						MetaAttributeInfo attrInfo = element.Store.MetaDataDirectory.FindMetaAttribute(PrimaryStringAttributeId);
						// This will recurse when the property descriptor is changed because the
						// transaction close will refresh the property browser. Make sure we don't
						// fire a second SetValue here so we only get one item on the undo stack.
						if (stringValue != (string)element.GetAttributeValue(attrInfo))
						{
							// We want exactly the same result as achieved by setting
							// the property directly in the property grid, so create a property
							// descriptor to do the actual work of setting the property inside
							// a transaction.
							element.CreatePropertyDescriptor(attrInfo, element).SetValue(element, stringValue);
						}
					}
					return instance;
				}
				else
				{
					return base.ConvertFrom(context, culture, value);
				}
			}
			/// <summary>
			/// Override to retrieve the ModelElement to modify from the context
			/// information.
			/// </summary>
			/// <param name="context">ITypeDescriptorContext</param>
			/// <returns>ModelElement</returns>
			protected abstract ModelElement ConvertContextToElement(ITypeDescriptorContext context);
			/// <summary>
			/// Override to specify the string property to represent
			/// as the string value for the object. Defaults to
			/// NamedElement.NameMetaAttributeGuid.
			/// </summary>
			/// <value></value>
			protected virtual Guid PrimaryStringAttributeId
			{
				get
				{
					return NamedElement.NameMetaAttributeGuid;
				}
			}
		}
		/// <summary>
		/// A property descriptor implementation to
		/// use a ModelElement as an attribute
		/// in the property grid. Use with a realized
		/// ExpandableStringConverter instance to create
		/// an expandable property with an editable text field.
		/// </summary>
		private class HeaderDescriptor : PropertyDescriptor
		{
			private ModelElement myWrappedElement;
			private TypeConverter myConverter;
			/// <summary>
			/// Create a descriptor for the specified element and
			/// type converter.
			/// </summary>
			/// <param name="wrapElement">ModelElement</param>
			/// <param name="converter">TypeConverter (can be null)</param>
			public HeaderDescriptor(ModelElement wrapElement, TypeConverter converter) : base(wrapElement.GetComponentName(), new Attribute[]{})
			{
				myWrappedElement = wrapElement;
				myConverter = converter;
			}
			/// <summary>
			/// Return the converter specified in the constructor
			/// </summary>
			public override TypeConverter Converter
			{
				get
				{
					return myConverter;
				}
			}
			/// <summary>
			/// Use the underlying class name as the display name
			/// </summary>
			public override string DisplayName
			{
				get { return myWrappedElement.GetClassName(); }
			}
			/// <summary>
			/// Return this object as the component type
			/// </summary>
			public override Type ComponentType
			{
				get { return typeof(HeaderDescriptor); }
			}
			/// <summary>
			/// Returns false
			/// </summary>
			public override bool IsReadOnly
			{
				get { return false; }
			}
			/// <summary>
			/// Specify the type of the wrapped element
			/// as the PropertyType
			/// </summary>
			public override Type PropertyType
			{
				get { return myWrappedElement.GetType(); }
			}
			/// <summary>
			/// Disallow resetting the value
			/// </summary>
			/// <param name="component">object</param>
			/// <returns>false</returns>
			public override bool CanResetValue(object component)
			{
				return false;
			}
			/// <summary>
			/// Return the wrapped element as the property value
			/// </summary>
			/// <param name="component">object (ignored)</param>
			/// <returns>wrapElement value specified in constructor</returns>
			public override object GetValue(object component)
			{
				return myWrappedElement;
			}
			/// <summary>
			/// Do not serialize
			/// </summary>
			/// <param name="component"></param>
			/// <returns></returns>
			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}
			/// <summary>
			/// Do not reset
			/// </summary>
			/// <param name="component"></param>
			public override void ResetValue(object component)
			{
			}
			/// <summary>
			/// Do nothing. All value setting in this case
			/// is done by the type converter.
			/// </summary>
			/// <param name="component">object</param>
			/// <param name="value">object</param>
			public override void SetValue(object component, object value)
			{
			}
		}
		#endregion //Reusable helper class for custom property descriptor creation
		#region Nested FactType-specific type converters
		/// <summary>
		/// A type converter for showing the raw fact type
		/// as an expandable property in a nested fact type.
		/// </summary>
		private class ObjectifiedFactPropertyConverter : ExpandableStringConverter
		{
			public static readonly TypeConverter Converter = new ObjectifiedFactPropertyConverter();
			private ObjectifiedFactPropertyConverter() { }
			/// <summary>
			/// Convert from a FactTypeShape to a FactType
			/// </summary>
			/// <param name="context">ITypeDescriptorContext</param>
			/// <returns></returns>
			protected override ModelElement ConvertContextToElement(ITypeDescriptorContext context)
			{
				FactTypeShape shape = context.Instance as FactTypeShape;
				FactType factType;
				if (null != (shape = context.Instance as FactTypeShape) &&
					null != (factType = shape.AssociatedFactType))
				{
					return factType;
				}
				return null;
			}
		}
		/// <summary>
		/// A type converter for showing the nesting type
		/// as an expandable property in a nested fact type.
		/// </summary>
		private class ObjectifyingEntityTypePropertyConverter : ExpandableStringConverter
		{
			public static readonly TypeConverter Converter = new ObjectifyingEntityTypePropertyConverter();
			private ObjectifyingEntityTypePropertyConverter() { }
			/// <summary>
			/// Convert from a FactTypeShape to the nesting EntityType
			/// </summary>
			/// <param name="context">ITypeDescriptorContext</param>
			/// <returns></returns>
			protected override ModelElement ConvertContextToElement(ITypeDescriptorContext context)
			{
				FactTypeShape shape = context.Instance as FactTypeShape;
				FactType factType;
				if (null != (shape = context.Instance as FactTypeShape) &&
					null != (factType = shape.AssociatedFactType))
				{
					return factType.NestingType;
				}
				return null;
			}
		}
		#endregion // Nested FactType-specific type converters
		/// <summary>
		/// Show selected properties from the nesting type and the
		/// fact type for an objectified type, as well as expandable
		/// nodes for each of the underlying instances.
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			FactType factType = AssociatedFactType;
			ObjectType nestingType = (factType == null) ? null : factType.NestingType;
			if (nestingType != null)
			{
				MetaDataDirectory metaDir = factType.Store.MetaDataDirectory;
				return new PropertyDescriptorCollection(new PropertyDescriptor[]{
					nestingType.CreatePropertyDescriptor(metaDir.FindMetaAttribute(NamedElement.NameMetaAttributeGuid), nestingType),
					nestingType.CreatePropertyDescriptor(metaDir.FindMetaAttribute(ObjectType.IsIndependentMetaAttributeGuid), nestingType),
					new HeaderDescriptor(factType, ObjectifiedFactPropertyConverter.Converter),
					new HeaderDescriptor(nestingType, ObjectifyingEntityTypePropertyConverter.Converter),
					});
			}
			else
			{
				return base.GetProperties(attributes);
			}
		}
		#endregion // Customize property display
		#region Customize connection points
		/// <summary>
		/// Enable custom connection points
		/// </summary>
		/// <value>true</value>
		public override bool HasConnectionPoints
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Determine the best connection point for a link
		/// attached to a role in this fact type.
		/// </summary>
		/// <param name="link"></param>
		public override void CreateConnectionPoint(LinkShape link)
		{
			RolePlayerLink roleLink;
			if (null != (roleLink = link as RolePlayerLink))
			{
				// Extract basic information from the shape
				NodeShape objShape = roleLink.ToShape;
				if (objShape is FactTypeShape)
				{
					// Objectified end of relationship
					base.CreateConnectionPoint(link);
					return;
				}
				Debug.Assert((FactTypeShape)roleLink.FromShape == this);
				ObjectTypePlaysRole rolePlayerLink = roleLink.AssociatedRolePlayerLink;
				Role role = rolePlayerLink.PlayedRoleCollection;
				RoleMoveableCollection roles = AssociatedFactType.RoleCollection;
				int roleCount = roles.Count;
				int roleIndex = roles.IndexOf(role);
				
				PointD objCenter = objShape.AbsoluteCenter;
				RectangleD factBox = myRolesShapeField.GetBounds(this); // This finds the role box for both objectified and simple fact types
				factBox.Offset(AbsoluteBoundingBox.Location);

				// Decide whether top or bottom works best
				double finalY;
				if (Math.Abs(objCenter.Y - factBox.Top) <= Math.Abs(objCenter.Y - factBox.Bottom))
				{
					finalY = factBox.Top;
				}
				else
				{
					finalY = factBox.Bottom;
				}

				// If we're the first or last (or both) role, then
				// prefer an edge attach point.

				double finalX = factBox.Left + (factBox.Width / roleCount) * (roleIndex + .5);
				// UNDONE: Finish this code when connection points are more reliable
//				if (roleCount == 1)
//				{
//				}
//				else if (roleIndex == 0)
//				{
//				}
//				else if (roleIndex == roleCount - 1)
//				{
//				}
				base.CreateConnectionPoint(new PointD(finalX, finalY), link);
				return;
			}
			base.CreateConnectionPoint(link);
		}
		/// <summary>
		/// Set the connection point to the middle of the object
		/// for when we're objectified. This is consistent with the
		/// ObjectTypeShape implementation.
		/// </summary>
		/// <value></value>
		protected override PointD ConnectionPoint
		{
			get
			{
				RectangleD bounds = AbsoluteBounds;
				return new PointD(bounds.X + bounds.Width / 2, bounds.Top + bounds.Height / 2);
			}
		}
		#endregion // Customize connection points
		#region FactTypeShape specific
		/// <summary>
		/// Get the FactType associated with this shape
		/// </summary>
		public FactType AssociatedFactType
		{
			get
			{
				return ModelElement as FactType;
			}
		}
		/// <summary>
		/// Return true if the associated fact type is an objectified fact
		/// </summary>
		public bool IsObjectified
		{
			get
			{
				FactType factType = AssociatedFactType;
				return (factType == null) ? false : (factType.NestingType != null);
			}
		}
		/// <summary>
		/// Static property set when an external constraint is being created. The active
		/// connection is used to track which roles are highlighted.
		/// </summary>
		public static ExternalConstraintConnectAction ActiveExternalConstraintConnectAction
		{
			get
			{
				return myActiveExternalConstraintConnectAction;
			}
			set
			{
				myActiveExternalConstraintConnectAction = value;
			}
		}
		#endregion // FactTypeShape specific
		#region Shape display update rules
		[RuleOn(typeof(NestingEntityTypeHasFactType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class SwitchToNestedFact : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				NestingEntityTypeHasFactType link = e.ModelElement as NestingEntityTypeHasFactType;
				FactType nestedFactType = link.NestedFactType;
				ObjectType nestingType = link.NestingType;

				// Part1: Make sure the fact shape is visible on any diagram where the
				// corresponding nestingType is displayed
				foreach (object obj in nestingType.AssociatedPresentationElements)
				{
					ObjectTypeShape objectShape = obj as ObjectTypeShape;
					if (objectShape != null)
					{
						ORMDiagram currentDiagram = objectShape.Diagram as ORMDiagram;
						NodeShape factShape = currentDiagram.FindShapeForElement(nestingType) as NodeShape;
						if (factShape == null)
						{
							Diagram.FixUpDiagram(currentDiagram.ModelElement, nestedFactType);
							factShape = currentDiagram.FindShapeForElement(nestingType) as NodeShape;
						}
						if (factShape != null)
						{
							factShape.Location = objectShape.Location;
						}
					}
				}

				// Part2: Move any links from the object type to the fact type
				foreach (ObjectTypePlaysRole modelLink in nestingType.GetElementLinks(ObjectTypePlaysRole.RolePlayerMetaRoleGuid))
				{
					foreach (object obj in modelLink.PresentationRolePlayers)
					{
						RolePlayerLink rolePlayer = obj as RolePlayerLink;
						if (rolePlayer != null)
						{
							ORMDiagram currentDiagram = rolePlayer.Diagram as ORMDiagram;
							NodeShape factShape = currentDiagram.FindShapeForElement(nestedFactType) as NodeShape;
							if (factShape != null)
							{
								rolePlayer.ToShape = factShape;
							}
							else
							{
								// Backup. Should only happen if the FixupDiagram call in part 1
								// did not add the fact type.
								rolePlayer.Remove();
							}
						}
					}
				}

				// Part3: Remove object type shapes from the diagram. Do this before
				// adding the labels to the objectified fact types so clearing the role
				// players doesn't blow the labels away. Also, FixUpDiagram will attempt
				// to fix up the existing shapes instead of creating new ones if the existing
				// ones are not cleared away.
				nestingType.PresentationRolePlayers.Clear();

				// Part4: Resize the fact type wherever it is displayed and add the
				// labels for the fact type display.
				foreach (object obj in nestedFactType.AssociatedPresentationElements)
				{
					FactTypeShape shape = obj as FactTypeShape;
					if (shape != null)
					{
						shape.AutoResize();
						Diagram.FixUpDiagram(nestedFactType, nestingType);
					}
				}
			}
		}
		[RuleOn(typeof(NestingEntityTypeHasFactType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class SwitchFromNestedFact : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				NestingEntityTypeHasFactType link = e.ModelElement as NestingEntityTypeHasFactType;
				FactType nestedFactType = link.NestedFactType;
				ObjectType nestingType = link.NestingType;

				// Part1: Remove any existing presentation elements for the object type.
				// This removes all of the ObjectifiedTypeNameShape objects
				nestingType.PresentationRolePlayers.Clear();

				// Part2: Resize the fact type wherever it is displayed, and make sure
				// the object type is made visible in the same location.
				foreach (object obj in nestedFactType.AssociatedPresentationElements)
				{
					FactTypeShape factShape = obj as FactTypeShape;
					if (factShape != null)
					{
						factShape.AutoResize();
						ORMDiagram currentDiagram = factShape.Diagram as ORMDiagram;
						NodeShape objectShape = currentDiagram.FindShapeForElement(nestingType) as NodeShape;
						if (objectShape == null)
						{
							Diagram.FixUpDiagram(nestingType.Model, nestingType);
							objectShape = currentDiagram.FindShapeForElement(nestingType) as NodeShape;
						}
						if (objectShape != null)
						{
							PointD location = factShape.Location;
							location.Offset(0.0, 2 * factShape.Size.Height);
							objectShape.Location = location;
						}
					}
				}
				
				// Part3: Move any links from the fact type to the object type
				foreach (ObjectTypePlaysRole modelLink in nestingType.GetElementLinks(ObjectTypePlaysRole.RolePlayerMetaRoleGuid))
				{
					foreach (RolePlayerLink rolePlayer in modelLink.PresentationRolePlayers)
					{
						NodeShape objShape = (rolePlayer.Diagram as ORMDiagram).FindShapeForElement(nestingType) as NodeShape;
						if (objShape != null)
						{
							rolePlayer.ToShape = objShape;
						}
						else
						{
							rolePlayer.Remove();
						}
					}
				}
			}
		}
		#endregion // Shape display update rules
	}
	#endregion // FactTypeShape class
	#region ObjectifiedFactTypeNameShape class
	/// <summary>
	/// A specialized display of the nesting type as a relative
	/// child element of an objectified fact type
	/// </summary>
	public partial class ObjectifiedFactTypeNameShape
	{
		private static AutoSizeTextField myTextShapeField;
		/// <summary>
		/// Associate the text box with the object type name
		/// </summary>
		protected override Guid AssociatedShapeMetaAttributeGuid
		{
			get { return ObjectTypeNameMetaAttributeGuid; }
		}
		/// <summary>
		/// Store per-type value for the base class
		/// </summary>
		[CLSCompliant(false)]
		protected override AutoSizeTextField TextShapeField
		{
			get
			{
				return myTextShapeField;
			}
			set
			{
				Debug.Assert(myTextShapeField == null); // This should only be called once per type
				myTextShapeField = value;
			}
		}
		/// <summary>
		/// Get the ObjectType associated with this shape
		/// </summary>s
		public ObjectType AssociatedObjectType
		{
			get
			{
				return ModelElement as ObjectType;
			}
		}
		/// <summary>
		/// Move the name label above the parent fact type shape
		/// </summary>
		/// <param name="fixupState">BoundsFixupState</param>
		/// <param name="iteration">int</param>
		public override void OnBoundsFixup(BoundsFixupState fixupState, int iteration)
		{
			base.OnBoundsFixup(fixupState, iteration);
			if (fixupState != BoundsFixupState.Invalid)
			{
				SizeD size = Size;
				Location = new PointD(0, -1.5 * size.Height);
			}
		}
	}
	#endregion // ObjectifiedFactTypeNameShape class
}  