using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Neumont.Tools.ORM.SDK.TestEngine
{
	#region TestsAttribute class
	/// <summary>
	/// An attribute to apply to a test class to signal the test engine
	/// to look for methods with a Test attribute in that class. The class
	/// must have a constructor with a single parameter of type
	/// Neumont.Tools.ORM.ObjectModel.IORMToolServices.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public sealed class ORMTestFixtureAttribute : Attribute
	{
		/// <summary>
		/// This class contains methods with the Test attribute
		/// </summary>
		public ORMTestFixtureAttribute() { }
	}
	#endregion // TestsAttribute class
	#region TestAttribute class
	/// <summary>
	/// An attribute to indicate that the attributed method is a test
	/// method. A test method must take a single parameter of type
	/// Microsoft.VisualStudio.Modeling.Store
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class ORMTestAttribute : Attribute
	{
		private string[] myCategories;

		/// <summary>
		/// This method is run as a test
		/// </summary>
		public ORMTestAttribute()
		{
		}
		/// <summary>
		/// This method is run as a test with specified categories
		/// </summary>
		/// <param name="categories">A list of categories</param>
		public ORMTestAttribute(params string[] categories)
		{
			ProcessCandidateCategories(categories);
		}
		/// <summary>
		/// The test supports categories with the specified names. Categories
		/// are used in a suite file to filter which tests are run. The Categories
		/// property can specify one or more categories in a delimited list. The
		/// recognized delimiters are ',;|'. Whitespace characters are not considered
		/// delimiters, but leading and trailing whitespace is removed from any name.
		/// Category comparisons are case insensitive.
		/// </summary>
		public string Categories
		{
			get
			{
				return null;
			}
			set
			{
				string[] candidates = null;
				if (value.Length != 0)
				{
					candidates = value.Split(new char[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
				}
				ProcessCandidateCategories(candidates);
			}
		}
		/// <summary>
		/// Helper function, share code with constructor and Categories property
		/// </summary>
		/// <param name="candidates">Candidate categories</param>
		private void ProcessCandidateCategories(string[] candidates)
		{
			int candidateCount;
			if (candidates == null || (0 == (candidateCount = candidates.Length)))
			{
				myCategories = null;
				return;
			}
			string[] categories = null;
			int totalCount = 0;
			for (int i = 0; i < candidateCount; ++i)
			{
				string candidate = candidates[i];
				if (candidate != null)
				{
					candidate = candidate.Trim();
					if (candidate.Length != 0)
					{
						++totalCount;
						candidates[i] = candidate;
					}
					else
					{
						candidates[i] = null;
					}
				}
			}
			if (totalCount != 0)
			{
				if (totalCount == candidateCount)
				{
					categories = candidates;
				}
				else
				{
					categories = new string[totalCount];
					int nextCategory = 0;
					for (int i = 0; i < candidateCount; ++i)
					{
						string candidate = candidates[i];
						if (candidate != null)
						{
							categories[nextCategory] = candidate;
							++nextCategory;
						}
					}
				}
				if (totalCount > 1)
				{
					Array.Sort<string>(categories, StringComparer.InvariantCultureIgnoreCase);
				}
			}
			myCategories = categories;
		}
		/// <summary>
		/// Test if the specified category is supported by this attribute instance
		/// </summary>
		/// <param name="category">A category value. Categories are case insensitive</param>
		/// <returns>true if a Categories property was specified with this value</returns>
		public bool SupportsCategory(string category)
		{
			string[] categories = myCategories;
			if (categories != null)
			{
				category = category.Trim();
				if (categories.Length == 1)
				{
					return categories[0].Equals(category, StringComparison.InvariantCultureIgnoreCase);
				}
				else
				{
					return 0 <= Array.BinarySearch<string>(categories, category, StringComparer.InvariantCultureIgnoreCase);
				}
			}
			return false;
		}
	}
	#endregion // TestAttribute class
}
