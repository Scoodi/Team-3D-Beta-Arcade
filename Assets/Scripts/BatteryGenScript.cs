using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryGenScript : MonoBehaviour
{
    public int StartingBatteryDistance;
    [Range(1.0f, 1.5f)]
    public float DifficultyModifier;
    private GameObject _player;
    public GameObject BatteryPrefab;
    private float _nextPosition = 0f;
    private float _nextDistance = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        Instantiate(BatteryPrefab, new Vector3(StartingBatteryDistance, Random.Range(2.0f, 3.0f), 0), Quaternion.identity);
        _nextDistance = StartingBatteryDistance * DifficultyModifier;
        _nextPosition = StartingBatteryDistance + _nextDistance;
        Instantiate(BatteryPrefab, new Vector3(_nextPosition, Random.Range(2.0f, 3.0f), 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.transform.position.x >= _nextPosition - 10) 
        {
            _nextDistance *= DifficultyModifier;
            _nextPosition += _nextDistance;
            Instantiate(BatteryPrefab, new Vector3(_nextPosition, Random.Range(1.0f, 3.0f), 0), Quaternion.identity);
        }
    }
}
