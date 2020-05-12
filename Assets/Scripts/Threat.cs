using System;
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
                if (location == ThreatNavManager.Office) JumpscareManager.Instance.PlayJumpscare(ThreatID);
                if (location >= 0 && location < ThreatNavManager.Instance.Rooms.Count)
                {
                    var room = ThreatNavManager.Instance.Rooms[location];
                    room.EnterRoom(this);
                }
                if (IsPowerbot) CheckPowerbotLights();
            }
            CameraControl.Instance.UpdateCameras(ThreatID);
        }
    }
    public int location;
    public int AILevel = 0;

    public float MoveTime = 5;
    public float RemainingTime = 5;

    private int PreviousLocation = -1;

    public bool PassesBars = false;

    //ID 0 = Powerbot
    //ID 1 = Crawler
    //ID 2 = Alpha
    //ID 3 = Beta
    //ID 4 = Serpent
    //ID 5 = [REDACTED]
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
        var room = ThreatNavManager.Instance.Rooms[location];
        room.EnterRoom(this);
        Debug.Log(ThreatNavManager.Instance.Rooms[Location].name);
    }

    void Update()
    {
        if (GameManager.Instance.NightInProgress)
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
    }

    //Determines whether threat can move at all
    bool CanThreatMove()
    {
        if (UnityEngine.Random.Range(0, 20) > AILevel) return false;
        else return true;
    }

    //Determines best next step in threat's movement
    void BestPathStep(int destination = -1)
    {
        var exits = ThreatNavManager.Instance.Rooms[Location].SortedExits(IsPowerbot, PassesBars);
        if (exits.Count > 0)
        {
            if (destination == -1)
            {
                if (Location == 1)
                {
                    int PickHall = UnityEngine.Random.Range(0, 2);
                    Location = exits[PickHall];
                }
                else
                {
                    Location = exits[0];
                }
            } else {
                Location = MoveTowards(destination, exits);
            }
        }
    }

    int MoveTowards (int destination, List<int> exits)
    {
        if (destination == 1)
        {
            return exits[exits.Count - 1];
        }
        exits = ThreatNavManager.Instance.Rooms[Location].SortedExits(IsPowerbot, PassesBars, destination);
        foreach (int exit in exits)
        {
            if (exit > 0)
            {
                Debug.Log("This is the " + exit);
                return exit;
            } 
        }
        return Location;
    }



    //Randomly selects next step in threat's movement
    void RandomPathStep()
    {
        var exits = ThreatNavManager.Instance.Rooms[Location].SortedExits(IsPowerbot, PassesBars);
        if (exits.Count > 0)
        {
            if (exits.Count > 1 && exits.Contains(PreviousLocation)) exits.Remove(PreviousLocation);
            int PickHall = UnityEngine.Random.Range(0, exits.Count);
            Location = exits[PickHall];
        }
    }

    public virtual void DetermineNextStep()
    {
        PreviousLocation = Location;

        int destination = -1;
        if (IsPowerbot)
        {
            destination = AudiblePhoneRinging;
        }

        float MovementEfficiency = AILevel/2;
        if (destination == -1)
        {
            float RandomSelect = UnityEngine.Random.Range(0, 100);
            if (RandomSelect > MovementEfficiency + 50) RandomPathStep();
            else BestPathStep();
        } else {
            BestPathStep(destination);
        }
        if (IsPowerbot && Location == ThreatNavManager.Office)
        {
            PowerManager.Instance.Blackout(false);
        }
    }

    //Is there a ringing phone in range of the threat?
    public int AudiblePhoneRinging
    {
        get
        {
            int CurrentPhone = CameraControl.Instance.RingingPhone;
            if (CurrentPhone < 0) return CurrentPhone;
            else
            {
                Debug.Log("Phone Is Ringing");

                int inRange = ThreatNavManager.Instance.RoomInRange(Location, CurrentPhone, 2);
                if (inRange > 0)
                {
                    Debug.Log("I hear Phone " + CurrentPhone);
                    return CurrentPhone;
                }
                else return -1;
            }
        }
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

    //Turns Powerbot lights on/off
    public void CheckPowerbotLights()
    {
        if (Location == ThreatNavManager.OutsideLeftDoor)
        {
            OfficeManager.Instance.LeftLightOn = true;
            return;
        }
        if (Location == ThreatNavManager.OutsideRightDoor)
        {
            OfficeManager.Instance.RightLightOn = true;
            return;
        }
        OfficeManager.Instance.LeftLightOn = false;
        OfficeManager.Instance.RightLightOn = false;
    }
}
