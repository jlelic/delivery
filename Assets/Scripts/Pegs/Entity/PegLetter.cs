using UnityEngine;

public class PegLetter : Peg
{
    [SerializeField] private LETTER letter;

    public override void OnPlayerCollision()
    {
        PegManager.Instance.AddLetter(letter);
        MusicMixer.instance.PlayRandomNote();
        TriggerParticleEfect();
        Destroy(gameObject);
    }
}
