using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;

namespace EVA
{
    /*
     * Attention, pour tous les utilisateurs de cette classe de test, cette classe inspecte votre arborescence de fichiers.
     * Cette arborescence étant inconnu, les résultats des tests peuvent variés.
     * Il est donc conseillé de lancer ce test quand le PersistantDataPath est vide, sinon les tests suivants seront en échecs :
     * - GetFilesNullPathTest
     * - GetFilesWhiteSpacePathTest
     * - GetFilesWithFiltersNullPathTest
     * - GetFilesWithFiltersWhiteSpacePathTest
     */
    public class FileChooserTest
    {
        private FileStream f1;
        private FileStream f2;
        private FileStream f3;
        private FileStream f4;

        [SetUp]
        public void SetUp()
        {
            Directory.CreateDirectory(Application.persistentDataPath + "\\EVAFileChooserTest");
            _ = new DirectoryInfo(Application.persistentDataPath + "\\EVAFileChooserTest")
            {
                Attributes = FileAttributes.Normal
            };
            f1 = new FileStream(Application.persistentDataPath + "\\EVAFileChooserTest\\logo.png", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            f2 = new FileStream(Application.persistentDataPath + "\\EVAFileChooserTest\\360test1.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            f3 = new FileStream(Application.persistentDataPath + "\\EVAFileChooserTest\\360test2.jpg", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            f4 = new FileStream(Application.persistentDataPath + "\\EVAFileChooserTest\\test3.mp4", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            Directory.CreateDirectory(Application.persistentDataPath + "\\EVAFileChooserTest\\Nouveau");
            _ = new DirectoryInfo(Application.persistentDataPath + "\\EVAFileChooserTest\\Nouveau")
            {
                Attributes = FileAttributes.Normal
            };
        }

        [TearDown]
        public void TearDown()
        {
            f1.Close();
            File.Delete(Application.persistentDataPath + "\\EVAFileChooserTest\\logo.png");
            f2.Close();
            File.Delete(Application.persistentDataPath + "\\EVAFileChooserTest\\360test1.txt");
            f3.Close();
            File.Delete(Application.persistentDataPath + "\\EVAFileChooserTest\\360test2.jpg");
            f4.Close();
            File.Delete(Application.persistentDataPath + "\\EVAFileChooserTest\\test3.mp4");
            Directory.Delete(Application.persistentDataPath + "\\EVAFileChooserTest\\Nouveau");
            Directory.Delete(Application.persistentDataPath + "\\EVAFileChooserTest");
        }

        [Test]
        public void CurrentPathTest()
        {
            List<string> res = FileChooser.GetFiles(Application.persistentDataPath + "\\EVAFileChooserTest", new List<string> { ".jpg", ".txt", ".mp4" }, new List<string> { "360" });

            Assert.AreEqual(Application.persistentDataPath + "\\EVAFileChooserTest", FileChooser.CurrentPath);
        }

        [Test]
        public void GetFilesTest()
        {
            // On met dans le persistent data path : test1.txt, test2.jpg, test3.mp4, logo.png et le dossier Nouveau
            List<string> res = FileChooser.GetFiles(Application.persistentDataPath + "\\EVAFileChooserTest", new List<string> { ".jpg", ".txt", ".mp4" });

            Assert.IsNotNull(res);
            Assert.AreEqual(4, res.Count);

            Assert.AreEqual(Application.persistentDataPath + "\\EVAFileChooserTest\\360test1.txt", res[1]);
            Assert.AreEqual(Application.persistentDataPath + "\\EVAFileChooserTest\\360test2.jpg", res[2]);
            Assert.AreEqual(Application.persistentDataPath + "\\EVAFileChooserTest\\test3.mp4", res[3]);
            Assert.AreEqual(Application.persistentDataPath + "\\EVAFileChooserTest\\Nouveau", res[0]);
        }

        [Test]
        public void GetFilesNullPathTest()
        {
            // On met dans le persistent data path : test1.txt, test2.jpg, test3.mp4, logo.png et le dossier Nouveau
            List<string> res = FileChooser.GetFiles(null, new List<string> { ".jpg", ".txt", ".mp4" });

            Assert.IsNotNull(res);
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual(Application.persistentDataPath + "\\EVAFileChooserTest", res[0]);
        }

        [Test]
        public void GetFilesWhiteSpacePathTest()
        {
            // On met dans le persistent data path : test1.txt, test2.jpg, test3.mp4, logo.png et le dossier Nouveau
            List<string> res = FileChooser.GetFiles(" ", new List<string> { ".jpg", ".txt", ".mp4" });

            Assert.IsNotNull(res);
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual(Application.persistentDataPath + "\\EVAFileChooserTest", res[0]);
        }

        [Test]
        public void GetFilesWithFiltersTest()
        {
            List<string> res = FileChooser.GetFiles(Application.persistentDataPath + "\\EVAFileChooserTest", new List<string> { ".jpg", ".txt", ".mp4" }, new List<string> { "360" });

            Assert.IsNotNull(res);
            Assert.AreEqual(3, res.Count);

            Assert.AreEqual(Application.persistentDataPath + "\\EVAFileChooserTest\\360test1.txt", res[1]);
            Assert.AreEqual(Application.persistentDataPath + "\\EVAFileChooserTest\\360test2.jpg", res[2]);
            Assert.AreEqual(Application.persistentDataPath + "\\EVAFileChooserTest\\Nouveau", res[0]);
        }

        [Test]
        public void GetFilesWithFiltersNullPathTest()
        {
            // On met dans le persistent data path : test1.txt, test2.jpg, test3.mp4, logo.png et le dossier Nouveau
            List<string> res = FileChooser.GetFiles(null, new List<string> { ".jpg", ".txt", ".mp4" }, new List<string> { "360" });

            Assert.IsNotNull(res);
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual(Application.persistentDataPath + "\\EVAFileChooserTest", res[0]);
        }

        [Test]
        public void GetFilesWithFiltersWhiteSpacePathTest()
        {
            // On met dans le persistent data path : test1.txt, test2.jpg, test3.mp4, logo.png et le dossier Nouveau
            List<string> res = FileChooser.GetFiles(" ", new List<string> { ".jpg", ".txt", ".mp4" }, new List<string> { "360" });

            Assert.IsNotNull(res);
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual(Application.persistentDataPath + "\\EVAFileChooserTest", res[0]);
        }

        [Test]
        public void IsFileExistsTest()
        {
            Assert.IsTrue(FileChooser.IsFileExists(Application.persistentDataPath + "\\EVAFileChooserTest\\360test1.txt"));
            Assert.IsTrue(FileChooser.IsFileExists(Application.persistentDataPath + "\\EVAFileChooserTest\\360test2.jpg"));
            Assert.IsTrue(FileChooser.IsFileExists(Application.persistentDataPath + "\\EVAFileChooserTest\\test3.mp4"));
            Assert.IsFalse(FileChooser.IsFileExists(Application.persistentDataPath + "\\EVAFileChooserTest\\test8.mp4"));
        }

        [Test]
        public void IsDirectoryExistsTest()
        {
            Assert.IsTrue(Directory.Exists(Application.persistentDataPath + "\\EVAFileChooserTest\\Nouveau"));
            Assert.IsFalse(Directory.Exists(Application.persistentDataPath + "\\EVAFileChooserTest\\PasNouveau"));
        }

    }
}
