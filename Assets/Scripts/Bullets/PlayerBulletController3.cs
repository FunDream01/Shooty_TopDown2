using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController3 : MonoBehaviour{
    public float forwardSpeed;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    public Transform RightAnchorPoint;
    public Transform LeftAnchorPoint;
    bool MoveForward=true;
    public int NumberOfTargets;
    private PlayerManager Player;
    //Vars for Laith's Controller System
    public Vector2 LastTapPos;
    public float RotationSpeed;
    
    public ParticleSystem DestroyEffect;
    void Start()
    {
        MoveForward = true;
        Player = FindObjectOfType<PlayerManager>();
        NumberOfTargets=FindObjectOfType<RoomManager>().Targets;
    }
    void Update()
    {
        //Mouse Slider Controller :
        if (Input.GetMouseButton(0))
        {
            Vector2 curTapPos = Input.mousePosition;
            if (LastTapPos == Vector2.zero)
            {
                LastTapPos = curTapPos;
            }
            float delta = LastTapPos.x - curTapPos.x;
            LastTapPos = curTapPos;
            //Value to be changed on swipe :
            // Value should be Delta * Speed
            //Debug.Log(delta);
            if (delta < 0)
            {               
                this.transform.RotateAround(RightAnchorPoint.position, Vector3.up, (-delta * RotationSpeed));
            }
            if (delta > 0)
            {                
                this.transform.RotateAround(LeftAnchorPoint.position, Vector3.down, (delta * RotationSpeed));
            }
            //this.transform.Rotate(new Vector3(0, (-delta * Speed), 0));
        }
        if (Input.GetMouseButtonUp(0))
        {
            LastTapPos = Vector3.zero;
        }
        // if not Rotating --> move forword
        if (MoveForward)
            transform.position+=transform.forward*forwardSpeed*Time.deltaTime;
        //Swipe();
    }
   
    private void OnCollisionEnter(Collision other){
        if (other.gameObject.tag==(Tag.Target)){
            other.gameObject.SendMessage("KillTarget");
             NumberOfTargets--;
            if (NumberOfTargets==0){
                Player.FinishRoom();
                
                DestroyEffect.Play();
                Destroy(this.gameObject);
            } 
        }
        else if (other.gameObject.tag==(Tag.TargetBullet)){
            DestroyEffect.Play();
            Destroy(other.gameObject);
        }
        else{
            Player.Lose();
            DestroyEffect.Play();
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(Tag.Entrance)||other.gameObject.CompareTag(Tag.Exit)){
            Player.Lose();
            
            DestroyEffect.Play();
            Destroy(this.gameObject);
        }
    }
}
