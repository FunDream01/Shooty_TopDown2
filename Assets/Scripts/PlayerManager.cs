using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerManager : MonoBehaviour{
    public GameObject Bullet;
    private GameObject TheShootBullet; // the clone bullet
    public float Speed;
    private float initialSpeed;
    public float FastSpeed;
    private Rigidbody Body;
    private int ReachedRoom=0;
    private LevelManager manager;
    private Animator animator;
    private bool StopMoving=true;
    private int State_Idle=0;
    private int State_shoot=1;
    private int State_Run=2;
    private int State_Victory=3;
    
    void Start(){
        Body=GetComponent<Rigidbody>();
        manager=FindObjectOfType<LevelManager>();
        animator=GetComponent<Animator>();
        initialSpeed=Speed;
        PlayerReset();
    }
    
    void Shoot(){
        animator.SetInteger("State",State_shoot);
        TheShootBullet =Instantiate(Bullet,transform.position+(Vector3.up*0.5f)+
            (Vector3.forward),Quaternion.identity);
        TheShootBullet.transform.parent=this.transform;
        TheShootBullet.GetComponent<PlayerBulletController2>().NumberOfTargets=
            FindObjectOfType<RoomManager>().Targets.Length;
    }
    void FixedUpdate(){
        if (!StopMoving){
            animator.SetInteger("State",State_Run);
            Body.MovePosition(transform.position+ Vector3.forward*Speed*Time.deltaTime);
        }
    }
    public void FinishRoom(){
        Destroy(TheShootBullet);
        Speed=FastSpeed;
    }
    void PlayerReset(){
        Speed=initialSpeed;
        StopMoving=true;
        this.transform.position=FindObjectOfType<RoomManager>().EntrancePostion;
        Shoot();
        Invoke("StartRunning",1f);
    }
    void StartRunning(){
        StopMoving=false;
    }
    void OnTriggerEnter(Collider other){
        if (other.CompareTag(Tag.Exit)){
            ReachedRoom++;
            Destroy(TheShootBullet);
            NextLode();
        }
    }
    public void Win(){
        animator.SetInteger("State",State_Victory);
        ScenesManager.Instance.SetActive_Win_Screen(true);
        StopMoving=true;
    }
    public void Lose(){
        animator.SetInteger("State",State_Idle);
        StopMoving=true;
        ScenesManager.Instance.SetActive_Loss_Screen(true);
    }
    public void NextLode(){
        if (ReachedRoom==1){
            ScenesManager.Instance.UnLoad("Room 1");
            ScenesManager.Instance.Load("Room 2");
            PlayerReset();
        }else if (ReachedRoom==2){
            ScenesManager.Instance.UnLoad("Room 2");
            ScenesManager.Instance.Load("Room 3");
            PlayerReset();
        }
        else if (ReachedRoom==3){
            Win();
        }
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag==(Tag.TargetBullet)||other.gameObject.tag==(Tag.PlayerBullet)){
            Lose();
            Destroy(other.gameObject);
        }
    }
}
