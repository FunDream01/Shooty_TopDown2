using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour{
    public float BulletSpace=1f;
    public GameObject Bullet;
    public GameObject TheShootBullet; // the clone bullet
    public bool DidShoot; 
    private Animator animator;
    public ParticleSystem Dlood;
    public Collider[]colliders;
    private int State_Idle=0;
    private int State_Shoot=1;
    private int State_Death=2;
    public ParticleSystem GunShot;
    void Start(){
        animator=GetComponent<Animator>();
        colliders=GetComponents<Collider>();
    } 
     void Shoot(){
        if (!DidShoot){
            
            GunShot.Play();
            animator.SetInteger("State",State_Shoot);
            TheShootBullet= Instantiate(Bullet,transform.position+(Vector3.up*0.5f)+
                (transform.forward*BulletSpace),Quaternion.identity);
            TheShootBullet.transform.parent=this.transform.parent;
            DidShoot=true;
        }
    }
    void OnTriggerStay(Collider other){
        // When player is near --> look at player
        if (other.CompareTag(Tag.Player)){
            transform.LookAt(other.transform,Vector3.zero*Time.deltaTime);
            RaycastHit hit;
            if (Physics.Raycast(transform.position+(Vector3.up*0.5f), transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position+(Vector3.up*0.5f), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                if (hit.transform.CompareTag(Tag.Player))
                    Shoot();
            }
        }
    }
    public void KillTarget(){
        foreach(Collider col in colliders){
            col.enabled=false;
        }
        Destroy(TheShootBullet);
        animator.SetInteger("State",State_Death);
        Dlood.Play();
        DidShoot=true;
        this.enabled = false;
    }
}
