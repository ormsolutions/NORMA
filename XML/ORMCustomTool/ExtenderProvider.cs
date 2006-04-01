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
using System;
using System.Runtime.InteropServices;
using EnvDTE;
using VSLangProj;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using MSOLE=Microsoft.VisualStudio.OLE.Interop;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	/// <summary>
	/// Extender Provider object. This is the "class factory" for the Extender and is registered
	/// under the CATID for document objects (C#,VB, and J# documents)it is extending.
	/// See SolutionExtender.reg for the registration entries.
	/// </summary>
	[CLSCompliant(false)]
	[Guid("6FDCC073-20C2-4435-9B2E-9E70451C81D8")]
	public class ExtenderProvider : IExtenderProvider
	{
		/// <summary>
		/// Constructor does nothing special.
		/// </summary>
		public ExtenderProvider()
		{
		}

		/// <summary>
		/// Implementation of IExtenderProvider::CanExtend.
		/// </summary>
		/// <param name="ExtenderCATID">CATID of the object being extended.</param>
		/// <param name="ExtenderName">Name of the Extension.</param>
		/// <param name="ExtendeeObject">Object being extended.</param>
		/// <returns>true if can provide an extender for Extendee Object, false otherwise.</returns>
		public bool CanExtend(string ExtenderCATID, string ExtenderName, object ExtendeeObject)
		{
			FileProperties properties = ExtendeeObject as FileProperties;
			if (properties != null &&
				0 == string.Compare(ExtenderName, ORMCustomToolExtension, StringComparison.InvariantCultureIgnoreCase))
			{
				return 0 == string.Compare(properties.CustomTool, "ORMCustomTool", StringComparison.InvariantCultureIgnoreCase);
			}
			return false;
		}

		/// <summary>
		/// Implementation of IExtenderProvider::GetExtender.
		/// </summary>
		/// <param name="ExtenderCATID">CATID of the object being extended.</param>
		/// <param name="ExtenderName">Name of the Extension.</param>
		/// <param name="ExtendeeObject">Object being extended.</param>
		/// <param name="ExtenderSite">Site object for the Extender.</param>
		/// <param name="Cookie">Cookie value that identifies the Extender to its Site.</param>
		/// <returns>A newly created Extender object.</returns>
		public object GetExtender(string ExtenderCATID, string ExtenderName, object ExtendeeObject, EnvDTE.IExtenderSite ExtenderSite, int Cookie)
		{
			Extender retVal = new Extender();
			retVal.Init(Cookie, ExtenderSite);
			return retVal;
		}

		/// <summary>
		/// The Extension name for this Extender.
		/// </summary>
		private string ORMCustomToolExtension = "ORMCustomTool";
	}
}
	
