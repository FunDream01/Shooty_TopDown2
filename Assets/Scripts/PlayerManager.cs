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
    [HideInInspector]
    public int ReachedRoom=0;
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
        Vector3 Enter=FindObjectOfType<RoomManager>().EntrancePostion;
        this.transform.position=new Vector3(Enter.x,0,Enter.z);
        Shoot();
        Invoke("StartRunning",1f);
    }
    void StartRunning(){
        StopMoving=false;
    }
    void OnTriggerEnter(Collider other){
        if (other.CompareTag(Tag.Exit)){
            ReachedRoom++;
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
    public void RestartLevel(){
        ReachedRoom=0;
    }
    public void NextLode(){
        if (ReachedRoom==1){
            ScenesManager.Instance.LoadRoom(ReachedRoom);
            PlayerReset();
        }else if (ReachedRoom==2){
            ScenesManager.Instance.LoadRoom(ReachedRoom);
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
