using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour{
    public bool RunningTarget; 
    public float RunningSpeed=0.5f;
    public float TimeBetweenShooting=5f;
    private float TimeRemain;
    public float BulletSpace=1f;
    public GameObject Bullet;
    public List<GameObject> ShootBullets = new List<GameObject>();// the clone bullet
    private Animator animator;
    //public ParticleSystem Dlood;
    private Collider[]colliders;
    private int State_Idle=0;
    private int State_Shoot=1;
    private int State_Running=2;
    private int State_Hit=3;
    private int State_Death=4;
    public ParticleSystem GunShot;
    private PlayerManager player;
    void Start(){
        player=FindObjectOfType<PlayerManager>();
        animator=GetComponent<Animator>();
        colliders=GetComponents<Collider>();
    } 
    void Shoot(){
        if (!FindObjectOfType<PlayerManager>().StopMoving){
            GunShot.Play();
            animator.SetInteger("State",State_Shoot);
            GameObject TheShootBullet= Instantiate(Bullet,transform.position+(Vector3.up*0.5f)+
                (transform.forward*BulletSpace),Quaternion.identity);
            TheShootBullet.transform.parent=this.transform.parent;
            ShootBullets.Add(TheShootBullet);
            //DidShoot=true;
        }
    }
    private IEnumerator Hit(PlayerManager player)
    {
        
        animator.SetInteger("State",State_Hit);
        player.StopMoving=true;
        yield return new WaitForSeconds(0.5f);
        player.Death();
        animator.SetInteger("State",State_Idle);
        
    }
    void OnTriggerStay(Collider other){
        
        player=FindObjectOfType<PlayerManager>();
        // When player is near --> look at player
        if (other.CompareTag(Tag.Player)&& !player.StopMoving){
            if(RunningTarget){
                RunToPlayer(other.gameObject);
            }else{
                LookToPlayer(other.gameObject);
            }
        }
    }
    void RunToPlayer(GameObject player){
        PlayerManager playerManager=player.GetComponent<PlayerManager>();
        if (playerManager.isDead==false){
            
            animator.SetInteger("State",State_Running);
            transform.LookAt(player.transform,Vector3.zero*Time.deltaTime);
            transform.Translate(Vector3.forward*Time.deltaTime*RunningSpeed);
            if(Vector3.Distance(player.transform.position,transform.position)<1f){
                //animator.SetInteger("State",State_Hit);
                StartCoroutine("Hit",playerManager);
                //playerManager.Death();
            }
            //Hit();
        }
    }
    void LookToPlayer(GameObject player){
        transform.LookAt(player.transform,Vector3.zero*Time.deltaTime);
        RaycastHit hit;
        if (Physics.Raycast(transform.position+(Vector3.up*0.5f), transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)){
            Debug.DrawRay(transform.position+(Vector3.up*0.5f), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.transform.CompareTag(Tag.Player)){
                if (TimeRemain<=0){
                    Shoot();
                    TimeRemain=TimeBetweenShooting;
                }else{
                    TimeRemain-=Time.deltaTime;
                }
            } 
        }
    }
    public void KillTarget(){
        foreach(Collider col in colliders){
            col.enabled=false;
        }
        foreach (GameObject bullet in ShootBullets){
            Destroy(bullet);   
        }
        animator.SetInteger("State",State_Death);
        FindObjectOfType<PlayerManager>().NumberOfTargets--;
        if (FindObjectOfType<PlayerManager>().NumberOfTargets==0){
            FindObjectOfType<PlayerManager>().FinishRoom();
        }
        Destroy(this);
    }
}
