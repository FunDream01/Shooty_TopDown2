using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : MonoBehaviour
{
    [HideInInspector]
    public ParticleSystem explosion ;
    public TargetManager[]Targets;
    public float distance;
    public List<TargetManager> TargetsNear = new List<TargetManager>();
    private PlayerManager player;
    // Start is called before the first frame update
    void Start()
    {
        player=FindObjectOfType<PlayerManager>();
        explosion=transform.GetComponentInChildren<ParticleSystem>();
        Targets=FindObjectsOfType<TargetManager>();
        GetInactiveInRadius();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GetInactiveInRadius(){
        foreach (TargetManager target in Targets){
            if(Vector3.Distance(transform.position,target.transform.position) < distance){
               TargetsNear.Add(target);
            }
        }
    }
    void Explode(){
        GameObject _explosion= Instantiate(explosion.gameObject,transform.position,Quaternion.identity);
        _explosion.GetComponent<ParticleSystem>().Play();
        foreach(TargetManager target in TargetsNear){
            if (target!=null){

            target.KillTarget();
            }

        }
        if(player.NumberOfTargets>0){
            
            FindObjectOfType<PlayerManager>().StartGame(0.5f);
        }
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag==(Tag.PlayerBullet)){
            

            Destroy(other.gameObject);
            Explode();
        }
    }
}
