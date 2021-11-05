using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReducePlayerSpeed : MonoBehaviour
{
    [Range(1.0f, 99.0f)]
    public float MaxSpeedPercentage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerScript>().torqueForce = collision.GetComponent<PlayerScript>().torqueForce * MaxSpeedPercentage / 100.0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerScript>().torqueForce = collision.GetComponent<PlayerScript>().torqueForce / MaxSpeedPercentage * 100.0f;
        }
    }
}
