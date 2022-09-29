using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State //���� ���¸� ǥ���ϴµ� ����� Ÿ���� ����
    {
        Ready, // �߻� �غ��
        Empty, // źâ�� ��
        Reloading // ������ ��
    }

    public State state { get; private set; } // ���� ���� ����

    public Transform fireTransform; // ź���� �߻�� ��ġ

    public ParticleSystem muzzleFlashEffect; // �ѱ� ȭ�� ȿ��
    public ParticleSystem shellEjectEffect; // ź�� ���� ȿ��

    private LineRenderer bulletLineRenderer; // ź�� ������ �׸��� ���� ������

    private AudioSource gunAudioPlayer; // �� �Ҹ� �����

    public GunData gunData; // ���� ���� ������

    private float fireDistance = 50f; // �����Ÿ�

    public int ammoRemain = 100; //���� ��ü ź��
    public int magAmmo; // ���� źâ�� ���� �ִ� ź��

    private float lastFireTime; // ���� ���������� �߻��� ����

    private void Awake()
    {   //����� ������Ʈ ���� ��������
        fireTransform = GetComponent<Transform>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2; //����� ���� �ΰ��� ����
        bulletLineRenderer.enabled = false; // ���� ������ ��Ȱ��ȭ
    }

    private void OnEnable() // �� ���� �ʱ�ȭ
    {
        ammoRemain = gunData.startAmmoRemain;

        magAmmo = gunData.magCapacity;

        state = State.Ready;
        lastFireTime = 0;
    }

    private void Fire() // �߻� �õ�
    {
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    private void Shot() // ���� �߻� ó��
    {
        //����ĳ��Ʈ�� ���� �浹 ������ �����ϴ� �����̳�
        RaycastHit hit;
        //ź���� ���� ���� ������ ����
        Vector3 hitposition = Vector3.zero;

        //����ĳ��Ʈ(���� ����,����,�浹 ���� �����̳�,�����Ÿ�)
        if (Physics.Raycast(fireTransform.position,fireTransform.forward,out hit, fireDistance))
        {
            //���̰� � ��ü�� �浹�� ���


            //�浹�� �������κ��� IDamageable ������Ʈ �������� �õ�
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            //�������κ��� IDamageable ������Ʈ�� �������� �� �����ߴٸ�
            if (target != null)
            {
                //������ OnDamage �޼��带 ������� ���濡 ����� �ֱ�
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            // ���̰� �浹�� ��ġ ����
            hitposition = hit.point;

        }
        else
        {
            //���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�
            //ź���� �ִ� �����Ÿ����� ���ư��� ���� ��ġ�� �浹 ��ġ�� ���
            hitposition = fireTransform.position + fireTransform.forward * fireDistance;
        }
        
        //�߻� ����Ʈ ��� ����
        StartCoroutine(ShotEffect(hitposition));
        //���� ź�� ��-1
        magAmmo--;
        if (magAmmo <= 0)
        {
            //���� ź���� ������ ���� ���¸� Empty�� ����
            state = State.Empty;
        }
    }
    
    //�߻� ����Ʈ�� �Ҹ��� ����ϰ� ź�� ������ �׶�
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        //���� �������� Ȱ��ȭ�Ͽ� ź�� ������ �׸�
        bulletLineRenderer.enabled = true;

        //0.03�� ���� ��� ó���� ���
        yield return new WaitForSeconds(0.03f);
        
        // ���� �������� ��Ȱ��ȭ�Ͽ� ź�� ������ �׸�
        bulletLineRenderer.enabled = false;
    }

    public bool Reload() //������ �õ�
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            //�̹� ������ ���̰ų� ���� ź���� ���ų�
            //źâ�� ź���� �̹� ������ ��� �������� �� ����
            return false;
        }

        //������ ó�� ����
        StartCoroutine(ReloadRoutine());
        return true;
    }

    // ���� ������ ó���� ����
    private IEnumerator ReloadRoutine()
    {
        //���� ���¸� ������ �� ���·� ��ȯ
        state = State.Reloading;
        //������ �Ҹ� ���
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);

        // ������ �ҿ� �ð���ŭ ó�� ����
        yield return new WaitForSeconds(gunData.reloadTime);

        //źâ�� ä�� ź�� ���
        int ammoToFill = gunData.magCapacity - magAmmo;

        //źâ�� ä���� �� ź���� ���� ź�˺��� ���ٸ�
        //ä���� �� ź�� ���� ���� ź�� ���� ���� ����
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }
        //źâ�� ä��
        magAmmo += ammoToFill;
        // ���� ź�˿��� źâ�� ä�ŭ ź���� ��
        ammoRemain -= ammoToFill;

        // ���� ���� ���¸� �߻� �غ�� ���·� ����
        state = State.Ready;
    }
}
