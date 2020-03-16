using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour{
    public float BulletSpace=1f;
    public GameObject Bullet;
    public GameObject TheShootBullet; // the clone bullet
    public bool DidShoot; 
    public GameObject Root; // Ragdall root
    private Animator animator;
    public Collider col;
    public ParticleSystem Dlood;
    void Start(){
        animator=GetComponent<Animator>();
    } 
     void Shoot(){
        if (!DidShoot){
            animator.SetBool("Shoot",true);
            TheShootBullet= Instantiate(Bullet,transform.position+(Vector3.up*0.5f)+
                (transform.forward*BulletSpace),Quaternion.identity);
            DidShoot=true;
        }
    }
    void OnTriggerStay(Collider other){
        // When player is near --> look at player
        if (other.CompareTag(Tag.Player)){
            transform.LookAt(other.transform,Vector3.zero*Time.deltaTime);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Shoot();
            }
        }
    }
    public void KillTarget(){
        Destroy(TheShootBullet);
        animator.enabled = false;
        col.enabled = false;
        Root.gameObject.SetActive(true);
        Dlood.Play();
        DidShoot=true;
        this.enabled = false;
    }
}
