using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerToolSelector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerReferences playerReferences;
    [SerializeField] private Transform toolParent;
    [Header("Tools")]
    [SerializeField] private List<GameObject> toolsList;
    [SerializeField] private int currentTool = 0;
    [SerializeField] private Vector2 scrollValue;

    private void Awake()
    {
        GetReferences();
        AddTools();

        currentTool = 0;

        SwitchToTool(currentTool);
    }

    private void GetReferences()
    {
        playerReferences = GetComponentInParent<PlayerReferences>();
    }

    public void AddTool(GameObject tool)
    {
        tool.transform.SetParent(transform);
        toolsList.Add(tool);
    }

    private void AddTools()
    {
        for (int i = 0; i < toolParent.childCount; i++)
        {
            if (!toolsList.Contains(toolParent.GetChild(i).gameObject))
            {
                toolsList.Add(toolParent.GetChild(i).gameObject);
            }
        }
    }

    private void RemoveAllTools()
    {
        toolsList.Clear();

        for (int i = 0; i < toolParent.childCount; i++)
        {
            Destroy(toolParent.GetChild(i).gameObject);
        }
    }

    private void RemoveTool(int index)
    {
        toolsList.Remove(toolsList[index]);
        Destroy(toolParent.GetChild(index).gameObject);
    }

    private void SwitchToolsUp()
    {
        currentTool++;
        if (currentTool > toolsList.Count - 1)
        {
            currentTool = 0;
        }
        SwitchToTool(currentTool);
    }

    private void SwitchToolsDown()
    {
        currentTool--;
        if (currentTool < 0)
        {
            currentTool = toolsList.Count - 1;
        }
        SwitchToTool(currentTool);
    }

    private void SwitchToTool(int index)
    {
        if (toolsList.Count <= 0) return;

        for (int i = 0; i < toolParent.childCount; i++)
        {
            toolParent.GetChild(i).gameObject.SetActive(false);
            if(toolParent.GetChild(i).gameObject.GetComponent<ITool>() != null)
            {
                toolParent.GetChild(i).gameObject.GetComponent<ITool>().DisableTool();
            }
        }

        toolParent.GetChild(index).gameObject.SetActive(true);
        if (toolParent.GetChild(index).gameObject.GetComponent<ITool>() != null)
        {
            toolParent.GetChild(index).gameObject.GetComponent<ITool>().EnableTool();
        }

    }

    private void OnScroll(InputValue value)
    {
        scrollValue = value.Get<Vector2>();

        if(scrollValue.y > 1)
        {
            SwitchToolsUp();
        }

        if (scrollValue.y < -1)
        {
            SwitchToolsDown();
        }
    }

}
