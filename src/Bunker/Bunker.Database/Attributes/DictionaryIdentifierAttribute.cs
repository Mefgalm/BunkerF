using System;

namespace Bunker.Database.Attributes
{
    public class DictionaryIdentifierAttribute : Attribute
    {
        public int    Identifier { get; set; }
        public string Name       { get; set; }
    }
}