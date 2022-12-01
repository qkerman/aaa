using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace EVA
{
    public class SaveManagerTest
    {
        private GameObject parent;
        private GameObject player;
        private SaveManager saveManager;
        private Creator creator;


        [SetUp]
        public void SetUp()
        {
            parent = new GameObject();
            player = new GameObject();
            player.transform.position = new Vector3(2, 1.8f, 2);
            player.transform.eulerAngles = new Vector3(90, 0, 0);
            creator = new GameObject().AddComponent<Creator>();
            creator.player = player;
            creator.gallery = parent;
            creator.importLabel = new GameObject().AddComponent<Canvas>();
            creator.prefabLight = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/Light.prefab");
            creator.prefab_360Video = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/360VideoArtwork.prefab");
            creator.prefab_Model = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/ModelArtwork.prefab");
            creator.prefab_Picture = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/ImageArtwork.prefab");
            creator.prefab_Sound = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/SoundArtwork.prefab");
            creator.prefab_Video = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/VideoArtwork.prefab");
            creator.prefab_wall = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/Wall.prefab");
            saveManager = parent.AddComponent<SaveManager>();
            saveManager.lightPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/Light.prefab");
            saveManager.video360Prefab= AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/360VideoArtwork.prefab");
            saveManager.modelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/ModelArtwork.prefab");
            saveManager.imagePrefab= AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/ImageArtwork.prefab");
            saveManager.soundPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/SoundArtwork.prefab");
            saveManager.videoPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/VideoArtwork.prefab");
            saveManager.wallPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/Wall.prefab");
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.Destroy(parent);
        }

        [UnityTest]
        public IEnumerator SaveLoadTest()
        {
            int type = 1;
            creator.CreateNewLight(type);
            creator.Import360Video(Application.dataPath + "/Resources/Tests/cat.mp4");
            creator.ImportModel(Application.dataPath + "/Resources/Tests/pipoudou.glb");
            creator.ImportPicture(Application.dataPath + "/Resources/Tests/dnd_logo.png");
            creator.ImportPicture(Application.dataPath + "/Resources/Tests/dnd_logo.png");
            creator.ImportSound(Application.dataPath + "/Resources/Tests/Skyloft.mp3");
            creator.ImportVideo(Application.dataPath + "/Resources/Tests/cat.mp4");
            GameObject wall = UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/Wall.prefab"), parent.transform);
            GameObject image = parent.GetComponentInChildren<Image>().gameObject;
            Assert.AreEqual(8, parent.transform.childCount);
            image.transform.SetParent(wall.transform);
            yield return new WaitUntil(() => Creator.semaphore == 0);
            Assert.Throws<System.ArgumentException>(() => saveManager.Save("bidon"));
            saveManager.Save(Application.dataPath + "/Resources/Tests/test.eva");
            yield return new WaitUntil(() => Creator.semaphore == 0);
            Assert.AreEqual(7, parent.transform.childCount);
            foreach (Transform child in parent.transform)
            {
                UnityEngine.GameObject.Destroy(child.gameObject);
            }
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(0, parent.transform.childCount);
            Assert.Throws<System.ArgumentException>(() => saveManager.Load("bidon"));
            saveManager.Load(Application.dataPath + "/Resources/Tests/test.eva");
            yield return new WaitUntil(() => Creator.semaphore == 0);
            Assert.AreEqual(7, parent.transform.childCount);
            Assert.IsNotNull(parent.GetComponentInChildren<Video>());
            Assert.IsNotNull(parent.GetComponentInChildren<Image>());
            Assert.IsNotNull(parent.GetComponentInChildren<Video360>());
            Assert.IsNotNull(parent.GetComponentInChildren<Model3D>());
            Assert.IsNotNull(parent.GetComponentInChildren<Sound>());
            Assert.IsNotNull(parent.GetComponentInChildren<Wall>());
            Assert.IsNotNull(parent.GetComponentInChildren<Wall>().GetComponentInChildren<Image>());
            Assert.IsNotNull(parent.GetComponentInChildren<Light>());
            System.IO.File.Delete(Application.dataPath + "/Resources/Tests/test.eva");
        }


    }
}
