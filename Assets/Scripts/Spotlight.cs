using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Spotlight : MonoBehaviour
{
    // SET PLAYER IN EDITOR!!!
    [SerializeField] private GameObject player;


    private Light2D lightComp;
    void Start()
    {
        lightComp = GetComponent<Light2D>();
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 3.0f)
            lightComp.enabled = true;
        else
            lightComp.enabled = false;
    }
}
