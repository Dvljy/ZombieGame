using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action onDeath;

    public void Die()
    {
        onDeath();
    }
}
public class GameData : MonoBehaviour
{
    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onDeath += Save;
        //player.onDeath(); 에러(player 밖에서는 onDeath 발동 불가
    }

    public void Save()
    {
        Debug.Log("게임 저장");
    }
}
