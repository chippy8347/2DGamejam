using UnityEngine;

public class CameraRunnerScript : MonoBehaviour 
{
    // MAKE SURE TO SET THE PLAYER OBJECT IN THE EDITOR
    [SerializeField] private Transform player;





    [SerializeField] private GameObject dashCool;
    private Vector3 offset = new Vector3 (5, 0, -5);
    private float cameraSpeed = 0.5f;
    private SpriteRenderer dashCoolComp;
    private Transform dashCoolNum;

    void Start()
    {
        dashCoolComp = dashCool.GetComponent<SpriteRenderer>();
        dashCoolNum = dashCool.transform.GetChild(0);
    }
    void Update () 
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed);
        transform.position = smoothedPosition;

        if(PlayerMovement.canDash) 
        {
            dashCoolComp.color = Color.white;
            dashCoolNum.gameObject.SetActive(false);
        } else {
            dashCoolComp.color = Color.gray;
            dashCoolNum.gameObject.SetActive(true);
            float rounded = Mathf.Round(10*(PlayerMovement.dashcooldown - PlayerMovement.dashTimer));
            dashCoolNum.gameObject.GetComponent<TextMesh>().text = (rounded/10).ToString();
        }
    }
}