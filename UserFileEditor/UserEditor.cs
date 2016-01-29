using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Forms;



/*******************************
    this solution uses Extensions. These extensions can be found in the Extension.cs class
*******************************/


namespace UserFileEditor
{
    public class UserEditor
    {
        string consoleUserID = ""; // Initializing string for use in AddUser() method
        string consoleUserName = ""; // Initializing string for use in AddUser() method
        string consolePassword = ""; // Initializing string for use in AddUser() method
        string consoleBday = ""; // Initializing string for use in AddUser() method
        string consoleFirstName = ""; // Initializing string for use in AddUser() method
        string consoleLastName = ""; // Initializing string for use in AddUser() method
        DateTime tempDoB; // used in AddUser() method for Date validation


        XAttribute returnID; // used for the FindUserByID method

        UserFile usersInfo = new UserFile();

        public UserEditor()
        {
            UserFile newUserEditor = new UserFile();
        }

        // display menu options
        public void DisplayMenu() 
        {
            Console.WriteLine("1: Add a user");
            Console.WriteLine("2: Delete a user");
            Console.WriteLine("3: Find a user");
            Console.WriteLine("4: Edit a user");
            Console.WriteLine("5: List all users");
            Console.WriteLine("6: Quit");
        }

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("Please make a selection from the menu below");
            DisplayMenu(); // display the menu
            string userInput = string.Empty; // initialize userInput

            while (userInput != "6")
            {
                userInput = Console.ReadLine(); 

                switch (userInput)
                {
                    case "1":
                        AddUser(); // call AddUser method
                        break;
                    case "2":
                        DeleteUser(); // call DeleteUser method
                        break;
                    case "3":
                        FindUser(); // call FindUser method
                        break;
                    case "4":
                        EditUser(); // call EditUser method
                        break;
                    case "5":
                        ListAllUsers(); // call ListAllUsers method
                        break;
                    case "6":
                        break;
                    default:
                        Console.Error.WriteLine("Error: Invalid selection made. Please make a valid selection ");
                        DisplayMenu();
                        continue;
                    }
                }
            if(userInput == "6")
            {
                Quit();
            }
        }


        // add a new user
        public void AddUser()
        {
            // for user ID we are going to loop through the list of users and extraxt their IDs. Then, we will set the new user ID to the 
            //// max value + 1
            List<int> userIds = new List<int>();
            var maxId = 0;
            foreach (User usr in usersInfo.userList)
            {
                int tempId = int.Parse(usr.Id);
                userIds.Add(tempId);
                maxId = userIds.Max(); // get max value
                if(tempId > maxId)
                {
                    maxId = tempId;
                }
            }
            consoleUserID = (maxId + 1).ToString(); // set console ID to max value +1 then convert it back to a string for XML

            do // don't move on until field Username has a value
            {
                Console.WriteLine("Please enter a Username: ");
                consoleUserName = Console.ReadLine(); // get user input Username
            }
            while (string.IsNullOrWhiteSpace(consoleUserName));

            do // don't move on until field Password has a value
            {
                Console.WriteLine("Please enter a Password: ");
                consolePassword = Console.ReadLine(); // get user input Password
            }
            while (string.IsNullOrWhiteSpace(consolePassword));

            do // don't move on until field DateOfBirth has a value
            {
                Console.WriteLine("Please enter a valid Date of Birth (MM/DD/YYYY): ");
                consoleBday = Console.ReadLine(); // get user input Date of Birth
            }
            // checks to make sure DoB is not null & that DoB is valid
            while (string.IsNullOrWhiteSpace(consoleBday) || (!DateTime.TryParse(consoleBday, out tempDoB)));

            Console.WriteLine("Please enter a Prefix: ");
            string consolePrefix = Console.ReadLine(); // get user input Prefix

            do // don't move on until field FirstName has a value
            {
                Console.WriteLine("Please enter a First Name: ");
                consoleFirstName = Console.ReadLine(); // get user input First Name
            }
            while (string.IsNullOrWhiteSpace(consoleFirstName));

            Console.WriteLine("Please enter a Middle Name: ");
            string consoleMiddleName = Console.ReadLine(); // get user input Middle Name

            do // don't move on until field LastName has a value
            {
                Console.WriteLine("Please enter a Last Name: ");
                consoleLastName = Console.ReadLine(); // get user input Last Name
            }
            while (string.IsNullOrWhiteSpace(consoleLastName));

            Console.WriteLine("Please enter a Suffix: ");
            string consoleSuffix = Console.ReadLine(); // get user input Suffix

            Console.WriteLine("Please enter a Job Title: ");
            string consoleJobTitle = Console.ReadLine(); // get user input JobTitle

            //we have all the values we need, now create a new user
            User newConsoleUser = new User(consoleUserID.ToString(), consolePrefix, consoleFirstName, consoleMiddleName, consoleLastName, 
                consoleSuffix, consoleUserName, consolePassword, consoleBday, consoleJobTitle);

            usersInfo.userList.Add(newConsoleUser); // add this new user to our userList
            usersInfo.SaveAsXML(); // save XML file of all our users
            Console.WriteLine("Saving... User saved successfully!");
            System.Threading.Thread.Sleep(1000);
            ListAllUsers(); // list users to make sure our user was successfully added
        }

        // delete a user
        public void DeleteUser()
        {
            XDocument document = XDocument.Load("Users.xml");
            XElement root;
            IEnumerable<XElement> docElements;
            if (document.Root != null)
            {
                root = document.Root;
                if (root.Elements() != null)
                {
                    docElements = root.Elements();
                    XAttribute FindUserByIdValue = FindUserByID(docElements); // prevents us from calling FindUserByID multiple times
                    if (FindUserByIdValue != null)
                    {
                        Console.WriteLine("Are you sure you want to delete this user? (Y/N)");
                        string response = Console.ReadLine();
                        if(response == "Y" || response == "y")
                        {
                            XAttribute userToRemove = FindUserByIdValue;
                            usersInfo.RemoveUser(userToRemove);
                            Console.WriteLine("User has been successfully deleted");
                            DisplayMenu();
                        }
                        else if(response == "N" || response == "n")
                        {
                            Console.WriteLine("Deletion canceled");
                            DisplayMenu();
                        }
                        else
                        {
                            Console.WriteLine("Invalid selection made. Please try again");
                            DisplayMenu();
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Error: User ID is invalid. Please try again");
                        DisplayMenu();
                    }

                }
            }
        }

        // find a user
        public void FindUser()
        {
            string searchSelection = ""; // initialize userInput

            XDocument document = XDocument.Load("users.xml");
            XElement root;
            IEnumerable<XElement> docElements;
            if (document.Root != null)
            {
                root = document.Root;
                if (root.Elements() != null)
                {
                    docElements = root.Elements();

                    do
                    {
                        FindUserMenu();
                        searchSelection = Console.ReadLine();
                    }
                    while (string.IsNullOrWhiteSpace(searchSelection));

                    do
                    {
                        switch (searchSelection)
                        {
                            case "1":
                                FindUserByID(docElements);
                                searchSelection = "exit";
                                break;
                            case "2":
                                bool doesNameExist = false;
                                int userCount = 0;
                                Console.WriteLine("Please enter the name you would like to look up ");
                                string nameLookup = Console.ReadLine();
                                foreach (var name in docElements)
                                {
                                    name.CheckForNull();
                                    XElement usrName = name.Element("Name").Element("LastName");
                                    usrName.CheckForNull();
                                    if (usrName.ToString().Contains(nameLookup))
                                    {
                                        Console.WriteLine("User id: " + name.Attribute("id").Value);
                                        Console.WriteLine("Username: " + name.Element("Username").Value);
                                        Console.WriteLine("Date of birth: " + name.Element("DateOfBirth").Value);
                                        Console.WriteLine("Prefix: " + name.Element("Name").Element("Prefix").Value);
                                        Console.WriteLine("First name: " + name.Element("Name").Element("FirstName").Value);
                                        Console.WriteLine("Last name: " + name.Element("Name").Element("LastName").Value);
                                        Console.WriteLine("Suffix: " + name.Element("Name").Element("Suffix").Value);
                                        Console.WriteLine("Job Title: " + name.Element("JobTitle").Value);
                                        doesNameExist = true;
                                        searchSelection = "exit";
                                        userCount++;
                                        System.Threading.Thread.Sleep(1000);
                                        if (userCount > 1)
                                        {
                                            Console.WriteLine("A total of " + userCount + "users were found");
                                        }
                                    }
                                    else
                                    {
                                        continue; //didn't find it this iteration - keep looping & looking
                                    }
                                    if (doesNameExist == false)
                                    {
                                        Console.Error.WriteLine("Error: Invalid last name entered");
                                        nameLookup = "";
                                        break;
                                    }
                                }
                                searchSelection = "exit";
                                break;
                            case "3":
                                Console.WriteLine("You selected Quit. You will be returned to the main menu");
                                System.Threading.Thread.Sleep(1500);
                                searchSelection = "exit";
                                break;
                            default:
                                Console.Error.WriteLine("Error: Invalid selection made. Please make a valid selection ");
                                FindUserMenu();
                                searchSelection = Console.ReadLine();
                                continue;
                        }
                    }
                    while (searchSelection != "exit");
                    if (searchSelection == "exit")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please make a selection from below");
                        DisplayMenu();
                    }
                }
            }
        }

        // helper function for FindUser method. This method displays the menu for FindUser()
        public void FindUserMenu()
        {
            Console.WriteLine("Please choose a search option below");
            Console.WriteLine("1: Search by User ID");
            Console.WriteLine("2: Search by Last Name");
            Console.WriteLine("3: Quit");
        }

        // edit a user
        public void EditUser()
        {
            XDocument document = XDocument.Load("Users.xml");
            XElement root;
            IEnumerable<XElement> docElements;
            string idValue;
            string usernameValue;
            string pwValue;
            string dobValue;
            string prefixValue;
            string firstNamevalue;
            string middleNameValue;
            string lastNameValue;
            string suffixValue;
            string jobTitlevalue;
            if (document.Root != null)
            {
                root = document.Root;
                if (root.Elements() != null)
                {
                    docElements = root.Elements();
                    XAttribute FindUserByIdValue = FindUserByID(docElements); // prevents us from calling FindUserByID multiple times
                    if (FindUserByIdValue != null)
                    {
                        //Console.WriteLine(FindUserByIdValue.Value);
                        Console.WriteLine();
                        Console.WriteLine("Please select the field you would like to edit");
                        string fieldToEdit = Console.ReadLine();
                        Console.WriteLine();
                        int usrListIndex;
                        bool parsed = int.TryParse(FindUserByIdValue.Value, out usrListIndex);
                        do
                        {
                            usrListIndex--;
                        }
                        while (usrListIndex > usersInfo.userList.Count);
                        if (parsed)
                            {
                                usrListIndex--; // subtract one from usrListIndex. This is because our Id field begins at 1, but our userList index begins at 0
                                switch (fieldToEdit)
                                {
                                case "1":
                                    Console.WriteLine("Editing the Id field");
                                    idValue = usersInfo.userList[usrListIndex].Id;
                                    Console.Write(idValue);
                                    idValue = EditField(idValue);
                                    usersInfo.userList[usrListIndex].Id = idValue;
                                    break;
                                case "2":
                                    Console.WriteLine("Editing the Username field");
                                    usernameValue = usersInfo.userList[usrListIndex].Username;
                                    Console.Write(usernameValue);
                                    usernameValue = EditField(usernameValue);
                                    usersInfo.userList[usrListIndex].Username = usernameValue;
                                    break;
                                case "3":
                                    Console.WriteLine("Editing the Password field");
                                    pwValue = usersInfo.userList[usrListIndex].Password;
                                    Console.Write(pwValue);
                                    pwValue = EditField(pwValue);
                                    usersInfo.userList[usrListIndex].Password = pwValue;
                                    break;
                                case "4":
                                    Console.WriteLine("Editing the Date of birth field");
                                    dobValue = usersInfo.userList[usrListIndex].DateOfBirth.ToString();
                                    Console.Write(dobValue);
                                    dobValue = EditField(dobValue);
                                    usersInfo.userList[usrListIndex].DateOfBirth = DateTime.Parse(dobValue); // we validate this in the Editfield method
                                    break;
                                case "5":
                                    Console.WriteLine("Editing the Prefix field");
                                    prefixValue = usersInfo.userList[usrListIndex].Prefix;
                                    Console.Write(prefixValue);
                                    prefixValue = EditField(prefixValue);
                                    usersInfo.userList[usrListIndex].Prefix = prefixValue;
                                    break;
                                case "6":
                                    Console.WriteLine("Editing the First name field");
                                    firstNamevalue = usersInfo.userList[usrListIndex].FirstName;
                                    Console.Write(firstNamevalue);
                                    firstNamevalue = EditField(firstNamevalue);
                                    usersInfo.userList[usrListIndex].FirstName = firstNamevalue;
                                    break;
                                case "7":
                                    Console.WriteLine("Editing the Middle name field");
                                    middleNameValue = usersInfo.userList[usrListIndex].MiddleName;
                                    Console.Write(middleNameValue);
                                    middleNameValue = EditField(middleNameValue);
                                    usersInfo.userList[usrListIndex].MiddleName = middleNameValue;
                                    break;
                                case "8":
                                    Console.WriteLine("Editing the Last name field");
                                    lastNameValue = usersInfo.userList[usrListIndex].LastName;
                                    Console.Write(lastNameValue);
                                    lastNameValue = EditField(lastNameValue);
                                    usersInfo.userList[usrListIndex].LastName = lastNameValue;
                                    break;
                                case "9":
                                    Console.WriteLine("Editing the Suffix field");
                                    suffixValue = usersInfo.userList[usrListIndex].Suffix;
                                    Console.Write(suffixValue);
                                    suffixValue = EditField(suffixValue);
                                    usersInfo.userList[usrListIndex].Suffix = suffixValue;
                                    break;
                                case "10":
                                    Console.WriteLine("Editing the Job title field");
                                    jobTitlevalue = usersInfo.userList[usrListIndex].JobTitle;
                                    Console.Write(jobTitlevalue);
                                    jobTitlevalue = EditField(jobTitlevalue);
                                    usersInfo.userList[usrListIndex].JobTitle = jobTitlevalue;
                                    break;
                                default:
                                    Console.WriteLine("Default case");
                                    break;
                            }
                        }
                        else 
                        {
                            Console.Error.WriteLine("Error: Invalid selection made. Please try again");
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Error: Invalid ID entered. Please try again");
                    }
                }
            }

         }

        // list all the users
        public void ListAllUsers()
        {
            Console.Clear(); // clear the console
            Console.WriteLine("ListAllUsers"); // write what method this is
            foreach(User usr in usersInfo.ReturnAllUsers()) // loop through our Users
            {
                if (usr != null)
                {
                    // write ToString() contents of our User objects
                    Console.WriteLine(usr.Id.ToString());
                    Console.WriteLine(usr.Username.ToString());
                    Console.WriteLine(usr.Password.ToString());
                    Console.WriteLine(usr.DateOfBirth.ToString());
                    Console.WriteLine(usr.Prefix.ToString());
                    Console.WriteLine(usr.FirstName.ToString());
                    Console.WriteLine(usr.MiddleName.ToString());
                    Console.WriteLine(usr.LastName.ToString());
                    Console.WriteLine(usr.Suffix.ToString());
                    Console.WriteLine(usr.JobTitle.ToString());

                    Console.WriteLine("_______________________"); // seperate users
                }
            }
            Console.WriteLine();   
            DisplayMenu();
        }
        
        //quit the application
        public void Quit()
        {
            Console.WriteLine("You selected Quit. Goodbye!");
            System.Threading.Thread.Sleep(1500);
        }


        public XAttribute FindUserByID(IEnumerable<XElement> element)
        {
            
            bool doesIdExist = false;
            Console.WriteLine("Please enter the ID you would like to look up ");
            string idLookup = Console.ReadLine();
            foreach (var d in element)
            {
                d.CheckForNull();
                XAttribute usrId = d.Attribute("id");
                usrId.CheckForNull();
                if (idLookup == usrId.Value)
                {
                    Console.WriteLine("1: User id: " + d.Attribute("id").Value);
                    Console.WriteLine("2: Username: " + d.Element("Username").Value);
                    Console.WriteLine("3: Password: " + d.Element("Password").Value);
                    Console.WriteLine("4: Date of birth: " + d.Element("DateOfBirth").Value);
                    Console.WriteLine("5: Prefix: " + d.Element("Name").Element("Prefix").Value);
                    Console.WriteLine("6: First name: " + d.Element("Name").Element("FirstName").Value);
                    Console.WriteLine("7: Middle name: " + d.Element("Name").Element("MiddleName").Value);
                    Console.WriteLine("8: Last name: " + d.Element("Name").Element("LastName").Value);
                    Console.WriteLine("9: Suffix: " + d.Element("Name").Element("Suffix").Value);
                    Console.WriteLine("10: Job title: " + d.Element("JobTitle").Value);
                    doesIdExist = true;
                    return usrId;
                }
            }
            if(doesIdExist == false)
            {
                Console.Error.WriteLine("Error: Invalid user ID entered");
                return null;
            }
            return null;
        }

        // helper function. allows the cursor to be moved around. Used in the EditUser method
        public string EditField(string fieldValue)
        {
            string newValue = fieldValue; // making a copy of fieldValue that we can manipulate (for data integrity)
            var xPos = 0;
            var yPos = Console.WindowTop + Console.WindowHeight - 1;
            Console.SetCursorPosition(xPos, yPos);
            ConsoleKeyInfo keyinfo;
            int insertCounter = 0;
            bool terminateLoop = false;
            do
            {
                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.Insert)
                {
                    insertCounter++;
                    if (insertCounter % 2 == 1)
                    {
                        Console.CursorSize = 100;
                        //change input method here
                        int loopCounter = 1;
                        for (int i=0; i<loopCounter; i++)
                        {
                            ConsoleKeyInfo otherKey = Console.ReadKey(true);
                            var readVal = ' ';
                            readVal = otherKey.KeyChar;
                            var arrowDetect = (char)otherKey.Key;

                            if (char.IsLetter(arrowDetect) || char.IsDigit(arrowDetect))
                            {
                                if (newValue.Length > 1)
                                {
                                    if (newValue.Length <= xPos+1)
                                    {
                                        newValue = string.Concat(newValue, readVal.ToString());
                                    }
                                    else
                                    {
                                        newValue = newValue.Remove(xPos, 1);
                                        newValue = newValue.Insert(xPos, readVal.ToString());
                                    }
                                }
                                else
                                {
                                    newValue = newValue.Insert(xPos, readVal.ToString());
                                }
                                Console.Write(readVal);
                                xPos++;
                                Console.SetCursorPosition(xPos, yPos);
                                loopCounter++;
                            }
                            else if (arrowDetect == (char)Keys.Left)
                            {
                                if(xPos > 0)
                                {
                                    xPos--;
                                    Console.SetCursorPosition(xPos, yPos);
                                    loopCounter++;
                                }
                            }
                            else if (arrowDetect == (char)Keys.Right)
                            {
                                if(xPos < newValue.Length)
                                {
                                    xPos++;
                                    Console.SetCursorPosition(xPos, yPos);
                                    loopCounter++;
                                    continue;
                                }
                            }
                            else if(arrowDetect == (char)Keys.Escape)
                            {
                                // escape method here
                                newValue = string.Empty;
                                xPos = 0;
                                Console.SetCursorPosition(xPos, yPos);
                                Console.Write("                      ");
                                xPos = 0;
                                Console.SetCursorPosition(xPos, yPos);
                                loopCounter++;
                            }
                            else if (arrowDetect == (char)Keys.Enter)
                            {
                                // enter method here
                                if (fieldValue.Contains("/"))
                                {
                                    // Date of Birth - need to validte
                                    DateTime newDOB;
                                    if(DateTime.TryParse(fieldValue, out newDOB))
                                    {
                                        fieldValue = newValue;
                                        Console.WriteLine("User has been successfully updated");
                                    }
                                    else
                                    {
                                        Console.Error.WriteLine("Error: Invalid Date of Birth entered ");
                                    }
                                }
                                else
                                {
                                    // not a Date of Birth - no need to validate
                                    fieldValue = newValue;
                                    //Console.WriteLine();
                                    //Console.WriteLine("User has been successfully updated");
                                }
                                terminateLoop = true; // exit loop
                                loopCounter++;
                                break;
                            }
                            else if (arrowDetect == (char)Keys.Home)
                            {
                                xPos = 0;
                                Console.SetCursorPosition(xPos, yPos);
                                loopCounter++;
                            }
                            else if(arrowDetect == (char)Keys.Delete)
                            {
                                // delete method here
                                int bspaceXpos = 0;
                                for (int j = 0; j < newValue.Length; j++)
                                {
                                    Console.SetCursorPosition(bspaceXpos, yPos);
                                    Console.Write(" ");
                                    bspaceXpos++;
                                }
                                Console.SetCursorPosition(0, yPos);
                                if(newValue.Length > 1 && newValue.Length != (xPos+1))
                                {
                                    newValue = newValue.Remove(xPos + 1, 1);
                                }
                                else if (newValue.Length > 1 && newValue.Length == (xPos + 1))
                                {
                                    newValue = newValue;
                                }
                                else if(newValue.Length == 1)
                                {
                                    newValue = string.Empty;
                                }
                                else
                                {
                                    continue;
                                }
                                Console.Write(newValue);
                                Console.SetCursorPosition(xPos, yPos);
                                loopCounter++;
                            }
                            else if (arrowDetect == (char)Keys.Back)
                            {
                                // backspace method here
                                int bspaceXpos = 0;
                                for (int j=0; j<newValue.Length; j++)
                                {
                                    Console.SetCursorPosition(bspaceXpos, yPos);
                                    Console.Write(" ");
                                    bspaceXpos++;
                                }
                                Console.SetCursorPosition(0, yPos);
                                if(newValue.Length > 0 && xPos > 0)
                                {
                                    newValue = newValue.Remove(xPos - 1, 1);
                                    xPos--;
                                }
                                else if (newValue.Length == 0)
                                {
                                    newValue = string.Empty;
                                }
                                else if (newValue.Length > 0 && xPos <=1)
                                {
                                    continue;
                                }
                                else
                                {
                                    continue;
                                }
                                Console.Write(newValue);
                                Console.SetCursorPosition(xPos, yPos);
                                loopCounter++;

                            }
                            else if (arrowDetect == (char)Keys.End)
                            {
                                xPos = newValue.Length;
                                Console.SetCursorPosition(xPos, yPos);
                                loopCounter++;
                            }
                            else
                            {
                                loopCounter++;
                            }
                        }    
                    }
                    else
                    {
                        Console.CursorSize = 1;
                    }

                    if (keyinfo.Key == ConsoleKey.LeftArrow && xPos > 0)
                    {
                        xPos--;
                        Console.SetCursorPosition(xPos, yPos);
                    }
                    else if (keyinfo.Key == ConsoleKey.RightArrow)
                    {
                        if (xPos < newValue.Length)
                        {
                            xPos++;
                            Console.SetCursorPosition(xPos, yPos);
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (keyinfo.Key == ConsoleKey.LeftArrow && xPos > 0)
                    {
                        xPos--;
                        Console.SetCursorPosition(xPos, yPos);
                    }
                    else if (keyinfo.Key == ConsoleKey.RightArrow)
                    {
                        if (xPos < newValue.Length)
                        {
                            xPos++;
                            Console.SetCursorPosition(xPos, yPos);
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            while (terminateLoop == false);
            Console.WriteLine();
            Console.WriteLine("End of program. The new value you entered is " + fieldValue);
            System.Threading.Thread.Sleep(2000);
            DisplayMenu();
            return fieldValue;
        }
    }
}
