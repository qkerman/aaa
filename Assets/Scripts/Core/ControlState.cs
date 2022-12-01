using System;
namespace EVA
{
	/// <summary>
    /// This enumeration helps to know what type of object is actually selected.
    /// </summary>
	public enum ControlState
	{
		/// <summary>
        /// A trigger is selected.
        /// </summary>
		TriggerSelected,
		/// <summary>
        /// An artwork is selected.
        /// </summary>
		ArtworkSelected,
		/// <summary>
        /// A light is selected.
        /// </summary>
		LightSelected,
		/// <summary>
        /// Nothing is selected.
        /// </summary>
		NothingSelected,
		/// <summary>
        /// A wall is actually in creation.
        /// </summary>
		WallCreation
	}
}
