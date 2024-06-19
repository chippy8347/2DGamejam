using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random=UnityEngine.Random;

public class levelStar : MonoBehaviour
{
    // MAKE SURE TO SET THE PLAYER OBJECT IN THE EDITOR
    [SerializeField] private GameObject player;
    [SerializeField] private string nextLevel;

    [SerializeField] private GameObject starchild;
    private Light2D lightComp;
    private float lightGrowSpeed = 0.07f;
    private float startingHeight;
    private float amplitude = 0.5f;
    private float rotateSpeed = 50.0f;
    private float elapsedTime;
    private bool collected = false;
    private bool growing = false;

    void Start()
    {
        lightComp = GetComponent<Light2D>();
        startingHeight = transform.position.y;
        elapsedTime = 0;
    }

    void Update()
    {
        if (!collected) 
        {
            starchild.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            float newY = startingHeight + amplitude * Mathf.Sin(elapsedTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            if (Vector3.Distance(player.transform.position, transform.position) < 1.5)
            {
                player.GetComponent<Rigidbody2D>().gravityScale = 0f;
                EndGame();
                collected = true;
                growing = true;
            }
        }else if (growing)
        {
            if (lightComp.pointLightOuterRadius < 40)
                lightComp.pointLightOuterRadius += lightGrowSpeed;
            else 
                growing = false;
                player.SendMessage("UpdateLevel", nextLevel);
        }
    }
    void EndGame()
    {
        starchild.transform.rotation = quaternion.Euler(0,0,0);
        PlayerMovement.playing = false;

    }
}
