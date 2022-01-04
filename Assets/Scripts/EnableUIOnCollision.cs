using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableUIOnCollision : MonoBehaviour
{
    public PlayerScript player;
    private List<GameObject> uiObjects = new List<GameObject>();
    private int UILayer = 5;
    public string Tag;
    public UIScript playerHudCanvas;

    // Start is called before the first frame update
    protected void Start()
    {
        uiObjects = findGameObjectsWithLayer(UILayer);
        foreach (GameObject obj in uiObjects)
        {
            obj.SetActive(false);
        }
        player.UIEnabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            player.UIEnabled = true;
            foreach (GameObject obj in uiObjects)
            {
                if (obj.tag == Tag)
                {
                    obj.SetActive(true);
                }
            }

            player.ui = playerHudCanvas;
        }
    }

    protected List<GameObject> findGameObjectsWithLayer(int layer)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
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
