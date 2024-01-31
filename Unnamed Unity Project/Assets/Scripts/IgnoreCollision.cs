using UnityEngine;
using System.Collections;

public class IgnoreCollision : MonoBehaviour {

    public Collider2D other;

    private void Awake()
    {
        //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other, true);
    }
}
