using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    //gun var 
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform FiringPoint;
    [Range(0.1f, 1f)]
    [SerializeField] private float fireRate = 0.5f;

    private Rigidbody2D rb;
    private float mx;
    private float my;

    private float fireTimer;
    private Vector2 mousePos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        mx = Input.GetAxisRaw("Horizontal");
        my = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;

        }
        else
        {
            fireTimer -= Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(mx, my).normalized * speed; 
    }

    private void Shoot()
    {
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
