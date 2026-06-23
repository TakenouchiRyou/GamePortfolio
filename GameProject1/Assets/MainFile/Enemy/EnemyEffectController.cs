using UnityEngine;
using UnityEngine.AI;

public class EnemyEffectController : MonoBehaviour
{
    [Header("Effects")]
    public ParticleSystem rightEffect;
    public ParticleSystem leftEffect;
    public ParticleSystem desserteffect;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent == null) return;

        // “G‚ª“®‚¢‚Ä‚¢‚é‚©”»’è
        if (agent.velocity.magnitude > 0.1f)
        {
            PlayEffect();
        }
        else
        {
            StopEffect();
        }
    }

    void PlayEffect()
    {
        if (rightEffect != null && !rightEffect.isPlaying)
            rightEffect.Play();

        if (leftEffect != null && !leftEffect.isPlaying)
            leftEffect.Play();

        if (desserteffect != null && !desserteffect.isPlaying)
            desserteffect.Play();
    }

    void StopEffect()
    {
        if (rightEffect != null && rightEffect.isPlaying)
            rightEffect.Stop();
        if (leftEffect != null && leftEffect.isPlaying)
            leftEffect.Stop();
        if (desserteffect != null && !desserteffect.isPlaying)  
            desserteffect.Stop();
    }
}