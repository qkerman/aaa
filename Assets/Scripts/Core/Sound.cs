using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Threading;

namespace EVA
{
	/// <summary>
	/// Class defining behaviours of imported audio artworks and initializing it with an audio path from persistent data path.
	/// </summary>
	/// <remarks>
	/// This script is attached to a prefab composed of : an empty object with an AudioSource.
	/// </remarks>
	public class Sound : Artwork, HasSound
	{

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
		/// This is a property for sound looping, which is a feature from the AudioSource components in Unity. This property has a getter and a setter which are modifying the AudioSource settings.
		/// </summary>
		/// <returns> The getter of this property returns the actual AudioSource's looping setting. </returns>
		/// <value name = "value"> The setter of this property sets the AudioSource's looping setting as given boolean value. </value>
		public bool Looping
		{
			get
			{
				return gameObject.GetComponent<AudioSource>().loop;
			}
			set
			{
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
		/// This is a property to know if the object is playing sound.
		/// </summary>
		/// <returns>A boolean whether the object is playing sound.</returns>
		public bool IsPlaying => gameObject.GetComponent<AudioSource>().isPlaying;

        /// <summary>
        /// Method launching a coruntine that initialize the AudioSource component using the given audio path.
        /// </summary>
        protected override void InitArtwork()
		{
			StartCoroutine(ImportAudio());
		}

		/// <summary>
		/// Coruntine that reads the compatible audio file and initialize the AudioSource component.
		/// </summary>
		/// <returns> This coruntine returns the SendWebRequest result from the UnityWebRequest that read the audio file.</returns>
		private IEnumerator ImportAudio()
		{
			Interlocked.Increment(ref Creator.semaphore);
			AudioType audioType;
			string fileExt = System.IO.Path.GetExtension(Path).ToLowerInvariant();
			switch (fileExt)
			{
				case ".mp3":
					audioType = AudioType.MPEG;
					break;
				case ".ogg":
					audioType = AudioType.OGGVORBIS;
					break;
				default:
					throw new ArgumentException("This extension isn't supported !");

			}
			UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + Path, audioType); //FICHIERS MP3 UNIQUEMENT
			yield return uwr.SendWebRequest();
			if (uwr.result == UnityWebRequest.Result.ConnectionError)
			{
				Interlocked.Decrement(ref Creator.semaphore);
				throw new ApplicationException("Can't connect to the Server !");
			}
			AudioClip myClip = DownloadHandlerAudioClip.GetContent(uwr);
			gameObject.GetComponent<AudioSource>().clip = myClip;
			Interlocked.Decrement(ref Creator.semaphore);
		}

		/// <summary>
		/// This function plays the sound from AudioSource component.
		/// </summary>
		public void Play()
		{
			GetComponent<AudioSource>().Play();
		}

		/// <summary>
		/// This function pauses the sound from AudioSource component.
		/// </summary>
		public void Pause()
		{
			GetComponent<AudioSource>().Pause();
		}

		/// <summary>
		/// This function stops the sound from AudioSource component.
		/// </summary>
		public void Stop()
		{
			GetComponent<AudioSource>().Stop();
		}
		/// <summary>
		/// Define how the object works depending on the mode, when the sound is in one of the visitor modes, the capsule model disappears.
		/// </summary>
		/// <param name="mode">The new gallery mode.</param>
		public override void ChangeMode(InteractionMode mode)
		{
			base.ChangeMode(mode);
			switch (mode)
			{
				case InteractionMode.VISITOR_ONLY:
				case InteractionMode.VISITOR:
					gameObject.GetComponent<MeshRenderer>().enabled = false;
					gameObject.GetComponent<CapsuleCollider>().enabled = false;
					break;
				case InteractionMode.EDITOR:
					gameObject.GetComponent<MeshRenderer>().enabled = true;
					gameObject.GetComponent<CapsuleCollider>().enabled = true;
					break;
			}
		}
		
		/// <summary>
		/// Returns the serialized sound.
		/// </summary>
		/// <returns>Downclassed SerialiazedSound in SerializedObject.</returns>
		public override SerializedObject GetSerializedObject()
		{
			return new SerializedSound(this);
		}

		/// <summary>
		/// Data class to serialize sounds.
		/// Saves the properties in Sound.
		/// </summary>
		[System.Serializable]
		public class SerializedSound : EVA.Artwork.SerializedArtWork
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
			public SerializedSound() { }

			/// <summary>
			/// Constructor to create the serialized sound from the sound in parameter.
			/// </summary>
			/// <param name="sound">The Sound to be serialized.</param>
			public SerializedSound(Sound sound) : base(sound)
			{
				objectType = ObjectType.SOUND;
				spatializeSound = sound.SpatializedSound;
				looping = sound.Looping;
				volume = sound.Volume;
			}

			/// <summary>
			/// Modify the properties of the Sound component on the gameobject in parameter,
			/// with the data saved in the attributes.
			/// </summary>
			/// <param name="gameObject">GameObject containing a Sound component.</param>
			public override void Load(GameObject gameObject)
			{
				base.Load(gameObject);
				Sound sound = gameObject.GetComponent<Sound>();
				sound.SpatializedSound = spatializeSound;
				sound.Looping = looping;
				sound.Volume = volume;
			}
		}

	}
}
