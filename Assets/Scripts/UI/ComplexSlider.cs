using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace EVA
{
    /// <summary>
    /// This class represents a complex slider, using a slider and an input field to change values.
    /// </summary>
    public class ComplexSlider : MonoBehaviour
    {
        /// <summary>
        /// The slider used to change values within a range.
        /// </summary>
        public Slider slider;
        /// <summary>
        /// The input field used to change values with the keyboard.
        /// </summary>
        public InputField inputField;

        /// <summary>
        /// Callback called when the input field value is changed.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void inputValueChanged(string value)
        {
            if (float.TryParse(value, out _))
                if (slider.value != float.Parse(value))
                    slider.value = float.Parse(value);
        }
        /// <summary>
        /// Callback called when the slider value is changed.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void sliderValueChanged(float value)
        {
            if (inputField.text != value.ToString())
                inputField.text = value.ToString();
        }
    }
}
