using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatNavManager : MonoBehaviour
{
    //Sets ThreatNavManager as a singleton (part 1)
    public static ThreatNavManager Instance;

    //Stores map of threat locations
    public List<Room> Rooms = new List<Room>();


    void Start()
    {
        //Sets ThreatNavManager as a singleton (part 2)
        if (ThreatNavManager.Instance == null) ThreatNavManager.Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
