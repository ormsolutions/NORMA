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
		private sealed class StringStream : Stream
		{
			private long _position;
			private string _value;
			public StringStream(string value)
				: base()
			{
				this._position = 0;
				this._value = value;
				base.Close();
			}

			public override bool CanRead
			{
				get { return true; }
			}

			public override bool CanSeek
			{
				get { return true; }
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
				get { return this._value.Length * System.Text.UnicodeEncoding.CharSize; }
			}

			public override long Position
			{
				get
				{
					return this._position;
				}
				set
				{
					this._position = value;
				}
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				// HACK: There are potentially some weird edge cases here, but this should do what we need.
				int bytesRead = (System.Text.Encoding.UTF8 as System.Text.UTF8Encoding).GetBytes(this._value, (int)this._position / 2, count / 2, buffer, offset);
				this._position += bytesRead;
				return bytesRead;
			}

			public override int ReadByte()
			{
				char c = this._value[(int)this._position++ / 2];
				// These may seem backwards, but remember, we already incremented position.
				return (this._position % 2 == 0 ? c & 0x000000ff : c >> 8);
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				switch (origin)
				{
					case SeekOrigin.Begin:
						return this._position = offset;
					case SeekOrigin.Current:
						return this._position += offset;
					case SeekOrigin.End:
						return this._position = this.Length + offset;
				}
				throw new System.ComponentModel.InvalidEnumArgumentException("origin", (int)origin, typeof(SeekOrigin));
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
