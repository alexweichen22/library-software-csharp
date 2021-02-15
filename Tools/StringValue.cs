using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTry3.Tools
{
    public static class StringEnum
    {
        public static string GetStringValue(System.Enum value)
        {
            string output = null;
            System.Type type = value.GetType();
            System.Reflection.FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            /*
            List<object> objects = fi.GetCustomAttributes(typeof(StringValue), false).ToList();*/
            if (attrs.Length > 0)
            {
                output = attrs[0].value;
            }
            

            return output;
        }
    }
    class StringValue : System.Attribute
    {
        private string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string value
        {
            get { return _value; }
        }
    }
}
