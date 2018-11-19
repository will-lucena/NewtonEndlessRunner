using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float movementSpeed;
    public float slideSpeed;
    public int lifes;
    public float minSpeed;
    public float maxSpeed;
    public float invencibleTime;
    
    public float jumpLength;
    public float jumpHeight;
    public float slideLength;
    public GameObject model;

    private Rigidbody rb;
    private Animator animator;
    private BoxCollider boxCollider;
    
    private int currentRoad = 1;
    private Vector3 movementVector;
    private bool jumping;
    private float jumpStart;
    private bool sliding;
    private float slideStart;
    private Vector3 boxColliderSize;
    private bool isSwiping;
    private Vector2 startTouch;
    private int currentLife;
    private bool invencible;
    private int blinkingValue;
    [HideInInspector] public int coinAmount;
    [HideInInspector] public float score;
    private bool canMove;
    private bool hasShield;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        GameController.instance.increaseSpeed += updateSpeed;
        GameController.instance.unsubscribeMethods += reset;
        GameController.instance.enableShield += activeShield;
    }

    private void activeShield()
    {
        hasShield = true;
    }

    private void reset()
    {
        GameController.instance.increaseSpeed -= updateSpeed;
        GameController.instance.enableShield -= activeShield;
        GameController.instance.unsubscribeMethods -= reset;
    }

    private void Start()
    {
        boxColliderSize = boxCollider.size;
        currentLife = lifes;
        blinkingValue = Shader.PropertyToID("_BlinkingValue");
        coinAmount = 0;
        score = 0;
        Invoke("run", 3);
    }

    private void run()
    {
        animator.Play("runStart");
        movementSpeed = minSpeed;
        canMove = true;
    }

    private void Update()
    {
        if (!canMove)
        {
            return;
        }

        score += Time.deltaTime * movementSpeed;

        GameController.instance.updateUI(UIComponent.Score, score);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            changeRoad(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
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

        /* Mobile input
         * 
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
                else if (Input.GetTouch(0).position.x > (Screen.width / 3) * 2)
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
        /**/

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

    private void OnTriggerEnter(Collider other)
    {
        if (invencible)
        {
            return;
        }
        if (other.CompareTag("Obstacle"))
        {
            if (!hasShield)
            {
                currentLife--;
                animator.SetTrigger("Hit");
                movementSpeed = 0;
                canMove = false;
                GameController.instance.updateUI(UIComponent.Heart, currentLife);
                if (currentLife == 0)
                {
                    movementSpeed = 0;
                    animator.SetBool("Dead", true);
                    GameController.instance.increaseSpeed -= updateSpeed;
                    GameController.instance.endGame(coinAmount, (int)score);
                }
                else
                {
                    Invoke("move", 0.75f);
                    StartCoroutine(invencibleRoutine(invencibleTime));
                }
            }
            else
            {
                hasShield = false;
                transform.position = GameController.instance.useShield(other.GetComponent<Obstacle>());
            }
        }
        else if (other.CompareTag("Coin"))
        {
            coinAmount++;
            GameController.instance.updateUI(UIComponent.Coin, coinAmount);
            other.gameObject.SetActive(false);
        }
    }

    private void move()
    {
        canMove = true;
    }

    private IEnumerator invencibleRoutine(float time)
    {
        invencible = true;
        float timer = 0;
        float currentBlink = 1f;
        float lastBlink = 0;
        float blinkPeriod = 0.1f;
        bool enabled = false;
        yield return new WaitForSeconds(1f);
        movementSpeed = minSpeed;
        while(timer < time && invencible)
        {
            model.SetActive(enabled);
            //Shader.SetGlobalFloat(blinkingValue, currentBlink);
            yield return null;
            timer += Time.deltaTime;
            lastBlink += Time.deltaTime;
            if (blinkPeriod < lastBlink)
            {
                lastBlink = 0;
                currentBlink = 1f - currentBlink;
                enabled = !enabled;
            }
        }
        //Shader.SetGlobalFloat(blinkingValue, 0);
        invencible = false;
        model.SetActive(true);
    }

    private void updateSpeed(float speedModifier)
    {
        movementSpeed *= speedModifier;
        Debug.Log(movementSpeed);
        if (movementSpeed > maxSpeed)
        {
            movementSpeed = maxSpeed;
        }
    }
}
