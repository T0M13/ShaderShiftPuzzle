using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerToolSelector : MonoBehaviour
{
    [Header("References")]
    [ShowOnly][SerializeField] private PlayerReferences playerReferences;
    [SerializeField] private Transform toolParent;
    [Header("Tools")]
    [SerializeField] private List<GameObject> toolsList;
    [ShowOnly][SerializeField] private int currentTool = 0;
    [ShowOnly][SerializeField] private Vector2 scrollValue;

    private void Awake()
    {
        GetReferences();
        GetTools();
        DeactivateAllTools();
        currentTool = 0;
        SwitchToTool(0);
    }

    private void GetReferences()
    {
        playerReferences = GetComponentInParent<PlayerReferences>();
    }

    private void GetTools()
    {
        for (int i = 0; i < toolParent.childCount; i++)
        {
            toolsList.Add(toolParent.GetChild(i).gameObject);
        }
    }

    public void AddTool(GameObject toAddTool)
    {
        toAddTool.SetActive(false);
        toAddTool.transform.parent = toolParent.transform;
        toolsList.Add(toAddTool);
    }

    private void DeactivateAllTools()
    {
        foreach (var tool in toolsList)
        {
            tool.SetActive(false);
        }
    }

    private void SwitchTools(int direction)
    {
        int oldToolIndex = currentTool;
        ToolState oldState = toolsList[currentTool].GetComponent<ToolObject>().toolState;

        currentTool += direction;
        if (currentTool >= toolsList.Count)
        {
            currentTool = 0;
        }
        else if (currentTool < 0)
        {
            currentTool = toolsList.Count - 1;
        }

        ToolState newState = toolsList[currentTool].GetComponent<ToolObject>().toolState;

        OnChangingTools(oldState, newState);

        toolsList[oldToolIndex].SetActive(false);

        SwitchToTool(currentTool);
    }

    private void OnChangingTools(ToolState oldState, ToolState newState)
    {
        if (oldState == newState) return;
        switch (oldState)
        {
            case ToolState.HoldTool:
                if (playerReferences.PlayerHoldTool.HoldingObject)
                    playerReferences.PlayerHoldTool.DropObject();
                break;
            case ToolState.ColorTool:
                break;
            default:
                break;
        }
    }

    private void SwitchToTool(int index)
    {
        toolsList[index].SetActive(true);
        playerReferences.CurrentToolState = toolsList[currentTool].GetComponent<ToolObject>().toolState;
    }

    public void OnScroll(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            var scrollValue = value.ReadValue<Vector2>();
            if (scrollValue.y > 0)
            {
                SwitchTools(1);
            }
            else if (scrollValue.y < 0)
            {
                SwitchTools(-1);
            }
        }
    }

}
