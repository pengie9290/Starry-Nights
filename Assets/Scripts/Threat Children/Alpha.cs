using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha : Threat
{
    public int UpsetDestination = 1;
    public float MaxAttackTime = 10;

    //Tells threat what to do if it hears Audio Repel
    public override void AudioSignal()
    {
        IsUpset = true;
        MoveTime = 0;
    }

    //Makeshift Update function
    public override void UpdateThreat()
    {
        if (CountDown())
        {
            if (CanThreatMove())
            {
                Debug.Log(CanThreatMove());
                if (Location > -1)
                {
                    DetermineNextStep();
                    if (IsUpset)
                    {
                        if (Location == ThreatNavManager.OutsideLeftDoor || Location == ThreatNavManager.OutsideRightDoor)
                        {
                            RemainingTime = MaxAttackTime * Time.deltaTime;
                        }
                    }
                }
                Debug.Log(ThreatNavManager.Instance.Rooms[Location].name);
            }
        }
    }

    public override void DetermineNextStep()
    {
        Debug.Log("DetermineNextStep OVERRIDDEN");
        if (!IsUpset)
        {
            if (Destination <= 0)
            {
                base.DetermineNextStep();
            }
            else
            {
                Location = MoveTowards(Destination);
            }
        }
        else
        {
            Location = MoveTowards(UpsetDestination, null, IsUpset);
        }
    }

    public override bool CanEnterRoom(int destination)
    {
        CheckDestinationArrival(destination);
        switch (destination)
        {
            case 16:
                //Keeps Alpha Threat outside the office when Beta Threat is also outside left door or Serpent Threat is in left hall
                return ThreatNavManager.Instance.GetThreatLocation(3) != 16 && ThreatNavManager.Instance.IsThreatInHall(4) != 16;
            case 17:
                //Keeps Alpha Threat outside the office when Beta Threat is also outside right door or Serpent Threat is in right hall
                return ThreatNavManager.Instance.GetThreatLocation(3) != 17 && ThreatNavManager.Instance.IsThreatInHall(4) != 17;
            default:
                return true;
        }
    }
}
