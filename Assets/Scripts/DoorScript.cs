using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator UIanim;
    public Transform destination;
    public GameObject Player;
    private Collider2D Collider;
    public CanvasScript CS;
    public int DoorID;
    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        {
            if (collision.gameObject.CompareTag("Player") && DoorID == 0)
            {
                Collider.enabled = false;
                StartCoroutine(EnterDoor());
            }
            if (collision.gameObject.CompareTag("Player") && DoorID == 1)
            {
                Collider.enabled = false;
                StartCoroutine(EndGame());
            }
        }
    }

    public IEnumerator EnterDoor()
    {
        UIanim.SetTrigger("EnterDoor");
        yield return new WaitForSeconds(1f);
        Player.transform.position = destination.transform.position;
        CS.RespawnPoint = destination;
    }
    public IEnumerator EndGame()
    {
        UIanim.SetTrigger("LightFlash");
        yield return new WaitForSeconds(1f);
        Player.transform.position = destination.transform.position;
        CS.RespawnPoint = destination;
    }
}
