using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed;
    public float slideSpeed;
    
    public float jumpLength;
    public float jumpHeight;
    public float slideLength;

    private Rigidbody rb;
    private Animator animator;
    private BoxCollider boxCollider;
    
    private int currentRoad;
    private Vector3 movementVector;
    private bool jumping;
    private float jumpStart;
    private bool sliding;
    private float slideStart;
    private Vector3 boxColliderSize;
    private bool isSwiping;
    private Vector2 startTouch;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        boxColliderSize = boxCollider.size;
        animator.Play("runStart");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            changeRoad(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            changeRoad(-1);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            slide();
        }

        if (Input.touchCount == 1)
        {
            if (isSwiping)
            {
                Vector2 diff = Input.GetTouch(0).position - startTouch;
                diff = new Vector2(diff.x / Screen.width, diff.y / Screen.height);

                if (diff.magnitude > 0.01f)
                {
                    if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                    {
                        if (diff.y < 0)
                        {
                            slide();
                        }
                        else
                        {
                            jump();
                        }
                    }
                    else
                    {
                        if (diff.x < 0)
                        {
                            changeRoad(-1);
                        }
                        else
                        {
                            changeRoad(1);
                        }
                    }
                    isSwiping = false;
                }
            }
            
            else
            {
                if (Input.GetTouch(0).position.x < Screen.width / 3)
                {
                    changeRoad(-1);
                }
                else
                {
                    changeRoad(1);
                }

            }

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouch = Input.GetTouch(0).position;
                isSwiping = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isSwiping = false;
            }
        }

        if (jumping)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLength;
            if (ratio >= 1f)
            {
                jumping = false;
                animator.SetBool("Jumping", false);
            }
            else
            {
                movementVector.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        }
        else
        {
            movementVector.y = Mathf.MoveTowards(movementVector.y, 0, 5 * Time.deltaTime);
        }

        if (sliding)
        {
            float ratio = (transform.position.z - slideStart) / slideLength;
            if (ratio >= 1f)
            {
                sliding = false;
                animator.SetBool("Sliding", false);
                boxCollider.size = boxColliderSize;
            }
        }

        Vector3 target = new Vector3(movementVector.x, movementVector.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, slideSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.forward * movementSpeed;
    }

    private void changeRoad(int index)
    {
        int targetRoad = currentRoad + index;

        if (targetRoad < 0 || targetRoad > 2)
        {
            return;
        }

        currentRoad = targetRoad;
        movementVector = new Vector3(currentRoad - 1, 0, 0);
    }

    private void jump()
    {
        if (!jumping)
        {
            jumpStart = transform.position.z;
            animator.SetFloat("JumpSpeed", movementSpeed / jumpLength);
            animator.SetBool("Jumping", true);
            jumping = true;
        }
    }

    private void slide()
    {
        if (!jumping && !sliding)
        {
            slideStart = transform.position.z;
            animator.SetFloat("JumpSpeed", movementSpeed / jumpLength);
            animator.SetBool("Sliding", true);
            Vector3 newSize = boxCollider.size;
            newSize.y = newSize.y / 2;
            boxCollider.size = newSize;
            sliding = true;
        }
    }
}
