using UnityEngine;
using System.Collections;

public class CheckpointTrigger : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController.Instance.startPos = transform.position;
        }
    }
}
