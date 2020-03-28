using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : MonoBehaviour
{
    [HideInInspector]
    public ParticleSystem explosion ;
    // Start is called before the first frame update
    void Start()
    {
        explosion=transform.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
