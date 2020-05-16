using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public void OnMouseDown()
    {
        Debug.Log("Button");
        OfficeManager.Instance.ToggleCurrentDoor();
    }
}
