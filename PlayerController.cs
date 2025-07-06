using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 20f;
    public float jumpForce = 20f;

    [Header("Dash Settings")]
    public float dashForce = 30f;
    public float dashTime = 0.2f;
    public float dashCooldown = 3f;

    private Rigidbody2D rb;
    private bool isDashing = false;
    private float dashTimer;
    private float dashCooldownTimer;

    [Header("Cooldown UI")]
    public Image cooldownBarFill; // Drag fill image from UI Slider/Bar

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dashSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashCooldownTimer = 0f;
    }

    void Update()
    {
        if (!isDashing)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f && moveInput != 0)
            {
                isDashing = true;
                dashTimer = dashTime;
                dashCooldownTimer = dashCooldown;

                if (audioSource && dashSound)
                {
                    audioSource.PlayOneShot(dashSound);
                }

                rb.linearVelocity = new Vector2(moveInput * dashForce, 0f);
            }
        }
        else
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Update UI Cooldown Bar
        if (cooldownBarFill)
        {
            cooldownBarFill.fillAmount = 1f - (dashCooldownTimer / dashCooldown);
        }
    }
}
