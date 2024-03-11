using UnityEngine;

public class RugInteractor : MonoBehaviour
{
    void Update()
    {
        Shader.SetGlobalVector("Position", new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z));
    }
}