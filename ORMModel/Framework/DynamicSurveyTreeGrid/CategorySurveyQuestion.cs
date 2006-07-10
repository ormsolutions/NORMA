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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid
{
	/// <summary>
	/// element type enum question answers
	/// </summary>
	public enum ElementType
	{	
		/// <summary>
		/// ORM element Object Type
		/// </summary>
		ObjectType,
		/// <summary>
		/// ORM element Fact Type
		/// </summary>
		FactType,
		/// <summary>
		/// ORM element Constraint
		/// </summary>
		Constraint,
	}
	/// <summary>
	/// error state enum question answers
	/// </summary>
	public enum ErrorState
	{
		/// <summary>
		/// ORM element contains error
		/// </summary>
		HasError,
		/// <summary>
		/// ORM element does not contain an error
		/// </summary>
		NoError,
		/// <summary>
		/// can not be determined whether the ORM element contains an error
		/// </summary>
		Inconclusive
	}
}
