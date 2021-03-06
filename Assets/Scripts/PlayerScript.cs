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
    public int maxDistanceTravelled = 0;

    private bool holdLock = false;

    public bool inAir = true;

    private IEnumerator batteryDrainCoroutine;

    public bool UIEnabled = true;
    private bool isTutorialCompleted = false;

    public Vector2 prev_velocity;
    Vector2 contained_velocity;
    float contained_av;

    public string currentBiome = "beach";
    public SkinOptionsScriptableObject skinOptions;
    PlayerSounds sounds;
    public LayerMask lm;
    bool canStomp = true;
    bool stompin = false;

    // Start is called before the first frame update
    void Start()
    {
        InitialiseVars();
        StartCoroutine(BatteryDrain(batteryDrain));
    }

    float counter = 0;
    float stompCooldown = 3;

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();

        if (counter >= stompCooldown)
        {
            canStomp = true;

            counter = 0;
        }
        else
        {
            counter += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && inAir && canStomp)
        {
            contained_velocity = rb.velocity;
            contained_av = rb.angularVelocity;
            rb.AddForce(Vector2.down * 20, ForceMode2D.Impulse);
            rb.angularVelocity = 0;
            stompin = true;
            canStomp = false;
        }

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
            maxDistanceTravelled = Mathf.RoundToInt(gameObject.transform.position.x);
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

        sr.sprite = !skinOptions.skin ? skinOptions.defaultSkin : skinOptions.skin;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        sounds.Landing();

        if (stompin)
        {
            rb.velocity = Vector2.zero;

            //Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + Vector2.Perpendicular(collision.contacts[0].normal) * 15, Color.red);

            var vec = Vector2.Perpendicular(collision.contacts[0].normal);

            rb.angularVelocity = contained_av * 1.05f;
            rb.AddForce(vec * contained_velocity.magnitude * Mathf.Sign(contained_av), ForceMode2D.Impulse);

            stompin = false;
        }

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Untagged" && currentBiome != collision.tag)
        {
            currentBiome = collision.tag;
        }
    }

    public void DrainBatteryByAmount(float amount)
    {
        batteryRemaining -= amount;
    }

    private IEnumerator BatteryDrain(float timeToDrain)
    {
        DrainBatteryByAmount(timeToDrain);

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
        if (batteryRemaining > 0)
        {
            StartCoroutine(batteryDrainCoroutine);
        }
        else
        {
            StartCoroutine(BeginDeath());
        }
    }

    private IEnumerator CheckIfHeld(string button, float time)
    {
        //Debug.Log("Beginning Hold Check");
        holdLock = true;
        yield return new WaitForSeconds(time);
        if (Input.GetButton(button))
        {
            //   Debug.Log(button + " was held");
            grapple.Grapple(true, rb);
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
            ui.DeathUI(maxDistanceTravelled);
        }
    }

    private void ProcessInputs()
    {
        if (batteryRemaining > 0 && (SceneManager.GetActiveScene().name != "Tutorial" || !isTutorialCompleted))
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (CheckCanJump())
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        rb.AddForce(Vector2.up * jumpForce * 1.15f, ForceMode2D.Impulse);
                        rb.velocity = new Vector2(rb.velocity.x * .7f, rb.velocity.y);
                        sounds.Jump();

                    }
                    else
                    {
                        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                        sounds.Jump();
                    }

                }
            }
            if (Input.GetButtonDown("Fire1") && holdLock == false)
            {
                StartCoroutine(CheckIfHeld("Fire1", 0.2f));
            }
            if (Input.GetButtonUp("Fire1") && holdLock == true)
            {
                grapple.Grapple(false, rb);
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

    bool CheckCanJump()
    {
        var center = (Vector2)transform.position;
        var half = GetComponent<CircleCollider2D>().radius;

        bool ir = Physics2D.OverlapBox(center + Vector2.right * .3f, new Vector2(0.1f, transform.localScale.y * .5f), 0, lm);
        bool il = Physics2D.OverlapBox(center - Vector2.right * .3f, new Vector2(0.1f, transform.localScale.y * .5f), 0, lm);
        bool l, r, g;

        l = Physics2D.Raycast(center - Vector2.right * .35f, Vector2.left, .5f);
        l = Physics2D.Raycast(center - Vector2.right * .35f + Vector2.down * transform.localScale.y * .5f * .5f, Vector2.left, .5f);
        l = Physics2D.Raycast(center - Vector2.right * .35f - Vector2.down * transform.localScale.y * .5f * .5f, Vector2.left, .5f);

        r = Physics2D.Raycast(center - Vector2.left * .35f, Vector2.right, .5f);
        r = Physics2D.Raycast(center - Vector2.left * .35f + Vector2.down * transform.localScale.y * .5f * .5f, Vector2.right, .5f);
        r = Physics2D.Raycast(center - Vector2.left * .35f - Vector2.down * transform.localScale.y * .5f * .5f, Vector2.right, .5f);

        g = Physics2D.Raycast(center, Vector2.down, 1.2f * half);
        g = Physics2D.Raycast(center + Vector2.right * .35f, Vector2.down, half * 1.05f);
        g = Physics2D.Raycast(center - Vector2.right * .35f, Vector2.down, half * 1.05f);

        if (inAir)
        {
            return false;
        }

        if (l || r)
        {
            if (g)
            {
                if (il || ir)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    void OnDrawGizmos()
    {
        //var center = (Vector2)transform.position;
        //var half = GetComponent<CircleCollider2D>().radius;
        //Gizmos.color = Color.blue;

        //Gizmos.DrawWireCube(center - Vector2.right * .3f, new Vector2(0.1f, transform.localScale.y * .5f));
        //Gizmos.DrawWireCube(center + Vector2.right * .3f, new Vector2(0.1f, transform.localScale.y * .5f));

        ////l
        //Color c = Color.red;

        //bool l;
        //l = Physics2D.Raycast(center - Vector2.right * .35f, Vector2.left, .5f);
        //l = Physics2D.Raycast(center - Vector2.right * .35f + Vector2.down * transform.localScale.y * .5f * .5f, Vector2.left, .5f);
        //l = Physics2D.Raycast(center - Vector2.right * .35f - Vector2.down * transform.localScale.y * .5f * .5f, Vector2.left, .5f);

        //if (l)
        //{
        //    c = Color.green;
        //}

        //Gizmos.color = c;

        //Gizmos.DrawRay(center - Vector2.right * .35f, Vector2.left * .5f);
        //Gizmos.DrawRay(center - Vector2.right * .35f + Vector2.down * transform.localScale.y * .5f * .5f, Vector2.left * .5f);
        //Gizmos.DrawRay(center - Vector2.right * .35f - Vector2.down * transform.localScale.y * .5f * .5f, Vector2.left * .5f);

        ////r
        //c = Color.red;
        //bool r;
        //r = Physics2D.Raycast(center - Vector2.left * .35f, Vector2.right, .5f);
        //r = Physics2D.Raycast(center - Vector2.left * .35f + Vector2.down * transform.localScale.y * .5f * .5f, Vector2.right, .5f);
        //r = Physics2D.Raycast(center - Vector2.left * .35f - Vector2.down * transform.localScale.y * .5f * .5f, Vector2.right, .5f);

        //if (r)
        //{
        //    c = Color.green;
        //}

        //Gizmos.color = c;

        //Gizmos.DrawRay(center - Vector2.left * .35f, Vector2.right * .5f);
        //Gizmos.DrawRay(center - Vector2.left * .35f + Vector2.down * transform.localScale.y * .5f * .5f, Vector2.right * .5f);
        //Gizmos.DrawRay(center - Vector2.left * .35f - Vector2.down * transform.localScale.y * .5f * .5f, Vector2.right * .5f);

        ////ground
        //c = Color.red;
        //bool g;
        //g = Physics2D.Raycast(center, Vector2.down, 1.2f * half);
        //g = Physics2D.Raycast(center + Vector2.right * .35f, Vector2.down, half * 1.05f);
        //g = Physics2D.Raycast(center - Vector2.right * .35f, Vector2.down, half * 1.05f);

        //if (g)
        //{
        //    c = Color.green;
        //}

        //Gizmos.color = c;

        //Gizmos.DrawRay(center, Vector2.down * half * 1.2f);
        //Gizmos.DrawRay(center + Vector2.right * .35f, (Vector2.down * half * 1.05f));
        //Gizmos.DrawRay(center - Vector2.right * .35f, (Vector2.down * half * 1.05f));
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
