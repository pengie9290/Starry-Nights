using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance = null;
    public List<CameraView> Cameras = new List<CameraView>();
    public int CurrentCam = 0;


    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        DisplayCameras();
    }

    void DisplayCameras()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) ShowCam(0);
        if (Input.GetKeyDown(KeyCode.Alpha1)) ShowCam(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ShowCam(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ShowCam(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ShowCam(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ShowCam(5);
        if (Input.GetKeyDown(KeyCode.Alpha6)) ShowCam(6);
        if (Input.GetKeyDown(KeyCode.Alpha7)) ShowCam(7);
        if (Input.GetKeyDown(KeyCode.Alpha8)) ShowCam(8);
        if (Input.GetKeyDown(KeyCode.Alpha9)) ShowCam(9);
    }

    //Shows the feed of the specific selected camera
    void ShowCam(int cam)
    {
        if (cam < Cameras.Count && cam >= 0)
        {
            Cameras[CurrentCam].DeactivateCam();
            CurrentCam = cam;
            Cameras[cam].gameObject.SetActive(true);
            Cameras[cam].ActivateCam();
        }
    }

    //Updates cameras when a creature moves
    public void UpdateCameras(int threatID)
    {
        foreach(var camera in Cameras)
        {
            camera.ThreatHasMoved(threatID);
            Debug.Log(threatID);
        }
    }
}
