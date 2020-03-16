using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController2 : MonoBehaviour{
    public float RotateSpeed;
    public float forwardSpeed;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    public Transform RightAnchorPoint;
    public Transform LeftAnchorPoint;
    bool MoveForward=true;
    public int NumberOfTargets;
    private PlayerManager Player;
    private void Awake() {
        NumberOfTargets=FindObjectOfType<RoomManager>().Targets.Length;
    }
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag(Tag.Player).GetComponent<PlayerManager>();
       
    }
    void Update()
    {
        // if not Rotating --> move forword
        if (MoveForward)
            transform.position+=transform.forward*forwardSpeed*Time.deltaTime;
        Swipe();
    }
    public void Swipe(){
        //first touch 
        if(Input.GetMouseButtonDown(0)){
             //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }
        //Rotate methods
        if(Input.GetMouseButton(0)){
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y); 
            //normalize the 2d vector
            currentSwipe.Normalize();
        //swipe left
            if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f){
                //Debug.Log("left swipe");
                MoveForward=false;
                this.transform.RotateAround(LeftAnchorPoint.position,Vector3.down,Time.deltaTime*RotateSpeed);
            }
        //swipe right
            if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f){
                //Debug.Log("right swipe");
                MoveForward=false;
                this.transform.RotateAround(RightAnchorPoint.position,Vector3.up,Time.deltaTime*RotateSpeed);
            }
        } 
        // move forword method
        if (Input.GetMouseButtonUp(0)){
            MoveForward=true;
        }
    }
    private void OnCollisionEnter(Collision other){
        if (other.gameObject.tag==(Tag.Target)){
            other.gameObject.SendMessage("KillTarget");
             NumberOfTargets--;
            if (NumberOfTargets==0){
                Player.FinishRoom();
                Destroy(this.gameObject);
            } 
        }
        if (other.gameObject.tag==(Tag.TargetBullet)){
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(Tag.Entrance)||other.gameObject.CompareTag(Tag.Exit)){
            Player.Lose();
            Destroy(this.gameObject);
        }
    }
}
