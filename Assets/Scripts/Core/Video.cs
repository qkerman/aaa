using System.Collections;
using UnityEngine;

namespace EVA
{
	/// <summary>
	/// Class defining behaviours of imported video artworks and initializing it with a video path from persistent data path.
	/// </summary>
	/// <remarks>
	/// This script is attached to a prefab composed of : an empty object with an AudioSource, and two plans child with a VideoPlayer.
	/// Each child with a VideoPlayer will display imported video, and the audio source will make the video sound once.
	/// </remarks>
	public class Video : Artwork, HasSound, IPinnable
	{

		/// <summary>
		/// GameObject which is a child of the object containing this script. This child displays front video.
		/// </summary>
		public GameObject Recto;
		/// <summary>
		/// GameObject which is a child of the object containing this script. This child displays the back video, which is the same as the front one.
		/// </summary>
		public GameObject Verso;

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
		/// This is a property for sound and video looping, which is a feature from the AudioSource and VideoPlayer components in Unity. This property has a getter and a setter which are modifying the AudioSource and VideoPlayer settings.
		/// </summary>
		/// <returns> The getter of this property returns the actual AudioSource's and VideoPlayer's looping setting. </returns>
		/// <value name = "value"> The setter of this property sets the AudioSource's and VideoPlayer's looping setting as given boolean value. </value>
		public bool Looping
		{
			get
			{
				return Recto.GetComponent<UnityEngine.Video.VideoPlayer>().isLooping && Verso.GetComponent<UnityEngine.Video.VideoPlayer>().isLooping && gameObject.GetComponent<AudioSource>().loop;
			}
			set
			{
				Recto.GetComponent<UnityEngine.Video.VideoPlayer>().isLooping = value;
				Verso.GetComponent<UnityEngine.Video.VideoPlayer>().isLooping = value;
				gameObject.GetComponent<AudioSource>().loop = value;
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
		/// Video can only change parent if they are not pin to the wall.
		/// </summary>
		/// <param name="obj">The new parent.</param>
        public override void ChangeParent(GameObject obj)
        {
            if (!transform.parent.GetComponent<Wall>())
            {
				base.ChangeParent(obj);
            }
        }

		/// <summary>
		/// Initialize the prefab and components with the file given in the path.
		/// Mainly setting the url of both of the VideoPlayer and initializing the AudioSource.
		/// </summary>
		protected override void InitArtwork()
		{
			Recto.GetComponent<UnityEngine.Video.VideoPlayer>().url = Path;
			Recto.GetComponent<UnityEngine.Video.VideoPlayer>().isLooping = true;
			Recto.GetComponent<UnityEngine.Video.VideoPlayer>().audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
			Recto.GetComponent<UnityEngine.Video.VideoPlayer>().SetTargetAudioSource(0, gameObject.GetComponent<AudioSource>());
			Recto.GetComponent<UnityEngine.Video.VideoPlayer>().Prepare();

			Verso.GetComponent<UnityEngine.Video.VideoPlayer>().url = Path;
			Verso.GetComponent<UnityEngine.Video.VideoPlayer>().isLooping = true;
			Verso.GetComponent<UnityEngine.Video.VideoPlayer>().SetDirectAudioMute(0, true);

			gameObject.GetComponent<AudioSource>().spatialize = true;
			gameObject.GetComponent<AudioSource>().loop = true;

			StartCoroutine(SetRatio(Recto.GetComponent<UnityEngine.Video.VideoPlayer>()));
		}
		
		/// <summary>
		/// Method that sets the ratio of the video, for the scale of the prefab to match it.
		/// </summary>
		/// <param name="player">The video player.</param>
		/// <returns>A IEnumerator for the corountine.</returns>
		private IEnumerator SetRatio(UnityEngine.Video.VideoPlayer player)
		{
			yield return new WaitUntil(() => Recto.GetComponent<UnityEngine.Video.VideoPlayer>().isPrepared);
			float width = player.width;
			float height = player.height;
			float ratio = width / height;
			Debug.Log("Width: " + width);
			Debug.Log("Height: " + height);
			Recto.transform.localScale = new Vector3(Recto.transform.localScale.x * ratio, Recto.transform.localScale.y, Recto.transform.localScale.z);
			Verso.transform.localScale = new Vector3(Verso.transform.localScale.x * ratio, Verso.transform.localScale.y, Verso.transform.localScale.z);
		}

		/// <summary>
		/// Function that plays the video on both side of the object, by synchronizing them using the prepare completed event handler.
		/// Prepare the recto and when it is prepared, prepare the verso then play both.
		/// </summary>
		public void Play()
		{
			Recto.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
			Verso.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
		}

		/// <summary>
		/// Function that pauses both videos (recto and verso sides).
		/// </summary>
		public void Pause()
		{
			Recto.GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
			Verso.GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
		}

		/// <summary>
		/// Function that stops both videos (recto and verso sides).
		/// </summary>
		public void Stop()
		{
			Recto.GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
			Verso.GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
		}

		/// <summary>
		/// Boolean useful to know if the video is pinned to a wall.
		/// </summary>
		public bool IsPinned => transform.parent.GetComponent<Wall>() is object;

		/// <summary>
		/// This is a property to know if the object is playing sound.
		/// </summary>
		/// <returns>A boolean whether the object is playing sound.</returns>
		public bool IsPlaying => Recto.GetComponent<UnityEngine.Video.VideoPlayer>().isPlaying;

		/// <summary>
		/// Method that unpins the video of its wall, putting a new parent.
		/// </summary>
		/// <param name="parent">The new parent of the video.</param>
        public void Unpin(GameObject parent)
		{
			transform.SetParent(parent.transform);
		}

		/// <summary>
		/// Function that takes a wall and a direction as parameters and that allows pinning to the wall a video.
		/// </summary>
		/// <param name="wall">Wall to set as parent, and to pin the video on.</param>
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
		/// Returns the serialized video.
		/// </summary>
		/// <returns>Downclassed SerialiazedVideo in SerializedObject.</returns>
		public override SerializedObject GetSerializedObject()
		{
			return new SerializedVideo(this);
		}

		/// <summary>
		/// Data class to serialize videos.
		/// Saves the properties in Video.
		/// </summary>
		[System.Serializable]
		public class SerializedVideo : EVA.Artwork.SerializedArtWork
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
			public SerializedVideo() { }

			/// <summary>
			/// Constructor to create the serialized video from the video in parameter.
			/// </summary>
			/// <param name="video">The Video to be serialized.</param>
			public SerializedVideo(Video video) : base(video)
			{
				objectType = ObjectType.VIDEO;
				isPinned = video.IsPinned;
				parentId = video.transform.parent.gameObject.GetInstanceID();
				spatializeSound = video.SpatializedSound;
				looping = video.Looping;
				volume = video.Volume;
			}

			/// <summary>
			/// Modify the properties of the Video component on the gameobject in parameter,
			/// with the data saved in the attributes.
			/// </summary>
			/// <param name="gameObject">GameObject containing a Video component.</param>
			public override void Load(GameObject gameObject)
			{
				base.Load(gameObject);
				Video video = gameObject.GetComponent<Video>();
				video.SpatializedSound = spatializeSound;
				video.Looping = looping;
				video.Volume = volume;
			}
		}

	}
}
