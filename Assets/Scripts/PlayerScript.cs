using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerScript : MonoBehaviour
{
    public UIScript ui;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public SpriteRenderer headGem;

    public float maxBatteryLevel = 100f;
    public float batteryRemaining = 100f;

    public float batteryDrain = 1f;
    public float torqueForce = 2f;
    public float airTorqueForce = 0.3f;
    public float airForce = 1f;
    public float jumpForce = 20f;

    public float currentSpeed;

    public float startPoint;
    public float maxDistanceTravelled = 0;

    private bool inAir = true;
    // Start is called before the first frame update
    void Start()
    {
        InitialiseVars();
        StartCoroutine(BatteryDrain(batteryDrain));
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        UpdateVars();
        ui.UpdateUI();
    }

    private void UpdateVars()
    {
        currentSpeed = rb.velocity.magnitude * 10;
        if (this.transform.position.x > maxDistanceTravelled)
        {
            maxDistanceTravelled = gameObject.transform.position.x;
        }
    }

    private void InitialiseVars()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ui = FindObjectOfType<UIScript>();
        startPoint = gameObject.transform.position.x;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        inAir = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        inAir = true;
    }
    private void FixedUpdate()
    {
        if (batteryRemaining > 0)
        {
            if (!inAir)
            {
                rb.AddTorque(-Input.GetAxis("Horizontal") * torqueForce);
            }
            else
            {
                rb.AddTorque(-Input.GetAxis("Horizontal") * airTorqueForce);
                rb.AddForce(Vector2.right * Input.GetAxis("Horizontal"));
            }
        }
    }

    private IEnumerator BatteryDrain (float timeToDrain)
    {
        batteryRemaining -= 1f;
        if (batteryRemaining >= (maxBatteryLevel * 0.66))
        {
            headGem.color = Color.green;
        } else if (batteryRemaining >= (maxBatteryLevel * 0.33)) {
            headGem.color = Color.yellow;
        } else if (batteryRemaining > 0)
        {
            headGem.color = Color.red;
        } else
        {
            headGem.color = Color.black;
        }
        ui.UpdateUI();
        yield return new WaitForSeconds(timeToDrain);
        StartCoroutine(BatteryDrain(timeToDrain));
    }

    private void ProcessInputs()
    {
        if (batteryRemaining > 0) {
            if (Input.GetButtonDown("Jump"))
            {
                if (!inAir)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            }
            if (Input.GetButtonDown("Fire1"))
            {
                //TODO: GRAPPLE

            }
    }
    }
}
