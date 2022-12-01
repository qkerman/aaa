using NUnit.Framework;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace EVA
{
    public class ImageTest
    {
        private GameObject imageObject;
        private GameObject wall;
        private Image image;

        [SetUp]
        public void SetUp()
        {
            imageObject = UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/ImageArtwork.prefab"));
            wall = UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Objects/Wall.prefab"));
            image = imageObject.GetComponent<Image>();
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.Destroy(imageObject);
            UnityEngine.Object.Destroy(wall);
        }

        [Test]
        public void PinTest()
        {
            image.Pin(wall.GetComponent<Wall>(), new Vector3(0, 0, 0));
            Assert.AreEqual(wall, image.transform.parent.gameObject);
        }

        [Test]
        public void PinVersoWall()
        {
            var comparer = QuaternionEqualityComparer.Instance;
            image.Pin(wall.GetComponent<Wall>(), new Vector3(0, 0, 1));
            Assert.That(image.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));
        }

        [Test]
        public void PinRectoWall()
        {
            var comparer = QuaternionEqualityComparer.Instance;
            image.Pin(wall.GetComponent<Wall>(), new Vector3(0, 0, -1));
            Assert.That(image.transform.rotation, Is.EqualTo(new Quaternion(0, 1, 0, 0)).Using(comparer));
        }

        [Test]
        public void UnpinTest()
        {
            GameObject help = new GameObject();
            image.Unpin(help);
            Assert.AreEqual(help, image.transform.parent.gameObject);
        }

        [Test]
        public void InitArtworkTest()
        {
            Texture[] texture = { image.Recto.GetComponent<Renderer>().material.mainTexture, image.Verso.GetComponent<Renderer>().material.mainTexture };
            image.Path = "Assets/Resources/Tests/dnd_logo.png";
            Assert.AreNotEqual(texture[0], image.Recto.GetComponent<Renderer>().material.mainTexture);
            Assert.AreNotEqual(texture[1], image.Verso.GetComponent<Renderer>().material.mainTexture);
        }

        [Test]
        public void InitArtworkNullTest()
        {
            //Test with null
            Assert.Throws<ArgumentNullException>(() => image.Path = null);
        }

        [Test]
        public void InitArtworkFalsePathTest()
        {
            //Test with a false path
            Assert.Throws<ArgumentException>(() => image.Path = "I/Do/Not/Exist");
        }

        [Test]
        public void ChangeParentNotWallTest()
        {
            GameObject help = new GameObject();
            GameObject help2 = new GameObject();
            image.transform.parent = help.transform;
            image.ChangeParent(help2);
            Assert.AreEqual(help2, image.transform.parent.gameObject);
        }

        [Test]
        public void ChangeParentWallTest()
        {
            GameObject help = new GameObject();
            image.transform.parent = wall.transform;
            image.ChangeParent(help);
            Assert.AreEqual(wall, image.transform.parent.gameObject);
        }

        [UnityTest]
        public IEnumerator onDestroyTest()
        {
            image.Path = "Assets/Resources/Tests/dnd_logo.png";
            Texture[] texture = { image.Recto.GetComponent<Renderer>().material.mainTexture, image.Verso.GetComponent<Renderer>().material.mainTexture };
            GameObject recto = image.Recto;
            GameObject verso = image.Verso;
            UnityEngine.Object.Destroy(image);
            yield return new WaitForSeconds(0.1f);
            Assert.AreNotEqual(texture[0], recto.GetComponent<Renderer>().material.mainTexture);
            Assert.AreNotEqual(texture[1], verso.GetComponent<Renderer>().material.mainTexture);
        }
    }
}
