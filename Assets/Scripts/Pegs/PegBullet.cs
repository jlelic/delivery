using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class PegBullet : MonoBehaviour
{
    [SerializeField] private FloatVariable moveSpeed;
    [SerializeField] private FloatVariable gravityScale;
    [SerializeField] private IntegerVariable bulletCount;
    [SerializeField] private FloatVariable bulletScale;

    [SerializeField] private GameObject trajectoryDotsParent;

    private Rigidbody2D rb;
    private bool isMoving = false;

    private Transform[] trajectoryDots;
    private Vector2 direction;

    private Queue<float> velocities = new Queue<float>();
    private float rescueForce = 2f;
    private float rescueVelocityLimit = 0.2f;
    private int rescueSampleCount = 10;

    public void OnBulletScaleChange()
    {
        transform.localScale = new Vector3(bulletScale.value, bulletScale.value, 1f);
    }

    public void OnBulletGravityChange()
    {
        rb.gravityScale = gravityScale.value;
    }

    private void Awake()
    {
        transform.localScale = new Vector3(bulletScale.value, bulletScale.value, 1f);
        rb = GetComponent<Rigidbody2D>();

        if (trajectoryDotsParent != null)
        {
            trajectoryDots = trajectoryDotsParent.GetComponentsInChildren<Transform>();
        }
    }

    private void FixedUpdate()
    {
        // check if the bullet was moving last x fixedUpdates and help it if not
        this.velocities.Enqueue(this.rb.velocity.magnitude);
        if (this.velocities.Count >= rescueSampleCount)
        {
            this.velocities.Dequeue();
        }
        float[] lastVelocities = velocities.ToArray();
        float total = 0f;
        foreach (var v in lastVelocities)
        {
            total += v;
        }
        if (isMoving && total <= rescueVelocityLimit)
        {
            this.rb.velocity = rescueForce * UnityEngine.Random.insideUnitCircle;
        }
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;
        DrawTrajectory();

        if (GameManager.Instance.inputEnabled && Input.GetMouseButtonDown(0) && !isMoving)
        {
            rb.gravityScale = gravityScale.value;
            rb.AddForce(direction.normalized * moveSpeed.value, ForceMode2D.Impulse);
            isMoving = true;
            bulletCount.SetValue(Mathf.Max(0, bulletCount.value - 1));
        }
    }

    private void DrawTrajectory()
    {
        if (isMoving || trajectoryDots.Length == 0)
        {
            trajectoryDotsParent.SetActive(false);
            return;
        }
        trajectoryDotsParent.SetActive(true);

        float t = 0.05f;
        foreach (Transform dot in trajectoryDots)
        {
            Vector2 pathPosition = CalculateParabolicPosition(transform.position, direction, moveSpeed.value, t);
            dot.position = new Vector3(pathPosition.x, pathPosition.y, transform.position.z - 0.5f);
            t += 0.05f;
        }
        CheckTrajectoryCollision();
    }

    private Vector2 CalculateParabolicPosition(Vector2 start, Vector2 direction, float force, float time)
    {
        Vector2 result = start;
        result += direction * force * time;
        result.y += 0.5f * Physics2D.gravity.y * time * time;
        return result;
    }

    private void CheckTrajectoryCollision()
    {
        bool hadCollision = false;
        for (int i = 0; i < trajectoryDots.Length - 1; i++)
        {
            Transform first = trajectoryDots[i];
            Transform second = trajectoryDots[i + 1];

            float distance = Vector2.Distance(first.position, second.position);
            float angle = 90f + Mathf.Atan2(second.position.y - first.position.y, second.position.x - first.position.x) * Mathf.Rad2Deg;

            Collider2D[] colliders = Physics2D.OverlapCapsuleAll(
                first.position + ((second.position - first.position) / 2),
                new Vector2(0.2f, distance),
                CapsuleDirection2D.Vertical,
                angle
            );

            if (colliders.Length > 0)
            {
                hadCollision = true;
            }

            if (!hadCollision)
            {
                second.gameObject.SetActive(true);
            }
            else
            {
                second.gameObject.SetActive(false);
            }
        }
    }
}