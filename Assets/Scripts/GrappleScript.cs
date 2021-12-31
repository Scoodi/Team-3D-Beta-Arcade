using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleScript : MonoBehaviour
{
    //Angle to check for collision and apply force (grappleBoost)
    public float shootAngle = 45;
    public float boostAngle = 20;
    public float grappleBoost = 10f;
    //Amount to drain battery for short or long grapple
    public float shortDrain = 5f;
    public float longDrain = 10f;


    public PlayerScript player;
    public LineRenderer lr;
    public DistanceJoint2D dj;
    public GameObject grappleHead;
    public Camera mc;
    // Start is called before the first frame update
    void Start()
    {
        dj.enabled = false;
        mc = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            DisableGrapple();
        }
        if (dj.enabled)
        {
            lr.SetPosition(1, transform.position);
        }
    }

    public void Grapple(bool held, Rigidbody2D playerRb)
    {
        Debug.Log("Grappling, held is: " + held.ToString());
        //Send at angle
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(shootAngle * Mathf.Deg2Rad), Mathf.Sin(shootAngle * Mathf.Deg2Rad)).normalized);
        if (hit.collider != null)
        {
            Debug.Log("Hit with Grapple:" + hit.collider.gameObject.name);
            GameObject g = Instantiate(grappleHead, new Vector2(hit.point.x, hit.point.y), Quaternion.LookRotation(new Vector3(hit.normal.x,0,0),Vector3.up));
            g.transform.Rotate(new Vector3(0, 90, 0));
            StartCoroutine(KillOldGrapple(g));
            if (!held)
            {
                player.DrainBatteryByAmount(shortDrain);
                playerRb.AddForce(new Vector2(Mathf.Cos(boostAngle * Mathf.Deg2Rad), Mathf.Sin(boostAngle * Mathf.Deg2Rad)).normalized * grappleBoost, ForceMode2D.Impulse);
            }
            else
            {
                player.DrainBatteryByAmount(longDrain);
                playerRb.AddForce(new Vector2(Mathf.Cos(boostAngle * Mathf.Deg2Rad), Mathf.Sin(boostAngle * Mathf.Deg2Rad)).normalized * grappleBoost, ForceMode2D.Impulse);
                lr.SetPosition(0, hit.point);
                lr.SetPosition(1, transform.position);
                dj.connectedAnchor = hit.point;
                dj.enabled = true;
                lr.enabled = true;
            }
            
        }
        else
        {
            Debug.Log("Hit nothing");
        }
    }

    public void DisableGrapple()
    {
        dj.enabled = false;
        lr.enabled = false;
    }

    public IEnumerator KillOldGrapple(GameObject oldGrapple)
    {
        yield return new WaitForSeconds(5f);
        Destroy(oldGrapple);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
