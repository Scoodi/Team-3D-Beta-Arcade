using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlayerInputOnTutorialCompletion : MonoBehaviour
{
    public PlayerScript Player;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player.IsTutorialCompleted = true;
        }
    }
}
