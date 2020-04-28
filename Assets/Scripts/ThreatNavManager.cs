using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatNavManager : MonoBehaviour
{
    //Sets ThreatNavManager as a singleton (part 1)
    public static ThreatNavManager Instance;

    //Stores map of threat locations
    public List<Room> Rooms = new List<Room>();

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


    void Start()
    {
        //Sets ThreatNavManager as a singleton (part 2)
        if (ThreatNavManager.Instance == null) ThreatNavManager.Instance = this;
        else Destroy(gameObject);

    }

    //Checks to see if two rooms being in range of one another is already recorded
    public bool RoomInRange(int Start, int Destination, int Range = 2, List<int> Visited = null)
    {
        if (Visited != null && Visited.Contains(Start)) return false;
        string theKey = Start.ToString() + ":" + Destination.ToString() + ":" + Range.ToString();
        if (RoomRanges.ContainsKey(theKey))
        {
            return RoomRanges[theKey];
        }
        else
        {
            bool Response = RoomInRangeCalculation(Start, Destination, Range, Visited);
            RoomRanges.Add(theKey, Response);

            return Response;
        }
    }

    //Determines whether two rooms are within specified proximity
    public bool RoomInRangeCalculation(int Start, int Destination, int Range = 2, List<int> Visited = null)
    {
        //Getting basic possibilities out of the way
        if (Start == Destination) return true;
        if (Start < 0 || Start >= Rooms.Count) return false;
        if (Destination < 0 || Destination >= Rooms.Count) return false;
        if (Range < 1) return false;

        //Keeps track of the observed locations to avoid repeating the loop
        if (Visited == null) Visited = new List<int>();
        if (Visited.Contains(Start)) return false;

        //Determines if the destination is one step away from the start
        if (Rooms[Start].SortedExits(true).Contains(Destination)) return true;
        Visited.Add(Start);

        //Determines if the destination is within range of the start
        if (Range > 1)
        {
            foreach (var exit in Rooms[Start].SortedExits(true))
            {
                if (RoomInRange(exit, Destination, Range - 1, Visited)) return true;
            }
        }
        return false;
    }

    //Stores the results of previous RoomInRange Calculations
    private Dictionary<string, bool> RoomRanges = new Dictionary<string, bool>();


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
}
