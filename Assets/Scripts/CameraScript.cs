using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: ADD CAMERA FOLLOW THAT KEEPS PLAYER WITHIN "SAFEZONE" ON SCREEN, LIKELY WITH MOVE DAMPENING
public class CameraScript : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(target.position.x + 8.5f, transform.position.y, transform.position.z);
    }
}
