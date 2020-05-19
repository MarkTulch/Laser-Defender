using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config params
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] int health = 500;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    [Header("Audio")]
    [SerializeField] AudioClip deathSFX;
    [SerializeField] GameObject explosionVFX;

    Coroutine firingCoroutine;

    // Camera range bounds
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(
                laserPrefab,
                transform.position,
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void SetUpMoveBoundaries()
    {
        InitCameraRange();
    }

    // Sets up the ranges for [x,y] camera
    private void InitCameraRange()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(.02f, 0, 0)).x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(.95f, 0, 0)).x;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, .05f, 0)).y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).y;
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax) ;
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void OnTriggerEnter2D(Collider2D incomingObject)
    {
        DamageDealer damageDealer = incomingObject.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) {return;}
        ProcessHit(damageDealer);
    }

    public int GetHealth()
    {
        return health > 0 ? health : 0;
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(deathSFX, new Vector3(transform.position.x, transform.position.y));
            GameObject deathExplosion = Instantiate(
                explosionVFX,
                transform.position,
                Quaternion.identity) as GameObject;
            Destroy(deathExplosion, 1f);
            Destroy(gameObject);
            FindObjectOfType<Level>().LoadGameOver();
        }
    }
}
