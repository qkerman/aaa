using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace EVA
{
    /// <summary>
    /// Script that manages the modular transform panel.
    /// </summary>
    public class TransformHandler : ModularHandler
    {
        /// <summary>
        /// The Object that is used for lock the transform's position of the selected object.
        /// </summary>
        public GameObject lockTogglePos;
        /// <summary>
        /// The Object that is used for lock the transform's rotation of the selected object.
        /// </summary>
        public GameObject lockToggleRot;
        /// <summary>
        /// The Object that is used for lock the transform's scale of the selected object.
        /// </summary>
        public GameObject lockToggleScale;

        /// <summary>
        /// The object that references the position in X of the manipulated.
        /// </summary>
        public GameObject posX;
        /// <summary>
        /// The object that references the position in Y of the manipulated.
        /// </summary>
        public GameObject posY;
        /// <summary>
        /// The object that references the position in Z of the manipulated.
        /// </summary>
        public GameObject posZ;

        /// <summary>
        /// The object that references the rotation in X of the manipulated.
        /// </summary>
        public GameObject rotX;
        /// <summary>
        /// The object that references the rotation in Y of the manipulated.
        /// </summary>
        public GameObject rotY;
        /// <summary>
        /// The object that references the rotation in Z of the manipulated.
        /// </summary>
        public GameObject rotZ;

        /// <summary>
        /// The object that references the scale in X of the manipulated.
        /// </summary>
        public GameObject scalX;
        /// <summary>
        /// The object that references the scale in Y of the manipulated.
        /// </summary>
        public GameObject scalY;
        /// <summary>
        /// The object that references the scale in Z of the manipulated.
        /// </summary>
        public GameObject scalZ;

        /// <summary>
        /// Object that contains all of the above relative to position, used to change the couleur depending on the type of control.
        /// </summary>
        public GameObject positionContent;
        /// <summary>
        /// Object that contains all of the above relative to rotation, used to change the couleur depending on the type of control.
        /// </summary>
        public GameObject rotationContent;
        /// <summary>
        /// Object that contains all of the above relative to scale, used to change the couleur depending on the type of control.
        /// </summary>
        public GameObject scaleContent;

        /// <summary>
        /// Boolean indicating if the manipulation mode has changed, this value is changed by control when the manipulation mode is changing.
        /// </summary>
        public static bool ManipulationModeChanged;
        /// <summary>
        /// Boolean indicating if the axe has changed, this value is changed by controle when the axe is changing.
        /// </summary>
        public static bool AxeChanged;
        /// <summary>
        /// The manipulation mode that needs to be outlined.
        /// </summary>
        public static ManipulationMode Manipulation;
        /// <summary>
        /// The axe that needs to be outlined.
        /// </summary>
        public static Axes Axe;

        /// <summary>
        /// Methods that updates the modular transform panel values when the handled object transform changes.
        /// Also updates the visual indications for the manipulation mode and axis.
        /// </summary>
        void Update()
        {
            if(handledObject != null)
            {
                if (handledObject.transform.hasChanged)
                {
                    UpdateValues();
                    handledObject.transform.hasChanged = false;
                }
            }

            if (ManipulationModeChanged)
            {
                changeOutlineTransform();
                ManipulationModeChanged = false;
            }

            if (AxeChanged)
            {
                changeColorAxe();
                AxeChanged = false;
            }
        }

        
        /// <summary>
        /// Updating values in the current used panel displaying informations.
        /// </summary>
        public void UpdateValues()
        {
            if(handledObject != null)
            {
                Vector3 positionSujet;
                Vector3 eulerSujet;
                Vector3 scaleSujet;

                positionSujet = handledObject.GetComponent<Object>().transform.position;
                eulerSujet = handledObject.GetComponent<Object>().transform.eulerAngles;
                scaleSujet = handledObject.GetComponent<Object>().transform.lossyScale;

                posX.GetComponentInChildren<InputField>().text = positionSujet.x.ToString();
                posY.GetComponentInChildren<InputField>().text = positionSujet.y.ToString();
                posZ.GetComponentInChildren<InputField>().text = positionSujet.z.ToString();

                rotX.GetComponentInChildren<InputField>().text = eulerSujet.x.ToString();
                rotY.GetComponentInChildren<InputField>().text = eulerSujet.y.ToString();
                rotZ.GetComponentInChildren<InputField>().text = eulerSujet.z.ToString();

                scalX.GetComponentInChildren<InputField>().text = scaleSujet.x.ToString();
                scalY.GetComponentInChildren<InputField>().text = scaleSujet.y.ToString();
                scalZ.GetComponentInChildren<InputField>().text = scaleSujet.z.ToString();

                
                lockTogglePos.GetComponent<Toggle>().isOn = handledObject.GetComponent<Object>().LockPos;
                lockToggleRot.GetComponent<Toggle>().isOn = handledObject.GetComponent<Object>().LockRot;
                lockToggleScale.GetComponent<Toggle>().isOn = handledObject.GetComponent<Object>().LockScale;
            }
            
        }

        /// <summary>
        /// Set to manipulate object to the current selected object displayed.
        /// </summary>
        /// <param name="obj">The object to be the target of the modular transform menu.</param>
        public override void setObject(GameObject obj)
        {
            base.setObject(obj);
            UpdateValues();
            changeOutlineTransform();
        }

        /// <summary>
        /// Lock the current selected object's position.
        /// </summary>
        /// <param name="b">The lock boolean.</param>
        public void lockPos(bool b)
        {
            this.handledObject.GetComponent<Object>().LockPos = b;
        }

        /// <summary>
        /// Lock the current selected object's rotate.
        /// </summary>
        /// <param name="b">The lock boolean.</param>
        public void lockRot(bool b)
        {
            this.handledObject.GetComponent<Object>().LockRot = b;
        }

        /// <summary>
        /// Lock the current selected object's scale.
        /// </summary>
        /// <param name="b">The lock boolean.</param>
        public void lockScale(bool b)
        {
            this.handledObject.GetComponent<Object>().LockScale = b;
        }
        /// <summary>
        /// Method that is called when the manipulation mode is changed, so that it can outline the good part of the transform panel, depending on this manipulation mode.
        /// </summary>
        public void changeOutlineTransform()
        {
            switch (Manipulation)
            {
                case ManipulationMode.TRANSLATION:
                    positionContent.GetComponent<UnityEngine.UI.Image>().enabled = true;
                    rotationContent.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    scaleContent.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    rotX.GetComponentInChildren<Text>().color = Color.black;
                    rotY.GetComponentInChildren<Text>().color = Color.black;
                    rotZ.GetComponentInChildren<Text>().color = Color.black;
                    scalX.GetComponentInChildren<Text>().color = Color.black;
                    scalY.GetComponentInChildren<Text>().color = Color.black;
                    scalZ.GetComponentInChildren<Text>().color = Color.black;
                    switch (Axe)
                    {
                        case Axes.ALL:
                        case Axes.X:
                            posX.GetComponentInChildren<Text>().color = Color.white;
                            posY.GetComponentInChildren<Text>().color = Color.black;
                            posZ.GetComponentInChildren<Text>().color = Color.black;
                            break;
                        case Axes.Y:
                            posX.GetComponentInChildren<Text>().color = Color.black;
                            posY.GetComponentInChildren<Text>().color = Color.white;
                            posZ.GetComponentInChildren<Text>().color = Color.black;
                            break;
                        case Axes.Z:
                            posX.GetComponentInChildren<Text>().color = Color.black;
                            posY.GetComponentInChildren<Text>().color = Color.black;
                            posZ.GetComponentInChildren<Text>().color = Color.white;
                            break;
                    }
                    break;
                case ManipulationMode.ROTATION:
                    positionContent.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    rotationContent.GetComponent<UnityEngine.UI.Image>().enabled = true;
                    scaleContent.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    posX.GetComponentInChildren<Text>().color = Color.black;
                    posY.GetComponentInChildren<Text>().color = Color.black;
                    posZ.GetComponentInChildren<Text>().color = Color.black;
                    scalX.GetComponentInChildren<Text>().color = Color.black;
                    scalY.GetComponentInChildren<Text>().color = Color.black;
                    scalZ.GetComponentInChildren<Text>().color = Color.black;
                    switch (Axe)
                    {
                        case Axes.ALL:
                        case Axes.X:
                            rotX.GetComponentInChildren<Text>().color = Color.white;
                            rotY.GetComponentInChildren<Text>().color = Color.black;
                            rotZ.GetComponentInChildren<Text>().color = Color.black;
                            break;
                        case Axes.Y:
                            rotX.GetComponentInChildren<Text>().color = Color.black;
                            rotY.GetComponentInChildren<Text>().color = Color.white;
                            rotZ.GetComponentInChildren<Text>().color = Color.black;
                            break;
                        case Axes.Z:
                            rotX.GetComponentInChildren<Text>().color = Color.black;
                            rotY.GetComponentInChildren<Text>().color = Color.black;
                            rotZ.GetComponentInChildren<Text>().color = Color.white;
                            break;
                    }
                    break;
                case ManipulationMode.SCALING:
                    positionContent.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    rotationContent.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    scaleContent.GetComponent<UnityEngine.UI.Image>().enabled = true;
                    posX.GetComponentInChildren<Text>().color = Color.black;
                    posY.GetComponentInChildren<Text>().color = Color.black;
                    posZ.GetComponentInChildren<Text>().color = Color.black;
                    rotX.GetComponentInChildren<Text>().color = Color.black;
                    rotY.GetComponentInChildren<Text>().color = Color.black;
                    rotZ.GetComponentInChildren<Text>().color = Color.black;
                    switch (Axe)
                    {
                        case Axes.X:
                            scalX.GetComponentInChildren<Text>().color = Color.white;
                            scalY.GetComponentInChildren<Text>().color = Color.black;
                            scalZ.GetComponentInChildren<Text>().color = Color.black;
                            break;
                        case Axes.Y:
                            scalX.GetComponentInChildren<Text>().color = Color.black;
                            scalY.GetComponentInChildren<Text>().color = Color.white;
                            scalZ.GetComponentInChildren<Text>().color = Color.black;
                            break;
                        case Axes.Z:
                            scalX.GetComponentInChildren<Text>().color = Color.black;
                            scalY.GetComponentInChildren<Text>().color = Color.black;
                            scalZ.GetComponentInChildren<Text>().color = Color.white;
                            break;
                        case Axes.ALL:
                            scalX.GetComponentInChildren<Text>().color = Color.white;
                            scalY.GetComponentInChildren<Text>().color = Color.white;
                            scalZ.GetComponentInChildren<Text>().color = Color.white;
                            break;
                    }
                    break;

            }  
        }

        /// <summary>
        /// Method that is called when the manipulation axis is changed, so that it can change the color of this axis on the highlighted part of the transform.
        /// </summary>
        public void changeColorAxe()
        {
            switch (Manipulation)
            {
                case ManipulationMode.TRANSLATION:
                    switch (Axe)
                    {
                        case Axes.X:
                            posX.GetComponentInChildren<Text>().color = Color.white;
                            posY.GetComponentInChildren<Text>().color = Color.black;
                            posZ.GetComponentInChildren<Text>().color = Color.black;
                            break;
                        case Axes.Y:
                            posX.GetComponentInChildren<Text>().color = Color.black;
                            posY.GetComponentInChildren<Text>().color = Color.white;
                            posZ.GetComponentInChildren<Text>().color = Color.black;
                            break;
                        case Axes.Z:
                            posX.GetComponentInChildren<Text>().color = Color.black;
                            posY.GetComponentInChildren<Text>().color = Color.black;
                            posZ.GetComponentInChildren<Text>().color = Color.white;
                            break;
                        case Axes.ALL:
                            posX.GetComponentInChildren<Text>().color = Color.white;
                            posY.GetComponentInChildren<Text>().color = Color.white;
                            posZ.GetComponentInChildren<Text>().color = Color.white;
                            break;
                    }
                    break;
                case ManipulationMode.ROTATION:
                    switch (Axe)
                    {
                        case Axes.X:
                            rotX.GetComponentInChildren<Text>().color = Color.white;
                            rotY.GetComponentInChildren<Text>().color = Color.black;
                            rotZ.GetComponentInChildren<Text>().color = Color.black;
                            break;
                        case Axes.Y:
                            rotX.GetComponentInChildren<Text>().color = Color.black;
                            rotY.GetComponentInChildren<Text>().color = Color.white;
                            rotZ.GetComponentInChildren<Text>().color = Color.black;
                            break;
                        case Axes.Z:
                            rotX.GetComponentInChildren<Text>().color = Color.black;
                            rotY.GetComponentInChildren<Text>().color = Color.black;
                            rotZ.GetComponentInChildren<Text>().color = Color.white;
                            break;
                        case Axes.ALL:
                            rotX.GetComponentInChildren<Text>().color = Color.white;
                            rotY.GetComponentInChildren<Text>().color = Color.white;
                            rotZ.GetComponentInChildren<Text>().color = Color.white;
                            break;
                    }
                    break;
                case ManipulationMode.SCALING:
                    switch (Axe)
                    {
                        case Axes.X:
                            scalX.GetComponentInChildren<Text>().color = Color.white;
                            scalY.GetComponentInChildren<Text>().color = Color.black;
                            scalZ.GetComponentInChildren<Text>().color = Color.black;
                            break;
                        case Axes.Y:
                            scalX.GetComponentInChildren<Text>().color = Color.black;
                            scalY.GetComponentInChildren<Text>().color = Color.white;
                            scalZ.GetComponentInChildren<Text>().color = Color.black;
                            break;
                        case Axes.Z:
                            scalX.GetComponentInChildren<Text>().color = Color.black;
                            scalY.GetComponentInChildren<Text>().color = Color.black;
                            scalZ.GetComponentInChildren<Text>().color = Color.white;
                            break;
                        case Axes.ALL:
                            scalX.GetComponentInChildren<Text>().color = Color.white;
                            scalY.GetComponentInChildren<Text>().color = Color.white;
                            scalZ.GetComponentInChildren<Text>().color = Color.white;
                            break;
                    }
                    break;

            }
        }
    }
}
