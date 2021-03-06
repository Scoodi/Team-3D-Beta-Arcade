using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePlayersBatteryDrain : MonoBehaviour
{
    [Range(1.0f, 5.0f)]
    public float BatteryDrainMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        //Set layer to avoid collisions with grapple hook
        gameObject.layer = 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            float timeToDrain = collision.GetComponent<PlayerScript>().batteryDrain / BatteryDrainMultiplier;
            collision.GetComponent<PlayerScript>().ModifyBatteryTimeToDrain(timeToDrain);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            float timeToDrain = collision.GetComponent<PlayerScript>().batteryDrain * BatteryDrainMultiplier;
            collision.GetComponent<PlayerScript>().ModifyBatteryTimeToDrain(timeToDrain);
        }
    }
}