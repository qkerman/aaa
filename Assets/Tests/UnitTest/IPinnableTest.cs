using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace EVA
{
    public class IPinnableTest
    {
        private class IPinnableStub : IPinnable
        {
            public Wall pinned;
            public bool IsPinned => pinned is object;

            public void Pin(Wall wall, Vector3 direction)
            {
                pinned = wall;
            }

            public void Unpin(GameObject parent)
            {
                pinned = null;
            }
        }

        private IPinnableStub stub;
        private Wall wall;

        [SetUp]
        public void SetUp()
        {
            stub = new IPinnableStub();
            wall = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/Wall.prefab")).GetComponent<Wall>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(wall);
        }

        [Test]
        public void IsPinnedTest()
        {
            stub.pinned = wall;
            Assert.IsTrue(stub.IsPinned);
        }

        [Test]
        public void PinTest()
        {
            stub.Pin(wall, Vector3.one);
            Assert.IsTrue(stub.IsPinned);
            Assert.AreEqual(wall, stub.pinned);
        }

        [Test]
        public void UnpinTest()
        {
            GameObject parent = new GameObject();
            stub.pinned = wall;
            stub.Unpin(parent);
            Assert.AreEqual(null, stub.pinned);
        }
    }
}