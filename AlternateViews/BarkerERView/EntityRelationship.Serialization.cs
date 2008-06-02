using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling.Design;
using System.ComponentModel;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORMAbstraction;
using Neumont.Tools.ORMToORMAbstractionBridge;
using Neumont.Tools.Modeling.Shell;
using Neumont.Tools.ORMAbstractionToBarkerERBridge;
using Neumont.Tools.EntityRelationshipModels.Barker;

namespace Neumont.Tools.ORM.Views.BarkerERView
{
	partial class BarkerERDiagram : IXmlSerializable
	{
		/// <summary>
		/// The local name of a BarkerEntityShape element.
		/// </summary>
		private const string BarkerEntityShapeElementName = "BarkerEntityShape";
		/// <summary>
		/// The local name of an ObjectTypeRef attribute.
		/// </summary>
		private const string ObjectTypeRefAttributeName = "ObjectTypeRef";
		/// <summary>
		/// The local name of a Location attribute.
		/// </summary>
		private const string LocationAttributeName = "Location";
		/// <summary>
		/// The local name of an Id attribute.
		/// </summary>
		private const string IdAttributeName = "id";
		/// <summary>
		/// The local name of a SubjectRef attribute
		/// </summary>
		private const string SubjectRefAttributeName = "SubjectRef";
		/// <summary>
		/// The local name of a BarkerERDiagram element.
		/// </summary>
		private const string BarkerERDiagramElementName = "BarkerERDiagram";
		///// <summary>
		///// DisplayDataTypes attribute name.
		///// </summary>
		//private const string DisplayDataTypesAttributeName = "DisplayDataTypes";
		/// <summary>
		/// Gets the <see cref="T:System.Xml.Schema.XmlSchema"/> that the custom serialization should conform to.
		/// </summary>
		/// <returns><see langword="null" />.</returns>
		System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}
		/// <summary>
		/// Processes the XML that this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram"/> has
		/// written.
		/// </summary>
		/// <param name="reader">A <see cref="T:System.Xml.XmlReader"/> that will read the XML associated with the
		/// serialization of this object.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string rvNamespace = BarkerERShapeDomainModel.XmlNamespace;

			Dictionary<object, object> context = Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
			ISerializationContext serializationContext = ((ISerializationContextHost)Store).SerializationContext;
			object barkerEntityPositionsObject;
			Dictionary<Guid, PointD> barkerEntityPositions;
			if (!context.TryGetValue(BarkerEntityPositionDictionaryKey, out barkerEntityPositionsObject) ||
				(barkerEntityPositions = barkerEntityPositionsObject as Dictionary<Guid, PointD>) == null)
			{
				context[BarkerEntityPositionDictionaryKey] = barkerEntityPositions = new Dictionary<Guid, PointD>();
			}
			while (reader.NodeType != XmlNodeType.Element && reader.Read()) ;
			if (Subject == null)
			{
				serializationContext.RealizeElementLink(
					null,
					this,
					serializationContext.RealizeElement(reader.GetAttribute(SubjectRefAttributeName), BarkerErModel.DomainClassId, true),
					PresentationViewsSubject.SubjectDomainRoleId,
					null);
			}

			TypeConverter pointConverter = TypeDescriptor.GetConverter(typeof(PointD));
			while (reader.Read())
			{
				string objectType = null;
				PointD? location = null;
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.LocalName == BarkerEntityShapeElementName)
					{
						while (reader.MoveToNextAttribute())
						{
							if (reader.LocalName == ObjectTypeRefAttributeName)
							{
								objectType = reader.Value;
							}
							else if (reader.LocalName == LocationAttributeName)
							{
								location = (PointD)pointConverter.ConvertFromInvariantString(reader.Value);
							}
						}
						if (objectType == null || location == null)
						{
							throw new InvalidOperationException();
						}
						barkerEntityPositions[serializationContext.ResolveElementIdentifier(objectType)] = location.Value;
					}
				}
			}
		}
		/// <summary>
		/// Serializes this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram"/>.
		/// </summary>
		/// <param name="writer">A <see cref="T:System.Xml.XmlWriter"/> that will write the custom serialization contents.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			string rvNamespace = BarkerERShapeDomainModel.XmlNamespace;
			// <RelationalDiagram>
			//    <TableShape ObjectTypeRef="" Location="x, y" />
			//    ...
			// </RelationalDiagram>
			ISerializationContext serializationContext = ((ISerializationContextHost)Store).SerializationContext;
			writer.WriteStartElement(BarkerERDiagramElementName, rvNamespace);
			writer.WriteAttributeString(IdAttributeName, serializationContext.GetIdentifierString(Id));
			writer.WriteAttributeString(SubjectRefAttributeName, serializationContext.GetIdentifierString(this.ModelElement.Id));
			TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(PointD));
			foreach (ShapeElement shapeElement in this.NestedChildShapes)
			{
				BarkerEntityShape barkerEntityShape = shapeElement as BarkerEntityShape;
				if (barkerEntityShape != null)
				{
					ConceptType conceptType;
					ObjectType objectType;
					if (null != (conceptType = EntityTypeIsPrimarilyForConceptType.GetConceptType((EntityType)barkerEntityShape.ModelElement)) &&
						null != (objectType = ConceptTypeIsForObjectType.GetObjectType(conceptType)))
					{
						writer.WriteStartElement(BarkerEntityShapeElementName, rvNamespace);
						writer.WriteAttributeString(ObjectTypeRefAttributeName, serializationContext.GetIdentifierString(objectType.Id));
						writer.WriteAttributeString(LocationAttributeName, typeConverter.ConvertToInvariantString(barkerEntityShape.Location));
						writer.WriteEndElement();
					}
				}
			}
			writer.WriteEndElement();
		}
	}
}
