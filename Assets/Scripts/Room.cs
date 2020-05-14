using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Room 
{
    public int ID;
    public int camID;
    public int PhoneID;
    public string name;
    public List<int> CreatureExits;
    public List<int> PowerbotExits;
    public List<Threat> PresentThreats;

    //Get sorted list of room exits 
    public List<int> SortedExits(bool isPowerBot = false, bool PassesBars = false, int destination = -1)
    {
        List<int> Result = new List<int>();
        foreach (var exit in CreatureExits)
        {
            if (exit > -1)
            {
                Result.Add(exit);
            }
        }
        if (isPowerBot)
        {
            foreach(var exit in PowerbotExits)
            {
                Result.Add(exit);
            }
        }

        //Office cannot be entered if the relevant door is closed
        if (Result.Contains(ThreatNavManager.Office) && !PassesBars)
        {
            if (OfficeManager.Instance.LeftDoorBarred == true && ID == 16) Result.Remove(ThreatNavManager.Office);
            if (OfficeManager.Instance.RightDoorBarred == true && ID == 17) Result.Remove(ThreatNavManager.Office);
        }

        if (destination == -1) Result.Sort((a, b) => b.CompareTo(a));
        else
        {
            Result.Sort((a, b) =>
            {
                int distA = ThreatNavManager.Instance.RoomInRange(a, destination, 30);
                int distB = ThreatNavManager.Instance.RoomInRange(b, destination, 30);
                return distA.CompareTo(distB);
            });
        }
        string message = "destination = " + destination + "; ";
        foreach (var exit in Result)
        {
            message = message + exit + ", ";
        }
        Debug.Log(message);
        return Result;
    }

    //Adds a threat to PresentThreats when they enter the room
    public void EnterRoom(Threat enteringThreat)
    {
        Debug.Log(enteringThreat.ThreatID + "says Hello!");
        if (!PresentThreats.Contains(enteringThreat)) PresentThreats.Add(enteringThreat);
        if (enteringThreat.IsPowerbot)
        {
            CameraControl.Instance.ShutDownPhone(ID);
        }
    }

    //Removes a threat to PresentThreats when they exit the room
    public void ExitRoom(Threat exitingThreat)
    {
        PresentThreats.Remove(exitingThreat);
    }
}