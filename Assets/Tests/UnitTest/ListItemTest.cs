using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
namespace EVA
{
    public class ListItemTest
    {

        private GameObject gameObject;
        private GameObject childrenImage;
        private GameObject childrenText;
        private ListItem stub;
        private readonly Sprite _sprite;

        [SetUp]
        public void SetUp()
        {
            gameObject = new GameObject();

            childrenImage = new GameObject();
            childrenImage.AddComponent<UnityEngine.UI.Image>();
            childrenImage.transform.parent = gameObject.transform;

            childrenText = new GameObject();
            childrenText.AddComponent<Text>();
            childrenText.transform.parent = gameObject.transform;

            gameObject.AddComponent<UnityEngine.UI.Button>();
            gameObject.AddComponent<ListItem>();
            stub = gameObject.GetComponent<ListItem>();
        }

        [TearDown]
        public void TearDown()
        {
            Sprite.Destroy(_sprite);
            Object.Destroy(childrenText);
            Object.Destroy(childrenImage);
            Object.Destroy(gameObject);
        }

        [Test]
        public void ItemImageGetTest()
        {
            gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = _sprite;
            Assert.AreEqual(stub.ItemImage, _sprite);
        }

        [Test]
        public void ItemImageSetTest()
        {
            stub.ItemImage = _sprite;
            Assert.AreEqual(gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite, _sprite);
        }

        [Test]
        public void ItemTextGetTest()
        {
            gameObject.transform.GetChild(1).GetComponent<Text>().text = "Bonjour";
            Assert.AreEqual(stub.ItemText, "Bonjour");
        }

        [Test]
        public void ItemTextSetTest()
        {
            stub.ItemText = "Bonjour";
            Assert.AreEqual(gameObject.transform.GetChild(1).GetComponent<Text>().text, "Bonjour");
        }

        [Test]
        public void ItemEventTest()
        {
            Assert.AreEqual(stub.ItemEvent, gameObject.GetComponent<UnityEngine.UI.Button>().onClick);
        }
    }
}