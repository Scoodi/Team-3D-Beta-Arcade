using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    public float length, startPos, pallax;
    public GameObject camera;
    //public PlayerScript player;

    void Start()
    {
        startPos = transform.parent.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = camera.transform.position.x * (1 - pallax);
        float dist = camera.transform.position.x * pallax;
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length ) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}
