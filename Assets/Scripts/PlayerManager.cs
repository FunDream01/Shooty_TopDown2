using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerManager : MonoBehaviour{
    public float BulletSpace;
    public GameObject Bullet;
    private GameObject TheShootBullet; // the clone bullet
    public float Speed;
    private float initialSpeed;
    public float FastSpeed;
    private Rigidbody Body;
    private int ReachedRoom=0;
    public LevelManager manager;
    public Animator animator;
    private bool DidWin;
    private int State_shoot=0;
    private int State_Run=1;
    private int State_Victory=2;
    
    void Start(){
        initialSpeed=Speed;
        Body=GetComponent<Rigidbody>();
        manager=FindObjectOfType<LevelManager>();
        animator=GetComponent<Animator>();
        Shoot();
    }
    void Shoot(){
        animator.SetInteger("State",State_shoot);
        TheShootBullet =Instantiate(Bullet,transform.position+(Vector3.up*0.5f)+
            (Vector3.forward*BulletSpace),Quaternion.identity);
        TheShootBullet.GetComponent<PlayerBulletController2>().NumberOfTargets=FindObjectOfType<RoomManager>().Targets.Length;
    }
    void FixedUpdate(){
        if (!DidWin){
            animator.SetInteger("State",State_Run);
            Body.MovePosition(transform.position+ Vector3.forward*Speed*Time.deltaTime);
        }
    }
    public void FinishRoom(){
        Destroy(TheShootBullet);
        Speed=FastSpeed;
    }
    void PlayerReset(){
        this.transform.position=FindObjectOfType<RoomManager>().EntrancePostion;
        Shoot();
    }
    void OnTriggerEnter(Collider other){
        if (other.CompareTag(Tag.Exit)){
            animator.SetInteger("State",State_Victory);
            Speed=initialSpeed;
            //ScenesManager.Instance.UnLoad(SceneManager.GetSceneByBuildIndex(ReachedRoom).name);
            ReachedRoom++;
            if (ReachedRoom==1){
                ScenesManager.Instance.UnLoad("Room 1");
                ScenesManager.Instance.Load("Room 2");
            }else if (ReachedRoom==2){
                ScenesManager.Instance.UnLoad("Room 2");
                ScenesManager.Instance.Load("Room 3");
            }
            else if (ReachedRoom==3){
                Debug.Log("Win");
            }
            //ScenesManager.Instance.Load(SceneManager.GetSceneByBuildIndex(ReachedRoom).name);
            PlayerReset();
        }
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag==(Tag.TargetBullet)||other.gameObject.tag==(Tag.PlayerBullet)){
            Debug.Log("Lose Game");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Destroy(other.gameObject);
        }
    }
}
