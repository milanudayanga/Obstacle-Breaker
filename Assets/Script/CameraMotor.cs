using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{

    public Transform lookAt;
    public Vector3 offset = new Vector3(0, 3.0f, -5.0f);
    public Vector3 rotation = new Vector3(0, 0, 0);
    public bool Ismoving { set; get; }

    //private void Start()
   // {
    //    transform.position = lookAt.position + offset;
   // }
    private void LateUpdate()
    {

        if (!Ismoving)
            return;

        transform.position = lookAt.position + offset;
        Vector3 desiredPosition = lookAt.position + offset;
       // Vector3 desiredPosition = transform.position;

        desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, .5f);
        transform.rotation = Quaternion.Euler(rotation);
    }

}

