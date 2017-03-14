using System;

namespace KeyCollectionTest.Classes
{
    public class UserType
    {
        public UserType(DateTime dt)
        {
            Date = dt;
        }

        public DateTime Date { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            UserType param = obj as UserType;
            bool equals = this.Date == param.Date;
            return equals;
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode();
        }

        public static bool operator ==(UserType uType1, UserType uType2)
        {
            if (System.Object.ReferenceEquals(uType1, uType2))
                return true;

            if (((object)uType1 == null) || ((object)uType2 == null))
                return false;

            return uType1.Date == uType2.Date;
        }

        public static bool operator !=(UserType uType1, UserType uType2)
        {
            return !(uType1.Date == uType2.Date);
        }

        public override string ToString()
        {
            return Date.ToString();
        }
    }

}
