using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace EVA
{
    public class ArtworkTest
    {
        private class ArtworkStub : Artwork
        {
            public override SerializedObject GetSerializedObject()
            {
                throw new System.NotImplementedException();
            }

            public bool TestComplete;
            protected override void InitArtwork()
            {
                TestComplete = true;
            }
        }

        // A Test behaves as an ordinary method
        private GameObject gameObject;
        private ArtworkStub stub;
        [SetUp]
        public void SetUp()
        {
            gameObject = new GameObject();
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<ArtworkStub>();
            gameObject.AddComponent<BoxCollider>();
            stub = gameObject.GetComponent<ArtworkStub>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(gameObject);
        }

        [Test]
        public void PathPropertyTest()
        {
            string newPath = "Assets/Resources/Tests/cat.webm";
            stub.Path = newPath;
            Assert.AreEqual(newPath, stub.Path);
            Assert.True(stub.TestComplete);
        }

        [Test]
        public void PathPropertyNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => stub.Path = null);
        }

        [Test]
        public void WrongPathPropertyTest()
        {
            Assert.Throws<ArgumentException>(() => stub.Path = "Je/n/existe/pas");
        }

        [Test]
        public void CollisionGetTest()
        {
            gameObject.GetComponent<Collider>().enabled = false;
            Assert.False(stub.Collision);
        }

        [Test]
        public void CollisionSetTest()
        {
            stub.Collision = false;
            Assert.False(gameObject.GetComponent<Collider>().enabled);
        }

    }
}
