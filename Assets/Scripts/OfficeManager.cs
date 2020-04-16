using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManager : MonoBehaviour
{
    //Sets OfficeManager as a singleton (part 1)
    public static OfficeManager Instance;




    void Start()
    {
        //Sets OfficeManager as a singleton (part 2)
        if (OfficeManager.Instance == null) OfficeManager.Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
