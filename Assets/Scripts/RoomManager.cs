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
    void OnBecameInvisible()
    {
        Debug.Log("Room "+this.gameObject.name + "Not Visible");
        this.gameObject.SetActive(false);
    }
    void OnBecameVisible()
    {
        Debug.Log("Room "+this.gameObject.name + "Visible");

        this.gameObject.SetActive(true);
    }
}
