using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EVA
{
    /// <summary>
    /// This class manages the creation or load a gallery.
    /// </summary>
    public class SceneCommunication : MonoBehaviour
    {
        /// <summary>
        /// The rig GameObject corresponds to the VR headset(or at 
        /// least the object that represents its transform.)
        /// </summary>
        public GameObject rig;

        /// <summary>
        /// When the program starts, the title menu opens itself.
        /// </summary>
        private void Start()
        {
            Open();
        }

        /// <summary>
        /// This method is used to open the title menu.
        /// </summary>
        public void Open()
        {
            Vector3 ourForward = rig.transform.forward;
            ourForward.y = rig.transform.position.y;
            transform.position = rig.transform.position + ourForward.normalized;
        }

        /// <summary>
        /// This methods load the gallery scene in editor mode.
        /// </summary>
        public void LoadScene()
        {
            SceneManager.LoadScene("Gallery");
            PlayerPrefs.SetInt("Mode", 2); //EDITOR
            /*
            if (rig.GetComponent<Control>().controller == OVRInput.Controller.LTouch)
                PlayerPrefs.SetInt("Hand", 0);
            else
                PlayerPrefs.SetInt("Hand", 1);
            */
        }

        /// <summary>
        /// This methods load the given gallery in the given mode. mode = true for visitor only mode, mode = false for editor mode.
        /// </summary>
        /// <param name="name">Name of the gallery to load (path).</param>
        /// <param name="mode">Boolean for the mode (visitor-only/editor).</param>
        public void LoadScene(string name, bool mode)
        {
            if (mode)
            {
                PlayerPrefs.SetInt("Mode", 0); //VISITOR ONLY
            }
            else
            {
                PlayerPrefs.SetInt("Mode", 2); //EDITOR
            }
            /*
            if (rig.GetComponent<Control>().controller == OVRInput.Controller.LTouch)
                PlayerPrefs.SetInt("Hand", 0);
            else
                PlayerPrefs.SetInt("Hand", 1);
            */
            PlayerPrefs.SetString("Path", name);
            SceneManager.LoadScene("Gallery");
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
