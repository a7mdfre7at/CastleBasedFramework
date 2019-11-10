using System;

namespace Castle.Framework.Communication
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class PrefixAttribute : Attribute
    {
        public PrefixAttribute(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }
    }
}