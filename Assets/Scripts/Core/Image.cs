using System.IO;
using UnityEngine;
using System;

namespace EVA
{
    /// <summary>
    /// Class defining behaviours of imported image artworks and initializing it with a image path from persistent data path.
    /// </summary>
    /// <remarks>
    /// This script is attached to a prefab composed of : an empty object, and two plans child with a Picture Material and a Renderer.
    /// Each child with a Picture Material will display imported image.
    /// </remarks>
    public class Image : Artwork, IPinnable
    {
        /// <summary>
        /// GameObject which is a child of the object containing this script. This child displays front image.
        /// </summary>
        public GameObject Recto;
        /// <summary>
        /// GameObject which is a child of the object containing this script. This child displays the back image, which is the same as the front one.
        /// </summary>
        public GameObject Verso;
        /// <summary>
        /// Boolean useful to know if the image is pinned to a wall.
        /// </summary>
        public bool IsPinned => transform.parent.GetComponent<Wall>() is object;

        /// <summary>
        /// Function that takes a wall and a direction as parameters and that allows pinning to the wall an image.
        /// </summary>
        /// <param name="wall">Wall to set as parent, and to pin the image on.</param>
        /// <param name="direction">Direction of the laser pointer.</param>
        public void Pin(Wall wall, Vector3 direction)
        {
            transform.SetParent(wall.transform);
            float dot = Vector3.Dot(wall.transform.forward, direction);
            Quaternion rotation;
            float zRotation = transform.localEulerAngles.z;
            if (dot > 0)
            {
                rotation = Quaternion.identity;
                transform.localRotation = rotation;
                transform.Rotate(new Vector3(0, 0, zRotation));
                transform.localPosition.Set(0.01f, 0, 0);
            }
            else
            {
                rotation = new Quaternion(0, 1, 0, 0);
                transform.localRotation = rotation;
                transform.Rotate(new Vector3(0, 0, zRotation));
                transform.localPosition.Set(-0.01f, 0, 0);
            }
        }

        /// <summary>
		/// Method that unpins the video of its wall, putting a new parent.
		/// </summary>
		/// <param name="parent">The new parent of the video.</param>
        public void Unpin(GameObject parent)
        {
            transform.SetParent(parent.transform);
        }

        /// <summary>
        /// Initialize the prefab and components with the file given in the path.
        /// Mainly setting the texture used by the Picture Material, and setting the plan's renderer.
        /// </summary>
        protected override void InitArtwork()
        {
            byte[] bytes = File.ReadAllBytes(Path);
            Texture2D texture = new Texture2D(900, 900, TextureFormat.RGB24, false)
            {
                filterMode = FilterMode.Point
            };
            texture.LoadImage(bytes);
            texture.Apply();
            Debug.Log("Width: " + texture.width);
            Debug.Log("Height: " + texture.height);
            float width = texture.width;
            float height = texture.height;
            float ratio = width / height;
            Recto.transform.localScale= new Vector3(Recto.transform.localScale.x * ratio, Recto.transform.localScale.y, Recto.transform.localScale.z);
            Verso.transform.localScale= new Vector3(Verso.transform.localScale.x * ratio, Verso.transform.localScale.y, Verso.transform.localScale.z);
            Recto.GetComponent<Renderer>().material.mainTexture = texture;
            Verso.GetComponent<Renderer>().material.mainTexture = texture;
        }

        /// <summary>
        /// Image can only change parent when they are not pin to a Wall.
        /// </summary>
        /// <param name="obj">The new parent of the object.</param>
        public override void ChangeParent(GameObject obj)
        {
            if (!transform.parent.GetComponent<Wall>())
            {
                base.ChangeParent(obj);
            }
        }

        /// <summary>
        /// Destroy the duplicated material, accordingly to the recommendation of Unity.
        /// </summary>
        private void OnDestroy()
        {
            Destroy(Recto.GetComponent<Renderer>().material);
            Destroy(Verso.GetComponent<Renderer>().material);
        }

        /// <summary>
        /// Returns the serialized light.
        /// </summary>
        /// <returns>Downclassed SerialiazedImage in SerializedObject.</returns>
        public override SerializedObject GetSerializedObject()
        {
            return new SerializedImage(this);
        }

        /// <summary>
        /// Data class to serialize images.
        /// Saves the properties in Image.
        /// </summary>
        [System.Serializable]
        public class SerializedImage : EVA.Artwork.SerializedArtWork
        {
            /// <summary>
            /// Value of the IsPinned property.
            /// </summary>
            public bool isPinned;

            /// <summary>
            /// InstanceId of the parent gameobject. Used when it is pinned.
            /// </summary>
            public int parentId;

            /// <summary>
            /// Default constructor for deserialization.
            /// </summary>
            public SerializedImage() { }

            /// <summary>
            /// Constructor to create the serialized image from the image in parameter.
            /// </summary>
            /// <param name="image"> Image to be serialized.</param>
            public SerializedImage(Image image) : base(image)
            {
                objectType = ObjectType.IMAGE;
                isPinned = image.IsPinned;
                parentId = image.transform.parent.gameObject.GetInstanceID();
            }

            /// <summary>
            /// Modify the properties of the Image component on the gameobject in parameter, with the data saved in the attributes. For now, it only calls the base method because the parent cannot be set here.
            /// </summary>
            /// <param name="gameObject">GameObject containing an Image component.</param>
            public override void Load(GameObject gameObject)
            {
                base.Load(gameObject);
            }
        }
    }
}
