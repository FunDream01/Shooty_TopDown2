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
    [HideInInspector]
    public bool StopMoving=true;
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
    public int NumberOfTargets;
    [HideInInspector]
    public  bool isDead=false;
    private void Awake() {
        NumberOfTargets=FindObjectsOfType<TargetManager>().Length;
    }
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
    public void StartGame(float time){
        if(isDead==false){
            animator.SetInteger("State",State_shoot);
            Speed=initialSpeed;
            StopMoving=true;
            Invoke("StartRunning",time);
        }
    }
    void PlayerReset(){
        
        NumberOfTargets=FindObjectsOfType<TargetManager>().Length;
        StartCoroutine(analytics.waitToCall(analytics.LogLevelStarted,scenesManager.RoomsIndex[ReachedRoom]));
        this.transform.rotation=Quaternion.identity;
        //Vector3 Enter=FindObjectOfType<RoomManager>().EntrancePostion;
        Vector3 Enter=GameObject.FindGameObjectWithTag(Tag.Entrance).transform.position;
        this.transform.position=new Vector3(Enter.x,0,Enter.z);
        //StartGame();
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
                
                StartCoroutine(analytics.waitToCall(analytics.LogLevelSucceeded,scenesManager.RoomsIndex[ReachedRoom]));
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
       // if (isDeath==false){
           
            StartCoroutine(analytics.waitToCall(analytics.LogLevelFailed,scenesManager.RoomsIndex[ReachedRoom]));
            animator.SetInteger("State",State_Idle);
            StopMoving=true;
            ScenesManager.Instance.SetActive_Loss_Screen(true);
       // }
    }public void Death(){
            StartCoroutine(analytics.waitToCall(analytics.LogLevelFailed,scenesManager.RoomsIndex[ReachedRoom]));
        animator.SetInteger("State",State_Death);
        StopMoving=true;
        isDead=true;
        ScenesManager.Instance.SetActive_Loss_Screen(true);
        Destroy(TheShootBullet);
    }
    public void RestartLevel(){
        ReachedRoom=0;
    }
    public void NextLode(){
        ScenesManager.Instance.UpdateLevelIndicator(ReachedRoom);
        if (ReachedRoom==1){
            //GameObject Room=ScenesManager.Instance.LevelRooms[ReachedRoom];
            ScenesManager.Instance.LoadRoom(ScenesManager.Instance.LevelRooms[ReachedRoom]);
            StartGame(1);
            PlayerReset();
        }else if (ReachedRoom==2){
            ScenesManager.Instance.LoadRoom(ScenesManager.Instance.LevelRooms[ReachedRoom]);
            StartGame(1);
            PlayerReset();
        }
        else if (ReachedRoom==3){
            Win();
        }
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag==(Tag.TargetBullet)||other.gameObject.tag==(Tag.PlayerBullet)){
            Death();
            Destroy(other.gameObject);
        }
    }
}
