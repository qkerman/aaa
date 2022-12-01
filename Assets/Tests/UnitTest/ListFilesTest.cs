using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.IO;
namespace EVA
{
    public class ListFilesTest
    {
        private GameObject scrollview;
        private GameObject prefab;
        private GameObject prefab_image;
        private GameObject prefab_text;
        private GameObject mainUi;
        private GameObject mainPanel;
        private GameObject returnButton;
        private GameObject setttingsButton;
        private GameObject downButton;

        private Sprite icon;

        private List<string> extensions;
        private List<string> patterns;

        private StringEvent func;
        private ListFiles stub;

        private FileStream f1;
        private FileStream f2;
        private FileStream f3;

        [SetUp]
        public void SetUp()
        {
            scrollview = new GameObject();

            prefab = new GameObject();
            prefab.AddComponent<ListItem>();
            prefab_image = new GameObject();
            prefab_image.AddComponent<UnityEngine.UI.Image>();
            prefab_image.transform.SetParent(prefab.transform);
            prefab_text = new GameObject();
            prefab_text.AddComponent<UnityEngine.UI.Text>();
            prefab_text.transform.SetParent(prefab.transform);
            prefab.AddComponent<UnityEngine.UI.Button>();

            mainUi = new GameObject();
            mainPanel = new GameObject();

            extensions = new List<string>() { ".png", ".jpg", ".txt" };
            patterns = new List<string>() { "360" };

            func = new StringEvent();
            Directory.CreateDirectory(Application.persistentDataPath + "\\ListFilesTest");
            _ = new DirectoryInfo(Application.persistentDataPath + "\\ListFilesTest")
            {
                Attributes = FileAttributes.Normal
            };
            f1 = new FileStream(Application.persistentDataPath + "\\ListFilesTest\\logo.png", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            f2 = new FileStream(Application.persistentDataPath + "\\ListFilesTest\\360test1.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            f3 = new FileStream(Application.persistentDataPath + "\\ListFilesTest\\360test2.jpg", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            Directory.CreateDirectory(Application.persistentDataPath + "\\ListFilesTest\\Test");

            returnButton = new GameObject().AddComponent<UnityEngine.UI.Button>().gameObject;
            setttingsButton = new GameObject().AddComponent<UnityEngine.UI.Button>().gameObject;
            downButton = new GameObject().AddComponent<UnityEngine.UI.Button>().gameObject;

            mainUi.AddComponent<UIManager>();
            mainUi.GetComponent<UIManager>().mainPanel = mainPanel;
            stub = mainUi.AddComponent<ListFiles>();
            stub.scrollView = scrollview;
            stub.prefab = prefab;
            stub.mainUi = mainUi;
            stub.icon = icon;
            stub.extensions = extensions;
            stub.patterns = patterns;
            stub.func = func;
            stub.returnButton = returnButton;
            stub.settingsButton = setttingsButton;
            stub.downButton = downButton;
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(scrollview);
            GameObject.Destroy(prefab_image);
            GameObject.Destroy(prefab_text);
            GameObject.Destroy(prefab);
            GameObject.Destroy(mainUi);
            GameObject.Destroy(mainPanel);
            GameObject.Destroy(returnButton);
            GameObject.Destroy(setttingsButton);
            GameObject.Destroy(downButton);

            Sprite.Destroy(icon);

            f1.Close();
            File.Delete(Application.persistentDataPath + "\\ListFilesTest\\logo.png");
            f2.Close();
            File.Delete(Application.persistentDataPath + "\\ListFilesTest\\360test1.txt");
            f3.Close();
            File.Delete(Application.persistentDataPath + "\\ListFilesTest\\360test2.jpg");
            Directory.Delete(Application.persistentDataPath + "\\ListFilesTest\\Test");
            Directory.Delete(Application.persistentDataPath + "\\ListFilesTest");
        }

        [UnityTest]
        public IEnumerator CreateListTest()
        {
            yield return new WaitForFixedUpdate();

            stub.CreateList(Application.persistentDataPath + "\\ListFilesTest");

            Assert.IsTrue(stub.scrollView.activeSelf);
            Assert.AreEqual(scrollview.transform.childCount, 3); //We want the files containing 360 with  .txt / .png / .jpg extension and the folders, here it's : "360test1.txt" "360test2.txt" and "Test" folder, so 3 are expected

            Assert.AreEqual(stub.scrollView, scrollview);
            Assert.AreEqual(stub.prefab, prefab);
            Assert.AreEqual(stub.mainUi, mainUi);
            Assert.AreEqual(stub.icon, icon);
            Assert.AreEqual(stub.extensions, extensions);
            Assert.AreEqual(stub.patterns, patterns);
            Assert.AreEqual(stub.func, func);

            Assert.AreEqual(stub.scrollView.transform.GetChild(0).GetChild(1).GetComponent<Text>().text.CompareTo("Test"), 0);
            Assert.AreEqual(stub.scrollView.transform.GetChild(1).GetChild(1).GetComponent<Text>().text.CompareTo("360test1"), 0);
            Assert.AreEqual(stub.scrollView.transform.GetChild(2).GetChild(1).GetComponent<Text>().text.CompareTo("360test2"), 0);
        }

        [UnityTest]
        public IEnumerator ReturnTest()
        {
            yield return new WaitForFixedUpdate();

            stub.CreateList(Application.persistentDataPath + "\\ListFilesTest\\Test");

            Assert.AreEqual(scrollview.transform.childCount, 0);

            stub.Return();

            Assert.AreEqual(scrollview.transform.childCount, 3);
        }
    }
}
