using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TargetBulletController : MonoBehaviour{
    public float Speed;
    private Rigidbody Body;
    private GameObject Player;
    void Start(){
        Body=FindObjectOfType<Rigidbody>();
        Player=GameObject.FindGameObjectWithTag(Tag.Player);
    }
    void Update(){
        Body.MovePosition(Vector3.MoveTowards(
            transform.position,Player.transform.position+(Vector3.up*0.5f),Speed*Time.deltaTime));
        
        transform.LookAt(Player.transform,Vector3.zero*Time.deltaTime);
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag==(Tag.Player)){
            Debug.Log("Lose Game");
            Destroy(this.gameObject);
        }
    }
}
