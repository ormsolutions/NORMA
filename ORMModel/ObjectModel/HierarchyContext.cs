using Microsoft.VisualStudio.Modeling;
using System;
using System.Collections.Generic;
namespace Neumont.Tools.ORM.ObjectModel
{
	/// <summary>
	/// Specifies the order that <see cref="IHierarchyContextEnabled"/> elements should
	/// be placed on the context diagram.
	/// </summary>
	public enum HierarchyContextPlacementPriority
	{
		/// <summary>
		/// VeryHigh
		/// </summary>
		VeryHigh = 100, // Object types
		/// <summary>
		/// High
		/// </summary>
		High = 75, // Objectified Fact Types
		/// <summary>
		/// Medium
		/// </summary>
		Medium = 50, // Fact types
		/// <summary>
		/// Low
		/// </summary>
		Low = 25, // roles
		/// <summary>
		/// VeryLow
		/// </summary>
		VeryLow = 0, // Constraints
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
		/// <value>The dependant context elements.</value>
		IEnumerable<IHierarchyContextEnabled> ForcedHierarchyContextElementCollection { get;}
		/// <summary>
		/// Gets the place priority. The place priority specifies the order in which the element will
		/// be placed on the context diagram.
		/// </summary>
		/// <value>The place priority.</value>
		HierarchyContextPlacementPriority HierarchyContextPlacementPriority { get;}
	}
}
