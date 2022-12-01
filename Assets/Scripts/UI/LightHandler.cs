using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Collections.Generic;

namespace EVA
{
    /// <summary>
    /// The script that is used by the light modular menu to update its values using the focused light, or to change this light settings.
    /// </summary>
    public class LightHandler : ModularHandler
    {
        /// <summary>
        /// GameObject containing toggles for the type of light.
        /// </summary>
        public GameObject typeToggles;
        /// <summary>
        /// The slider used to be a On/Off switch.
        /// </summary>
        public Slider slider;
        /// <summary>
        /// The list of parameters for the complex slider.
        /// </summary>
        public List<GameObject> parametersComplexSlider = new List<GameObject>();
        /// <summary>
        /// The toggle groupe for the light type.
        /// </summary>
        private ToggleGroup typeToggleGroup;

        /// <summary>
        /// The Toggle that is currently selected whithin the toggle group.
        /// </summary>
        public Toggle CurrentSelection
        {
            get { return typeToggleGroup.ActiveToggles().FirstOrDefault(); }
        }

        /// <summary>
        /// Function that is called when a toggle is selected, doing things on the light depending on the selected type of light.
        /// </summary>
        /// <param name="id">The toggle id of the selected toggle.</param>
        public void selectTypeToggle(int id)
        {
            Toggle[] toggles = typeToggleGroup.GetComponentsInChildren<Toggle>();
            switch (toggles[id].gameObject.GetComponentInChildren<Text>().text)
            {
                case "Directional":
                    handledObject.GetComponent<Light>().Type = LightType.Directional;
                    parametersComplexSlider[0].SetActive(false);
                    parametersComplexSlider[1].SetActive(false);
                    break;
                case "Spot":
                    handledObject.GetComponent<Light>().Type = LightType.Spot;
                    parametersComplexSlider[0].SetActive(true);
                    parametersComplexSlider[1].SetActive(true);
                    break;
                case "Point":
                    handledObject.GetComponent<Light>().Type = LightType.Point;
                    parametersComplexSlider[0].SetActive(true);
                    parametersComplexSlider[1].SetActive(false);
                    break;
                default:
                    Debug.Log("Invalid type of light !!!");
                    break;
            }
        }
        /// <summary>
        /// The function called for the light on/off switch.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void Switch(Single value)
        {
            handledObject.GetComponent<Light>().Switch = (1f == value);
        }
        /// <summary>
        /// The function called when range is modified.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void ModifyRange(float value) 
        {
            handledObject.GetComponent<Light>().Range=value;
        }
        /// <summary>
        /// The function called when spot angle is modified.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void ModifySpotAngle(float value) 
        {
            handledObject.GetComponent<Light>().SpotAngle = value;
        }

        /// <summary>
        /// The function called when light intensity is modified.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void ModifyIntensity(float value) 
        {
            handledObject.GetComponent<Light>().Intensity = value;
        }

        /// <summary>
        /// The function called when the target light of this modular panel changes.
        /// </summary>
        /// <param name="obj">The new target object.</param>
        public override void setObject(GameObject obj)
        {
            base.setObject(obj);
            if(handledObject != null)
            {
                typeToggleGroup = typeToggles.GetComponent<ToggleGroup>();
                Toggle[] toggles = typeToggleGroup.GetComponentsInChildren<Toggle>();
                switch (handledObject.GetComponent<Light>().Type)
                {
                    case LightType.Directional:
                        toggles[0].isOn = true;
                        parametersComplexSlider[0].SetActive(false);
                        parametersComplexSlider[1].SetActive(false);
                        break;
                    case LightType.Spot:
                        toggles[1].isOn = true;
                        break;
                    case LightType.Point:
                        toggles[2].isOn = true;
                        parametersComplexSlider[1].SetActive(false);
                        break;
                    default:
                        Debug.Log("Invalid type of light !!!");
                        break;
                }
                if (handledObject.GetComponent<Light>().Switch)
                {
                    slider.value = 1;
                }
                else slider.value = 0;
                parametersComplexSlider[0].GetComponentInChildren<Slider>().value = handledObject.GetComponent<Light>().Range;
                parametersComplexSlider[1].GetComponentInChildren<Slider>().value = handledObject.GetComponent<Light>().SpotAngle;
                parametersComplexSlider[2].GetComponentInChildren<Slider>().value = handledObject.GetComponent<Light>().Intensity;
            }
        }
    }
}

