using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;


/*******************************
    this solution uses Extensions. These extensions can be found in the Extension.cs class
*******************************/


namespace UserFileEditor
{
    public class UserFile
    {
        //variables
        public List<User> userList = new List<User>(); // a list of user objects. Users will be added to this list for easy retrieval

        public List<User> ReturnAllUsers()
        {
            return userList;
        }

        // constructors
        public UserFile()
        {
            string filePath = (AppDomain.CurrentDomain.BaseDirectory + (@"\users.xml")); // check current directory (bin) for file

            if (File.Exists(filePath))
            {
                // the file exists, so we can create users from the data
                XDocument document = XDocument.Load("users.xml");
                XElement root;
                IEnumerable<XElement> docElements;
                if (document.Root != null)
                {
                    root = document.Root;
                    if (root.Elements() != null)
                    {
                        docElements = root.Elements();

                        foreach (var d in docElements)
                        {
                            User userName = new User();
                            XElement nameElement = d.Element("Name").IsValidOrDefault();

                            d.Attribute("id").CheckForNull(); // check if value is null before assigning it
                            userName.Id = d.Attribute("id").Value;

                            nameElement.Element("Prefix").IsValidOrDefault(); // check if value is null before assigning it
                            userName.Prefix = nameElement.Element("Prefix").Value;

                            nameElement.Element("FirstName").IsValidOrDefault();  // check if value is null before assigning it
                            userName.FirstName = nameElement.Element("FirstName").Value;

                            nameElement.Element("MiddleName").IsValidOrDefault();  // check if value is null before assigning it
                            userName.MiddleName = nameElement.Element("MiddleName").Value;

                            nameElement.Element("LastName").IsValidOrDefault();  // check if value is null before assigning it
                            userName.LastName = nameElement.Element("LastName").Value;

                            nameElement.Element("Suffix").IsValidOrDefault();  // check if value is null before assigning it
                            userName.Suffix = nameElement.Element("Suffix").Value;

                            d.Element("Username").IsValidOrDefault();  // check if value is null before assigning it
                            userName.Username = d.Element("Username").Value;

                            d.Element("Password").IsValidOrDefault();  // check if value is null before assigning it
                            userName.Password = d.Element("Password").Value;

                            d.Element("DateOfBirth").IsValidOrDefault();  // check if value is null before assigning it (checking when it is a string, before converting)
                            userName.DateOfBirth = DateTime.Parse(d.Element("DateOfBirth").Value);

                            d.Element("JobTitle").IsValidOrDefault();  // check if value is null before assigning it
                            userName.JobTitle = d.Element("JobTitle").Value;

                            userList.Add(userName); //add user to our list of users
                                                    //Console.WriteLine(userList.Count()); //used for testing. Make sure all Users are being added to the list
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Error: root.Elements() is null ");
                    }
                }
                Console.Error.WriteLine("Error: document.Root is null ");
            }
            else
            {
                TestXmlData(); //method call to create test data
                //now that we have data, we can create users from the data
                XDocument document = XDocument.Load("users.xml");
                XElement root = document.Root;
                IEnumerable<XElement> docElements = root.Elements();

                foreach (var d in docElements)
                {
                    User userName = new User();

                    XElement nameElement = d.Element("Name").IsValidOrDefault();

                    d.Attribute("id").CheckForNull(); // check if value is null before assigning it
                    userName.Id = d.Attribute("id").Value; // not null - assign value

                    nameElement.Element("Prefix").IsValidOrDefault(); // check if value is null before assigning it
                    userName.Prefix = nameElement.Element("Prefix").Value; // not null - assign value

                    nameElement.Element("FirstName").IsValidOrDefault();  // check if value is null before assigning it
                    userName.FirstName = nameElement.Element("FirstName").Value; // not null - assign value

                    nameElement.Element("MiddleName").IsValidOrDefault();  // check if value is null before assigning it
                    userName.MiddleName = nameElement.Element("MiddleName").Value; // not null - assign value

                    nameElement.Element("LastName").IsValidOrDefault();  // check if value is null before assigning it
                    userName.LastName = nameElement.Element("LastName").Value; // not null - assign value

                    nameElement.Element("Suffix").IsValidOrDefault();  // check if value is null before assigning it
                    userName.Suffix = nameElement.Element("Suffix").Value; // not null - assign value

                    d.Element("Username").IsValidOrDefault();  // check if value is null before assigning it
                    userName.Username = d.Element("Username").Value; // not null - assign value

                    d.Element("Password").IsValidOrDefault();  // check if value is null before assigning it
                    userName.Password = d.Element("Password").Value; // not null - assign value

                    d.Element("DateOfBirth").IsValidOrDefault();  // check if value is null before assigning it (checking when it is a string, before converting)
                    userName.DateOfBirth = DateTime.Parse(d.Element("DateOfBirth").Value); // not null - assign value & do the conversion

                    d.Element("JobTitle").IsValidOrDefault();  // check if value is null before assigning it
                    userName.JobTitle = d.Element("JobTitle").Value; // not null - assign value

                    userList.Add(userName); //add user to our list of users
                    //Console.WriteLine(userList.Count()); //used for testing. Make sure all Users are being added to the list
                }
            }
        }

        // methods

        //method to populate with test data if users.xml is not found by the constructor
        public void TestXmlData()
        {
            XElement ele = new XElement("Root");
            XDocument document = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Test XML Data"),
                    ele = new XElement("Users",
                        new XElement("User", new XAttribute("id", 1),
                            new XElement("Username", "ImBatman"),
                            new XElement("Password", "BunBun"),
                            new XElement("DateOfBirth", "10/10/1970"), //XML won't properly display DateTime elements. Stored as a string and converted as needed
                        new XElement("Name",
                            new XElement("Prefix", "Mr"),
                            new XElement("FirstName", "Bruce"),
                            new XElement("MiddleName", "TheBatman"),
                            new XElement("LastName", "Wayne"),
                            new XElement("Suffix", "")
                            ),
                            new XElement("JobTitle", "Dark Knight")),
                        new XElement("User", new XAttribute("id", 2),
                            new XElement("Username", "Superman"),
                            new XElement("Password", "BunBun1"),
                            new XElement("DateOfBirth", "7/7/1960"), //XML won't properly display DateTime elements. Stored as a string and converted as needed
                        new XElement("Name",
                            new XElement("Prefix", "Mr"),
                            new XElement("FirstName", "Clark"),
                            new XElement("MiddleName", "Kal-El"),
                            new XElement("LastName", "Kent"),
                            new XElement("Suffix", "")
                            ),
                            new XElement("JobTitle", "Journalist")),
                        new XElement("User", new XAttribute("id", 3),
                            new XElement("Username", "Starlord"),
                            new XElement("Password", "BunBun2"),
                            new XElement("DateOfBirth", "5/5/1975"), //XML won't properly display DateTime elements. Stored as a string and converted as needed
                        new XElement("Name",
                            new XElement("Prefix", "Mr"),
                            new XElement("FirstName", "Peter"),
                            new XElement("MiddleName", ""),
                            new XElement("LastName", "Quill"),
                            new XElement("Suffix", "")
                            ),
                            new XElement("JobTitle", "Gaurdian of the Galaxy"))
                    )
                );
            //save constructed document
            document.Save("users.xml");
        }

        public void SaveAsXML()
        {
            // NULLs will be checked for prior to assigning them to a User object - so there is no need to check for them here
            XDocument xmlDoc = new XDocument(

                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Save User objects as XML || usrs.xml"),
                new XElement("Users",
                    from usr in userList
                    select new XElement("User", new XAttribute("ID", usr.Id),
                            new XElement("Username", usr.Username),
                            new XElement("Password", usr.Password),
                            new XElement("DateOfBirth", usr.DateOfBirth),
                    new XElement("Name",
                            new XElement("Prefix", usr.Prefix),
                            new XElement("FirstName", usr.FirstName),
                            new XElement("MiddleName", usr.MiddleName),
                            new XElement("LastName", usr.LastName),
                            new XElement("Suffix", usr.Suffix)
                            ),
                            new XElement("JobTitle", usr.JobTitle))
                        )
                );
            xmlDoc.Save("usrs.xml"); //Different from the file users.xml
        }

        public void RemoveUser(XAttribute id)
        {
            userList.RemoveAll(x => x.Id == id.Value);
            SaveAsXML();
            //Console.WriteLine(userList.Count);
        }

    }
}
