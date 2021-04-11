using System;
using System.Collections.Generic;
using System.Text;

namespace evapi
{
    internal static class Convert
    {
        public static Dictionary<string, object> ToDictionary(object obj)
        {
            // JObject -> Dictionary.
            if (obj is Newtonsoft.Json.Linq.JObject jobj)
                return jobj.ToObject<Dictionary<string, object>>();
            // Try regular cast.
            return (Dictionary<string, object>) obj;
        }
    }
}
