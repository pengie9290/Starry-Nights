using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha : Threat
{
    public bool Aggressive = false;

    //Tells threat what to do if it hears Audio Repel
    public override void AudioSignal()
    {
        Debug.Log("Do Something");
        Aggressive = true;
        MoveTime = 0;
    }

    void Awake()
    {
        Aggressive = false;
    }

    public override void UpdateThreat()
    {
        if (Aggressive)
        {
            RemainingTime = Time.deltaTime;
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

    public override void DetermineNextStep()
    {
        if (!Aggressive) base.DetermineNextStep();
        else
        {
            Location = MoveTowards(ThreatNavManager.Office, null, Aggressive);
        }
        
    }

}
