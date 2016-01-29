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
    class Program
    {
        static void Main(string[] args)
        {
            User newUser = new User();
            newUser.SaveAsXml();
            UserFile uf = new UserFile();
            uf.SaveAsXML();
            UserEditor ue = new UserEditor();
            ue.Start();
              
        }
    }
}
