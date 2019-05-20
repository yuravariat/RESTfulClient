using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfulClient
{
    public class RoutingAttribute : Attribute
    {
        public RoutingAttribute(string path)
        {
            Path = path;
        }
        public RoutingAttribute(string path, string verbs)
        {
            Path = path;
            Verbs = verbs;
        }
        public string Path { get; set; }
        public string Summary { get; set; }
        public string Notes { get; set; }
        public string Verbs { get; set; }
    }
    public static class Extenstions
    {
        public static string GetPath(this IReturn requestType)
        {
            var attrs = TypeDescriptor.GetAttributes(requestType).OfType<RoutingAttribute>();
            if (attrs.DefaultIfEmpty() != null)
            {
                return attrs.First().Path;
            }
            return string.Empty;
        }
    }
}
