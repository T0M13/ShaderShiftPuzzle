using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextChapter : MonoBehaviour
{
    [SerializeField] private string chapterName;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.SwitchToLevel(chapterName);
    }
}
