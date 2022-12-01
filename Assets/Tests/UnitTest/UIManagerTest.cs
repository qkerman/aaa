using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace EVA
{
    public class UIManagerTest
    {
        private GameObject panel;
        private GameObject paneltest;
        private GameObject canva;
        private UIManager stub;
        private GameObject firstButton;
        private GameObject settings;
        private UnityEngine.EventSystems.EventSystem eventSystem;

        [SetUp]
        public void SetUp()
        {
            panel = new GameObject();
            eventSystem = new GameObject().AddComponent<UnityEngine.EventSystems.EventSystem>();
            paneltest = new GameObject();
            canva = new GameObject();
            panel.SetActive(false);
            paneltest.SetActive(false);
            canva.AddComponent<UnityEngine.Canvas>();
            stub = canva.AddComponent<UIManager>();
            stub.mainPanel = panel;
            stub.rig = new GameObject();
            firstButton = new GameObject().AddComponent<UnityEngine.UI.Button>().gameObject;
            settings = new GameObject().AddComponent<UnityEngine.UI.Button>().gameObject;
            stub.firstButton = firstButton;
            stub.settings = settings;

        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(panel);
            Object.Destroy(paneltest);
            Object.Destroy(canva);
            Object.Destroy(firstButton);
            Object.Destroy(settings);
            Object.Destroy(eventSystem);
        }

        [Test]
        public void NextTest()
        {
            stub.Next(panel);
            Assert.IsTrue(panel.activeSelf);
            stub.Next(paneltest);
            Assert.IsTrue(paneltest.activeSelf);
            Assert.IsFalse(panel.activeSelf);
        }

        [Test]
        public void ReturnTest()
        {
            stub.Next(panel);
            stub.Next(paneltest);
            stub.Return();
            Assert.IsFalse(paneltest.activeSelf);
            Assert.IsTrue(panel.activeSelf);
        }

        [Test]
        public void OpenTest()
        {
            stub.Open();
            Assert.IsTrue(stub.GetComponent<UnityEngine.Canvas>().enabled);
        }

        [Test]
        public void CloseTest()
        {
            stub.Close();
            Assert.IsFalse(stub.GetComponent<UnityEngine.Canvas>().enabled);
        }
    }
}
