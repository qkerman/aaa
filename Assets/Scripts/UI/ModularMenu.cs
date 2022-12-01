using UnityEngine;

namespace EVA
{
    /// <summary>
    /// Script that manages the modular menu.
    /// </summary>
    public class ModularMenu : MonoBehaviour
    {
        /// <summary>
        /// The Object that is used for the modular menu informations.
        /// </summary>
        public GameObject Object;
        /// <summary>
        /// The title of the modular menu panel.
        /// </summary>
        public GameObject Object_title;
        /// <summary>
        /// The parent of all modular sub menus.
        /// </summary>
        public GameObject Content;

        /// <summary>
        /// Method that selects the given Object and create the relative sub menus.
        /// </summary>
        /// <param name="obj">The object to be the target of the modular menu.</param>
        public void SelectObject(Object obj)
        {
            
            DestroyObjectPanelContent();
            Object = null;
            Object_title.GetComponent<UnityEngine.UI.Text>().text = null;
            if (obj != null)
            {

                Object = obj.gameObject;
                Object_title.GetComponent<UnityEngine.UI.Text>().text = Object.name;
                if (obj.associatedModularPrefabs != null) 
                {   
                    //Une ou plusieurs interfaces modulaires sont presentes dans le prefab de l'objet contenant un EVA.Object ou derive
                    if (obj.associatedModularPrefabs.Count > 0)
                    {
                        GameObject modularMenu;
                        foreach (GameObject prefab in obj.associatedModularPrefabs)
                        {
                            modularMenu = Instantiate(prefab, Content.transform);
                            modularMenu.GetComponent<ModularHandler>().setObject(Object);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method that destroys all the modular sub menus.
        /// </summary>
        private void DestroyObjectPanelContent()
        {
            foreach (Transform child in Content.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
