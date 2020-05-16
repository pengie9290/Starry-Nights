using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneButton : MonoBehaviour
{
    public void OnMouseDown()
    {
        CameraControl.Instance.RingPhone();
    }
}
