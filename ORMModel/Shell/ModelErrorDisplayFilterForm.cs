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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	#region ModelErrorDisplayFilterForm class
	public partial class ModelErrorDisplayFilterForm : Form
	{
		#region Form fields and constructor
		private ORMModel myModel;
		private Category[] myCategories;
		private Error[] myErrors;
		/// <summary>
		/// Creates a new form to filter errors for the given ORM model.
		/// </summary>
		/// <param name="model">The orm model for which to filter errors.</param>
		public ModelErrorDisplayFilterForm(ORMModel model)
		{
			this.myModel = model;
			InitializeComponent();
			LoadCategories();

			ITree tree = new VirtualTree();
			IBranch rootBranch = new InitialCategoriesBranch(myCategories, myErrors);
			tree.Root = rootBranch;

#if VISUALSTUDIO_9_0 // MSBUG: Hack workaround crashing bug in VirtualTreeControl.OnToggleExpansion
			this.virtualTreeControl.ColumnPermutation = new ColumnPermutation(1, new int[]{0}, false);
#endif
			this.virtualTreeControl.Tree = tree;
		}
		#endregion //Form fields and constructor
		#region Load Categories
		private void LoadCategories()
		{
			ModelErrorDisplayFilter filter = myModel.ModelErrorDisplayFilter;

			DomainDataDirectory dataDirectory = myModel.Store.DomainDataDirectory;
			DomainClassInfo baseType = dataDirectory.GetDomainClass(typeof(ModelErrorCategory));
			ReadOnlyCollection<DomainClassInfo> categoryDescendants = baseType.AllDescendants;

			int numCategories = categoryDescendants.Count;
			Category[] categories = new Category[numCategories + 1];
			categories[numCategories] = new Category(null, false);
			for (int i = 0; i < numCategories; ++i)
			{
				DomainClassInfo info = categoryDescendants[i];
				Type categoryType = info.ImplementationClass;
				categories[i] = new Category(info, filter != null && filter.IsCategoryExcluded(categoryType));
			}

			// Presort the categories list
			Category.Sort(categories);

			baseType = dataDirectory.GetDomainClass(typeof(ModelError));
			ReadOnlyCollection<DomainClassInfo> errorDescendants = baseType.AllDescendants;
			int errorDescendantCount = errorDescendants.Count;
			int includedErrorCount = 0;

			for (int i = 0; i < errorDescendantCount; ++i)
			{
				if (!errorDescendants[i].ImplementationClass.IsAbstract)
				{
					++includedErrorCount;
				}
			}

			Error[] errors = new Error[includedErrorCount];
			int includedErrorIndex = -1;
			for (int i = 0; i < errorDescendantCount; ++i)
			{
				DomainClassInfo info = errorDescendants[i];
				Type error = info.ImplementationClass;
				if (!error.IsAbstract)
				{
					++includedErrorIndex;
					errors[includedErrorIndex] = new Error(
						info,
						Category.IndexOf(categories, ResolveErrorCategory(error)),
						filter != null && filter.IsErrorExcluded(error));
				}
			}

			// Sort the errors with a primary sort on the presorted category index
			Array.Sort(errors,
				delegate(Error error1, Error error2)
				{
					int retVal = 0;
					int categoryIndex1 = error1.CategoryIndex;
					int categoryIndex2 = error2.CategoryIndex;
					if (categoryIndex1 < categoryIndex2)
					{
						retVal = -1;
					}
					else if (categoryIndex1 > categoryIndex2)
					{
						retVal = 1;
					}
					else
					{
						retVal = string.Compare(error1.DisplayName, error2.DisplayName, StringComparison.CurrentCultureIgnoreCase);
					}
					return retVal;
				});

			// Walk the errors and bind the categories to the errors
			int currentCategoryStartErrorIndex = -1;
			int lastCategoryIndex = -1;
			for (int i = 0; i < includedErrorCount; ++i)
			{
				int currentIndex = errors[i].CategoryIndex;
				if (currentIndex != lastCategoryIndex)
				{
					if (lastCategoryIndex == -1)
					{
						lastCategoryIndex = currentIndex;
						currentCategoryStartErrorIndex = i;
					}
					else
					{
						categories[lastCategoryIndex].BindErrorRange(currentCategoryStartErrorIndex, i - 1);
						currentCategoryStartErrorIndex = i;
						lastCategoryIndex = currentIndex;
					}
				}
			}
			if (lastCategoryIndex != -1)
			{
				categories[lastCategoryIndex].BindErrorRange(currentCategoryStartErrorIndex, includedErrorCount - 1);
			}
			myCategories = categories;
			myErrors = errors;
		}
		private static Type ResolveErrorCategory(Type error)
		{
			object[] attributes = error.GetCustomAttributes(typeof(ModelErrorDisplayFilterAttribute), true);
			ModelErrorDisplayFilterAttribute attribute = null;
			for (int i = 0; i < attributes.Length && attribute == null; ++i)
			{
				attribute = attributes[i] as ModelErrorDisplayFilterAttribute;
			}

			if (attribute != null)
			{
				return attribute.Category;
			}
			return null;
		}
		#endregion //Load Categories
		#region Branches
		private class ErrorBranch : IBranch
		{
			private Category myCategory;
			private Error[] myErrors;
			public ErrorBranch(Category category, Error[] allErrors)
			{
				myCategory = category;
				myErrors = category.GetErrors(allErrors);
			}

			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				switch (style)
				{
					case ObjectStyle.TrackingObject:
						if (row < myErrors.Length)
						{
							return myErrors[row];
						}
						return null;
				}
				return null;
			}
			string IBranch.GetText(int row, int column)
			{
				if (row < myErrors.Length)
				{
					return myErrors[row].DisplayName;
				}
				return string.Empty;
			}

			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return VirtualTreeLabelEditData.Invalid;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return LabelEditResult.CancelEdit;
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return BranchFeatures.StateChanges;
				}
			}
			VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				if (row < myErrors.Length)
				{
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					retVal.StateImageIndex = (short)(myErrors[row].IsExcluded ? StandardCheckBoxImage.Unchecked : StandardCheckBoxImage.Checked);
					return retVal;
				}
				return VirtualTreeDisplayData.Empty;
			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				return null;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return false;
			}
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				return default(LocateObjectData);
			}
			event BranchModificationEventHandler IBranch.OnBranchModification
			{
				add { }
				remove { }
			}
			void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, System.Windows.Forms.DragEventArgs args)
			{
			}
			void IBranch.OnGiveFeedback(System.Windows.Forms.GiveFeedbackEventArgs args, int row, int column)
			{
			}
			void IBranch.OnQueryContinueDrag(System.Windows.Forms.QueryContinueDragEventArgs args, int row, int column)
			{
			}
			VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return VirtualTreeStartDragData.Empty;
			}
			StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}
			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				if (row < myErrors.Length)
				{
					myErrors[row].Toggle();
					return StateRefreshChanges.Parents;
				}
				return StateRefreshChanges.None;
			}

			int IBranch.UpdateCounter
			{
				get
				{
					return 0;
				}
			}

			int IBranch.VisibleItemCount
			{
				get
				{
					return myErrors.Length;
				}
			}
		}
		private class InitialCategoriesBranch : IBranch
		{
			private Category[] myCategories;
			private Error[] myErrors;
			public InitialCategoriesBranch(Category[] categories, Error[] errors)
			{
				myCategories = categories;
				myErrors = errors;
			}

			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				switch (style)
				{
					case ObjectStyle.ExpandedBranch:
					case ObjectStyle.TrackingObject:
						Category[] categories = myCategories;
						if (row < categories.Length)
						{
							if (style == ObjectStyle.ExpandedBranch)
							{
								return new ErrorBranch(categories[row], myErrors);
							}
							return categories[row];
						}
						break;
				}
				return null;
			}
			string IBranch.GetText(int row, int column)
			{
				Category[] categories = myCategories;
				if (row < categories.Length)
				{
					return categories[row].DisplayName;
				}
				return string.Empty;
			}

			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return VirtualTreeLabelEditData.Invalid;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return LabelEditResult.CancelEdit;
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return BranchFeatures.Expansions | BranchFeatures.StateChanges;
				}
			}
			VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				Category[] categories = myCategories;
				if (row < categories.Length)
				{
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					switch (categories[row].GetCheckState(myErrors))
					{
						case CheckState.Checked:
							retVal.StateImageIndex = (short)StandardCheckBoxImage.Checked;
							break;
						case CheckState.Indeterminate:
							retVal.StateImageIndex = (short)StandardCheckBoxImage.Indeterminate;
							break;
						case CheckState.Unchecked:
							retVal.StateImageIndex = (short)StandardCheckBoxImage.Unchecked;
							break;
						case CheckState.Disabled:
							retVal.StateImageIndex = (short)StandardCheckBoxImage.CheckedDisabled;
							break;
					}
					return retVal;

				}
				return VirtualTreeDisplayData.Empty;
			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				return null;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				Category[] categories = myCategories;
				if (row < categories.Length)
				{
					return categories[row].FirstError != -1;
				}
				return false;
			}
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				return default(LocateObjectData);
			}
			event BranchModificationEventHandler IBranch.OnBranchModification
			{
				add { }
				remove { }
			}
			void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, System.Windows.Forms.DragEventArgs args)
			{
			}
			void IBranch.OnGiveFeedback(System.Windows.Forms.GiveFeedbackEventArgs args, int row, int column)
			{
			}
			void IBranch.OnQueryContinueDrag(System.Windows.Forms.QueryContinueDragEventArgs args, int row, int column)
			{
			}
			VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return VirtualTreeStartDragData.Empty;
			}
			StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}
			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				Category[] categories = myCategories;
				if (row < categories.Length)
				{
					categories[row].Toggle(myErrors);
					return StateRefreshChanges.Children;
				}

				return StateRefreshChanges.None;
			}

			int IBranch.UpdateCounter
			{
				get
				{
					return 0;
				}
			}

			int IBranch.VisibleItemCount
			{
				get
				{
					Category[] categories = myCategories;
					int numCategories = categories.Length;
					int shortLength = numCategories - 1;
					if (categories[shortLength].FirstError == -1)
					{
						return shortLength;
					}
					return numCategories;
				}
			}
		}
		#endregion //Branches
		#region Inner Category and Error classes
		private enum CheckState
		{
			Checked,
			Unchecked,
			Indeterminate,
			Disabled,
		}
		private class Category
		{
			public static int IndexOf(Category[] categories, Type categoryType)
			{
				int categoryCount = categories.Length - 1;
				if (categoryType == null)
				{
					return categoryCount; // Keep this consistent with sort
				}
				for (int i = 0; i < categoryCount; ++i)
				{
					if (categoryType == categories[i].Type)
					{
						return i;
					}
				}
				return categoryCount; // Return untyped if we can't find it
			}
			public static void Sort(Category[] categories)
			{
				Array.Sort(
					categories,
					delegate(Category category1, Category category2)
					{
						Type categoryType2 = category2.Type;
						if (category1.Type == null)
						{
							return categoryType2 == null ? 0 : 1;
						}
						else if (categoryType2 == null)
						{
							return -1;
						}
						return string.Compare(category1.DisplayName, category2.DisplayName, StringComparison.CurrentCultureIgnoreCase);
					});
			}
			private int myFirstError;
			private int myLastError;
			public int FirstError
			{
				get { return myFirstError; }
			}
			public int LastError
			{
				get { return myLastError; }
			}
			public void BindErrorRange(int firstError, int lastError)
			{
				myFirstError = firstError;
				myLastError = lastError;
			}

			public CheckState GetCheckState(Error[] errors)
			{
				if (myFirstError == -1)
				{
					return CheckState.Disabled;
				}

				bool excluded = false;
				bool included = false;
				for (int i = myFirstError; i <= myLastError; ++i)
				{
					Error error = errors[i];

					if (!excluded)
					{
						if (error.IsExcluded)
						{
							excluded = true;
						}
					}
					if (!included)
					{
						if (!error.IsExcluded)
						{
							included = true;
						}
					}
					if (included && excluded)
					{
						break;
					}
				}

				if (excluded && !included)
				{
					return CheckState.Unchecked;
				}
				else if (included && !excluded)
				{
					return CheckState.Checked;
				}
				return CheckState.Indeterminate;
			}

			private bool myIsExcluded;
			public bool IsExcluded
			{
				get { return myIsExcluded; }
				set { myIsExcluded = value; }
			}
			private bool myWasExcluded;
			public bool WasExcluded
			{
				get { return myWasExcluded; }
			}

			private Type myType;
			public Type Type
			{
				get { return myType; }
			}

			private string myDisplayName;
			public string DisplayName
			{
				get { return myDisplayName; }
			}

			public Category(DomainClassInfo classInfo, bool excluded)
			{
				if (classInfo != null)
				{
					myType = classInfo.ImplementationClass;
					myDisplayName = classInfo.DisplayName;
				}
				else
				{
					myType = null;
					myDisplayName = ResourceStrings.ModelErrorUncategorized;
				}
				myIsExcluded = excluded;
				myWasExcluded = excluded;
				myFirstError = -1;
				myLastError = -1;
			}

			public void Toggle(Error[] allErrors)
			{
				myIsExcluded = !myIsExcluded;

				if (myFirstError != -1)
				{
					for (int i = myFirstError; i <= myLastError; ++i)
					{
						allErrors[i].Toggle(myIsExcluded);
					}
				}
			}

			public Error[] GetErrors(Error[] allErrors)
			{
				if (myFirstError == -1)
				{
					return new Error[0];
				}
				else
				{
					Error[] retVal = new Error[myLastError - myFirstError + 1];
					int counter = -1;
					for (int i = myFirstError; i <= myLastError; ++i)
					{
						retVal[++counter] = allErrors[i];
					}
					return retVal;
				}
			}
		}
		private class Error
		{
			private int myCategoryIndex;
			public int CategoryIndex
			{
				get
				{
					// Note that the category indices are presorted, so this
					// is stable over time.
					return myCategoryIndex;
				}
			}
			private bool myIsExcluded;
			public bool IsExcluded
			{
				get { return myIsExcluded; }
			}
			private bool myWasExcluded;
			public bool WasExcluded
			{
				get { return myWasExcluded; }
			}

			private Type myType;
			public Type Type
			{
				get { return myType; }
			}

			private string myDisplayName;
			public string DisplayName
			{
				get { return myDisplayName; }
			}

			public Error(DomainClassInfo classInfo, int categoryIndex, bool excluded)
			{
				myType = classInfo.ImplementationClass;
				myCategoryIndex = categoryIndex;
				myDisplayName = classInfo.DisplayName;

				myIsExcluded = excluded;
				myWasExcluded = excluded;
			}

			public void Toggle()
			{
				myIsExcluded = !myIsExcluded;
			}

			public void Toggle(bool excluded)
			{
				myIsExcluded = excluded;
			}
		}
		#endregion //Inner Category and Error classes
		#region Save Changes
		private void btnOK_Click(object sender, EventArgs e)
		{
			SaveChanges();
		}
		private void SaveChanges()
		{
			ORMModel model = myModel;
			ModelErrorDisplayFilter filter = model.ModelErrorDisplayFilter;
			Store store = model.Store;
			using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ModelErrorDisplayFilterChangeTransactionName))
			{
				bool deleteFilter = true, anyChanges = false;
				Error[] errors = myErrors;
				Category[] categories = myCategories;
				foreach (Category category in categories)
				{
					bool allExcluded = true, allIncluded = true;

					if (category.FirstError == -1)
					{
						allExcluded = false;
						allIncluded = true;
					}
					else
					{
						for (int i = category.FirstError; i <= category.LastError; ++i)
						{
							Error error = errors[i];
							if (error.IsExcluded != error.WasExcluded)
							{
								anyChanges = true;
							}
							if (error.IsExcluded)
							{
								deleteFilter = false;

								allIncluded = false;
							}
							else
							{
								allExcluded = false;
							}

							if (anyChanges && !deleteFilter && !allExcluded && !allIncluded)
							{
								break;
							}
						}
					}

					if (allIncluded)
					{
						category.IsExcluded = false;
					}
					else if (allExcluded)
					{
						category.IsExcluded = true;
					}
				}

				if (deleteFilter)
				{
					//if all errors are included, we can delete the filter
					anyChanges = true;
					if (filter != null)
					{
						filter.Delete();
					}
				}
				else if (anyChanges)
				{
					if (filter == null)
					{
						filter = new ModelErrorDisplayFilter(store);
						filter.Model = model;
					}

					foreach (Category category in categories)
					{
						if (category.Type != null)
						{
							filter.ToggleCategory(category.Type, category.IsExcluded);
						}
					}

					foreach (Error error in errors)
					{
						filter.ToggleError(error.Type, error.IsExcluded);
					}

					filter.CommitChanges();
				}
				if (anyChanges && t.HasPendingChanges)
				{
					t.Commit();
				}
			}
		}
		#endregion //Save Changes
	}
	#endregion //ModelErrorDisplayFilterForm class
}
