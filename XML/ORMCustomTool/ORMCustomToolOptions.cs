using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	internal interface ITransformationInput
	{
		XmlReader GetInput();
	}

	[Serializable]
	[XmlTypeAttribute(AnonymousType = true)]
	[XmlRootAttribute(ElementName = "ormCustomToolOptions", Namespace = "http://schemas.neumont.edu/ORM/ORMCustomToolOptions", IsNullable = false)]
	public class ORMCustomToolOptions
	{
		internal static readonly XmlReaderSettings XmlReaderSettings;

		static ORMCustomToolOptions()
		{
			XmlReaderSettings = new XmlReaderSettings();
			XmlReaderSettings.CloseInput = true;
		}

		#region Transformation
		private Transformation _transformation;

		[XmlElement(ElementName = "transformation", Type = typeof(Transformation))]
		public Transformation Transformation
		{
			get
			{
				return this._transformation;
			}
			set
			{
				this._transformation = value;
			}
		}
		#endregion

		#region OutputFileExtension
		private string _outputFileExtension;

		[XmlAttribute(AttributeName = "outputFileExtension", DataType = "NMTOKEN")]
		public string OutputFileExtension
		{
			get
			{
				return this._outputFileExtension;
			}
			set
			{
				this._outputFileExtension = value;
			}
		}
		#endregion
	}


	[Serializable]
	[XmlType(Namespace = "http://schemas.neumont.edu/ORM/ORMCustomToolOptions")]
	public class Transformation : ITransformationInput
	{

		private ITransformationInput _input;
		private string _stylesheet;

		#region InputForXmlSerializaer
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		[XmlElement("file", typeof(TransformationFile))]
		[XmlElement("transformation", typeof(Transformation))]
		public object InputForXmlSerializer
		{
			get
			{
				return this._input;
			}
			set
			{
				this._input = value as ITransformationInput;
			}
		}
		#endregion

		#region Input
		[XmlIgnore]
		internal ITransformationInput Input
		{
			get
			{
				return this._input;
			}
			set
			{
				this._input = value;
			}
		}
		#endregion

		#region Stylesheet
		[XmlAttribute(AttributeName = "stylesheet", DataType = "anyURI")]
		public string Stylesheet
		{
			get
			{
				return this._stylesheet;
			}
			set
			{
				this._stylesheet = value;
			}
		}
		#endregion

		public Stream GetOutput()
		{
			using (XmlReader xmlReaderInput = this.Input.GetInput())
			{
				MemoryStream buffer = new MemoryStream();
				ORMCustomTool.Transform(this.Stylesheet, xmlReaderInput, buffer);
				buffer.Position = 0;
				return buffer;
			}
		}

		public XmlReader GetInput()
		{
			return XmlReader.Create(this.GetOutput(), ORMCustomToolOptions.XmlReaderSettings);
		}
	}

	[Serializable]
	[XmlType(AnonymousType = true)]
	public class TransformationFile : ITransformationInput
	{
		private string _href;

		[XmlAttribute(AttributeName = "href", DataType = "anyURI")]
		public string Href
		{
			get
			{
				return this._href;
			}
			set
			{
				this._href = value;
			}
		}

		public XmlReader GetInput()
		{
			return XmlReader.Create(this.Href, ORMCustomToolOptions.XmlReaderSettings);
		}
	}
}