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
    public List<int> SortedExits(bool isPowerBot = false, int destination = -1)
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
        if (destination == -1) Result.Sort((a, b) => b.CompareTo(a));
        else
        {
            Result.Sort((a, b) =>
            {
                int distA = ThreatNavManager.Instance.RoomInRange(a, destination, 3);
                int distB = ThreatNavManager.Instance.RoomInRange(b, destination, 3);
                return distA.CompareTo(distB);
            });
        }
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