using System;
using System.Collections.Generic;
using System.Text;

namespace Mapper
{
    class MapException : Exception
    {
        public MapException(string message) : base(message) { }

        public MapException(string message, Exception innerException) : base(message, innerException) { }
    }
}
