using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace EVA
{
    /// <summary>
    /// SaveManager component to be put on the gallery gameobject.
    /// In charge saving and loading the gallery.
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        /// <summary>
        /// Prefab to instantiate when loading an image.
        /// </summary>
        public GameObject imagePrefab;

        /// <summary>
        /// Prefab to instantiate when loading a video.
        /// </summary>
        public GameObject videoPrefab;

        /// <summary>
        /// Prefab to instantiate when loading a 360° video.
        /// </summary>
        public GameObject video360Prefab;

        /// <summary>
        /// Prefab to instantiate when loading a 3d model.
        /// </summary>
        public GameObject modelPrefab;

        /// <summary>
        /// Prefab to instantiate when loading a sound.
        /// </summary>
        public GameObject soundPrefab;

        /// <summary>
        /// Prefab to instantiate when loading a light.
        /// </summary>
        public GameObject lightPrefab;

        /// <summary>
        /// Prefab to instantiate when loading a wall.
        /// </summary>
        public GameObject wallPrefab;

        /// <summary>
        /// Dictionary containing the loaded gameobjects with their old instanceIds as key.
        /// Used to pin the images and videos to the same walls they were pinned to. 
        /// </summary>
        private Dictionary<int, GameObject> oldIds = new Dictionary<int, GameObject>();

        /// <summary>
        /// Creates an instance of SaveData with this.gameobject (the gallery) as parameter, to construct all the serialized objects.
        /// Saves the savedata as json in the file indicated by the path parameter.
        /// </summary>
        /// <param name="path">Path of the file to put the json in.</param>
        public void Save(string path)
        {
            if (Creator.semaphore == 0)
            {
                if (EVA.FileChooser.IsFolderExists(System.IO.Path.GetDirectoryName(path)) && System.IO.Path.GetExtension(path).ToLower() == ".eva")
                {
                    Interlocked.Increment(ref Creator.semaphore);
                    SaveData saveData = new SaveData(gameObject);
                    string json = JsonUtility.ToJson(saveData);
                    System.IO.File.WriteAllText(path, json);
                    Interlocked.Decrement(ref Creator.semaphore);
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }
            else
            {
                Debug.Log("Can't save when importing an object.");
            }
        }

        /// <summary>
        /// Reads the file indicated by the path parameter in a string.
        /// Creates an instance of SaveData from the json.
        /// Destroy all the children of the gallery and calls AfterDeserialize.
        /// </summary>
        /// <param name="path">The loading path.</param>
        public void Load(string path)
        {
            if (Creator.semaphore == 0)
            {
                if (EVA.FileChooser.IsFileExists(path) && System.IO.Path.GetExtension(path).ToLower() == ".eva")
                {
                    Interlocked.Increment(ref Creator.semaphore);
                    string json = System.IO.File.ReadAllText(path);
                    SaveData saveData = JsonUtility.FromJson<SaveData>(json);
                    foreach (Transform child in transform)
                    {
                        Destroy(child.gameObject);
                    }
                    AfterDeserialize(saveData);
                    Interlocked.Decrement(ref Creator.semaphore);
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }
            else
            {
                Debug.Log("Can't load when importing an object.");
            }
        }

        /// <summary>
        /// Coroutine to set the parent of the prefab in parameter to the gameobject linked to the oldParentId in parameter.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="oldParentId">The Id of the parent before save.</param>
        /// <returns></returns>
        private IEnumerator PinToWall(GameObject prefab, int oldParentId)
        {
            yield return new WaitUntil(() => oldIds.ContainsKey(oldParentId));
            prefab.transform.parent = oldIds[oldParentId].transform;
        }

        /// <summary>
        /// Method to load every objects contained in the SaveData depending on their type.
        /// </summary>
        /// <param name="saveData">SaveData containing the list of jsons to load.</param>
        private void AfterDeserialize(SaveData saveData)
        {
            oldIds.Clear();   
            foreach (string json in saveData.serializedObjectsJson)
            {
                //Deserialize the json a first time as the base class SerializeObject to be able to get the type of the object.
                EVA.Object.SerializedObject serializedObject = JsonUtility.FromJson<EVA.Object.SerializedObject>(json);
                EVA.Object.SerializedObject serialized;
                GameObject prefab;
                //depending on the type, we instantiate the corresponding prefab, deserialize again but as the corresponding derived class the call the Load method of the serialized object.
                //if the object is an image or a video, we check if it was pinned and in this case reattach it to the wall by calling the PinToWall coroutine.
                switch (serializedObject.objectType)
                {
                    case EVA.ObjectType.IMAGE:
                        prefab = Instantiate(imagePrefab, gameObject.transform);
                        serialized = JsonUtility.FromJson<EVA.Image.SerializedImage>(json);
                        serialized.Load(prefab);
                        EVA.Image.SerializedImage serializedImage = ((EVA.Image.SerializedImage)serialized);
                        if (serializedImage.isPinned)
                            StartCoroutine(PinToWall(prefab, serializedImage.parentId));
                        oldIds.Add(serializedObject.instanceId, prefab);
                        break;
                    case EVA.ObjectType.VIDEO:
                        prefab = Instantiate(videoPrefab, gameObject.transform);
                        serialized = JsonUtility.FromJson<EVA.Video.SerializedVideo>(json);
                        serialized.Load(prefab);
                        EVA.Video.SerializedVideo serializedVideo = ((EVA.Video.SerializedVideo)serialized);
                        if (serializedVideo.isPinned)
                            StartCoroutine(PinToWall(prefab, serializedVideo.parentId));
                        oldIds.Add(serializedObject.instanceId, prefab);
                        break;
                    case EVA.ObjectType.VIDEO360:
                        prefab = Instantiate(video360Prefab, gameObject.transform);
                        serialized = JsonUtility.FromJson<EVA.Video360.SerializedVideo360>(json);
                        serialized.Load(prefab);
                        oldIds.Add(serializedObject.instanceId, prefab);
                        break;
                    case EVA.ObjectType.MODEL3D:
                        prefab = Instantiate(modelPrefab, gameObject.transform);
                        serialized = JsonUtility.FromJson<EVA.Model3D.SerializedModel>(json);
                        serialized.Load(prefab);
                        oldIds.Add(serializedObject.instanceId, prefab);
                        break;
                    case EVA.ObjectType.WALL:
                        prefab = Instantiate(wallPrefab, gameObject.transform);
                        serialized = JsonUtility.FromJson<EVA.Wall.SerializedWall>(json);
                        serialized.Load(prefab);
                        oldIds.Add(serializedObject.instanceId, prefab);
                        break;
                    case EVA.ObjectType.LIGHT:
                        prefab = Instantiate(lightPrefab, gameObject.transform);
                        serialized = JsonUtility.FromJson<EVA.Light.SerializedLight>(json);
                        serialized.Load(prefab);
                        oldIds.Add(serializedObject.instanceId, prefab);
                        break;
                    case EVA.ObjectType.SOUND:
                        prefab = Instantiate(soundPrefab, gameObject.transform);
                        serialized = JsonUtility.FromJson<EVA.Sound.SerializedSound>(json);
                        serialized.Load(prefab);
                        oldIds.Add(serializedObject.instanceId, prefab);
                        break;
                }
            }
        }

        /// <summary>
        /// Internal data class to be serialized.
        /// Implement the ISerializationCallbackReceiver interface, to call the OnBeforeSerialize method.
        /// </summary>
        class SaveData 
        {
            /// <summary>
            /// List of string containing the json of each serialized object.
            /// </summary>
            public List<string> serializedObjectsJson = new List<string>();


            /// <summary>
            /// Constructor that Add the json generated by the serialized object of each Object component in children, to the list.
            /// </summary>
            /// <param name="gameObject">The GameObject to save.</param>
            public SaveData(GameObject gameObject)
            {
                EVA.Object[] objects = gameObject.GetComponentsInChildren<EVA.Object>();
                foreach (EVA.Object evaobject in objects)
                {
                    serializedObjectsJson.Add(JsonUtility.ToJson(evaobject.GetSerializedObject()));
                }
            }
        }
    }
}
