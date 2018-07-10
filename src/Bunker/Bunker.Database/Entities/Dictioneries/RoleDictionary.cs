using Bunker.Database.Attributes;

namespace Bunker.Database.Entities.Dictioneries
{
    public enum RoleDictionary
    {
        [DictionaryIdentifier(Identifier = 1, Name = "Guest")]
        Guest,
    }
}