using UnityEditor;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Habtic.YUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Habtic.Games.Colr; //<-- Change this namespace to the correct one to find the "Game" component.

public class CreateGameCSV : EditorWindow
{

    static List<string> Codes = new List<string>();
    bool groupEnabled;

    [MenuItem("Yunify/CreateGameCSV")]
    static void GetCSV()
    {
        Codes.Clear();
        GameObject[] rootObject = SceneManager.GetActiveScene().GetRootGameObjects();
        string rootObjectName = rootObject[0].gameObject.name.ToLower();
        string CSVDirectory = "Assets/AssetBundles/GameCSVFile";
        string csvFileName = "gamecsv_" + rootObjectName + ".csv";
        object[] components = Resources.FindObjectsOfTypeAll(typeof(YUILocalizedText));

        if (!Directory.Exists(CSVDirectory))
        {
            Directory.CreateDirectory(CSVDirectory);
        }
        if (File.Exists(CSVDirectory + "/" + csvFileName))
        {
            File.Delete(CSVDirectory + "/" + csvFileName);
        }

        File.AppendAllText(CSVDirectory + "/" + csvFileName, "LanguageName;SourceType;Source;Key;Value\r\n");

        Game g = rootObject[0].GetComponent<Game>(); // <-- TODO Set the correct namespace at the top of this script
        Dictionary<string, string> ls = g.LocalizedStrings;
        foreach (var item in ls)
        {
            string localizedCodeToAdd = $"en;content;{rootObjectName};{item.Key};\"{item.Value}\"";
            Codes.Add(localizedCodeToAdd);
            File.AppendAllText(CSVDirectory + "/" + csvFileName, localizedCodeToAdd + "\r\n");
        }

        foreach (YUILocalizedText item in components)
        {
            string localizedKey = item.GetLocalizationKey();
            string itemText = item.GetComponent<TMP_Text>().text;
            string codeToAdd = $"en;content;{rootObjectName};{localizedKey};\"{itemText}\"";

            if (!Codes.Contains(codeToAdd))
            {
                bool keyAlreadyExcists = false;
                foreach (string codeLine in Codes)
                {
                    if (codeLine.Contains(localizedKey))
                    {
                        keyAlreadyExcists = true;
                        string errorLine = "Duplicate localized key found: " + localizedKey + " with different text: \"" + itemText + "\"";
                        Debug.LogError(errorLine);
                        Codes.Add(errorLine.ToUpper());
                        break; //Break out of "codes"
                    }
                }
                if (!keyAlreadyExcists)
                {
                    Codes.Add(codeToAdd);
                    File.AppendAllText(CSVDirectory + "/" + csvFileName, codeToAdd + "\r\n");
                }
                else
                {
                    break; //Break out of "components"
                }
            }
        }

        EditorWindow.GetWindow(typeof(CreateGameCSV));
    }

    private void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        if (Codes.Count > 0)
        {
            for (int i = 0; i < Codes.Count; i++)
            {
                EditorGUILayout.TextField("code", Codes[i]);
            }
        }
        else
        {
            EditorGUILayout.TextArea("No localized keys found!!!");
        }

        if (GUILayout.Button("refresh and save"))
        {
            GetCSV();
        }
    }
}
