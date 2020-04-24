using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Makes GameManager a singleton (part 1)
    public static GameManager Instance;

    //Tracks which night the game is on
    public int NightNumber = 1;

    //Tracks elapsed (ingame) seconds
    public float IngameSeconds = 0;

    //Tracks length of a night (in ingame seconds)
    public float NightLength = 6 * 60 * 60;

    //Conversion rate of IRL time to ingame time (IRL time * TimeConversionFactor = ingame time)
    public float TimeConversionFactor = 60;

    //Tracks whether the night is in progress
    public bool NightInProgress = false;





    void Start()
    {
        //Makes GameManager a singleton (part 2)
        if (GameManager.Instance == null) GameManager.Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (NightInProgress)
        {
            IngameSeconds += Time.deltaTime * TimeConversionFactor;
            if (IngameSeconds >= NightLength)
            {
                NightEnded();
            }
        }
    }

    public void NightStarted(int nightNum = 0)
    {
        //Determines whether the game is played sequentially or selectively
        if (nightNum > 0) NightNumber = nightNum;
        else NightNumber++;

        IngameSeconds = 0;
        NightInProgress = true;
    }

    void NightEnded()
    {
        NightInProgress = false;
    }
}
