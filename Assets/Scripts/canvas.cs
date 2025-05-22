using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvas : MonoBehaviour
{
    public Canvas a;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CanvasOff());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CanvasOff()
    {
        yield return new WaitForSeconds(2);
        a.enabled = false;
    }
}
