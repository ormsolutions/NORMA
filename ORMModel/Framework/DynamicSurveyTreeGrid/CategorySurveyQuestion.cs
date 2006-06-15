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
