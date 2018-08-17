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

namespace UnitTestProject1
{
    [TestClass]
    public class SeagullTests
    {
        public string MasterImagePath
        {
            get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MasterImagePath"); }
        }

        public string ExteriorRepositoryPath
        {
            get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "photos\\exterior"); }
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        public void GetPhotos(int initialImageCount)
        {
            //arrange
            InitializeFolder(initialImageCount);

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
        public void AddPhotos(int initialImageCount, int addCount)
        {
            InitializeFolder(initialImageCount);
            AddFromMaster(addCount);

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
        public void RemovePhotos(int initialImageCount, int removeCount)
        {
            InitializeFolder(initialImageCount);
            string[] files = GetFileNames();

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

        private void InitializeFolder(int photos)
        {
            string[] files = Directory.GetFiles(MasterImagePath);

            for(int index = 0 ; index < photos; index++)
            {
                FileInfo fi = new FileInfo(files[index]);
                File.Move(files[index], Path.Combine(ExteriorRepositoryPath, fi.Name));
            }
        }

        private string[] GetFileNames()
        {
            return Directory.GetFiles(ExteriorRepositoryPath);
        }

        private void AddFromMaster(int photos)
        {
            InitializeFolder(photos);
        }

        [TestInitialize]
        public void CleanUpImageRepository()
        {
            foreach(string file in Directory.GetFiles(ExteriorRepositoryPath))
            {
                FileInfo fileInfo = new FileInfo(file);
                File.Move(file, Path.Combine(MasterImagePath, fileInfo.Name));
            }
        }
    }
}
