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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.Modeling;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.ObjectModel.Verbalization
{
	#region VerbalizationReportGenerator and Enum
	/// <summary>
	/// Determines the elements of the Model to report
	/// </summary>
	[global::System.ComponentModel.TypeConverter(typeof(global::Neumont.Tools.Modeling.Design.EnumConverter<VerbalizationReportContent, global::Neumont.Tools.ORM.ObjectModel.ORMModel>))]
	[Flags]
	public enum VerbalizationReportContent
	{
		/// <summary>
		/// Reports all ObjectTypes in the Model
		/// </summary>
		ObjectTypes = 1,
		/// <summary>
		/// Reports all FactTypes in the Model
		/// </summary>
		FactTypes = 2,
		/// <summary>
		/// Reports all Internal Uniquness constraints in the Model
		/// </summary>
		InternalUniquenessConstraints = 4,
		/// <summary>
		/// Reports all External Uniqueness constraints in the Model
		/// </summary>
		ExternalUniquenessConstraints = 8,
		/// <summary>
		/// Reports all Subset constraints in the Model
		/// </summary>
		SubsetConstraints = 0x10,
		/// <summary>
		/// Reports all Equality constraints in the Model
		/// </summary>
		EqualityConstraints = 0x20,
		/// <summary>
		/// Reports all Exclusion constraints in the Model
		/// </summary>
		ExclusionConstraints = 0x40,
		/// <summary>
		/// Reports all Ring constraints in the Model
		/// </summary>
		RingConstraints = 0x80,
		/// <summary>
		/// Reports all Frequency constraints in the Model
		/// </summary>
		FrequencyConstraints = 0x100,
		/// <summary>
		/// Reports all constraints in the Model in a format to be used in validating with the Domain Expert
		/// </summary>
		ValidationReport = 0x200,
		/// <summary>
		/// Reports all Disjunctive Mandatory constraints in the Model
		/// </summary>
		DisjunctiveMandatoryConstraints = 0x400,
		/// <summary>
		/// Reports all Simple Mandatory constraints in the Model
		/// </summary>
		SimpleMandatoryConstraints = 0x800,
		/// <summary>
		/// Reports all Set Constraints in the Model
		/// </summary>
		AllSetConstraints = (InternalUniquenessConstraints | ExternalUniquenessConstraints | RingConstraints | FrequencyConstraints | SimpleMandatoryConstraints | DisjunctiveMandatoryConstraints),
		/// <summary>
		/// Reports all Set Constraints in the Model
		/// </summary>
		AllSetComparisonConstraints = (SubsetConstraints | EqualityConstraints | ExclusionConstraints),
		/// <summary>
		/// Reports all constraints in the Model
		/// </summary>
		AllConstraints = (AllSetConstraints | AllSetComparisonConstraints),
		/// <summary>
		/// Reports all elements in the Model
		/// </summary>
		All = ObjectTypes | FactTypes | AllConstraints | ValidationReport,
	}
	/// <summary>
	/// Provides methods for generating Verbalization Reports
	/// </summary>
	public class VerbalizationReportGenerator
	{
		/// <summary>
		/// Generates a report of the Verbalizations
		/// </summary>
		/// <param name="model">The model to report on</param>
		/// <param name="reportContent">The filter to apply to the report</param>
		/// <param name="baseDir">The base directory to output the report</param>
		public static void GenerateReport(ORMModel model, VerbalizationReportContent reportContent, string baseDir)
		{
			bool isNegative = false;
			IDictionary<Type, IVerbalizationSets> snippetsDictionary = (model.Store as IORMToolServices).GetVerbalizationSnippetsDictionary(VerbalizationTarget.Report);
			IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			Dictionary<IVerbalize, IVerbalize> verbalized;
			Stream fileStream;
			TextWriter textWriter;
			VerbalizationCallbackWriter writer;

			ObjectType[] objectTypeList = null;
			if (0 != (reportContent & VerbalizationReportContent.ObjectTypes))
			{
				verbalized = new Dictionary<IVerbalize, IVerbalize>();
				objectTypeList = model.ObjectTypeCollection.ToArray();

				int objectTypeCount = objectTypeList.Length;
				if (objectTypeCount != 0)
				{
					Array.Sort<ObjectType>(objectTypeList, NamedElementComparer<ObjectType>.CurrentCulture);
					// Write Object Type List Report page
					fileStream = new FileStream(Path.Combine(baseDir, "ObjectTypeList.html"), FileMode.Create, FileAccess.ReadWrite);
					textWriter = new StreamWriter(fileStream);
					writer = new VerbalizationReportCallbackWriter(snippetsDictionary, textWriter);

					bool firstCall = true;
					IVerbalizeCustomChildren childVerbalize = new ObjectTypeListReport(objectTypeList, snippets, ReportVerbalizationSnippetType.ObjectTypeListHeader, ReportVerbalizationSnippetType.ObjectTypeListFooter, reportContent) as IVerbalizeCustomChildren;
					VerbalizationHelper.VerbalizeElement(
						childVerbalize,
						snippetsDictionary,
						verbalized,
						isNegative,
						writer,
						false,
						ref firstCall);
					textWriter.Flush();
					textWriter.Close();

					string objectTypeDir = Path.Combine(baseDir, "ObjectTypes");
					if (!Directory.Exists(objectTypeDir)) Directory.CreateDirectory(objectTypeDir);

					// Write individual object type pages
					for (int i = 0; i < objectTypeCount; ++i)
					{
						bool firstCallPending = true;

						fileStream = new FileStream(Path.Combine(objectTypeDir, objectTypeList[i].Name + ".html"), FileMode.Create, FileAccess.ReadWrite);
						textWriter = new StreamWriter(fileStream);
						writer = new VerbalizationReportCallbackWriter(snippetsDictionary, textWriter);

						IVerbalizeCustomChildren child = new ObjectTypePageReport(objectTypeList[i], reportContent, snippets) as IVerbalizeCustomChildren;
						VerbalizationHelper.VerbalizeElement(
							child,
							snippetsDictionary,
							verbalized,
							isNegative,
							writer,
							false,
							ref firstCallPending);

						textWriter.Flush();
						textWriter.Close();
					}
				}
			}

			FactType[] factTypeList = null;
			if (0 != (VerbalizationReportContent.FactTypes & reportContent))
			{
				factTypeList = model.FactTypeCollection.ToArray();
				verbalized = new Dictionary<IVerbalize, IVerbalize>();

				int factCount = factTypeList.Length;
				if (factCount != 0)
				{
					string factTypeDir = Path.Combine(baseDir, "FactTypes");
					if (!Directory.Exists(factTypeDir)) Directory.CreateDirectory(factTypeDir);

					Array.Sort<FactType>(factTypeList, NamedElementComparer<FactType>.CurrentCulture);
					for (int i = 0; i < factCount; ++i)
					{
						bool firstCallPending = true;

						fileStream = new FileStream(Path.Combine(factTypeDir, factTypeList[i].Name + ".html"), FileMode.Create, FileAccess.ReadWrite);
						textWriter = new StreamWriter(fileStream);
						writer = new VerbalizationReportCallbackWriter(snippetsDictionary, textWriter);

						IVerbalizeCustomChildren child = new FactTypePageReport(factTypeList[i], reportContent, snippets) as IVerbalizeCustomChildren;
						VerbalizationHelper.VerbalizeElement(
							child,
							snippetsDictionary,
							verbalized,
							isNegative,
							writer,
							false,
							ref firstCallPending);

						textWriter.Flush();
						textWriter.Close();
					}
				}
			}

			if (0 != (reportContent & VerbalizationReportContent.ValidationReport))
			{
				verbalized = new Dictionary<IVerbalize, IVerbalize>();
				bool firstCall = true;
				if (factTypeList == null)
				{
					factTypeList = model.FactTypeCollection.ToArray();
					Array.Sort<FactType>(factTypeList, NamedElementComparer<FactType>.CurrentCulture);
				}

				if (factTypeList.Length != 0)
				{
					fileStream = new FileStream(Path.Combine(baseDir, "ConstraintValidationReport.html"), FileMode.Create, FileAccess.ReadWrite);
					textWriter = new StreamWriter(fileStream);
					writer = new VerbalizationReportCallbackWriter(snippetsDictionary, textWriter);


					IVerbalizeCustomChildren child = new FactTypeConstraintValidationListReport(factTypeList, reportContent, snippets) as IVerbalizeCustomChildren;
					VerbalizationHelper.VerbalizeElement(
						child,
						snippetsDictionary,
						verbalized,
						isNegative,
						writer,
						false,
						ref firstCall);

					textWriter.Flush();
					textWriter.Close();
				}
			}
		}
		/// <summary>
		/// Retrieves a list of Constraints in the specified Model filtered with the given filter
		/// </summary>
		private static IList<IConstraint> FilterConstraints(ORMModel model, VerbalizationReportContent reportContent)
		{
			IList<IConstraint> constraintList = new List<IConstraint>();
			if (0 != (reportContent & VerbalizationReportContent.AllSetConstraints))
			{
				LinkedElementCollection<SetConstraint> setConstraints = model.SetConstraintCollection;
				int setConstraintCount = setConstraints.Count;
				for (int i = 0; i < setConstraintCount; ++i)
				{
					if ((setConstraints[i] as IConstraint).ConstraintType == ConstraintType.DisjunctiveMandatory &&
						(0 != (reportContent & VerbalizationReportContent.DisjunctiveMandatoryConstraints)))
					{
						constraintList.Add(setConstraints[i].Constraint);
					}
					else if ((setConstraints[i] as IConstraint).ConstraintType == ConstraintType.ExternalUniqueness &&
						(0 != (reportContent & VerbalizationReportContent.ExternalUniquenessConstraints)))
					{
						constraintList.Add(setConstraints[i].Constraint);
					}
					else if ((setConstraints[i] as IConstraint).ConstraintType == ConstraintType.Frequency &&
						(0 != (reportContent & VerbalizationReportContent.FrequencyConstraints)))
					{
						constraintList.Add(setConstraints[i].Constraint);
					}
					else if ((setConstraints[i] as IConstraint).ConstraintType == ConstraintType.InternalUniqueness &&
						(0 != (reportContent & VerbalizationReportContent.InternalUniquenessConstraints)))
					{
						constraintList.Add(setConstraints[i].Constraint);
					}
					else if ((setConstraints[i] as IConstraint).ConstraintType == ConstraintType.Ring &&
						(0 != (reportContent & VerbalizationReportContent.RingConstraints)))
					{
						constraintList.Add(setConstraints[i].Constraint);
					}
					else if ((setConstraints[i] as IConstraint).ConstraintType == ConstraintType.SimpleMandatory &&
						(0 != (reportContent & VerbalizationReportContent.SimpleMandatoryConstraints)))
					{
						constraintList.Add(setConstraints[i].Constraint);
					}
				}
			}
			if (0 != (reportContent & VerbalizationReportContent.AllSetComparisonConstraints))
			{
				LinkedElementCollection<SetComparisonConstraint> setComparisonConstraints = model.SetComparisonConstraintCollection;
				int constraintCount = setComparisonConstraints.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					if ((setComparisonConstraints[i] as IConstraint).ConstraintType == ConstraintType.Equality &&
						(0 != (reportContent & VerbalizationReportContent.EqualityConstraints)))
					{
						constraintList.Add(setComparisonConstraints[i]);
					}
					else if ((setComparisonConstraints[i] as IConstraint).ConstraintType == ConstraintType.Exclusion &&
						(0 != (reportContent & VerbalizationReportContent.ExclusionConstraints)))
					{
						constraintList.Add(setComparisonConstraints[i]);
					}
					else if ((setComparisonConstraints[i] as IConstraint).ConstraintType == ConstraintType.Subset &&
						(0 != (reportContent & VerbalizationReportContent.SubsetConstraints)))
					{
						constraintList.Add(setComparisonConstraints[i]);
					}
				}
			}
			return constraintList;
		}
	}
	#endregion // VerbalizationReport

	#region VerbalizationReportWrapper class
	/// <summary>
	/// Represents a wrapper used to wrap IVerbalize objects with snippets
	/// </summary>
	public abstract class VerbalizationReportWrapper : IVerbalize
	{
		/// <summary>
		/// The object to wrap with a snippet
		/// </summary>
		protected IVerbalize myVerbalizationObject;
		/// <summary>
		/// The verbalization report snippet set
		/// </summary>
		protected IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets;

		/// <summary>
		/// Initializes a new instance of VerbalizationReportWrapper
		/// </summary>
		/// <param name="snippets"></param>
		public VerbalizationReportWrapper(IVerbalizationSets<ReportVerbalizationSnippetType> snippets)
		{
			mySnippets = snippets;
		}
		/// <summary>
		/// Initializes a new instance of VerbalizationReportWrapper
		/// </summary>
		/// <param name="snippets"></param>
		/// <param name="verbalizationObject"></param>
		public VerbalizationReportWrapper(IVerbalizationSets<ReportVerbalizationSnippetType> snippets, IVerbalize verbalizationObject)
		{
			mySnippets = snippets;
			myVerbalizationObject = verbalizationObject;
		}

		#region IVerbalize Members
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
		#endregion

		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		public abstract bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative);
		/// <summary>
		/// Provides a wrapper for the ObjectType name value
		/// </summary>
		public class ObjectTypeVerbalizationWrapper : VerbalizationReportWrapper
		{
			private ReportVerbalizationSnippetType myObjectTypeSnippet;

			/// <summary>
			/// Initializes a new instance of FactTypeVerbalizationWrapper
			/// </summary>
			/// <param name="objectTypeSnippet"></param>
			/// <param name="snippets"></param>
			/// <param name="verbalizationObject"></param>
			public ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType objectTypeSnippet, IVerbalizationSets<ReportVerbalizationSnippetType> snippets, IVerbalize verbalizationObject)
				: base(snippets, verbalizationObject)
			{
				myObjectTypeSnippet = objectTypeSnippet;
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			public override bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
			{
				bool retVal = true;
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemOpen));
				writer.Write(string.Format(writer.FormatProvider, mySnippets.GetSnippet(myObjectTypeSnippet),
					(myVerbalizationObject as ObjectType).Name));
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemClose));
				return retVal;
			}
		}
		/// <summary>
		/// Provides a wrapper for FactType verbalizations
		/// </summary>
		public class FactTypeVerbalizationWrapper : VerbalizationReportWrapper
		{
			/// <summary>
			/// Initializes a new instance of FactTypeVerbalizationWrapper
			/// </summary>
			/// <param name="snippets"></param>
			/// <param name="verbalizationObject"></param>
			public FactTypeVerbalizationWrapper(IVerbalizationSets<ReportVerbalizationSnippetType> snippets, IVerbalize verbalizationObject)
				: base(snippets, verbalizationObject) { }
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			public override bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
			{
				bool retVal;
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemOpen));
				writer.Write(string.Format(mySnippets.GetSnippet(ReportVerbalizationSnippetType.FactTypeRelationshipLinkOpen), (myVerbalizationObject as FactType).Name));
				retVal = myVerbalizationObject.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.FactTypeRelationshipLinkClose));
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemClose));
				return retVal;
			}
		}
		/// <summary>
		/// Represents a list of ObjectTypes for a Verbalization Report
		/// </summary>
		public class ObjectTypeListWrapper : VerbalizationReportWrapper
		{
			private IList<ObjectType> myObjectTypeList;
			private ReportVerbalizationSnippetType myHeaderSnippet;
			private ReportVerbalizationSnippetType myFooterSnippet;

			/// <summary>
			/// Initializes a new instance of FactTypeVerbalizationWrapper
			/// </summary>
			public ObjectTypeListWrapper(IList<ObjectType> objectTypeList, IVerbalizationSets<ReportVerbalizationSnippetType> snippets,
				ReportVerbalizationSnippetType headerSnippet, ReportVerbalizationSnippetType footerSnippet)
				: base(snippets, null)
			{
				myObjectTypeList = objectTypeList;
				myHeaderSnippet = headerSnippet;
				myFooterSnippet = footerSnippet;
			}
			/// <summary>
			/// Implements IVerbalize.GetVerbalization
			/// </summary>
			public override bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
			{
				beginVerbalization(VerbalizationContent.Normal);
				bool retVal = true;
				writer.Write(mySnippets.GetSnippet(myHeaderSnippet));
				int objectCount = myObjectTypeList.Count;
				for (int i = 0; i < objectCount; ++i)
				{
					IVerbalize instance = new VerbalizationReportWrapper.ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeListObjectTypeValueLink, mySnippets, myObjectTypeList[i] as IVerbalize) as IVerbalize;
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
				}
				writer.Write(mySnippets.GetSnippet(myFooterSnippet));
				return retVal;
			}
		}
		/// <summary>
		/// Provides a wrapper for verbalizing Constraints
		/// </summary>
		public class ConstraintVerbalizationWrapper : VerbalizationReportWrapper
		{
			private ReportVerbalizationSnippetType myOpeningSnippet;
			private ReportVerbalizationSnippetType myClosingSnippet;

			/// <summary>
			/// Initializes a new instance of ConstraintVerbalizationWrapper
			/// </summary>
			/// <param name="openingSnippet"></param>
			/// <param name="closingSnippet"></param>
			/// <param name="snippets"></param>
			/// <param name="verbalizationObject"></param>
			public ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType openingSnippet, ReportVerbalizationSnippetType closingSnippet, IVerbalizationSets<ReportVerbalizationSnippetType> snippets, IVerbalize verbalizationObject)
				: base(snippets, verbalizationObject)
			{
				myOpeningSnippet = openingSnippet;
				myClosingSnippet = closingSnippet;
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			public override bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
			{
				bool retVal = false;
				writer.Write(mySnippets.GetSnippet(myOpeningSnippet), (myVerbalizationObject as ORMNamedElement).Name,
					(myVerbalizationObject as IConstraint).ConstraintType.ToString());
				retVal = myVerbalizationObject.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
				writer.Write(mySnippets.GetSnippet(myClosingSnippet));
				return retVal;
			}
		}
		/// <summary>
		/// Provides a wrapper for verbalizing Constraints
		/// </summary>
		public class ConstraintNameVerbalizationWrapper : VerbalizationReportWrapper
		{
			/// <summary>
			/// Initializes a new instance of ConstraintNameVerbalizationWrapper
			/// </summary>
			/// <param name="snippets"></param>
			/// <param name="verbalizationObject"></param>
			public ConstraintNameVerbalizationWrapper(IVerbalizationSets<ReportVerbalizationSnippetType> snippets, IVerbalize verbalizationObject)
				: base(snippets, verbalizationObject) { }
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			public override bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
			{
				bool retVal = false;
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemLink), "Constraints", (myVerbalizationObject as ORMNamedElement).Name);
				return retVal;
			}
		}
		/// <summary>
		/// Represents the Summary section of the FactType page Verbalization Report
		/// </summary>
		public class FactTypePageHeaderSummary : VerbalizationReportWrapper
		{
			/// <summary>
			/// Initializes a new instance of FactTypePageHeaderSummary
			/// </summary>
			public FactTypePageHeaderSummary(IVerbalizationSets<ReportVerbalizationSnippetType> snippets, IVerbalize verbalizationObject)
				: base(snippets, verbalizationObject) { }
			/// <summary>
			/// Implements IVerbalize.GetVerbalization
			/// </summary>
			public override bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
			{
				beginVerbalization(VerbalizationContent.Normal);
				mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				bool retVal = false;
				writer.Write(string.Format(writer.FormatProvider, mySnippets.GetSnippet(ReportVerbalizationSnippetType.FactTypePageHeader),
					(myVerbalizationObject as FactType).Name));
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSummaryOpen));
				retVal = myVerbalizationObject.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSummaryClose));
				return retVal;
			}
		}
	}
	#endregion

	#region VerbalizationCallbackWriters
	/// <summary>
	/// Handles the writing of utility snippets for Verbalization targets
	/// </summary>
	public class VerbalizationCallbackWriter
	{
		/// <summary>
		/// Writer used to write Verbalizations
		/// </summary>
		private TextWriter myWriter;
		/// <summary>
		/// Replacement fields for CSS stylesheet declaration
		/// </summary>
		private string[] myDocumentHeaderReplacementFields;
		/// <summary>
		/// List of Core Verbalization Snippets
		/// </summary>
		private IVerbalizationSets<CoreVerbalizationSnippetType> myCoreSnippets;

		/// <summary>
		/// Initializes a new instance of <see cref="VerbalizationCallbackWriter"/>
		/// </summary>
		/// <param name="snippets">The <see cref="IVerbalizationSets"/> containing the Verbalization snippet set to use</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to</param>
		public VerbalizationCallbackWriter(IVerbalizationSets<CoreVerbalizationSnippetType> snippets, TextWriter writer)
		{
			myCoreSnippets = snippets;
			myWriter = writer;
			writer.NewLine = snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerNewLine);
		}
		/// <summary>
		/// Initializes a new instance of <see cref="VerbalizationCallbackWriter"/>
		/// </summary>
		/// <param name="snippets">The <see cref="IVerbalizationSets"/> containing the Verbalization snippet set to use</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to</param>
		/// <param name="documentHeaderReplacementFields">The replacement fields used to define the colors for the CSS applied to snippets</param>
		public VerbalizationCallbackWriter(IVerbalizationSets<CoreVerbalizationSnippetType> snippets, TextWriter writer, string[] documentHeaderReplacementFields)
			: this(snippets, writer)
		{
			myDocumentHeaderReplacementFields = documentHeaderReplacementFields;
		}
		/// <summary>
		/// Retrieves replacement fields for the Document Header snippet from the given IORMFontAndColorService implementation
		/// </summary>
		/// <param name="colorService"></param>
		/// <param name="snippets"></param>
		/// <returns></returns>
		public static string[] GetDocumentHeaderReplacementFields(IORMFontAndColorService colorService, IVerbalizationSets<CoreVerbalizationSnippetType> snippets)
		{
			string[] retVal;
			// The replacement fields, pulled from VerbalizationGenerator.xsd
			//{0} font-family
			//{1} font-size
			//{2} predicate text color
			//{3} predicate text bold
			//{4} object name color
			//{5} object name bold
			//{6} formal item color
			//{7} formal item bold
			//{8} notes item color
			//{9} notes item bold
			//{10} refmode item color
			//{11} refmode item bold
			//{12} instance value item color
			//{13} instance value item bold
			string boldWeight = snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerFontWeightBold);
			string normalWeight = snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerFontWeightNormal);
			retVal = new string[] { "Tahoma", "8", "darkgreen", normalWeight, "purple", normalWeight, "mediumblue", boldWeight, "brown", normalWeight, "darkgray", normalWeight, "brown", normalWeight };
			using (Font font = colorService.GetFont(ORMDesignerColorCategory.Verbalizer))
			{
				retVal[0] = font.FontFamily.Name;
				retVal[1] = (font.Size * 72f).ToString(CultureInfo.InvariantCulture);
				retVal[2] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerPredicateText));
				retVal[3] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerPredicateText) & FontStyle.Bold)) ? boldWeight : normalWeight;
				retVal[4] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerObjectName));
				retVal[5] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerObjectName) & FontStyle.Bold)) ? boldWeight : normalWeight;
				retVal[6] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerFormalItem));
				retVal[7] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerFormalItem) & FontStyle.Bold)) ? boldWeight : normalWeight;
				retVal[8] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerNotesItem));
				retVal[9] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerNotesItem) & FontStyle.Bold)) ? boldWeight : normalWeight;
				retVal[10] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerRefMode));
				retVal[11] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerRefMode) & FontStyle.Bold)) ? boldWeight : normalWeight;
				retVal[12] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerInstanceValue));
				retVal[13] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerInstanceValue) & FontStyle.Bold)) ? boldWeight : normalWeight;
			}
			return retVal;
		}
		/// <summary>
		/// Gets the underlying TextWriter
		/// </summary>
		public TextWriter Writer
		{
			get { return myWriter; }
		}
		/// <summary>
		/// Writes the Document Header defined in the current Verbalization Snippet Set
		/// </summary>
		public virtual void WriteDocumentHeader()
		{
			myWriter.Write(string.Format(CultureInfo.InvariantCulture, myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerDocumentHeader), myDocumentHeaderReplacementFields));
		}
		/// <summary>
		/// Writes the Document Footer defined in the current Verbalization Snippet Set
		/// </summary>
		public virtual void WriteDocumentFooter()
		{
			myWriter.Write(string.Format(CultureInfo.InvariantCulture, myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerDocumentFooter)));
		}
		/// <summary>
		/// Writes the verbalization opener defined in the current Verbalization Snippet Set
		/// </summary>
		public virtual void WriteOpen()
		{
			myWriter.Write(myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerOpenVerbalization));
		}
		/// <summary>
		/// Writes the verbalization closer defined in the current Verbalization Snippet Set
		/// </summary>
		public virtual void WriteClose()
		{
			myWriter.Write(myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerCloseVerbalization));
		}
		/// <summary>
		/// Writes the snippet used to increase indentation defined in the current Verbalization Snippet Set
		/// </summary>
		public virtual void IncreaseIndent()
		{
			myWriter.Write(myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerIncreaseIndent));
		}
		/// <summary>
		/// Writes the snippet used to decrease indentation defined in the current Verbalization Snippet Set
		/// </summary>
		public virtual void DecreaseIndent()
		{
			myWriter.Write(myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerDecreaseIndent));
		}
	}
	/// <summary>
	/// Handles the writing of utility snippets for the Verbalization Report target
	/// </summary>
	public class VerbalizationReportCallbackWriter : VerbalizationCallbackWriter
	{
		private IVerbalizationSets<ReportVerbalizationSnippetType> myReportSnippets;
		/// <summary>
		/// Initializes a new instance of Neumont.Tools.ORM.ObjectModel.VerbalizationReportCallbackWriter
		/// </summary>
		public VerbalizationReportCallbackWriter(IDictionary<Type, IVerbalizationSets> snippetDictionary, TextWriter writer)
			: base((IVerbalizationSets<CoreVerbalizationSnippetType>)snippetDictionary[typeof(CoreVerbalizationSnippetType)], writer)
		{
			myReportSnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetDictionary[typeof(ReportVerbalizationSnippetType)];
		}
		/// <summary>
		/// Writes the Document Header defined in the current Verbalization Snippet Set
		/// </summary>
		public override void WriteDocumentHeader()
		{
			Writer.Write(myReportSnippets.GetSnippet(ReportVerbalizationSnippetType.ReportDocumentHeader));
		}
		/// <summary>
		/// Writes the Document Footer defined in the current Verbalization Snippet Set
		/// </summary>
		public override void WriteDocumentFooter()
		{
			Writer.Write(myReportSnippets.GetSnippet(ReportVerbalizationSnippetType.ReportDocumentHeader));
		}

	}
	#endregion // VerbalizationCallbackWriters

	#region VerbalizationHelper class
	/// <summary>
	/// Provides helper methods for Verbalizations
	/// </summary>
	public static class VerbalizationHelper
	{
		/// <summary>
		/// An enum to determine callback handling during verbalization
		/// </summary>
		private enum VerbalizationResult
		{
			/// <summary>
			/// The element was successfully verbalized
			/// </summary>
			Verbalized,
			/// <summary>
			/// The element was previously verbalized
			/// </summary>
			AlreadyVerbalized,
			/// <summary>
			/// The element was not verbalized
			/// </summary>
			NotVerbalized,
		}
		/// <summary>
		/// Callback for child verbalizations
		/// </summary>
		private delegate VerbalizationResult VerbalizationHandler(VerbalizationCallbackWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IVerbalize verbalizer, int indentationLevel, bool isNegative, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel);
		/// <summary>
		/// Handles the callback made by VerbalizeElement to execute verbalization
		/// </summary>
		/// <param name="writer">The VerbalizationCallbackWriter object used to write target specific snippets</param>
		/// <param name="snippetsDictionary"></param>
		/// <param name="alreadyVerbalized"></param>
		/// <param name="verbalizer">The IVerbalize element to verbalize</param>
		/// <param name="indentationLevel">The indentation level of the verbalization</param>
		/// <param name="isNegative"></param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		/// <param name="firstWrite"></param>
		/// <param name="lastLevel"></param>
		/// <returns></returns>
		private static VerbalizationResult VerbalizeElement_VerbalizationResult(VerbalizationCallbackWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IVerbalize verbalizer, int indentationLevel, bool isNegative, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel)
		{
			if (indentationLevel == 0)
			{
				if (alreadyVerbalized.ContainsKey(verbalizer))
				{
					return VerbalizationResult.AlreadyVerbalized;
				}
			}
			bool localFirstWrite = firstWrite;
			bool localFirstCallPending = firstCallPending;
			int localLastLevel = lastLevel;
			bool retVal = verbalizer.GetVerbalization(
				writer.Writer,
				snippetsDictionary,
				delegate(VerbalizationContent content)
				{
					// Prepare for verbalization on this element. Everything
					// is delayed to this point in case the verbalization implementation
					// does not callback to the text writer.
					if (localFirstWrite)
					{
						if (localFirstCallPending)
						{
							localFirstCallPending = false;
							// Write the HTML header to the buffer
							writer.WriteDocumentHeader();
						}

						// write open tag for new verbalization
						writer.WriteOpen();
						localFirstWrite = false;
					}
					else if (writeSecondaryLines)
					{
						writer.Writer.WriteLine();
					}

					// Write indentation tags as needed
					if (indentationLevel > localLastLevel)
					{
						do
						{
							writer.IncreaseIndent();
							++localLastLevel;
						} while (localLastLevel != indentationLevel);
					}
					else if (localLastLevel > indentationLevel)
					{
						do
						{
							writer.DecreaseIndent();
							--localLastLevel;
						} while (localLastLevel != indentationLevel);
					}
				},
				isNegative);
			lastLevel = localLastLevel;
			firstWrite = localFirstWrite;
			firstCallPending = localFirstCallPending;
			if (retVal)
			{
				if (indentationLevel == 0)
				{
					alreadyVerbalized.Add(verbalizer, verbalizer);
				}
				return VerbalizationResult.Verbalized;
			}
			else
			{
				return VerbalizationResult.NotVerbalized;
			}
		}
		/// <summary>
		/// Determine the indentation level for verbalizing a ModelElement, and fire
		/// the delegate for verbalization
		/// </summary>
		/// <param name="element">The element to verbalize</param>
		/// <param name="snippetsDictionary">The default or loaded verbalization sets. Passed through all verbalization calls.</param>
		/// <param name="alreadyVerbalized">A dictionary of top-level (indentationLevel == 0) elements that have already been verbalized.</param>
		/// <param name="isNegative">Use the negative form of the reading</param>
		/// <param name="writer">The VerbalizationCallbackWriter for verbalization output</param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		public static void VerbalizeElement(ModelElement element, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, bool isNegative, VerbalizationCallbackWriter writer, bool writeSecondaryLines, ref bool firstCallPending)
		{
			int lastLevel = 0;
			bool firstWrite = true;
			bool localFirstCallPending = firstCallPending;
			VerbalizeElement(
				element,
				snippetsDictionary,
				alreadyVerbalized,
				null,
				writer,
				new VerbalizationHandler(VerbalizeElement_VerbalizationResult),
				isNegative,
				0,
				writeSecondaryLines,
				ref localFirstCallPending,
				ref firstWrite,
				ref lastLevel);
			while (lastLevel > 0)
			{
				writer.DecreaseIndent();
				--lastLevel;
			}
			// close the opening tag for the new verbalization
			if (!firstWrite)
			{
				writer.WriteClose();
			}
			firstCallPending = localFirstCallPending;
		}
		/// <summary>
		/// Determine the indentation level for verbalizing an implementation of IVerbalizeCustomChildren, and fire
		/// the delegate for verbalization
		/// </summary>
		/// <param name="customChildren">The IVerbalizeCustomChildren implementation to verbalize</param>
		/// <param name="snippetsDictionary">The default or loaded verbalization sets. Passed through all verbalization calls.</param>
		/// <param name="alreadyVerbalized">A dictionary of top-level (indentationLevel == 0) elements that have already been verbalized.</param>
		/// <param name="isNegative">Use the negative form of the reading</param>
		/// <param name="writer">The VerbalizationCallbackWriter for verbalization output</param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		public static void VerbalizeElement(IVerbalizeCustomChildren customChildren, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, bool isNegative, VerbalizationCallbackWriter writer, bool writeSecondaryLines, ref bool firstCallPending)
		{
			int lastLevel = 0;
			bool firstWrite = true;
			bool localFirstCallPending = firstCallPending;
			VerbalizeCustomChildren(customChildren, writer,
				new VerbalizationHandler(VerbalizeElement_VerbalizationResult),
				snippetsDictionary,
				alreadyVerbalized,
				isNegative,
				0,
				writeSecondaryLines,
				ref localFirstCallPending,
				ref firstWrite,
				ref lastLevel);
			while (lastLevel > 0)
			{
				writer.DecreaseIndent();
				--lastLevel;
			}
			// close the opening tag for the new verbalization
			if (!firstWrite)
			{
				writer.WriteClose();
			}
			firstCallPending = localFirstCallPending;
		}
		/// <summary>
		/// Verbalize the passed in element and all its children
		/// </summary>
		/// <param name="element"></param>
		/// <param name="snippetsDictionary"></param>
		/// <param name="alreadyVerbalized"></param>
		/// <param name="filter"></param>
		/// <param name="writer"></param>
		/// <param name="callback"></param>
		/// <param name="isNegative"></param>
		/// <param name="indentLevel"></param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		/// <param name="firstWrite"></param>
		/// <param name="lastLevel"></param>
		private static void VerbalizeElement(ModelElement element, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IVerbalizeFilterChildren filter, VerbalizationCallbackWriter writer, VerbalizationHandler callback, bool isNegative, int indentLevel, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel)
		{
			IVerbalize parentVerbalize = null;
			IRedirectVerbalization surrogateRedirect;
			if (indentLevel == 0 &&
				null != (surrogateRedirect = element as IRedirectVerbalization) &&
				null != (parentVerbalize = surrogateRedirect.SurrogateVerbalizer))
			{
				element = parentVerbalize as ModelElement;
			}
			else
			{
				parentVerbalize = element as IVerbalize;
			}
			bool disposeVerbalizer = false;
			if (filter != null && parentVerbalize != null)
			{
				CustomChildVerbalizer filterResult = filter.FilterChildVerbalizer(parentVerbalize, isNegative);
				parentVerbalize = filterResult.Instance;
				disposeVerbalizer = filterResult.Options;
			}
			try
			{
				VerbalizationResult result = (parentVerbalize != null) ? callback(writer, snippetsDictionary, alreadyVerbalized, parentVerbalize, indentLevel, isNegative, writeSecondaryLines, ref firstCallPending, ref firstWrite, ref lastLevel) : VerbalizationResult.NotVerbalized;
				if (result == VerbalizationResult.AlreadyVerbalized)
				{
					return;
				}
				bool parentVerbalizeOK = result == VerbalizationResult.Verbalized;
				bool verbalizeChildren = parentVerbalizeOK ? (element != null) : (element is IVerbalizeChildren);
				if (verbalizeChildren)
				{
					if (parentVerbalizeOK)
					{
						++indentLevel;
					}
					filter = parentVerbalize as IVerbalizeFilterChildren;
					ReadOnlyCollection<DomainRoleInfo> aggregatingList = element.GetDomainClass().AllDomainRolesPlayed;
					int aggregatingCount = aggregatingList.Count;
					for (int i = 0; i < aggregatingCount; ++i)
					{
						DomainRoleInfo roleInfo = aggregatingList[i];
						if (roleInfo.IsEmbedding)
						{
							LinkedElementCollection<ModelElement> children = roleInfo.GetLinkedElements(element);
							int childCount = children.Count;
							for (int j = 0; j < childCount; ++j)
							{
								VerbalizeElement(children[j], snippetsDictionary, alreadyVerbalized, filter, writer, callback, isNegative, indentLevel, writeSecondaryLines, ref firstCallPending, ref firstWrite, ref lastLevel);
							}
						}
					}
					// TODO: Need BeforeNaturalChildren/AfterNaturalChildren/SkipNaturalChildren settings for IVerbalizeCustomChildren
					IVerbalizeCustomChildren customChildren = parentVerbalize as IVerbalizeCustomChildren;
					VerbalizeCustomChildren(customChildren, writer, callback, snippetsDictionary, alreadyVerbalized, isNegative, indentLevel, writeSecondaryLines, ref firstCallPending, ref firstWrite, ref lastLevel);
				}
			}
			finally
			{
				if (disposeVerbalizer)
				{
					IDisposable dispose = parentVerbalize as IDisposable;
					if (dispose != null)
					{
						dispose.Dispose();
					}
				}
			}
		}
		/// <summary>
		/// Verbalizes the children specified by the given IVerbalizeCustomChildren implementation
		/// </summary>
		/// <param name="customChildren">The IVerbalizeCustomChildren implementation to verbalize</param>
		/// <param name="writer">The target specific Writer to use</param>
		/// <param name="callback">The delegate used to handle the verbalization</param>
		/// <param name="snippetsDictionary"></param>
		/// <param name="alreadyVerbalized"></param>
		/// <param name="isNegative">Whether or not the verbalization is negative</param>
		/// <param name="indentationLevel">The current level of indentation</param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		/// <param name="firstWrite"></param>
		/// <param name="lastLevel"></param>
		private static void VerbalizeCustomChildren(IVerbalizeCustomChildren customChildren, VerbalizationCallbackWriter writer, VerbalizationHandler callback, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, bool isNegative, int indentationLevel, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel)
		{
			if (customChildren != null)
			{
				foreach (CustomChildVerbalizer customChild in customChildren.GetCustomChildVerbalizations(isNegative))
				{
					IVerbalize childVerbalize = customChild.Instance;
					if (childVerbalize != null)
					{
						try
						{
							callback(writer, snippetsDictionary, alreadyVerbalized, childVerbalize, indentationLevel, isNegative, writeSecondaryLines, ref firstCallPending, ref firstWrite, ref lastLevel);
						}
						finally
						{
							if (customChild.Options)
							{
								IDisposable dispose = childVerbalize as IDisposable;
								if (dispose != null)
								{
									dispose.Dispose();
								}
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Gets the Fact Types for which the specified Object Type plays a role
		/// </summary>
		public static IList<FactType> GetFactTypesFromObjectType(ObjectType objectType)
		{
			List<FactType> factList = new List<FactType>();
			LinkedElementCollection<Role> roleCollecton = objectType.PlayedRoleCollection;
			int roleCount = roleCollecton.Count;
			for (int i = 0; i < roleCount; ++i)
			{
				if (!(roleCollecton[i].FactType is SubtypeFact) && !factList.Contains(roleCollecton[i].FactType)) factList.Add(roleCollecton[i].FactType);
			}
			factList.Sort(NamedElementComparer<FactType>.CurrentCulture);
			return (IList<FactType>)factList;
		}
		/// <summary>
		/// Retrieves a dictionary of all Constraints for the given Fact Type, filtered by the specified VerbalizationReportContent, indexed by ConstraintType
		/// </summary>
		/// <param name="factType">The Fact Type to retrieve constraints for</param>
		/// <param name="reportContent">The VerbalizationReportContent to filter constraints</param>
		/// <returns>dictionary of all Constraints for the given Fact Type, filtered by the specified VerbalizationReportContent, indexed by ConstraintType</returns>
		public static IDictionary<ConstraintType, IList<IConstraint>> GetConstraintsFromFactType(FactType factType, VerbalizationReportContent reportContent)
		{
			IDictionary<ConstraintType, IList<IConstraint>> constraintList = GetConstraintDictionary(reportContent);
			ProcessFactTypeConstraints(factType, reportContent, constraintList);
			return constraintList;
		}
		/// <summary>
		/// Processes the specified FactType and adds the appropriate constraints based on the specified filter
		/// </summary>
		private static void ProcessFactTypeConstraints(FactType factType, VerbalizationReportContent reportContent, IDictionary<ConstraintType, IList<IConstraint>> constraintList)
		{
			foreach (IFactConstraint factConstraint in factType.FactConstraintCollection)
			{
				bool addConstraint = false;
				if (factConstraint.Constraint.ConstraintType == ConstraintType.DisjunctiveMandatory && (reportContent & VerbalizationReportContent.DisjunctiveMandatoryConstraints) != 0)
				{
					addConstraint = true;
				}
				else if (factConstraint.Constraint.ConstraintType == ConstraintType.Equality && (reportContent & VerbalizationReportContent.EqualityConstraints) != 0)
				{
					addConstraint = true;
				}
				else if (factConstraint.Constraint.ConstraintType == ConstraintType.Exclusion && (reportContent & VerbalizationReportContent.ExclusionConstraints) != 0)
				{
					addConstraint = true;
				}
				else if (factConstraint.Constraint.ConstraintType == ConstraintType.ExternalUniqueness && (reportContent & VerbalizationReportContent.ExternalUniquenessConstraints) != 0)
				{
					addConstraint = true;
				}
				else if (factConstraint.Constraint.ConstraintType == ConstraintType.Frequency && (reportContent & VerbalizationReportContent.FrequencyConstraints) != 0)
				{
					addConstraint = true;
				}
				else if (factConstraint.Constraint.ConstraintType == ConstraintType.InternalUniqueness && (reportContent & VerbalizationReportContent.InternalUniquenessConstraints) != 0)
				{
					addConstraint = true;
				}
				else if (factConstraint.Constraint.ConstraintType == ConstraintType.Ring && (reportContent & VerbalizationReportContent.RingConstraints) != 0)
				{
					addConstraint = true;
				}
				else if (factConstraint.Constraint.ConstraintType == ConstraintType.SimpleMandatory && (reportContent & VerbalizationReportContent.SimpleMandatoryConstraints) != 0)
				{
					addConstraint = true;
				}
				else if (factConstraint.Constraint.ConstraintType == ConstraintType.Subset && (reportContent & VerbalizationReportContent.SubsetConstraints) != 0)
				{
					addConstraint = true;
				}

				if (addConstraint && !constraintList[factConstraint.Constraint.ConstraintType].Contains(factConstraint.Constraint))
				{
					constraintList[factConstraint.Constraint.ConstraintType].Add(factConstraint.Constraint);
				}
			}
		}
		/// <summary>
		/// Retrieves a dictionary of all Constraints for the given Fact Type list, filtered by the specified VerbalizationReportContent, indexed by ConstraintType
		/// </summary>
		/// <param name="factTypeList">The Fact Types to retrieve constraints for</param>
		/// <param name="reportContent">The VerbalizationReportContent to filter constraints</param>
		/// <returns>dictionary of all Constraints for the given Fact Type, filtered by the specified VerbalizationReportContent, indexed by ConstraintType</returns>
		public static IDictionary<ConstraintType, IList<IConstraint>> GetConstraintsFromFactType(IList<FactType> factTypeList, VerbalizationReportContent reportContent)
		{
			IDictionary<ConstraintType, IList<IConstraint>> constraintList = GetConstraintDictionary(reportContent);

			int factCount = factTypeList.Count;
			for (int i = 0; i < factCount; ++i)
			{
				ProcessFactTypeConstraints(factTypeList[i], reportContent, constraintList);
			}

			return constraintList;
		}
		/// <summary>
		/// Retrieves a dictionary of constraint lists based on the given filter
		/// </summary>
		private static IDictionary<ConstraintType, IList<IConstraint>> GetConstraintDictionary(VerbalizationReportContent reportContent)
		{
			IDictionary<ConstraintType, IList<IConstraint>> constraintList = new Dictionary<ConstraintType, IList<IConstraint>>();

			if ((reportContent & VerbalizationReportContent.DisjunctiveMandatoryConstraints) != 0)
			{
				constraintList.Add(ConstraintType.DisjunctiveMandatory, new List<IConstraint>());
			}
			if ((reportContent & VerbalizationReportContent.EqualityConstraints) != 0)
			{
				constraintList.Add(ConstraintType.Equality, new List<IConstraint>());
			}
			if ((reportContent & VerbalizationReportContent.ExclusionConstraints) != 0)
			{
				constraintList.Add(ConstraintType.Exclusion, new List<IConstraint>());
			}
			if ((reportContent & VerbalizationReportContent.ExternalUniquenessConstraints) != 0)
			{
				constraintList.Add(ConstraintType.ExternalUniqueness, new List<IConstraint>());
			}
			if ((reportContent & VerbalizationReportContent.FrequencyConstraints) != 0)
			{
				constraintList.Add(ConstraintType.Frequency, new List<IConstraint>());
			}
			if ((reportContent & VerbalizationReportContent.InternalUniquenessConstraints) != 0)
			{
				constraintList.Add(ConstraintType.InternalUniqueness, new List<IConstraint>());
			}
			if ((reportContent & VerbalizationReportContent.RingConstraints) != 0)
			{
				constraintList.Add(ConstraintType.Ring, new List<IConstraint>());
			}
			if ((reportContent & VerbalizationReportContent.SimpleMandatoryConstraints) != 0)
			{
				constraintList.Add(ConstraintType.SimpleMandatory, new List<IConstraint>());
			}
			if ((reportContent & VerbalizationReportContent.SubsetConstraints) != 0)
			{
				constraintList.Add(ConstraintType.Subset, new List<IConstraint>());
			}

			return constraintList;
		}
	}
	#endregion // VerbalizationHelper class

	#region Object Type Reports
	/// <summary>
	/// Represents the Object Type List
	/// </summary>
	public class ObjectTypeListReport : IVerbalizeCustomChildren
	{
		private VerbalizationReportContent myReportContent;
		private IList<ObjectType> myObjectTypeList;
		private IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets;
		private ReportVerbalizationSnippetType myHeaderSnippet;
		private ReportVerbalizationSnippetType myFooterSnippet;

		/// <summary>
		/// Initializes a new instance of ObjectTypeListReport
		/// </summary>
		public ObjectTypeListReport(IList<ObjectType> objectTypeList, IVerbalizationSets<ReportVerbalizationSnippetType> snippets,
			ReportVerbalizationSnippetType headerSnippet, ReportVerbalizationSnippetType footerSnippet, VerbalizationReportContent reportContent)
		{
			myObjectTypeList = objectTypeList;
			mySnippets = snippets;
			myHeaderSnippet = headerSnippet;
			myFooterSnippet = footerSnippet;
			myReportContent = reportContent;
		}

		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(bool isNegative)
		{
			return GetCustomChildVerbalizations(isNegative);
		}
		/// <summary>
		/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(bool isNegative)
		{
			yield return new CustomChildVerbalizer(new VerbalizationReportTableOfContentsWrapper(myReportContent, mySnippets));
			yield return new CustomChildVerbalizer(new VerbalizationReportWrapper.ObjectTypeListWrapper(myObjectTypeList, mySnippets, myHeaderSnippet, myFooterSnippet) as IVerbalize);
		}
	}
	/// <summary>
	/// Represents an individual Object Type report
	/// </summary>
	public class ObjectTypePageReport : IVerbalizeCustomChildren
	{
		private ObjectType myObjectType;
		private IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets;
		private VerbalizationReportContent myReportContent;

		/// <summary>
		/// Initializes a new instance of ObjectTypePageReport
		/// </summary>
		public ObjectTypePageReport(ObjectType objectType, VerbalizationReportContent reportContent, IVerbalizationSets<ReportVerbalizationSnippetType> snippets)
		{
			myObjectType = objectType;
			myReportContent = reportContent;
			mySnippets = snippets;
		}
		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(bool isNegative)
		{
			return GetCustomChildVerbalizations(isNegative);
		}
		/// <summary>
		/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(bool isNegative)
		{
			yield return new CustomChildVerbalizer(new ObjectTypePageHeaderSummary(mySnippets, myObjectType));
			yield return new CustomChildVerbalizer(new ObjectTypePageFactTypeSection(myObjectType));
			yield return new CustomChildVerbalizer(new ObjectTypePageRelationshipSection(myObjectType));
			yield return new CustomChildVerbalizer(new GenericSuperTypeSection(myObjectType));
			yield return new CustomChildVerbalizer(new GenericSubTypeSection(myObjectType));
		}
		/// <summary>
		/// Represents the Summary section of the ObjectType page Verbalization Report
		/// </summary>
		private class ObjectTypePageHeaderSummary : VerbalizationReportWrapper
		{
			/// <summary>
			/// Initializes a new instance of FactTypeVerbalizationWrapper
			/// </summary>
			public ObjectTypePageHeaderSummary(IVerbalizationSets<ReportVerbalizationSnippetType> snippets, IVerbalize verbalizationObject)
				: base(snippets, verbalizationObject) { }
			/// <summary>
			/// Implements IVerbalize.GetVerbalization
			/// </summary>
			public override bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
			{
				beginVerbalization(VerbalizationContent.Normal);
				mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				bool retVal = false;
				writer.Write(string.Format(writer.FormatProvider, mySnippets.GetSnippet(ReportVerbalizationSnippetType.ObjectTypePageHeader),
					(myVerbalizationObject as ObjectType).Name));
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSummaryOpen));
				retVal = myVerbalizationObject.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSummaryClose));
				return retVal;
			}
		}
	}
	/// <summary>
	/// Represents the FactType section of the ObjectType page Verbalization Report
	/// </summary>
	public class ObjectTypePageFactTypeSection : IVerbalize
	{
		private ObjectType myObjectType;
		private IList<FactType> uniqueFactList;

		/// <summary>
		/// Initializes a new instance of ObjectTypePageFactTypeSection
		/// </summary>
		/// <param name="objectType"></param>
		public ObjectTypePageFactTypeSection(ObjectType objectType)
		{
			myObjectType = objectType;
		}

		private IList<FactType> UniqueFactList
		{
			get
			{
				if (uniqueFactList == null) uniqueFactList = VerbalizationHelper.GetFactTypesFromObjectType(myObjectType);
				return uniqueFactList;
			}
		}

		#region IVerbalize Members
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			int factTypeCount = UniqueFactList.Count;

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.ObjectTypePageFactTypeListOpen));
			for (int i = 0; i < factTypeCount; ++i)
			{
				IVerbalize instance = new VerbalizationReportWrapper.FactTypeVerbalizationWrapper(mySnippets, UniqueFactList[i]) as IVerbalize;
				if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
			}
			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.ObjectTypePageFactTypeListClose));
			return retVal;
		}
		#endregion
	}
	/// <summary>
	/// Represents the Relationship section of the ObjectType page Verbalization Report
	/// </summary>
	public class ObjectTypePageRelationshipSection : IVerbalize
	{
		private ObjectType myObjectType;
		private IList<ObjectType> relatedObjectList;

		/// <summary>
		/// Initializes a new instance of ObjectTypePageConstraintSection
		/// </summary>
		public ObjectTypePageRelationshipSection(ObjectType objectType)
		{
			myObjectType = objectType;
		}
		/// <summary>
		/// Gets a list of related object types
		/// </summary>
		private IList<ObjectType> RelatedObjectList
		{
			get
			{
				if (relatedObjectList == null)
				{
					relatedObjectList = new List<ObjectType>();
					IList<FactType> uniqueFactList = VerbalizationHelper.GetFactTypesFromObjectType(myObjectType);
					int factCount = uniqueFactList.Count;
					for (int i = 0; i < factCount; i++)
					{
						LinkedElementCollection<RoleBase> currentFactRoles = uniqueFactList[i].RoleCollection;
						int currentFactRoleCount = currentFactRoles.Count;
						for (int j = 0; j < currentFactRoleCount; ++j)
						{
							if (currentFactRoles[j].Role.RolePlayer != null && currentFactRoles[j].Role.RolePlayer != myObjectType && !relatedObjectList.Contains(currentFactRoles[j].Role.RolePlayer))
							{
								relatedObjectList.Add(currentFactRoles[j].Role.RolePlayer);
							}
						}
					}
					(relatedObjectList as List<ObjectType>).Sort(NamedElementComparer<ObjectType>.CurrentCulture);
				}
				return relatedObjectList;
			}
		}

		#region IVerbalize Members
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericRelationshipsListOpen));

			int relatedObjectCount = RelatedObjectList.Count;
			for (int i = 0; i < relatedObjectCount; ++i)
			{
				IVerbalize instance = new VerbalizationReportWrapper.ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeValueLink, mySnippets, RelatedObjectList[i] as IVerbalize) as IVerbalize;
				if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
			}

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericRelationshipsListClose));
			return retVal;
		}
		#endregion
	}
	#endregion

	#region Fact Type Reports
	/// <summary>
	/// Represents an individual Fact Type report
	/// </summary>
	public class FactTypePageReport : IVerbalizeCustomChildren
	{
		private FactType myFactType;
		private IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets;
		private VerbalizationReportContent myReportContent;

		/// <summary>
		/// Initializes a new instance of FactTypePageReport
		/// </summary>
		public FactTypePageReport(FactType factType, VerbalizationReportContent reportContent, IVerbalizationSets<ReportVerbalizationSnippetType> snippets)
		{
			myFactType = factType;
			myReportContent = reportContent;
			mySnippets = snippets;
		}
		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(bool isNegative)
		{
			return GetCustomChildVerbalizations(isNegative);
		}
		/// <summary>
		/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(bool isNegative)
		{
			yield return new CustomChildVerbalizer(new VerbalizationReportWrapper.FactTypePageHeaderSummary(mySnippets, myFactType));
			yield return new CustomChildVerbalizer(new FactTypePageObjectTypeSection(ReportVerbalizationSnippetType.ObjectTypeRelationshipValueLink, myFactType));
			yield return new CustomChildVerbalizer(new GenericConstraintSection(myReportContent,
				VerbalizationHelper.GetConstraintsFromFactType(myFactType, myReportContent)));
			if (myFactType.Objectification != null)
			{
				yield return new CustomChildVerbalizer(new GenericSuperTypeSection(myFactType.Objectification.NestingType));
				yield return new CustomChildVerbalizer(new GenericSubTypeSection(myFactType.Objectification.NestingType));
			}
		}
	}
	/// <summary>
	/// Represents the Fact Type List
	/// </summary>
	public class FactTypePageObjectTypeSection : IVerbalize
	{
		private FactType myFactType;
		private IList<ObjectType> objectTypeList;
		private ReportVerbalizationSnippetType myObjectTypeSnippet;

		/// <summary>
		/// Initializes a new instance of ObjectTypePageFactTypeSection
		/// </summary>
		/// <param name="objectTypeSnippet"></param>
		/// <param name="factType"></param>
		public FactTypePageObjectTypeSection(ReportVerbalizationSnippetType objectTypeSnippet, FactType factType)
		{
			myObjectTypeSnippet = objectTypeSnippet;
			myFactType = factType;
		}

		private IList<ObjectType> ObjectTypeList
		{
			get
			{
				if (objectTypeList == null)
				{
					objectTypeList = new List<ObjectType>();
					LinkedElementCollection<RoleBase> roleBaseCollection = myFactType.RoleCollection;
					int roleBaseCount = roleBaseCollection.Count;
					for (int i = 0; i < roleBaseCount; ++i)
					{
						ObjectType rolePlayer = roleBaseCollection[i].Role.RolePlayer;
						if (rolePlayer != null && !objectTypeList.Contains(rolePlayer)) objectTypeList.Add(rolePlayer);
					}
				}
				return objectTypeList;
			}
		}

		#region IVerbalize Members
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			int objectTypeCount = ObjectTypeList.Count;

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.FactTypePageObjectTypeListOpen));
			for (int i = 0; i < objectTypeCount; ++i)
			{
				IVerbalize instance = new VerbalizationReportWrapper.ObjectTypeVerbalizationWrapper(myObjectTypeSnippet, mySnippets, ObjectTypeList[i] as IVerbalize) as IVerbalize;
				if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
			}
			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.FactTypePageObjectTypeListClose));
			return retVal;
		}
		#endregion
	}
	#endregion

	#region Shared Reports
	/// <summary>
	/// Provides Verbalization of the Table of Contents for the Verbalization Reports
	/// </summary>
	public class VerbalizationReportTableOfContentsWrapper : IVerbalize
	{
		private VerbalizationReportContent myReportContent;
		private IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets;
		/// <summary>
		/// Initializes a new instance of VerbalizationReportTableOfContentsWrapper
		/// </summary>
		public VerbalizationReportTableOfContentsWrapper(VerbalizationReportContent reportContent, IVerbalizationSets<ReportVerbalizationSnippetType> snippets)
		{
			myReportContent = reportContent;
			mySnippets = snippets;
		}

		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			ReportVerbalizationSnippetType snippetFormat = 0;
			if (0 != (myReportContent & VerbalizationReportContent.ObjectTypes) &&
				0 != (myReportContent & VerbalizationReportContent.ValidationReport))
			{
				snippetFormat = ReportVerbalizationSnippetType.ReportDocumentContentsReplacementAll;
			}
			else if (0 == (myReportContent & VerbalizationReportContent.ObjectTypes) &&
				 0 != (myReportContent & VerbalizationReportContent.ValidationReport))
			{
				snippetFormat = ReportVerbalizationSnippetType.ReportDocumentContentsReplacementValidation;
			}
			else if (0 != (myReportContent & VerbalizationReportContent.ObjectTypes) &&
				0 == (myReportContent & VerbalizationReportContent.ValidationReport))
			{
				snippetFormat = ReportVerbalizationSnippetType.ReportDocumentContentsReplacementObject;
			}

			beginVerbalization(VerbalizationContent.Normal);
			writer.Write(string.Format(mySnippets.GetSnippet(ReportVerbalizationSnippetType.ReportDocumentContents),
				mySnippets.GetSnippet(snippetFormat)));
			return true;
		}
	}
	/// <summary>
	/// Represents the Constraint section of the Verbalization Report Pages
	/// </summary>
	public class GenericConstraintSection : IVerbalize
	{
		private VerbalizationReportContent myReportContent;
		private IDictionary<ConstraintType, IList<IConstraint>> myConstraintList;

		/// <summary>
		/// Initializes a new instance of ObjectTypePageConstraintSection
		/// </summary>
		public GenericConstraintSection(VerbalizationReportContent reportContent, IDictionary<ConstraintType, IList<IConstraint>> constraintList)
		{
			myReportContent = reportContent;
			myConstraintList = constraintList;
		}

		#region IVerbalize Members
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericConstraintListOpen));

			// Internal Uniqueness Constraints
			if ((myReportContent & VerbalizationReportContent.InternalUniquenessConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.InternalUniqueness];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
				}
			}
			// External Uniqueness Constraints
			if ((myReportContent & VerbalizationReportContent.ExternalUniquenessConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.ExternalUniqueness];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
				}
			}
			// Disjunctive Mandatory Constraints
			if ((myReportContent & VerbalizationReportContent.DisjunctiveMandatoryConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.DisjunctiveMandatory];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
				}
			}
			// Simple Mandatory Constraints
			if ((myReportContent & VerbalizationReportContent.SimpleMandatoryConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.SimpleMandatory];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
				}
			}
			// Ring Constraints
			if ((myReportContent & VerbalizationReportContent.RingConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.Ring];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
				}
			}
			// Frequency Constraints
			if ((myReportContent & VerbalizationReportContent.FrequencyConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.Frequency];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize constraint = constraintList[i] as IVerbalize;
					if (constraint != null)
					{
						IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
						if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
					}
				}
			}
			// Equality Constraints
			if ((myReportContent & VerbalizationReportContent.EqualityConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.Equality];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
				}
			}
			// Exclusion Constraints
			if ((myReportContent & VerbalizationReportContent.ExclusionConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.Exclusion];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
				}
			}
			// Subset Constraints
			if ((myReportContent & VerbalizationReportContent.SubsetConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.Subset];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
				}
			}

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericConstraintListClose));
			return retVal;
		}
		#endregion
	}
	/// <summary>
	/// Wraps any given Snippet for verbalization
	/// </summary>
	public class GenericSnippetVerbalizer : IVerbalize
	{
		private ReportVerbalizationSnippetType mySnippet;

		/// <summary>
		/// Initializes a new instance of GenericSnippetVerbalizer
		/// </summary>
		public GenericSnippetVerbalizer(ReportVerbalizationSnippetType snippet)
		{
			mySnippet = snippet;
		}

		#region IVerbalize Members
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			writer.Write(snippets.GetSnippet(mySnippet));
			return retVal;
		}
		#endregion
	}
	/// <summary>
	/// Represents the Super Type section of a Verbalization Report Page
	/// </summary>
	public class GenericSuperTypeSection : IVerbalize
	{
		private ObjectType myObjectType;

		/// <summary>
		/// Initializes a new instance of GenericSuperTypeSection
		/// </summary>
		public GenericSuperTypeSection(ObjectType objectType)
		{
			myObjectType = objectType;
		}
		#region IVerbalize Members
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSuperTypeListOpen));

			bool hasContent = false;
			foreach (ObjectType objectType in myObjectType.SupertypeCollection)
			{
				IVerbalize instance = new VerbalizationReportWrapper.ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeRelationshipValueLink, mySnippets, objectType as IVerbalize) as IVerbalize;
				if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
				hasContent = true;
			}
			if (!hasContent)
			{
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.EmptyContentListItemSnippet));
			}

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSuperTypeListClose));
			return retVal;
		}
		#endregion
	}
	/// <summary>
	/// Represents the Sub Type section of a Verbalization Report Page
	/// </summary>
	public class GenericSubTypeSection : IVerbalize
	{
		private ObjectType myObjectType;

		/// <summary>
		/// Initializes a new instance of GenericSuperTypeSection
		/// </summary>
		public GenericSubTypeSection(ObjectType objectType)
		{
			myObjectType = objectType;
		}
		#region IVerbalize Members
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSubTypeListOpen));

			bool hasContent = false;
			foreach (ObjectType objectType in myObjectType.SubtypeCollection)
			{
				IVerbalize instance = new VerbalizationReportWrapper.ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeRelationshipValueLink, mySnippets, objectType as IVerbalize) as IVerbalize;
				if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
				hasContent = true;
			}
			if (!hasContent)
			{
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.EmptyContentListItemSnippet));
			}

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSubTypeListClose));
			return retVal;
		}
		#endregion
	}
	/// <summary>
	/// Represents the FactType/Constraint Validation List
	/// </summary>
	public class FactTypeConstraintValidationListReport : IVerbalizeCustomChildren
	{
		private IList<FactType> myFactTypeList;
		private IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets;
		private VerbalizationReportContent myReportContent;

		/// <summary>
		/// Initializes a new instance of FactTypeConstraintValidationListReport
		/// </summary>
		public FactTypeConstraintValidationListReport(IList<FactType> factTypeList, VerbalizationReportContent reportContent, IVerbalizationSets<ReportVerbalizationSnippetType> snippets)
		{
			myFactTypeList = factTypeList;
			mySnippets = snippets;
			myReportContent = reportContent;
		}

		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(bool isNegative)
		{
			return GetCustomChildVerbalizations(isNegative);
		}
		/// <summary>
		/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(bool isNegative)
		{
			yield return new CustomChildVerbalizer(new VerbalizationReportTableOfContentsWrapper(myReportContent, mySnippets));
			yield return new CustomChildVerbalizer(new GenericSnippetVerbalizer(ReportVerbalizationSnippetType.FactTypeConstraintValidationHeader));
			int factTypeCount = myFactTypeList.Count;
			for (int i = 0; i < factTypeCount; ++i)
			{
				yield return new CustomChildVerbalizer(new VerbalizationReportWrapper.FactTypePageHeaderSummary(mySnippets, myFactTypeList[i]));
				yield return new CustomChildVerbalizer(new FactTypePageObjectTypeSection(ReportVerbalizationSnippetType.ObjectTypeListObjectTypeValueLink, myFactTypeList[i]));
				yield return new CustomChildVerbalizer(new ConstraintValidationSection(myReportContent,
					VerbalizationHelper.GetConstraintsFromFactType(myFactTypeList[i], myReportContent)));
			}
			yield return new CustomChildVerbalizer(new GenericSnippetVerbalizer(ReportVerbalizationSnippetType.FactTypeConstraintValidationSignature));
		}
		/// <summary>
		/// Represents the Validation Constraint section of the Verbalization Report
		/// </summary>
		private class ConstraintValidationSection : IVerbalize
		{
			private VerbalizationReportContent myReportContent;
			private IDictionary<ConstraintType, IList<IConstraint>> myConstraintList;

			/// <summary>
			/// Initializes a new instance of ConstraintValidationSection
			/// </summary>
			public ConstraintValidationSection(VerbalizationReportContent reportContent, IDictionary<ConstraintType, IList<IConstraint>> constraintList)
			{
				myReportContent = reportContent;
				myConstraintList = constraintList;
			}

			#region IVerbalize Members
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
			{
				return GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
			}
			/// <summary>
			/// Implements IVerbalize.GetVerbalization
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
			{
				bool retVal = true;
				IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];

				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericConstraintListOpen));

				// Internal Uniqueness Constraints
				if ((myReportContent & VerbalizationReportContent.InternalUniquenessConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.InternalUniqueness];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
						if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
					}
				}
				// External Uniqueness Constraints
				if ((myReportContent & VerbalizationReportContent.ExternalUniquenessConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.ExternalUniqueness];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
						if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
					}
				}
				// Disjunctive Mandatory Constraints
				if ((myReportContent & VerbalizationReportContent.DisjunctiveMandatoryConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.DisjunctiveMandatory];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
						if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
					}
				}
				// Simple Mandatory Constraints
				if ((myReportContent & VerbalizationReportContent.SimpleMandatoryConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.SimpleMandatory];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
						if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
					}
				}
				// Ring Constraints
				if ((myReportContent & VerbalizationReportContent.RingConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Ring];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
						if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
					}
				}
				// Frequency Constraints
				if ((myReportContent & VerbalizationReportContent.FrequencyConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Frequency];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						IVerbalize constraint = constraintList[i] as IVerbalize;
						if (constraint != null)
						{
							IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
							if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
						}
					}
				}
				// Equality Constraints
				if ((myReportContent & VerbalizationReportContent.EqualityConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Equality];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
						if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
					}
				}
				// Exclusion Constraints
				if ((myReportContent & VerbalizationReportContent.ExclusionConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Exclusion];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
						if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
					}
				}
				// Subset Constraints
				if ((myReportContent & VerbalizationReportContent.SubsetConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Subset];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						IVerbalize instance = new VerbalizationReportWrapper.ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, mySnippets, constraintList[i] as IVerbalize) as IVerbalize;
						if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative)) retVal = false;
					}
				}

				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericConstraintListClose));
				return retVal;
			}
			#endregion
		}
	}
	#endregion
}
