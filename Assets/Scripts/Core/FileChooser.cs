using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace EVA
{
    /// <summary>
    /// This class is used to manage the methods linked
    /// to the file chooser and the navigation between folders.
    /// </summary>
    public static class FileChooser
    {

        /// <summary>
        /// This string represents the path of the current folder we are in.
        /// </summary>
        /// <remarks>
        /// This attributes is static in order to be accessed from anywhere.</remarks>
        private static string current_path;

        /// <summary>
        /// This property allows the access to the current_path attribute.
        /// </summary>
        public static string CurrentPath
        {
            get => current_path;
        }

        /// <summary>
        /// This method is used to get the file names of all the files
        /// with a specific extension present in a specific path.
        /// </summary>
        /// <param name="m_path">The path which we want to get the files.</param>
        /// <param name="extensions">A filter for the type of file we want to get.</param>
        /// <returns>The method returns a string list corresponding to the name of all the files in the path m_path.</returns>
        public static List<string> GetFiles(string m_path, List<string> extensions)
        {

            if (String.IsNullOrWhiteSpace(m_path))
            {
                m_path = Application.persistentDataPath;
            }
            current_path = Path.GetFullPath(m_path);
            List<string> returnFiles = new List<string>();
            List<string> returnFolders = new List<string>();
            List<string> fileEntries = new List<string>(Directory.GetFiles(m_path));
            List<string> foldersEntries = new List<string>(Directory.GetDirectories(m_path));

            foreach (string path in fileEntries)
            {
                if (IsFileExists(path))
                {
                    string extension = Path.GetExtension(path).ToLowerInvariant();
                    foreach (string ext in extensions)
                    {
                        if (extension.CompareTo(ext) == 0)
                        {
                            returnFiles.Add(path);
                        }
                    }
                }
            }

            foreach (string pathFolders in foldersEntries)
            {
                if (IsFolderExists(m_path))
                {
                    returnFolders.Add(pathFolders);
                }

            }

            returnFolders.Sort();
            returnFiles.Sort();
            returnFolders.AddRange(returnFiles);

            return returnFolders;
        }

        /// <summary>
        /// This method is used to get the file names of all the files
        /// with a specific extension present in a specific path with a
        /// recognisable pattern in the file name(like 360).
        /// </summary>
        /// <param name="m_path">The path which we want to get the files.</param>
        /// <param name="extensions">A filter for the type of file we want to get.</param>
        /// <param name="patterns">A pattern that must be recognise in the file name.</param>
        /// <returns>The method returns a string list corresponding to the name of all the files in the path m_path respecting the patterns.</returns>
        public static List<string> GetFiles(string m_path, List<string> extensions, List<string> patterns)
        {

            if (String.IsNullOrWhiteSpace(m_path))
            {
                m_path = Application.persistentDataPath;
            }
            current_path = m_path;
            List<string> returnFiles = new List<string>();
            List<string> returnFolders = new List<string>();
            List<string> fileEntries = new List<string>(Directory.GetFiles(m_path));
            List<string> foldersEntries = new List<string>(Directory.GetDirectories(m_path));

            foreach (string path in fileEntries)
            {
                if (IsFileExists(path))
                {
                    foreach (string pattern in patterns)
                    {
                        if (Path.GetFileName(path).Contains(pattern))
                        {
                            string extension = Path.GetExtension(path).ToLowerInvariant();
                            foreach (string ext in extensions)
                            {
                                if (extension.CompareTo(ext) == 0)
                                {
                                    returnFiles.Add(path);
                                }
                            }
                        }
                    }
                }
            }

            foreach (string pathFolders in foldersEntries)
            {
                if (IsFolderExists(m_path))
                {
                    returnFolders.Add(pathFolders);
                }

            }

            returnFolders.Sort();
            returnFiles.Sort();
            returnFolders.AddRange(returnFiles);

            return returnFolders;
        }

        /// <summary>
        /// A static function used to test if files exist.
        /// </summary>
        /// <param name="m_path">The path where we want to test if there is files.</param>
        /// <returns>The method returns a boolean that tells if files exist in the given path.</returns>
        public static bool IsFileExists(string m_path)
        {
            return (File.Exists(m_path));
        }

        /// <summary>
        /// A static function used to test if folders exist.
        /// </summary>
        /// <param name="m_path">The path where we want to test if there is folders.</param>
        /// <returns>The method returns a boolean that tells if folders exist in the given path.</returns>
        public static bool IsFolderExists(string m_path)
        {
            return (Directory.Exists(m_path));
        }
    }
}
