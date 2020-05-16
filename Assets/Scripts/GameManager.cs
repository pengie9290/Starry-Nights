using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //What to show when the player wins
    public GameObject VictoryScreen;


    //In-game Clock
    public Text Clock;

    //Determines what do display in ingame clock
    public string HourString
    {
        get
        {
            int hours = Mathf.FloorToInt(IngameSeconds / (60 * 60));
            switch (hours)
            {
                case 0:
                    return "12";
                case 10:
                case 11:
                case 12:
                    return hours.ToString();
                default:
                    return "0" + hours.ToString();
            }
        }
    }
    public string MinuteString
    {
        get
        {
            int seconds = Mathf.FloorToInt(IngameSeconds % (60 * 60));
            int minutes = seconds / 60;
            if (minutes < 10) return "0" + minutes.ToString();
            else return minutes.ToString();
        }
    }




    void Start()
    {
        //Makes GameManager a singleton (part 2)
        if (GameManager.Instance == null) GameManager.Instance = this;
        else Destroy(gameObject);

        NightInProgress = true;
    }

    void Update()
    {
        if (NightInProgress)
        {
            IngameSeconds += Time.deltaTime * TimeConversionFactor;
            DisplayTime();
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

    public void NightEnded(bool Killed = false)
    {
        NightInProgress = false;
        if (!Killed)
        {
            if (PowerManager.Instance.RemainingPower <= 0)
            {
                if (!JumpscareManager.Instance.ForcedJumpscare())
                {
                    NightWon();
                }
            }
            else
            {
                NightWon();
            }
        }
    }

    public void PlayerDeath()
    {
        NightEnded(true);
    }

    public void NightWon()
    {
        VictoryScreen.SetActive(true);
    }

    public void DisplayTime()
    {
        Clock.text = HourString + ":" + MinuteString;
    }
}
