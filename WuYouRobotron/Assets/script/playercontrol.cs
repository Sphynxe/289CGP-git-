using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    private float nextFireTime;

    private Animator animator;

    [SerializeField] private AudioClip shootSound;
    private AudioSource audioSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // Movement restricted to Arrow Keys
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1;
        else if (Input.GetKey(KeyCode.RightArrow)) moveX = 1;

        if (Input.GetKey(KeyCode.UpArrow)) moveY = 1;
        else if (Input.GetKey(KeyCode.DownArrow)) moveY = -1;

        moveInput = new Vector2(moveX, moveY).normalized;

        UpdateAnimation(moveX, moveY);
        HandleShooting();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void UpdateAnimation(float x, float y)
    {
        float inputSpeed = moveInput.sqrMagnitude;
        animator.SetFloat("Speed", inputSpeed);

        if (inputSpeed > 0.01f)
        {
            animator.SetFloat("horizontal", x);
            animator.SetFloat("vertical", y);
        }
    }

    void HandleShooting()
    {
        if (Time.time >= nextFireTime)
        {
            Vector2 shootDir = Vector2.zero;

            // Check for diagonal inputs by checking multiple keys
            if (Input.GetKey(KeyCode.W)) shootDir.y += 1;
            if (Input.GetKey(KeyCode.S)) shootDir.y -= 1;
            if (Input.GetKey(KeyCode.A)) shootDir.x -= 1;
            if (Input.GetKey(KeyCode.D)) shootDir.x += 1;

            if (shootDir != Vector2.zero)
            {
                Shoot(shootDir.normalized);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot(Vector2 direction)
    {
        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Create rotation based on the calculated angle
        Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

        // Instantiate with the correct rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = direction * 12f;
        }

        Destroy(bullet, 2f);
    }
}