using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public Image healthBar;
    public Text waveCounter;
    private Pawn player;

    // Start is called before the first frame update
    void Start()
    {
        GetHealthBar();
        GetWaveCounter();
    }

    public void GetHealthBar()
    {
        HealthBar health = FindAnyObjectByType<HealthBar>();
        if (health != null)
        {
            healthBar = health.GetComponent<Image>();
        }
    }

    public void GetWaveCounter()
    {
        WaveCounter wave = FindAnyObjectByType<WaveCounter>();
        if (wave != null)
        {
            waveCounter = wave.GetComponent<Text>();
        }
    }
}
