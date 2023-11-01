using System.IO;
using System.Text.Json;

namespace App_Progetto.DatiDb;

public class JsonReader
{
    public static T DeserializeJsonFile<T>(string filePath)
    {
        var stream = File.OpenRead(filePath);

        var jsonData = JsonSerializer.Deserialize<T>(stream);

        return jsonData;
    }
}
