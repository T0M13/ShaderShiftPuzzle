#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundPlayerOnObject))]
public class SoundPlayerOnObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SoundPlayerOnObject soundPlayer = (SoundPlayerOnObject)target;

        // Attempt to find the AudioManager instance using a tag
        AudioManager audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            GameObject audioManagerObject = GameObject.FindGameObjectWithTag("AudioManager");
            if (audioManagerObject != null)
            {
                audioManager = audioManagerObject.GetComponent<AudioManager>();
            }
        }

        if (audioManager != null)
        {
            // Get the list of sound names
            string[] soundNames = new string[audioManager.soundEffects.Length];
            for (int i = 0; i < audioManager.soundEffects.Length; i++)
            {
                soundNames[i] = audioManager.soundEffects[i].name;
            }

            // Display the dropdown with sound names
            int selectedIndex = System.Array.IndexOf(soundNames, soundPlayer.SoundName);
            if (selectedIndex == -1) selectedIndex = 0;
            selectedIndex = EditorGUILayout.Popup("Sound Name", selectedIndex, soundNames);
            soundPlayer.SoundName = soundNames[selectedIndex];
        }
        else
        {
            EditorGUILayout.HelpBox("AudioManager instance not found in the scene.", MessageType.Warning);
        }

        DrawDefaultInspector();
    }
}
#endif
