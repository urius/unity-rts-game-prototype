using UnityEngine;

public class CameraYRotationSync : MonoBehaviour
{
    void Update()
    {
        var rotation = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(rotation.x, Camera.main.transform.rotation.eulerAngles.y, rotation.z);
    }
}
