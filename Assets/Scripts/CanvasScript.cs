using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    public Transform RespawnPoint;
    public GameObject Player;
    public Animator UIanim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        Player.transform.position = RespawnPoint.position;
        UIanim.ResetTrigger("Respawn");
    }
}
