using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] private int jumpHeight;
    [SerializeField] private int movementSpeed;
    [SerializeField] private float jumpTime;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRange;
    [SerializeField] private LayerMask groundLayer;

    private float moveInput;
    private Animator animator;
    private Rigidbody2D rb;
    private bool grounded, moving, lookingLeft,inverted;
    private Color backGroundColor,invertedBackgroundColor;
    private float jumpTimer,groundedTimer;
    private int jumpTimes;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Shader.SetGlobalFloat("_InvertColors", 0);
        backGroundColor=Camera.main.backgroundColor;
        invertedBackgroundColor = new Color(1-backGroundColor.r, 1-backGroundColor.g, 1-backGroundColor.b, backGroundColor.a);
    }

    // Update is called once per frame
    void Update()
    {
        groundedTimer -= Time.deltaTime;
        jumpTimer -= Time.deltaTime;

        if (grounded && !Physics2D.OverlapCircle(groundCheck.position, groundCheckRange, groundLayer))
        {
            groundedTimer = 0.1f;
        }
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRange, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpTimer = 0.1f;
        }
        
        if (jumpTimer > 0 && (groundedTimer>0||grounded))
        {
            rb.velocity=new Vector2(rb.velocity.x,0);
            rb.AddForce(new Vector2(0, jumpHeight));
            jumpTimer = 0;
            groundedTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.E)&&!GameManager.Instance.levelManager.nameUIopen)
        {
            if (!inverted)
            {
                Shader.SetGlobalFloat("_InvertColors", 1);
                Camera.main.backgroundColor = invertedBackgroundColor;
                inverted = true;
                GameManager.Instance.levelManager.SwitchInversion(inverted);
            }
            else
            {
                Shader.SetGlobalFloat("_InvertColors", 0);
                Camera.main.backgroundColor = backGroundColor;
                inverted = false;
                GameManager.Instance.levelManager.SwitchInversion(inverted);
            }
        }
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        if (moveInput == 0)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }

        if (rb.velocity.x<0)
        {
            animator.SetFloat("Xvelocity", -rb.velocity.x);
        }
        else
        {
            animator.SetFloat("Xvelocity", rb.velocity.x);
        }
        
        animator.SetBool("Jump", !grounded);
        animator.SetBool("Grounded",grounded);
        animator.SetBool("Moving", moving);

        rb.velocity = new Vector2(moveInput*movementSpeed, rb.velocity.y);

        if ((rb.velocity.x<0 &&!lookingLeft) || (rb.velocity.x > 0 && lookingLeft))
        {
            lookingLeft = !lookingLeft;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}
