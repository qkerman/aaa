using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVA
{
    /// <summary>
    /// Abstract class that manages sub menus for specific modular menus.
    /// </summary>
    public abstract class ModularHandler : MonoBehaviour
    {
        /// <summary>
        /// The GameObject used for the modular menu informations.
        /// </summary>
        protected GameObject handledObject;

        /// <summary>
        /// Set the object used for the modular menu informations.
        /// </summary>
        /// <param name="obj">The object to be the target of the modular menu.</param>
        public virtual void setObject(GameObject obj)
        {
            handledObject = obj;
        }
    }
}

