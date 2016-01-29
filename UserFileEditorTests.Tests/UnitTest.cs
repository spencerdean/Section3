using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserFileEditorTests.Tests
{
    [TestClass]
    public class UnitTest
    {
        UserFileEditor.User usr = new UserFileEditor.User(); // User object
        UserFileEditor.UserFile userFile = new UserFileEditor.UserFile(); // UserFile object
        UserFileEditor.UserEditor userEdit = new UserFileEditor.UserEditor(); // UserEditor object
        
        // tries to open a valid user file
        [TestMethod]
        public void OpenUserFile()
        {
            string filePath = (AppDomain.CurrentDomain.BaseDirectory + (@"\users.xml"));
            Assert.AreEqual(AppDomain.CurrentDomain.BaseDirectory + (@"\users.xml"), filePath);
        }

        // tries to open a userFile with a bad file path
        [TestMethod]
        public void OpenBadUserFile()
        {
            string filePath = (AppDomain.CurrentDomain.BaseDirectory + (@"\users.xml"));
            Assert.AreNotEqual(AppDomain.CurrentDomain.BaseDirectory + (@"\badusers.xml"), filePath);
        }

        // test for saving user file
        [TestMethod]
        public void SaveUserFile()
        {
            string usrsFilePath = (AppDomain.CurrentDomain.BaseDirectory + (@"\usrs.xml"));
            userFile.SaveAsXML();
            Assert.AreEqual(usrsFilePath, AppDomain.CurrentDomain.BaseDirectory + (@"\usrs.xml"));
        }

        //test for adding a valid user
        [TestMethod]
        public void AddUser()
        {
            UserFileEditor.User userAdd = new UserFileEditor.User("81","Mr", "James", "P", "Sullivan", "III", "Sully", "testpw1", "06/07/1990", "Lead Scarer");
            userFile.userList.Add(userAdd);
            Assert.AreEqual(userFile.userList.Contains(userAdd), true);
        }

        //test for adding a null user
        [TestMethod]
        [ExpectedException(typeof(Exception), "Error: DateOfBirth is invalid")]
        public void AddNullUser()
        {
            UserFileEditor.User userAddNull = new UserFileEditor.User("", "", "", "", "", "", "", "", "", "");
            userFile.userList.Add(userAddNull);
            Assert.IsNull(userAddNull);
        }

        // test for edit user with valid inputs
        [TestMethod]
        public void EditUser()
        {

        }

        // test for editing a user with invalid inputs
        [TestMethod]
        public void EditUserInvalid()
        {

        }

        // test for deleting a valid user
        [TestMethod]
        public void DeleteUser()
        {

        }

        // test for deleting an invalid user (user doesn't exist)
        [TestMethod]
        public void DeleteBadUser()
        {

        }

        // test for finding a valid user
        [TestMethod]
        public void FindUser()
        {

        }

        // test for finding an invalid user
        [TestMethod]
        public void FindInvalidUser()
        {

        }

        // test to list all users
        [TestMethod]
        public void ListUsers()
        {
            Assert.AreEqual(userFile.ReturnAllUsers(), userFile.userList);
        }


    }
}
