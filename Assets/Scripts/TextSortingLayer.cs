using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSortingLayer : MonoBehaviour
{
    public TextMeshPro R;
    // Start is called before the first frame update
    void Start()
    {
        R.sortingOrder = 11;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
