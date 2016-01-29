using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


/*******************************
    this solution uses Extensions. These extensions can be found in the Extension.cs class
*******************************/


namespace UserFileEditor
{
    public class User
    {

        //fields
        private string _id;
        public string Id {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        private DateTime _dateofbirth;
        public DateTime DateOfBirth
        {
            get
            {
                return _dateofbirth;
            }
            set
            {
                _dateofbirth = value;
            }
        }


        private string _prefix;
        public string Prefix
        {
            get
            {
                return _prefix;
            }
            set
            {
                _prefix = value;
            }
        }

        private string _firstname;
        public string FirstName
        {
            get
            {
                return _firstname;
            }
            set
            {
                _firstname = value;
            }
        }

        private string _middlename;
        public string MiddleName
        {
            get
            {
                return _middlename;
            }
            set
            {
                _middlename = value;
            }
        }

        private string _lastname;
        public string LastName
        { 
            get
            {
                return _lastname;
            }
            set
            {
                _lastname = value;
            }
        }

        private string _suffix;
        public string Suffix
        {
            get
            {
                return _suffix;
            }
            set
            {
                _suffix = value;
            }
        }

        private string _jobtitle;
        private IEnumerable<XElement> docElements;

        public string JobTitle
        {
            get
            {
                return _jobtitle;
            }
            set
            {
                _jobtitle = value;
            }
        }

        //Constructors

        public User()
        {
            Id = "";
            Prefix = "";
            FirstName = "";
            MiddleName = "";
            LastName = "";
            Suffix = "";
            Username = "";
            Password = "";
            DateOfBirth = DateTime.Parse("1/1/2001");
            JobTitle = "";
        }

        public User( string id, string prefix, string firstname, string middlename, string lastname, 
            string suffix, string username, string password, string dateofbirth, string jobtitle)
        {
            DateTime dob;
            if (!DateTime.TryParse(dateofbirth, out dob))
            {
                Exception e = new Exception("Error: DateOfBirth is invalid");
                throw e;
            }
            Id = id;
            Prefix = prefix;
            FirstName = firstname;
            MiddleName = middlename;
            LastName = lastname;
            Suffix = suffix;
            Username = username;
            Password = password;
            DateOfBirth = dob;
            JobTitle = jobtitle;
        }

        public User(XElement xele)
        {
            IEnumerable<XElement> usersXele = xele.Elements();
            foreach(var user in usersXele)
            {
                try {
                    User userName = new User();
                    XElement nameElement = user.Element("Name").IsValidOrDefault();

                    user.Attribute("id").CheckForNull();
                    userName.Id = user.Attribute("id").Value;

                    nameElement.Element("Prefix").IsValidOrDefault(); // check if value is null before assigning it
                    userName.Prefix = nameElement.Element("Prefix").Value;

                    nameElement.Element("FirstName").IsValidOrDefault(); // check if value is null before assigning it
                    userName.FirstName = nameElement.Element("FirstName").Value;

                    nameElement.Element("MiddleName").IsValidOrDefault(); // check if value is null before assigning it
                    userName.MiddleName = nameElement.Element("MiddleName").Value;

                    nameElement.Element("LaseName").IsValidOrDefault(); // check if value is null before assigning it
                    userName.LastName = nameElement.Element("LastName").Value;

                    nameElement.Element("Suffix").IsValidOrDefault(); // check if value is null before assigning it
                    userName.Suffix = nameElement.Element("Suffix").Value;

                    user.Element("Username").IsValidOrDefault();  // check if value is null before assigning it
                    userName.Username = user.Element("Username").Value;

                    user.Element("Username").IsValidOrDefault();  // check if value is null before assigning it
                    userName.Password = user.Element("Password").Value;

                    user.Element("DateOfBirth").IsValidOrDefault();  // check if value is null before assigning it (checking when it is a string, before converting)
                    userName.DateOfBirth = DateTime.Parse(user.Element("DateOfBirth").Value);

                    user.Element("JobTitle").IsValidOrDefault();  // check if value is null before assigning it (checking when it is a string, before converting)
                    userName.JobTitle = user.Element("JobTitle").Value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public User(IEnumerable<XElement> docElements)
        {
            this.docElements = docElements;
        }

        //Methods

        public XElement SaveAsXml()
        {
            XElement ele = new XElement("Root");
            XDocument document = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("users xml file"),
                    ele = new XElement("Users",
                        new XElement("User", new XAttribute("id", 1),
                            new XElement("Username", "TavenerD"),
                            new XElement("Password", "BunBun"),
                            new XElement("DateOfBirth", "9/9/1968"), //XML won't properly display DateTime elements. Stored as a string and converted as needed
                        new XElement("Name",
                            new XElement("Prefix", "Mr"),
                            new XElement("FirstName", "Raymond"),
                            new XElement("MiddleName", "David"),
                            new XElement("LastName", "Tavener"),
                            new XElement("Suffix", "BS")
                            ),
                            new XElement("JobTitle", "Senior Software Developer")),
                        new XElement("User", new XAttribute("id", 2),
                            new XElement("Username", "BlessingN"),
                            new XElement("Password", "BunBun1"),
                            new XElement("DateOfBirth", "8/8/1990"), //XML won't properly display DateTime elements. Stored as a string and converted as needed
                        new XElement("Name",
                            new XElement("Prefix", "Mrs"),
                            new XElement("FirstName", "Nathan"),
                            new XElement("MiddleName", "RobinHood"),
                            new XElement("LastName", "Blessing"),
                            new XElement("Suffix", "")
                            ),
                            new XElement("JobTitle", "Software Engineer")),
                        new XElement("User", new XAttribute("id", 3),
                            new XElement("Username", "WilmsE"),
                            new XElement("Password", "BunBun2"),
                            new XElement("DateOfBirth", "7/7/1977"), //XML won't properly display DateTime elements. Stored as a string and converted as needed
                        new XElement("Name",
                            new XElement("Prefix", "Mr"),
                            new XElement("FirstName", "Eric"),
                            new XElement("MiddleName", "BunBun"),
                            new XElement("LastName", "Wilms"),
                            new XElement("Suffix", "BS")
                            ),
                            new XElement("JobTitle", "Manager, Software Product Engineer")),
                         new XElement("User", new XAttribute("id", 4),
                            new XElement("Username", "HerbertA"),
                            new XElement("Password", "BunBun3"),
                            new XElement("DateOfBirth", "6/6/1988"), //XML won't properly display DateTime elements. Stored as a string and converted as needed
                        new XElement("Name",
                            new XElement("Prefix", "Mrs"),
                            new XElement("FirstName", "Anne"),
                            new XElement("MiddleName", "The Mean"),
                            new XElement("LastName", "Herbert"),
                            new XElement("Suffix", "BS")
                            ),
                            new XElement("JobTitle", "Software Engineer")),
                         new XElement("User", new XAttribute("id", 5),
                            new XElement("Username", "DeanS"),
                            new XElement("Password", "BunBun4"),
                            new XElement("DateOfBirth", "6/19/1992"), //XML won't properly display DateTime elements. Stored as a string and converted as needed
                        new XElement("Name",
                            new XElement("Prefix", "Mr"),
                            new XElement("FirstName", "Spencer"),
                            new XElement("MiddleName", "Steele"),
                            new XElement("LastName", "Dean"),
                            new XElement("Suffix", "")
                    ),
                            new XElement("JobTitle", "Software Development Co-Op"))
                    )
                );

            //save constructed document
            document.Save("users.xml");
            return ele;
        }

    }
}
