using System.Collections.Generic;
using UnityEngine;

namespace EVA
{
	/// <summary>
	/// Class describing an object of the gallery, with its caracteristics.
	/// </summary>
	public abstract class Object : MonoBehaviour
	{
		/// <summary>
		/// List of prefabs needed to build a modular menu from this object.
		/// </summary>
		public List<GameObject> associatedModularPrefabs;
		/// <summary>
		/// Lock the position of the object.
		/// </summary>
		public bool LockPos = false;
		/// <summary>
		/// Lock the rotation of the object.
		/// </summary>
		public bool LockRot = false;
		/// <summary>
		/// Lock the scale of the object.
		/// </summary>
		public bool LockScale = false;
		/// <summary>
		/// Property for the gravity of an object.
		/// <returns>
		/// The getter of the property return the isKinematic setting of the object's Rigidbody.
		/// </returns>
		/// <value name="value">
		/// The setter of the property sets the isKinematic setting of the object's Rigidbody, using the given boolean value.
		/// </value>
		/// </summary>
		public bool Gravity
		{
			get
			{
				return GetComponent<Rigidbody>().useGravity;
			}

			set
			{
				GetComponent<Rigidbody>().useGravity = value;
			}
		}

		/// <summary>
		/// Property for the layer of an object.
		/// <returns>
		/// The getter of the property return the actual layer of the object.
		/// </returns>
		/// <value name="value">
		/// The setter of the property sets the layer of the object, using the given integer (layer number) value.
		/// </value>
		/// </summary>
		public int Layer
		{
			get
			{
				return gameObject.layer;
			}

			set
			{
				gameObject.layer = value;
			}
		}

		/// <summary>
		/// Property for the active state of an object.
		/// <returns>
		/// The getter of the property return the activeSelf setting of the object.
		/// </returns>
		/// <value name="value">
		/// The setter of the property sets the active setting of the object's Rigidbody, using the given boolean value.
		/// </value>
		/// </summary>
		public bool Active
		{
			get
			{
				return gameObject.activeSelf;
			}

			set
			{
				gameObject.SetActive(value);
			}
		}

		/// <summary>
		/// Method called to rotate an object from a step towards an axis.
		/// </summary>
		/// <param name="pas">Step of the rotation, the value added to the rotation.</param>
		/// <param name="axe">Axis for the rotation.</param>
		public virtual void Rotate(float pas, Axes axe)
		{
			if (!LockRot)
			{
                switch (axe)
				{
					case Axes.X:
						transform.Rotate(pas, 0, 0);
						break;
					case Axes.Y:
						transform.Rotate(0, pas, 0);
                        break;
					case Axes.Z:
						transform.Rotate(0, 0, pas);
                        break;
					case Axes.ALL:
						transform.Rotate(pas, pas, pas);
                        break;
				}
			}

		}
		/// <summary>
		/// Method called to scale the object from a step and towards an axis.
		/// </summary>
		/// <param name="pas">Step of the scale, the value added to the actual scale of the object.</param>
		/// <param name="axe">The axis for the scale.</param>
		public virtual void Resize(float pas, Axes axe)
		{
			if (!LockScale)
			{
				switch (axe)
				{
					case Axes.X:
						transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1 + pas, 1, 1));
						break;
					case Axes.Y:
						transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1, 1 + pas, 1));
						break;
					case Axes.Z:
						transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1, 1, 1 + pas));
						break;
					case Axes.ALL:
						transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1 + pas, 1 + pas, 1 + pas));
						break;
				}
			}
		}

		/// <summary>
		/// Method called to set a rotation to the object, using a Quaternion.
		/// </summary>
		/// <param name="rotation">The Quaternion to be set as the new rotation.</param>
		public virtual void SetRotation(Quaternion rotation)
		{
			if (!LockRot)
			{
				transform.localRotation = rotation;
			}
		}

		/// <summary>
		/// Set the position of an object to the vector in parameter.
		/// </summary>
		/// <param name="newPos">The new position of the object.</param>
		public virtual void SetPosition(Vector3 newPos)
		{
			if (!LockPos)
			{
				transform.position = newPos;
			}
		}

		/// <summary>
		/// Set the scale of an object to the vector in parameter.
		/// </summary>
		/// <param name="scale">The new ccale of the object.</param>
		public virtual void SetScale(Vector3 scale)
		{
			if (!LockScale)
			{
				transform.localScale = scale;
			}
		}

		/// <summary>
		/// Method that moves the object, translating it from an offset.
		/// </summary>
		/// <param name="offset">Offset for translation, is a Vector3 containing distance to add for each axis.</param>
		public virtual void Translate(Vector3 offset)
		{
			if (!LockPos)
			{
				transform.Translate(offset);
			}
		}

		/// <summary>
		/// Grab the object by setting his parent to the GameObject in parameter.
		/// </summary>
		/// <param name="obj">The new parent of the object.</param>
		public virtual void ChangeParent(GameObject obj)
		{
			if(!(LockPos || LockRot))
				transform.SetParent(obj.transform, true);
		}
		
		/// <summary>
		/// Define how the object works depending on the mode (for example, hiding meshes for audio artworks).
		/// The Objects including the <see cref="EVA.HasSound"/> have their Play method launched on Visitor modes and Stop method launched on Editor mode.
		/// </summary>
		/// <param name="mode">The new gallery mode.</param>
		public virtual void ChangeMode(InteractionMode mode)
        {
            if (this is HasSound conversion)
            {
                switch (mode)
                {
                    case InteractionMode.VISITOR_ONLY:
                    case InteractionMode.VISITOR:
                        conversion.Play();
                        break;
                    case InteractionMode.EDITOR:
                        conversion.Stop();
                        break;
                }
            }
        }

		/// <summary>
		/// Method that gives the serialized version of the object, for saving purpose.
		/// </summary>
		/// <returns>The generated serialized object.</returns>
		public abstract SerializedObject GetSerializedObject();


		/// <summary>
		/// Base class of the serialized objects.
		/// Save the data of the transform and the properies in Object.
		/// </summary>
		[System.Serializable]
		public class SerializedObject
		{
			/// <summary>
			/// Instance id of the object.
			/// </summary>
			public int instanceId;

			/// <summary>
			/// Type of the object.
			/// </summary>
			public ObjectType objectType;

			/// <summary>
			/// World position of the gameobject.
			/// </summary>
			public Vector3 position;

			/// <summary>
			/// World rotation of the gameobject.
			/// </summary>
			public Quaternion rotation;

			/// <summary>
			/// Local scale of the gameobject.
			/// </summary>
			public Vector3 scale;

			/// <summary>
			/// Value of the LockPos property.
			/// </summary>
			public bool lockPos;

			/// <summary>
			/// Value of the LockRot property.
			/// </summary>
			public bool lockRot;

			/// <summary>
			/// Value of the LockScale property.
			/// </summary>
			public bool lockScale;

			/// <summary>
			/// Value of the Gravity property.
			/// </summary>
			public bool gravity;

			/// <summary>
			/// Value of Layer property.
			/// </summary>
			public int layer;

			/// <summary>
			/// Value of the Active property.
			/// </summary>
			public bool active;

			/// <summary>
			/// Default constructor for deserialization.
			/// </summary>
			public SerializedObject() { }

			/// <summary>
			/// Constructor to create the serialized object from the object in parameter.
			/// </summary>
			/// <param name="evaObject">The Object to be serialized.</param>
			public SerializedObject(Object evaObject)
			{
				this.instanceId = evaObject.gameObject.GetInstanceID();
				this.objectType = ObjectType.IMAGE;
				this.position = evaObject.transform.position;
				this.rotation = evaObject.transform.rotation;
				this.scale = evaObject.transform.localScale;
				this.lockPos = evaObject.LockPos;
				this.lockRot = evaObject.LockRot;
				this.lockScale = evaObject.LockScale;
				this.gravity = evaObject.Gravity;
				this.layer = evaObject.Layer;
				this.active = evaObject.Active;

			}

			/// <summary>
			/// Modify the properties of the Object component of the gameobject in parameter,
			/// with the data saved in the attributes.
			/// </summary>
			/// <param name="gameObject">GameObject containing an Object component.</param>
			public virtual void Load(GameObject gameObject)
			{
				Object evaobject = gameObject.GetComponent<EVA.Object>();
				evaobject.transform.position = position;
				evaobject.transform.rotation = rotation;
				evaobject.transform.localScale = scale;
				evaobject.LockPos = lockPos;
				evaobject.LockRot = lockRot;
				evaobject.LockScale = lockScale;
				evaobject.Gravity = gravity;
				evaobject.Layer = layer;
				evaobject.Active = active;
			}
		}

	}
}
