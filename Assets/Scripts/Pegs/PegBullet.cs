using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PegBullet : MonoBehaviour
{
    [SerializeField] private FloatVariable moveSpeed;
    [SerializeField] private FloatVariable gravityScale;

    [SerializeField] private IntegerVariable bulletCount;

    private Rigidbody2D rb;
    private bool isMoving = false;
    [SerializeField] private GameObject trajectoryDotsParent;
    private Transform[] trajectoryDots;

    private Vector2 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (trajectoryDotsParent != null)
        {
            trajectoryDots = trajectoryDotsParent.GetComponentsInChildren<Transform>();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && !isMoving)
        {
            rb.gravityScale = gravityScale.value;
            rb.AddForce(direction.normalized * moveSpeed.value, ForceMode2D.Impulse);
            isMoving = true;
            bulletCount.SetValue(Mathf.Max(0, bulletCount.value - 1));

        }

    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;
        DrawTrajectory();
    }

    private void DrawTrajectory()
    {
        if (isMoving || trajectoryDots.Length == 0)
        {
            trajectoryDotsParent.SetActive(false);
            return;
        }
        trajectoryDotsParent.SetActive(true);

        float t = 0f;
        foreach (Transform dot in trajectoryDots)
        {
            Vector2 pathPosition = CalculateParabolicPosition(transform.position, direction, moveSpeed.value, t);
            dot.position = new Vector3(pathPosition.x, pathPosition.y, -1f);
            t += 0.05f;
        }

    }

    private Vector2 CalculateParabolicPosition(Vector2 start, Vector2 direction, float force, float time)
    {
        Vector2 result = start;
        result += direction * force * time;
        result.y += 0.5f * Physics2D.gravity.y * time * time;
        return result;
    }
}