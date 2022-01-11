using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBatteryCap : MonoBehaviour
{
    public PlayerScript Player;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.ModifyBatteryTimeToDrain(1f);
        }
    }
}
