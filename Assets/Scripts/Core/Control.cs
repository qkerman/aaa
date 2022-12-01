using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using System.Collections;

namespace EVA
{

    /// <summary>
    /// This class is managing the controls, but it's not finished yet.
    /// TODO : Don't forget to write better documentation when this class will be finished.
    /// </summary>
    public class Control : MonoBehaviour
    {
        /// <summary>
        /// Hovered object (its object script component).
        /// </summary>
        private Object hoverObject;
        /// <summary>
        /// Grabbed object (its object script component).
        /// </summary>
        private Object grabObject;
        /// <summary>
        /// Selected object (its object script component).
        /// </summary>
        public Object selectedObject;
        /// <summary>
        /// Control state.
        /// </summary>
        public ControlState state;

        /// <summary>
        /// The axis on which the resizing method is applied.
        /// </summary>
        public Axes manipulationAxis = Axes.X;

        /// <summary>
        /// The grabbed object's transform relative to the controller.
        /// </summary>
        Vector3 localGrabOffset = Vector3.zero;
        /// <summary>
        /// Local grab rotation.
        /// </summary>
        Quaternion localGrabRotation = Quaternion.identity;
        /// <summary>
        /// the raycast used to display a laser coming from the controller.
        /// </summary>
        public LineRenderer laser;
        /// <summary>
        /// Layer for passthrough, only used in this script for fading in from black.
        /// </summary>
        public OVRPassthroughLayer passthrough;
        /// <summary>
        /// gameobject representing the menu accesible with the menu button.
        /// </summary>
        private GameObject mainMenu;

        /// <summary>
        /// The main controller from OVRInput (usually Right, can be changed (many inputs won't change well)).
        /// </summary>
        public OVRInput.Controller controller;
        
        /// <summary>
        /// GameObject containing the camera.
        /// </summary>
        public GameObject centerEyeAnchor;

        /// <summary>
        /// The modular menu relative to an object, completed with sub menus relative to the object.
        /// </summary>
        public GameObject modularMenu;
        /// <summary>
        /// Right controller of the headset.
        /// </summary>
        public GameObject rightController;
        /// <summary>
        /// Left controller of the headset.
        /// </summary>
        public GameObject leftController;

        /// <summary>
        /// List of the points for the creation of a wall.
        /// </summary>
        public List<GameObject> points = new List<GameObject>();

        /// <summary>
        /// Gallery containing all objects.
        /// </summary>
        public GameObject gallery;

        /// <summary>
        /// Old value of the z rotation of the controller.
        /// </summary>
        float oldZRotation;

        /// <summary>
        /// Delta of the z rotation of the controller.
        /// </summary>
        float deltaZRotation;

        /// <summary>
        /// Manipulation mode.
        /// </summary>
        private ManipulationMode manipulationMode = ManipulationMode.TRANSLATION;

        /// <summary>
        /// Object creator (and importer for artwork).
        /// </summary>
        public GameObject creator;

        /// <summary>
        /// gameobject representing the editor menu.
        /// </summary>
        public GameObject editorMenu;
        /// <summary>
        /// gameobject representing the visitor menu.
        /// </summary>
        public GameObject visitorMenu;
        /// <summary>
        /// gameobject representing the visitor only menu.
        /// </summary>
        public GameObject visitorOnlyMenu;

        /// <summary>
        /// Gallery mode (visitor/editor).
        /// </summary>
        private InteractionMode galleryMode;

        /// <summary>
        /// If the stick was tilted last update.
        /// </summary>
        private bool isRemainStick;

        /// <summary>
        /// Property for the gallery mode.
        /// </summary>

        public GameObject gizmoTranslation;
        public GameObject gizmoScale;
        public GameObject gizmoRotation;


        private GameObject gizmo;

        public Color defaultXColor;
        public Color defaultYColor;
        public Color defaultZColor;

        // Coroutine to vibrate
        IEnumerator Haptics(float frequency, float amplitude, float duration, bool rightHand, bool leftHand)
        {
            if (rightHand) OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.RTouch);
            if (leftHand) OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.LTouch);

            yield return new WaitForSeconds(duration);

            if (rightHand) OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
            if (leftHand) OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        }

        public InteractionMode GalleryMode
        {
            get
            {
                return galleryMode;
            }
            set
            {
                if(value == InteractionMode.EDITOR || value == InteractionMode.VISITOR || value == InteractionMode.VISITOR_ONLY)
                {
                    bool aActiver = false;
                    Vector3 positionActuelleMenu = new Vector3();
                    galleryMode = value;
                    foreach (Object obj in gallery.transform.GetComponentsInChildren<Object>())
                    {
                        obj.ChangeMode(galleryMode);
                    }
                    if (selectedObject != null)
                        selectedObject.GetComponent<Object>().ChangeMode(galleryMode);
                    if (grabObject != null)
                        grabObject.GetComponent<Object>().ChangeMode(galleryMode);

                    positionActuelleMenu = mainMenu.transform.position;
                    if (mainMenu.GetComponent<Canvas>().enabled)
                    {
                        aActiver = true;
                        mainMenu.GetComponent<UIManager>().Close();
                    }
                    
                    switch (galleryMode)
                    {
                        case InteractionMode.EDITOR:
                            mainMenu = editorMenu;
                            break;
                        case InteractionMode.VISITOR:
                            mainMenu = visitorMenu;
                            state = ControlState.NothingSelected;
                            modularMenu.GetComponent<Canvas>().enabled = false;
                            break;
                        case InteractionMode.VISITOR_ONLY:
                            mainMenu = visitorOnlyMenu;
                            state = ControlState.NothingSelected;
                            modularMenu.GetComponent<Canvas>().enabled = false;
                            break;
                    }
                    if (aActiver)
                    {
                        mainMenu.GetComponent<UIManager>().Open();
                        mainMenu.transform.position = positionActuelleMenu;
                    }
                    UISettings[] ss = mainMenu.GetComponentsInChildren<UISettings>(true);
                    foreach (UISettings s in ss)
                        s.initSliders();
                }
            }
        }

        /// <summary>
        /// Read the actual main menu.
        /// </summary>
        /// <returns>the actual main menu.</returns>
        public GameObject GetMainMenu()
        {
            return mainMenu;
        }
        
        /// <summary>
        /// Setup Passtrhough and the primary controller on the right hand.
        /// </summary>
        private void Start()
        {
            gizmoTranslation.SetActive(false);
            gizmoScale.SetActive(false);
            gizmoRotation.SetActive(false);

            gizmo = gizmoTranslation;
            
            defaultXColor = gizmo.transform.GetChild(0).GetComponent<Renderer>().material.color;
            defaultYColor = gizmo.transform.GetChild(1).GetComponent<Renderer>().material.color;
            defaultZColor = gizmo.transform.GetChild(2).GetComponent<Renderer>().material.color;

            gizmo.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
            gizmo.transform.GetChild(1).GetComponent<Outline>().enabled = false;
            gizmo.transform.GetChild(2).GetComponent<Outline>().enabled = false;

            controller = OVRInput.Controller.RTouch;
            mainMenu = editorMenu;
            if (PlayerPrefs.HasKey("Mode"))
            {
                GalleryMode = (InteractionMode)System.Enum.ToObject(typeof(InteractionMode), PlayerPrefs.GetInt("Mode"));
            }
            else
            {
                GalleryMode = InteractionMode.EDITOR; //Si le programme est lanc� directement depuis la sc�ne de galerie, ou �tat dans le title menu
            }
            /*
            if (PlayerPrefs.HasKey("Hand"))
            {
                switch (PlayerPrefs.GetInt("Hand"))
                {
                    case 0:
                        controller = OVRInput.Controller.LTouch;
                        break;
                    case 1:
                        controller = OVRInput.Controller.RTouch;
                        break;
                    default:
                        controller = OVRInput.Controller.RTouch;
                        break;
                }
            }
            else
            {
                controller = OVRInput.Controller.RTouch; //Si le programme est lanc� directement depuis la sc�ne de galerie, ou �tat dans le title menu
            }
            */
            state = ControlState.NothingSelected;
            if (passthrough)
            {
                passthrough.colorMapEditorBrightness = 0;
                passthrough.colorMapEditorContrast = 0;
            }

            TransformHandler.Manipulation = manipulationMode;
            TransformHandler.ManipulationModeChanged = true;
            TransformHandler.Axe = manipulationAxis;
            TransformHandler.AxeChanged = true;

        }

        /// <summary>
        /// Manages the controls every frame.
        /// </summary>
        void Update()
        {
            if(selectedObject != null)
            {
                gizmo.transform.position = selectedObject.transform.position;
                if(manipulationMode != ManipulationMode.TRANSLATION)
                {
                    gizmo.transform.rotation = selectedObject.transform.rotation;
                }

            }
            // Open the mainMenu with the start button
            if (state != ControlState.WallCreation &&(OVRInput.GetDown(OVRInput.Button.Start) || Input.GetKeyDown(KeyCode.Tab)))
            {
                if (mainMenu.GetComponent<Canvas>().enabled)
                {
                    mainMenu.GetComponent<UIManager>().Close();
                }
                else
                {
                    if(selectedObject is object)
                        unselectObject();
                    mainMenu.GetComponent<UIManager>().Open();
                }
            }
            switch (GalleryMode)
            {
                case InteractionMode.EDITOR:
                    Vector3 controllerPos = OVRInput.GetLocalControllerPosition(controller);
                    Quaternion controllerRot = OVRInput.GetLocalControllerRotation(controller);
                    deltaZRotation = oldZRotation - controllerRot.eulerAngles.z;
                    oldZRotation = controllerRot.eulerAngles.z;

                    // If there is no grabObject, do the rayCasting
                    if (grabObject == null)
                    {
                        FindHoverObject(controllerPos, controllerRot);
                    }

                    // If we are creating a wall, go to that function, else do the normal controls
                    if (state == ControlState.WallCreation)
                    {
                        ManageWallCreationInputs(controllerPos);
                    }
                    else
                    {
                        // When selecting the void
                        if (selectedObject && hoverObject == null && (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller)) || Input.GetKeyDown(KeyCode.LeftControl))
                        {
                            Vector3 vector3 = laser.GetPosition(1) - laser.GetPosition(0);
                            if (vector3.magnitude < 10.0f)
                                Debug.Log("is in canvas");
                            else
                            {
                                unselectObject();
                            }
                        }

                        //if Oculus button B or Escape key are pressed
                        if (OVRInput.GetDown(OVRInput.Button.Two) || Input.GetButtonDown("Cancel"))
                        {
                            //if the canvas is enabled
                            if (mainMenu.GetComponent<Canvas>().enabled)
                            {
                                //we get the active return button
                                GameObject[] returnButtons = GameObject.FindGameObjectsWithTag("ReturnButton");
                                GameObject returnButton = null;
                                foreach (GameObject button in returnButtons)
                                {
                                    if (button.activeInHierarchy)
                                    {
                                        returnButton = button;
                                    }
                                }
                                //if it exists
                                if (returnButton != null)
                                {
                                    //execute the methods link to its onclick property
                                    ExecuteEvents.Execute(returnButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
                                    //then we select the first button of the active panel
                                    GameObject[] firstButtons = GameObject.FindGameObjectsWithTag("FirstButton");
                                    GameObject firstButton = null;
                                    foreach (GameObject button in firstButtons)
                                    {
                                        if (button.activeInHierarchy)
                                        {
                                            firstButton = button;
                                        }
                                    }
                                    if (firstButton != null)
                                    {
                                        firstButton.GetComponent<UnityEngine.UI.Button>().Select();
                                    }
                                }
                            }
                        }


                        if (OVRInput.GetDown(OVRInput.Button.Four) && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) || Input.GetKeyDown(KeyCode.Delete))
                        {
                            if (grabObject is object)
                            {
                                DeleteGrabbedObject();
                            }
                        }

                        // on an object
                        if (hoverObject)
                        {
                            // If the Index Trigger is pressed, select the object
                            if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller) || Input.GetKeyDown(KeyCode.Space)) && hoverObject != selectedObject)
                            {
                                // selecting
                                if (selectedObject != null)
                                {
                                    unselectObject();
                                }
                                gizmo.SetActive(true);
                                selectObject();
                                
                            }

                            // If the hand trigger is pressed, grab the object
                            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller))
                            {
                                // grabbing
                                StartCoroutine(Haptics(0.2f, 0.2f, 0.1f, true, false));
                                
                                grabObject = hoverObject;
                                if (selectedObject != hoverObject && selectedObject is object)
                                {
                                    unselectObject();
                                    selectObject();
                                }
                                if(selectedObject is null)
                                {
                                    selectObject();
                                }
                                // Change the parent of the object to keep his rotation
                                grabObject.ChangeParent(rightController);
                                GrabHoverObject(controllerPos, controllerRot);
                            }
                        }

                        // when an object is grabbed
                        if (grabObject)
                        {
                            ManipulateObject(controllerPos, controllerRot);
                            if (!OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, controller))
                            {
                                grabObject.ChangeParent(gallery);
                                grabObject = null;
                            }
                            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
                            {
                                PinToWall(controllerPos, controllerRot);
                            }
                        }

                        // If the button B and the  left hand trigger is pressed, change the manipulation mode
                        if( OVRInput.GetUp(OVRInput.Button.One, controller) || Input.GetKeyDown(KeyCode.LeftAlt))
                        {
                            if (manipulationMode == ManipulationMode.SCALING)
                            {
                                manipulationMode = ManipulationMode.TRANSLATION;
                                if (manipulationAxis == Axes.ALL)
                                {
                                    manipulationAxis = Axes.X;
                                    TransformHandler.Axe = manipulationAxis;
                                    TransformHandler.AxeChanged = true;
                                }
                            }
                            else
                            {
                                manipulationMode++;
                            }
                            TransformHandler.Manipulation = manipulationMode;
                            TransformHandler.ManipulationModeChanged = true;

                            Vector3 currentSize = gizmo.transform.localScale;
                            gizmo.SetActive(false);
                            switch (manipulationMode)
                            {
                                case ManipulationMode.TRANSLATION:
                                    gizmo = gizmoTranslation;
                                    gizmo.transform.localRotation = Quaternion.Euler(0, 0, 0);
                                    break;

                                case ManipulationMode.SCALING:
                                    gizmo = gizmoScale;
                                    break;
                                    
                                case ManipulationMode.ROTATION:
                                    gizmo = gizmoRotation;
                                    break;
                            }
                            gizmo.SetActive(true);
                            gizmo.transform.localScale = currentSize;
                            highlightAxis(manipulationAxis);
                        }
                        // else if the button B is pressed, change the Axis
                        else if (OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.Space))
                        {
                            if (manipulationAxis == Axes.Z)
                            {
                                if (manipulationMode == ManipulationMode.SCALING)
                                    manipulationAxis = Axes.ALL;
                                else manipulationAxis = Axes.X;
                            }
                            else
                            {
                                manipulationAxis++;
                            }
                            TransformHandler.Axe = manipulationAxis;
                            TransformHandler.AxeChanged = true;

                            highlightAxis(manipulationAxis);

                        }

                        // when an object is selected
                        if (selectedObject)
                        {
                            ManipulateSelectedObject(controllerPos, controllerRot);
                            if (grabObject is null && (OVRInput.GetDown(OVRInput.Button.Two) && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) || Input.GetKeyUp(KeyCode.D))
                            {
                                Debug.Log("Bouton Duplication : ");
                                Debug.Log(creator.GetComponent<EVA.Creator>().Duplicate(selectedObject.gameObject));
                            }
                        }
                    }
                    break;
                case InteractionMode.VISITOR_ONLY:
                case InteractionMode.VISITOR:
                default:
                    //TODO Gestion des contr�les du mode visiteur
                    break;
            }
        }

        private void resetGizmoColor()
        {
            gizmo.transform.GetChild(0).GetComponent<Renderer>().material.color = defaultXColor;
            gizmo.transform.GetChild(0).GetComponent<Outline>().enabled = false;

            gizmo.transform.GetChild(1).GetComponent<Renderer>().material.color = defaultYColor;
            gizmo.transform.GetChild(1).GetComponent<Outline>().enabled = false;

            gizmo.transform.GetChild(2).GetComponent<Renderer>().material.color = defaultZColor;
            gizmo.transform.GetChild(2).GetComponent<Outline>().enabled = false;

            
        }
        private void highlightAxis(Axes axis)
        {
            resetGizmoColor();
            switch (axis)
            {
                case Axes.X:
                    gizmo.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
                    gizmo.transform.GetChild(0).GetComponent<Outline>().enabled = true;
                    break;

                case Axes.Y:
                    gizmo.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.white;
                    gizmo.transform.GetChild(1).GetComponent<Outline>().enabled = true;
                    break;

                case Axes.Z:
                    gizmo.transform.GetChild(2).GetComponent<Renderer>().material.color = Color.white;
                    gizmo.transform.GetChild(2).GetComponent<Outline>().enabled = true;
                    break;

                case Axes.ALL:
                    gizmo.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
                    gizmo.transform.GetChild(0).GetComponent<Outline>().enabled = true;

                    gizmo.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.white;
                    gizmo.transform.GetChild(1).GetComponent<Outline>().enabled = true;

                    gizmo.transform.GetChild(2).GetComponent<Renderer>().material.color = Color.white;
                    gizmo.transform.GetChild(2).GetComponent<Outline>().enabled = true;
                    break;
            }
        }
        
        /// <summary>
        /// Delete the grabbed object.
        /// If its a wall, its children are reattached to the gallery before it is deleted.
        /// </summary>
        private void DeleteGrabbedObject()
        {
            GameObject toDelete = grabObject.gameObject;
            if (toDelete.GetComponent<Wall>())
            {
                while (toDelete.transform.childCount > 1)
                {
                    toDelete.transform.GetChild(1).GetComponent<IPinnable>().Unpin(gallery);
                }
            }
            if (modularMenu != null)
            {
                modularMenu.GetComponent<ModularMenu>().SelectObject(null);
                modularMenu.GetComponent<Canvas>().enabled = false;
            }
            grabObject = null;
            selectedObject = null;
            Destroy(toDelete);
        }

        /// <summary>
        /// Pin an object to the closest wall in front of the laser pointer, if it is an image or a video.
        /// </summary>
        /// <param name="controllerPos">Vector for controller position.</param>
        /// <param name="controllerRot">Quaternion for controller rotation.</param>
        private void PinToWall(Vector3 controllerPos, Quaternion controllerRot)
        {
            Debug.Log("Pin to wall.");
            //if the object is pinnable
            if (grabObject.gameObject.GetComponent<Object>() is IPinnable)
            {
                Debug.Log(grabObject.transform.parent);
                //if its already in a wall, unpin the object
                if (grabObject.transform.parent.GetComponent<Wall>())
                {
                    Debug.Log("Parent is wall.");
                    grabObject.transform.GetComponent<IPinnable>().Unpin(rightController);
                    Debug.Log(grabObject.transform.parent);
                }
                //else get the closest wall in the direction of the laser pointer
                else
                {
                    Debug.Log("Parent is not wall.");
                    Wall wall = null;
                    RaycastHit[] objectsHit = Physics.RaycastAll(controllerPos, controllerRot * Vector3.forward);
                    float closestObject = Mathf.Infinity;
                    Vector3 normal = new Vector3();
                    foreach (RaycastHit hit in objectsHit)
                    {
                        float thisHitDistance = Vector3.Distance(hit.point, controllerPos);
                        if (thisHitDistance < closestObject && hit.collider.gameObject.transform.parent.GetComponent<Wall>())
                        {
                            wall = hit.collider.gameObject.transform.parent.GetComponent<Wall>();
                            Debug.Log(wall);
                            closestObject = thisHitDistance;
                            normal = hit.normal;
                        }
                    }
                    //if there is a wall, pin the object.
                    if (wall)
                    {
                        Debug.Log("Set parent to wall.");
                        Debug.Log(grabObject.transform.parent);
                        grabObject.transform.GetComponent<IPinnable>().Pin(wall,normal);
                        Debug.Log(grabObject.transform.parent);
                    }
                }
            }
        }

        /// <summary>
        /// Select the <see cref="hoverObject"/>, by putting it in the <see cref="selectedObject"/> and by enabling the Outline component.
        /// </summary>
        private void selectObject()
        {
            selectedObject = hoverObject;
            selectedObject.gameObject.GetComponent<Outline>().enabled = true;
            gizmo.transform.localScale = selectedObject.transform.localScale.magnitude * Vector3.one;


            if (modularMenu != null && selectedObject is object)
            {
                modularMenu.GetComponent<Canvas>().enabled = true;
                
                if (controller == OVRInput.Controller.RTouch)
                {
                    modularMenu.transform.SetParent(leftController.transform);
                    modularMenu.transform.localPosition = new Vector3(0.15f, 0.2f, 0.1f);
                }
                else if (controller == OVRInput.Controller.LTouch)
                {
                    modularMenu.transform.parent = rightController.transform;
                    modularMenu.transform.localPosition = new Vector3(-0.15f, 0.2f, 0.1f);
                }
                modularMenu.GetComponent<ModularMenu>().SelectObject(selectedObject);
                gizmo.SetActive(true);
            }
        }

        /// <summary>
        /// Unselect the <see cref="hoverObject"/>, by putting null in the <see cref="selectedObject"/> and by disabling the Outline component.
        /// </summary>
        private void unselectObject()
        {
            selectedObject.gameObject.GetComponent<Outline>().enabled = false;
            selectedObject = null;
            if (modularMenu != null)
            {
                gizmo.SetActive(false);
                modularMenu.GetComponent<ModularMenu>().SelectObject(null);
                modularMenu.GetComponent<Canvas>().enabled = false;
            }

        }

        /// <summary>
        /// Find the closest object to be touched by the raycast (laser pointer from the controller).
        /// </summary>
        /// <param name="controllerPos">Vector for controller position.</param>
        /// <param name="controllerRot">Quaternion for controller rotation.</param>
        private void FindHoverObject(Vector3 controllerPos, Quaternion controllerRot)
        {
            RaycastHit[] objectsHit = Physics.RaycastAll(controllerPos, controllerRot * Vector3.forward);
            float closestObject = Mathf.Infinity;
            bool showLaser = true;
            float lineLenght = (laser.GetPosition(1) - laser.GetPosition(0)).magnitude;
            Vector3 labelPosition = Vector3.zero;
            if (objectsHit.Length == 0)
            {
                hoverObject = null;
            }
            else
            {
                foreach (RaycastHit hit in objectsHit)
                {
                    float thisHitDistance = Vector3.Distance(hit.point, controllerPos);
                    if (thisHitDistance < closestObject && thisHitDistance < lineLenght)
                    {
                        hoverObject = hit.collider.gameObject.GetComponentInParent<Object>();
                        closestObject = thisHitDistance;
                        labelPosition = hit.point;
                    }
                }
            }
            
            

            // if intersecting with an object, grab it
            Collider[] hitColliders = Physics.OverlapSphere(controllerPos, 0.05f);
            foreach (var hitCollider in hitColliders)
            {
                // use the last object, if there are multiple hits.
                // If objects overlap, this would require improvements.
                Object test = hitCollider.gameObject.GetComponentInParent<Object>();
                if (test != null)
                {
                    hoverObject = test;
                    showLaser = false;
                    labelPosition = hitCollider.ClosestPoint(controllerPos);
                    labelPosition += (Camera.main.transform.position - labelPosition).normalized * 0.05f;
                }
            }
            // show/hide laser pointer
            if (laser)
            {
                laser.enabled = (showLaser);
            }
        }

        /// <summary>
        /// Grabs the actual hovered object by modifying local grab offset and rotation.
        /// </summary>
        /// <param name="controllerPos">Vector for controller position.</param>
        /// <param name="controllerRot">Quaternion for controller rotation.</param>
        void GrabHoverObject(Vector3 controllerPos, Quaternion controllerRot)
        {
            localGrabOffset = Quaternion.Inverse(controllerRot) * (grabObject.transform.position - controllerPos);
            localGrabRotation = Quaternion.Inverse(controllerRot) * grabObject.transform.rotation;
        }


        /// <summary>
        /// Object manipulation with the controller.
        /// </summary>
        /// <param name="controllerPos">Vector for controller position.</param>
        /// <param name="controllerRot">Quaternion for controller rotation.</param>
        void ManipulateObject(Vector3 controllerPos, Quaternion controllerRot)
        {
            float zRotation = controllerRot.eulerAngles.z;
            Vector2 thumbstickL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            Vector2 thumbstickR = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            //if the object is in a wall, it can only be moved alongside it.
            if (grabObject.transform.parent.GetComponent<Wall>())
            {
                RaycastHit[] objectsHit = Physics.RaycastAll(controllerPos, controllerRot * Vector3.forward);
                foreach (RaycastHit hit in objectsHit)
                {
                    if (hit.collider.gameObject == grabObject.transform.parent.GetComponent<Wall>().plane)
                    {
                        grabObject.SetPosition(hit.point + hit.normal * 0.001f);
                        grabObject.Rotate(deltaZRotation, Axes.Z);
                    }
                }
            }
            else
            {
                ClampGrabOffset(ref localGrabOffset, thumbstickR.y);
                grabObject.SetPosition(controllerPos + controllerRot * localGrabOffset);
            }
        }

        /// <summary>
        /// Selected object manipulation with the controller.
        /// </summary>
        /// <param name="controllerPos">Vector for controller position.</param>
        /// <param name="controllerRot">Quaternion for controller rotation.</param>
        void ManipulateSelectedObject(Vector3 controllerPos, Quaternion controllerRot)
        {
            if (grabObject is null)
            {
                Vector2 thumbstickR = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
                float inputThumb = (Mathf.Abs(thumbstickR.x) > Math.Abs(thumbstickR.y) ? thumbstickR.x : thumbstickR.y);
                if(inputThumb < 0.1 && inputThumb > -0.1)
                {
                    isRemainStick = false;
                }
                switch (manipulationMode)
                {
                    case ManipulationMode.TRANSLATION:
                        TranslateSelectedObject(inputThumb);
                        break;
                    case ManipulationMode.ROTATION:
                        RotateSelectedObject(inputThumb);
                        break;
                    case ManipulationMode.SCALING:
                        ScaleSelectedObject(inputThumb);
                        break;
                }
            }
        }

        /// <summary>
        /// Scale the selected object.
        /// </summary>
        /// <param name="inputThumb">Analogical value of the thumbStick.</param>
        private void ScaleSelectedObject(float inputThumb)
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
            {
                if (!isRemainStick && (inputThumb > 0.1 || inputThumb < -0.1))
                {
                    Vector3 localScale = selectedObject.transform.localScale;
                    switch (manipulationAxis)
                    {
                        case Axes.X:
                            localScale.x += (inputThumb >= 0 ? 0.1f : -0.1f);
                            localScale.x = Mathf.Round(localScale.x * 10) / 10;
                            break;
                        case Axes.Y:
                            localScale.y += (inputThumb >= 0 ? 0.1f : -0.1f);
                            localScale.y = Mathf.Round(localScale.y * 10) / 10;
                            break;
                        case Axes.Z:
                            localScale.z += (inputThumb >= 0 ? 0.1f : -0.1f);
                            localScale.z = Mathf.Round(localScale.z * 10) / 10;
                            break;
                        case Axes.ALL:
                            localScale += (inputThumb >= 0 ? Vector3.one * 0.1f : Vector3.one * -0.1f);
                            localScale = new Vector3(Mathf.Round(localScale.x * 10) / 10, Mathf.Round(localScale.y * 10) / 10, Mathf.Round(localScale.z * 10) / 10);
                            break;
                        default:
                            Debug.Log("There is a big error");
                            break;
                    }
                    selectedObject.SetScale(localScale);
                    isRemainStick = true;
                }
            }
            else
            {
                selectedObject.Resize(inputThumb * 0.05f, manipulationAxis);
                gizmo.transform.localScale = selectedObject.transform.localScale.magnitude * Vector3.one;
            }
        }

        /// <summary>
        /// Rotate the selected object.
        /// </summary>
        /// <param name="inputThumb">Analogical value of the thumbStick.</param>
        private void RotateSelectedObject(float inputThumb)
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
            {
                if (!isRemainStick && (inputThumb > 0.1 || inputThumb < -0.1))
                {
                    selectedObject.Rotate((inputThumb >= 0 ? 5f : -5f), manipulationAxis);
                    isRemainStick = true;
                }
            }
            else
            {
                selectedObject.Rotate(inputThumb * 2, manipulationAxis);
            }
        }

        /// <summary>
        /// Translate the selected object.
        /// </summary>
        /// <param name="value">Analogical value of the thumbStick.</param>
        private void TranslateSelectedObject(float value)
        {
            Vector3 translation;
            if (selectedObject is IPinnable pinnable && pinnable.IsPinned)
            {
                Transform wall = selectedObject.transform.parent.GetComponent<Wall>().plane.transform;
                float dot = Vector3.Dot(Vector3.right, wall.right);
                float sign = (dot >= 0) ? 1 : -1;
                if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
                {
                    Vector3 locPos = selectedObject.transform.localPosition;
                    if (!isRemainStick && (value > 0.1 || value < -0.1))
                    {
                        switch (manipulationAxis)
                        {
                            case Axes.X:
                                translation = (value >= 0 ? 0.1f * sign * Vector3.right : -0.1f * sign * Vector3.right);
                                selectedObject.Translate(translation);
                                selectedObject.transform.localPosition = new Vector3(Mathf.Round(locPos.x * 10) / 10, locPos.y, locPos.z);
                                break;
                            case Axes.Y:
                                translation = (value >= 0 ? 0.1f * Vector3.up : -0.1f * Vector3.up);
                                selectedObject.Translate(translation);
                                selectedObject.transform.localPosition = new Vector3(locPos.x, Mathf.Round(locPos.y * 10) / 10, locPos.z);
                                break;
                            default:
                                translation = new Vector3();
                                Debug.Log("Can't translate on those axes");
                                break;
                        }                        
                        isRemainStick = true;
                    }
                }
                else
                {
                    switch (manipulationAxis)
                    {
                        case Axes.X:
                            translation = 0.05f * sign * value * Vector3.right;
                            break;
                        case Axes.Y:
                            translation = 0.05f * value * Vector3.up;
                            break;
                        default:
                            translation = new Vector3();
                            Debug.Log("Can't translate on those axes");
                            break;
                    }
                    selectedObject.Translate(translation);
                }
            }
            else
            {
                Vector3 newPos = selectedObject.transform.position;
                if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
                {
                    if (!isRemainStick && (value > 0.1 || value < -0.1))
                    {
                        switch (manipulationAxis)
                        {
                            case Axes.X:
                                translation = (value >= 0 ? 0.1f * Vector3.right : -0.1f * Vector3.right);
                                newPos += translation;
                                newPos.x = Mathf.Round(newPos.x * 10) / 10;
                                break;
                            case Axes.Y:
                                translation = (value >= 0 ? 0.1f * Vector3.up : -0.1f * Vector3.up);
                                newPos += translation;
                                newPos.y = Mathf.Round(newPos.y * 10) / 10;
                                break;
                            case Axes.Z:
                                translation = (value >= 0 ? 0.1f * Vector3.forward : -0.1f * Vector3.forward);
                                newPos += translation;
                                newPos.z = Mathf.Round(newPos.z * 10) / 10;
                                break;
                            default:
                                Debug.Log("Can't translate on those axes");
                                translation = new Vector3();
                                break;
                        }
                        isRemainStick = true;
                    }
                }
                else
                {
                    switch (manipulationAxis)
                    {
                        case Axes.X:
                            translation = 0.05f * value * Vector3.right;
                            break;
                        case Axes.Y:
                            translation = 0.05f * value * Vector3.up;
                            break;
                        case Axes.Z:
                            translation = 0.05f * value * Vector3.forward;
                            break;
                        default:
                            Debug.Log("Can't translate on those axes");
                            translation = new Vector3();
                            break;
                    }
                    newPos += translation;
                }
                selectedObject.SetPosition(newPos);
            }
        }

        /// <summary>
        /// Clamps the grabbed offset.
        /// </summary>
        /// <param name="localOffset">The local offset.</param>
        /// <param name="thumbY">Value of Thumb Y.</param>
        void ClampGrabOffset(ref Vector3 localOffset, float thumbY)
        {
            Vector3 projectedGrabOffset = localOffset + Vector3.forward * thumbY * 0.05f;
            if (projectedGrabOffset.z > 0.1f)
            {
                localOffset = projectedGrabOffset;
            }
        }

        /// <summary>
        /// Manage the input when in wall creation state.
        /// The Index Trigger is used to create the points and validate them.
        /// The B button is used to delete a point or quit when there is no point.
        /// Pressing The Hand Trigger and the B button at the same time quit the creation.
        /// </summary>
        /// <param name="controllerPos">Vector for controller position.</param>
        private void ManageWallCreationInputs(Vector3 controllerPos)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
            {
                if (points.Count < 3)
                {
                    GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    g.transform.position = controllerPos;
                    g.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                    points.Add(g);
                }
                else
                {
                    state = ControlState.NothingSelected;
                }
            }
            else if (OVRInput.GetDown(OVRInput.Button.Two, controller) && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, controller))
            {
                if (points.Count > 0)
                {
                    int lastIndex = points.Count - 1;
                    Destroy(points[lastIndex]);
                    points.RemoveAt(lastIndex);
                }
                state = ControlState.NothingSelected;
            }
            else if (OVRInput.GetDown(OVRInput.Button.Two, controller))
            {
                if (points.Count > 0)
                {
                    int lastIndex = points.Count - 1;
                    Destroy(points[lastIndex]);
                    points.RemoveAt(lastIndex);
                }
                else state = ControlState.NothingSelected;
            }

        }


        /// <summary>
        /// Detects whether a ray intersects a RectTransform and if it does also
        /// returns the world position of the intersection.
        /// </summary>
        /// <param name="rectTransform">The rect transform to test the intersection with.</param>
        /// <param name="ray">The ray to test the interaction with.</param>
        /// <returns>Boolean true if the ray intersects the rect transform, false otherwise.</returns>
        static bool RayIntersectsRectTransform(RectTransform rectTransform, Ray ray)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            Plane plane = new Plane(corners[0], corners[1], corners[2]);

            float enter;
            if (!plane.Raycast(ray, out enter))
            {
                return false;
            }

            Vector3 intersection = ray.GetPoint(enter);

            Vector3 BottomEdge = corners[3] - corners[0];
            Vector3 LeftEdge = corners[1] - corners[0];
            float BottomDot = Vector3.Dot(intersection - corners[0], BottomEdge);
            float LeftDot = Vector3.Dot(intersection - corners[0], LeftEdge);
            if (BottomDot < BottomEdge.sqrMagnitude && // Can use sqrMag because BottomEdge is not normalized
                LeftDot < LeftEdge.sqrMagnitude &&
                    BottomDot >= 0 &&
                    LeftDot >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

