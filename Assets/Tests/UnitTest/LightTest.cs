using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System;

namespace EVA
{
    public class LightTest
    {
        public GameObject gameObject;
        public Light testedObject;

        [SetUp]
        public void SetUp()
        {
            gameObject = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/Light.prefab"));
            testedObject = gameObject.GetComponent<Light>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator SetColorTest()
        {
            testedObject.Color = Color.red;
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(Color.red, gameObject.GetComponent<UnityEngine.Light>().color);
            Assert.AreEqual(Color.red, testedObject.dirModel.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"));
            Assert.AreEqual(Color.red, testedObject.pointModel.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"));
            Assert.AreEqual(Color.red, testedObject.spotModel.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"));
        }

        [Test]
        public void GetColorTest()
        {
            gameObject.GetComponent<UnityEngine.Light>().color = Color.red;
            Assert.AreEqual(Color.red, testedObject.Color);
        }

        [Test]
        public void SetRangeTest()
        {
            testedObject.Range = 10;
            Assert.AreEqual(10, gameObject.GetComponent<UnityEngine.Light>().range);
        }

        [Test]
        public void GetRangeTest()
        {
            gameObject.GetComponent<UnityEngine.Light>().range = 10;
            Assert.AreEqual(10, testedObject.Range);
        }

        [Test]
        public void SetSpotAngleTest()
        {
            testedObject.SpotAngle = 10;
            Assert.AreEqual(10, gameObject.GetComponent<UnityEngine.Light>().spotAngle);
        }

        [Test]
        public void GetSpotAngleTest()
        {
            gameObject.GetComponent<UnityEngine.Light>().spotAngle = 10;
            Assert.AreEqual(10, testedObject.SpotAngle);
        }

        [Test]
        public void SetIntensityTest()
        {
            testedObject.Intensity = 10;
            Assert.AreEqual(10, gameObject.GetComponent<UnityEngine.Light>().intensity);
        }

        [Test]
        public void GetIntensityTest()
        {
            gameObject.GetComponent<UnityEngine.Light>().intensity = 10;
            Assert.AreEqual(10, testedObject.Intensity);
        }

        [Test]
        public void SetCullingMaskTest()
        {
            testedObject.CullingMask = 10;
            Assert.AreEqual(10, gameObject.GetComponent<UnityEngine.Light>().cullingMask);
        }

        [Test]
        public void GetCullingMaskTest()
        {
            gameObject.GetComponent<UnityEngine.Light>().cullingMask = 10;
            Assert.AreEqual(10, testedObject.CullingMask.value);
        }

        [Test]
        public void GetTypeTest()
        {
            gameObject.GetComponent<UnityEngine.Light>().type = LightType.Spot;
            Assert.AreEqual(LightType.Spot, testedObject.Type);
        }

        [Test]
        public void SetTypeTest()
        {
            testedObject.Type = LightType.Spot;
            Assert.AreEqual(LightType.Spot, testedObject.Type);
        }
        
        [Test]
        public void SetTypeNotImplementedTest()
        {
            Assert.Throws<ArgumentException>(() => testedObject.Type = LightType.Area);
        }

        [Test]
        public void SetModelVisibilityTest()
        {
            testedObject.ModelVisibility = false;
            Assert.False(testedObject.spotModel.activeSelf);
        }

        [Test]
        public void GetModelVisibilityTest()
        {
            testedObject.spotModel.SetActive(false);
            Assert.False(testedObject.ModelVisibility);
        }

        [Test]
        public void SetSwitchTest()
        {
            testedObject.Switch = true;
            Assert.True(gameObject.GetComponent<UnityEngine.Light>().isActiveAndEnabled);
            Assert.AreEqual(gameObject.GetComponent<UnityEngine.Light>().color, testedObject.dirModel.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"));
            Assert.AreEqual(gameObject.GetComponent<UnityEngine.Light>().color, testedObject.spotModel.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"));
            Assert.AreEqual(gameObject.GetComponent<UnityEngine.Light>().color, testedObject.pointModel.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"));
        }

        [Test]
        public void GetSwitchTest()
        {
            gameObject.GetComponent<UnityEngine.Light>().enabled = false;
            testedObject.dirModel.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
            testedObject.spotModel.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
            testedObject.pointModel.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
            Assert.False(testedObject.Switch);
        }

        [Test]
        public void RemoveCullingMaskLayerTest()
        {
            int previous = gameObject.GetComponent<UnityEngine.Light>().cullingMask;
            testedObject.RemoveCullingMaskLayer(2);
            Assert.AreNotEqual(previous, gameObject.GetComponent<UnityEngine.Light>().cullingMask);
        }

        [Test]
        public void AddCullingMaskLayerTest()
        {
            int previous = gameObject.GetComponent<UnityEngine.Light>().cullingMask;
            testedObject.RemoveCullingMaskLayer(2);
            testedObject.AddCullingMaskLayer(2);
            Assert.AreEqual(previous, gameObject.GetComponent<UnityEngine.Light>().cullingMask);
        }

        [Test]
        public void SetCullingMaskToEverythingTest()
        {
            testedObject.RemoveCullingMaskLayer(2);
            testedObject.RemoveCullingMaskLayer(1);
            testedObject.SetCullingMaskToEverything();
            Assert.AreEqual(-1, gameObject.GetComponent<UnityEngine.Light>().cullingMask);
        }

        [Test]
        public void SetCullingMaskToNothingTest()
        {
            testedObject.SetCullingMaskToNothing();
            Assert.AreEqual(0, gameObject.GetComponent<UnityEngine.Light>().cullingMask);
        }
    }
}