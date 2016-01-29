using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UserFileEditor
{
    public static class Extensions
    {
        // helper function to throw custom error
        public static void NullError()
        {
            ArgumentNullException NullError = new ArgumentNullException("XML", "Error: Invalid XML");
            Console.Error.WriteLine(NullError.ToString());
        }

        //helper function to check for null values
        public static bool CheckForNull(this XObject check)
        {
            if (check != null)
            {
                return false;
            }
            else
            {
                NullError();
                return true;
            }
        }

        //helper function to check for null values
        public static XElement IsValidOrDefault(this XElement check)
        {
            if (check != null)
            {
                return check; // non null value
            }
            else
            {
                NullError();
                return default(XElement); // null
            }
        }

    }
}
