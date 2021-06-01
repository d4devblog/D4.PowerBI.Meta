using System;

namespace D4.PowerBI.Meta.Exceptions
{
    public class ContentEmptyException: ApplicationException
    {
        public ContentEmptyException(string message) : base(message) { }
    }
}
