using Microsoft.VisualStudio.Modeling;
using System;
using System.Collections.Generic;
namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	/// <summary>
	/// Specifies the order that <see cref="IHierarchyContextEnabled"/> elements should
	/// be placed on the context diagram.
	/// </summary>
	public enum HierarchyContextPlacementPriority
	{
		/// <summary>
		/// Highest
		/// </summary>
		Highest = 100,
		/// <summary>
		/// VeryHigh
		/// </summary>
		VeryHigh = 90, // Entity types
		/// <summary>
		/// Higher
		/// </summary>
		Higher = 80, // Objectified Fact Types
		/// <summary>
		/// High
		/// </summary>
		High = 70, // Value types
		/// <summary>
		/// MediumHigh
		/// </summary>
		MediumHigh = 60,
		/// <summary>
		/// Medium
		/// </summary>
		Medium = 50, // Fact types
		/// <summary>
		/// MediumLow
		/// </summary>
		MediumLow = 40,
		/// <summary>
		/// Low
		/// </summary>
		Low = 30, // roles
		/// <summary>
		/// Lower
		/// </summary>
		Lower = 20,
		/// <summary>
		/// VeryLow
		/// </summary>
		VeryLow = 10, // Constraints
		/// <summary>
		/// Lowest
		/// </summary>
		Lowest = 0,
	}
	/// <summary>
	/// Defines the methods and properties required for an object to display in the ORMContextWindow
	/// </summary>
	public interface IHierarchyContextEnabled
	{
		/// <summary>
		/// Gets the model that the current <see cref="T:ORMModel"/> that the <see cref="T:ModelElement"/> is related to.
		/// </summary>
		/// <value>The model.</value>
		ORMModel Model { get;}
		/// <summary>
		/// Gets the current object GUID.
		/// </summary>
		/// <value>The id.</value>
		Guid Id { get;}
		/// <summary>
		/// Gets the number of generations to decriment when this object is walked.
		/// </summary>
		/// <value>The number of generations.</value>
		int HierarchyContextDecrementCount { get;}
		/// <summary>
		/// Gets a value indicating whether the path through the diagram should be followed through 
		/// this element.
		/// </summary>
		/// <value><c>true</c> to continue walking; otherwise, <c>false</c>.</value>
		bool ContinueWalkingHierarchyContext { get;}
		/// <summary>
		/// Gets the contextable object that this instance should resolve to.
		/// </summary>
		/// <remarks>For example a role should resolve to a fact type since a role is displayed with a fact type</remarks>
		/// <value>The forward context. Null if none</value>
		IHierarchyContextEnabled ForwardHierarchyContextTo { get;}
		/// <summary>
		/// Gets the elements that the current instance is dependant on for display.
		/// The returned elements will be forced to display in the context window.
		/// </summary>
		/// <param name="minimalElements">The final elements have been retrieved,
		/// retrieve the minimal number of elements for display.</param>
		/// <returns>Elements that always need to be shown with this element</returns>
		IEnumerable<IHierarchyContextEnabled> GetForcedHierarchyContextElements(bool minimalElements);
		/// <summary>
		/// Gets the place priority. The place priority specifies the order in which the element will
		/// be placed on the context diagram.
		/// </summary>
		/// <value>The place priority.</value>
		HierarchyContextPlacementPriority HierarchyContextPlacementPriority { get;}
	}
	/// <summary>
	/// Implement this interface on an <see cref="ElementLink"/> connected to elements that
	/// implement <see cref="IHierarchyContextEnabled"/> to limit hierarchy navigation across
	/// a link.
	/// </summary>
	public interface IHierarchyContextLinkFilter
	{
		/// <summary>
		/// Determine if hierarchy navigation should continue from a given element.
		/// </summary>
		/// <param name="fromRoleInfo">The domain role the navigation is coming from.</param>
		/// <returns><see langword="true"/> to continue navigation.</returns>
		bool ContinueHierachyWalking(DomainRoleInfo fromRoleInfo);
	}
}
