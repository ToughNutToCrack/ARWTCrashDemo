using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem tntParticle;
    public ParticleSystem trunkParticle;
    public AudioSource sfx;
    public AudioClip explosion, trunk;

    public void PlayParticle(int n)
    {
        switch (n)
        {
            case 0:
                tntParticle.Play();
                sfx.PlayOneShot(explosion);
                break;
            case 1:
                trunkParticle.Play();
                sfx.PlayOneShot(trunk);
                break;
        }
    }
}
