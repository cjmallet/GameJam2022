using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] private int jumpHeight;
    [SerializeField] private int movementSpeed;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRange;
    [SerializeField] private LayerMask groundLayer;

    private float moveInput;
    private Animator animator;
    private Rigidbody2D rb;
    private bool grounded, moving, lookingLeft,inverted;
    private Color backGroundColor,invertedBackgroundColor;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Shader.SetGlobalFloat("_InvertColors", 0);
        backGroundColor=Camera.main.backgroundColor;
        invertedBackgroundColor = new Color(1-backGroundColor.r, 1-backGroundColor.g, 1-backGroundColor.b, backGroundColor.a);
        Debug.Log(invertedBackgroundColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&grounded)
        {
            rb.AddForce(new Vector2(0,jumpHeight));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!inverted)
            {
                Shader.SetGlobalFloat("_InvertColors", 1);
                Camera.main.backgroundColor = invertedBackgroundColor;
                inverted = true;
            }
            else
            {
                Shader.SetGlobalFloat("_InvertColors", 0);
                Camera.main.backgroundColor = backGroundColor;
                inverted = false;
            }
        }
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRange, groundLayer);

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
