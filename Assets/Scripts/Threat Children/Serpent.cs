using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serpent : Beta
{
    public bool CanLeaveSpawn = false;
    public float BaseMaxChargeTime = 3 * 60;
    public float CurrentMaxChargeTime = 3 * 60;
    public float RemainingChargeTime = 3 * 60;

    protected override void Awake()
    {
        StartsUpset = true;
        base.Awake();
        NewMaxChargeTime();
    }

    public void NewMaxChargeTime()
    {
        CurrentMaxChargeTime = BaseMaxChargeTime - AILevel - Random.Range(0, AILevel * 2);
        RemainingChargeTime = CurrentMaxChargeTime;
    }

    public override void UpdateThreat()
    {
        if (RemainingChargeTime > 0 && !CanLeaveSpawn)
        {
            RemainingChargeTime -= Time.deltaTime;
            if (RemainingChargeTime <= 0)
            {
                CanLeaveSpawn = true;
                NewMaxChargeTime();
            }
        }
        base.UpdateThreat();
    }

    public override bool CanEnterRoom(int destination)
    {
        Debug.Log("CanSnakeEnter");
        switch (destination)
        {
            case 1:
                //Resets Serpent Threat to approaching the office
                UpsetDestination = ThreatNavManager.Office;
                return true;
            case 2:
                //Keeps Serpent Threat outside the left hall when Alpha Threat is outside left door
                return ThreatNavManager.Instance.GetThreatLocation(2) != 16 && CanLeaveSpawn;
            case 3:
                //Keeps Serpent Threat outside the right hall when Alpha Threat is  outside right door
                return ThreatNavManager.Instance.GetThreatLocation(2) != 17 && CanLeaveSpawn;
            case 18:
                //Keeps Serpent Threat from entering the office after the audio repel has been sounded
                if (UpsetDestination == ThreatNavManager.Office) return true;
                else
                {
                    //IsUpset = false;
                    return false;
                }
            default:
                IsUpset = true;
                return true;
        }
    }
}
