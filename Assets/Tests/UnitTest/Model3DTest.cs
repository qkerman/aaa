using NUnit.Framework;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace EVA
{
    public class Model3DTest
    {
        public GameObject gameObject;
        public Model3D testedObject;

        [SetUp]
        public void SetUp()
        {
            gameObject = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/ModelArtwork.prefab"));
            testedObject = gameObject.GetComponent<Model3D>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(gameObject);
        }

        [Test]
        public void InitArtworkNotGLBTest()
        {
            Assert.Throws<ArgumentException>(() => testedObject.Path = "Assets/Resources/Tests/cat.mp4");
        }

        [UnityTest]
        public IEnumerator InitArtworkSmallObjectTest()
        {
            testedObject.Path = "Assets/Resources/Tests/pipoudou.glb";
            yield return new WaitForSeconds(1f);
            Assert.IsTrue(gameObject.transform.childCount >= 1);
            Assert.IsTrue(gameObject.GetComponentsInChildren<Collider>().Length >= 1);
            Assert.IsTrue(gameObject.GetComponent<Outline>());
        }

        [UnityTest]
        public IEnumerator InitArtworkBigObjectTest()
        {
            testedObject.Path = "Assets/Resources/Tests/Persian.glb";
            yield return new WaitForSeconds(5f);
            Assert.IsTrue(gameObject.transform.childCount >= 1);
            Assert.IsTrue(gameObject.GetComponentsInChildren<Collider>().Length >= 1);
            Assert.IsTrue(gameObject.GetComponent<Outline>());
        }
    }
}