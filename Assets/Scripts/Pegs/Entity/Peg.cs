using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Peg : MonoBehaviour
{
    [SerializeField]
    Color particleColor = Color.white;

    public abstract void OnPlayerCollision();

    protected void TriggerParticleEfect()
    {
        var particleSystemObject = Instantiate(GameManager.Instance.pegGameManager.particleTemplate, transform.position, Quaternion.identity, transform.parent);
        var particleSystem = particleSystemObject.GetComponent<ParticleSystem>();
        var col = particleSystem.colorOverLifetime;
        col.color = new ParticleSystem.MinMaxGradient(particleColor, new Color(particleColor.r, particleColor.g, particleColor.b, 0));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnPlayerCollision();
        }
    }
}
