using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    public static UImanager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UImanager>();
            }
            return m_instance;
        }
    }

    public static UImanager m_instance;

    public Text ammoText;
    public Text scoreText;
    public Text waveText;
    public GameObject gameoverUI;


    public void UpdateAmmoText(int magAmmo,int ammoRemain)
    {
        ammoText.text = magAmmo + "/" + ammoRemain;
    }

    public void UpdateScoreText(int newScore)
    {
        ammoText.text = "Score : " + newScore;
    }

    public void UpdateWaveText(int waves, int count)
    {
        ammoText.text = "Wave : " + waves + "\nEnemy Left : " + count ;
    }

    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}