using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnableUIOnCollision : MonoBehaviour
{
    public PlayerScript player;
    protected List<GameObject> uiObjects = new List<GameObject>();
    protected int UILayer = 5;

    // Start is called before the first frame update
    protected void Start()
    {
        uiObjects = findGameObjectsWithLayer(UILayer);
        foreach (GameObject obj in uiObjects)
        {
            obj.SetActive(false);
        }
    }

    protected List<GameObject> findGameObjectsWithLayer(int layer)
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
