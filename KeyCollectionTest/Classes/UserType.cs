using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyCollectionTest.Classes
{

    public class UserType
    {
        public UserType(DateTime birthday)
        {
            Birthday = birthday;
        }

        public DateTime Birthday { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            UserType param = obj as UserType;
            bool equals = this.Birthday == param.Birthday;
            return equals;
        }

        public override int GetHashCode()
        {
            return Birthday.GetHashCode();
        }

        public static bool operator ==(UserType uType1, UserType uType2)
        {
            if (System.Object.ReferenceEquals(uType1, uType2))
                return true;

            if (((object)uType1 == null) || ((object)uType2 == null))
                return false;

            return uType1.Birthday == uType2.Birthday;
        }

        public static bool operator !=(UserType uType1, UserType uType2)
        {
            return !(uType1.Birthday == uType2.Birthday);
        }

        public override string ToString()
        {
            return Birthday.ToString();
        }
    }

}
