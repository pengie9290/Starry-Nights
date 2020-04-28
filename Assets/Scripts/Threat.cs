using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threat : MonoBehaviour
{
    public int Spawnpoint;
    public int Location
    {
        get
        {
            return location;
        }
        set
        {
            if (value == location) return;
            else
            {
                if (location >= 0 && location < ThreatNavManager.Instance.Rooms.Count)
                {
                    var room = ThreatNavManager.Instance.Rooms[location];
                    room.ExitRoom(this);
                }
                location = value;
                if (location >= 0 && location < ThreatNavManager.Instance.Rooms.Count)
                {
                    var room = ThreatNavManager.Instance.Rooms[location];
                    room.EnterRoom(this);
                }
            }
            CameraControl.Instance.UpdateCameras(ThreatID);
        }
    }
    public int location;
    public int AILevel = 0;

    public float MoveTime = 5;
    public float RemainingTime = 5;

    private int PreviousLocation = -1;

    //ID 0 = Powerbot
    public int ThreatID = 0;
    public bool IsPowerbot
    {
        get
        {
            return ThreatID == 0;
        }
    }


    void Start()
    {
        ThreatNavManager.Instance.RegisterThreat(this);
        Location = Spawnpoint;
        RemainingTime = MoveTime;
        Debug.Log(ThreatNavManager.Instance.Rooms[Location].name);
    }

    void Update()
    {
        if (CountDown())
        {
            if (CanThreatMove())
            {
                if (Location > -1)
                {
                    DetermineNextStep();
                }
                Debug.Log(ThreatNavManager.Instance.Rooms[Location].name);
            }
        }
    }

    //Determines whether threat can move at all
    bool CanThreatMove()
    {
        if (Random.Range(0, 20) > AILevel) return false;
        else return true;
    }

    //Determines best next step in threat's movement
    void BestPathStep()
    {
        var exits = ThreatNavManager.Instance.Rooms[Location].SortedExits(IsPowerbot);
        if (exits.Count > 0)
        {
            if (Location == 1)
            {
                int PickHall = Random.Range(0, 2);
                Location = exits[PickHall];
            } else {
                Location = exits[0];
            }
        }
    }

    //Randomly selects next step in threat's movement
    void RandomPathStep()
    {
        var exits = ThreatNavManager.Instance.Rooms[Location].SortedExits(IsPowerbot);
        if (exits.Count > 0)
        {
            if (exits.Count > 1 && exits.Contains(PreviousLocation)) exits.Remove(PreviousLocation);
            int PickHall = Random.Range(0, exits.Count);
            Location = exits[PickHall];
        }
    }

    void DetermineNextStep()
    {
        PreviousLocation = Location;
        float MovementEfficiency = AILevel/2;
        float RandomSelect = Random.Range(0, 100);
        if (RandomSelect > MovementEfficiency + 50) RandomPathStep();
        else BestPathStep();
    }

//Determines when threat can move
bool CountDown()
    {
        RemainingTime -= Time.deltaTime;
        if (RemainingTime <= 0)
        {
            RemainingTime = MoveTime;
            return true;
        } else {
            return false;
        }
    }
}
