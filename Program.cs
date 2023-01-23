using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json.Serialization;

namespace Hometask_17._01_KazanovAlexandr
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var squad = new Squad("Alfa", ".NET", 12);
            JsonSerializer jsonSerializer = new JsonSerializer();
            using (StreamWriter stream = new StreamWriter("squad.json"))
            {
                jsonSerializer.Serialize(stream, squad);
            }

            try
            {
                string jsonPath = GetJsonFile(GetListOfFiles("E:\\Programms\\Hometask_17.01_KazanovAlexandr\\Files"));
                StreamReader streamReader = new StreamReader(jsonPath);
                //object? squad1 = jsonSerializer.Deserialize(streamReader, typeof(Squad));
                //streamReader.Dispose();
                string json = streamReader.ReadToEnd();
                string[] classFields = json.Split(',');
                var dictionary = new Dictionary<string, string>();
                foreach (string classField in classFields)
                {
                    string[] classFieldSplit = classField.Split(':');
                    dictionary.Add(StringClearer(classFieldSplit[0]), StringClearer(classFieldSplit[1]));
                }
                string[] fieldValues = GetFieldValues(dictionary);
                var squad1 = new Squad(fieldValues[0], fieldValues[1], int.Parse(fieldValues[2]));

                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream("squadName.bin", FileMode.Create, FileAccess.Write, FileShare.None))
                {
#pragma warning disable SYSLIB0011
                    formatter.Serialize(stream, squad1);
#pragma warning restore SYSLIB0011
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            static List<string> GetListOfFiles(string path)
                {
                    var files = new List<string>();
                    foreach (var file in Directory.GetFiles(path))
                    {
                        files.Add(file);
                    }
                    return files;
                }

            static string GetJsonFile(List<string> files)
            {
                var jsonFiles = new List<string>();
                foreach (var file in files)
                {
                    if (file.EndsWith(".json"))
                    {
                        jsonFiles.Add(file);
                    }
                }
                if (jsonFiles.Count != 1)
                {
                    throw new Exception("Error! Can't find correct json file!");
                }
                else return jsonFiles[0];
            }

            static string StringClearer(string str)
            {
                var sb = new StringBuilder();
                foreach (var ch in str)
                {
                    if (char.IsLetter(ch) || char.IsDigit(ch))
                    {
                        sb.Append(ch);
                    }
                }
                return sb.ToString();
            }

            static string[] GetFieldValues(Dictionary <string,string> dictionary)
            {
                var type = typeof(Squad);
                string[] fieldValues = new string[dictionary.Count];
                int i = 0;
                foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static))
                {
                    if (dictionary.ContainsKey(prop.Name))
                    {
                        fieldValues[i] = dictionary[prop.Name];
                        i++;
                    }
                }
                return fieldValues;
            }
        }

        [Serializable]
        [DataContract]
        public class Squad
        {
            [DataMember]
            public string Name { get; set; }
            [DataMember]
            public string Description { get; set; }
            [DataMember]
            public int AmountOfMembers { get; set; }
            public Squad(string name, string description, int amountOfMembers)
            {
                Name = name;
                Description = description;
                AmountOfMembers = amountOfMembers;
            }
        }
    }
}