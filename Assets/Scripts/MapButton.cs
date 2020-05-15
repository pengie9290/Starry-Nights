using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : MonoBehaviour
{
    public int CamToActivate;

    public void OnMouseDown()
    {
        CameraControl.Instance.ShowCam(CamToActivate);
    }
}
