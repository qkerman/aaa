using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace EVA
{
    public class SoundTest
    {
        private GameObject gameObject;
        private Sound stub;

        [SetUp]
        public void SetUp()
        {
            gameObject = UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/SoundArtwork.prefab"));
            stub = gameObject.GetComponent<Sound>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(gameObject);
        }

        [Test]
        public void SpatializeGetTest()
        {
            gameObject.GetComponent<AudioSource>().spatialBlend = 1;
            Assert.True(stub.SpatializedSound);
            gameObject.GetComponent<AudioSource>().spatialBlend = 0;
            Assert.False(stub.SpatializedSound);
        }

        [Test]
        public void SpatializeSetTest()
        {
            stub.SpatializedSound = true;
            Assert.AreEqual(1,gameObject.GetComponent<AudioSource>().spatialBlend);
            stub.SpatializedSound = false;
            Assert.AreEqual(0,gameObject.GetComponent<AudioSource>().spatialBlend);
        }

        [Test]
        public void LoopingGetTest()
        {
            gameObject.GetComponent<AudioSource>().loop = true;
            Assert.True(stub.Looping);
            gameObject.GetComponent<AudioSource>().loop = false;
            Assert.False(stub.Looping);
        }

        [Test]
        public void LoopingSetTest()
        {
            stub.Looping = true;
            Assert.True(gameObject.GetComponent<AudioSource>().loop);
            stub.Looping = false;
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
        public IEnumerator InitArtworkOGGTest()
        {
            //Test with ogg
            AudioClip previous = gameObject.GetComponent<AudioSource>().clip;
            stub.Path = Application.dataPath + "/Resources/Tests/Skyloft.ogg";
            yield return new WaitUntil(() => gameObject.GetComponent<AudioSource>().clip is object);
            Assert.AreNotEqual(previous, gameObject.GetComponent<AudioSource>().clip);
        }
        
        [UnityTest]
        public IEnumerator InitArtworkMP3Test()
        {
            //Test with mp3
            AudioClip previous = gameObject.GetComponent<AudioSource>().clip;
            stub.Path = Application.dataPath + "/Resources/Tests/Skyloft.mp3";
            yield return new WaitUntil(() => gameObject.GetComponent<AudioSource>().clip is object);
            Assert.AreNotEqual(previous, gameObject.GetComponent<AudioSource>().clip);
        }

        [UnityTest]
        public IEnumerator PlayTest()
        {
            stub.Path = Application.dataPath + "/Resources/Tests/Skyloft.mp3";
            yield return new WaitUntil(() => gameObject.GetComponent<AudioSource>().clip is object);
            stub.Play();
            Assert.True(gameObject.GetComponent<AudioSource>().isPlaying);
        }

        [UnityTest]
        public IEnumerator PauseTest()
        {
            stub.Path = Application.dataPath + "/Resources/Tests/Skyloft.mp3";
            yield return new WaitUntil(() => gameObject.GetComponent<AudioSource>().clip is object);
            stub.Play();
            stub.Pause();
            Assert.False(gameObject.GetComponent<AudioSource>().isPlaying);
        }

        [UnityTest]
        public IEnumerator StopTest()
        {
            stub.Path = Application.dataPath + "/Resources/Tests/Skyloft.mp3";
            yield return new WaitUntil(() => gameObject.GetComponent<AudioSource>().clip is object);
            stub.Play();
            stub.Stop();
            Assert.False(gameObject.GetComponent<AudioSource>().isPlaying);
            Assert.AreEqual(0f, gameObject.GetComponent<AudioSource>().time);
        }
    }
}