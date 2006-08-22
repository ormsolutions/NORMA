using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.Modeling.Diagrams.Design;

namespace Neumont.Tools.ORM.Views.RelationalView
{
	internal partial class ForeignKeyConnector
	{
		/// <summary>
		/// Overridden to disallow selection of this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ForeignKeyConnector"/>.
		/// </summary>
		public override bool CanSelect
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Overridden to disallow manual routing of this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ForeignKeyConnector"/>.
		/// </summary>
		/// <remarks>
		/// If this returns <see langword="true"/> while the <see cref="P:Neumont.Tools.ORM.Views.RelationalView.ForeignKeyConnector.CanSelect"/>
		/// property returns <see langword="false"/>, the application will crash while trying to manually route the connector.
		/// </remarks>
		public override bool CanManuallyRoute
		{
			get
			{
				return false;
			}
		}
	}
}
