using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVA
{
    /// <summary>
    /// Interface to pin object to walls.
    /// </summary>
    public interface IPinnable
    {
        /// <summary>
        /// Boolean useful to know if the pinnable object is pinned to a wall.
        /// </summary>
        public abstract bool IsPinned
        {
            get;
        }

        /// <summary>
        /// Pin the object to the wall.
        /// </summary>
        /// <param name="wall">Wall to set as parent, and to pin the pinnable object on.</param>
        /// <param name="direction">Direction of the laser pointer.</param>
        public abstract void Pin(Wall wall, Vector3 direction);

        /// <summary>
        /// Unpin the object and reattach it to the parent in parameter.
        /// </summary>
        /// <param name="parent">Gameobject to set as the new parent.</param>
        public abstract void Unpin(GameObject parent);
    }
}
