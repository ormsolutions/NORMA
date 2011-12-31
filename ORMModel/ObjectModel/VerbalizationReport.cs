#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using System.ComponentModel;
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
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Verbalization
{
	#region HtmlReport class
	/// <summary>
	/// Provided as a service domain model (no elements or relationships) for the HtmlReport
	/// </summary>
	[VerbalizationTargetProvider("VerbalizationTargets")]
	[DomainObjectId("8532FB7F-8F25-4E0E-A6B5-370EBE89E455")]
	public class HtmlReport : DomainModel
	{
		#region Public constants
		/// <summary>
		/// HtmlReport domain model Id.
		/// </summary>
		public static readonly global::System.Guid DomainModelId = new global::System.Guid(0x8532fb7f, 0x8f25, 0x4e0e, 0xa6, 0xb5, 0x37, 0x0e, 0xbe, 0x89, 0xe4, 0x55);

		/// <summary>
		/// The unique name of the HtmlReport verbalization target. Used in the Xml files and in code to identify this target provider.
		/// </summary>
		public const string HtmlReportTargetName = "HtmlReport";
		#endregion // Public constants
		#region Constructor
		/// <summary>
		/// Required constructor for a domain model
		/// </summary>
		/// <param name="store">The <see cref="Store"/> being populated</param>
		public HtmlReport(Store store)
			: base(store, new Guid("8532FB7F-8F25-4E0E-A6B5-370EBE89E455"))
		{
		}
		#endregion // Constructor
		#region VerbalizationTargets class
		private sealed class VerbalizationTargets : IVerbalizationTargetProvider
		{
			#region IVerbalizationTargetProvider implementation
			VerbalizationTargetData[] IVerbalizationTargetProvider.ProvideVerbalizationTargets()
			{
				return new VerbalizationTargetData[] {new VerbalizationTargetData(
				HtmlReportTargetName,
				ResourceStrings.VerbalizationTargetHtmlReportDisplayName,
				ResourceStrings.VerbalizationTargetHtmlReportCommandName,
				#region Report generation
				delegate(Store store)
				{
					foreach (ORMModel model in store.ElementDirectory.FindElements<ORMModel>())
					{
						IServiceProvider provider;
						System.Windows.Forms.Design.IUIService uiService;
						if (null != (provider = (model.Store as IORMToolServices).ServiceProvider) &&
								null != (uiService = (System.Windows.Forms.Design.IUIService)provider.GetService(typeof(System.Windows.Forms.Design.IUIService))))
						{
							uiService.ShowDialog(new GenerateReportDialog(model));
						}
					}
				})};
				#endregion // Report generation
			}
			#endregion // IVerbalizationTargetProvider implementation
		}
		#endregion // VerbalizationTargets class
	}
	#endregion // HtmlReport class
	#region VerbalizationReportContent enum
	/// <summary>
	/// Determines the elements of the Model to report
	/// </summary>
	[global::System.ComponentModel.TypeConverter(typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter<VerbalizationReportContent, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel>))]
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
	#endregion // VerbalizationReportContent enum
	#region VerbalizationReportGenerator class
	/// <summary>
	/// Provides methods for generating Verbalization Reports
	/// </summary>
	public class VerbalizationReportGenerator
	{
		#region Public Methods
		/// <summary>
		/// Generates a report of the Verbalizations
		/// </summary>
		/// <param name="model">The model to report on</param>
		/// <param name="reportContent">The filter to apply to the report</param>
		/// <param name="baseDir">The base directory to output the report</param>
		public static void GenerateReport(ORMModel model, VerbalizationReportContent reportContent, string baseDir)
		{
			#region Member Variable
			const VerbalizationSign sign = VerbalizationSign.Positive | VerbalizationSign.AttemptOppositeSign;
			IORMToolServices toolServices = (IORMToolServices)model.Store;
			IDictionary<Type, IVerbalizationSets> snippetsDictionary = toolServices.GetVerbalizationSnippetsDictionary(HtmlReport.HtmlReportTargetName);
			IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			IExtensionVerbalizerService extensionVerbalizer = toolServices.ExtensionVerbalizerService;
			IDictionary<string, object> verbalizationOptions = toolServices.VerbalizationOptions;
			Dictionary<IVerbalize, IVerbalize> alreadyVerbalized;
			Dictionary<object, object> locallyVerbalized = new Dictionary<object, object>();
			Stream fileStream;
			TextWriter textWriter;
			VerbalizationCallbackWriter writer;
			#endregion // Member Variable
			#region ObjectType Reports
			ObjectType[] objectTypeList = null;
			if (0 != (reportContent & VerbalizationReportContent.ObjectTypes))
			{
				alreadyVerbalized = new Dictionary<IVerbalize, IVerbalize>();
				objectTypeList = model.ObjectTypeCollection.ToArray();

				int objectTypeCount = objectTypeList.Length;
				if (objectTypeCount != 0)
				{
					Array.Sort<ObjectType>(objectTypeList, NamedElementComparer<ObjectType>.CurrentCulture);
					#region Object Type List Report
					fileStream = new FileStream(Path.Combine(baseDir, "ObjectTypeList.html"), FileMode.Create, FileAccess.ReadWrite);
					textWriter = new StreamWriter(fileStream);
					writer = new VerbalizationReportCallbackWriter(snippetsDictionary, textWriter);
					bool firstCall = true;

					IVerbalizeCustomChildren objectTypeListReport = new ObjectTypeListReport(model, objectTypeList, snippets, ReportVerbalizationSnippetType.ObjectTypeListHeader, ReportVerbalizationSnippetType.ObjectTypeListFooter, reportContent);
					VerbalizationHelper.VerbalizeChildren(
						objectTypeListReport.GetCustomChildVerbalizations(null, verbalizationOptions, sign),
						null,
						snippetsDictionary,
						extensionVerbalizer,
						verbalizationOptions,
						HtmlReport.HtmlReportTargetName,
						alreadyVerbalized,
						locallyVerbalized,
						sign,
						writer,
						false,
						ref firstCall);

					if (!firstCall)
					{
						writer.WriteDocumentFooter();
					}

					textWriter.Flush();
					textWriter.Close();
					#endregion // Object Type List Report
					string objectTypeDir = Path.Combine(baseDir, "ObjectTypes");
					if (!Directory.Exists(objectTypeDir))
					{
						Directory.CreateDirectory(objectTypeDir);
					}
					#region Individual ObjectType Pages
					for (int i = 0; i < objectTypeCount; ++i)
					{
						bool firstCallPending = true;
						locallyVerbalized.Clear();
						ObjectType objectType = objectTypeList[i];

						fileStream = new FileStream(Path.Combine(objectTypeDir, AsFileName(objectType.Name) + ".html"), FileMode.Create, FileAccess.ReadWrite);
						textWriter = new StreamWriter(fileStream);
						writer = new VerbalizationReportCallbackWriter(snippetsDictionary, textWriter);

						ObjectTypePageReport objectTypePageReport = new ObjectTypePageReport(objectType, reportContent, snippets);
						VerbalizationHelper.VerbalizeChildren(
							((IVerbalizeCustomChildren)objectTypePageReport).GetCustomChildVerbalizations(objectTypePageReport, verbalizationOptions, sign),
							null,
							snippetsDictionary,
							extensionVerbalizer,
							verbalizationOptions,
							HtmlReport.HtmlReportTargetName,
							alreadyVerbalized,
							locallyVerbalized,
							sign,
							writer,
							false,
							ref firstCallPending);

						if (!firstCallPending)
						{
							writer.WriteDocumentFooter();
						}
						textWriter.Flush();
						textWriter.Close();
					}
					#endregion // Individual ObjectType Pages
				}
			}
			#endregion // ObjectType Reports
			#region Individual FactType Page Reports
			FactType[] factTypeList = null;
			if (0 != (VerbalizationReportContent.FactTypes & reportContent))
			{
				factTypeList = model.FactTypeCollection.ToArray();
				alreadyVerbalized = new Dictionary<IVerbalize, IVerbalize>();

				int factCount = factTypeList.Length;
				if (factCount != 0)
				{
					string factTypeDir = Path.Combine(baseDir, "FactTypes");
					if (!Directory.Exists(factTypeDir))
					{
						Directory.CreateDirectory(factTypeDir);
					}

					Array.Sort<FactType>(factTypeList, NamedElementComparer<FactType>.CurrentCulture);
					for (int i = 0; i < factCount; ++i)
					{
						bool firstCallPending = true;
						alreadyVerbalized.Clear();
						locallyVerbalized.Clear();
						fileStream = new FileStream(Path.Combine(factTypeDir, AsFileName(factTypeList[i].Name) + ".html"), FileMode.Create, FileAccess.ReadWrite);
						textWriter = new StreamWriter(fileStream);
						writer = new VerbalizationReportCallbackWriter(snippetsDictionary, textWriter);

						FactTypePageReport factTypePageReport = new FactTypePageReport(factTypeList[i], reportContent, snippets);
						VerbalizationHelper.VerbalizeChildren(
							((IVerbalizeCustomChildren)factTypePageReport).GetCustomChildVerbalizations(factTypePageReport, verbalizationOptions, sign),
							null,
							snippetsDictionary,
							extensionVerbalizer,
							verbalizationOptions,
							HtmlReport.HtmlReportTargetName,
							alreadyVerbalized,
							locallyVerbalized,
							sign,
							writer,
							false,
							ref firstCallPending);

						if (!firstCallPending)
						{
							writer.WriteDocumentFooter();
						}
						textWriter.Flush();
						textWriter.Close();
					}
				}
			}
			#endregion // Individual FactType Page Reports
			#region Constraint Validation Report
			if (0 != (reportContent & VerbalizationReportContent.ValidationReport))
			{
				alreadyVerbalized = new Dictionary<IVerbalize, IVerbalize>();
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

					FactTypeConstraintValidationListReport factTypeConstraintValidationReport = new FactTypeConstraintValidationListReport(model, factTypeList, reportContent, snippets);
					VerbalizationHelper.VerbalizeChildren(
						((IVerbalizeCustomChildren)factTypeConstraintValidationReport).GetCustomChildVerbalizations(factTypeConstraintValidationReport, verbalizationOptions, sign),
						null,
						snippetsDictionary,
						extensionVerbalizer,
						verbalizationOptions,
						HtmlReport.HtmlReportTargetName,
						alreadyVerbalized,
						null,
						sign,
						writer,
						false,
						ref firstCall);

					if (!firstCall)
					{
						writer.WriteDocumentFooter();
					}

					textWriter.Flush();
					textWriter.Close();
				}
			}
			#endregion // Constraint Validation Report
		}
		#endregion // Public Methods
		#region Helper methods
		private static readonly char[] InvalidFileChars = Path.GetInvalidFileNameChars();
		/// <summary>
		/// Normalize the name as a valid file name.
		/// </summary>
		/// <param name="name">Any string</param>
		/// <returns>The name with invalid file characters stripped out.</returns>
		/// <remarks>This is far from perfect (you can get an empty name) but
		/// is better than nothing.</remarks>
		protected static string AsFileName(string name)
		{
			string[] validNameParts = name.Split(InvalidFileChars, StringSplitOptions.RemoveEmptyEntries);
			if (validNameParts.Length > 1)
			{
				return string.Join(null, validNameParts);
			}
			return name;
		}
		/// <summary>
		/// Gets the Fact Types for which the specified Object Type plays a role
		/// </summary>
		protected static IList<FactType> GetFactTypesFromObjectType(ObjectType objectType)
		{
			List<FactType> factTypeList = new List<FactType>();
			LinkedElementCollection<Role> roleCollecton = objectType.PlayedRoleCollection;
			int roleCount = roleCollecton.Count;
			for (int i = 0; i < roleCount; ++i)
			{
				FactType factType = roleCollecton[i].FactType;
				if (!factTypeList.Contains(factType))
				{
					factTypeList.Add(factType);
				}
			}
			factTypeList.Sort(NamedElementComparer<FactType>.CurrentCulture);
			return (IList<FactType>)factTypeList;
		}
		/// <summary>
		/// Retrieves a dictionary of all Constraints for the given Fact Type, filtered by the specified VerbalizationReportContent, indexed by ConstraintType
		/// </summary>
		/// <param name="factType">The Fact Type to retrieve constraints for</param>
		/// <param name="reportContent">The VerbalizationReportContent to filter constraints</param>
		/// <returns>dictionary of all Constraints for the given Fact Type, filtered by the specified VerbalizationReportContent, indexed by ConstraintType</returns>
		protected static IDictionary<ConstraintType, IList<IConstraint>> GetConstraintsFromFactType(FactType factType, VerbalizationReportContent reportContent)
		{
			IDictionary<ConstraintType, IList<IConstraint>> constraintList = GetConstraintDictionary(reportContent);
			ProcessFactTypeConstraints(factType, reportContent, constraintList);
			return constraintList;
		}
		/// <summary>
		/// Processes the specified FactType and adds the appropriate constraints based on the specified filter
		/// </summary>
		private static void ProcessFactTypeConstraints(FactType factType, VerbalizationReportContent reportContent, IDictionary<ConstraintType, IList<IConstraint>> typedConstraintLists)
		{
			foreach (IFactConstraint factConstraint in factType.FactConstraintCollection)
			{
				IConstraint constraint = factConstraint.Constraint;
				ConstraintType constraintType = constraint.ConstraintType;
				VerbalizationReportContent allowedContent;
				switch (constraintType)
				{
					case ConstraintType.DisjunctiveMandatory:
						allowedContent = VerbalizationReportContent.DisjunctiveMandatoryConstraints;
						break;
					case ConstraintType.Equality:
						allowedContent = VerbalizationReportContent.EqualityConstraints;
						break;
					case ConstraintType.Exclusion:
						allowedContent = VerbalizationReportContent.ExclusionConstraints;
						break;
					case ConstraintType.ExternalUniqueness:
						allowedContent = VerbalizationReportContent.ExternalUniquenessConstraints;
						break;
					case ConstraintType.Frequency:
						allowedContent = VerbalizationReportContent.FrequencyConstraints;
						break;
					case ConstraintType.InternalUniqueness:
						allowedContent = VerbalizationReportContent.InternalUniquenessConstraints;
						break;
					case ConstraintType.Ring:
						allowedContent = VerbalizationReportContent.RingConstraints;
						break;
					case ConstraintType.SimpleMandatory:
						allowedContent = VerbalizationReportContent.SimpleMandatoryConstraints;
						break;
					case ConstraintType.Subset:
						allowedContent = VerbalizationReportContent.SubsetConstraints;
						break;
					default:
						continue;
				}
				if (0 != (reportContent & allowedContent))
				{
					IList<IConstraint> typedList = typedConstraintLists[constraintType];
					if (!typedList.Contains(constraint))
					{
						typedList.Add(constraint);
					}
				}
			}
		}
		/// <summary>
		/// Retrieves a dictionary of all Constraints for the given Fact Type list, filtered by the specified VerbalizationReportContent, indexed by ConstraintType
		/// </summary>
		/// <param name="factTypeList">The Fact Types to retrieve constraints for</param>
		/// <param name="reportContent">The VerbalizationReportContent to filter constraints</param>
		/// <returns>dictionary of all Constraints for the given Fact Type, filtered by the specified VerbalizationReportContent, indexed by ConstraintType</returns>
		protected static IDictionary<ConstraintType, IList<IConstraint>> GetConstraintsFromFactType(IList<FactType> factTypeList, VerbalizationReportContent reportContent)
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
		#endregion // Helper methods
		#region VerbalizationReportCallbackWriter class
		/// <summary>
		/// Handles the writing of utility snippets for the Verbalization Report target
		/// </summary>
		protected class VerbalizationReportCallbackWriter : VerbalizationCallbackWriter
		{
			private IVerbalizationSets<ReportVerbalizationSnippetType> myReportSnippets;
			/// <summary>
			/// Initializes a new instance of ORMSolutions.ORMArchitect.Core.ObjectModel.VerbalizationReportCallbackWriter
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
				Writer.Write(myReportSnippets.GetSnippet(ReportVerbalizationSnippetType.ReportDocumentFooter));
			}

		}
		#endregion // VerbalizationReportCallbackWriter class
		#region Verbalization helper classes
		#region Object Type Reports
		#region ObjectTypePageReport class
		/// <summary>
		/// Represents an individual Object Type report
		/// </summary>
		protected class ObjectTypePageReport : IVerbalizeCustomChildren, IVerbalizeFilterChildren
		{
			#region Member Variables
			private ObjectType myObjectType;
			private IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets;
			private VerbalizationReportContent myReportContent;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of ObjectTypePageReport
			/// </summary>
			public ObjectTypePageReport(ObjectType objectType, VerbalizationReportContent reportContent, IVerbalizationSets<ReportVerbalizationSnippetType> snippets)
			{
				myObjectType = objectType;
				myReportContent = reportContent;
				mySnippets = snippets;
			}
			#endregion // Constructor
			#region IVerbalizeCustomChildren Implementation
			IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign)
			{
				return GetCustomChildVerbalizations(filter, verbalizationOptions, sign);
			}
			/// <summary>
			/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
			/// </summary>
			protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign)
			{
				yield return CustomChildVerbalizer.VerbalizeInstance(new ObjectTypePageHeaderSummary(myObjectType, filter));
				yield return CustomChildVerbalizer.VerbalizeInstance(new ObjectTypePageFactTypeSection(myObjectType));
				yield return CustomChildVerbalizer.VerbalizeInstance(new ObjectTypePageRelationshipSection(myObjectType));
				yield return CustomChildVerbalizer.VerbalizeInstance(new GenericSuperTypeSection(myObjectType));
				yield return CustomChildVerbalizer.VerbalizeInstance(new GenericSubTypeSection(myObjectType));
			}
			#endregion // IVerbalizeCustomChildren Implementation
			#region IVerbalizeFilterChildren Implementation
			CustomChildVerbalizer IVerbalizeFilterChildren.FilterChildVerbalizer(object child, VerbalizationSign sign)
			{
				return FilterChildVerbalizer(child, sign);
			}
			/// <summary>
			/// Provides an opportunity for a parent object to filter the
			/// verbalization of aggregated child verbalization implementations
			/// </summary>
			/// <param name="child">A direct or indirect child object.</param>
			/// <param name="sign">The preferred verbalization sign</param>
			/// <returns>
			/// Return the provided childVerbalizer to verbalize normally, null to block verbalization, or an
			/// alternate IVerbalize. The value is returned with a boolean option. The element will be disposed with
			/// this is true.
			/// </returns>
			protected CustomChildVerbalizer FilterChildVerbalizer(object child, VerbalizationSign sign)
			{
				return CustomChildVerbalizer.VerbalizeInstance(child as IVerbalize);
			}
			#endregion // IVerbalizeFilterChildren Implementation
		}
		#endregion // ObjectTypePageReport class
		#region ObjectTypeListReport class
		/// <summary>
		/// Represents the Object Type List
		/// </summary>
		protected class ObjectTypeListReport : IVerbalizeCustomChildren
		{
			#region Member Variables
			private ORMModel myModel;
			private VerbalizationReportContent myReportContent;
			private IList<ObjectType> myObjectTypeList;
			private IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets;
			private ReportVerbalizationSnippetType myHeaderSnippet;
			private ReportVerbalizationSnippetType myFooterSnippet;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of ObjectTypeListReport
			/// </summary>
			/// <param name="model">The model for the report</param>
			/// <param name="objectTypeList">The object type list.</param>
			/// <param name="snippets">The snippets.</param>
			/// <param name="headerSnippet">The header snippet.</param>
			/// <param name="footerSnippet">The footer snippet.</param>
			/// <param name="reportContent">Content of the report.</param>
			public ObjectTypeListReport(
				ORMModel model,
				IList<ObjectType> objectTypeList,
				IVerbalizationSets<ReportVerbalizationSnippetType> snippets,
				ReportVerbalizationSnippetType headerSnippet,
				ReportVerbalizationSnippetType footerSnippet,
				VerbalizationReportContent reportContent)
			{
				myModel = model;
				myObjectTypeList = objectTypeList;
				mySnippets = snippets;
				myHeaderSnippet = headerSnippet;
				myFooterSnippet = footerSnippet;
				myReportContent = reportContent;
			}
			#endregion // Constructor
			#region IVerbalizeCustomChildren Implementation
			IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign)
			{
				return GetCustomChildVerbalizations(filter, verbalizationOptions, sign);
			}
			/// <summary>
			/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
			/// </summary>
			/// <param name="filter">A <see cref="IVerbalizeFilterChildren"/> instance. Can be <see langword="null"/>.
			/// If the <see cref="IVerbalizeFilterChildren.FilterChildVerbalizer">FilterChildVerbalizer</see> method returns
			/// <see cref="CustomChildVerbalizer.Block"/> for any constituent components used to create a <see cref="CustomChildVerbalizer"/>,
			/// then that custom child should not be created</param>
			/// <param name="verbalizationOptions">Verbalization options</param>
			/// <param name="sign">The preferred verbalization sign</param>
			/// <returns>
			/// IEnumerable of CustomChildVerbalizer structures
			/// </returns>
			protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign)
			{
				yield return CustomChildVerbalizer.VerbalizeInstance(new VerbalizationReportTableOfContentsWrapper(myReportContent, mySnippets));
				yield return CustomChildVerbalizer.VerbalizeInstance(new ModelContextWrapper(myModel, ReportVerbalizationSnippetType.ContextModelDescriptionOpen, ReportVerbalizationSnippetType.ContextModelDescriptionClose));
				yield return CustomChildVerbalizer.VerbalizeInstance(new ObjectTypeListWrapper(myObjectTypeList, myHeaderSnippet, myFooterSnippet));
			}
			#endregion // IVerbalizeCustomChildren Implementation
		}
		#endregion // ObjectTypeListReport class
		#region ObjectTypePageHeaderSummary class
		/// <summary>
		/// Represents the Summary section of the ObjectType page Verbalization Report
		/// </summary>
		protected class ObjectTypePageHeaderSummary : IVerbalize
		{
			#region Member Variables
			private IVerbalize myVerbalizationObject;
			private IVerbalizeFilterChildren myFilterChildren;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of FactTypeVerbalizationWrapper
			/// </summary>
			/// <param name="verbalizationObject">The verbalization object.</param>
			/// <param name="filter">The filter.</param>
			public ObjectTypePageHeaderSummary(IVerbalize verbalizationObject, IVerbalizeFilterChildren filter)
			{
				myVerbalizationObject = verbalizationObject;
				myFilterChildren = filter;
			}
			#endregion // Constructor
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(ReportVerbalizationSnippetType.ObjectTypePageHeader), (myVerbalizationObject as ObjectType).Name));
				writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericSummaryOpen));
				verbalizationContext.DeferVerbalization(myVerbalizationObject, DeferVerbalizationOptions.AlwaysWriteLine, myFilterChildren);
				// This must be called before additional calls to the writer or an increased indentation will not decrease before
				// the close element is written.
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericSummaryClose));
				return true;
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // ObjectTypePageHeaderSummary class
		#region ObjectTypePageFactTypeSection class
		/// <summary>
		/// Represents the FactType section of the ObjectType page Verbalization Report
		/// </summary>
		protected class ObjectTypePageFactTypeSection : IVerbalize
		{
			#region Member Variables
			private ObjectType myObjectType;
			private IList<FactType> myUniqueFactTypeList;
			#endregion Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of ObjectTypePageFactTypeSection
			/// </summary>
			/// <param name="objectType">The ObjectType.</param>
			public ObjectTypePageFactTypeSection(ObjectType objectType)
			{
				myObjectType = objectType;
			}
			#endregion // Constructor
			#region Property
			/// <summary>
			/// Gets the unique fact list.
			/// </summary>
			/// <value>The unique fact list.</value>
			private IList<FactType> UniqueFactTypeList
			{
				get
				{
					IList<FactType> retVal = myUniqueFactTypeList;
					if (retVal == null)
					{
						myUniqueFactTypeList = retVal = GetFactTypesFromObjectType(myObjectType);
					}
					return retVal;
				}
			}
			#endregion // Property
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				IList<FactType> uniqueFactTypes = UniqueFactTypeList;
				int factTypeCount = uniqueFactTypes.Count;

				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.ObjectTypePageFactTypeListOpen));
				if (factTypeCount != 0)
				{
					FactTypeVerbalizationWrapper wrapper = new FactTypeVerbalizationWrapper(null);
					IVerbalize verbalize = wrapper;
					for (int i = 0; i < factTypeCount; ++i)
					{
						wrapper.VerbalizationObject = uniqueFactTypes[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.ObjectTypePageFactTypeListClose));
				return false; // No children to verbalize
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // ObjectTypePageFactTypeSection class
		#region ObjectTypePageRelationshipSection class
		/// <summary>
		/// Represents the Relationship section of the ObjectType page Verbalization Report
		/// </summary>
		protected class ObjectTypePageRelationshipSection : IVerbalize
		{
			#region Member Variables
			private ObjectType myObjectType;
			private IList<ObjectType> myRelatedObjectList;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of ObjectTypePageConstraintSection
			/// </summary>
			/// <param name="objectType">The ObjectType.</param>
			public ObjectTypePageRelationshipSection(ObjectType objectType)
			{
				myObjectType = objectType;
			}
			#endregion // Constructor
			#region Property
			/// <summary>
			/// Gets a list of related object types
			/// </summary>
			/// <value>The related object list.</value>
			private IList<ObjectType> RelatedObjectList
			{
				get
				{
					IList<ObjectType> retVal = myRelatedObjectList;
					if (retVal == null)
					{
						List<ObjectType> relatedObjectList = new List<ObjectType>();
						IList<FactType> uniqueFactList = GetFactTypesFromObjectType(myObjectType);
						int factCount = uniqueFactList.Count;
						for (int i = 0; i < factCount; i++)
						{
							FactType factType = uniqueFactList[i];
							if (!(factType is SubtypeFact)) // Role players are shown in subtype/supertype sections, not in relationships
							{
								LinkedElementCollection<RoleBase> currentFactRoles = factType.RoleCollection;
								int currentFactRoleCount = currentFactRoles.Count;
								for (int j = 0; j < currentFactRoleCount; ++j)
								{
									ObjectType rolePlayer = currentFactRoles[j].Role.RolePlayer;
									if (rolePlayer != null && rolePlayer != myObjectType && !relatedObjectList.Contains(rolePlayer))
									{
										relatedObjectList.Add(rolePlayer);
									}
								}
							}
						}
						relatedObjectList.Sort(NamedElementComparer<ObjectType>.CurrentCulture);
						myRelatedObjectList = retVal = relatedObjectList;
					}
					return retVal;
				}
			}
			#endregion // Property
			#region IVerbalize Members
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericRelationshipsListOpen));

				IList<ObjectType> relatedObjects = RelatedObjectList;
				int relatedObjectCount = relatedObjects.Count;
				if (relatedObjectCount != 0)
				{
					ObjectTypeVerbalizationWrapper wrapper = new ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeValueLink, null);
					IVerbalize verbalize = wrapper;
					for (int i = 0; i < relatedObjectCount; ++i)
					{
						wrapper.VerbalizationObject = relatedObjects[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}

				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericRelationshipsListClose));
				return false; // No children to verbalize
			}
			#endregion
		}
		#endregion // ObjectTypePageRelationshipSection class
		#region ObjectTypeVerbalizationWrapper class
		/// <summary>
		/// Provides a wrapper for the ObjectType name value
		/// </summary>
		protected class ObjectTypeVerbalizationWrapper : IVerbalize
		{
			#region Member Variables
			private ReportVerbalizationSnippetType myObjectTypeSnippet;
			private ObjectType myVerbalizationObject;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of the <see cref="ObjectTypeVerbalizationWrapper"/> class.
			/// </summary>
			/// <param name="objectTypeSnippet">The object type snippet.</param>
			/// <param name="verbalizationObject">The verbalization object.</param>
			public ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType objectTypeSnippet, ObjectType verbalizationObject)
			{
				myObjectTypeSnippet = objectTypeSnippet;
				myVerbalizationObject = verbalizationObject;
			}
			#endregion // Constructor
			#region Accessor properties
			/// <summary>
			/// Access the verbalization object
			/// </summary>
			public ObjectType VerbalizationObject
			{
				get
				{
					return myVerbalizationObject;
				}
				set
				{
					myVerbalizationObject = value;
				}
			}
			#endregion // Accessor properties
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemOpen));
				string objectTypeName = myVerbalizationObject.Name;
				writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(myObjectTypeSnippet), objectTypeName, AsFileName(objectTypeName)));
				writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemClose));
				return false; // No children to verbalize
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // ObjectTypeVerbalizationWrapper class
		#region ObjectTypeListWrapper class
		/// <summary>
		/// Represents a list of ObjectTypes for a Verbalization Report
		/// </summary>
		protected class ObjectTypeListWrapper : IVerbalize
		{
			#region Member Variables
			private IList<ObjectType> myObjectTypeList;
			private ReportVerbalizationSnippetType myHeaderSnippet;
			private ReportVerbalizationSnippetType myFooterSnippet;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of FactTypeVerbalizationWrapper
			/// </summary>
			/// <param name="objectTypeList">The object type list.</param>
			/// <param name="headerSnippet">The header snippet.</param>
			/// <param name="footerSnippet">The footer snippet.</param>
			public ObjectTypeListWrapper(IList<ObjectType> objectTypeList, ReportVerbalizationSnippetType headerSnippet, ReportVerbalizationSnippetType footerSnippet)
			{
				myObjectTypeList = objectTypeList;
				myHeaderSnippet = headerSnippet;
				myFooterSnippet = footerSnippet;
			}
			#endregion // Constructor
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				writer.Write(snippets.GetSnippet(myHeaderSnippet));
				IList<ObjectType> objectTypes = myObjectTypeList;
				int objectCount = objectTypes.Count;
				if (objectCount != 0)
				{
					ObjectTypeVerbalizationWrapper wrapper = new ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeListObjectTypeValueLink, null);
					IVerbalize verbalize = wrapper;
					for (int i = 0; i < objectCount; ++i)
					{
						wrapper.VerbalizationObject = objectTypes[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				writer.Write(snippets.GetSnippet(myFooterSnippet));
				return false; // No children to verbalize
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // ObjectTypeListWrapper class
		#endregion // Object Type Reports
		#region Fact Type Reports
		#region FactTypePageReport class
		/// <summary>
		/// Represents an individual Fact Type report
		/// </summary>
		protected class FactTypePageReport : IVerbalizeCustomChildren, IVerbalizeFilterChildren
		{
			#region Member Variables
			private FactType myFactType;
			private IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets;
			private VerbalizationReportContent myReportContent;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of FactTypePageReport
			/// </summary>
			public FactTypePageReport(FactType factType, VerbalizationReportContent reportContent, IVerbalizationSets<ReportVerbalizationSnippetType> snippets)
			{
				myFactType = factType;
				myReportContent = reportContent;
				mySnippets = snippets;
			}
			#endregion // Constructor
			#region IVerbalizeCustomChildren Implementation
			IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign)
			{
				return GetCustomChildVerbalizations(filter, verbalizationOptions, sign);
			}
			/// <summary>
			/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
			/// </summary>
			protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign)
			{
				yield return CustomChildVerbalizer.VerbalizeInstance(new FactTypePageHeaderSummary(myFactType, filter, false));
				yield return CustomChildVerbalizer.VerbalizeInstance(new FactTypePageObjectTypeSection(ReportVerbalizationSnippetType.ObjectTypeRelationshipValueLink, myFactType));
				yield return CustomChildVerbalizer.VerbalizeInstance(new GenericConstraintSection(myReportContent, GetConstraintsFromFactType(myFactType, myReportContent)));
				if (myFactType.Objectification != null)
				{
					yield return CustomChildVerbalizer.VerbalizeInstance(new GenericSuperTypeSection(myFactType.Objectification.NestingType));
					yield return CustomChildVerbalizer.VerbalizeInstance(new GenericSubTypeSection(myFactType.Objectification.NestingType));
				}
			}
			#endregion // IVerbalizeCustomChildren Implementation
			#region IVerbalizeFilterChildren Members
			CustomChildVerbalizer IVerbalizeFilterChildren.FilterChildVerbalizer(object child, VerbalizationSign sign)
			{
				return FilterChildVerbalizer(child, sign);
			}
			/// <summary>
			/// Provides an opportunity for a parent object to filter the
			/// verbalization of aggregated child verbalization implementations
			/// </summary>
			/// <param name="child">A direct or indirect child object.</param>
			/// <param name="sign">The preferred verbalization sign</param>
			/// <returns>
			/// Return the provided childVerbalizer to verbalize normally, null to block verbalization, or an
			/// alternate IVerbalize. The value is returned with a boolean option. The element will be disposed with
			/// this is true.
			/// </returns>
			protected CustomChildVerbalizer FilterChildVerbalizer(object child, VerbalizationSign sign)
			{
				if (child is IConstraint)
				{
					return CustomChildVerbalizer.Block;
				}
				return CustomChildVerbalizer.VerbalizeInstance(child as IVerbalize);
			}
			#endregion // IVerbalizeFilterChildren Members
		}
		#endregion // FactTypePageReport class
		#region FactTypePageHeaderSummary class
		/// <summary>
		/// Represents the Summary section of the FactType page Verbalization Report
		/// </summary>
		protected class FactTypePageHeaderSummary : IVerbalize
		{
			#region Member Variables
			private IVerbalize myVerbalizationObject;
			private IVerbalizeFilterChildren myFilter;
			private bool myTopLevelSection;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of the <see cref="FactTypePageHeaderSummary"/> class.
			/// </summary>
			/// <param name="verbalizationObject">The verbalization object.</param>
			/// <param name="filter">The <see cref="IVerbalizeFilterChildren"/> you want to use to filter aggregates.</param>
			/// <param name="topLevelSection">Set to <see langword="true"/> if the head is used as a section in a top-level page.</param>
			public FactTypePageHeaderSummary(IVerbalize verbalizationObject, IVerbalizeFilterChildren filter, bool topLevelSection)
			{
				myVerbalizationObject = verbalizationObject;
				myFilter = filter;
				myTopLevelSection = topLevelSection;
			}
			#endregion // Constructor
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(myTopLevelSection ? ReportVerbalizationSnippetType.FactTypeSectionHeader : ReportVerbalizationSnippetType.FactTypePageHeader), (myVerbalizationObject as FactType).Name));
				writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericSummaryOpen));
				verbalizationContext.DeferVerbalization(myVerbalizationObject, DeferVerbalizationOptions.AlwaysWriteLine, myFilter);
				// This must be called before additional calls to the writer or an increased indentation will not decrease before
				// the close element is written.
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericSummaryClose));
				return true;
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // FactTypePageHeaderSummary class
		#region FactTypePageObjectTypeSection class
		/// <summary>
		/// Represents the Fact Type List
		/// </summary>
		protected class FactTypePageObjectTypeSection : IVerbalize
		{
			#region Member Variables
			private FactType myFactType;
			private IList<ObjectType> objectTypeList;
			private ReportVerbalizationSnippetType myObjectTypeSnippet;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of ObjectTypePageFactTypeSection
			/// </summary>
			/// <param name="objectTypeSnippet">The object type snippet.</param>
			/// <param name="factType">The FactType.</param>
			public FactTypePageObjectTypeSection(ReportVerbalizationSnippetType objectTypeSnippet, FactType factType)
			{
				myObjectTypeSnippet = objectTypeSnippet;
				myFactType = factType;
			}
			#endregion // Constructor
			#region Property
			/// <summary>
			/// Gets the object type list.
			/// </summary>
			/// <value>The object type list.</value>
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
			#endregion // Property
			#region IVerbalize Members
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				IList<ObjectType> objectTypes = ObjectTypeList;
				int objectTypeCount = objectTypes.Count;

				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.FactTypePageObjectTypeListOpen));
				if (objectTypeCount != 0)
				{
					ObjectTypeVerbalizationWrapper wrapper = new ObjectTypeVerbalizationWrapper(myObjectTypeSnippet, null);
					IVerbalize verbalize = wrapper;
					for (int i = 0; i < objectTypeCount; ++i)
					{
						wrapper.VerbalizationObject = objectTypes[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.FactTypePageObjectTypeListClose));
				return false; // No children to verbalize
			}
			#endregion
		}
		#endregion // FactTypePageObjectTypeSection class
		#region FactTypeVerbalizationWrapper class
		/// <summary>
		/// Provides a wrapper for FactType verbalizations
		/// </summary>
		protected class FactTypeVerbalizationWrapper : IVerbalize
		{
			#region Member Variables
			private FactType myVerbalizationObject;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of the <see cref="FactTypeVerbalizationWrapper"/> class.
			/// </summary>
			/// <param name="verbalizationObject">The verbalization object.</param>
			public FactTypeVerbalizationWrapper(FactType verbalizationObject)
			{
				myVerbalizationObject = verbalizationObject;
			}
			#endregion // Constructor
			#region Accessor properties
			/// <summary>
			/// Access the verbalization object
			/// </summary>
			public FactType VerbalizationObject
			{
				get
				{
					return myVerbalizationObject;
				}
				set
				{
					myVerbalizationObject = value;
				}
			}
			#endregion // Accessor properties
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemOpen));
				writer.Write(string.Format(snippets.GetSnippet(ReportVerbalizationSnippetType.FactTypeRelationshipLinkOpen), AsFileName(myVerbalizationObject.Name)));
				((IVerbalize)myVerbalizationObject).GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
				writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.FactTypeRelationshipLinkClose));
				writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemClose));
				return false; // No children to verbalize
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // FactTypeVerbalizationWrapper class
		#endregion // Fact Type Reports
		#region Shared Reports
		#region VerbalizationReportTableOfContentsWrapper class
		/// <summary>
		/// Provides Verbalization of the Table of Contents for the Verbalization Reports
		/// </summary>
		protected class VerbalizationReportTableOfContentsWrapper : IVerbalize
		{
			#region Member Variables
			private VerbalizationReportContent myReportContent;
			private IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of VerbalizationReportTableOfContentsWrapper
			/// </summary>
			public VerbalizationReportTableOfContentsWrapper(VerbalizationReportContent reportContent, IVerbalizationSets<ReportVerbalizationSnippetType> snippets)
			{
				myReportContent = reportContent;
				mySnippets = snippets;
			}
			#endregion // Constructor
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
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

				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				writer.Write(string.Format(mySnippets.GetSnippet(ReportVerbalizationSnippetType.ReportDocumentContents), mySnippets.GetSnippet(snippetFormat)));
				return true;
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // VerbalizationReportTableOfContentsWrapper class
		#region GenericConstraintSection class
		/// <summary>
		/// Represents the Constraint section of the Verbalization Report Pages
		/// </summary>
		protected class GenericConstraintSection : IVerbalize
		{
			#region Member Variables
			private VerbalizationReportContent myReportContent;
			private IDictionary<ConstraintType, IList<IConstraint>> myConstraintList;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of ObjectTypePageConstraintSection
			/// </summary>
			public GenericConstraintSection(VerbalizationReportContent reportContent, IDictionary<ConstraintType, IList<IConstraint>> constraintList)
			{
				myReportContent = reportContent;
				myConstraintList = constraintList;
			}
			#endregion // Constructor
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);

				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericConstraintListOpen));
				ConstraintVerbalizationWrapper wrapper = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, null);
				IVerbalize verbalize = wrapper;

				// Internal Uniqueness Constraints
				if ((myReportContent & VerbalizationReportContent.InternalUniquenessConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.InternalUniqueness];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// External Uniqueness Constraints
				if ((myReportContent & VerbalizationReportContent.ExternalUniquenessConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.ExternalUniqueness];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Frequency Constraints
				if ((myReportContent & VerbalizationReportContent.FrequencyConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Frequency];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Simple Mandatory Constraints
				if ((myReportContent & VerbalizationReportContent.SimpleMandatoryConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.SimpleMandatory];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Disjunctive Mandatory Constraints
				if ((myReportContent & VerbalizationReportContent.DisjunctiveMandatoryConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.DisjunctiveMandatory];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Ring Constraints
				if ((myReportContent & VerbalizationReportContent.RingConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Ring];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Equality Constraints
				if ((myReportContent & VerbalizationReportContent.EqualityConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Equality];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Exclusion Constraints
				if ((myReportContent & VerbalizationReportContent.ExclusionConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Exclusion];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Subset Constraints
				if ((myReportContent & VerbalizationReportContent.SubsetConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Subset];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}

				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericConstraintListClose));
				return false; // No children to verbalize
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // GenericConstraintSection class
		#region GenericSnippetVerbalizer class
		/// <summary>
		/// Wraps any given Snippet for verbalization
		/// </summary>
		protected class GenericSnippetVerbalizer : IVerbalize
		{
			#region Member Variables
			private ReportVerbalizationSnippetType mySnippet;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Initializes a new instance of GenericSnippetVerbalizer
			/// </summary>
			public GenericSnippetVerbalizer(ReportVerbalizationSnippetType snippet)
			{
				mySnippet = snippet;
			}
			#endregion // Constructor
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				writer.Write(snippets.GetSnippet(mySnippet));
				return true;
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // GenericSnippetVerbalizer class
		#region GenericSuperTypeSection class
		/// <summary>
		/// Represents the Super Type section of a Verbalization Report Page
		/// </summary>
		protected class GenericSuperTypeSection : IVerbalize
		{
			#region Member Variables
			private ObjectType myObjectType;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of GenericSuperTypeSection
			/// </summary>
			public GenericSuperTypeSection(ObjectType objectType)
			{
				myObjectType = objectType;
			}
			#endregion // Constructor
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSuperTypeListOpen));

				ObjectTypeVerbalizationWrapper wrapper = null;
				IVerbalize verbalize = null;
				bool hasContent = false;
				foreach (ObjectType objectType in myObjectType.SupertypeCollection)
				{
					if (!hasContent)
					{
						wrapper = new ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeRelationshipValueLink, objectType);
						verbalize = wrapper;
						hasContent = true;
					}
					else
					{
						wrapper.VerbalizationObject = objectType;
					}
					IVerbalize instance = new ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeRelationshipValueLink, objectType);
					verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
				}
				if (!hasContent)
				{
					writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.EmptyContentListItemSnippet));
				}

				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSuperTypeListClose));
				return false; // No children to verbalize
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // GenericSuperTypeSection class
		#region GenericSubTypeSection class
		/// <summary>
		/// Represents the Sub Type section of a Verbalization Report Page
		/// </summary>
		protected class GenericSubTypeSection : IVerbalize
		{
			#region Member Variables
			private ObjectType myObjectType;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of GenericSuperTypeSection
			/// </summary>
			public GenericSubTypeSection(ObjectType objectType)
			{
				myObjectType = objectType;
			}
			#endregion // Constructor
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSubTypeListOpen));

				ObjectTypeVerbalizationWrapper wrapper = null;
				IVerbalize verbalize = null;
				bool hasContent = false;
				foreach (ObjectType objectType in myObjectType.SubtypeCollection)
				{
					if (!hasContent)
					{
						wrapper = new ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeRelationshipValueLink, objectType);
						verbalize = wrapper;
						hasContent = true;
					}
					else
					{
						wrapper.VerbalizationObject = objectType;
					}
					IVerbalize instance = new ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeRelationshipValueLink, objectType);
					verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
				}
				if (!hasContent)
				{
					writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.EmptyContentListItemSnippet));
				}

				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSubTypeListClose));
				return false; // No children to verbalize
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // GenericSubTypeSection class
		#region FactTypeConstraintValidationListReport class
		/// <summary>
		/// Represents the FactType/Constraint Validation List
		/// </summary>
		protected class FactTypeConstraintValidationListReport : IVerbalizeCustomChildren, IVerbalizeFilterChildren
		{
			#region Member Variables
			private ORMModel myModel;
			private IList<FactType> myFactTypeList;
			private IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets;
			private VerbalizationReportContent myReportContent;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of FactTypeConstraintValidationListReport
			/// </summary>
			/// <param name="model">Context model</param>
			/// <param name="factTypeList">The fact type list.</param>
			/// <param name="reportContent">Content of the report.</param>
			/// <param name="snippets">The snippets.</param>
			public FactTypeConstraintValidationListReport(ORMModel model, IList<FactType> factTypeList, VerbalizationReportContent reportContent, IVerbalizationSets<ReportVerbalizationSnippetType> snippets)
			{
				myModel = model;
				myFactTypeList = factTypeList;
				mySnippets = snippets;
				myReportContent = reportContent;
			}
			#endregion // Constructor
			#region IVerbalizeCustomChildren Implementation
			IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign)
			{
				return GetCustomChildVerbalizations(filter, verbalizationOptions, sign);
			}
			/// <summary>
			/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
			/// </summary>
			protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign)
			{
				yield return CustomChildVerbalizer.VerbalizeInstance(new VerbalizationReportTableOfContentsWrapper(myReportContent, mySnippets));
				yield return CustomChildVerbalizer.VerbalizeInstance(new ModelContextWrapper(myModel, ReportVerbalizationSnippetType.ContextModelDescriptionOpen, ReportVerbalizationSnippetType.ContextModelDescriptionClose));
				yield return CustomChildVerbalizer.VerbalizeInstance(new GenericSnippetVerbalizer(ReportVerbalizationSnippetType.FactTypeConstraintValidationHeader));
				int factTypeCount = myFactTypeList.Count;
				for (int i = 0; i < factTypeCount; ++i)
				{
					yield return CustomChildVerbalizer.VerbalizeInstance(new FactTypePageHeaderSummary(myFactTypeList[i], filter, true));
					yield return CustomChildVerbalizer.VerbalizeInstance(new FactTypePageObjectTypeSection(ReportVerbalizationSnippetType.ObjectTypeListObjectTypeValueLink, myFactTypeList[i]));
					yield return CustomChildVerbalizer.VerbalizeInstance(new ConstraintValidationSection(myReportContent, GetConstraintsFromFactType(myFactTypeList[i], myReportContent)));
				}
				yield return CustomChildVerbalizer.VerbalizeInstance(new GenericSnippetVerbalizer(ReportVerbalizationSnippetType.FactTypeConstraintValidationSignature));
			}
			#endregion // IVerbalizeCustomChildren Implementation
			#region IVerbalizeFilterChildren Implementation
			CustomChildVerbalizer IVerbalizeFilterChildren.FilterChildVerbalizer(object child, VerbalizationSign sign)
			{
				return FilterChildVerbalizer(child, sign);
			}
			/// <summary>
			/// Provides an opportunity for a parent object to filter the
			/// verbalization of aggregated child verbalization implementations
			/// </summary>
			/// <param name="child">A direct or indirect child object.</param>
			/// <param name="sign">The preferred verbalization sign</param>
			/// <returns>
			/// Return the provided childVerbalizer to verbalize normally, null to block verbalization, or an
			/// alternate IVerbalize. The value is returned with a boolean option. The element will be disposed with
			/// this is true.
			/// </returns>
			protected CustomChildVerbalizer FilterChildVerbalizer(object child, VerbalizationSign sign)
			{
				if (child is IConstraint)
				{
					return CustomChildVerbalizer.Block;
				}
				return CustomChildVerbalizer.VerbalizeInstance(child as IVerbalize);
			}
			#endregion // IVerbalizeFilterChildren Implementation
		}

		#endregion // FactTypeConstraintValidationListReport class
		#region ConstraintValidationSection class
		/// <summary>
		/// Represents the Validation Constraint section of the Verbalization Report
		/// </summary>
		protected class ConstraintValidationSection : IVerbalize
		{
			#region Member Variables
			private VerbalizationReportContent myReportContent;
			private IDictionary<ConstraintType, IList<IConstraint>> myConstraintList;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of ConstraintValidationSection
			/// </summary>
			public ConstraintValidationSection(VerbalizationReportContent reportContent, IDictionary<ConstraintType, IList<IConstraint>> constraintList)
			{
				myReportContent = reportContent;
				myConstraintList = constraintList;
			}
			#endregion // Constructor
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);

				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericConstraintListOpen));
				ConstraintVerbalizationWrapper wrapper = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, null);
				IVerbalize verbalize = wrapper;

				// Internal Uniqueness Constraints
				if ((myReportContent & VerbalizationReportContent.InternalUniquenessConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.InternalUniqueness];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// External Uniqueness Constraints
				if ((myReportContent & VerbalizationReportContent.ExternalUniquenessConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.ExternalUniqueness];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Frequency Constraints
				if ((myReportContent & VerbalizationReportContent.FrequencyConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Frequency];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Simple Mandatory Constraints
				if ((myReportContent & VerbalizationReportContent.SimpleMandatoryConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.SimpleMandatory];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Disjunctive Mandatory Constraints
				if ((myReportContent & VerbalizationReportContent.DisjunctiveMandatoryConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.DisjunctiveMandatory];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Ring Constraints
				if ((myReportContent & VerbalizationReportContent.RingConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Ring];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Equality Constraints
				if ((myReportContent & VerbalizationReportContent.EqualityConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Equality];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Exclusion Constraints
				if ((myReportContent & VerbalizationReportContent.ExclusionConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Exclusion];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}
				// Subset Constraints
				if ((myReportContent & VerbalizationReportContent.SubsetConstraints) != 0)
				{
					IList<IConstraint> constraintList = myConstraintList[ConstraintType.Subset];
					int constraintCount = constraintList.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						wrapper.VerbalizationObject = constraintList[i];
						verbalize.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
					}
				}

				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericConstraintListClose));
				return false; // No children to verbalize
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // ConstraintValidationSection class
		#region ConstraintVerbalizationWrapper class
		/// <summary>
		/// Provides a wrapper for verbalizing Constraints
		/// </summary>
		protected class ConstraintVerbalizationWrapper : IVerbalize
		{
			#region Member Variables
			private object myVerbalizationObject;
			private ReportVerbalizationSnippetType myOpeningSnippet;
			private ReportVerbalizationSnippetType myClosingSnippet;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of the <see cref="ConstraintVerbalizationWrapper"/> class.
			/// </summary>
			/// <param name="openingSnippet">The opening snippet.</param>
			/// <param name="closingSnippet">The closing snippet.</param>
			/// <param name="verbalizationObject">The verbalization object.</param>
			public ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType openingSnippet, ReportVerbalizationSnippetType closingSnippet, object verbalizationObject)
			{
				myOpeningSnippet = openingSnippet;
				myClosingSnippet = closingSnippet;
				myVerbalizationObject = verbalizationObject;
			}
			#endregion // Constructor
			#region Accessor properties
			/// <summary>
			/// Access the verbalization object
			/// </summary>
			public object VerbalizationObject
			{
				get
				{
					return myVerbalizationObject;
				}
				set
				{
					myVerbalizationObject = value;
				}
			}
			#endregion // Accessor properties
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				object verbalizationObject = this.myVerbalizationObject;
				if (verbalizationObject == null || verbalizationContext.AlreadyVerbalized(verbalizationObject))
				{
					return false;
				}
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
				writer.Write(snippets.GetSnippet(myOpeningSnippet), TypeDescriptor.GetComponentName(verbalizationObject), TypeDescriptor.GetClassName(verbalizationObject));
				verbalizationContext.DeferVerbalization(verbalizationObject, DeferVerbalizationOptions.AlwaysWriteLine, null);
				// This must be called before additional calls to the writer or an increased indentation will not decrease before
				// the close element is written.
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				writer.Write(snippets.GetSnippet(myClosingSnippet));
				return true;
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // ConstraintVerbalizationWrapper class
		#region ModelContextWrapper class
		/// <summary>
		/// Provide report verbalization for the context ORM model
		/// </summary>
		protected class ModelContextWrapper : IVerbalize
		{
			#region Member Variables
			private ORMModel myModel;
			private ReportVerbalizationSnippetType myOpenSnippet;
			private ReportVerbalizationSnippetType myCloseSnippet;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Initializes a new instance of ModelContextWrapper
			/// </summary>
			/// <param name="model">The context model to describe</param>
			/// <param name="openSnippet">The open snippet.</param>
			/// <param name="closeSnippet">The close snippet.</param>
			public ModelContextWrapper(ORMModel model, ReportVerbalizationSnippetType openSnippet, ReportVerbalizationSnippetType closeSnippet)
			{
				myModel = model;
				myOpenSnippet = openSnippet;
				myCloseSnippet = closeSnippet;
			}
			#endregion // Constructor
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
			/// <summary>
			/// Implements <see cref="IVerbalize.GetVerbalization"/>
			/// </summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				ORMModel model = myModel;
				if (model != null)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(myOpenSnippet), model.Name));
					verbalizationContext.DeferVerbalization(model, DeferVerbalizationOptions.AlwaysWriteLine, null);
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					writer.Write(snippets.GetSnippet(myCloseSnippet));
				}
				return false; // No children to verbalize
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // ModelContextWrapper class
		#endregion // Shared Reports
		#endregion // Verbalization helper classes
	}
	#endregion // VerbalizationReportGenerator class
}
