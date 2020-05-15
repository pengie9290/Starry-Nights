using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beta : Alpha
{
    protected virtual void Awake()
    {
        UpsetDestination = ThreatNavManager.Office;
    }

    //Tells threat what to do if it hears Audio Repel
    public override void AudioSignal()
    {
        IsUpset = true;
        UpsetDestination = 1;
    }

    //Makeshift Update function
    public override void UpdateThreat()
    {
        if (IsUpset)
        {
            if (Location == ThreatNavManager.OutsideLeftDoor || Location == ThreatNavManager.OutsideRightDoor)
            {
                //RemainingTime = MaxAttackTime * Time.deltaTime;
            }
        }
        if (CountDown())
        {
            if (CanThreatMove())
            {
                Debug.Log(CanThreatMove());
                if (Location > -1)
                {
                    DetermineNextStep();
                }
                Debug.Log(ThreatNavManager.Instance.Rooms[Location].name);
            }
        }
    }

    public override bool CanEnterRoom(int destination)
    {
        Debug.Log("CanBetaEnter");
        switch (destination)
        {
            case 1:
                //Resets Beta Threat to normal wandering
                if (IsUpset)
                {
                    IsUpset = false;
                    UpsetDestination = ThreatNavManager.Office;
                }
                return true;
            case 16:
                //Keeps Beta Threat outside the office when Alpha Threat is also outside left door
                IsUpset = true;
                return ThreatNavManager.Instance.GetThreatLocation(2) != 16;
            case 17:
                //Keeps Beta Threat outside the office when Alpha Threat is also outside right door
                IsUpset = true;
                return ThreatNavManager.Instance.GetThreatLocation(2) != 17;
            default:
                return true;
        }
    }
}
