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
    private bool StopMoving=true;
    public int RemainTargets;
    public ParticleSystem GunShot;
    //Animation
    private Animator animator;
    private int State_Idle=0;
    private int State_shoot=1;
    private int State_SlowWalk=2;
    private int State_Run=3;
    private int State_Victory=4;
    private int State_Death=5;
    private Analytics analytics;
    private ScenesManager scenesManager;
    
    void Start(){
        analytics=FindObjectOfType<Analytics>();
        scenesManager=FindObjectOfType<ScenesManager>();
        Body=GetComponent<Rigidbody>();
        manager=FindObjectOfType<LevelManager>();
        animator=GetComponent<Animator>();
        initialSpeed=Speed;
        PlayerReset();
    }
    void Shoot(){
        GunShot.Play();
        TheShootBullet =Instantiate(Bullet,transform.position+(Vector3.up*0.5f)+
            (Vector3.forward*1.5f),Quaternion.identity);
        TheShootBullet.transform.parent=this.transform;
    }
    void FixedUpdate(){
        if (!StopMoving){
            animator.SetInteger("State",State_SlowWalk);
            Body.MovePosition(transform.position+ Vector3.forward*Speed*Time.deltaTime);
        }
    }
    public void FinishRoom(){
        Destroy(TheShootBullet);
        animator.SetInteger("State",State_Run);
        Speed=FastSpeed;
    }
    public void StartGame(){
        animator.SetInteger("State",State_shoot);
        Speed=initialSpeed;
        StopMoving=true;
        Invoke("StartRunning",2f);
    }
    void PlayerReset(){
        
        analytics.LogLevelStarted(scenesManager.RoomsIndex[ReachedRoom]);
        this.transform.rotation=Quaternion.identity;
        Vector3 Enter=FindObjectOfType<RoomManager>().EntrancePostion;
        this.transform.position=new Vector3(Enter.x,0,Enter.z);
    }
    void StartRunning(){
        Shoot();
        StopMoving=false;
    }
    void OnTriggerEnter(Collider other){
        if (other.CompareTag(Tag.Exit)){
            if (TheShootBullet!=null){
                Lose();
            }else{
                analytics.LogLevelSucceeded(scenesManager.RoomsIndex[ReachedRoom]);
                ReachedRoom++;
                NextLode();
            }
            
        }
    }
    public void Win(){
        animator.SetInteger("State",State_Victory);
        ScenesManager.Instance.SetActive_Win_Screen(true);
        StopMoving=true;
    }
    public void Lose(){
        analytics.LogLevelFailed(scenesManager.RoomsIndex[ReachedRoom]);
        animator.SetInteger("State",State_Death);
        StopMoving=true;
        ScenesManager.Instance.SetActive_Loss_Screen(true);
    }
    public void RestartLevel(){
        ReachedRoom=0;
    }
    public void NextLode(){
        ScenesManager.Instance.UpdateLevelIndicator(ReachedRoom);
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
