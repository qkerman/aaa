using System;

namespace EVA
{
	/// <summary>
    /// This enumeration helps to manages the Axes (axis).
    /// </summary>
	public enum Axes
	{
        /// <summary>
        /// Represents all the axis, at the same time, only for scaling <see cref="EVA.ManipulationMode"/></see>. 
        /// </summary>
        ALL = 0,
        
        /// <summary>
        /// Represents the X Axis.
        /// </summary>
        X = 1,
        
		/// <summary>
        /// Represents the Y Axis.
        /// </summary>
		Y = 2,
        
		/// <summary>
        /// Represents the Z Axis.
        /// </summary>
		Z = 3,

        /// <summary>
        /// Represents the NONE Axis.
        /// </summary>
        NONE = 4
        

    }
}
