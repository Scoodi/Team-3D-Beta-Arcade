using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColourSprite : MonoBehaviour
{
    public SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(Random.Range(0f, 1), Random.Range(0f, 1), Random.Range(0f, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
