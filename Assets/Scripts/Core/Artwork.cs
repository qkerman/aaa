using System;
using UnityEngine;

namespace EVA
{
	/// <summary>
	/// This abstract class inherits from EVAObject. This class will allow us to describe every artwork that can be placed in our application.
	/// </summary>
	public abstract class Artwork : Object
	{

		/// <summary>
		/// This string contains the path of the imported file, displays on the artwork prefab (depending on the type of artwork).
		/// </summary>
		private string _path;

		/// <summary>
		/// Set the internal value of <see cref="Path"/> to the value in parameter.
		/// We were forced to do this for the save because the duplicate method of Unity
		/// does not copy the path from the source object
		/// </summary>
		/// <param name="path">The new value of the path.</param>
		public void SetDuplicateString(string path)
        {
			_path = path;
        }

		/// <summary>
		/// This property manages the path of the imported file.
		/// </summary>
		/// <returns> The getter is returning the string of the imported file path.</returns>
		/// <value name = "value"> The setter is setting the path with the given string value, and then initializing the artwork by calling <see cref="EVAArtwork.InitArtwork()"/> method.</value>
		public string Path
		{
			get => _path;
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException();
				if (FileChooser.IsFileExists(value))
				{
					_path = value;
					InitArtwork();
				}
				else
				{
					throw new ArgumentException("This path doesn't exists !");
				}
			}
		}

		/// <summary>
		/// This property manages the collision of the artwork, by enabling its collider.
		/// </summary>
		/// <returns> The getter is returning the enabling state of the artwork's collider.</returns>
		/// <value name = "value"> The setter is setting the collider enabling state with the given boolean value.</value>
		public bool Collision
		{
			get
			{
				return GetComponentInChildren<Collider>().enabled;
			}
			set
			{
				foreach(Collider collider in GetComponentsInChildren<Collider>())
					collider.enabled = value;
			}
		}

		/// <summary>
		/// Initialize the prefab and components of the artwork with the file given in the path.
		/// </summary>
		protected abstract void InitArtwork();


		/// <summary>
		/// Base class for serialized artworks like videos and images. Inherit from SerializedObject.
		/// Saves the path and the collision property.
		/// </summary>
		[System.Serializable]
		public class SerializedArtWork : EVA.Object.SerializedObject
		{
			/// <summary>
			/// Value of the Path property.
			/// </summary>
			public string path;

			/// <summary>
			/// Value of the collision property.
			/// </summary>
			public bool collision;

			/// <summary>
			/// Default constructor for deserialization.
			/// </summary>
			public SerializedArtWork() { }

			/// <summary>
			/// Constructor to create the serialized artwork from the artwork in parameter.
			/// </summary>
			/// <param name="artwork"> Artwork to be serialized.</param>
			public SerializedArtWork(Artwork artwork) : base(artwork)
			{
				path = artwork.Path;
				collision = artwork.Collision;
			}

			/// <summary>
			/// Modify the properties of the Artwork component on the gameobject in parameter,
			/// with the data saved in the attributes.
			/// </summary>
			/// <param name="gameObject">GameObject containing an Artwork component.</param>
			public override void Load(GameObject gameObject)
			{
				base.Load(gameObject);
				Artwork artwork = gameObject.GetComponent<Artwork>();
				artwork.Path = path;
				artwork.Collision = collision;
			}
		}


	}
}
