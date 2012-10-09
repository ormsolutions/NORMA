#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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

#region Using Directives
using System;
using Microsoft.VisualStudio.Package;
#endregion
namespace ORMSolutions.ORMArchitect.Core.Shell
{
#if FACTEDITOR_TIPTEXT
	partial class FactEditorLanguageService
	{
		/// <summary>
		/// Wraps a TokenInfo class to provide more information.
		/// </summary>
		private sealed class FactTokenInfo : TokenInfo
		{
			#region Private Members
			private string m_Value;
#if FACTEDITOR_TIPTEXT
			private bool m_ExistsOnModel;
#endif // FACTEDITOR_TIPTEXT
			#endregion // Private Members
			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="FactTokenInfo"/> class.
			/// </summary>
			public FactTokenInfo()
				: base()
			{
				m_Value = String.Empty;
			}

			public FactTokenInfo(int startIndex, int endIndex, TokenType type)
				: this(String.Empty, startIndex, endIndex, type)
			{ }

			public FactTokenInfo(string value, int startIndex, int endIndex, TokenType type)
				: base(startIndex, endIndex, type)
			{
				m_Value = value;
			}
			#endregion // Constructors
			#region Public Properties
			/// <summary>
			/// Gets the value (text) of the token.
			/// </summary>
			public string Value
			{
				get
				{
					return m_Value;
				}
				set
				{
					m_Value = value;
				}
			}

			/// <summary>
			/// Gets the length of the token.
			/// </summary>
			public int Length
			{
				get
				{
					return (this.EndIndex + 1) - this.StartIndex;
				}
			}

#if FACTEDITOR_TIPTEXT
			/// <summary>
			/// Gets or sets a value indicating whether there is an ObjectType on the model
			/// with the name as this token's value.
			/// </summary>
			/// <value>
			///   <c>true</c> if there is an ObjectType that exists on model with the same name;
			///   otherwise, <c>false</c>.
			/// </value>
			public bool ExistsOnModel
			{
				get
				{
					return m_ExistsOnModel;
				}
				set
				{
					m_ExistsOnModel = value;
				}
			}
#endif // FACTEDITOR_TIPTEXT
			#endregion // Public Properties
		}
	}
#endif // FACTEDITOR_TIPTEXT
}
