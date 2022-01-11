using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleScript : MonoBehaviour
{
    //Angle to apply force (grappleBoost)
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

    private int grapplePointLayerValue = 6;
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
        Vector2 distanceToMouseClick = CalculateDistanceToMouseClick();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, distanceToMouseClick.normalized);
        if (hit.collider != null)
        {
            GameObject g = Instantiate(grappleHead, new Vector2(hit.point.x, hit.point.y), Quaternion.LookRotation(new Vector3(hit.normal.x,0,0),Vector3.up));
            g.transform.Rotate(new Vector3(0, 90, 0));
            StartCoroutine(KillOldGrapple(g));

            if (hit.collider.gameObject.layer == grapplePointLayerValue)
            {
                if (!held)
                {
                    player.DrainBatteryByAmount(shortDrain);
                    playerRb.AddForce(distanceToMouseClick.normalized * grappleBoost, ForceMode2D.Impulse);
                }
                else
                {
                    player.DrainBatteryByAmount(longDrain);
                    playerRb.AddForce(distanceToMouseClick.normalized * grappleBoost, ForceMode2D.Impulse);
                    lr.SetPosition(0, hit.point);
                    lr.SetPosition(1, transform.position);
                    dj.connectedAnchor = hit.point;
                    dj.enabled = true;
                    lr.enabled = true;
                }
            }
        }
        else
        {
            lr.SetPosition(0, transform.position + (new Vector3(distanceToMouseClick.x, distanceToMouseClick.y, 0).normalized * 30));
            lr.SetPosition(1, transform.position);
            lr.enabled = true;
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

    private Vector2 CalculateDistanceToMouseClick()
    {
        //Send player towards mouse position where clicked - adjust z value for perspective camera
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mc.transform.position.z;
        mousePos = mc.ScreenToWorldPoint(mousePos);
        return new Vector2((mousePos.x - transform.position.x), (mousePos.y - transform.position.y));
    }
}
