using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace EVA
{
    public class ObjectTest
    {
        private class ObjectStub : Object
        {
            public override SerializedObject GetSerializedObject()
            {
                throw new System.NotImplementedException();
            }
        }

        private GameObject gameObject;
        private ObjectStub stub;
        [SetUp]
        public void SetUp()
        {
            gameObject = new GameObject();
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<ObjectStub>();
            stub = gameObject.GetComponent<ObjectStub>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(gameObject);
        }

        [Test]
        public void GravityGetTest()
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            Assert.True(stub.Gravity);
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            Assert.False(stub.Gravity);
        }

        [Test]
        public void GravitySetTest()
        {
            stub.Gravity = true;
            Assert.True(gameObject.GetComponent<Rigidbody>().useGravity);
            stub.Gravity = false;
            Assert.False(gameObject.GetComponent<Rigidbody>().useGravity);
        }

        [Test]
        public void LayerGetTest()
        {
            gameObject.layer = 2;
            Assert.AreEqual(2, stub.Layer);
        }

        [Test]
        public void LayerSetTest()
        {
            stub.Layer = 3;
            Assert.AreEqual(3, gameObject.layer);
        }

        [Test]
        public void ActiveGetTest()
        {
            gameObject.SetActive(true);
            Assert.True(stub.Active);
            gameObject.SetActive(false);
            Assert.False(stub.Active);
        }

        [Test]
        public void ActiveSetTest()
        {
            stub.Active = true;
            Assert.True(gameObject.activeSelf);
            stub.Active = false;
            Assert.False(gameObject.activeSelf);
        }

        [Test]
        public void RotateXTest()
        {
            var comparer = new FloatEqualityComparer(10e-1f);
            float xRotation = 5;
            stub.Rotate(xRotation, Axes.X);
            Assert.That(stub.transform.rotation.eulerAngles.x, Is.EqualTo(xRotation).Using(comparer));
        }

        [Test]
        public void RotateYTest()
        {
            var comparer = new FloatEqualityComparer(10e-1f);
            float yRotation = 3;
            stub.Rotate(yRotation, Axes.Y);
            Assert.That(stub.transform.rotation.eulerAngles.y, Is.EqualTo(yRotation).Using(comparer));
        }

        [Test]
        public void RotateZTest()
        {
            var comparer = new FloatEqualityComparer(10e-1f);
            float zRotation = 7;
            stub.Rotate(zRotation, Axes.Z);
            Assert.That(stub.transform.rotation.eulerAngles.z, Is.EqualTo(zRotation).Using(comparer));
        }
        [Test]
        public void RotateALLTest()
        {
            var comparer = new FloatEqualityComparer(10e-1f);
            float allRotation = 10;
            stub.Rotate(allRotation, Axes.ALL);
            Assert.That(stub.transform.rotation.eulerAngles.x, Is.EqualTo(allRotation).Using(comparer));
            Assert.That(stub.transform.rotation.eulerAngles.y, Is.EqualTo(allRotation).Using(comparer));
            Assert.That(stub.transform.rotation.eulerAngles.z, Is.EqualTo(allRotation).Using(comparer));
        }

        [Test]
        public void ResizeXTest()
        {
            var comparer = Vector3ComparerWithEqualsOperator.Instance;
            float xScale = 5;
            stub.Resize(xScale, Axes.X);
            Assert.That(stub.transform.localScale, Is.EqualTo(new Vector3(xScale + 1, 1, 1)).Using(comparer));
        }

        [Test]
        public void ResizeYTest()
        {
            var comparer = Vector3ComparerWithEqualsOperator.Instance;
            float yScale = 5;
            stub.Resize(yScale, Axes.Y);
            Assert.That(stub.transform.localScale, Is.EqualTo(new Vector3(1, yScale + 1, 1)).Using(comparer));
        }

        [Test]
        public void ResizeZTest()
        {
            var comparer = Vector3ComparerWithEqualsOperator.Instance;
            float zScale = 5;
            stub.Resize(zScale, Axes.Z);
            Assert.That(stub.transform.localScale, Is.EqualTo(new Vector3(1, 1, zScale + 1)).Using(comparer));
        }

        [Test]
        public void ResizeALLTest()
        {
            var comparer = Vector3ComparerWithEqualsOperator.Instance;
            float allScale = 5;
            stub.Resize(allScale, Axes.ALL);
            Assert.That(stub.transform.localScale, Is.EqualTo(new Vector3(allScale + 1, allScale + 1, allScale + 1)).Using(comparer));
        }


        [Test]
        public void TranslateTest()
        {
            var comparer = Vector3ComparerWithEqualsOperator.Instance;
            stub.Translate(new Vector3(1, 1, 1));
            Assert.That(stub.transform.position, Is.EqualTo(new Vector3(1, 1, 1)).Using(comparer));
            stub.Translate(new Vector3(0, 8, 0));
            Assert.That(stub.transform.position, Is.EqualTo(new Vector3(1, 9, 1)).Using(comparer));
        }

        [Test]
        public void SetRotationTest()
        {
            var comparer = QuaternionEqualityComparer.Instance;
            stub.SetRotation(new Quaternion(1, 1, 1, 1));
            Assert.That(stub.transform.rotation, Is.EqualTo(new Quaternion(1,1,1,1)).Using(comparer));
        }

        [Test]
        public void SetPositionTest()
        {
            var comparer = Vector3ComparerWithEqualsOperator.Instance;
            stub.SetPosition(new Vector3(10,2,87));
            Assert.That(stub.transform.position, Is.EqualTo(new Vector3(10, 2, 87)).Using(comparer));
        }

        [Test]
        public void SetScaleTest()
        {
            var comparer = Vector3ComparerWithEqualsOperator.Instance;
            stub.SetScale(new Vector3(10, 2, 87));
            Assert.That(stub.transform.localScale, Is.EqualTo(new Vector3(10, 2, 87)).Using(comparer));
        }

        [Test]
        public void ChangeParentTest()
        {
            GameObject parent = new GameObject();
            stub.ChangeParent(parent);
            Assert.AreEqual(parent, stub.transform.parent.gameObject);
        }

        [Test]
        public void GetSerializedObjectTest()
        {
            Assert.Throws<NotImplementedException>(() => {
                stub.GetSerializedObject();
            });
        }
    }
}
