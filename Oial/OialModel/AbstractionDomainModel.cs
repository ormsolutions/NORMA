using ORMSolutions.ORMArchitect.Framework;

using System.Collections.Generic;
using System;
using Microsoft.VisualStudio.Modeling;

namespace ORMSolutions.ORMArchitect.ORMAbstraction
{
	/// <summary>
	/// Implement on a domain model to receive notifications during deserialization that
	/// the serialized abstraction model is about to be rebuilt.
	/// </summary>
	public interface IAbstractionModelRebuilding
	{
		/// <summary>
		/// An abstraction model is about to be rebuilt. Any information related to the current model needs to be cached.
		/// </summary>
		void AbstractionModelRebuilding(AbstractionModel abstractionModel);
	}

	[ORMSolutions.ORMArchitect.Core.Load.NORMAExtensionLoadKey(
#if NORMA_Official
		"k48sSChL5ZRCD/EGcHvmdJLYNIzgenJV1xsoMJ6t0ppy6Jp5fC0vOwHoMHHngtk45eZdubiWUVELJr7X4inoCw=="
#else
		"BHrWfIvZuqiu8p3DyWS4SXbXII/btuG+AZKDfkrckD+6rUOs8IG28aZBGkIWynP3Vq2VBj6cf/Lf7rXzrgGctg=="
#endif
	)]
	partial class AbstractionDomainModel
	{
		/// <summary>
		/// AddRule: typeof(AbstractionModelHasInformationTypeFormat)
		/// The per-model-singleton links to the positive and unary formats can be directly
		/// deduced from the broader collection. This is on during load so a correspoinding
		/// fixup listener is not needed.
		/// </summary>
		private static void EnsureUnaryFormat(ElementAddedEventArgs e)
		{
			AbstractionModelHasInformationTypeFormat link = (AbstractionModelHasInformationTypeFormat)e.ModelElement;
			InformationTypeFormat format = link.InformationTypeFormat;
			if (format.GetType() != typeof(InformationTypeFormat))
			{
				PositiveUnaryInformationTypeFormat positiveFormat;
				NegativeUnaryInformationTypeFormat negativeFormat;
				if (null != (positiveFormat = format as PositiveUnaryInformationTypeFormat))
				{
					link.Model.PositiveUnaryInformationTypeFormat = positiveFormat;
				}
				else if (null != (negativeFormat = format as NegativeUnaryInformationTypeFormat))
				{
					link.Model.NegativeUnaryInformationTypeFormat = negativeFormat;
				}
			}
		}
	}
}
