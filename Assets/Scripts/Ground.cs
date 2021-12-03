﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;

    public float groundHeight;
    public float groundRight;
    public float screenRight;
    BoxCollider2D collider;

    bool didGenerateGround = false;

    public GameObject minion;
    
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (!player.isDead)
        {

            collider = GetComponent<BoxCollider2D>();
        }
        groundHeight = transform.position.y + (collider.size.y / 2);
        screenRight = Camera.main.transform.position.x * 2;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!player.isDead)
        {
            Vector2 pos = transform.position;
            pos.x -= player.velocity.x * Time.fixedDeltaTime;


            groundRight = transform.position.x + (collider.size.x / 2);
            if (groundRight < -40)
            {
                Destroy(gameObject);
                //return;
            }

            if (!didGenerateGround)
            {
                if (groundRight < screenRight)
                {
                    didGenerateGround = true;
                    generateGround();
                }
            }

            transform.position = pos;
        }
    }

    void generateGround()
    {
        GameObject go = Instantiate(gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        //Maximum high
        float h1 = player.jumpVelocity * player.maxHoldJumpTime / 2;
        float t = player.jumpVelocity / -player.gravity;
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
        float maxJumpHeight = h1 + h2;
        float maxY = maxJumpHeight * 0.3f;
        maxY += groundHeight;
        float minY = 5;
        float actualY = Random.Range(minY, maxY); //- goCollider.size.y / 2;

        pos.y = actualY - goCollider.size.y / 2;
        if (pos.y > 1f)
            pos.y = 1.7f;

        float t1 = t + player.maxHoldJumpTime;
        float t2 = Mathf.Sqrt((4.0f * (maxY - actualY)) / -player.gravity);
        float totalTime = t1 + t2;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.7f;
        maxX += groundRight;
        float minX = screenRight + 5;
        float actualX = Random.Range(minX, maxX);

        pos.x = actualX + goCollider.size.x / 2;
        go.transform.position = pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);


        float minionNum = Random.Range(0, 3);
        for (int i=0; i< minionNum; i++)
        {
            GameObject goMinion = Instantiate(minion);
            float y = goGround.groundHeight;
            float halfWidth = goCollider.size.x / 2 - 1;
            float left = go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            float x = Random.Range(left, right);
            Vector2 minionPos = new Vector2(x, y);
            goMinion.transform.position = minionPos;
        }
    }
}
