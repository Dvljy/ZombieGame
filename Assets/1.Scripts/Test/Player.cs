using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action onDeath;

    public void Die()
    {
        UImanager.instance.SetActiveGameoverUI(true);
        onDeath();
    }
}
public class GameData : MonoBehaviour
{
    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onDeath += Save;
        //player.onDeath(); ����(player �ۿ����� onDeath �ߵ� �Ұ�
    }

    public void Save()
    {
        Debug.Log("���� ����");
    }
}
