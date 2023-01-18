using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json.Serialization;

namespace Hometask_17._01_KazanovAlexandr
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var squad = new Squad("Alfa", ".NET", 12);
            IFormatter formatter = new BinaryFormatter();
            JsonSerializer jsonSerializer = new JsonSerializer();

            using (StreamWriter stream = new StreamWriter("squad.json"))
            {
                jsonSerializer.Serialize(stream, squad);
            }

            string jsonPath = JsonFile(ListOfFiles("E:\\Programms\\Hometask_17.01_KazanovAlexandr\\Files"));
            StreamReader streamReader = new StreamReader(jsonPath);
            object? squad1 = jsonSerializer.Deserialize(streamReader, typeof(Squad));
            streamReader.Dispose();

#pragma warning disable SYSLIB0011
            using (Stream stream = new FileStream("squadName.bin", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, squad1);
            }
#pragma warning disable SYSLIB0011

            static List<string> ListOfFiles(string path)
                {
                    var files = new List<string>();
                    foreach (var file in Directory.GetFiles(path))
                    {
                        files.Add(file);
                    }
                    return files;
                }

            static string JsonFile(List<string> files)
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