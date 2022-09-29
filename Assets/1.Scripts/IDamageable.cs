using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//대미지 처리를 한번에 하기 위해 씀
public interface IDamageable
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
