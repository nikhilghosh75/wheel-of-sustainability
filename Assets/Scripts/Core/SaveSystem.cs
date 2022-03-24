using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

public class SaveSystem
{
    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);

    public static void Save(SaveData saveData)
    {
        CreateSaveDirectory();

        string dataPath = string.Format("{0}/Wheel of Sustainability/SaveData.dat", Application.persistentDataPath);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream;
        try
        {
            if (File.Exists(dataPath))
            {
                File.WriteAllText(dataPath, string.Empty);
                fileStream = File.Open(dataPath, FileMode.Open);
            }
            else
            {
                fileStream = File.Create(dataPath);
            }

            binaryFormatter.Serialize(fileStream, saveData);
            fileStream.Close();

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                SyncFiles();
            }

        }
        catch (Exception e)
        {
            PlatformSafeMessage("Failed to save: " + e.Message);
        }
    }

    public static SaveData Load()
    {
        CreateSaveDirectory();
        SaveData saveData = null;
        string dataPath = string.Format("{0}/Wheel of Sustainability/SaveData.dat", Application.persistentDataPath);

        try
        {
            if (File.Exists(dataPath))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = File.Open(dataPath, FileMode.Open);

                saveData = (SaveData)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
            }
        }
        catch (Exception e)
        {
            PlatformSafeMessage("Failed to load: " + e.Message);
        }

        return saveData;
    }

    private static void PlatformSafeMessage(string message)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            WindowAlert(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    private static void CreateSaveDirectory()
    {
        Directory.CreateDirectory(string.Format("{0}/Wheel of Sustainability/", Application.persistentDataPath));
    }
}
