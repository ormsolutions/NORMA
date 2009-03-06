#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
#endregion

namespace ORMSolutions.ORMArchitectSDK.TestEngine
{
	public partial struct Suite
	{
		private class ORMTaskItem : IORMToolTaskItem
		{
			#region Member variables and constructors
			IRepresentModelElements myElementLocator;
			IORMToolTaskProvider myOwner;
			private string myText;

			private ORMTaskItem()
			{
			}
			/// <summary>
			/// Create a task item for the specified owning provider
			/// </summary>
			/// <param name="owner">IORMToolTaskProvider</param>
			public ORMTaskItem(IORMToolTaskProvider owner)
			{
				myOwner = owner;
			}
			#endregion // Member variables and constructors
			#region IORMToolTaskItem Members
			/// <summary>
			/// Implements IORMToolTaskItem.ElementLocator property
			/// </summary>
			protected IRepresentModelElements ElementLocator
			{
				get { return myElementLocator; }
				set { myElementLocator = value; }
			}
			IRepresentModelElements IORMToolTaskItem.ElementLocator
			{
				get { return ElementLocator; }
				set { ElementLocator = value; }
			}
			/// <summary>
			/// Implements IORMToolTaskItem.Text property
			/// </summary>
			protected string Text
			{
				get { return myText; }
				set
				{
					//Don't trigger task list change unless needed
					string oldText = myText;
					if (oldText != value)
					{
						myText = value;
					}
				}
			}
			string IORMToolTaskItem.Text
			{
				get { return Text; }
				set { Text = value; }
			}
			#endregion
		}
	}
}
