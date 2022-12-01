using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVA
{
    /// <summary>
    /// Component to put on the save panel.
    /// Contains the methods called by the Save button and by the list items.
    /// </summary>
    public class SaveUI : MonoBehaviour
    {
        /// <summary>
        /// InputField containing the name of the file in which the save will be put.
        /// </summary>
        public UnityEngine.UI.InputField inputField;

        /// <summary>
        /// SaveManager component on the gallery to call its save method.
        /// </summary>
        public SaveManager saveManager;

        /// <summary>
        /// ListFiles component on the list view of the panel to update its list of item when saving.
        /// </summary>
        public ListFiles listFiles;

        /// <summary>
        /// Method to be called when clicking a list item.
        /// Set the text of input field to the name of the file.
        /// </summary>
        /// <param name="path">Path of the savefile.</param>
        public void OnClick(string path)
        {
            inputField.text = System.IO.Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Method to be called when clicking the Save button.
        /// Save the gallery in the choosed file and update the list in the ui.
        /// </summary>
        public void OnSave()
        {
            saveManager.Save(FileChooser.CurrentPath +"/"+ inputField.text + ".eva");
            listFiles.CreateList(FileChooser.CurrentPath);
        }
    }
}