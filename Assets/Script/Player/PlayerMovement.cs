using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private float originalSpeed;

    private Vector3 direction;

    public float jumpForce = 5f;
    public float gravity = -20f;
    public float jumpHeight = 2;

    public float increTime = 0.5f;

    private bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float time;
    public float interval = 5f;

    private Vector3 velocity;
    private CharacterController controller;

    private int desiredLane = 1;
    public float laneDistance = 4;


    public Animator animator;

    private bool isSliding = false;
    public float slideDuration = 1.5f;

    public static bool offShield = false;
    public static bool isShield = false;
    private Coroutine shieldCoroutine;
    public float shieldDuration = 5f;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        originalSpeed = speed;
        isShield = false;
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
        {
            return;
        }
        controller.Move(direction * Time.deltaTime);
    }


    void Update()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
        {
            return;
        }
        animator.SetBool("isGameStarted", true);

        if (PlayerManager.isGameStarted && !isShield)
        {
            time += Time.deltaTime;
            if (time >= interval)
            {
                speed += increTime;
                originalSpeed += increTime;
                time = 0;
            }
        }

        direction.z = speed;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }
            

        if (isGrounded)
        {
            if (SwipeManager.swipeUp )
            {
                jump();
            }
            if (SwipeManager.swipeDown && !isSliding)
            {
                StartCoroutine(Slide());
            }
        }
        else
        {

            velocity.y += gravity * Time.deltaTime;
            if (SwipeManager.swipeDown && !isSliding)
            {
                StartCoroutine(Slide());
                velocity.y = -10;
            }
        }
        //
        controller.Move(velocity * Time.deltaTime);

        //KeyCode
        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane >= 3)
            {
                desiredLane = 2;
            }
        }
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane <= -1)
            {
                desiredLane = 0;
            }
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }
        //transform.position = targetPosition;

        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 30 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.magnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }
        controller.Move(direction * Time.deltaTime);

    }
   
    private void jump()
    {

        StopCoroutine(Slide());
        animator.SetBool("isSliding", false);
        animator.SetTrigger("jump");
        controller.center = Vector3.zero;
        controller.height = 2;
        isSliding = false;

        velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shield"))
        {
            ActivateShield();
        }
        else if (other.gameObject.CompareTag("Obstacle") && !isShield)
        {
            PlayerManager.gameOver = true;
            AudioManager.instance.PlaySFX("Game Over Effect");
            Time.timeScale = 0f;
        }
    }

    private void ActivateShield()
    {
        if (isShield && shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }

        isShield = true;
        speed += 10f;
        shieldCoroutine = StartCoroutine(ShieldEffect());
    }

    private IEnumerator ShieldEffect()
    {
        yield return new WaitForSeconds(shieldDuration);
        StartCoroutine(DecreaseSpeed());
    }

    private IEnumerator DecreaseSpeed()
    {
        float initialSpeed = speed;
        float targetSpeed = originalSpeed;
        float duration = 5f; // Duration over which speed decreases

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (elapsed >= duration - 2)
            {
                offShield = true;
            }
            speed = Mathf.Lerp(initialSpeed, targetSpeed, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        speed = targetSpeed;
        isShield = false;
        offShield = false;
    }


    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        yield return new WaitForSeconds(0.25f / Time.timeScale);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds((slideDuration - 0.25f) / Time.timeScale);

        animator.SetBool("isSliding", false);

        controller.center = Vector3.zero;
        controller.height = 2;

        isSliding = false;
    }
}
