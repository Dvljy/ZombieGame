using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����� ó���� �ѹ��� �ϱ� ���� ��
public interface IDamageable
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
