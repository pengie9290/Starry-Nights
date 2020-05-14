using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Checks to see if two rooms being in range of one another is already recorded
    public int RoomInRange(int Start, int Destination, int Range = 2, List<int> Visited = null)
    {
        if (Visited != null && Visited.Contains(Start)) return 0;
        string theKey = Start.ToString() + ":" + Destination.ToString() + ":" + Range.ToString();
        if (OfficeManager.Instance.LeftDoorBarred) theKey += "L";
        if (OfficeManager.Instance.RightDoorBarred) theKey += "R";
        if (RoomRanges.ContainsKey(theKey))
        {
            return RoomRanges[theKey];
        }
        else
        {
            int Response = RoomInRangeCalculation(Start, Destination, Range, Visited);
            RoomRanges.Add(theKey, Response);

            Debug.Log(Start + " -> " + Destination + " = " + Response);
            return Response;
        }
    }

    //Determines whether two rooms are within specified proximity
    public int RoomInRangeCalculation(int Start, int Destination, int Range = 2, List<int> Visited = null)
    {
        //Getting basic possibilities out of the way
        if (Start == Destination) return 1;
        if (Start < 0 || Start >= Rooms.Count) return 0;
        if (Destination < 0 || Destination >= Rooms.Count) return 0;
        if (Range < 1) return 0;

        //Keeps track of the observed locations to avoid repeating the loop
        if (Visited == null) Visited = new List<int>();
        if (Visited.Contains(Start)) return 0;

        //Determines if the destination is one step away from the start
        if (Rooms[Start].SortedExits(true).Contains(Destination)) return 2;
        Visited.Add(Start);

        //Determines if the destination is within range of the start
        if (Range > 1)
        {
            foreach (var exit in Rooms[Start].SortedExits(true))
            {
                int dist = RoomInRange(exit, Destination, Range - 1, Visited);
                if (dist > 0) return dist + 1;
            }
        }
        return 0;
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
