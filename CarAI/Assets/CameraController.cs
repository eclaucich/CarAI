using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance = null;

    private Car followTarget;

    private void Start()
    {
        if (instance)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

}
