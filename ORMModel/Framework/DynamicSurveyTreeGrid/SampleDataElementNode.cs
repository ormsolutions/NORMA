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
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid
{
	/// <summary>
	/// wrapper for objects to be dispalyed in the Survey Tree
	/// </summary>
	public struct SampleDataElementNode
	{
		/// <summary>
		/// public constructor
		/// </summary>
		/// <param name="element"></param>
		public SampleDataElementNode(object element)
		{
			this.myElement = element;
			this.myName = element as ISurveyName;
			this.nodeData = 0;
			this.myIndex = 0;
		}
		/// <summary>
		/// public constructor
		/// </summary>
		/// <param name="element"></param>
		/// <param name="index"></param>
		public SampleDataElementNode(object element, int index)
		{
			this.myIndex = index;
			this.myElement = element;
			this.myName = element as ISurveyName;
			this.nodeData = 0;
		}
		private object myElement;
		private ISurveyName myName;
		/// <summary>
		/// returns object wrapped by this node
		/// </summary>
		public object SampleDataElement
		{
			get { return myElement; }
		}
		int myIndex;
		/// <summary>
		/// returns nodes index in container, must be set by container to work
		/// </summary>
		public int Index
		{
			get 
			{ 
				return myIndex; 
			}
			set 
			{
				myIndex = value;
			}
		}
		/// <summary>
		/// the object this SampleDataElementNode wraps
		/// </summary>
		public Object Element
		{
			get
			{
				return myElement;
			}
		}
		private int nodeData;
		/// <summary>
		/// gets or sets the integer that holds the answers to all questions in myNodeCachedQuestions
		/// </summary>
		public int NodeData
		{
			get { return nodeData; }
			set { nodeData = value; }
		}
		#region ISurveyName property wrappers
		/// <summary>
		/// returns name of the wrapped element
		/// </summary>
		public string ElementName
		{
			get
			{
				if (myName != null)
				{
					return myName.SurveyName;
				}
				return myElement.ToString();
			}
		}
		/// <summary>
		/// for edit mode returns the name the user can change
		/// </summary>
		public string EditName
		{
			get
			{
				return myName.EditableSurveyName;
			}
			set
			{
				myName.EditableSurveyName = value;
			}
		}
		/// <summary>
		/// returns whether or not this object can be edited.
		/// </summary>
		public bool IsEditable
		{
			get
			{
				return myName.IsEditable;
			}
		}
		#endregion
	}
}
