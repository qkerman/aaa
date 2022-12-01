using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EVA
{
    /// <summary>
    /// This script is attached to GameObjects which have buttons, in order to manages their selections with laser pointers in VR.
    /// </summary>
    public class AutoSelect : MonoBehaviour, IPointerEnterHandler
    {
        /// <summary>
        /// This method selects the button, used when the laser pointer enter the zone.
        /// </summary>
        /// <param name="eventData">The eventData of the laser pointer</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.GetComponent<UnityEngine.UI.Selectable>().Select();
        }
    }
}
