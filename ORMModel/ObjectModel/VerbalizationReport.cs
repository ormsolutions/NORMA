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
	#region VerbalizationReport Implementation
	#region VerbalizationReportContent enum
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
			bool isNegative = false;
			IDictionary<Type, IVerbalizationSets> snippetsDictionary = (model.Store as IORMToolServices).GetVerbalizationSnippetsDictionary(HtmlReport.HtmlReportTargetName);
			IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			Dictionary<IVerbalize, IVerbalize> verbalized;
			Stream fileStream;
			TextWriter textWriter;
			VerbalizationCallbackWriter writer;
			#endregion // Member Variable
			#region ObjectType Reports
			ObjectType[] objectTypeList = null;
			if (0 != (reportContent & VerbalizationReportContent.ObjectTypes))
			{
				verbalized = new Dictionary<IVerbalize, IVerbalize>();
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

					ObjectTypeListReport objectTypeListReport = new ObjectTypeListReport(objectTypeList, snippets, ReportVerbalizationSnippetType.ObjectTypeListHeader, ReportVerbalizationSnippetType.ObjectTypeListFooter, reportContent);
					VerbalizationHelper.VerbalizeElement(
						objectTypeListReport,
						snippetsDictionary,
						verbalized,
						null,
						isNegative,
						writer,
						false,
						ref firstCall);

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
						ObjectType objectType = objectTypeList[i];

						fileStream = new FileStream(Path.Combine(objectTypeDir, objectType.Name + ".html"), FileMode.Create, FileAccess.ReadWrite);
						textWriter = new StreamWriter(fileStream);
						writer = new VerbalizationReportCallbackWriter(snippetsDictionary, textWriter);

						ObjectTypePageReport objectTypePageReport = new ObjectTypePageReport(objectType, reportContent, snippets);
						VerbalizationHelper.VerbalizeElement(
							objectTypePageReport,
							snippetsDictionary,
							verbalized,
							objectTypePageReport,
							isNegative,
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
				verbalized = new Dictionary<IVerbalize, IVerbalize>();

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
						fileStream = new FileStream(Path.Combine(factTypeDir, factTypeList[i].Name + ".html"), FileMode.Create, FileAccess.ReadWrite);
						textWriter = new StreamWriter(fileStream);
						writer = new VerbalizationReportCallbackWriter(snippetsDictionary, textWriter);

						FactTypePageReport factTypePageReport = new FactTypePageReport(factTypeList[i], reportContent, snippets);
						VerbalizationHelper.VerbalizeElement(
							factTypePageReport,
							snippetsDictionary,
							verbalized,
							factTypePageReport,
							isNegative,
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

					FactTypeConstraintValidationListReport factTypeConstraintValidationReport = new FactTypeConstraintValidationListReport(factTypeList, reportContent, snippets);
					VerbalizationHelper.VerbalizeElement(
						factTypeConstraintValidationReport,
						snippetsDictionary,
						verbalized,
						factTypeConstraintValidationReport,
						isNegative,
						writer,
						false,
						ref firstCall);

					textWriter.Flush();
					textWriter.Close();
				}
			}
			#endregion // Constraint Validation Report
		}
		#endregion // Public Methods
		#region Private Methods
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
		#endregion // Private Methods
	}
	#endregion // VerbalizationReportGenerator class
	#endregion // VerbalizationReport Implementation
	#region VerbalizationCallbackWriters
	#region VerbalizationCallbackWriter class
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
	#endregion // VerbalizationCallbackWriter class
	#region VerbalizationReportCallbackWriter class
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
			Writer.Write(myReportSnippets.GetSnippet(ReportVerbalizationSnippetType.ReportDocumentFooter));
		}

	}
	#endregion // VerbalizationReportCallbackWriter class
	#endregion // VerbalizationCallbackWriters
	#region VerbalizationHelper class
	/// <summary>
	/// Provides helper methods for Verbalizations
	/// </summary>
	public static class VerbalizationHelper
	{
		#region VerbalizationResult Enum
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
		#endregion // VerbalizationResult Enum
		#region VerbalizationHandler Delegate
		/// <summary>
		/// Callback for child verbalizations
		/// </summary>
		private delegate VerbalizationResult VerbalizationHandler(VerbalizationCallbackWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IVerbalize verbalizer, VerbalizationHandler callback, int indentationLevel, bool isNegative, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel);
		#endregion // VerbalizationHandler Delegate
		#region VerbalizationContextImpl class
		private sealed class VerbalizationContextImpl : IVerbalizationContext
		{
			/// <summary>
			/// A callback delegate enabling a verbalizer to tell
			/// the hosting window that it is about to begin verbalizing.
			/// This enables the host window to delay writing content outer
			/// content until it knows that text is about to be written by
			/// the verbalizer to the writer
			/// </summary>
			/// <param name="content">The style of verbalization content</param>
			public delegate void NotifyBeginVerbalization(VerbalizationContent content);
			public delegate void NotifyDeferVerbalization(object target, IVerbalizeFilterChildren childFilter);
			private NotifyBeginVerbalization myBeginCallback;
			private NotifyDeferVerbalization myDeferCallback;
			public VerbalizationContextImpl(NotifyBeginVerbalization beginCallback, NotifyDeferVerbalization deferCallback)
			{
				myBeginCallback = beginCallback;
				myDeferCallback = deferCallback;
			}
			#region IVerbalizationContext Implementation
			void IVerbalizationContext.BeginVerbalization(VerbalizationContent content)
			{
				myBeginCallback(content);
			}
			void IVerbalizationContext.DeferVerbalization(object target, IVerbalizeFilterChildren childFilter)
			{
				if (myDeferCallback != null)
				{
					myDeferCallback(target, childFilter);
				}
			}
			#endregion // IVerbalizationContext Implementation
		}
		#endregion // VerbalizationContextImpl class
		/// <summary>
		/// Handles the callback made by VerbalizeElement to execute verbalization
		/// </summary>
		/// <param name="writer">The VerbalizationCallbackWriter object used to write target specific snippets</param>
		/// <param name="snippetsDictionary"></param>
		/// <param name="alreadyVerbalized"></param>
		/// <param name="verbalizer">The IVerbalize element to verbalize</param>
		/// <param name="callback">The original callback handler.</param>
		/// <param name="indentationLevel">The indentation level of the verbalization</param>
		/// <param name="isNegative"></param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		/// <param name="firstWrite"></param>
		/// <param name="lastLevel"></param>
		/// <returns></returns>
		private static VerbalizationResult VerbalizeElement_VerbalizationResult(VerbalizationCallbackWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IVerbalize verbalizer, VerbalizationHandler callback, int indentationLevel, bool isNegative, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel)
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
				new VerbalizationContextImpl(
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
				delegate(object target, IVerbalizeFilterChildren childFilter)
				{
					bool localfcp = localFirstCallPending;
					bool localfw = localFirstWrite;
					int localll = localLastLevel;
					VerbalizationHelper.VerbalizeElement(
						target as ModelElement,
						snippetsDictionary,
						alreadyVerbalized,
						childFilter,
						writer,
						callback,
						isNegative,
						indentationLevel,
						true,
						ref localfcp,
						ref localfw,
						ref localll);
					localFirstCallPending = localfcp;
					localFirstWrite = localfw;
					localLastLevel = localll;
				}),
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
		/// <param name="filter"></param>
		/// <param name="isNegative">Use the negative form of the reading</param>
		/// <param name="writer">The VerbalizationCallbackWriter for verbalization output</param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		public static void VerbalizeElement(IVerbalizeCustomChildren customChildren, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IVerbalizeFilterChildren filter, bool isNegative, VerbalizationCallbackWriter writer, bool writeSecondaryLines, ref bool firstCallPending)
		{
			int lastLevel = 0;
			bool firstWrite = true;
			bool localFirstCallPending = firstCallPending;
			VerbalizeCustomChildren(
				customChildren,
				writer,
				new VerbalizationHandler(VerbalizeElement_VerbalizationResult),
				snippetsDictionary,
				alreadyVerbalized,
				filter,
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
		/// <param name="outerFilter"></param>
		/// <param name="writer"></param>
		/// <param name="callback"></param>
		/// <param name="isNegative"></param>
		/// <param name="indentLevel"></param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		/// <param name="firstWrite"></param>
		/// <param name="lastLevel"></param>
		private static void VerbalizeElement(ModelElement element, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IVerbalizeFilterChildren outerFilter, VerbalizationCallbackWriter writer, VerbalizationHandler callback, bool isNegative, int indentLevel, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel)
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
			if (outerFilter != null && parentVerbalize != null)
			{
				CustomChildVerbalizer filterResult = outerFilter.FilterChildVerbalizer(parentVerbalize, isNegative);
				parentVerbalize = filterResult.Instance;
				disposeVerbalizer = filterResult.Options;
			}
			try
			{
				VerbalizationResult result = (parentVerbalize != null) ? callback(writer, snippetsDictionary, alreadyVerbalized, parentVerbalize, callback, indentLevel, isNegative, writeSecondaryLines, ref firstCallPending, ref firstWrite, ref lastLevel) : VerbalizationResult.NotVerbalized;
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
					IVerbalizeFilterChildren filter = parentVerbalize as IVerbalizeFilterChildren;
					if (filter != null)
					{
						if (outerFilter != null)
						{
							filter = new CompositeChildFilter(outerFilter, filter);
						}
					}
					else
					{
						filter = outerFilter;
					}
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
					VerbalizeCustomChildren(
						customChildren,
						writer,
						callback,
						snippetsDictionary,
						alreadyVerbalized,
						filter,
						isNegative,
						indentLevel,
						writeSecondaryLines,
						ref firstCallPending,
						ref firstWrite,
						ref lastLevel);
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
		#region CompositeChildFilter class
		private sealed class CompositeChildFilter : IVerbalizeFilterChildren
		{
			#region Member Variables
			private IVerbalizeFilterChildren[] myFilters;
			#endregion // Member Variables
			#region Constructor
			public CompositeChildFilter(params IVerbalizeFilterChildren[] filters)
			{
				myFilters = filters;
			}
			#endregion // Constructor
			#region IVerbalizeFilterChildren Implementation
			CustomChildVerbalizer IVerbalizeFilterChildren.FilterChildVerbalizer(object child, bool isNegative)
			{
				CustomChildVerbalizer retVal = CustomChildVerbalizer.Empty;
				IVerbalizeFilterChildren[] filters = myFilters;
				for (int i = 0; i < filters.Length; ++i)
				{
					retVal = filters[i].FilterChildVerbalizer(child, isNegative);
					if (!retVal.IsEmpty)
					{
						break;
					}
					else if (retVal.IsBlocked)
					{
						break;
					}
				}
				return retVal;
			}
			#endregion // IVerbalizeFilterChildren Implementation
		}
		#endregion // CompositeChildFilter class
		/// <summary>
		/// Verbalizes the children specified by the given IVerbalizeCustomChildren implementation
		/// </summary>
		/// <param name="customChildren">The IVerbalizeCustomChildren implementation to verbalize</param>
		/// <param name="writer">The target specific Writer to use</param>
		/// <param name="callback">The delegate used to handle the verbalization</param>
		/// <param name="snippetsDictionary"></param>
		/// <param name="alreadyVerbalized"></param>
		/// <param name="filter"></param>
		/// <param name="isNegative">Whether or not the verbalization is negative</param>
		/// <param name="indentationLevel">The current level of indentation</param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		/// <param name="firstWrite"></param>
		/// <param name="lastLevel"></param>
		private static void VerbalizeCustomChildren(IVerbalizeCustomChildren customChildren, VerbalizationCallbackWriter writer, VerbalizationHandler callback, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IVerbalizeFilterChildren filter, bool isNegative, int indentationLevel, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel)
		{
			if (customChildren != null)
			{
				foreach (CustomChildVerbalizer customChild in customChildren.GetCustomChildVerbalizations(filter, isNegative))
				{
					IVerbalize childVerbalize = customChild.Instance;
					if (childVerbalize != null)
					{
						try
						{
							callback(writer, snippetsDictionary, alreadyVerbalized, childVerbalize, callback, indentationLevel, isNegative, writeSecondaryLines, ref firstCallPending, ref firstWrite, ref lastLevel);
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
				if (!(roleCollecton[i].FactType is SubtypeFact) && !factList.Contains(roleCollecton[i].FactType))
				{
					factList.Add(roleCollecton[i].FactType);
				}
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
	#region ObjectTypePageReport class
	/// <summary>
	/// Represents an individual Object Type report
	/// </summary>
	public class ObjectTypePageReport : IVerbalizeCustomChildren, IVerbalizeFilterChildren
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
		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			return GetCustomChildVerbalizations(filter, isNegative);
		}
		/// <summary>
		/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			yield return new CustomChildVerbalizer(new ObjectTypePageHeaderSummary(myObjectType, filter));
			yield return new CustomChildVerbalizer(new ObjectTypePageFactTypeSection(myObjectType));
			yield return new CustomChildVerbalizer(new ObjectTypePageRelationshipSection(myObjectType));
			yield return new CustomChildVerbalizer(new GenericSuperTypeSection(myObjectType));
			yield return new CustomChildVerbalizer(new GenericSubTypeSection(myObjectType));
		}
		#endregion // IVerbalizeCustomChildren Implementation
		#region IVerbalizeFilterChildren Implementation
		CustomChildVerbalizer IVerbalizeFilterChildren.FilterChildVerbalizer(object child, bool isNegative)
		{
			return FilterChildVerbalizer(child, isNegative);
		}
		/// <summary>
		/// Provides an opportunity for a parent object to filter the
		/// verbalization of aggregated child verbalization implementations
		/// </summary>
		/// <param name="child">A direct or indirect child object.</param>
		/// <param name="isNegative">true if a negative verbalization is being requested</param>
		/// <returns>
		/// Return the provided childVerbalizer to verbalize normally, null to block verbalization, or an
		/// alternate IVerbalize. The value is returned with a boolean option. The element will be disposed with
		/// this is true.
		/// </returns>
		protected CustomChildVerbalizer FilterChildVerbalizer(object child, bool isNegative)
		{
			return new CustomChildVerbalizer(child as IVerbalize);
		}
		#endregion // IVerbalizeFilterChildren Implementation
	}
	#endregion // ObjectTypePageReport class
	#region ObjectTypeListReport class
	/// <summary>
	/// Represents the Object Type List
	/// </summary>
	public class ObjectTypeListReport : IVerbalizeCustomChildren
	{
		#region Member Variables
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
		/// <param name="objectTypeList">The object type list.</param>
		/// <param name="snippets">The snippets.</param>
		/// <param name="headerSnippet">The header snippet.</param>
		/// <param name="footerSnippet">The footer snippet.</param>
		/// <param name="reportContent">Content of the report.</param>
		public ObjectTypeListReport(IList<ObjectType> objectTypeList, IVerbalizationSets<ReportVerbalizationSnippetType> snippets,
			ReportVerbalizationSnippetType headerSnippet, ReportVerbalizationSnippetType footerSnippet, VerbalizationReportContent reportContent)
		{
			myObjectTypeList = objectTypeList;
			mySnippets = snippets;
			myHeaderSnippet = headerSnippet;
			myFooterSnippet = footerSnippet;
			myReportContent = reportContent;
		}
		#endregion // Constructor
		#region IVerbalizeCustomChildren Implementation
		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			return GetCustomChildVerbalizations(filter, isNegative);
		}
		/// <summary>
		/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
		/// </summary>
		/// <param name="filter">A <see cref="IVerbalizeFilterChildren"/> instance. Can be <see langword="null"/>.
		/// If the <see cref="IVerbalizeFilterChildren.FilterChildVerbalizer">FilterChildVerbalizer</see> method returns
		/// <see cref="CustomChildVerbalizer.Block"/> for any constituent components used to create a <see cref="CustomChildVerbalizer"/>,
		/// then that custom child should not be created</param>
		/// <param name="isNegative">true if a negative verbalization is being requested</param>
		/// <returns>
		/// IEnumerable of CustomChildVerbalizer structures
		/// </returns>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			yield return new CustomChildVerbalizer(new VerbalizationReportTableOfContentsWrapper(myReportContent, mySnippets));
			yield return new CustomChildVerbalizer(new ObjectTypeListWrapper(myObjectTypeList, myHeaderSnippet, myFooterSnippet));
		}
		#endregion // IVerbalizeCustomChildren Implementation
	}
	#endregion // ObjectTypeListReport class
	#region ObjectTypePageHeaderSummary class
	/// <summary>
	/// Represents the Summary section of the ObjectType page Verbalization Report
	/// </summary>
	public class ObjectTypePageHeaderSummary : IVerbalize
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
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Verbalize in the requested form
		/// </summary>
		/// <param name="writer">The output text writer</param>
		/// <param name="snippetsDictionary">The IVerbalizationSets to use</param>
		/// <param name="verbalizationContext">A set callback function to interact with the outer verbalization context</param>
		/// <param name="isNegative">true for a negative reading</param>
		/// <returns>
		/// true to continue with child verbalization, otherwise false
		/// </returns>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(ReportVerbalizationSnippetType.ObjectTypePageHeader), (myVerbalizationObject as ObjectType).Name));
			writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericSummaryOpen));
			verbalizationContext.DeferVerbalization(myVerbalizationObject, myFilterChildren);
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
	public class ObjectTypePageFactTypeSection : IVerbalize
	{
		#region Member Variables
		private ObjectType myObjectType;
		private IList<FactType> uniqueFactList;
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
		private IList<FactType> UniqueFactList
		{
			get
			{
				if (uniqueFactList == null) uniqueFactList = VerbalizationHelper.GetFactTypesFromObjectType(myObjectType);
				return uniqueFactList;
			}
		}
		#endregion // Property
		#region IVerbalize Implementation
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Verbalize in the requested form
		/// </summary>
		/// <param name="writer">The output text writer</param>
		/// <param name="snippetsDictionary">The IVerbalizationSets to use</param>
		/// <param name="verbalizationContext">A set callback function to interact with the outer verbalization context</param>
		/// <param name="isNegative">true for a negative reading</param>
		/// <returns>
		/// true to continue with child verbalization, otherwise false
		/// </returns>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			bool retVal = true;
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			int factTypeCount = UniqueFactList.Count;

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.ObjectTypePageFactTypeListOpen));
			for (int i = 0; i < factTypeCount; ++i)
			{
				IVerbalize instance = new FactTypeVerbalizationWrapper(UniqueFactList[i]);
				if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
				{
					retVal = false;
				}
			}
			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.ObjectTypePageFactTypeListClose));
			return retVal;
		}
		#endregion // IVerbalize Implementation
	}
	#endregion // ObjectTypePageFactTypeSection class
	#region ObjectTypePageRelationshipSection class
	/// <summary>
	/// Represents the Relationship section of the ObjectType page Verbalization Report
	/// </summary>
	public class ObjectTypePageRelationshipSection : IVerbalize
	{
		#region Member Variables
		private ObjectType myObjectType;
		private IList<ObjectType> relatedObjectList;
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
		#endregion // Property
		#region IVerbalize Members
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericRelationshipsListOpen));

			int relatedObjectCount = RelatedObjectList.Count;
			for (int i = 0; i < relatedObjectCount; ++i)
			{
				IVerbalize instance = new ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeValueLink, RelatedObjectList[i]);
				if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
				{
					retVal = false;
				}
			}

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericRelationshipsListClose));
			return retVal;
		}
		#endregion
	}
	#endregion // ObjectTypePageRelationshipSection class
	#region ObjectTypeVerbalizationWrapper class
	/// <summary>
	/// Provides a wrapper for the ObjectType name value
	/// </summary>
	public class ObjectTypeVerbalizationWrapper : IVerbalize
	{
		#region Memeber Variables
		private ReportVerbalizationSnippetType myObjectTypeSnippet;
		private IVerbalize myVerbalizationObject;
		#endregion // Memeber Variables
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectTypeVerbalizationWrapper"/> class.
		/// </summary>
		/// <param name="objectTypeSnippet">The object type snippet.</param>
		/// <param name="verbalizationObject">The verbalization object.</param>
		public ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType objectTypeSnippet, IVerbalize verbalizationObject)
		{
			myObjectTypeSnippet = objectTypeSnippet;
			myVerbalizationObject = verbalizationObject;
		}
		#endregion // Constructor
		#region IVerbalize Implementation
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Verbalize in the requested form
		/// </summary>
		/// <param name="writer">The output text writer</param>
		/// <param name="snippetsDictionary">The IVerbalizationSets to use</param>
		/// <param name="verbalizationContext">A set callback function to interact with the outer verbalization context</param>
		/// <param name="isNegative">true for a negative reading</param>
		/// <returns>
		/// true to continue with child verbalization, otherwise false
		/// </returns>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemOpen));
			writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(myObjectTypeSnippet), (myVerbalizationObject as ObjectType).Name));
			writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemClose));
			return true;
		}
		#endregion // IVerbalize Implementation
	}
	#endregion // ObjectTypeVerbalizationWrapper class
	#region ObjectTypeListWrapper class
	/// <summary>
	/// Represents a list of ObjectTypes for a Verbalization Report
	/// </summary>
	public class ObjectTypeListWrapper : IVerbalize
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
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Verbalize in the requested form
		/// </summary>
		/// <param name="writer">The output text writer</param>
		/// <param name="snippetsDictionary">The IVerbalizationSets to use</param>
		/// <param name="verbalizationContext">A set callback function to interact with the outer verbalization context</param>
		/// <param name="isNegative">true for a negative reading</param>
		/// <returns>
		/// true to continue with child verbalization, otherwise false
		/// </returns>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			bool retVal = true;
			writer.Write(snippets.GetSnippet(myHeaderSnippet));
			int objectCount = myObjectTypeList.Count;
			for (int i = 0; i < objectCount; ++i)
			{
				IVerbalize instance = new ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeListObjectTypeValueLink, myObjectTypeList[i]);
				if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
				{
					retVal = false;
				}
			}
			writer.Write(snippets.GetSnippet(myFooterSnippet));
			return retVal;
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
	public class FactTypePageReport : IVerbalizeCustomChildren, IVerbalizeFilterChildren
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
		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			return GetCustomChildVerbalizations(filter, isNegative);
		}
		/// <summary>
		/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			yield return new CustomChildVerbalizer(new FactTypePageHeaderSummary(myFactType, filter));
			yield return new CustomChildVerbalizer(new FactTypePageObjectTypeSection(ReportVerbalizationSnippetType.ObjectTypeRelationshipValueLink, myFactType));
			yield return new CustomChildVerbalizer(new GenericConstraintSection(myReportContent, VerbalizationHelper.GetConstraintsFromFactType(myFactType, myReportContent)));
			if (myFactType.Objectification != null)
			{
				yield return new CustomChildVerbalizer(new GenericSuperTypeSection(myFactType.Objectification.NestingType));
				yield return new CustomChildVerbalizer(new GenericSubTypeSection(myFactType.Objectification.NestingType));
			}
		}
		#endregion // IVerbalizeCustomChildren Implementation
		#region IVerbalizeFilterChildren Members
		CustomChildVerbalizer IVerbalizeFilterChildren.FilterChildVerbalizer(object child, bool isNegative)
		{
			return FilterChildVerbalizer(child, isNegative);
		}
		/// <summary>
		/// Provides an opportunity for a parent object to filter the
		/// verbalization of aggregated child verbalization implementations
		/// </summary>
		/// <param name="child">A direct or indirect child object.</param>
		/// <param name="isNegative">true if a negative verbalization is being requested</param>
		/// <returns>
		/// Return the provided childVerbalizer to verbalize normally, null to block verbalization, or an
		/// alternate IVerbalize. The value is returned with a boolean option. The element will be disposed with
		/// this is true.
		/// </returns>
		protected CustomChildVerbalizer FilterChildVerbalizer(object child, bool isNegative)
		{
			if (child is IConstraint)
			{
				return CustomChildVerbalizer.Block;
			}
			return new CustomChildVerbalizer(child as IVerbalize);
		}
		#endregion // IVerbalizeFilterChildren Members
	}
	#endregion // FactTypePageReport class
	#region FactTypePageHeaderSummary class
	/// <summary>
	/// Represents the Summary section of the FactType page Verbalization Report
	/// </summary>
	public class FactTypePageHeaderSummary : IVerbalize
	{
		#region Member Variables
		private IVerbalize myVerbalizationObject;
		private IVerbalizeFilterChildren myFilter;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="FactTypePageHeaderSummary"/> class.
		/// </summary>
		/// <param name="verbalizationObject">The verbalization object.</param>
		/// <param name="filter">The <see cref="IVerbalizeFilterChildren"/> you want to use to filter aggregates.</param>
		public FactTypePageHeaderSummary(IVerbalize verbalizationObject, IVerbalizeFilterChildren filter)
		{
			myVerbalizationObject = verbalizationObject;
			myFilter = filter;
		}
		#endregion // Constructor
		#region IVerbalize Implementation
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Verbalize in the requested form
		/// </summary>
		/// <param name="writer">The output text writer</param>
		/// <param name="snippetsDictionary">The IVerbalizationSets to use</param>
		/// <param name="verbalizationContext">A set callback function to interact with the outer verbalization context</param>
		/// <param name="isNegative">true for a negative reading</param>
		/// <returns>
		/// true to continue with child verbalization, otherwise false
		/// </returns>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(ReportVerbalizationSnippetType.FactTypePageHeader), (myVerbalizationObject as FactType).Name));
			writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericSummaryOpen));
			verbalizationContext.DeferVerbalization(myVerbalizationObject, myFilter);
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
	public class FactTypePageObjectTypeSection : IVerbalize
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
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		/// <param name="writer">The output text writer</param>
		/// <param name="snippetsDictionary">The IVerbalizationSets to use</param>
		/// <param name="verbalizationContext">A set callback function to interact with the outer verbalization context</param>
		/// <param name="isNegative">true for a negative reading</param>
		/// <returns>
		/// true to continue with child verbalization, otherwise false
		/// </returns>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			int objectTypeCount = ObjectTypeList.Count;

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.FactTypePageObjectTypeListOpen));
			for (int i = 0; i < objectTypeCount; ++i)
			{
				IVerbalize instance = new ObjectTypeVerbalizationWrapper(myObjectTypeSnippet, ObjectTypeList[i]);
				if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
				{
					retVal = false;
				}
			}
			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.FactTypePageObjectTypeListClose));
			return retVal;
		}
		#endregion
	}
	#endregion // FactTypePageObjectTypeSection class
	#region FactTypeVerbalizationWrapper class
	/// <summary>
	/// Provides a wrapper for FactType verbalizations
	/// </summary>
	public class FactTypeVerbalizationWrapper : IVerbalize
	{
		#region Member Variables
		private IVerbalize myVerbalizationObject;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="FactTypeVerbalizationWrapper"/> class.
		/// </summary>
		/// <param name="verbalizationObject">The verbalization object.</param>
		public FactTypeVerbalizationWrapper(IVerbalize verbalizationObject)
		{
			myVerbalizationObject = verbalizationObject;
		}
		#endregion // Constructor
		#region IVerbalize Implementation
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Verbalize in the requested form
		/// </summary>
		/// <param name="writer">The output text writer</param>
		/// <param name="snippetsDictionary">The IVerbalizationSets to use</param>
		/// <param name="verbalizationContext">A set callback function to interact with the outer verbalization context</param>
		/// <param name="isNegative">true for a negative reading</param>
		/// <returns>
		/// true to continue with child verbalization, otherwise false
		/// </returns>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			bool retVal;
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemOpen));
			writer.Write(string.Format(snippets.GetSnippet(ReportVerbalizationSnippetType.FactTypeRelationshipLinkOpen), (myVerbalizationObject as FactType).Name));
			retVal = myVerbalizationObject.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
			writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.FactTypeRelationshipLinkClose));
			writer.Write(snippets.GetSnippet(ReportVerbalizationSnippetType.GenericListItemClose));
			return retVal;
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
	public class VerbalizationReportTableOfContentsWrapper : IVerbalize
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
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
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
	public class GenericConstraintSection : IVerbalize
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
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericConstraintListOpen));

			// Internal Uniqueness Constraints
			if ((myReportContent & VerbalizationReportContent.InternalUniquenessConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.InternalUniqueness];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}
			// External Uniqueness Constraints
			if ((myReportContent & VerbalizationReportContent.ExternalUniquenessConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.ExternalUniqueness];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}
			// Disjunctive Mandatory Constraints
			if ((myReportContent & VerbalizationReportContent.DisjunctiveMandatoryConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.DisjunctiveMandatory];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}
			// Simple Mandatory Constraints
			if ((myReportContent & VerbalizationReportContent.SimpleMandatoryConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.SimpleMandatory];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}
			// Ring Constraints
			if ((myReportContent & VerbalizationReportContent.RingConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.Ring];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
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
						IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, constraintList[i] as IVerbalize);
						if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
						{
							retVal = false;
						}
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
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}
			// Exclusion Constraints
			if ((myReportContent & VerbalizationReportContent.ExclusionConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.Exclusion];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}
			// Subset Constraints
			if ((myReportContent & VerbalizationReportContent.SubsetConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.Subset];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.GenericConstraintListItemOpen, ReportVerbalizationSnippetType.GenericConstraintListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericConstraintListClose));
			return retVal;
		}
		#endregion // IVerbalize Implementation
	}
	#endregion // GenericConstraintSection class
	#region GenericSnippetVerbalizer class
	/// <summary>
	/// Wraps any given Snippet for verbalization
	/// </summary>
	public class GenericSnippetVerbalizer : IVerbalize
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
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
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
	public class GenericSuperTypeSection : IVerbalize
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
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSuperTypeListOpen));

			bool hasContent = false;
			foreach (ObjectType objectType in myObjectType.SupertypeCollection)
			{
				IVerbalize instance = new ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeRelationshipValueLink, objectType);
				if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
				{
					retVal = false;
				}
				hasContent = true;
			}
			if (!hasContent)
			{
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.EmptyContentListItemSnippet));
			}

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSuperTypeListClose));
			return retVal;
		}
		#endregion // IVerbalize Implementation
	}
	#endregion // GenericSuperTypeSection class
	#region GenericSubTypeSection class
	/// <summary>
	/// Represents the Sub Type section of a Verbalization Report Page
	/// </summary>
	public class GenericSubTypeSection : IVerbalize
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
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSubTypeListOpen));

			bool hasContent = false;
			foreach (ObjectType objectType in myObjectType.SubtypeCollection)
			{
				IVerbalize instance = new ObjectTypeVerbalizationWrapper(ReportVerbalizationSnippetType.ObjectTypeRelationshipValueLink, objectType);
				if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
				{
					retVal = false;
				}
				hasContent = true;
			}
			if (!hasContent)
			{
				writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.EmptyContentListItemSnippet));
			}

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericSubTypeListClose));
			return retVal;
		}
		#endregion // IVerbalize Implementation
	}
	#endregion // GenericSubTypeSection class
	#region FactTypeConstraintValidationListReport class
	/// <summary>
	/// Represents the FactType/Constraint Validation List
	/// </summary>
	public class FactTypeConstraintValidationListReport : IVerbalizeCustomChildren, IVerbalizeFilterChildren
	{
		#region Member Variables
		private IList<FactType> myFactTypeList;
		private IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets;
		private VerbalizationReportContent myReportContent;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Initializes a new instance of FactTypeConstraintValidationListReport
		/// </summary>
		/// <param name="factTypeList">The fact type list.</param>
		/// <param name="reportContent">Content of the report.</param>
		/// <param name="snippets">The snippets.</param>
		public FactTypeConstraintValidationListReport(IList<FactType> factTypeList, VerbalizationReportContent reportContent, IVerbalizationSets<ReportVerbalizationSnippetType> snippets)
		{
			myFactTypeList = factTypeList;
			mySnippets = snippets;
			myReportContent = reportContent;
		}
		#endregion // Constructor
		#region IVerbalizeCustomChildren Implementation
		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			return GetCustomChildVerbalizations(filter, isNegative);
		}
		/// <summary>
		/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			yield return new CustomChildVerbalizer(new VerbalizationReportTableOfContentsWrapper(myReportContent, mySnippets));
			yield return new CustomChildVerbalizer(new GenericSnippetVerbalizer(ReportVerbalizationSnippetType.FactTypeConstraintValidationHeader));
			int factTypeCount = myFactTypeList.Count;
			for (int i = 0; i < factTypeCount; ++i)
			{
				yield return new CustomChildVerbalizer(new FactTypePageHeaderSummary(myFactTypeList[i], filter));
				yield return new CustomChildVerbalizer(new FactTypePageObjectTypeSection(ReportVerbalizationSnippetType.ObjectTypeListObjectTypeValueLink, myFactTypeList[i]));
				yield return new CustomChildVerbalizer(new ConstraintValidationSection(myReportContent, VerbalizationHelper.GetConstraintsFromFactType(myFactTypeList[i], myReportContent)));
			}
			yield return new CustomChildVerbalizer(new GenericSnippetVerbalizer(ReportVerbalizationSnippetType.FactTypeConstraintValidationSignature));
		}
		#endregion // IVerbalizeCustomChildren Implementation
		#region IVerbalizeFilterChildren Implementation
		CustomChildVerbalizer IVerbalizeFilterChildren.FilterChildVerbalizer(object child, bool isNegative)
		{
			return FilterChildVerbalizer(child, isNegative);
		}
		/// <summary>
		/// Provides an opportunity for a parent object to filter the
		/// verbalization of aggregated child verbalization implementations
		/// </summary>
		/// <param name="child">A direct or indirect child object.</param>
		/// <param name="isNegative">true if a negative verbalization is being requested</param>
		/// <returns>
		/// Return the provided childVerbalizer to verbalize normally, null to block verbalization, or an
		/// alternate IVerbalize. The value is returned with a boolean option. The element will be disposed with
		/// this is true.
		/// </returns>
		protected CustomChildVerbalizer FilterChildVerbalizer(object child, bool isNegative)
		{
			if (child is IConstraint)
			{
				return CustomChildVerbalizer.Block;
			}
			return new CustomChildVerbalizer(child as IVerbalize);
		}
		#endregion // IVerbalizeFilterChildren Implementation
	}

	#endregion // FactTypeConstraintValidationListReport class
	#region ConstraintValidationSection class
	/// <summary>
	/// Represents the Validation Constraint section of the Verbalization Report
	/// </summary>
	public class ConstraintValidationSection : IVerbalize
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
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		/// <param name="writer">The output text writer</param>
		/// <param name="snippetsDictionary">The IVerbalizationSets to use</param>
		/// <param name="verbalizationContext">A set callback function to interact with the outer verbalization context</param>
		/// <param name="isNegative">true for a negative reading</param>
		/// <returns>
		/// true to continue with child verbalization, otherwise false
		/// </returns>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			bool retVal = true;
			IVerbalizationSets<ReportVerbalizationSnippetType> mySnippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericConstraintListOpen));

			// Internal Uniqueness Constraints
			if ((myReportContent & VerbalizationReportContent.InternalUniquenessConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.InternalUniqueness];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}
			// External Uniqueness Constraints
			if ((myReportContent & VerbalizationReportContent.ExternalUniquenessConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.ExternalUniqueness];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}
			// Disjunctive Mandatory Constraints
			if ((myReportContent & VerbalizationReportContent.DisjunctiveMandatoryConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.DisjunctiveMandatory];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}
			// Simple Mandatory Constraints
			if ((myReportContent & VerbalizationReportContent.SimpleMandatoryConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.SimpleMandatory];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}
			// Ring Constraints
			if ((myReportContent & VerbalizationReportContent.RingConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.Ring];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
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
						IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, constraintList[i] as IVerbalize);
						if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
						{
							retVal = false;
						}
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
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}
			// Exclusion Constraints
			if ((myReportContent & VerbalizationReportContent.ExclusionConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.Exclusion];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}
			// Subset Constraints
			if ((myReportContent & VerbalizationReportContent.SubsetConstraints) != 0)
			{
				IList<IConstraint> constraintList = myConstraintList[ConstraintType.Subset];
				int constraintCount = constraintList.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IVerbalize instance = new ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemOpen, ReportVerbalizationSnippetType.FactTypeConstraintValidationListItemClose, constraintList[i] as IVerbalize);
					if (instance != null && !instance.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative))
					{
						retVal = false;
					}
				}
			}

			writer.Write(mySnippets.GetSnippet(ReportVerbalizationSnippetType.GenericConstraintListClose));
			return retVal;
		}
		#endregion // IVerbalize Implementation
	}
	#endregion // ConstraintValidationSection class
	#region ConstraintVerbalizationWrapper class
	/// <summary>
	/// Provides a wrapper for verbalizing Constraints
	/// </summary>
	public class ConstraintVerbalizationWrapper : IVerbalize
	{
		#region Member Variables
		private IVerbalize myVerbalizationObject;
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
		public ConstraintVerbalizationWrapper(ReportVerbalizationSnippetType openingSnippet, ReportVerbalizationSnippetType closingSnippet, IVerbalize verbalizationObject)
		{
			myOpeningSnippet = openingSnippet;
			myClosingSnippet = closingSnippet;
			myVerbalizationObject = verbalizationObject;
		}
		#endregion // Constructor
		#region IVerbalize Implementation
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		/// <summary>
		/// Verbalize in the requested form
		/// </summary>
		/// <param name="writer">The output text writer</param>
		/// <param name="snippetsDictionary">The IVerbalizationSets to use</param>
		/// <param name="verbalizationContext">A set callback function to interact with the outer verbalization context</param>
		/// <param name="isNegative">true for a negative reading</param>
		/// <returns>
		/// true to continue with child verbalization, otherwise false
		/// </returns>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			IVerbalizationSets<ReportVerbalizationSnippetType> snippets = (IVerbalizationSets<ReportVerbalizationSnippetType>)snippetsDictionary[typeof(ReportVerbalizationSnippetType)];
			writer.Write(snippets.GetSnippet(myOpeningSnippet), (myVerbalizationObject as ORMNamedElement).Name, (myVerbalizationObject as IConstraint).ConstraintType.ToString());
			verbalizationContext.DeferVerbalization(myVerbalizationObject, null);
			writer.Write(snippets.GetSnippet(myClosingSnippet));
			return true;
		}
		#endregion // IVerbalize Implementation
	}
	#endregion // ConstraintVerbalizationWrapper class
	#endregion // Shared Reports
}
