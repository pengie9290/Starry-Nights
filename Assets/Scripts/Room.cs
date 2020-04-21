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
    public List<int> SortedExits(bool isPowerBot = false)
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
        Result.Sort((a, b) => b.CompareTo(a));
        return Result;
    }

    //Adds a threat to PresentThreats when they enter the room
    public void EnterRoom(Threat enteringThreat)
    {
        PresentThreats.Add(enteringThreat);
    }

    //Removes a threat to PresentThreats when they exit the room
    public void ExitRoom(Threat exitingThreat)
    {
        PresentThreats.Remove(exitingThreat);
    }
}