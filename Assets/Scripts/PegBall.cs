using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PegBall : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float gravityScale = 0.1f;

    private Rigidbody2D rb;
    private bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && !isMoving)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 direction = mousePos - objectPos;

            rb.gravityScale = gravityScale;
            rb.AddForce(direction.normalized * moveSpeed, ForceMode2D.Impulse);
            isMoving = true;
        }
    }
}