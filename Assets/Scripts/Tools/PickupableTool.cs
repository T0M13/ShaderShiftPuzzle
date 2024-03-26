using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableTool : InteractableObject
{
    [SerializeField] private GameObject toolToPickup;

    public override void Interact(PlayerReferences playRef)
    {
        GameObject toSpawnTool = Instantiate(toolToPickup, Vector3.zero, Quaternion.identity);
        playRef.PlayerToolSelector.AddTool(toSpawnTool);
        toSpawnTool.transform.localPosition = Vector3.zero;
        toSpawnTool.transform.localRotation = Quaternion.identity;

        if (toSpawnTool.GetComponent<ColorToolManager>())
            playRef.PlayerColorTool.ColorToolManager = toSpawnTool.GetComponent<ColorToolManager>();

        Destroy(gameObject);
    }
}
