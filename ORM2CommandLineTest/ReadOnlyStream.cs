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
using System.IO;

namespace Neumont.Tools.ORM.SDK.TestReportViewer
{
	public sealed partial class ReportViewer
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
		private sealed class ReadOnlyStream : WrappedStream
		{
			public ReadOnlyStream(Stream backingStream)
				: base(backingStream)
			{
			}
			public override void Close()
			{
				// Don't close the backing stream
			}
			public override bool CanWrite
			{
				get { return false; }
			}
			public override void Flush()
			{
				throw new NotSupportedException();
			}
			public override void SetLength(long value)
			{
				throw new NotSupportedException();
			}
			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException();
			}
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				throw new NotSupportedException();
			}
			public override void WriteByte(byte value)
			{
				throw new NotSupportedException();
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
	}
}
