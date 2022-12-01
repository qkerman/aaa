using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVA
{
    /// <summary>
    /// This class manages the navigation between the different panels of the main menu.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// This GameObject represents the main menu panel.
        /// </summary>
        /// <remarks>
        /// The main menu is the menu containing the different buttons 
        /// used to navigate between the different functionalities (import,
        /// save, load...).
        /// </remarks>
        public GameObject mainPanel;

        /// <summary>
        /// The rig GameObject corresponds to the VR headset(or at 
        /// least the object that represents its transform.)
        /// </summary>
        public GameObject rig;

        /// <summary>
        /// The offset will be used to make the different UI elements 
        /// spawn with a specific offset to the side of the player(headset).
        /// </summary>
        public Vector3 offset;

        /// <summary>
        /// This stack of GameObject is used to contain all the different
        /// panels that the user will be navigating through.
        /// </summary>
        private Stack<GameObject> panelStack = new Stack<GameObject>();

        /// <summary>
        /// The first button that will be selected when you access the ui panel.
        /// </summary>
        public GameObject firstButton;

        /// <summary>
        /// Panel containing the settings.
        /// </summary>
        public GameObject settings;

        /// <summary>
        /// At the start of the program, it opens the mainpanel.
        /// </summary>
        void Start()
        {
            Next(mainPanel);
        }

        /// <summary>
        /// This method is used to go to the next panel. If there is
        /// already a panel open then it will deactivate this one before
        /// going to the next.
        /// </summary>
        public void Next(GameObject panel)
        {
            if (panelStack.Count > 0)
            {
                panelStack.Peek().SetActive(false);
            }
            panelStack.Push(panel);
            panel.SetActive(true);
        }

        /// <summary>
        /// Click on the settings button.
        /// </summary>
        public void Settings()
        {
            if (settings != null)
            {
                if (!settings.activeSelf)
                {
                    Next(settings);
                    settings.GetComponent<UISettings>().initSliders();
                }
                else
                {
                    panelStack.Pop().SetActive(false);
                    panelStack.Peek().SetActive(true);
                }
            }
        }

        /// <summary>
        /// This method is used to go back to the previous panel
        /// if there is one and select its return button. 
        /// It does nothing otherwise.
        /// </summary>
        public void Return()
        {
            if (panelStack.Count > 1)
                {
                    panelStack.Pop().SetActive(false);
                    panelStack.Peek().SetActive(true);
                    GameObject[] returnButtons = GameObject.FindGameObjectsWithTag("ReturnButton");
                    GameObject returnButton = null;
                    foreach (GameObject button in returnButtons)
                    {
                        if (button.activeInHierarchy)
                        {
                            returnButton = button;
                        }
                    }
                    if (returnButton != null)
                    {
                        returnButton.GetComponent<UnityEngine.UI.Button>().Select();
                    }
                }
        }

        /// <summary>
        /// This method is used to open the main menu.
        /// </summary>
        public void Open()
        {
            Vector3 ourForward = rig.transform.forward;
            transform.position = rig.transform.position + ourForward.normalized + new Vector3(0,-0.2f);
            transform.eulerAngles = new Vector3(0, rig.transform.eulerAngles.y, 0);
            GetComponent<Canvas>().enabled = true;
            firstButton.GetComponent<UnityEngine.UI.Button>().Select();
            UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents = true;
        }

        /// <summary>
        /// This method is used to close the main menu.
        /// </summary>
        public void Close()
        {
            UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents = false;
            GetComponent<Canvas>().enabled = false;
            while (panelStack.Count > 1)
            {
                panelStack.Pop().SetActive(false);
                panelStack.Peek().SetActive(true);
            }
        }

        /// <summary>
        /// This method is called by the Quit button in the main menu
        /// and closes the application.
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }
    }
}