using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    public LayerMask whatIsTarget;

    private LivingEntity targetEntity;
    private NavMeshAgent navMeshAgent;

    public ParticleSystem hitEffect;
    public AudioSource deathSound;
    public AudioSource hitSound;

    private Animator zombieAnimator;
    private AudioSource zombieAudioPlayer;
    private Renderer zombieRenderer;

    public float damage = 20f;
    public float timeBetAttack = 0.5f;
    private float lastAttackTime;

    private bool hasTarget
    {
        get
        {

        }
    }
}
