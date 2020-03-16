using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class PlayerBulletController : MonoBehaviour{
    public float Speed;
    public int NumberOfTargets;
    private Rigidbody Body;
    Vector3 TouchDir;
    private PlayerManager Player;
    void Start(){

        Body=FindObjectOfType<Rigidbody>();
        //Player=GameObject.FindGameObjectWithTag(Tag.Player).GetComponent<PlayerManager>();
    }
    void Update(){
        if (Input.GetMouseButton(0)){
            Vector3 TouchMagnitude=new Vector3(CnInputManager.GetAxis(Axis.Axis_X),0,CnInputManager.GetAxis(Axis.Axis_Y));
            Vector3 TouchPos=transform.position+TouchMagnitude.normalized;
            TouchDir=TouchPos-transform.position;
        }
    }
    void FixedUpdate()
    {
        if (TouchDir== Vector3.zero){
            TouchDir=Vector3.forward;
        }else{
            
            Body.MovePosition(transform.position+(TouchDir*Speed*Time.fixedDeltaTime));
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag==(Tag.Target)){
            other.gameObject.SendMessage("KillTarget");
            Debug.Log("Kill");
            NumberOfTargets--;
            if (NumberOfTargets==0){
                //Player.FinishRoom();
                Destroy(this.gameObject);
            }
        }
        if (other.gameObject.tag==(Tag.TargetBullet)){
            Destroy(other.gameObject);
        }
    }
}
