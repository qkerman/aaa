using System;
using UnityEngine;
namespace EVA
{
	/// <summary>
	/// This class is managing the lights (attached to a prefab containing a light component).
	/// </summary>
	public class Light : Object
	{
		/// <summary>
		/// GameObject that represents the model of a directionnal light (typically a cone).
		/// </summary>
		public GameObject dirModel;
		/// <summary>
		/// GameObject that represents the model of a spot light (typically a cylinder or a torch 3D model).
		/// </summary>
		public GameObject spotModel;
		/// <summary>
		/// GameObject that represents the model of a point light (typically a sphere).
		/// </summary>
		public GameObject pointModel;

		/// <summary>
		/// Property that represents the color of the light (also affects the 3D model EmissionColor).
		/// </summary>
		public Color Color
        {
            get => gameObject.GetComponent<UnityEngine.Light>().color;
            set
            {
				gameObject.GetComponent<UnityEngine.Light>().color = value;
				dirModel.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", value);
				pointModel.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", value);
				spotModel.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", value);

			}
        }

        /// <summary>
        /// This property manages the range of the light.
        /// </summary>
        public float Range
		{
			get => GetComponent<UnityEngine.Light>().range;
			set => GetComponent<UnityEngine.Light>().range = value;
		}

		/// <summary>
		/// This property manages the spot angle of the light.
		/// </summary>
		public float SpotAngle
		{
			get => GetComponent<UnityEngine.Light>().spotAngle;
			set => GetComponent<UnityEngine.Light>().spotAngle = value;
		}

		/// <summary>
		/// This property manages the intensity of the light.
		/// </summary>
		public float Intensity
		{
			get => GetComponent<UnityEngine.Light>().intensity;
			set => GetComponent<UnityEngine.Light>().intensity = value;
		}

		/// <summary>
		/// This property manages the mask of the objects this light should impact. Also add the light to the given layer, so that its mesh is always lighten.
		/// <remarks>This property is a culling mask and not a layer, to make a culling mask of a specific layer, give as an parameter : (1 << layer) instead of the layer only.</remarks>
		/// </summary>
		public LayerMask CullingMask
		{
			get => GetComponent<UnityEngine.Light>().cullingMask;
			set => GetComponent<UnityEngine.Light>().cullingMask = value;
		}

		/// <summary>
		/// Property that represents the visibility of the 3D model representing the light.
		/// </summary>
		public bool ModelVisibility
        {
            get => (dirModel.activeSelf || pointModel.activeSelf || spotModel.activeSelf);
			set
			{
				switch(Type)
				{
					case LightType.Directional:
						dirModel.SetActive(value);
						break;
					case LightType.Spot:
						spotModel.SetActive(value);
						break;
					case LightType.Point:
						pointModel.SetActive(value);
						break;
					default:
						Debug.Log("No model to activate for this type of light");
						break;
				}
			}
		}

		/// <summary>
		/// This property manages the type of the light.
		/// </summary>
		public LightType Type
		{
			get => GetComponent<UnityEngine.Light>().type;
			set
			{
                switch (value)
                {
					case LightType.Spot:
					case LightType.Directional:
					case LightType.Point:
						bool visib = ModelVisibility;
						GetComponent<UnityEngine.Light>().type = value;
						dirModel.SetActive(false);
						pointModel.SetActive(false);
						spotModel.SetActive(false);
						ModelVisibility = visib;
						break;
					default:
						throw new ArgumentException("This type of light is not supported !");
				}
				
			}
		}

		/// <summary>
		/// Set the light on or off.
		/// </summary>
		public bool Switch
        {
            get => GetComponent<UnityEngine.Light>().enabled;
            set
            {
				GetComponent<UnityEngine.Light>().enabled = value;
				dirModel.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", value ? GetComponent<UnityEngine.Light>().color : Color.black);
				pointModel.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", value ? GetComponent<UnityEngine.Light>().color : Color.black);
				spotModel.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", value ? GetComponent<UnityEngine.Light>().color : Color.black);
			}
        }

		/// <summary>
		/// Add a layer to the cullingmask of the light.
		/// </summary>
		/// <param name="layer">The layer to add.</param>
		public void AddCullingMaskLayer(int layer)
        {
			CullingMask |= (1 << layer);
		}

		/// <summary>
		/// Remove a layer from the cullingmask of the light.
		/// </summary>
		/// <param name="layer">The layer to remove.</param>
		public void RemoveCullingMaskLayer(int layer)
		{
			CullingMask &= ~(1 << layer);
		}

		/// <summary>
		/// Set the culling mask to everything, so that it affects everything on the scene.
		/// </summary>
		public void SetCullingMaskToEverything()
        {
			CullingMask = -1;
        }

		/// <summary>
		/// Set the culling mask to Nothing, so that it affects nothing on the scene.
		/// </summary>
		public void SetCullingMaskToNothing()
        {
			CullingMask = 0;
        }

		/// <summary>
		/// Changes the mode of the light, no modelvisibility when in any visitor mode.
		/// </summary>
		/// <param name="mode">The new gallery mode.</param>
        public override void ChangeMode(InteractionMode mode)
        {
			base.ChangeMode(mode);
            switch (mode)
            {
				case InteractionMode.VISITOR_ONLY:
				case InteractionMode.VISITOR:
					ModelVisibility = false;
					break;
				case InteractionMode.EDITOR:
					ModelVisibility = true;
					break;
            }
        }
		
		/// <summary>
		/// Returns the serialized light.
		/// </summary>
		/// <returns>Downclassed SerialiazedLight in SerializedObject.</returns>
		public override SerializedObject GetSerializedObject()
		{
			return new SerializedLight(this);
		}

		/// <summary>
		/// Data class to serialize lights.
		/// Saves the properties in EVA.Light.
		/// </summary>
		[System.Serializable]
		public class SerializedLight : EVA.Object.SerializedObject
		{
			/// <summary>
			/// Value of the Color property.
			/// </summary>
			public Color color;

			/// <summary>
			/// Value of the Range property.
			/// </summary>
			public float range;

			/// <summary>
			/// Value of the SpotAngle property.
			/// </summary>
			public float spotAngle;

			/// <summary>
			/// Value of the Intensity property.
			/// </summary>
			public float intensity;

			/// <summary>
			/// Value of the CullingMask property.
			/// </summary>
			public int cullingMask;

			/// <summary>
			/// Value of the ModelVisibility property.
			/// </summary>
			public bool modelVisibility;

			/// <summary>
			/// Value of the LightType property.
			/// </summary>
			public LightType lightType;

			/// <summary>
			/// Value of the switch property.
			/// </summary>
			public bool state;

			/// <summary>
			/// Default constructor for deserialization.
			/// </summary>
			public SerializedLight() { }

			/// <summary>
			/// Constructor to create the serialized light from the light in parameter.
			/// </summary>
			/// <param name="light">The light to be serialized.</param>
			public SerializedLight(Light light) : base(light)
			{
				objectType = ObjectType.LIGHT;
				color = light.Color;
				range = light.Range;
				spotAngle = light.SpotAngle;
				intensity = light.Intensity;
				cullingMask = light.CullingMask;
				modelVisibility = light.ModelVisibility;
				lightType = light.Type;
				state = light.Switch;
			}

			/// <summary>
			/// Modify the properties of the Light component on the gameobject in parameter,
			/// with the data saved in the attributes.
			/// </summary>
			/// <param name="gameObject">GameObject containing an Light component.</param>
			public override void Load(GameObject gameObject)
			{
				base.Load(gameObject);
				Light light = gameObject.GetComponent<Light>();
				light.Color = color;
				light.Range = range;
				light.SpotAngle = spotAngle;
				light.Intensity = intensity;
				light.CullingMask = cullingMask;
				light.ModelVisibility = modelVisibility;
				light.Type = lightType;
				light.Switch = state;
			}
		}
	}
}
