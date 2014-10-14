using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FootballApp.Core.Helper
{
    public class JsonStringToIntConvertor : JsonConverter
    {
        // Implementation following the blog post http://stackoverflow.com/questions/9825624/when-using-newtonsoft-json-net-to-deserialize-a-string-how-do-i-convert-an-empt
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                if (String.IsNullOrEmpty((string)reader.Value))
                {
                    return null;
                }
                int number;
                if (int.TryParse((string)reader.Value, out number))
                    return number;

                throw new JsonReaderException(String.Format("Expected integer, got {0}", reader.Value));
            }
            else
            {
                return reader.Value;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
