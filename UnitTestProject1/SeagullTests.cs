using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SeagullAPI.Controllers;
using SeagullAPI.Models;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Results;

namespace UnitTestProject1
{
    [TestClass]
    public class SeagullTests
    {
        public string MasterImagePath
        {
            get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MasterImagePath"); }
        }

        static string ExteriorRepositoryPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "photos\\exterior");

        static string InteriorRepositoryPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "photos\\interior");

        readonly string filepath = "";

        [DataTestMethod]
        [DataRow(4, "exterior")]
        [DataRow(4, "interior")]
        public void PhotosAreSorted(int initialPhotoCount, string category)
        {
            
            string path = category == "exterior" ? ExteriorRepositoryPath : InteriorRepositoryPath;

            InitializeFolder(4, path);
            PhotoRepository repo = new PhotoRepository(path);
            List<string> photos = repo.GetFiles().ToList();

            string[] fileNames = GetFileNames(path);

            bool isSorted = true;
            for(int index = 0; index < fileNames.Length; index++)
            {
                
                if(fileNames[index] == photos[index])
                {
                    continue;
                }

                isSorted = false;
                break;
            }

            Assert.IsTrue(isSorted, "Files Are Not Sorted Properly");
        }


        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        public void GetExteriorPhotos(int initialImageCount)
        {
            //arrange
            InitializeFolder(initialImageCount, ExteriorRepositoryPath);

            //act
            PhotoRepository repo = new PhotoRepository(ExteriorRepositoryPath);
            int photoCount = repo.GetPhotos().Count;

            //assert
            Assert.IsTrue(photoCount == initialImageCount, "Number of photos does not match expected count.");

        }

        [DataTestMethod]
        [DataRow(1,1)]
        [DataRow(2, 1)]
        [DataRow(3, 1)]
        [DataRow(4, 0)]
        public void AddExteriorPhotos(int initialImageCount, int addCount)
        {
            InitializeFolder(initialImageCount, ExteriorRepositoryPath);
            AddFromMaster(addCount, ExteriorRepositoryPath);

            PhotoRepository repo = new PhotoRepository(ExteriorRepositoryPath);
            int photoCount = repo.GetPhotos().Count;

            //assert
            Assert.IsTrue(initialImageCount + addCount ==  photoCount, "Number of photos does not match expected count.");

        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 1)]
        [DataRow(3, 1)]
        [DataRow(4, 3)]
        public void RemoveExteriorPhotos(int initialImageCount, int removeCount)
        {
            InitializeFolder(initialImageCount, ExteriorRepositoryPath);
            string[] files = GetFileNames(ExteriorRepositoryPath);

            for(int index = 0; index < removeCount; index++)
            {
                FileInfo fi = new FileInfo(files[index]);
                File.Move(files[index], Path.Combine(MasterImagePath, fi.Name));
            }

            

            PhotoRepository repo = new PhotoRepository(ExteriorRepositoryPath);
            int photoCount = repo.GetPhotos().Count;

            //assert
            Assert.IsTrue(initialImageCount - removeCount == photoCount, "Number of photos does not match expected count.");

        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        public void GetInteriorPhotos(int initialImageCount)
        {
            //arrange
            InitializeFolder(initialImageCount, InteriorRepositoryPath);

            //act
            PhotoRepository repo = new PhotoRepository(InteriorRepositoryPath);
            int photoCount = repo.GetPhotos().Count;

            //assert
            Assert.IsTrue(photoCount == initialImageCount, "Number of photos does not match expected count.");

        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 1)]
        [DataRow(3, 1)]
        [DataRow(4, 0)]
        public void AddInteriorPhotos(int initialImageCount, int addCount)
        {
            InitializeFolder(initialImageCount, InteriorRepositoryPath);
            AddFromMaster(addCount, InteriorRepositoryPath);

            PhotoRepository repo = new PhotoRepository(InteriorRepositoryPath);
            int photoCount = repo.GetPhotos().Count;

            //assert
            Assert.IsTrue(initialImageCount + addCount == photoCount, "Number of photos does not match expected count.");

        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 1)]
        [DataRow(3, 1)]
        [DataRow(4, 3)]
        public void RemoveInteriorPhotos(int initialImageCount, int removeCount)
        {
            InitializeFolder(initialImageCount, InteriorRepositoryPath);
            string[] files = GetFileNames(InteriorRepositoryPath);

            for (int index = 0; index < removeCount; index++)
            {
                FileInfo fi = new FileInfo(files[index]);
                File.Move(files[index], Path.Combine(MasterImagePath, fi.Name));
            }



            PhotoRepository repo = new PhotoRepository(InteriorRepositoryPath);
            int photoCount = repo.GetPhotos().Count;

            //assert
            Assert.IsTrue(initialImageCount - removeCount == photoCount, "Number of photos does not match expected count.");

        }

        private void InitializeFolder(int photos, string path)
        {
            string[] files = Directory.GetFiles(MasterImagePath);

            for(int index = 0 ; index < photos; index++)
            {
                FileInfo fi = new FileInfo(files[index]);
                File.Move(files[index], Path.Combine(path, fi.Name));
            }
        }

        private string[] GetFileNames(string path)
        {
            return Directory.GetFiles(path);
        }

        private void AddFromMaster(int photos, string path)
        {
            InitializeFolder(photos, path);
        }

        [TestInitialize]
        public void CleanUpImageRepository()
        {
            foreach(string file in Directory.GetFiles(ExteriorRepositoryPath))
            {
                FileInfo fileInfo = new FileInfo(file);
                File.Move(file, Path.Combine(MasterImagePath, fileInfo.Name));
            }

            foreach (string file in Directory.GetFiles(InteriorRepositoryPath))
            {
                FileInfo fileInfo = new FileInfo(file);
                File.Move(file, Path.Combine(MasterImagePath, fileInfo.Name));
            }
        }
    }
}
