using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableUIOnCollision : MonoBehaviour
{
    public PlayerScript player;
    public GameObject ui;
    public UIScript playerHudCanvas;

    // Start is called before the first frame update
    void Start()
    {
        SetUIActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            SetUIActive(true);
            player.ui = playerHudCanvas;
        }
    }

    private void SetUIActive(bool isActive)
    {
        player.UIEnabled = isActive;
        ui.SetActive(isActive);
        foreach (Transform child in ui.transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}
