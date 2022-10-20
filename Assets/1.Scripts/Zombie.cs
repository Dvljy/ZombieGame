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
    public AudioClip deathSound;
    public AudioClip hitSound;

    private Animator zombieAnimator;
    private AudioSource zombieAudioPlayer;
    private Renderer zombieRenderer;

    public float damage = 20f;
    public float timeBetAttack = 0.5f;
    private float lastAttackTime;

    //추적할 대상이 존재하는지 알려주는 프로퍼티
    private bool hasTarget
    {
        get
        {
            //추적할 대상이 존재하고 살아있다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            //그렇지 않으면 false
            return false;
        }
        
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioPlayer = GetComponent<AudioSource>();
        zombieRenderer = GetComponentInChildren<Renderer>();

    }

    public void SetUp(ZombieData zombieData)
    {
        //체력설정
        startingHealth = zombieData.health;
        health = zombieData.damage;

        //공격력 설정
        damage = zombieData.damage;

        //navMeshAgent의 이동속도 설정
        navMeshAgent.speed = zombieData.speed;

        //렌더러가 사용 중인 머티리얼의 컬러를 변경, 외형 색이 변함
        zombieRenderer.material.color = zombieData.skinColor;
    }

    private void Start()
    {
        //게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        //추적 대상의 존재 여부에 따라 다른 애니메이션 재생
        zombieAnimator.SetBool("HasTarget", hasTarget);
    }

    private IEnumerator UpdatePath()
    {
        //살아 있는 동안 무한 루프
        while (!dead)
        {
            //추적 대상이 존재하면
            if (hasTarget)
            {
                //AI 이동을 계속함
                navMeshAgent.isStopped = false;
                //타겟의 위치를 받아옴
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            //추적 대상이 존재하지 않으면
            else
            {
                //Ai 이동 중지
                navMeshAgent.isStopped = true;
                //20유닛의 반지름을 가진 가상의 구를 그렸을 때 구와 겹치는 모든 콜라이더를 가져옴
                //단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                //모든 콜라이더를 순회하면서 살아 있는 LivingEntity 찾기
                for (int i = 0; i < colliders.Length; i++)
                {
                    //콜라이더로부터 LivingEntity 컴포넌트 가져오기
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    //LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아 있다면
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        //추적 대상을 LivingEntity로 설정
                        targetEntity = livingEntity;
                        //for 문 루프 즉시 정지
                        break;
                    }
                }
            }
            //0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    //대미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // 살아 있으면 피격 효과 재생
        if (!dead)
        {
            //공격받은 지점과 방향으로 파티클 효과 재생
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            zombieAudioPlayer.PlayOneShot(hitSound);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    //사망 처리
    public override void Die()
    {
        base.Die();

        Collider[] zombieColliders = GetComponents<Collider>();
        for (int i = 0; i < zombieColliders.Length; i++)
        {
            zombieColliders[i].enabled = false;
        }

        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        zombieAnimator.SetTrigger("Die");
        zombieAudioPlayer.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;

                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }
}
