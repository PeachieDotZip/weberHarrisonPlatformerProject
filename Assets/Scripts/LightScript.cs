using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    public float lightRadius;
    public GameObject lightIndicator;
    private GameObject Player;
    public Animator lightAnim;
    public Animator indicatorAnim;
    private CharacterController2D Pcontroller;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        Pcontroller = Player.GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector2.Distance(Player.transform.position, transform.position);

        //print("distance " + dist);

        if (dist <= lightRadius && Pcontroller.canChomp == true)
        {
            indicatorAnim.SetBool("On", true);
            //^^^ Animates indicator to turn on
            Vector3 offset = Player.transform.position - transform.position;

            transform.rotation = Quaternion.LookRotation(
                                   Vector3.forward, // Keep z+ pointing straight into the screen.
                                   offset           // Point y+ toward the target.
                                 );
            Pcontroller.target = gameObject;
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(Pcontroller.LightChomp());
            }
        }
        else
        {
            indicatorAnim.SetBool("On", false);
            //^^^ Animates indicator to turn off
        }

    }

    //Radius Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lightRadius);
    }
}
