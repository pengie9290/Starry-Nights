using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThreatNavManager : MonoBehaviour
{
    //Sets ThreatNavManager as a singleton (part 1)
    public static ThreatNavManager Instance;

    //Stores map of threat locations
    public List<Room> Rooms = new List<Room>();

    //Stores location of office
    public static int Office = 18;

    //Stores locations outside office
    public static int OutsideLeftDoor = 16;
    public static int OutsideRightDoor = 17;

    //Stores location of hall rooms
    public static List<int> LeftHall = new List<int>(new int[] { 2, 5, 9, 13, 16 });
    public static List<int> RightHall = new List<int>(new int[] { 3, 6, 10, 14, 17 });

    //Stores list of all threats, active or otherwise
    public List<Threat> Threats = new List<Threat>();

    //Populates list of threats
    public void RegisterThreat(Threat threat)
    {
        if (!Threats.Contains(threat)) Threats.Add(threat);
    }

    //Depopulates list of threats
    public void RemoveThreat(Threat threat)
    {
        if (Threats.Contains(threat)) Threats.Remove(threat);
    }


    void Awake()
    {
        //Sets ThreatNavManager as a singleton (part 2)
        if (ThreatNavManager.Instance == null) ThreatNavManager.Instance = this;
        else Destroy(gameObject);

    }

    string MakeKey(int Start, int Destination, int Range, bool isPowerbot, bool ignoreBars)
    {
        string theKey = Start.ToString() + ":" + Destination.ToString() + ":" + Range.ToString();
        if (isPowerbot) theKey += ":PB:";
        if (ignoreBars) theKey += ":IB";
        if (OfficeManager.Instance.LeftDoorBarred) theKey += ":L";
        if (OfficeManager.Instance.RightDoorBarred) theKey += ":R";
        return theKey;
    }

    //Checks to see if two rooms being in range of one another is already recorded
    public int RoomInRange(int Start, int Destination, int Range = 2, List<int> Visited = null, bool isPowerbot=false, bool ignoreBars=false)
    {
        if (Visited != null && Visited.Contains(Start)) return 20000;
        var theKey = MakeKey(Start, Destination, Range, isPowerbot, ignoreBars);
        if (RoomRanges.ContainsKey(theKey))
        {
            Debug.Log(".." + theKey+" === " + RoomRanges[theKey]);
            return RoomRanges[theKey];
        }
        else
        {
            int Response = RoomInRangeCalculation(Start, Destination, Range, Visited, isPowerbot, ignoreBars);
            if (Response > 0 && Response < 1000) //make sure we don't save a failure from retracking our steps
            {
                if (RoomRanges.ContainsKey(theKey))
                {
                    // make sure we don't have a lower path already
                    // this may have been found earlier during this recursive search
                    var oldKey = RoomRanges[theKey];
                    if (oldKey < Response)
                        Response = oldKey;
                }
                RoomRanges[theKey] = Response;
                var message = theKey + " = " + Response + ", " + "Range = " + Range + "   [";
                
                if (Visited != null)
                    foreach (var previous in Visited)
                    {
                        message += previous + ", ";
                    }
                
                message += "]";
                Debug.Log(message);
            }
            return Response;
        }
    }

    //Determines whether two rooms are within specified proximity
    public int RoomInRangeCalculation(int Start, int Destination, int Range = 2, List<int> Visited = null, bool isPowerbot=false, bool ignoreBars=false)
    {
        //Getting basic possibilities out of the way
        if (Start == Destination) return 1;
        if (Start < 0 || Start >= Rooms.Count) return 10000;
        if (Destination < 0 || Destination >= Rooms.Count) return 10000;
        if (Range < 1) return 10000;

        //Keeps track of the observed locations to avoid repeating the loop
        if (Visited == null) Visited = new List<int>();
        if (Visited.Contains(Start)) return 10000;

        //Determines if the destination is one step away from the start
        if (Rooms[Start].SortedExits(isPowerbot).Contains(Destination)) return 2;
        Visited.Add(Start);

        var closest = 10000;
        //Determines if the destination is within range of the start
        if (Range > 1)
        {
            Debug.Log("---- From room "+Start);
            foreach (var exit in Rooms[Start].SortedExits(isPowerbot))
            {
                int dist = RoomInRange(exit, Destination, Range - 1, Visited)+1;
                Debug.Log("  --- Exit "+exit+" is "+(dist-1));
                if (dist > 1 && dist < 10000 && dist < closest)
                {
                    closest = dist;
                }
            }
            Debug.Log("          ---- Closest is "+closest);
        }
        return closest;
    }

    //Stores the results of previous RoomInRange Calculations
    private Dictionary<string, int> RoomRanges = new Dictionary<string, int>();


    //Literally only exists to test whether "RoomInRange" works properly
    void RoomTests()
    {
        int TestInt1 = Random.Range(0, Rooms.Count);
        int TestInt2 = Random.Range(0, Rooms.Count);

        Debug.Log(TestInt1 + ", " + TestInt2 + ", " + RoomInRange(TestInt1, TestInt2));
    }

    void Update()
    {
        //RoomTests();
    }

    //Finds location of a specific threat
    public int GetThreatLocation(int threatID)
    {
        foreach (var threat in Threats)
        {
            if (threat.ThreatID == threatID) return threat.location;
        }
        return -1;
    }

    //Checks to see if threat is in a hallway
    public int IsThreatInHall(int CheckID)
    {
        int LocationToFind = GetThreatLocation(CheckID); 
        foreach (var room in LeftHall)
        {
            if (room == LocationToFind) return OutsideLeftDoor; 
        }
        foreach (var room in RightHall)
        {
            if (room == LocationToFind) return OutsideRightDoor;
        }
        return -1;
    }

    //Sends a signal to threats when an audio repel is used
    public void SoundAlarm(bool leftAudioRepel)
    {
        //Determines which room to signal
        int SignaledRoom = leftAudioRepel ? OutsideLeftDoor : OutsideRightDoor;
        foreach (var threat in Threats)
        {
            if (threat.Location == SignaledRoom) threat.AudioSignal();
        }
        
        
    }
}
