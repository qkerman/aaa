using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EVA
{
    [System.Serializable]
    public class StringEvent : UnityEngine.Events.UnityEvent<string> { }

    /// <summary>
    /// This class is used to create the items that will
    /// be shown in the file chooser list view.
    /// </summary>
    public class ListFiles : MonoBehaviour
    {
        /// <summary>
        /// This scrollview will be the place where
        /// the items will be displayed.
        /// </summary>
        public GameObject scrollView;

        /// <summary>
        /// This prefab is the item to display
        /// in the scrollview.
        /// </summary>
        /// <remarks>
        /// This prefab had a script component ListItem
        /// allowing it to set its text and image component 
        /// to the right value according to the file linked to it.
        /// </remarks>
        public GameObject prefab;

        /// <summary>
        /// This GameObject represents the main menu.
        /// This menu contains all the panel where 
        /// the items will be displayed. 
        /// </summary>
        public GameObject mainUi;

        /// <summary>
        /// This sprite will be used as the image for
        /// the item of the current panel.
        /// </summary>
        public Sprite icon;

        /// <summary>
        /// This sprite represents a folder and will be used 
        /// as the image for the item if the item is supposed
        /// to be a folder.
        /// </summary>
        private Sprite folder;

        /// <summary>
        /// This string list contains the different extensions
        /// accepted for 3D models.(.glb .gltf)
        /// </summary>
        public List<string> extensions = new List<string>();

        /// <summary>
        /// This string list contains the different possible
        /// paterns that we will be looking for in the path of
        /// some files(like "360").
        /// </summary>
        public List<string> patterns = new List<string>();

        /// <summary>
        /// This is a function that take a string as
        /// parameter. It will be used to add a method to
        /// the button component of the item.
        /// </summary>
        public StringEvent func;


        /// <summary>
        /// This GameObject is the return button of the panel.
        /// It is used to link the buttons for the explicit navigation of the UI with the thumbsticks.
        /// </summary>
        public GameObject returnButton;

        /// <summary>
        /// This GameObject is the settings button of the panel.
        /// It is used to link the buttons for the explicit navigation of the UI with the thumbsticks.
        /// </summary>
        public GameObject settingsButton;

        /// <summary>
        /// This GameObject represents the button down.
        /// </summary>
        public GameObject downButton;

        /// <summary>
        /// This function initialize the folder sprite.
        /// </summary>
        private void Awake()
        {
            folder = Resources.Load<Sprite>("Sprites/folder_icon");
        }

        /// <summary>
        /// This function checks if we are in the persistent data path.
        /// If so, then it goes back to the previous panel. If not we 
        /// display the items of the parent directory.
        /// </summary>
        public void Return()
        {
            foreach (Transform child in scrollView.transform)
            {
                Destroy(child.gameObject);
            }

            string current_directory = FileChooser.CurrentPath;
            current_directory = current_directory.Replace('\\', '/');
            Debug.Log("Current directory");
            Debug.Log(current_directory);
            Debug.Log("Persisten data path");
            Debug.Log(Application.persistentDataPath);
            if (current_directory == Application.persistentDataPath)
            {
                mainUi.GetComponent<UIManager>().Return();
            }
            else
            {
                string parent_directory = Directory.GetParent(current_directory).FullName;
                CreateList(parent_directory);
            }
        }

        /// <summary>
        /// This method is called to create the list of items in the scrollview with the right image and text. 
        /// It also links the buttons of the ui to navigate with the controller.
        /// </summary>
        /// <param name="path">The path of the folder to create a list of item from.</param>
        public void CreateList(string path)
        {
            returnButton.GetComponent<UnityEngine.UI.Selectable>().Select();
            settingsButton.tag = "FirstButton";
            foreach (Transform child in scrollView.transform)
            {
                Destroy(child.gameObject);
            }
            List<string> files;
            if (patterns != null && patterns.Count > 0)
            {
                files = FileChooser.GetFiles(path, extensions, patterns);
            }
            else
            {
                files = FileChooser.GetFiles(path, extensions);
            }
            bool isfirst = true;
            int i = 0;
            if (files.Count == 0)
            {
                if (downButton)
                {
                    Navigation retNav = returnButton.GetComponent<UnityEngine.UI.Selectable>().navigation;
                    Navigation downNav = downButton.GetComponent<UnityEngine.UI.Selectable>().navigation;
                    retNav.selectOnDown = downButton.GetComponent<UnityEngine.UI.Selectable>();
                    downNav.selectOnUp = returnButton.GetComponent<UnityEngine.UI.Selectable>();
                    returnButton.GetComponent<UnityEngine.UI.Selectable>().navigation = retNav;
                    downButton.GetComponent<UnityEngine.UI.Selectable>().navigation = downNav;
                }
            }
            else
            {
                foreach (string file in files)
                {
                    Debug.Log(file);
                    GameObject item = Instantiate(prefab, scrollView.transform);
                    item.name = "item" + i;
                    i++;
                    Navigation navigation = item.GetComponent<UnityEngine.UI.Selectable>().navigation;
                    navigation.mode = Navigation.Mode.Explicit;
                    navigation.selectOnLeft = returnButton.GetComponent<UnityEngine.UI.Selectable>();
                    navigation.selectOnRight = settingsButton.GetComponent<UnityEngine.UI.Selectable>();

                    //if it's the first button of the scrollview, we link it to the return and settings button
                    if (isfirst)
                    {
                        isfirst = false;
                        settingsButton.tag = "Untagged";
                        item.tag = "FirstButton";
                        item.GetComponent<UnityEngine.UI.Button>().Select();
                        navigation.selectOnUp = returnButton.GetComponent<UnityEngine.UI.Selectable>();
                        Navigation retNav = returnButton.GetComponent<UnityEngine.UI.Selectable>().navigation;
                        retNav.selectOnDown = item.GetComponent<UnityEngine.UI.Selectable>();
                        returnButton.GetComponent<UnityEngine.UI.Selectable>().navigation = retNav;
                        Navigation setNav = settingsButton.GetComponent<UnityEngine.UI.Selectable>().navigation;
                        setNav.selectOnDown = item.GetComponent<UnityEngine.UI.Selectable>();
                        settingsButton.GetComponent<UnityEngine.UI.Selectable>().navigation = setNav;
                    }
                    //else we link it to the previous button
                    else
                    {
                        int index = item.transform.GetSiblingIndex();
                        GameObject prev = item.transform.parent.transform.GetChild(index - 1).gameObject;
                        navigation.selectOnUp = prev.GetComponent<UnityEngine.UI.Selectable>();
                        Navigation prevNav = prev.GetComponent<UnityEngine.UI.Selectable>().navigation;
                        prevNav.selectOnDown = item.GetComponent<UnityEngine.UI.Selectable>();
                        prev.GetComponent<UnityEngine.UI.Button>().navigation = prevNav;
                    }
                    //if it is the last button, link to the button down if it exists
                    if (i == files.Count)
                    {
                        Debug.Log("last button");
                        if (downButton && downButton.activeInHierarchy)
                        {
                            navigation.selectOnDown = downButton.GetComponent<UnityEngine.UI.Selectable>();
                            Navigation nextNav = downButton.GetComponent<UnityEngine.UI.Selectable>().navigation;
                            nextNav.selectOnUp = item.GetComponent<UnityEngine.UI.Selectable>();
                            downButton.GetComponent<UnityEngine.UI.Selectable>().navigation = nextNav;
                        }
                    }
                    item.GetComponent<UnityEngine.UI.Selectable>().navigation = navigation;
                    if (FileChooser.IsFolderExists(file))
                    {
                        item.GetComponent<ListItem>().ItemImage = folder;
                        item.GetComponent<ListItem>().ItemText = new DirectoryInfo(file).Name;
                        item.GetComponent<ListItem>().ItemEvent.AddListener(delegate { CreateList(file); });
                    }
                    else
                    {
                        item.GetComponent<ListItem>().ItemImage = icon;
                        item.GetComponent<ListItem>().ItemText = Path.GetFileNameWithoutExtension(file);
                        item.GetComponent<ListItem>().ItemEvent.AddListener(() => func.Invoke(file));
                    }
                }
            }
        }
    }
}
