using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerManager : MonoBehaviour
{
    //Sets PowerManager as a singleton (part 1)
    public static PowerManager Instance;

    public Text PowerDisplay;

    public float MaxPower = 100;
    public float IdleDrain = 0.01f;
    public float DoorDrain = 0.5f;
    public float PhoneDrain = 1f;
    public bool PowerbotShutdown = false;

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
        if (RemainingPower > 0)
        {
            RemainingPower -= IdleDrain * Time.deltaTime;
            CheckDoorStates();
            if (RemainingPower <= 0)
            {
                Blackout(true);
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
        if (!OutOfPower)
        {
            PowerbotShutdown = true;
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
