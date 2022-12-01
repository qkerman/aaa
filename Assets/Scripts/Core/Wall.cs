using UnityEngine;

namespace EVA
{
    /// <summary>
    /// Class defining behaviours of walls.
    /// </summary>
    /// <remarks>
    /// This script is attached to a prefab composed of : an object witht an Outline component, and a child with a mesh of a thin cube with Renderer and Collider.
    /// </remarks>
    public class Wall : Object
    {
        /// <summary>
        /// GameObject which is a child of the object containing this script. Contains the mesh and collider. 
        /// </summary>
        public GameObject plane;
        
        /// <summary>
        /// Method called to scale the object from a step and towards an axis.
        /// Override to not scale in the Z axis.
        /// </summary>
        /// <param name="pas">Step of the scale, the value added to the actual scale of the object.</param>
        /// <param name="axe">The axis for the scale.</param>
        public override void Resize(float pas, Axes axe)
        {
            switch (axe)
            {
                case Axes.X:
                    plane.transform.localScale = Vector3.Scale(plane.transform.localScale, new Vector3(1 + pas, 1, 1));
                    break;
                case Axes.Y:
                    plane.transform.localScale = Vector3.Scale(plane.transform.localScale, new Vector3(1, 1 + pas, 1));
                    break;
                case Axes.Z:
                    break;
                case Axes.ALL:
                    plane.transform.localScale = Vector3.Scale(plane.transform.localScale, new Vector3(1 + pas, 1 + pas, 1));
                    break;
            }
        }


        /// <summary>
        /// Define the behaviour of a wall depending mode (mesh renderer enabled only if the mode is Editor mode).
        /// </summary>
        /// <param name="mode">The new interaction mode.</param>
        public override void ChangeMode(InteractionMode mode)
        {
            base.ChangeMode(mode);
            plane.GetComponent<MeshRenderer>().enabled = (mode == InteractionMode.EDITOR);
        }


        /// <summary>
        /// Set the wall on a plane in 3D space, using 3 points.
        /// </summary>
        /// <param name="A">First point.</param>
        /// <param name="B">Second point.</param>
        /// <param name="C">Third point.</param>
        /// <param name="position">The position.</param>
        public void SetPlane(Vector3 A, Vector3 B, Vector3 C, Vector3 position)
        {
            Vector3 AB = B - A;
            Vector3 AC = C - A;
            Vector3 N = Vector3.Cross(AB, AC).normalized;
            float dot = Vector3.Dot(N, position - A);
            if (dot < 0)
            {
                N = -N;
            }
            Debug.Log(N);
            transform.position = A;
            transform.forward = N;           
        }

        /// <summary>
		/// Returns the serialized wall.
		/// </summary>
		/// <returns>Downclassed SerialiazedWall in SerializedObject.</returns>
        public override SerializedObject GetSerializedObject()
        {
            return new SerializedWall(this);
        }

        /// <summary>
		/// Data class to serialize walls.
		/// Saves the properties in Wall.
		/// </summary>
        [System.Serializable]
        public class SerializedWall : EVA.Object.SerializedObject
        {
            /// <summary>
            /// Constructor of a serialized wall from nothing, does nothing.
            /// </summary>
            public SerializedWall() { }
            /// <summary>
            /// Constructor of a serialized wall from a wall.
            /// </summary>
            /// <param name="wall">The wall that will be serialized.</param>
            public SerializedWall(Wall wall) : base(wall)
            {
                objectType = ObjectType.WALL;
            }

            /// <summary>
			/// Modify the properties of the Wall component of the gameobject in parameter,
			/// with the data saved in the attributes.
			/// </summary>
			/// <param name="gameObject">GameObject containing an Object component.</param>
            public override void Load(GameObject gameObject)
            {
                base.Load(gameObject);
            }
        }

    }
}