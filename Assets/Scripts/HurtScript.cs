using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtScript : MonoBehaviour
{
    private Collider2D Collider;
    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(RemoveCollider());
        }
    }
    public IEnumerator RemoveCollider()
    {
        Collider.enabled = false;
        yield return new WaitForSeconds(2.75f);
        Collider.enabled = true;
    }
}
