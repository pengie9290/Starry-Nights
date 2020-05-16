using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRepel : MonoBehaviour
{
    public bool LeftAudioRepel = true;

    public void OnMouseDown()
    {
        PowerManager.Instance.AlarmRings();
        Debug.Log("Button");
        AudioSource Source = gameObject.GetComponent<AudioSource>();
        if (Source != null) Source.Play();
        ThreatNavManager.Instance.SoundAlarm(LeftAudioRepel);
    }
}
