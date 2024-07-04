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
            EditorGUILayout.IntField("Current Game Version", SaveData.Current.playerGameData.currentGameVersion);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Unlocked Levels", EditorStyles.boldLabel);
            for (int i = 0; i < SaveData.Current.playerGameData.unlockedLevels.Count; i++)
            {
                EditorGUILayout.TextField($"{SaveData.Current.playerGameData.unlockedLevels[i]}", SaveData.Current.playerGameData.unlockedLevels[i]);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Player Profile Settings", EditorStyles.boldLabel);
            EditorGUILayout.Slider("Master Volume", SaveData.Current.playerProfile.masterVolume, 0, 100);
            EditorGUILayout.Slider("Music Volume", SaveData.Current.playerProfile.musicVolume, 0, 100);
            EditorGUILayout.Slider("Effects Volume", SaveData.Current.playerProfile.effectsVolume, 0, 100);
            EditorGUILayout.Space();
            EditorGUILayout.Slider("Aim Sensitivity", SaveData.Current.playerProfile.aimSensitivity, 1f, 100f);
            EditorGUILayout.Toggle("Reverse Mouse", SaveData.Current.playerProfile.reverseMouse);
            EditorGUILayout.Space();
            EditorGUILayout.Slider("Brightness", SaveData.Current.playerProfile.brightness, -.5f, .5f);
            EditorGUILayout.Slider("Gamma", SaveData.Current.playerProfile.gamma, -.5f, .5f);
            EditorGUILayout.IntField("Vsync", SaveData.Current.playerProfile.vsync);
            EditorGUILayout.IntField("Resolution Width", SaveData.Current.playerProfile.currentResolutionWidth);
            EditorGUILayout.IntField("Resolution Height", SaveData.Current.playerProfile.currentResolutionHeight);
            EditorGUILayout.LabelField("Window Mode", SaveData.Current.playerProfile.currentScreenMode.ToString());


        }
        else
        {
            EditorGUILayout.HelpBox("No current SaveData loaded", MessageType.Info);
        }
    }
}
#endif
