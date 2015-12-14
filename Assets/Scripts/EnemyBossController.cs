﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBossController : MonoBehaviour {
    bool movingRight = true;
    List<GameObject> miniBaddies;
    float timeTillNextAttack;

    public float attackRate = 1;
    public int hp = 6;
    public float movementSpeed;
    public float travelWidth;

    void Awake()
    {
        miniBaddies = new List<GameObject>();
        timeTillNextAttack = Time.time + attackRate;
    }

	void Start () {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.transform.parent == transform)
            {
                miniBaddies.Add(enemy);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move();

        if (Time.time >= timeTillNextAttack)
            Attack();
    }

    void LateUpdate()
    {
        if (hp <= 0)
            Destroy(gameObject);
    }

    void Move()
    {
        if (movingRight && transform.position.x <= travelWidth)
            transform.Translate(Vector2.right * Time.deltaTime * movementSpeed);
        if (transform.position.x >= travelWidth)
            movingRight = false;
        if (!movingRight && transform.position.x >= -travelWidth)
            transform.Translate(Vector2.left * Time.deltaTime * movementSpeed);
        if (transform.position.x <= -travelWidth)
            movingRight = true;
    }

    void Attack()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NanoBot")
        {
            NanoBotController nanoCon = other.GetComponent<NanoBotController>();
            if (!nanoCon.CheckIfAttacking() && !nanoCon.CheckIfSwarming())
                hp--;

            other.GetComponent<NanoBotController>().Attack(gameObject);
        }
    }
}