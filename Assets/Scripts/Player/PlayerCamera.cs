using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector2 offsetBase = new Vector2(5, 0);
    [SerializeField] private Vector2 offset = new Vector2(5, 0);
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private Vector2 velocity = Vector2.zero;
    [SerializeField] private PlayerMovement playerMovement;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (playerMovement.isFacingRight)
        {
            offset = offsetBase;
        }
        else
        {
            offset = -offsetBase;
        }
        Vector2 targetPosition = new Vector2(player.position.x, player.position.y) + offset;
        transform.position = Vector2.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
