using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Zombie zombiePrefeb;

    public ZombieData[] zombieDatas;
    public Transform[] spawnPoints;

    private List<Zombie> zombies = new List<Zombie>;
    private int wave;

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        if (zombies.Count <= 0)
        {
            SpawnWave();
        }
    }

    private void UpdateUI()
    {
        UImanager.instance.UpdateWaveText(wave, zombies.Count);
    }

    private void SpawnWave()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        for (int i = 0; i < spawnCount; i++)
        {
            CreateZombie();
        }
    }

    private void CreateZombie()
    {

    }
}
