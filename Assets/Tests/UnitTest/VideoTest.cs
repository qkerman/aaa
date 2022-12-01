using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace EVA
{
    public class VideoTest
    {
        private GameObject gameObject;
        private GameObject wall;
        private Video stub;

        [SetUp]
        public void SetUp()
        {
            gameObject = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/VideoArtwork.prefab"));
            wall = UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/Wall.prefab"));
            stub = gameObject.GetComponent<Video>();
        }

        [Test]
        public void SpatializedSoundGetTest()
        {
            gameObject.GetComponent<AudioSource>().spatialBlend = 1;
            Assert.True(stub.SpatializedSound);
            gameObject.GetComponent<AudioSource>().spatialBlend = 0;
            Assert.False(stub.SpatializedSound);
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(gameObject);
        }

        [Test]
        public void SpatializedSoundSetTest()
        {
            stub.SpatializedSound = true;
            Assert.AreEqual(1, gameObject.GetComponent<AudioSource>().spatialBlend);
            stub.SpatializedSound = false;
            Assert.AreEqual(0, gameObject.GetComponent<AudioSource>().spatialBlend);
        }

        [Test]
        public void LoopingGetTest()
        {
            gameObject.GetComponent<AudioSource>().loop = true;
            foreach (UnityEngine.Video.VideoPlayer videoplayer in gameObject.GetComponentsInChildren<UnityEngine.Video.VideoPlayer>())
            {
                videoplayer.isLooping = true;
            }
            Assert.True(stub.Looping);
            gameObject.GetComponent<AudioSource>().loop = false;
            foreach (UnityEngine.Video.VideoPlayer videoplayer in gameObject.GetComponentsInChildren<UnityEngine.Video.VideoPlayer>())
            {
                videoplayer.isLooping = false;
            }
            Assert.False(stub.Looping);
        }

        [Test]
        public void LoopingSetTest()
        {
            stub.Looping = true;
            foreach (UnityEngine.Video.VideoPlayer videoplayer in gameObject.GetComponentsInChildren<UnityEngine.Video.VideoPlayer>())
            {
                Assert.True(videoplayer.isLooping);
            }
            Assert.True(gameObject.GetComponent<AudioSource>().loop);
            stub.Looping = false;
            foreach (UnityEngine.Video.VideoPlayer videoplayer in gameObject.GetComponentsInChildren<UnityEngine.Video.VideoPlayer>())
            {
                Assert.False(videoplayer.isLooping);
            }
            Assert.False(gameObject.GetComponent<AudioSource>().loop);
        }

        [Test]
        public void VolumeGetTest()
        {
            gameObject.GetComponent<AudioSource>().volume = 0.5f;
            Assert.AreEqual(50, stub.Volume);
        }

        [Test]
        public void VolumeSetTest()
        {
            stub.Volume = 50;
            Assert.AreEqual(0.5f, gameObject.GetComponent<AudioSource>().volume);
        }

        [Test]
        public void VolumeSetMaxTest()
        {
            stub.Volume = 200;
            Assert.AreEqual(1f, gameObject.GetComponent<AudioSource>().volume);
        }

        [Test]
        public void VolumeSetMinTest()
        {
            stub.Volume = -100;
            Assert.AreEqual(0f, gameObject.GetComponent<AudioSource>().volume);
        }

        [UnityTest]
        public IEnumerator InitArtworkWebmTest()
        {
            //Test with webm
            string testWEBM = "Assets/Resources/Tests/cat.webm";
            stub.Path = testWEBM;
            yield return new WaitForSeconds(1.0f);
            foreach (UnityEngine.Video.VideoPlayer videoplayer in gameObject.GetComponentsInChildren<UnityEngine.Video.VideoPlayer>())
            {
                Assert.False(videoplayer.isPlaying);
                Assert.AreEqual(testWEBM, videoplayer.url);
            }
        }

        [UnityTest]
        public IEnumerator InitArtworkMp4Test()
        {
            //Test with mp4
            string testMP4 = "Assets/Resources/Tests/cat.mp4";
            stub.Path = testMP4;
            yield return new WaitForSeconds(1.0f);
            foreach (UnityEngine.Video.VideoPlayer videoplayer in gameObject.GetComponentsInChildren<UnityEngine.Video.VideoPlayer>())
            {
                Assert.False(videoplayer.isPlaying);
                Assert.AreEqual(testMP4, videoplayer.url);
            }
        }

        [UnityTest]
        public IEnumerator PlayTest()
        {
            stub.Path = "Assets/Resources/Tests/cat.mp4";
            yield return new WaitForSeconds(1f);
            stub.Play();
            yield return new WaitForSeconds(0.5f);
            foreach (UnityEngine.Video.VideoPlayer videoplayer in gameObject.GetComponentsInChildren<UnityEngine.Video.VideoPlayer>())
            {
                Assert.IsTrue(videoplayer.isPlaying);
            }
        }

        [UnityTest]
        public IEnumerator PauseTest()
        {
            stub.Path = "Assets/Resources/Tests/cat.mp4";
            stub.Play();
            yield return new WaitForSeconds(0.4f);
            stub.Pause();
            foreach (UnityEngine.Video.VideoPlayer videoplayer in gameObject.GetComponentsInChildren<UnityEngine.Video.VideoPlayer>())
            {
                Assert.IsFalse(videoplayer.isPlaying);
                Assert.AreNotEqual(0d, videoplayer.time);
            }
        }

        [UnityTest]
        public IEnumerator StopTest()
        {
            stub.Path = "Assets/Resources/Tests/cat.mp4";
            yield return new WaitForSeconds(1.0f);
            stub.Play();
            yield return new WaitForSeconds(0.5f);
            stub.Stop();
            foreach (UnityEngine.Video.VideoPlayer videoplayer in gameObject.GetComponentsInChildren<UnityEngine.Video.VideoPlayer>())
            {
                Assert.False(videoplayer.isPlaying);
                Assert.AreEqual(0d, videoplayer.time);
            }
        }

        [Test]
        public void ChangeParentNotWallTest()
        {
            GameObject help = new GameObject();
            GameObject help2 = new GameObject();
            stub.transform.parent = help.transform;
            stub.ChangeParent(help2);
            Assert.AreEqual(help2, stub.transform.parent.gameObject);
        }

        [Test]
        public void ChangeParentWallTest()
        {
            GameObject help = new GameObject();
            stub.transform.parent = wall.transform;
            stub.ChangeParent(help);
            Assert.AreEqual(wall, stub.transform.parent.gameObject);
        }

        [Test]
        public void PinTest()
        {
            stub.Pin(wall.GetComponent<Wall>(), new Vector3(0, 0, 0));
            Assert.AreEqual(wall, stub.transform.parent.gameObject);
        }

        [Test]
        public void PinVersoWall()
        {
            var comparer = QuaternionEqualityComparer.Instance;
            stub.Pin(wall.GetComponent<Wall>(), new Vector3(0, 0, 1));
            Assert.That(stub.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));
        }

        [Test]
        public void PinRectoWall()
        {
            var comparer = QuaternionEqualityComparer.Instance;
            stub.Pin(wall.GetComponent<Wall>(), new Vector3(0, 0, -1));
            Assert.That(stub.transform.rotation, Is.EqualTo(new Quaternion(0, 1, 0, 0)).Using(comparer));
        }

        [Test]
        public void UnpinTest()
        {
            GameObject help = new GameObject();
            stub.Unpin(help);
            Assert.AreEqual(help, stub.transform.parent.gameObject);
        }
    }
}
