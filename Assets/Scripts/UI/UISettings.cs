using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EVA
{
    /// <summary>
    /// Script to manage settings panel and its action on the gallery.
    /// </summary>
    public class UISettings : MonoBehaviour
    {
        /// <summary>
        /// The control script that contains the settings to change.
        /// </summary>
        public Control control;
        /// <summary>
        /// The switch for the settings of main hand.
        /// </summary>
        public GameObject switchHand;
        /// <summary>
        /// The switch for the settings of gallery mode.
        /// </summary>
        public GameObject switchMode;

        /// <summary>
        /// This function updates visual sliders in settings panel for shows state to the user.
        /// </summary>
        public void initSliders()
        {
            if(switchHand != null)
                updateMainHandSwitch();
            if(switchMode != null)
                updateGalleryModeSwitch();
        }


        /// <summary>
        /// Function called on the slider value changed to switch the main hand (Left : 0; right : 1).
        /// Note : The content of this function is commented, as this functionnality is not implemented in the final release.
        /// </summary>
        /// <param name="value">The new hand (0 for left, 1 for right).</param>
        public void changeMainHand(float value)
        {
            /*
            int v = Mathf.RoundToInt(value);
            switch (v)
            {
                case 0:
                    control.controller = OVRInput.Controller.LTouch;
                    PlayerPrefs.SetInt("Hand", 0);
                    break;
                case 1:
                    control.controller = OVRInput.Controller.RTouch;
                    PlayerPrefs.SetInt("Hand", 1);
                    break;
            }
            */
        }
        /// <summary>
        /// Update function to show on the settings panel the actual main hand in the control script.
        /// Note : The content of this function is commented, as this functionnality is not implemented in the final release.
        /// </summary>
        public void updateMainHandSwitch()
        {
            /*
            if(control.controller == OVRInput.Controller.LTouch)
            {
                switchHand.GetComponent<Slider>().value = 0;
            }
            else if (control.controller == OVRInput.Controller.RTouch)
            {
                switchHand.GetComponent<Slider>().value = 1;
            }
            */
        }
        /// <summary>
        /// Function to change the gallery mode, called when the switch slider is moved.
        /// </summary>
        /// <param name="value">The new gallery mode (0 for editor, 1 for visitor)</param>
        public void changeMode(float value)
        {
            int v = Mathf.RoundToInt(value);
            switch (v)
            {
                case 0:
                    control.GalleryMode = InteractionMode.EDITOR;
                    PlayerPrefs.SetInt("Mode", 2);
                    break;
                case 1:
                    control.GalleryMode = InteractionMode.VISITOR;
                    PlayerPrefs.SetInt("Mode", 1);
                    break;
            }
            
        }
        /// <summary>
        /// Update function to show on the settings panel the actual gallery mode.
        /// </summary>
        public void updateGalleryModeSwitch()
        {
            if (control.GalleryMode == InteractionMode.VISITOR_ONLY || control.GalleryMode == InteractionMode.VISITOR)
            {
                switchMode.GetComponent<Slider>().value = 1;
                if(control.GalleryMode == InteractionMode.VISITOR_ONLY)
                {
                    switchMode.GetComponent<Slider>().enabled = false; //When in visitor only mode, slider is disabled
                }
            }
            else if (control.GalleryMode == InteractionMode.EDITOR)
            {
                switchMode.GetComponent<Slider>().value = 0;
            }
        }
    }
}

