using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVA
{
    /// <summary>
    /// This class handles the different states of the user when using the application.
    /// </summary>
    public class VisibilityMode
    {
        /// <summary>
        /// The main menu of the app.
        /// </summary>
        public GameObject menu;

        /// <summary>
        /// The modular menu of the app.
        /// </summary>
        public GameObject modular;

        /// <summary>
        /// The quit menu of the app, for the visitor only mode.
        /// </summary>
        public GameObject quit;

        /// <summary>
        /// The visitor menu, that permits to change back to editor mode.
        /// </summary>
        public GameObject changer;

        /// <summary>
        /// Method that modify menus visibility depending on interaction mode.
        /// </summary>
        /// <param name="state">The interaction mode (visitor/editor)</param>
        public void setVisibility(InteractionMode state)
        {
            switch(state)
            {
                case InteractionMode.VISITOR_ONLY:
                    menu.SetActive(false);
                    modular.SetActive(false);
                    changer.SetActive(false);
                    break;

                case InteractionMode.VISITOR :
                    menu.SetActive(false);
                    modular.SetActive(false);
                    quit.SetActive(false);
                    break;

                case InteractionMode.EDITOR :
                    quit.SetActive(false);
                    break;
            }
        }
    }
}
