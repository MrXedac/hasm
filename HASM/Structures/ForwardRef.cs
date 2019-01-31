using System;
using System.IO;

namespace HASM.Structures
{
    public class ForwardRef
    {
        public Int32 offset;
        public string identifier;

        public ForwardRef(string id, Int32 offset)
        {
            this.offset = offset;
            this.identifier = id;
        }
    }
}
