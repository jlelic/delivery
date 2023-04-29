using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PegBullet : MonoBehaviour
{
    [SerializeField] private FloatVariable moveSpeed;
    [SerializeField] private FloatVariable gravityScale;

    [SerializeField] private IntegerVariable bulletCount;

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

            rb.gravityScale = gravityScale.value;
            rb.AddForce(direction.normalized * moveSpeed.value, ForceMode2D.Impulse);
            isMoving = true;
            bulletCount.SetValue(Mathf.Max(0, bulletCount.value - 1));
        }
    }
}