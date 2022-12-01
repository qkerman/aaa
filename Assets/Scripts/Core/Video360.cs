using UnityEngine;

namespace EVA
{
	/// <summary>
	/// Class defining behaviours of imported 360 video artworks and initializing it with a video path from persistent data path.
	/// </summary>
	/// <remarks>
	/// This script is attached to a prefab composed of : an empty object with an AudioSource, and two plans child with a VideoPlayer.
	/// Each child with a VideoPlayer will display imported video, and the audio source will make the video sound once.
	/// </remarks>
	public class Video360 : Artwork, HasSound
	{
		/// <summary>
		/// Sphere inverted, that shows the video on the inside.
		/// </summary>
		public GameObject invertedSphere;

		/// <summary>
		/// This is a property for sound and video looping, which is a feature from the AudioSource and VideoPlayer components in Unity. This property has a getter and a setter which are modifying the AudioSource and VideoPlayer settings.
		/// </summary>
		/// <returns> The getter of this property returns the actual AudioSource's and VideoPlayer's looping setting. </returns>
		/// <value name = "value"> The setter of this property sets the AudioSource's and VideoPlayer's looping setting as given boolean value. </value>
		public bool Looping
		{
			get
			{
				return invertedSphere.GetComponent<UnityEngine.Video.VideoPlayer>().isLooping && gameObject.GetComponent<AudioSource>().loop;
			}
			set
			{
				invertedSphere.GetComponent<UnityEngine.Video.VideoPlayer>().isLooping = value;
				gameObject.GetComponent<AudioSource>().loop = value;
			}
		}

		/// <summary>
		/// This is a property for sound spatialization, which is a feature from the AudioSource component in Unity. This property has a getter and a setter which are modifying the AudioSource setting.
		/// </summary>
		/// <returns> The getter of this property returns the actual AudioSource's spatialize setting. </returns>
		/// <value name = "value"> The setter of this property sets the AudioSource's spatialize setting as given boolean value. </value>
		public bool SpatializedSound
		{
			get
			{
				return gameObject.GetComponent<AudioSource>().spatialBlend == 1;
			}
			set
			{
				if (value)
				{
					gameObject.GetComponent<AudioSource>().spatialBlend = 1;
				}
				else
				{
					gameObject.GetComponent<AudioSource>().spatialBlend = 0;
				};
			}
		}

		/// <summary>
		/// This is a property for sound volume, which is a feature from the AudioSource component in Unity. This property has a getter and a setter which are modifying the AudioSource setting.
		/// </summary>
		/// <returns> The getter of this property returns the actual AudioSource's volume setting as an integer between 0 and 100, while it's a float from 0f to 1f in the component. </returns>
		/// <value name = "value"> The setter of this property sets the AudioSource's volume setting as given int (actually is a float (from 0f to 1f) in the component, converted to an integer from 0 to 100). </value>
		public int Volume
		{
			get
			{
				return (int)(gameObject.GetComponent<AudioSource>().volume * 100);
			}
			set
			{
				if (value < 0)
					gameObject.GetComponent<AudioSource>().volume = 0.0F;
				else if (value > 100)
					gameObject.GetComponent<AudioSource>().volume = 1.0F;
				else
				{
					gameObject.GetComponent<AudioSource>().volume = 0.01F * value;

				}
			}
		}

		/// <summary>
		/// This is a property to know if the object is playing sound.
		/// </summary>
		/// <returns>A boolean whether the object is playing sound</returns>
        public bool IsPlaying => invertedSphere.GetComponent<UnityEngine.Video.VideoPlayer>().isPlaying;

        /// <summary>
        /// Initialize the prefab and components with the file given in the path.
        /// Mainly setting the url of the VideoPlayer and initializing the AudioSource.
        /// </summary>
        protected override void InitArtwork()
		{
			invertedSphere.GetComponent<UnityEngine.Video.VideoPlayer>().url = Path;
			invertedSphere.GetComponent<UnityEngine.Video.VideoPlayer>().Prepare();
			invertedSphere.GetComponent<UnityEngine.Video.VideoPlayer>().prepareCompleted += SetRenderTexture;
			invertedSphere.GetComponent<UnityEngine.Video.VideoPlayer>().audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
			invertedSphere.GetComponent<UnityEngine.Video.VideoPlayer>().SetTargetAudioSource(0, GetComponent<AudioSource>());
			gameObject.GetComponent<AudioSource>().spatialize = true;
			gameObject.GetComponent<AudioSource>().loop = true;
		}


		/// <summary>
		/// Method that sets the render texture of the sphere.
		/// </summary>
		/// <param name="vPlayer">The VideoPlayer of the sphere.</param>
		private void SetRenderTexture(UnityEngine.Video.VideoPlayer vPlayer)
		{
			RenderTextureDescriptor rtd = vPlayer.targetTexture.descriptor;
			rtd.height = (int)vPlayer.height;
			rtd.width = (int)vPlayer.width;
			RenderTexture rt = new RenderTexture(rtd);
			vPlayer.targetTexture = rt;
			vPlayer.GetComponent<MeshRenderer>().material.mainTexture = rt;

		}

		/// <summary>
		/// Function that plays the video inside the sphere.
		/// </summary>
		public void Play()
		{
			invertedSphere.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
		}
		/// <summary>
		/// Function that pauses the video inside the sphere.
		/// </summary>
		public void Pause()
		{
			invertedSphere.GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
		}
		/// <summary>
		/// Function that stops the video inside the sphere.
		/// </summary>
		public void Stop()
		{
			invertedSphere.GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
		}

		/// <summary>
		/// Returns the serialized 360° video.
		/// </summary>
		/// <returns>Downclassed SerialiazedVideo360 in SerializedObject.</returns>
		public override SerializedObject GetSerializedObject()
		{
			return new SerializedVideo360(this);
		}

		/// <summary>
		/// Data class to serialize 360° videos.
		/// Saves the properties in Video360.
		/// </summary>
		[System.Serializable]
		public class SerializedVideo360 : EVA.Artwork.SerializedArtWork
		{
			/// <summary>
			/// Value of the SpatializeSound property.
			/// </summary>
			public bool spatializeSound;
			
			/// <summary>
			/// Value of the Looping property.
			/// </summary>
			public bool looping;

			/// <summary>
			/// Value of the Volume property.
			/// </summary>
			public int volume;

			/// <summary>
			/// Default constructor for deserialization.
			/// </summary>
			public SerializedVideo360() { }

			/// <summary>
			/// Constructor to create the serialized 360° video from the 360° video in parameter.
			/// </summary>
			/// <param name="video360">The Video360 to be serialized.</param>
			public SerializedVideo360(Video360 video360) : base(video360)
			{
				objectType = ObjectType.VIDEO360;
				spatializeSound = video360.SpatializedSound;
				looping = video360.Looping;
				volume = video360.Volume;
			}

			/// <summary>
			/// Modify the properties of the Video360 component on the gameobject in parameter,
			/// with the data saved in the attributes.
			/// </summary>
			/// <param name="gameObject">GameObject containing a Video360 component.</param>
			public override void Load(GameObject gameObject)
			{
				base.Load(gameObject);
				Video360 video360 = gameObject.GetComponent<Video360>();
				video360.SpatializedSound = spatializeSound;
				video360.Looping = looping;
				video360.Volume = volume;

			}
		}

	}
}
