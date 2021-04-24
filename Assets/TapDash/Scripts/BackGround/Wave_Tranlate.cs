using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Tranlate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * 0.5f);
        if (transform.localPosition.x == 17f)
        {
            transform.localPosition = new Vector3(-16.96f, -4.61f);
        }
    }
}
