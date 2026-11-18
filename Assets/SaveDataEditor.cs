using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveDataEditor : EditorWindow
{
    private playerData data;
    private string savePath;
    private Vector2 scroll;

    [MenuItem("Tools/Save Data Editor")]

    public static void ShowWindow()
    {
        GetWindow<SaveDataEditor>("Save Editor");
    }

    private void OnEnable()
    {
        savePath = Application.persistentDataPath + "/saves.game";
    }

    private void OnGUI()
    {
        GUILayout.Label("Unity Save Data Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Save Location", savePath);

        if (GUILayout.Button("Load Save"))
            LoadSave();

        if (data == null)
            return;
        EditorGUILayout.Space();

        scroll = EditorGUILayout.BeginScrollView(scroll);
        GUILayout.Label("Player Stats", EditorStyles.boldLabel);
        data.money = EditorGUILayout.IntField("Money", data.money);
        data.enemyDestroyed = EditorGUILayout.IntField("Enemy Destroyed", data.enemyDestroyed);

        EditorGUILayout.Space();
        GUILayout.Label("Weapon Levels", EditorStyles.boldLabel);
        data.levelwep1 = EditorGUILayout.IntField("Weapon 1", data.levelwep1);
        data.levelwep2 = EditorGUILayout.IntField("Weapon 2", data.levelwep2);
        data.levelwep3 = EditorGUILayout.IntField("Weapon 3", data.levelwep3);

        EditorGUILayout.Space();
        GUILayout.Label("Weapon Price", EditorStyles.boldLabel);
        data.pricewep1 = EditorGUILayout.IntField("Weapon Price 1", data.pricewep1);
        data.pricewep2 = EditorGUILayout.IntField("Weapon Price 2", data.pricewep2);
        data.pricewep3 = EditorGUILayout.IntField("Weapon Price 3", data.pricewep3);

        EditorGUILayout.Space();
        GUILayout.Label("Ammo Amount", EditorStyles.boldLabel);
        data.ammoAmount1 = EditorGUILayout.IntField("Ammo 1", data.ammoAmount1);
        data.ammoAmount2 = EditorGUILayout.IntField("Ammo 2", data.ammoAmount2);
        data.ammoAmount3 = EditorGUILayout.IntField("Ammo 3", data.ammoAmount3);

        GUILayout.Label("Health Kit", EditorStyles.boldLabel);
        data.healthKitAmount1 = EditorGUILayout.IntField("Small Health Kit", data.healthKitAmount1);
        data.healthKitAmount2 = EditorGUILayout.IntField("Big Health Kit", data.healthKitAmount2);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space();

        if (GUILayout.Button("Save Changes"))
            SaveChanges();

        GUI.backgroundColor = new Color(1f, 0.5f, 0.4f);
        if (GUILayout.Button("Reset Variables"))
            resetButton();
        GUI.backgroundColor = Color.white;
    }

    private void resetButton()
    {
        if (data == null)
            return;

        bool confirm = EditorUtility.DisplayDialog("Reset Variable", "Are you sure you want to reset the variables?", "Reset", "Cancel");

        if (!confirm) return;

        data.money = 0;
        data.enemyDestroyed = 0;
        data.levelwep1 = 1;
        data.levelwep2 = 1;
        data.levelwep3 = 1;
        data.pricewep1 = 1;
        data.pricewep2 = 3;
        data.pricewep3 = 4;
        data.ammoAmount1 = 0;
        data.ammoAmount2 = 0;
        data.ammoAmount3 = 0;
        data.healthKitAmount1 = 0;
        data.healthKitAmount2 = 0;

        SaveChanges();
        Repaint();
    }

    private void LoadSave()
    {
        if (!File.Exists(savePath))
        {
            EditorUtility.DisplayDialog("Error", "Save File not found!", "OK");
            return;
        }

        try

        {
            playerData loadedData;
            using (FileStream file = File.OpenRead(savePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                loadedData = (playerData)formatter.Deserialize(file);
            }
            data = loadedData;
            GUI.changed = true;

            Repaint();

        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", e.Message, "OK");
        }
    }

    private void SaveChanges()
    {
        if (data == null)
            return;
        try
        {
            string directory = Path.GetDirectoryName(savePath);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string backupPath = savePath + ".bak";

            if (File.Exists(savePath) && !File.Exists(backupPath))
                File.Copy(savePath, backupPath);

            using (FileStream file = File.Create(savePath))

            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, data);
            }
            LoadSave();

            EditorUtility.DisplayDialog("Success", "Save data modified", "OK");
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
            EditorUtility.DisplayDialog("Error", e.Message, "OK");
        }
    }

}
