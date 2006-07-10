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
using System.Diagnostics;
using System.Resources;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Design;
using Neumont.Tools.ORM.Shell;

namespace Neumont.Tools.ORM.ShapeModel
{
	/// <summary>
	/// Pseudo-<see cref="DomainModel"/> for ORM <see cref="ShapeElement"/>s.
	/// </summary>
	/// <remarks>
	/// Most of this class just delegates to <see cref="ORMMetaModel"/>.
	/// Its existence is mostly just a sad reminder of the "good old days" when
	/// one <see cref="DomainModel"/> could actually reference another. But they
	/// weren't called "<see cref="DomainModel"/>s" back then, they were called
	/// "MetaModels" or "SubStores", and you had to write them on punch cards…
	/// </remarks>
	[DomainObjectId("C52FB9A5-6BF4-4267-8716-71D74C7AA89C")]
	public partial class ORMShapeModel : DomainModel
	{
		/// <summary><see cref="ORMShapeModel"/> domain model Id.</summary>
		public static readonly Guid DomainModelId = new Guid(0xC52FB9A5, 0x6BF4, 0x4267, 0x87, 0x16, 0x71, 0xD7, 0x4C, 0x7A, 0xA8, 0x9C);

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="store"><see cref="Store"/> containing the <see cref="DomainModel"/>.</param>
		public ORMShapeModel(Store store)
			: base(store, DomainModelId)
		{
		}

		#region Resource manager
		/// <summary>
		/// The base name of this model's resources.
		/// </summary>
		public const string ResourceBaseName = ORMMetaModel.ResourceBaseName;

		/// <summary>
		/// Gets the <see cref="DomainModel"/>'s <see cref="ResourceManager"/>.
		/// If the <see cref="ResourceManager"/> does not already exist, then it is created.
		/// </summary>
		public override ResourceManager ResourceManager
		{
			[DebuggerStepThrough]
			get
			{
				return ORMMetaModel.SingletonResourceManager;
			}
		}

		/// <summary>
		/// Gets the Singleton <see cref="ResourceManager"/> for this <see cref="DomainModel"/>.
		/// </summary>
		public static ResourceManager SingletonResourceManager
		{
			[DebuggerStepThrough]
			get
			{
				return ORMMetaModel.SingletonResourceManager;
			}
		}
		#endregion // Resource manager

		#region Diagram rule helpers
		/// <summary>
		/// Enables <see cref="Rule"/>s in this <see cref="DomainModel"/> related to
		/// <see cref="Diagram"/> fixup for the <see cref="Store"/> specified by
		/// <paramref name="store"/>. If <see cref="Diagram"/> data will be loaded
		/// into the <see cref="Store"/>, this method should be called first to ensure
		/// that the <see cref="Diagram"/> behaves properly.
		/// </summary>
		public static void EnableDiagramRules(Store store)
		{
			ORMMetaModel.EnableDiagramRules(store);
		}

		/// <summary>
		/// Disables <see cref="Rule"/>s in this <see cref="DomainModel"/> related to
		/// <see cref="Diagram"/> fixup for the <see cref="Store"/> specified by
		/// <paramref name="store"/>.
		/// </summary>
		public static void DisableDiagramRules(Store store)
		{
			ORMMetaModel.DisableDiagramRules(store);
		}
		#endregion // Diagram rule helpers

		#region Copy/Delete closures
		/// <summary>
		/// Returns an <see cref="IElementVisitorFilter"/> that corresponds to the
		/// <see cref="ClosureType"/> specified by <paramref name="type"/>.
		/// </summary>
		public override IElementVisitorFilter GetClosureFilter(ClosureType type, ICollection<ModelElement> rootElements)
		{
			DomainModel domainModel = base.Store.FindDomainModel(ORMMetaModel.DomainModelId);
			if (domainModel != null)
			{
				return domainModel.GetClosureFilter(type, rootElements);
			}
			return base.GetClosureFilter(type, rootElements);
		}
		#endregion // Copy/Delete closures
	}
}
