using NUnit.Framework;
using UnityEngine;

namespace EVA
{
    public class HasSoundTest
    {
        private class HasSoundStub : MonoBehaviour, HasSound
        {
            public bool _spatial;
            public bool _loop;
            public int _vol;
            public bool SpatializedSound
            {
                get => _spatial;
                set => _spatial = value;
            }
            public bool Looping
            {
                get => _loop;
                set => _loop = value;
            }
            public int Volume
            {
                get => _vol;
                set => _vol = value;
            }

            public bool IsPlaying => throw new System.NotImplementedException();

            public void Pause()
            {
                throw new System.NotImplementedException();
            }

            public void Play()
            {
                throw new System.NotImplementedException();
            }

            public void Stop()
            {
                throw new System.NotImplementedException();
            }
        }

        private GameObject gameObject;
        private HasSoundStub stub;
        [SetUp]
        public void SetUp()
        {
            gameObject = new GameObject();
            stub = gameObject.AddComponent<HasSoundStub>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(gameObject);
        }

        [Test]
        public void SpatialSoundGetPropertyTest()
        {
            stub._spatial = true;
            Assert.True(stub.SpatializedSound);
        }

        [Test]
        public void SpatialSoundSetPropertyTest()
        {
            stub.SpatializedSound = true;
            Assert.True(stub._spatial);
        }

        [Test]
        public void LoopingGetPropertyTest()
        {
            stub._loop = true;
            Assert.True(stub.Looping);
        }

        [Test]
        public void LoopingSetPropertyTest()
        {
            stub.Looping = true;
            Assert.True(stub._loop);
        }

        [Test]
        public void VolumeGetPropertyTest()
        {
            stub._vol = 50;
            Assert.AreEqual(50, stub.Volume);
        }

        [Test]
        public void VolumeSetPropertyTest()
        {
            stub.Volume = 50;
            Assert.AreEqual(50, stub._vol);
        }

        [Test]
        public void PlayTest()
        {
            Assert.Throws<System.NotImplementedException>(() => stub.Play());
        } 

        [Test]
        public void PauseTest()
        {
            Assert.Throws<System.NotImplementedException>(() => stub.Pause());
        }

        [Test]
        public void StopTest()
        {
            Assert.Throws<System.NotImplementedException>(() => stub.Stop());
        }
    }
}
