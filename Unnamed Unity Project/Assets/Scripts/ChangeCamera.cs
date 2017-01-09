using UnityEngine;
using System.Collections;

public class ChangeCamera : MonoBehaviour {

    public CameraFollow cameraFollow;
    public Vector3 minCameraPos;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            cameraFollow.minCameraPos = minCameraPos;
        }
    }
}
