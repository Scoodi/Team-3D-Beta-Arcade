using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTutorialCompleteOnCollision : BaseEnableUIOnCollision
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            player.UIEnabled = true;
            foreach (GameObject obj in uiObjects)
            {
                if (obj.tag == "LevelComplete")
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}
