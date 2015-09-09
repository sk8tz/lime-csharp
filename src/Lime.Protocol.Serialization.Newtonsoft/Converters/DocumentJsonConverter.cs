﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Lime.Protocol.Serialization.Newtonsoft.Converters
{
    class DocumentJsonConverter : JsonConverter
    {
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (Document).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
        {
            if (objectType.IsAbstract)
            {
                // The serialization is made by the
                // container class (Message or Command)
                return null;
            }
            else if (objectType == typeof (DocumentCollection))
            {
                var instance = new DocumentCollection();
                var jObject = JObject.Load(reader);
                if (jObject[DocumentCollection.ITEM_TYPE_KEY] != null)
                {
                    instance.ItemType = jObject[DocumentCollection.ITEM_TYPE_KEY].ToObject<MediaType>();
                }

                if (jObject[DocumentCollection.ITEMS_KEY] != null && instance.ItemType != null)
                {
                    Type itemType;
                    if (TypeUtil.TryGetTypeForMediaType(instance.ItemType, out itemType))
                    {
                        var items = jObject[DocumentCollection.ITEMS_KEY];
                        if (items.Type == JTokenType.Array)
                        {
                            var itemsArray = (JArray)items;
                            instance.Items = new Document[itemsArray.Count];
                            for (int i = 0; i < itemsArray.Count; i++)
                            {
                                var item = itemsArray[i];
                                instance.Items[i] = (Document)Activator.CreateInstance(itemType);
                                serializer.Populate(item.CreateReader(), instance.Items[i]);
                            }
                        }
                    }
                }

                if (jObject[DocumentCollection.TOTAL_KEY] != null)
                {
                    instance.Total = jObject[DocumentCollection.TOTAL_KEY].ToObject<int>();
                }

                return instance;
            }
            else
            {
                var instance = Activator.CreateInstance(objectType);
                serializer.Populate(reader, instance);
                return instance;
            }
        }

        public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer)
        {
            // TODO: Implement DocumentCollection serialization
            serializer.Serialize(writer, value);
        }
    }
}