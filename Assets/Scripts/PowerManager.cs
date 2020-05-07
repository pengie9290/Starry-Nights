using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerManager : MonoBehaviour
{
    //Sets PowerManager as a singleton (part 1)
    public static PowerManager Instance;

    public Text PowerDisplay;

    public bool BlackedOut = false;

    public float MaxPower = 100;
    public float IdleDrain = 0.01f;
    public float DoorDrain = 0.5f;
    public float PhoneDrain = 1f;
    public bool PowerbotShutdown = false;
    public GameObject BlackoutSprite1;
    public GameObject BlackoutSprite2;

    //Allows calculation of length of time for Night 1 Powerbot blackouts
    public float MaxPowerOnTime = 5;
    public float RemainingPowerOnTime = 0;

    //Calculates remaining power
    public float remainingPower;
    public float RemainingPower
    {
        get
        {
            if (PowerbotShutdown) return 0;
            else return remainingPower;
        }
        set
        {
            remainingPower = value;
            if ( remainingPower <= 0)
            {
                OfficeManager.Instance.PowerOff();
            }
        }
    }

    void Awake()
    {
        //Sets PowerManager as a singleton (part 2)
        if (PowerManager.Instance == null) PowerManager.Instance = this;
        else Destroy(gameObject);
    }


    void Start()
    {
        ResetPower();
    }

    //Resets power to max
    public void ResetPower()
    {
        PowerbotShutdown = false;
        RemainingPower = MaxPower;
    }

    void Update()
    {
        //Checks remaining power
        if (RemainingPower > 0)
        {
            RemainingPower -= IdleDrain * Time.deltaTime;
            CheckDoorStates();
            if (RemainingPower <= 0)
            {
                Blackout(true);
            }
        }

        //Turns power on if blackout was caused on Night 1 by Powerbot
        if(RemainingPowerOnTime > 0)
        {
            RemainingPowerOnTime -= Time.deltaTime;
            if (RemainingPowerOnTime <= 0)
            {
                BlackoutSprite1.SetActive(false);
                BlackoutSprite2.SetActive(false);
                PowerbotShutdown = false;
                BlackedOut = false;
            }
        }
    }

    void FixedUpdate()
    {
        UpdatePowerDisplay();
    }

    //Subtracts power when doors are closed
    public void CheckDoorStates()
    {
        if (OfficeManager.Instance.LeftDoorBarred) RemainingPower -= DoorDrain * Time.deltaTime;
        if (OfficeManager.Instance.RightDoorBarred) RemainingPower -= DoorDrain * Time.deltaTime;
    }

    //Subtracts power when a phone is called
    public void PhoneCalled()
    {
        RemainingPower -= PhoneDrain;
    }

    //Tells the manager what to do in a blackout
    public void Blackout(bool OutOfPower)
    {
        BlackedOut = true;
        if (!OutOfPower)
        {
            PowerbotShutdown = true;
        }
        BlackoutSprite1.SetActive(true);
        BlackoutSprite2.SetActive(true);
        OfficeManager.Instance.PowerOff();
        if (PowerbotShutdown && GameManager.Instance.NightNumber == 1)
        {
            RemainingPowerOnTime = MaxPowerOnTime;
        }
    }

    public void UpdatePowerDisplay()
    {
        if (PowerDisplay != null)
        {
            int DisplayedPower = (int)Mathf.Ceil(RemainingPower);
            PowerDisplay.text = DisplayedPower.ToString() + "%";
        }
    }
}
