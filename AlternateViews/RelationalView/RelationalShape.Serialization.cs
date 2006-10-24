using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling.Design;
using System.ComponentModel;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.Views.RelationalView
{
	internal partial class RelationalDiagram : IXmlSerializable
	{
		/// <summary>
		/// The local name of a TableShape element.
		/// </summary>
		private const string TableShapeElementName = "TableShape";
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
		/// The local name of a SubjectRef element
		/// </summary>
		private const string SubjectRefElementName = "SubjectRef";
		/// <summary>
		/// The local name of a RelationalDiagram element.
		/// </summary>
		private const string RelationalDiagramElementName = "RelationalDiagram";
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
			string rvNamespace = RelationalShapeDomainModel.XmlNamespace;

			Dictionary<object, object> context = Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
			object tablePositionsObject;
			Dictionary<ObjectType, PointD> tablePositions;
			if (!context.TryGetValue(RelationalModel.TablePositionDictionaryKey, out tablePositionsObject) ||
				(tablePositions = tablePositionsObject as Dictionary<ObjectType, PointD>) == null)
			{
				context[RelationalModel.TablePositionDictionaryKey] = tablePositions = new Dictionary<ObjectType, PointD>();
			}
			while (reader.NodeType != XmlNodeType.Element && reader.Read()) ;
			this.Associate(Store.ElementDirectory.GetElement(FromXml(reader.GetAttribute("SubjectRef"))));

			TypeConverter pointConverter = TypeDescriptor.GetConverter(typeof(PointD));
			while (reader.Read())
			{
				ObjectType objectType = null;
				PointD? location = null;
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.LocalName == TableShapeElementName)
					{
						while (reader.MoveToNextAttribute())
						{
							if (reader.LocalName == ObjectTypeRefAttributeName)
							{
								objectType = (ObjectType)Store.ElementDirectory.GetElement(FromXml(reader.Value));
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
						tablePositions[objectType] = location.Value;
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
			string rvNamespace = RelationalShapeDomainModel.XmlNamespace;
			// <RelationalDiagram>
			//    <TableShape ObjectTypeRef="" Location="x, y" />
			//    ...
			// </RelationalDiagram>
			writer.WriteStartElement(RelationalDiagramElementName, rvNamespace);
			writer.WriteAttributeString(IdAttributeName, ToXml(Id));
			writer.WriteAttributeString(SubjectRefElementName, ToXml(this.ModelElement.Id));
			TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(PointD));
			foreach (ShapeElement shapeElement in this.NestedChildShapes)
			{
				TableShape tableShape = shapeElement as TableShape;
				if (tableShape != null)
				{
					writer.WriteStartElement(TableShapeElementName, rvNamespace);
					writer.WriteAttributeString(ObjectTypeRefAttributeName, ToXml(((Table)tableShape.ModelElement).ConceptType.ObjectType.Id));
					writer.WriteAttributeString(LocationAttributeName, typeConverter.ConvertToInvariantString(tableShape.Location));
					writer.WriteEndElement();
				}
			}
			writer.WriteEndElement();
		}
		/// <summary>
		/// Converts a <see cref="T:System.Guid"/> to a <see cref="T:System.String"/>.
		/// </summary>
		/// <param name="guid">The <see cref="T:System.Guid"/> to convert.</param>
		/// <returns>The <see cref="T:System.String"/> representation of the <see cref="T:System.Guid"/>.</returns>
		private static string ToXml(Guid guid)
		{
			return '_' + XmlConvert.ToString(guid).ToUpperInvariant();
		}
		/// <summary>
		/// Converts a <see cref="T:System.String"/> to a <see cref="T:System.Guid"/>.
		/// </summary>
		/// <param name="guidString">The <see cref="T:System.String"/> to convert.</param>
		/// <returns>The <see cref="T:System.Guid"/> representation of the <see cref="T:System.String"/>.</returns>
		private static Guid FromXml(string guidString)
		{
			return XmlConvert.ToGuid(guidString.Substring(1));
		}
	}
}
