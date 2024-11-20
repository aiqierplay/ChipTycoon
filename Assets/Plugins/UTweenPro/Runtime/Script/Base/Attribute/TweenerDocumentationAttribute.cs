using System;

namespace Aya.TweenPro
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TweenerDocumentationAttribute : Attribute
    {
        public string Title { get; }
        public string Document { get; }

        public TweenerDocumentationAttribute(string title, string document)
        {
            Title = title;
            Document = document;
        }
    }
}
