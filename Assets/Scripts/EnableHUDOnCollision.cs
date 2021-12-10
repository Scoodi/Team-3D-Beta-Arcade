using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableHUDOnCollision : MonoBehaviour
{
    public PlayerScript player;
    private List<GameObject> uiObjects = new List<GameObject>();
    private int UILayer = 5;

    // Start is called before the first frame update
    void Start()
    {
        player.UIEnabled = false;
        uiObjects = findGameObjectsWithLayer(UILayer);
        foreach (GameObject obj in uiObjects)
        {
            obj.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            player.UIEnabled = true;
            foreach (GameObject obj in uiObjects)
            {
                obj.SetActive(true);
            }
        }
    }

    private List<GameObject> findGameObjectsWithLayer(int layer)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<GameObject> objectsWithLayer = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layer)
            {
                objectsWithLayer.Add(obj);
            }
        }

        return objectsWithLayer;
    }
}
