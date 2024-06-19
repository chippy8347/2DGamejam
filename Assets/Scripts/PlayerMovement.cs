using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Vector3=UnityEngine.Vector3;
using Vector2=UnityEngine.Vector2;
using Random=UnityEngine.Random;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private GameObject checkPoint;

    private Vector3 checkPointPos;
    private float speed = 15.0f;
    private float jumpPower = 20.0f;
    private float dashSpeed = 30.0f;
    private float dashMaxDistance = 7f;
    private Rigidbody2D body;
    private Animator anim;
    private CapsuleCollider2D capsuleCollider;
    private bool isPlatform;
    private bool isGrounded;
    private bool jumpedOffPlatform;
    private bool checkPointreached;
    private Vector2 jumpedOffPlatformVel;
    private float jumpCooldownTime = 0.25f;
    private bool canJump;
    private float jumpTimer;
    public static bool playing;
    public static float dashcooldown = 1.5f;
    public static float dashTimer;
    public static bool canDash;
    private bool dashing;
    private float dashTime;
    private float dashDistance;
    private Vector3 dashDirection;
    private float gravScale;
    private bool riding;
    private AudioSource[] audioSources;

    private void Awake()
    {
        audioSources = GetComponents<AudioSource>();
        checkPointPos = checkPoint.transform.position;
        body = GetComponent<Rigidbody2D>();
        gravScale = body.gravityScale;
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        canDash = true;
        canJump = true;
        playing = true;
        if (transform.parent != null)
            riding = true;
        else
            riding = false;
    }
    void Update()
    {
        if (playing)
        {
            if (transform.position.y < -15.0f)
                Die();
            if (transform.position.x > checkPointPos.x)
            {
                checkPoint.GetComponent<Light2D>().color = new Color(0f, 1f, 0f, 200.0f / 255f);
                checkPointreached = true;
            }
            // bool for any ground
            isGrounded = Physics2D.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, CapsuleDirection2D.Vertical, 0, Vector2.down, 0.1f, groundLayer | platformLayer | wallLayer);
            // bool for if the ground is a platform (moves)
            isPlatform = Physics2D.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, CapsuleDirection2D.Vertical, 0, Vector2.down, 0.1f, platformLayer);
            // specific platform collided with
            RaycastHit2D platformHit = Physics2D.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, CapsuleDirection2D.Vertical, 0, Vector2.down, 0.1f, platformLayer);
            if (!canJump && isGrounded) 
            {
                if (jumpTimer > jumpCooldownTime)
                {
                    jumpTimer = 0;
                    canJump = true;
                }else
                {
                    jumpTimer += Time.deltaTime;
                }
            }

            if (!dashing)
            {
                if (!canDash)
                {
                    if (dashTimer > dashcooldown)
                    {
                        dashTimer = 0;
                        canDash = true;
                    }else
                    {
                        dashTimer += Time.deltaTime;
                    }
                }
                // left and right movement
                float horizontalInput = Input.GetAxis("Horizontal");
                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

                // If on a platform, add its velocity to the player
                if (isPlatform)
                    body.velocity += new Vector2 (platformHit.collider.GetComponent<Rigidbody2D>().velocity.x, 0);


                // Flip player when moving left-right
                
                if (horizontalInput > 0.01f)
                {
                    if (riding)
                        transform.localScale = new Vector3(1/transform.parent.localScale.x, 1/transform.parent.localScale.y, 1/transform.parent.localScale.z)/transform.parent.parent.localScale.x;
                    else
                        transform.localScale = Vector3.one;
                }
                else if (horizontalInput < -0.01f)
                    if (riding)
                        transform.localScale = new Vector3(-1/transform.parent.localScale.x, 1/transform.parent.localScale.y, 1/transform.parent.localScale.z)/transform.parent.parent.localScale.x;
                    else
                        transform.localScale = new Vector3(-1, 1, 1);

                // jumping
                if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && isGrounded && canJump) 
                {
                    audioSources[Random.Range(2,4)].Play();
                    canJump = false;
                    if (isPlatform) // platform jumping
                    {
                        jumpedOffPlatform = true;
                        isPlatform = false;
                        isGrounded = false;
                        jumpedOffPlatformVel = platformHit.collider.GetComponent<Rigidbody2D>().velocity;
                    }
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    anim.SetTrigger("jump");
                }

                if (Input.GetMouseButtonDown(0) && !dashing && canDash)
                {
                    canDash = false;
                    dashing = true;
                    Vector3 startPos = transform.position;
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = -Camera.main.transform.position.z;
                    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
                    mouseWorldPos.z = 0f;

                    dashDistance = Vector3.Distance(startPos, mouseWorldPos);
                    dashDirection = (mouseWorldPos - startPos).normalized;

                    dashDistance = Mathf.Min(dashDistance, dashMaxDistance);

                    dashTime = dashDistance / dashSpeed;

                    body.gravityScale = 0;

                    body.velocity = dashDirection * dashSpeed;
                    audioSources[Random.Range(0,2)].Play();

                    anim.SetTrigger("dash");

                    Invoke("StopDash", dashTime);
                } 
                else if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift)) && !dashing && canDash)
                {
                    dashDirection = Vector3.zero;
                    canDash = false;
                    dashing = true;

                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
                        dashDirection += Vector3.up;
                    if (Input.GetKey(KeyCode.S))
                        dashDirection += Vector3.down;
                    if (Input.GetKey(KeyCode.A))
                        dashDirection += Vector3.left;
                    if (Input.GetKey(KeyCode.D))
                        dashDirection += Vector3.right;
                    if (dashDirection == Vector3.zero)
                    {
                        if (transform.localScale.x > 0)
                            dashDirection += Vector3.right;
                        else
                            dashDirection += Vector3.left;
                    }
                    
                    dashDirection = dashDirection.normalized;
                    dashDistance = dashMaxDistance;
                    dashTime = dashDistance / dashSpeed;
                    body.gravityScale = 0;
                    audioSources[Random.Range(0,2)].Play();

                    body.velocity = dashDirection * dashSpeed;
                    anim.SetTrigger("dash");
                    
                    Invoke("StopDash", dashTime);
                }

                // jumping off a moving platform closed
                if (jumpedOffPlatform) {
                    if (isGrounded && jumpTimer > jumpCooldownTime/5)
                        jumpedOffPlatform = false;
                    else
                        body.velocity += new Vector2 (jumpedOffPlatformVel.x, 0);
                }
                //Set animator parameter
                anim.SetBool("falling", body.velocity.y < 0 && !isGrounded);
                anim.SetBool("run", horizontalInput != 0 && isGrounded);

            }

            
            
        }else
        {
            body.velocity = Vector2.zero;
            anim.SetBool("run", false);
            anim.SetBool("falling", false);

        }
    }
    void StopDash()
    {
        body.gravityScale = gravScale;
        body.velocity = Vector2.zero;
        dashing = false;
    }
    void Die()
    {
        StartCoroutine(Dying());

        
    }
    IEnumerator Dying()
    {
        audioSources[Random.Range(4,6)].Play();
        GetComponent<SpriteRenderer>().color = Color.red;
        playing = false;
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().color = Color.white;

        playing = true;

        if (checkPointreached)
        {
            checkPointPos = checkPoint.transform.position;
            if (riding)
            {
                transform.parent.position = new Vector2 (checkPointPos.x, 0);
                transform.position = new Vector2 (-1+transform.parent.position.x, 5+transform.parent.position.y);;

            }
            else
                transform.position = checkPointPos;
        }
        else
        {
            if (riding)
            {
                transform.parent.position = new Vector2 (spawnPoint.x, 0);
                transform.position = new Vector2 (-1+transform.parent.position.x, 5+transform.parent.position.y);
            }
            else
                transform.position = spawnPoint;
        }
    }
}