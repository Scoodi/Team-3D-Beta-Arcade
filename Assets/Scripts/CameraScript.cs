using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//TODO: ADD CAMERA FOLLOW THAT KEEPS PLAYER WITHIN "SAFEZONE" ON SCREEN, LIKELY WITH MOVE DAMPENING
public class CameraScript : MonoBehaviour
{
    public Transform target;
    private float _width;

    // Start is called before the first frame update
    void Start()
    {
        float height = 2f * Camera.main.orthographicSize;
        _width = height * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "TutorialMap")
        {
            this.transform.position = new Vector3(Mathf.Max(Mathf.Min(target.position.x + ((_width / 2f) - 8), 320f),2f), transform.position.y, transform.position.z);
        } 
        else
        {
            this.transform.position = new Vector3(target.position.x + ((_width / 2f) - 8), transform.position.y, transform.position.z);
        }
    }
}
