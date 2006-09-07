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
		System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
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
					if (reader.LocalName == "TableShape")
					{
						while (reader.MoveToNextAttribute())
						{
							if (reader.LocalName == "ObjectTypeRef")
							{
								objectType = (ObjectType)Store.ElementDirectory.GetElement(FromXml(reader.Value));
							}
							else if (reader.LocalName == "Location")
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

		void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
		{
			string rvNamespace = RelationalShapeDomainModel.XmlNamespace;
			// <RelationalDiagram>
			//    <TableShape ObjectTypeRef="" Location="x, y" />
			//    ...
			// </RelationalDiagram>
			writer.WriteStartElement("RelationalDiagram", rvNamespace);
			writer.WriteAttributeString("id", ToXml(Id));
			writer.WriteAttributeString("SubjectRef", ToXml(this.ModelElement.Id));
			TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(PointD));
			foreach (ShapeElement shapeElement in this.NestedChildShapes)
			{
				TableShape tableShape = shapeElement as TableShape;
				if (tableShape != null)
				{
					writer.WriteStartElement("TableShape", rvNamespace);
					writer.WriteAttributeString("ObjectTypeRef", ToXml(((Table)tableShape.ModelElement).ConceptType.ObjectType.Id));
					writer.WriteAttributeString("Location", typeConverter.ConvertToInvariantString(tableShape.Location));
					writer.WriteEndElement();
				}
			}
			writer.WriteEndElement();
		}

		private static string ToXml(Guid guid)
		{
			return '_' + XmlConvert.ToString(guid).ToUpperInvariant();
		}
		private static Guid FromXml(string @string)
		{
			return XmlConvert.ToGuid(@string.Substring(1));
		}
	}
}
