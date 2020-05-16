using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ThreatList
{
    public List<GameObject> Threats = new List<GameObject>();

    public void HideThreats()
    {
        foreach (var threat in Threats)
        {
            threat.SetActive(false);
        }
    }
}
