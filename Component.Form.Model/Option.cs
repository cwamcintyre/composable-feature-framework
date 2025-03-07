using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Component.Form.Model
{
    public class Option
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public static implicit operator Option((string Key, string Value) tuple)
        {
            return new Option
            {
                Key = tuple.Key,
                Value = tuple.Value
            };
        }

        public static implicit operator (string Key, string Value)(Option option)
        {
            return (option.Key, option.Value);
        }

        public static implicit operator Option(KeyValuePair<string, string> pair)
        {
            return new Option
            {
                Key = pair.Key,
                Value = pair.Value
            };
        }

        public static implicit operator KeyValuePair<string, string>(Option option)
        {
            return new KeyValuePair<string, string>(option.Key, option.Value);
        }        
    }

    public static class OptionExtensions
    {
        public static IEnumerable<Option> FromDictionary(this Dictionary<string, string> dictionary)
        {
            return dictionary.Select(pair => (Option)pair).ToList();
        }
    }
}