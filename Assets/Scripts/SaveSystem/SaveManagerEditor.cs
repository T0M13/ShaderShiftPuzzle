#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using tomi.SaveSystem;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Draws the default inspector

        // Check if SaveManager and SaveData.Current are available
        if (SaveManager.Instance != null && SaveData.Current != null)
        {
            EditorGUILayout.LabelField("Current Save Data", EditorStyles.boldLabel);
            EditorGUILayout.TextField("Save Name", SaveData.Current.saveMetaData.saveName);
            EditorGUILayout.TextField("Description", SaveData.Current.saveMetaData.description);
            EditorGUILayout.Toggle("Autosave", SaveData.Current.saveMetaData.autoSave);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Player Game Data", EditorStyles.boldLabel);
            EditorGUILayout.TextField("Current Level Name", SaveData.Current.playerGameData.currentLevelName);
            EditorGUILayout.IntField("Current Level Thumbnail Index", SaveData.Current.playerGameData.currentLevelThumbnailIndex);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Player Profile Settings", EditorStyles.boldLabel);
            EditorGUILayout.Slider("Master Volume", SaveData.Current.playerProfile.masterVolume, 0, 100);
            EditorGUILayout.Slider("Music Volume", SaveData.Current.playerProfile.musicVolume, 0, 100);
            EditorGUILayout.Slider("Effects Volume", SaveData.Current.playerProfile.effectsVolume, 0, 100);
            EditorGUILayout.Slider("Aim Sensitivity", SaveData.Current.playerProfile.aimSensitivity, 1f, 100f);
            EditorGUILayout.Toggle("Reverse Mouse", SaveData.Current.playerProfile.reverseMouse);
        }
        else
        {
            EditorGUILayout.HelpBox("No current SaveData loaded", MessageType.Info);
        }
    }
}
#endif
