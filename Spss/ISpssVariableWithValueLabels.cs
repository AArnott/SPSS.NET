using System;

namespace Spss
{
	/// <summary>
	/// Interface that indicates that a certain class of SpssVariable has value labels.
	/// </summary>
	public interface ISpssVariableWithValueLabels
	{
		/// <summary>
		/// A generic dictionary of value labels.
		/// </summary>
		SpssVariableValueLabelsCollection ValueLabels { get; }
	}
}
