using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour{
    public TargetManager[] Targets ;
    public Vector3 EntrancePostion;
    void Awake(){
        Targets=GetComponentsInChildren<TargetManager>(); 
        EntrancePostion=transform.Find("Entrance").transform.position;
    }
}
