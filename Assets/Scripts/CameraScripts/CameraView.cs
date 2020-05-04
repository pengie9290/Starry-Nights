using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    public List<int> Rooms = new List<int>();
    public List<ThreatList> PotentialThreats = new List<ThreatList>();
    public Dictionary<int, int> ActiveThreats = new Dictionary<int, int>();

    public List<int> FoundThreats = new List<int>();

    public GameObject StaticScreen;
    public GameObject Background;
    public GameObject RingingPhone;
    public string CamName;

    public float RemainingPhoneDelay = 0f;
    public bool PhoneIsRinging = false;

    public int PhoneID
    {
        get
        {
            int phone = -1;
            if (RemainingPhoneDelay > 0)
            {
                return -1;
            }
            foreach (int i in Rooms)
            {
                Room room = ThreatNavManager.Instance.Rooms[i];
                if (room.PhoneID >= 0)
                {
                    phone = room.PhoneID;
                    return phone;
                }
            }
            return phone;
        }
    }

    public int RoomID
    {
        get
        {
            if (Rooms.Count < 1) return -1;
            else return Rooms[0];
        }
    }


    public void ActivateCam()
    {
        UpdateCam();
    }

    public void DeactivateCam()
    {
        StaticScreen.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void UpdateCam()
    {
        StaticScreen.SetActive(true);
        ActiveThreats.Clear();
        FoundThreats.Clear();
        for (int position = 0; position < Rooms.Count; position++)
        {
            var room = ThreatNavManager.Instance.Rooms[Rooms[position]];
            foreach(var threat in room.PresentThreats)
            {
                if (ActiveThreats.ContainsKey(threat.ThreatID)) ActiveThreats.Remove(threat.ThreatID);
                ActiveThreats.Add(threat.ThreatID, position);
                FoundThreats.Add(threat.ThreatID);
            }
        }
        HideAllThreats();
        DisplayPresentThreats();
    }

    void HideAllThreats()
    {
        foreach (var threatList in PotentialThreats)
        {
            threatList.HideThreats();
        }
    }

    void DisplayPresentThreats()
    {
        foreach (var ID in ActiveThreats.Keys)
        {
            var Position = ActiveThreats[ID];
            if (Position >= 0 && Position < PotentialThreats.Count)
            {
                var ThreatSet = PotentialThreats[Position];
                if (ID >= 0 && ID < ThreatSet.Threats.Count)
                {
                    ThreatSet.Threats[ID].SetActive(true);
                }
            }
        }
    }

    public void ThreatHasMoved(int threatID)
    {
        if (ActiveThreats.ContainsKey(threatID)) UpdateCam();
        else
        {
            var RoomNumber = ThreatNavManager.Instance.GetThreatLocation(threatID);
            if (Rooms.Contains(RoomNumber)) UpdateCam();
        }
    }

    //Rings the phone in the selected room
    public void RingPhone()
    {
        PhoneIsRinging = true;
        CameraControl.Instance.StartRinging(this);
        if (RingingPhone != null) RingingPhone.SetActive(true);
    }

    public void PhoneOff()
    {
        RemainingPhoneDelay = CameraControl.Instance.MaxPhoneDelay;
        if (RingingPhone != null) RingingPhone.SetActive(false);

    }

    void Update()
    {
        if (RemainingPhoneDelay > 0)
        {
            RemainingPhoneDelay -= Time.deltaTime;
            if (RemainingPhoneDelay <= 0)
            {
                RemainingPhoneDelay = 0;
            }
        }
    }
}
