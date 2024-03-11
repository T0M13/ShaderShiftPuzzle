using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterDoor : InteractableObject
{

    [SerializeField] private string newScene;

    public override void Interact()
    {
        base.Interact();
        TeleportToScene();
    }

    private void TeleportToScene()
    {
        if(newScene.Length > 0 &&  !string.IsNullOrEmpty(newScene))
        SceneManager.LoadScene(newScene);
    }

}
