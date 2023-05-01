public class PegPlatform : Peg
{
    public override void OnPlayerCollision()
    {
        TriggerParticleEfect();
        MusicMixer.instance.PlayNextNote(0.3f);
    }
}
