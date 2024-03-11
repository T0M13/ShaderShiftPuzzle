using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldTool : ToolObject
{
    [Header("References")]
    [SerializeField] PlayerReferences playerReferences;
    [SerializeField] PlayerHoldTool playerHoldTool;

    private void Awake()
    {
        GetReferences();

        if (playerReferences != null)
        {
            playerHoldTool = playerReferences.PlayerHoldTool;
            scriptTool = playerHoldTool;
        }
    }

    private void GetReferences()
    {
        playerReferences = GetComponentInParent<PlayerReferences>();
    }

}
