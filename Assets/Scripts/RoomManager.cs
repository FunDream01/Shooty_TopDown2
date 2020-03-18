using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour{
    public TargetManager[] Tragets;
    public Vector3 EntrancePostion;
    void Awake(){
        Tragets=GetComponentsInChildren<TargetManager>();
        EntrancePostion=transform.Find("Entrance").transform.position;
    }
}
