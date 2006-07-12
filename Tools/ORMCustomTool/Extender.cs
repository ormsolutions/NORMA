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
using EnvDTE;
using VSLangProj;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing.Design;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	/// <summary>
	/// The interface for the Extender object. It implements one property ORMGeneratorSettings:
	/// </summary>
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IORMCustomToolExtender
	{	
		/// <summary>
		/// ORMGenerator Settings. Placeholder property to launch the property descriptor
		/// </summary>
		string ORMGeneratorSettings
		{
			get;
		}
	};
	/// <summary>
	/// This is the Extender Object itself.
	/// </summary>
	[CLSCompliant(false)]
	[ClassInterface(ClassInterfaceType.None)]
	public partial class Extender : IORMCustomToolExtender
	{
		/// <summary>
		/// Notes property stores its value in the Solution.Globals object and hence 
		/// is persisted with the solution.
		/// </summary>
		[Editor(typeof(ORMCustomToolPropertyDescriptor.ORMCustomToolUITypeEditor), typeof(UITypeEditor))]
		[MergableProperty(false)]
		public string ORMGeneratorSettings
		{
			get
			{
				return "";
			}
		}		

		/// <summary>
		/// Constructor does nothing. All initialization work is done in Init.
		/// </summary>
		public Extender()
		{
		}

		/// <summary>
		/// Initializes the members of the SolnExtender class.
		/// </summary>
		/// <param name="ExtenderCookie">Cookie value that identifies the Extender to its Site.</param>
		/// <param name="ExtenderSite">Site object for the Extender.</param>
		public void Init(int ExtenderCookie, EnvDTE.IExtenderSite ExtenderSite)
		{
			mySite = ExtenderSite;
			myCookie = ExtenderCookie;
		}

		/// <summary>
		/// Tells the Site object we are going away.
		/// </summary>
		~Extender()
		{
			// Wrap this call in a try-catch to avoid any failure code the
			// Site may return. For instance, since this object is GC'ed,
			// the Site may have already let go of its Cookie.
			try
			{
				if (mySite != null)
				{
					mySite.NotifyDelete(myCookie);
				}
			}
			catch 
			{
			}
		}

		//Data members of the class.
		private int myCookie;
		private EnvDTE.IExtenderSite mySite;
	}
}
