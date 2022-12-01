using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace EVA
{
    public class CreatorTest
    {
        private GameObject parent;
        private GameObject player;
        private Creator creator;
        private GameObject wall;

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
            creator.importLabel = parent.AddComponent<Canvas>();
            creator.prefabLight = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/Light.prefab");
            creator.prefab_360Video = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/360VideoArtwork.prefab");
            creator.prefab_Model = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/ModelArtwork.prefab");
            creator.prefab_Picture = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/ImageArtwork.prefab");
            creator.prefab_Sound = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/SoundArtwork.prefab");
            creator.prefab_Video = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/VideoArtwork.prefab");
            creator.prefab_wall = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/Wall.prefab");
            wall = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/Wall.prefab"),parent.transform);
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(creator.gameObject);
            Object.Destroy(player);
            Object.Destroy(parent);
            Creator.semaphore = 0;
        }

        [Test]
        public void CreateLightTest()
        {
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            int type = 1;
            creator.CreateNewLight(type);
            var evaLight = parent.GetComponentInChildren<Light>();
            Assert.AreEqual(parent, evaLight.transform.parent.gameObject);
            Assert.That(evaLight.transform.localPosition, Is.EqualTo(player.transform.position + player.transform.forward).Using(comparerVec3));
            Quaternion compared = Quaternion.identity;
            compared.eulerAngles = new Vector3(0, player.transform.rotation.eulerAngles.y, 0);
            Assert.That(evaLight.transform.rotation, Is.EqualTo(compared).Using(comparerQuat));
            Assert.AreEqual((LightType) type, evaLight.Type);
        }
        
        [Test]
        public void Create360VideoTest()
        {
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            creator.Import360Video(Application.dataPath + "/Resources/Tests/cat.mp4");
            var video360 = parent.GetComponentInChildren<Video360>();
            Assert.AreEqual(parent, video360.transform.parent.gameObject);
            Assert.AreEqual(Application.dataPath + "/Resources/Tests/cat.mp4", video360.Path);
            Assert.That(video360.transform.localPosition, Is.EqualTo(player.transform.position + player.transform.forward).Using(comparerVec3));
            Quaternion compared = Quaternion.identity;
            compared.eulerAngles = new Vector3(0, player.transform.rotation.eulerAngles.y, 0);
            Assert.That(video360.transform.rotation, Is.EqualTo(compared).Using(comparerQuat));
        }
        
        [UnityTest]
        public IEnumerator CreateModelTest()
        {
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            creator.ImportModel(Application.dataPath + "/Resources/Tests/pipoudou.glb");
            var model3D = parent.GetComponentInChildren<Model3D>();
            yield return new WaitForSeconds(2f);
            Assert.AreEqual(parent, model3D.transform.parent.gameObject);
            Assert.AreEqual(Application.dataPath + "/Resources/Tests/pipoudou.glb", model3D.Path);
            Assert.That(model3D.transform.localPosition, Is.EqualTo(player.transform.position + player.transform.forward).Using(comparerVec3));
            Quaternion compared = Quaternion.identity;
            compared.eulerAngles = new Vector3(0, player.transform.rotation.eulerAngles.y, 0);
            Assert.That(model3D.transform.rotation, Is.EqualTo(compared).Using(comparerQuat));
        }

        [Test]
        public void CreateImageTest()
        {
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            Debug.Log(Application.dataPath + "/Resources/Tests/dnd_logo.png");
            creator.ImportPicture(Application.dataPath + "/Resources/Tests/dnd_logo.png");
            var image = parent.GetComponentInChildren<Image>();
            Assert.AreEqual(parent, image.transform.parent.gameObject);
            Assert.AreEqual(Application.dataPath + "/Resources/Tests/dnd_logo.png", image.Path);
            Assert.That(image.transform.localPosition, Is.EqualTo(player.transform.position + player.transform.forward).Using(comparerVec3));
            Quaternion compared = Quaternion.identity;
            compared.eulerAngles = new Vector3(0, 180 + player.transform.rotation.eulerAngles.y, 0);
            Assert.That(image.transform.rotation, Is.EqualTo(compared).Using(comparerQuat));
        }
        
        
        [Test]
        public void CreateSoundTest()
        {
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            creator.ImportSound(Application.dataPath + "/Resources/Tests/Skyloft.mp3");
            var sound = parent.GetComponentInChildren<Sound>();
            Assert.AreEqual(parent, sound.transform.parent.gameObject);
            Assert.AreEqual(Application.dataPath + "/Resources/Tests/Skyloft.mp3", sound.Path);
            Assert.That(sound.transform.localPosition, Is.EqualTo(player.transform.position + player.transform.forward).Using(comparerVec3));
            Quaternion compared = Quaternion.identity;
            compared.eulerAngles = new Vector3(0, player.transform.rotation.eulerAngles.y, 0);
            Assert.That(sound.transform.rotation, Is.EqualTo(compared).Using(comparerQuat));
        }

        [Test]
        public void CreateVideoTest()
        {
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            creator.ImportVideo(Application.dataPath + "/Resources/Tests/cat.mp4");
            var video = parent.GetComponentInChildren<Video>();
            Assert.AreEqual(parent, video.transform.parent.gameObject);
            Assert.AreEqual(Application.dataPath + "/Resources/Tests/cat.mp4", video.Path);
            Assert.That(video.transform.localPosition, Is.EqualTo(player.transform.position + player.transform.forward).Using(comparerVec3));
            Quaternion compared = Quaternion.identity;
            compared.eulerAngles = new Vector3(0, 180 + player.transform.rotation.eulerAngles.y, 0);
            Assert.That(video.transform.rotation, Is.EqualTo(compared).Using(comparerQuat));
        }

        [Test]
        public void DuplicateLightTest()
        {
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            creator.CreateNewLight(1);
            var evalight = parent.GetComponentInChildren<Light>();
            GameObject retour = creator.Duplicate(evalight.gameObject);
            Assert.AreEqual(retour.transform.parent.gameObject, evalight.transform.parent.gameObject);
            Assert.That(retour.transform.position, Is.EqualTo(evalight.transform.position + player.transform.right * 0.25f).Using(comparerVec3));
            Assert.That(retour.transform.rotation, Is.EqualTo(evalight.transform.rotation).Using(comparerQuat));
            Assert.That(retour.transform.localScale, Is.EqualTo(evalight.transform.localScale).Using(comparerVec3));
            var copyLight = retour.GetComponent<Light>();
            Assert.AreEqual(evalight.Color, copyLight.Color);
            Assert.AreEqual(evalight.CullingMask, copyLight.CullingMask);
            Assert.AreEqual(evalight.ModelVisibility, copyLight.ModelVisibility);
            Assert.AreEqual(evalight.Range, copyLight.Range);
            Assert.AreEqual(evalight.SpotAngle, copyLight.SpotAngle);
            Assert.AreEqual(evalight.Switch, copyLight.Switch);
            Assert.AreEqual(evalight.Type, copyLight.Type);
        }

        [Test]
        public void DuplicateVideo360Test()
        {
            creator.Import360Video(Application.dataPath + "/Resources/Tests/cat.mp4");
            var video360 = parent.GetComponentInChildren<Video360>();
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            GameObject retour = creator.Duplicate(video360.gameObject);
            Assert.AreEqual(retour.transform.parent.gameObject, video360.transform.parent.gameObject);
            Assert.That(retour.transform.position, Is.EqualTo(video360.transform.position + player.transform.right * 0.25f).Using(comparerVec3));
            Assert.That(retour.transform.rotation, Is.EqualTo(video360.transform.rotation).Using(comparerQuat));
            Assert.That(retour.transform.localScale, Is.EqualTo(video360.transform.localScale).Using(comparerVec3));
            var copyVideo360 = retour.GetComponent<Video360>();
            Assert.AreEqual(video360.Path, copyVideo360.Path);
            Assert.AreEqual(video360.Looping, copyVideo360.Looping);
            Assert.AreEqual(video360.SpatializedSound, copyVideo360.SpatializedSound);
            Assert.AreEqual(video360.Volume, copyVideo360.Volume);
        }

        [UnityTest]
        public IEnumerator DuplicateModelTest()
        {
            creator.ImportModel(Application.dataPath + "/Resources/Tests/pipoudou.glb");
            var model = parent.GetComponentInChildren<Model3D>();
            yield return new WaitForSeconds(2f);
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            GameObject retour = creator.Duplicate(model.gameObject);
            Assert.AreEqual(retour.transform.parent.gameObject, model.transform.parent.gameObject);
            Assert.That(retour.transform.position, Is.EqualTo(model.transform.position + player.transform.right * 0.25f).Using(comparerVec3));
            Assert.That(retour.transform.rotation, Is.EqualTo(model.transform.rotation).Using(comparerQuat));
            Assert.That(retour.transform.localScale, Is.EqualTo(model.transform.localScale).Using(comparerVec3));
            var copyModel = retour.GetComponent<Model3D>();
            Assert.AreEqual(model.Path, copyModel.Path);
        }

        [Test]
        public void DuplicateImageWithoutWallTest()
        {
            creator.ImportPicture(Application.dataPath + "/Resources/Tests/dnd_logo.png");
            var image = parent.GetComponentInChildren<Image>();
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            GameObject retour = creator.Duplicate(image.gameObject);
            Assert.AreEqual(retour.transform.parent.gameObject, image.transform.parent.gameObject);
            Assert.That(retour.transform.position, Is.EqualTo(image.transform.position + player.transform.right * 0.25f).Using(comparerVec3));
            Assert.That(retour.transform.rotation, Is.EqualTo(image.transform.rotation).Using(comparerQuat));
            Assert.That(retour.transform.localScale, Is.EqualTo(image.transform.localScale).Using(comparerVec3));
            var copyImage = retour.GetComponent<Image>();
            Assert.AreEqual(image.Path, copyImage.Path);
            Assert.AreEqual(image.IsPinned, copyImage.IsPinned);
        }

        [Test]
        public void DuplicateImageWithWallTest()
        {
            creator.ImportPicture(Application.dataPath + "/Resources/Tests/dnd_logo.png");
            var image = parent.GetComponentInChildren<Image>();
            image.Pin(wall.GetComponent<Wall>(), Vector3.up);
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            GameObject retour = creator.Duplicate(image.gameObject);
            Assert.AreEqual(retour.transform.parent.gameObject, image.transform.parent.gameObject);
            Assert.That(retour.transform.position, Is.EqualTo(image.transform.position + player.transform.right * 0.25f).Using(comparerVec3));
            Assert.That(retour.transform.rotation, Is.EqualTo(image.transform.rotation).Using(comparerQuat));
            Assert.That(retour.transform.localScale, Is.EqualTo(image.transform.localScale).Using(comparerVec3));
            var copyImage = retour.GetComponent<Image>();
            Assert.AreEqual(image.Path, copyImage.Path);
            Assert.AreEqual(image.IsPinned, copyImage.IsPinned);
        }

        [Test]
        public void DuplicateSoundTest()
        {
            creator.ImportSound(Application.dataPath + "/Resources/Tests/Skyloft.mp3");
            var sound = parent.GetComponentInChildren<Sound>();
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            GameObject retour = creator.Duplicate(sound.gameObject);
            Assert.AreEqual(retour.transform.parent.gameObject, sound.transform.parent.gameObject);
            Assert.That(retour.transform.position, Is.EqualTo(sound.transform.position + player.transform.right * 0.25f).Using(comparerVec3));
            Assert.That(retour.transform.rotation, Is.EqualTo(sound.transform.rotation).Using(comparerQuat));
            Assert.That(retour.transform.localScale, Is.EqualTo(sound.transform.localScale).Using(comparerVec3));
            var copySound = retour.GetComponent<Sound>();
            Assert.AreEqual(sound.Path, copySound.Path);
            Assert.AreEqual(sound.Looping, copySound.Looping);
            Assert.AreEqual(sound.SpatializedSound, copySound.SpatializedSound);
            Assert.AreEqual(sound.Volume, copySound.Volume);
        }

        [Test]
        public void DuplicateVideoWithoutWallTest()
        {
            creator.ImportVideo(Application.dataPath + "/Resources/Tests/cat.mp4");
            var video = parent.GetComponentInChildren<Video>();
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            GameObject retour = creator.Duplicate(video.gameObject);
            Assert.AreEqual(retour.transform.parent.gameObject, video.transform.parent.gameObject);
            Assert.That(retour.transform.position, Is.EqualTo(video.transform.position + player.transform.right * 0.25f).Using(comparerVec3));
            Assert.That(retour.transform.rotation, Is.EqualTo(video.transform.rotation).Using(comparerQuat));
            Assert.That(retour.transform.localScale, Is.EqualTo(video.transform.localScale).Using(comparerVec3));
            var copyVideo = retour.GetComponent<Video>();
            Assert.AreEqual(video.Path, copyVideo.Path);
            Assert.AreEqual(video.IsPinned, copyVideo.IsPinned);
            Assert.AreEqual(video.Looping, copyVideo.Looping);
            Assert.AreEqual(video.SpatializedSound, copyVideo.SpatializedSound);
            Assert.AreEqual(video.Volume, copyVideo.Volume);
        }

        [Test]
        public void DuplicateVideoWithWallTest()
        {
            var comparerVec3 = Vector3ComparerWithEqualsOperator.Instance;
            var comparerQuat = QuaternionEqualityComparer.Instance;
            creator.ImportVideo(Application.dataPath + "/Resources/Tests/cat.mp4");
            var video = parent.GetComponentInChildren<Video>();
            video.Pin(wall.GetComponent<Wall>(), Vector3.up);
            GameObject retour = creator.Duplicate(video.gameObject);
            Assert.AreEqual(retour.transform.parent.gameObject, video.transform.parent.gameObject);
            Assert.That(retour.transform.position, Is.EqualTo(video.transform.position + player.transform.right * 0.25f).Using(comparerVec3));
            Assert.That(retour.transform.rotation, Is.EqualTo(video.transform.rotation).Using(comparerQuat));
            Assert.That(retour.transform.localScale, Is.EqualTo(video.transform.localScale).Using(comparerVec3));
            var copyVideo = retour.GetComponent<Video>();
            Assert.AreEqual(video.Path, copyVideo.Path);
            Assert.AreEqual(video.IsPinned, copyVideo.IsPinned);
            Assert.AreEqual(video.Looping, copyVideo.Looping);
            Assert.AreEqual(video.SpatializedSound, copyVideo.SpatializedSound);
            Assert.AreEqual(video.Volume, copyVideo.Volume);
        }
    }
}
