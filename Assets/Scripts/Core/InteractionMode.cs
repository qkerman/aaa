using System;
namespace EVA
{
	/// <summary>
    /// This enumeration lists the mode for opening a gallery.
    /// </summary>
	public enum InteractionMode
	{
		/// <summary>
        /// This mode is for visiting a gallery without an option to modify it, can be described as a read-only mode.
        /// </summary>
		VISITOR_ONLY,
		/// <summary>
        /// This mode is for visiting a gallery, with a possibility to switch to editing mode.
        /// </summary>
		VISITOR,
		/// <summary>
        /// This mode is for editing a gallery, for example, showing up some 3D model for invisible things (sounds, lights).
        /// </summary>
		EDITOR,

	}
}
