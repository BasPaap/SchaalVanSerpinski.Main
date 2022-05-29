using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class Settings
{
    private const string directoryName = "Serpinski";
    private const string fileName = "settings.xml";

    public string PortName { get; set; } = "COM1";
    public int BaudRate { get; set; } = 9600;

    public float GongInterval { get; set; } = 3.0f;
   
    private static string SettingsDirectoryPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), directoryName);
    private static string FullPath => Path.Combine(SettingsDirectoryPath, fileName);

    public void Save()
    {
        var serializer = new XmlSerializer(typeof(Settings));
        using (var fileStream = new FileStream(FullPath, FileMode.Create))
        {
            serializer.Serialize(fileStream, this);
            fileStream.Close();
        }
    }

    public static Settings Load()
    {
        if (!Directory.Exists(SettingsDirectoryPath))
        {
            Directory.CreateDirectory(SettingsDirectoryPath);
        }

        Settings settings = null;
        var serializer = new XmlSerializer(typeof(Settings));

        if (!File.Exists(FullPath))
        {
            settings = new Settings();

            using (var fileStream = new FileStream(FullPath, FileMode.Create))
            {
                serializer.Serialize(fileStream, settings);
            }
        }
        else
        {
            using (var fileStream = new FileStream(FullPath, FileMode.Open))
            {
                settings = (Settings)serializer.Deserialize(fileStream);
            }
        }

        return settings;
    }

}
