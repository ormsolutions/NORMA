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

namespace Neumont.Tools.ORM.ORMCustomTool
{
	public sealed partial class ORMCustomTool
	{
		private sealed class ReadOnlyStream : Stream
		{
			private readonly Stream _backingStream;
			public ReadOnlyStream(Stream backingStream)
				: base()
			{
				this._backingStream = backingStream;
				base.Close();
			}

			public void CloseBackingStream()
			{
				this._backingStream.Close();
			}

			public override bool CanRead
			{
				get { return this._backingStream.CanRead; }
			}

			public override bool CanSeek
			{
				get { return this._backingStream.CanSeek; }
			}

			public override bool CanWrite
			{
				get { return false; }
			}

			public override void Flush()
			{
				throw new NotSupportedException();
			}

			public override long Length
			{
				get { return this._backingStream.Length; }
			}

			public override long Position
			{
				get
				{
					return this._backingStream.Position;
				}
				set
				{
					this._backingStream.Position = value;
				}
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				return this._backingStream.Read(buffer, offset, count);
			}

			public override int ReadByte()
			{
				return this._backingStream.ReadByte();
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				return this._backingStream.Seek(offset, origin);
			}

			public override void SetLength(long value)
			{
				throw new NotSupportedException();
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException();
			}
		}
	}
}
