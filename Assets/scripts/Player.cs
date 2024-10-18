using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    // Gun variables
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform FiringPoint;
    [Range(0.1f, 1f)]
    [SerializeField] private float fireRate = 0.5f;

    private Rigidbody2D rb;
    private float mx;
    private float my;

    private float fireTimer;

    // Joystick references (add these to your UI)
    [SerializeField] private Joystick movementJoystick;  // Joystick for movement
    [SerializeField] private Joystick shootingJoystick;  // Joystick for shooting

    private bool isMobile;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Detect the platform at the start
#if UNITY_ANDROID || UNITY_IOS
            isMobile = true;   // Mobile platform
#else
        isMobile = false;  // Desktop platform (PC)
#endif
    }

    private void Update()
    {
        // Handle movement input based on platform
        if (isMobile)
        {
            // Mobile joystick input for movement
            mx = movementJoystick.Horizontal;
            my = movementJoystick.Vertical;
        }
        else
        {
            // PC keyboard input
            mx = Input.GetAxisRaw("Horizontal");
            my = Input.GetAxisRaw("Vertical");
        }

        // Mobile aiming with shooting joystick
        if (isMobile)
        {
            if (shootingJoystick.Horizontal != 0 || shootingJoystick.Vertical != 0)
            {
                AimAndShoot(shootingJoystick.Horizontal, shootingJoystick.Vertical);
            }
        }
        else
        {
            // PC aiming with mouse
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90f;
            transform.localRotation = Quaternion.Euler(0, 0, angle);

            if (Input.GetMouseButton(0) && fireTimer <= 0f)
            {
                Shoot();
                fireTimer = fireRate;
            }
        }

        // Fire rate timer
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        // Apply movement to Rigidbody
        rb.velocity = new Vector2(mx, my).normalized * speed;
    }

    private void AimAndShoot(float aimX, float aimY)
    {
        // Calculate the aiming direction from the shooting joystick
        Vector2 aimDirection = new Vector2(aimX, aimY).normalized;

        // Rotate the player to aim
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        // Shoot when joystick is aimed and fire timer is ready
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    private void Shoot()
    {
        // Instantiate bullet at firing point
        Instantiate(BulletPrefab, FiringPoint.position, FiringPoint.rotation);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            LevelManager.manager.GameOver();
            Destroy(gameObject);
        }
    }
}
