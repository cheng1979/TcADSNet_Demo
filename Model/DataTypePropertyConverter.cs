using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Ads.TypeSystem;

namespace TcADSNet_Demo.Model
{
    public class DataTypePropertyConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);

        }


        private DataType ParseValue(JToken value)
        {
            //switch (value.Type)
            //{
            //    case JTokenType.Object:
            //        return value.ToObject<DataType>();

            //    default:
            //        return null;
            //}

            //var v = value.Root;
            //DataType dt;
            //dt.Attributes = v.Attributes;
            return value.ToObject<DataType>();
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            /// Convert object to TwinCat DataType
            switch (reader.TokenType)
            {
                //case JsonToken.String:
                case JsonToken.StartObject:
                    //return new List<TextProperty> { ParseValue(JToken.Load(reader)) };
                    var readVar = JToken.Load(reader).Root;
                    //return (DataType)ParseValue(JToken.Load(reader));
                    return ParseValue(JToken.Load(reader));

                default:
                    return null;
            }
        }

        public override bool CanConvert(Type objectType) => false;
    }
}
