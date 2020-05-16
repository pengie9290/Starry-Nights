using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Plays one of 5 random phone calls at the start of each night.
//8 Phone calls happen over the course of the game.
//Night 1's phone call is split into 2 calls, and wouldn't work here, so it was excluded from this list.
//Night 6 has an extra phone call that happens late into the night that wouldn't be played through this function.
//All 8 phone calls can be found in the SFX file, if you're interested in hearing them.

public class PhoneGuy : MonoBehaviour
{
    public float MaxTimer = 10;
    public float CurrentTimer = 10;
    public bool Played = false;
    public List<GameObject> RandomCalls;

    void Start()
    {
        CurrentTimer = MaxTimer;
    }

    void Update()
    {
        if (CurrentTimer > 0)
        {
            CurrentTimer -= Time.deltaTime;
            if (CurrentTimer <= 0 && Played == false && RandomCalls.Count > 0)
            {
                Played = true;
                int Pick = Random.Range(0, RandomCalls.Count - 1);
                RandomCalls[Pick].SetActive(true);
            }
        }
    }
}
