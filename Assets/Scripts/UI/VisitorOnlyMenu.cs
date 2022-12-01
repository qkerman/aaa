using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EVA {
    /// <summary>
    /// Class corresponding to all manipulations done when the users is in VisitorOnly mode.
    /// </summary>
    public class VisitorOnlyMenu : MonoBehaviour
    {
        /// <summary>
        /// Object corresponding to the displayed text of the menu.
        /// </summary>
        public GameObject label;
        /// <summary>
        /// Object corresponding to the quit button that opens the next menu.
        /// </summary>
        public GameObject quit;
        /// <summary>
        /// Object corresponding to the quit_refuse button that re opens the previous menu.
        /// </summary>
        public GameObject quit_refuse;
        /// <summary>
        /// Object corresponding to the quit_validate button that quit totally the application.
        /// </summary>
        public GameObject quit_validate;

        /// <summary>
        /// The start method, that enabled the quit button and disables the yes/no buttons.
        /// </summary>
        private void Start()
        {
            quit_refuse.SetActive(false);
            quit_validate.SetActive(false);
            quit.SetActive(true);
        }

        /// <summary>
        /// Set the content of the panel for quit the application.
        /// </summary>
        public void quitPressure()
        {
            label.GetComponent<Text>().text = "Are you sure about this ?";
            quit.SetActive(false);
            quit_refuse.SetActive(true);
            quit_validate.SetActive(true);
        }

        /// <summary>
        /// Set the content to the base of the panel.
        /// </summary>
        public void refusePressure()
        {
            label.GetComponent<Text>().text = "Do you want to quit the application ?";
            quit_refuse.SetActive(false);
            quit_validate.SetActive(false);
            quit.SetActive(true);
        }
    }
}
