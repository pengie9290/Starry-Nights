using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareManager : MonoBehaviour
{
    //Sets JumpscareManager as a singleton (part 1)
    public static JumpscareManager Instance;

    //Jumpscares
    public List<GameObject> Jumpscares;




    void Start()
    {
        //Sets JumpscareManager as a singleton (part 2)
        if (JumpscareManager.Instance == null) JumpscareManager.Instance = this;
        else Destroy(gameObject);

        foreach (var scare in Jumpscares)
        {
            if (scare != null) scare.SetActive(false);
        }
    }

    public void PlayJumpscare(int threatID)
    {
        if (threatID < Jumpscares.Count && threatID > -1 && Jumpscares[threatID] != null)
        {
            Jumpscares[threatID].SetActive(true);
            PlayScareSound();
            CameraControl.Instance.DeactivateCameras();
            GameManager.Instance.PlayerDeath();
            OfficeManager.Instance.HideOverlay();
        }
    }

    public void PlayScareSound()
    {
        AudioSource Source = gameObject.GetComponent<AudioSource>();
        if (Source != null) Source.Play();
    }

    public bool ForcedJumpscare()
    {
        foreach (var threat in ThreatNavManager.Instance.Threats)
        {
            if (threat.gameObject.activeSelf && threat.ThreatID > 0)
            {
                PlayJumpscare(threat.ThreatID);
                return true;
            }
        }
        return false;
    }
}
