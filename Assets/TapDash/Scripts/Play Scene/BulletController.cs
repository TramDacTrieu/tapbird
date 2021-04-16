using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed = 25.5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitBeforeDestroy());
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(GameController.instance.player.transform.up * speed * Time.deltaTime);
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    private IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
