using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CostumeSelector : MonoBehaviour
{
    public List<Sprite> costumes = new List<Sprite>();
    public SkinOptionsScriptableObject skinOptions;

    public Image costume;
    public Sprite currentCostume;
    public int target = 0;

    public void selectLeft()
    {
        target--;
        if (target < 0)
        {
            target = costumes.Count - 1;
        }

        currentCostume = costumes[target];
    }

    public void selectRight()
    {
        target++;
        if (target > costumes.Count - 1)
        {
            target = 0;
        }

        currentCostume = costumes[target];
    }

    public void ApplyChanges()
    {
        skinOptions.skin = currentCostume;
    }

    void WriteFromOptions()
    {
        currentCostume = skinOptions.skin;
    }

    void Start()
    {
        if (!skinOptions.skin)
        {
            skinOptions.skin = costumes[0];
        }
        WriteFromOptions();
        costume.sprite = currentCostume;
    }

    void Update()
    {
        if (currentCostume != costume.sprite)
        {
            costume.sprite = currentCostume;
        }
    }
}
