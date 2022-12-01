using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace EVA
{
    /// <summary>
    /// This class is used to manage the individual item 
    /// that will appear in the file chooser list view.
    /// </summary>
    public class ListItem : MonoBehaviour
    {
        /// <summary>
        /// This is a property with a getter and a setter allowing us
        /// to modify the image of the desired item.
        /// </summary>
        /// <returns>The get function returns the sprite parameter of the
        ///  Image component of our item.</returns>
        /// <value name="value">This is the parameter to give to the setter.
        ///  It will make the sprite parameter of the Image component of our 
        ///  item equals to value.</param>
        public Sprite ItemImage
        {
            get
            {
                return transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite;
            }
            set
            {
                transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = value;
            }
        }

        /// <summary>
        /// This is a property with a getter and a setter allowing us
        /// to modify the text of the desired item.
        /// </summary>
        /// <returns>The get function returns the text parameter of the
        ///  Text component of our item.</returns>
        /// <value name="value">This is the parameter to give to the setter.
        ///  It will make the text parameter of the Text component of our 
        ///  item equals to value.</param>
        public string ItemText
        {
            get
            {
                return transform.GetChild(1).GetComponent<Text>().text;
            }
            set
            {
                transform.GetChild(1).GetComponent<Text>().text = value;
            }
        }

        /// <summary>
        /// This is a property with a getter allowing us
        /// to access the method called by the desired item.
        /// </summary>
        /// <returns>The get function returns the method called by our item.</returns>
        public UnityEngine.UI.Button.ButtonClickedEvent ItemEvent
        {
            get
            {
                return GetComponent<UnityEngine.UI.Button>().onClick;
            }
        }
    }
}
