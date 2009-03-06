using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace ORMSolutions.ORMArchitect.CustomProperties
{
	#region CustomPropertyVerbalizationSnippetType enum
	/// <summary>An enum with one value for each recognized snippet</summary>
	public enum CustomPropertyVerbalizationSnippetType
	{
		/// <summary>The 'CustomPropertiesVerbalization' format string snippet. Contains 4 replacement fields.</summary>
		CustomPropertiesVerbalization,
	}
	#endregion // CustomPropertyVerbalizationSnippetType enum
	#region CustomPropertyVerbalizationSets class
	/// <summary>A class deriving from VerbalizationSets.</summary>
	public class CustomPropertyVerbalizationSets : VerbalizationSets<CustomPropertyVerbalizationSnippetType>
	{
		/// <summary>The default verbalization snippet set. Contains english HTML snippets.</summary>
		public static readonly CustomPropertyVerbalizationSets Default = (CustomPropertyVerbalizationSets)VerbalizationSets<CustomPropertyVerbalizationSnippetType>.Create<CustomPropertyVerbalizationSets>(null);
		/// <summary>Populates the snippet sets of the CustomPropertyVerbalizationSets object.</summary>
		/// <param name="sets">The sets to be populated.</param>
		/// <param name="userData">User-defined data passed to the Create method</param>
		protected override void PopulateVerbalizationSets(VerbalizationSet[] sets, object userData)
		{
			sets[0] = new ArrayVerbalizationSet(new string[]{
				@"<span class=""quantifier"" title=""{2}.{0}: {3}"">{0} = </span> {1}"});
			sets[1] = sets[0];
			sets[2] = sets[0];
			sets[3] = sets[0];
		}
		/// <summary>Converts enum value of CustomPropertyVerbalizationSnippetType to an integer index value.</summary>
		protected override int ValueToIndex(CustomPropertyVerbalizationSnippetType enumValue)
		{
			return (int)enumValue;
		}
	}
	#endregion // CustomPropertyVerbalizationSets class
}
