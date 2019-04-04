using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    static SaveManager current;

    private string settingsFilename = "settings.json";
    private string recordsFilename = "records.json";

    private Settings settings;
    private Records records;

    void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);
    }

    //Settings
    public static void SetSettings(Settings settings)
    {
        if (current == null || settings == null)
            return;
        current.settings = settings;
        current.SaveSettings();
    }

    public static Settings GetSettings()
    {
        if (current == null)
            return null;

        if (current.settings == null)
            current.LoadSettings();
        return current.settings;
    }

    void SaveSettings()
    {
        if (settings == null)
            LoadSettings();
        string jsonData = JsonUtility.ToJson(settings);
        string filePath = Path.Combine(Application.persistentDataPath, settingsFilename);
        File.WriteAllText(filePath, jsonData);
    }

    void LoadSettings()
    {
        if (settings != null) // prevent double loading
        {
            settings = new Settings();
            return;
        }

        // Path.Combine combines strings into a file path
        string filePath = Path.Combine(Application.persistentDataPath, settingsFilename);
        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath); 
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            settings = JsonUtility.FromJson<Settings>(dataAsJson);
        }
        else
            settings = new Settings();
    }

    //Records

    public static void SetRecords(Records records)
    {
        if (current == null || records == null)
            return;
        current.records = records;
        current.SaveRecords();
    }

    public static Records GetRecords()
    {
        if (current == null)
            return null;

        if (current.records == null)
            current.LoadRecords();
        return current.records;
    }

    void SaveRecords()
    {
        if (records == null)
            LoadRecords();
        string jsonData = JsonUtility.ToJson(records);
        string filePath = Path.Combine(Application.persistentDataPath, recordsFilename);
        File.WriteAllText(filePath, jsonData);
    }

    void LoadRecords()
    {
        if (records != null) // prevent double loading
        {
            records = new Records();
            return;
        }

        // Path.Combine combines strings into a file path
        string filePath = Path.Combine(Application.persistentDataPath, recordsFilename);
        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            records = JsonUtility.FromJson<Records>(dataAsJson);
        }
        else
            records = new Records();
    }
}
