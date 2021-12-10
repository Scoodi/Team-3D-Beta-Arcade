using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabEndPointScript : MonoBehaviour
{
    public GameObject LabCompleted;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            LabCompleted.gameObject.SetActive(true);
        }

    }



  
}
