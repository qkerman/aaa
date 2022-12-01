using System;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

namespace EVA
{
	/// <summary>
	/// This class permits to import each type of artworks in the gallery.
	/// </summary>
	public class Creator : MonoBehaviour
	{
		/// <summary>
		/// GameObject representing the player, especially its position, to place the imported objects.
		/// </summary>
		public GameObject player; // To modify for give the location of the headset with player.transform
		/// <summary>
		/// GameObject representing the gallery, especially to be imported objects' parent.
		/// </summary>
		public GameObject gallery;
		/// <summary>
		/// Prefab to instantiate when creating a 3D artwork from imported gltf file.
		/// </summary>
		public GameObject prefab_Model;
		/// <summary>
		/// Prefab to instantiate when creating a sound artwork from a file.
		/// </summary>
		public GameObject prefab_Sound;
		/// <summary>
		/// Prefab to instantiate when creating an image artwork from a file.
		/// </summary>
		public GameObject prefab_Picture;
		/// <summary>
		/// Prefab to instantiate when creating a video artwork from a file.
		/// </summary>
		public GameObject prefab_Video;
		/// <summary>
		/// Prefab to instantiate when creating a 360 video artwork from a file.
		/// </summary>
		public GameObject prefab_360Video;

		/// <summary>
		/// Prefab to instantiate a wall.
		/// </summary>
		public GameObject prefab_wall;

		/// <summary>
		/// Reference to control.
		/// </summary>
		public Control control;

		/// <summary>
        /// The canvas for wall creation.
        /// </summary>
		public GameObject wallCreationCanvas;

		/// <summary>
		/// Canvas to display a message while an object is imported.
		/// </summary>
		public Canvas importLabel;

		/// <summary>
		/// Prefab to instantiate when creating a new light, composed of a GameObject with a unity light component and the script <see cref="EVA.Light"/></see>. This prefabs also has 3D model children, for the visualization of lights in space, depending on its type.
		/// </summary>
		public GameObject prefabLight;

		/// <summary>
		/// The semaphore for the creator.
		/// </summary>
		public static int semaphore = 0;

		/// <summary>
		/// The update function that is executed every frame, to check the semaphore.
		/// </summary>
		private void Update()
        {
			if (semaphore > 0)
				importLabel.enabled = true;
			else importLabel.enabled = false;
        }

		/// <summary>
		/// This method permits to import 3D models.
		/// </summary>
		/// <remarks>
		/// First, the prefab model is instantiated. Then, the 3D Model transform is rotated depending on Vector3 created before, 
		/// the parent transform is associated to the gallery transform and the local position transform is equal to
		/// the player transform position added to the player transform forward.
		/// Finally, the path passed in parameter is assigned to the 3D Model path.
		/// </remarks>
		/// <param name="path">
		/// The method takes in parameter the 3D model path.
		/// </param>
		public void ImportModel(String path)
		{
			GameObject imported = Instantiate(prefab_Model);
			Vector3 rotationToAdd = new Vector3(0, player.transform.rotation.eulerAngles.y, 0);
			imported.transform.Rotate(rotationToAdd);
			imported.transform.parent = gallery.transform;
			imported.transform.localPosition = player.transform.position + player.transform.forward;
			Model3D mdl = imported.GetComponent<Model3D>();
			imported.GetComponent<Model3D>().Path = path;
		}

		/// <summary>
		/// This method permits to import sounds.
		/// </summary>
		/// <remarks>
		/// First, the prefab sound is instantiated. Then, the sound transform is rotated depending on Vector3 created before, 
		/// the parent transform is associated to the gallery transform and the local position transform is equal to
		/// the player transform position added to the player transform forward.
		/// Finally, the path passed in parameter is assigned to the sound path.
		/// </remarks>
		/// <param name="path">
		/// The method takes in parameter the sound path.
		/// </param>
		public void ImportSound(String path)
		{
			GameObject imported = Instantiate(prefab_Sound);
			Vector3 rotationToAdd = new Vector3(0, player.transform.rotation.eulerAngles.y, 0);
			imported.transform.Rotate(rotationToAdd);
			imported.transform.parent = gallery.transform;
			imported.transform.localPosition = player.transform.position + player.transform.forward;
			imported.GetComponent<Sound>().Path = path;
		}

		/// <summary>
		/// This method permits to import pictures.
		/// </summary>
		/// <remarks>
		/// First, the prefab picture is instantiated. Then, the picture transform is rotated depending on Vector3 created before, 
		/// the parent transform is associated to the gallery transform and the local position transform is equal to
		/// the player transform position added to the player transform forward.
		/// Finally, the path passed in parameter is assigned to the picture path.
		/// </remarks>
		/// <param name="path">
		/// The method takes in parameter the picture path.
		/// </param>
		public void ImportPicture(String path)
		{
			GameObject imported = Instantiate(prefab_Picture);
			Vector3 rotationToAdd = new Vector3(0, 180 + player.transform.rotation.eulerAngles.y, 0);
			imported.transform.Rotate(rotationToAdd);
			imported.transform.parent = gallery.transform;
			imported.transform.localPosition = player.transform.position + player.transform.forward;
			imported.GetComponent<Image>().Path = path;
		}

		/// <summary>
		/// This method permits to import videos.
		/// </summary>
		/// <remarks>
		/// First, the prefab video is instantiated. Then, the video transform is rotated depending on Vector3 created before, 
		/// the parent transform is associated to the gallery transform and the local position transform is equal to
		/// the player transform position added to the player transform forward.
		/// Finally, the path passed in parameter is assigned to the video path.
		/// </remarks>
		/// <param name="path">
		/// The method takes in parameter the video path.
		/// </param>
		public void ImportVideo(String path)
		{
			GameObject imported = Instantiate(prefab_Video);
			Vector3 rotationToAdd = new Vector3(0, 180 + player.transform.rotation.eulerAngles.y, 0);
			imported.GetComponent<Video>().Path = path;
			imported.transform.Rotate(rotationToAdd);
			imported.transform.parent = gallery.transform;
			imported.transform.localPosition = player.transform.position + player.transform.forward;

		}

		/// <summary>
		/// This method permits to import 360videos.
		/// </summary>
		/// <remarks>
		/// First, the prefab 360video is instantiated. Then, the 360video transform is rotated depending on Vector3 created before, 
		/// the parent transform is associated to the gallery transform and the local position transform is equal to
		/// the player transform position added to the player transform forward.
		/// Finally, the path passed in parameter is assigned to the 360video path.
		/// </remarks>
		/// <param name="path">
		/// The method takes in parameter the 360video path.
		/// </param>
		public void Import360Video(String path)
		{
			GameObject imported = Instantiate(prefab_360Video);
			Vector3 rotationToAdd = new Vector3(0, player.transform.rotation.eulerAngles.y, 0);
			imported.GetComponent<Video360>().Path = path;
			imported.transform.Rotate(rotationToAdd);
			imported.transform.parent = gallery.transform;
			imported.transform.localPosition = player.transform.position + player.transform.forward;
		}

		/// <summary>
		/// Start the  <see cref="EVA.Creator.CreateWall"/>CreateWall</see> method.
		/// </summary>
		public void AddWall()
		{
			StartCoroutine(CreateWall());
		}

		/// <summary>
		/// Limit input to build a wall by creating 3 points.
		/// When the wall creation is ended, it creates a wall if 3 points were saved.
		/// </summary>
		/// <returns>The IEnumerator, for this function to be a coroutine.</returns>
		private IEnumerator CreateWall()
		{
			control.GetMainMenu().GetComponent<UIManager>().Close();
			wallCreationCanvas.SetActive(true);
			control.state = ControlState.WallCreation;
			// Wait until the control state is changed, then the rest of the method is executed.
			yield return new WaitUntil(() => control.state == ControlState.NothingSelected);
			if (control.points.Count == 3)
			{
				GameObject wall = Instantiate(prefab_wall, gallery.transform);
				wall.GetComponent<Wall>().SetPlane(control.points[0].transform.position, control.points[1].transform.position, control.points[2].transform.position, player.transform.position);
			}
			foreach (GameObject g in control.points)
			{
				Destroy(g);
			}
			control.points.Clear();
			wallCreationCanvas.SetActive(false);
		}

		/// <summary>
		/// This methods creates a new light by instanciating the light prefab, moving the object in front of the viewer and assigning his type.
		/// </summary>
		/// <param name="type">The type of the instanciated light.</param>
		public void CreateNewLight(int type)
		{
			GameObject light = Instantiate(prefabLight);
			Vector3 rotationToAdd = new Vector3(0, player.transform.rotation.eulerAngles.y, 0);
			light.transform.Rotate(rotationToAdd);
			light.transform.parent = gallery.transform;
			light.transform.localPosition = player.transform.position + player.transform.forward;
			light.GetComponent<Light>().Type = (LightType)type;

		}
        /// <summary>
        /// The duplicate function duplicates a GameObject by instanciating a clone.
        /// </summary>
        /// <param name="source">The GameObject to duplicate.</param>
        /// <returns>The duplicated GameObject.</returns>
        public GameObject Duplicate(GameObject source)
        {
			GameObject clone = Instantiate(source, source.transform.parent);
			Artwork art = clone.GetComponentInChildren<Artwork>();
			if (art)
            {
				art.SetDuplicateString(source.GetComponentInChildren<Artwork>().Path);
            }
			clone.transform.localPosition = source.transform.localPosition + player.transform.right * 0.25f;
			clone.transform.rotation = source.transform.rotation;
			clone.transform.localScale = source.transform.localScale;
			return clone;
        }
	}
}
