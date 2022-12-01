using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EVA
{
    /// <summary>
    /// Component to activate the Oculus Keyboard Overlay when the input field is selected.
    /// Implement the ISelectHandler interface to be triggered when the object is selected.
    /// </summary>
    public class VRInputField : MonoBehaviour, UnityEngine.EventSystems.ISelectHandler
    {
        /// <summary>
        /// The starts method, enabling the input field only if not in Unity Editor.
        /// </summary>
        private void Start()
        {
#if UNITY_EDITOR
            this.enabled = false;
#else
            this.enabled = true;
#endif
        }

        /// <summary>
        /// Oculus Keyboard Overlay
        /// </summary>
        private TouchScreenKeyboard overlayKeyboard;

        /// <summary>
        /// Update the input field on the gameobject when the Keyboard Overlay is activated.
        /// </summary>
        private void Update()
        {
            if (overlayKeyboard is object && TouchScreenKeyboard.visible)
                GetComponent<UnityEngine.UI.InputField>().text = overlayKeyboard.text;
            if (overlayKeyboard is object && !TouchScreenKeyboard.visible)
                overlayKeyboard = null;
        }

        /// <summary>
        /// Triggered when object is selected, activate the Keyboard Overlay.
        /// </summary>
        /// <param name="eventData">The Event Data.</param>
        public void OnSelect(BaseEventData eventData)
        {
            overlayKeyboard = TouchScreenKeyboard.Open(GetComponent<UnityEngine.UI.InputField>().text, TouchScreenKeyboardType.Default);
        }
    }
}
