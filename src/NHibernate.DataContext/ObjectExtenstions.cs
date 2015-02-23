namespace NHibernate.DataContext
{
    using System;
    using System.Collections.Generic;

    public static class ObjectExtenstions
    {
        public static void SetProperty(this object o, string propName, object propValue)
        {
            var propInfo = o.GetType().GetProperty(propName);
            if (propInfo != null)
            {
                propInfo.SetValue(o, propValue);
            }
            else
            {
                throw MemberNotFound(propName, o);
            }
        }

        private static Exception MemberNotFound(string propName, object o)
        {
            return new MissingMemberException(string.Format("Property or member {0} of {1} not found.", propName, o.GetType()));
        }

        public static bool HasProperty(this object o, string propName)
        {
            var v = o.GetProperty(propName);

            if (v == null)
            {
                return false;
            }

            if (v is DateTime)
            {
                if (((DateTime)v) == DateTime.MinValue)
                {
                    return false;
                }
            }

            if (v is Guid)
            {
                if (((Guid)v) == Guid.Empty)
                {
                    return false;
                }
            }

            return true;
        }

        public static T GetProperty<T>(this object o, string propName)
        {
            return (T)o.GetProperty(propName);
        }

        public static object GetProperty(this object o, string propName)
        {
            var propInfo = o.GetType().GetProperty(propName);
            if (propInfo != null)
            {
                return propInfo.GetValue(o);
            }

            throw MemberNotFound(propName, o);
        }

        public static IDictionary<string, object> ToDictionary(this object source)
        {
            Dictionary<string, object> dic = null;
            if (source != null)
            {
                dic = new Dictionary<string, object>();
                var props = source.GetType().GetProperties();
                foreach (var propInfo in props)
                {
                    dic.Add(propInfo.Name, propInfo.GetValue(source));
                }
            }

            return dic;
        }
    }
}
