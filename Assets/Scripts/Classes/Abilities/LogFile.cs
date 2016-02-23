using UnityEngine;
using System.Collections;
using System.IO;

public static class LogFile {
    public static string fileName;
    private static TextAsset asset;
    private static StreamWriter writer;
    
    public static void AppendString(string appendString) {
        asset = Resources.Load("LogFile/" + fileName + ".txt") as TextAsset;
        writer = new StreamWriter("Resources/LogFile/" + fileName + ".txt");
        writer.WriteLine(appendString);
    }
}
