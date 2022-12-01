using Siccity.GLTFUtility;
using System;
using UnityEngine;
using System.Threading;

namespace EVA
{
    /// <summary>
    /// Class defining behaviours of imported 3D model artworks and initializing it with a gltf file from persistent data path.
    /// </summary>
    /// <remarks>
    /// This script is attached to a prefab composed of : an empty object.
    /// The imported model's GameObject will be attached to this prefab as a child.
    /// </remarks>
    public class Model3D : Artwork
    {
        /// <summary>
        /// Initialize the prefab by creating the child GameObject from the <a href="https://github.com/Siccity/GLTFUtility">GLTFUtility</a> loading method, by giving a gltf file path.
        /// </summary>
        protected override void InitArtwork()
        {
            Interlocked.Increment(ref Creator.semaphore);
            Importer.LoadFromFileAsync(Path, new ImportSettings(), onFinishAsync);
            
        }

        /// <summary>
        /// The callback called when the 3D model mesh importation is finished, setting the model position, rotation, and outline.
        /// </summary>
        /// <param name="Model">The generated model.</param>
        /// <param name="arg2">The animation clip.</param>
        private void onFinishAsync(GameObject Model, AnimationClip[] arg2)
        {
            if(Model is object)
            {
                Model.transform.parent = gameObject.transform;
                Model.transform.localPosition = Vector3.zero;
                Model.transform.localRotation = Quaternion.identity;
                Model.transform.localScale = Vector3.one;
                foreach (Renderer renderer in Model.GetComponentsInChildren<Renderer>())
                {
                    GameObject obj = renderer.gameObject;
                    MeshCollider collider = obj.AddComponent<MeshCollider>();
                    collider.convex = true;
                    if (renderer is SkinnedMeshRenderer)
                    {
                        var sRenderer = (SkinnedMeshRenderer)renderer;
                        collider.sharedMesh = sRenderer.sharedMesh;
                    }
                }
                Outline outline = gameObject.AddComponent<Outline>();
                outline.enabled = false;
                outline.OutlineWidth = 5.0f;
                outline.OutlineColor = new Color32(64, 130, 114, 255);
                Interlocked.Decrement(ref Creator.semaphore);
            }
            else
            {
                Interlocked.Decrement(ref Creator.semaphore);
                throw new ArgumentException("This extension isn't supported !");
            }
        }

        /// <summary>
        /// Returns the serialized 3d model.
        /// </summary>
        /// <returns>Downclassed SerialiazedModel in SerializedObject.</returns>
        public override SerializedObject GetSerializedObject()
        {
            return new SerializedModel(this);
        }

        /// <summary>
        /// Data class to serialize 3d model.
        /// Saves the properties in Model3D.
        /// </summary>
        [System.Serializable]
        public class SerializedModel : EVA.Artwork.SerializedArtWork
        {
            /// <summary>
            /// Default constructor for deserialization.
            /// </summary>
            public SerializedModel() { }

            /// <summary>
            /// Constructor to create the serialized 3d model from the 3d model in parameter.
            /// </summary>
            /// <param name="model">The Model3D to be serialized.</param>
            public SerializedModel(Model3D model) : base(model)
            {
                objectType = ObjectType.MODEL3D;
            }

            /// <summary>
            /// Modify the properties of the Model3D component on the gameobject in parameter,
            /// with the data saved in the attributes. 
            /// For now, there are no useful attributes to be saved so it only calls the base method.
            /// </summary>
            /// <param name="gameObject">GameObject containing a Video360 component.</param>
            public override void Load(GameObject gameObject)
            {
                base.Load(gameObject);
            }
        }

    }
}
