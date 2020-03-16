using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    LevelManager Manager;
    public float AnimationSpeed;
    void Start()
    {
        Manager=FindObjectOfType<LevelManager>();
    }
    public void MoveNextRoom(int RoomIndex){
        Debug.Log("MoveNextRoom");
        //transform.position=Manager.rooms[RoomIndex].transform.position;
    }

}
