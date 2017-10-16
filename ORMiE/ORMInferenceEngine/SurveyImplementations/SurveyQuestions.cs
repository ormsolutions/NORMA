using System;
using ORMSolutions.ORMArchitect.Framework.Design;
using System.ComponentModel;

namespace unibz.ORMInferenceEngine
{
	[TypeConverter(typeof(EnumConverter<SurveyRootElementType, Hierarchy>))]
	public enum SurveyRootElementType
	{
		/// <summary>
		/// Display as a top level object type.
		/// </summary>
		TopLevelObjectType,
		/// <summary>
		/// Display Inferred Constraint as a top level group element
		/// </summary>
		InferredConstraint,
		/// <summary>
		/// Display Unsatisfiable Object Type as a top level group element
		/// </summary>
		UnsatisfiableElement,


	}
}
