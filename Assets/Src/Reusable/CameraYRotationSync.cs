using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraYRotationSync : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rotattion = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(rotattion.x, Camera.main.transform.rotation.eulerAngles.y, rotattion.z);
    }
}
