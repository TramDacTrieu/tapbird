using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroductionScript : MonoBehaviour
{
    public float waitTime = 2.0f;

    void Start()
    {
        StartCoroutine(Waitting(waitTime));
    }

    IEnumerator Waitting(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainMenu");
    }
}
