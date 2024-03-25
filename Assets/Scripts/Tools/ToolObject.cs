using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolObject : MonoBehaviour, ITool
{
    public ToolState toolState;

    public void ChangeActiveTool(PlayerReferences playerRef, ToolState state)
    {
        playerRef.CurrentToolState = state;
    }
}



