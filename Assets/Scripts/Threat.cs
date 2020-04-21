using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threat : MonoBehaviour
{
    public int Spawnpoint;
    public int Location;
    public int AILevel = 0;

    public bool IsPowerbot = false;

    public float MoveTime = 5;
    public float RemainingTime = 5;


    void Start()
    {
        Location = Spawnpoint;
        RemainingTime = MoveTime;
    }

    void Update()
    {
        if (CountDown())
        {
            Debug.Log(ThreatNavManager.Instance.Rooms[Location].name);
            if (Location > -1)
            {
                var exits = ThreatNavManager.Instance.Rooms[Location].SortedExits(IsPowerbot);
                if (exits.Count > 0)
                {
                    Location = exits[0];
                }
            }
        }
    }

    bool CountDown()
    {
        RemainingTime -= Time.deltaTime;
        if (RemainingTime <= 0)
        {
            RemainingTime = MoveTime;
            return true;
        } else
        {
            return false;
        }
    }
}
