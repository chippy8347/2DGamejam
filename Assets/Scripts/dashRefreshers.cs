using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class dashRefreshers : MonoBehaviour
{
    // MAKE SURE TO SET THE PLAYER OBJECT IN THE EDITOR
    [SerializeField] private GameObject player;



    private bool used;
    private SpriteRenderer sprt;
    private Light2D dashLight;

    void Start()
    {
        dashLight = GetComponent<Light2D>();
        sprt = GetComponent<SpriteRenderer>();
        used = false;
    }
    void Update()
    {
        if(Vector2.Distance(player.transform.position, transform.position) < 3.0f && !used && !PlayerMovement.canDash)
        {
            PlayerMovement.dashTimer = PlayerMovement.dashcooldown;
            used = true;
            sprt.color = Color.black;
            dashLight.intensity = 0.25f;
            StartCoroutine(refreshRefresher());
        }
    }
    IEnumerator refreshRefresher()
    {
        yield return new WaitForSeconds(2);
        used = false;
        sprt.color = Color.blue;
        dashLight.intensity = 0.75f;
    }
}
