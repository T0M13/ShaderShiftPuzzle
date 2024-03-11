using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTool : ToolObject
{
    [Header("References")]
    [SerializeField] PlayerReferences playerReferences;
    [SerializeField] PlayerColorTool playerColorTool;

    private void Awake()
    {
        GetReferences();

        if (playerReferences != null)
        {
            playerColorTool = playerReferences.PlayerColorTool;
            scriptTool = playerColorTool;
        }
    }

    private void GetReferences()
    {
        playerReferences = GetComponentInParent<PlayerReferences>();
    }

}
