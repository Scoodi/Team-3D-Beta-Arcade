using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerScript : MonoBehaviour
{
    public UIScript ui;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public SpriteRenderer headGem;
    public GrappleScript grapple;

    public float maxBatteryLevel = 100f;
    public float batteryRemaining;

    public float deathTimer = 3f;
    public float batteryDrain;
    public float torqueForce = 2f;
    public float airTorqueForce = 0.3f;
    public float airForce = 1f;
    public float jumpForce = 20f;

    public float currentSpeed;
    public float maxVelocityMagnitude;

    public float startPoint;
    public float maxDistanceTravelled = 0;

    private bool holdCheck = false;
    private bool holdLock = false;

    public bool inAir = true;

    private IEnumerator batteryDrainCoroutine;

    public bool UIEnabled = true;
    private bool isTutorialCompleted = false;

    public Vector2 prev_velocity;
    PlayerSounds sounds;



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

        if (UIEnabled)
        {
            ui.UpdateUI();
        }
    }

    private void UpdateVars()
    {
        if (rb.velocity.magnitude > maxVelocityMagnitude)
        {
            rb.velocity *= 0.999f;
        }
        currentSpeed = rb.velocity.magnitude * 10;
        if (this.transform.position.x > maxDistanceTravelled)
        {
            maxDistanceTravelled = gameObject.transform.position.x;
        }

        prev_velocity = rb.velocity;
    }

    private void InitialiseVars()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ui = FindObjectOfType<UIScript>();
        grapple = FindObjectOfType<GrappleScript>();
        startPoint = gameObject.transform.position.x;
        sounds = GetComponent<PlayerSounds>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        sounds.Landing();
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
        if (batteryRemaining > 0 && (SceneManager.GetActiveScene().name != "Tutorial" || !isTutorialCompleted) && rb.velocity.magnitude <= maxVelocityMagnitude)
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

    private IEnumerator BatteryDrain(float timeToDrain)
    {
        batteryRemaining -= timeToDrain;
        if (batteryRemaining >= (maxBatteryLevel * 0.66))
        {
            headGem.color = Color.green;
        }
        else if (batteryRemaining >= (maxBatteryLevel * 0.33))
        {
            headGem.color = Color.yellow;
        }
        else if (batteryRemaining > 0)
        {
            headGem.color = Color.red;
        }

        if (UIEnabled)
        {
            ui.UpdateUI();
        }
        yield return new WaitForSeconds(timeToDrain);
        batteryDrainCoroutine = BatteryDrain(timeToDrain);
        StartCoroutine(batteryDrainCoroutine);
    }

    private IEnumerator CheckIfHeld(string button, float time)
    {
        Debug.Log("Beginning Hold Check");
        holdLock = true;
        yield return new WaitForSeconds(time);
        if (Input.GetButton(button))
        {
            Debug.Log(button + " was held");
            grapple.Grapple(true);
        }
        holdLock = false;
    }

    private IEnumerator BeginDeath()
    {
        headGem.color = Color.gray;
        yield return new WaitForSeconds(deathTimer);
        if (batteryRemaining <= 0)
        {
            headGem.color = Color.black;
            ui.DeathUI(maxDistanceTravelled, 420f);
        }
    }

    private void ProcessInputs()
    {
        if (batteryRemaining > 0 && (SceneManager.GetActiveScene().name != "Tutorial" || !isTutorialCompleted))
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (!inAir)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    sounds.Jump();
                }
            }
            if (Input.GetButtonDown("Fire1") && holdLock == false)
            {
                StartCoroutine(CheckIfHeld("Fire1", 0.5f));
            }
            if (Input.GetButtonUp("Fire1") && holdLock == true)
            {
                grapple.Grapple(false);
            }
        }
    }

    public void ModifyBatteryTimeToDrain(float timeToDrain)
    {
        StopCoroutine(batteryDrainCoroutine);
        batteryDrain = timeToDrain;
        batteryDrainCoroutine = BatteryDrain(batteryDrain);
        StartCoroutine(batteryDrainCoroutine);
    }

    void OnDrawGizmos()
    {
        var center = (Vector2)transform.position;
        var half = GetComponent<CircleCollider2D>().radius;

        bool right = Physics2D.OverlapBox(center + Vector2.right * half, new Vector2(0.2f, transform.localScale.y), 0);
        bool left = Physics2D.OverlapBox(center - Vector2.right * half, new Vector2(0.2f, transform.localScale.y), 0);
        bool down = Physics2D.OverlapBox(center + Vector2.down * half, new Vector3(transform.localScale.x, 0.2f), 0);

        Gizmos.color = Color.blue;

        if (right)
        {
            print("r" + right);
        }

        if (left)
        {
            print("l" + left);
        }

        if (down)
        {
            print("d" + down);
        }




    }

    public bool IsTutorialCompleted
    {
        get
        {
            return IsTutorialCompleted;
        }

        set
        {
            isTutorialCompleted = value;
        }
    }
}
