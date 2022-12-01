using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
namespace EVA
{
    public class Video360Test
    {

        private GameObject gameObject;
        private Video360 stub;

        [SetUp]
        public void SetUp()
        {
            gameObject = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/360VideoArtwork.prefab"));
            stub = gameObject.GetComponent<Video360>();
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
            gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isLooping = true;
            Assert.True(stub.Looping);
            gameObject.GetComponent<AudioSource>().loop = false;
            gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isLooping = false;
            Assert.False(stub.Looping);
        }

        [Test]
        public void LoopingSetTest()
        {
            stub.Looping = true;
            Assert.True(gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isLooping);
            Assert.True(gameObject.GetComponent<AudioSource>().loop);
            stub.Looping = false;
            Assert.False(gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isLooping);
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
            string testWEBM = "Assets/Resources/Tests/cat.webm";
            stub.Path = testWEBM;
            yield return new WaitUntil(() => gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isPrepared);
            Assert.False(gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isPlaying);
            Assert.AreEqual(testWEBM, gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().url);
        }

        [UnityTest]
        public IEnumerator InitArtworkMp4Test()
        {
            string testMP4 = "Assets/Resources/Tests/cat.mp4";
            stub.Path = testMP4;
            yield return new WaitUntil(() => gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isPrepared);
            Assert.False(gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isPlaying);
            Assert.AreEqual(testMP4, gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().url);
        }

        [UnityTest]
        public IEnumerator PlayTest()
        {
            stub.Path = "Assets/Resources/Tests/cat.mp4";
            yield return new WaitUntil(() => gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isPrepared);
            stub.Play();
            Assert.True(gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isPlaying);
        }

        [UnityTest]
        public IEnumerator PauseTest()
        {
            stub.Path = "Assets/Resources/Tests/cat.mp4";
            yield return new WaitUntil(() => gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isPrepared);
            stub.Play();
            yield return new WaitForSeconds(0.5f);
            stub.Pause();
            Assert.False(gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isPlaying);
        }

        [UnityTest]
        public IEnumerator StopTest()
        {
            stub.Path = "Assets/Resources/Tests/cat.mp4";
            yield return new WaitUntil(() => gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isPrepared);
            stub.Play();
            yield return new WaitForSeconds(0.5f);
            stub.Stop();
            Assert.False(gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().isPlaying);
            Assert.AreEqual(0f, gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().time);
        }
    }
}
