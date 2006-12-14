using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Build.BuildEngine;
using Neumont.Tools.ORM.ORMCustomTool;
using System.Collections.ObjectModel;
using Microsoft.XmlDiffPatch;
using System.Diagnostics;


public sealed class OIALDiffCustomTool : IORMGenerator
{
	private class WrappedStream : Stream
	{
		private readonly Stream _backingStream;
		public WrappedStream(Stream backingStream)
			: base()
		{
			_backingStream = backingStream;
			base.Close();
		}
		protected Stream BackingStream()
		{
			return _backingStream;
		}
		public void CloseBackingStream()
		{
			_backingStream.Close();
		}
		public override void Close()
		{
			_backingStream.Close();
		}
		public override bool CanTimeout
		{
			get
			{
				return _backingStream.CanTimeout;
			}
		}
		public override bool CanRead
		{
			get { return _backingStream.CanRead; }
		}

		public override bool CanSeek
		{
			get { return _backingStream.CanSeek; }
		}

		public override bool CanWrite
		{
			get { return _backingStream.CanWrite; }
		}

		public override void Flush()
		{
			_backingStream.Flush();
		}

		public override long Length
		{
			get { return _backingStream.Length; }
		}

		public override long Position
		{
			get
			{
				return _backingStream.Position;
			}
			set
			{
				_backingStream.Position = value;
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return _backingStream.Read(buffer, offset, count);
		}

		public override int ReadByte()
		{
			return _backingStream.ReadByte();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _backingStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			_backingStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_backingStream.Write(buffer, offset, count);
		}
		public override int WriteTimeout
		{
			get
			{
				return _backingStream.WriteTimeout;
			}
			set
			{
				_backingStream.WriteTimeout = value;
			}
		}
		public override int ReadTimeout
		{
			get
			{
				return _backingStream.ReadTimeout;
			}
			set
			{
				_backingStream.ReadTimeout = value;
			}
		}
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return _backingStream.BeginRead(buffer, offset, count, callback, state);
		}
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return _backingStream.BeginWrite(buffer, offset, count, callback, state);
		}
		public override int EndRead(IAsyncResult asyncResult)
		{
			return _backingStream.EndRead(asyncResult);
		}
		public override void EndWrite(IAsyncResult asyncResult)
		{
			_backingStream.EndWrite(asyncResult);
		}
		public override void WriteByte(byte value)
		{
			_backingStream.WriteByte(value);
		}
	}
	private sealed class UncloseableStream : WrappedStream
	{
		public UncloseableStream(Stream backingStream)
			: base(backingStream)
		{
		}
		public override void Close()
		{
			// This is the only thing that changes
		}
	}

	private const string ITEMMETADATA_DEPENDENTUPON = "DependentUpon";
	private const string ITEMMETADATA_GENERATOR = "Generator";
	private const string ITEMMETADATA_ORMGENERATOR = "ORMGenerator";
	private const string ITEMMETADATA_AUTOGEN = "AutoGen";
	private const string ITEMMETADATA_DESIGNTIME = "DesignTime";

	private static readonly XmlResolver XmlResolver = new XmlUrlResolver();
	private static readonly XmlReaderSettings XmlReaderSettings = InitializeXmlReaderSettings();
	private static XmlReaderSettings InitializeXmlReaderSettings()
	{
		XmlReaderSettings settings = new XmlReaderSettings();
		settings.CloseInput = false;
		settings.IgnoreComments = true;
		settings.IgnoreWhitespace = true;
		settings.XmlResolver = XmlResolver;
		return settings;
	}

	public void GenerateOutput(BuildItem buildItem, Stream outputStream, IDictionary<string, Stream> inputFormatStreams, string defaultNamespace)
	{
		outputStream = new UncloseableStream(outputStream);
		Stream undeadOial = inputFormatStreams["UndeadOIAL"];
		Stream liveOial = inputFormatStreams["LiveOIAL"];

		XmlDiff xmlDiff = new XmlDiff(XmlDiffOptions.IgnoreComments | XmlDiffOptions.IgnoreXmlDecl | XmlDiffOptions.IgnorePrefixes);

		xmlDiff.Algorithm = XmlDiffAlgorithm.Precise;

		bool identical = false;

		MemoryStream diffgram = new MemoryStream(8192);

		using (XmlWriter diffgramWriter = XmlWriter.Create(diffgram))
		{
			try
			{
				using (XmlReader undeadReader = XmlReader.Create(undeadOial, XmlReaderSettings), liveReader = XmlReader.Create(liveOial, XmlReaderSettings))
				{
					identical = xmlDiff.Compare(undeadReader, liveReader, diffgramWriter);
				}
			}
			finally
			{
				undeadOial.Seek(0, SeekOrigin.Begin);
				liveOial.Seek(0, SeekOrigin.Begin);
			}
		}

		// Files have been compared, and the diff has been written to the diffgramwriter.

		TextWriter resultHtml = new StreamWriter(outputStream);
		resultHtml.WriteLine("<html><head>");
		resultHtml.WriteLine("<style TYPE='text/css' MEDIA='screen'>");
		resultHtml.Write("<!-- td { font-family: Courier New; font-size:14; } " +
							"th { font-family: Arial; } " +
							"p { font-family: Arial; } -->");
		resultHtml.WriteLine("</style></head>");
		resultHtml.WriteLine("<body><h3 style='font-family:Arial'>XmlDiff view</h3><table border='0'><tr><td><table border='0'>");
		resultHtml.WriteLine("<tr><th>Undead OIAL</th><th>Live OIAL</th></tr>" +
							"<tr><td colspan=2><hr size=1></td></tr>");

		if (identical)
		{
			resultHtml.WriteLine("<tr><td colspan='2' align='middle'>Files are identical.</td></tr>");
		}
		else
		{
			resultHtml.WriteLine("<tr><td colspan='2' align='middle'>Files are different.</td></tr>");
		}

		diffgram.Seek(0, SeekOrigin.Begin);
		XmlDiffView xmlDiffView = new XmlDiffView();

		XmlTextReader sourceReader = new XmlTextReader(undeadOial);

		sourceReader.XmlResolver = null;

		xmlDiffView.Load(sourceReader, new XmlTextReader(diffgram));

		xmlDiffView.GetHtml(resultHtml);

		resultHtml.WriteLine("</table></table></body></html>");

		resultHtml.Flush();
		resultHtml.Close();
	}

	public BuildItem AddGeneratedFileBuildItem(BuildItemGroup buildItemGroup, string sourceFileName, string outputFileName)
	{
		if (outputFileName == null || outputFileName.Length == 0)
		{
			outputFileName = GetOutputFileDefaultName(sourceFileName);
		}
		BuildItem buildItem = buildItemGroup.AddNewItem("None", outputFileName);
		buildItem.SetMetadata(ITEMMETADATA_AUTOGEN, "True");
		buildItem.SetMetadata(ITEMMETADATA_DEPENDENTUPON, sourceFileName);
		buildItem.SetMetadata(ITEMMETADATA_ORMGENERATOR, this.OfficialName);
		return buildItem;
	}

	public string DisplayDescription
	{
		get
		{
			return "OIALDiff";
		}
	}

	public string DisplayName
	{
		get
		{
			return "OIALDiff";
		}
	}

	public bool GeneratesSupportFile
	{
		get
		{
			return false;
		}
	}

	public string GetOutputFileDefaultName(string sourceFileName)
	{
		return Path.ChangeExtension(sourceFileName, ".OIALDiff.html");
	}

	public string OfficialName
	{
		get
		{
			return "OIALDiff";
		}
	}

	public string ProvidesOutputFormat
	{
		get
		{
			return "OIALDiff";
		}
	}

	private static readonly ReadOnlyCollection<string> _requiresInputFormats = Array.AsReadOnly(new string[] { "UndeadOIAL", "LiveOIAL" });
	public IList<string> RequiresInputFormats
	{
		get
		{
			return _requiresInputFormats;
		}
	}
}