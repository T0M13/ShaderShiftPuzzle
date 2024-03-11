using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolObject : MonoBehaviour, ITool
{
    [Header("Tool to be Toggled")]
    public MonoBehaviour scriptTool = null;

    public virtual void DisableTool()
    {
        if (scriptTool != null)
            scriptTool.enabled = false;
    }

    public virtual void EnableTool()
    {
        if (scriptTool != null)
            scriptTool.enabled = true;
    }
}

