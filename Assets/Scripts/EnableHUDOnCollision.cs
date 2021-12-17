using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableHUDOnCollision : BaseEnableUIOnCollision
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        player.UIEnabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            player.UIEnabled = true;
            foreach (GameObject obj in uiObjects)
            {
                if (obj.tag == "HUD")
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}
