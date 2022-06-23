﻿using Newtonsoft.Json;
using System;
using System.IO;

namespace RoomFinishing
{
    public static class Json
    {
        public static string applicationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string jsonFolderPath = $"{applicationPath}\\Nika_RD_Data\\Nika_RD_Json\\RoomFinishing";

        static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            CheckAdditionalContent = true,
            Formatting = Formatting.Indented
        };

        public static void Write(object objectToSerialize, string revitFileName)
        {
            string jsonFilePath = $"{jsonFolderPath}\\{revitFileName}.json";

            if (!Directory.Exists(jsonFolderPath))
            {
                Directory.CreateDirectory(jsonFolderPath);
            }
            using (StreamWriter writer = File.CreateText(jsonFilePath))
            {
                string output = JsonConvert.SerializeObject(objectToSerialize);
                writer.Write(output);
            }
        }

        public static T Deserialize<T>(string json) where T : new()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, settings);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: Failed to deserialize json string into a valid object___{ex.Message}");
            }
        }

        public static T Read<T>(string revitFileName) where T : new()
        {
            string jsonFilePath = $"{jsonFolderPath}\\{revitFileName}.json";

            T result = default(T);
            try
            {
                if (File.Exists(jsonFilePath))
                {
                    using (var reader = File.OpenText(jsonFilePath))
                    {
                        string fileText = reader.ReadToEnd();
                        var deserializedObject = Deserialize<T>(fileText);
                        if (deserializedObject != null)
                        {
                            result = deserializedObject;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: Failed to deserialize json string into a valid object.\n{ex.Message}");
            }

            return result;
        }
    }
}
