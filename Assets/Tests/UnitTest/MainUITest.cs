using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVA
{
    public class MainUITest : MonoBehaviour
    {
        //Simple methods to be called by the different buttons of the interface
        public void TestImport()
        {
            Debug.Log("Import button works");
        }

        public void TestSave()
        {
            Debug.Log("Save button works");
        }

        public void TestLoad()
        {
            Debug.Log("Load button works");
        }

        public void TestQuit()
        {
            Debug.Log("Quit button works");
        }

        public void TestSettings()
        {
            Debug.Log("Settings button works");
        }
    }
}