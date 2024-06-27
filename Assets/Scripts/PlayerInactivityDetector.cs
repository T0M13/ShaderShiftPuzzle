using UnityEngine;

public class PlayerInactivityDetector : MonoBehaviour
{
    public float timeToTriggerHelp = 35f;
    [SerializeField] private float timer = 0f;
    [SerializeField] private bool enableOnce = true;
    [SerializeField] private bool isEnabled = false;
    public InteractiveHelpPad helpPad;
    public bool IsEnabled { get => isEnabled; set => isEnabled = value; }

    private void OnTriggerStay(Collider other)
    {
        if (IsEnabled) return;

        if (other.GetComponent<PlayerReferences>())
        {
            //var player = other.GetComponent<PlayerReferences>();
            timer += Time.deltaTime;
            if (timer >= timeToTriggerHelp)
            {
                helpPad.EnableHelppad();
                timer = 0f;
                if (enableOnce)
                    IsEnabled = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsEnabled) return;
        timer = 0f;
        if (enableOnce)
            IsEnabled = true;
    }

}
