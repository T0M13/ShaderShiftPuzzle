using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMessageBox : MonoBehaviour
{
    [SerializeField] private string taskMessage = "";

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerReferences>() != null)
            InteractiveMessageUI.Instance.ChangeTaskMessage(taskMessage);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerReferences>() != null)
            InteractiveMessageUI.Instance.ChangeTaskMessage("");
    }
}
