using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour{
    public int Targets;
    public Vector3 EntrancePostion;
    void Awake(){
        Targets=GetComponentsInChildren<TargetManager>().Length;
        EntrancePostion=transform.Find("Entrance").transform.position;
    }
}
 