using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleScript : MonoBehaviour
{
    public float shootAngle = 45;
    public GameObject grappleHead;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grapple (bool held)
    {
        Debug.Log("Grappling, held is: " + held.ToString());
        //Send at angle
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(shootAngle * Mathf.Deg2Rad), Mathf.Sin(shootAngle * Mathf.Deg2Rad)).normalized);
        if (hit.collider != null)
        {
            Debug.Log("Hit with Grapple:" + hit.collider.gameObject.name);
            Instantiate(grappleHead, hit.point, Quaternion.identity);
        } else
        {
            Debug.Log("Hit nothing");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
