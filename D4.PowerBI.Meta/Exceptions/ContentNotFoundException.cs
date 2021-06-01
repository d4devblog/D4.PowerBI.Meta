using System;

namespace D4.PowerBI.Meta.Exceptions
{
    public class ContentNotFoundException: ApplicationException
    {
        public ContentNotFoundException(string message) : base(message) { }
    }
}
