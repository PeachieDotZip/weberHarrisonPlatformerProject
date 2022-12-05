using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpringScript : MonoBehaviour
{
    private Animator anim;
    private AudioSource Sprang;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Sprang = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("Spring");
            Sprang.Play();
        }
    }
}
