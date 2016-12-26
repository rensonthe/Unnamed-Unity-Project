using UnityEngine;
using System.Collections;
using Fungus;

public class OnTrigger : MonoBehaviour {

    public string Message;
    public Flowchart flowchart;
    public PlayerController playerController;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            if (flowchart.GetBooleanVariable("Triggered") == false)
            {
                playerController.moveSpeed = 0;
                playerController.trig = true;


                flowchart.SendFungusMessage(Message);
            }
        } 
    }
}
