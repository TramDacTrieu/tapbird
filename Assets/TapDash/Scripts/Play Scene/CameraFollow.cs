using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    [HideInInspector]
    public Transform targetTransform;

    [HideInInspector]
    public Vector3 finishLine;

    private Camera myCamera;

    private float timeStartedLerping;

    private Vector3 startPoint;

    private bool triggerZoomCamera = false;
    private bool isLerping = false;

    void Awake()
    {
        myCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (!GameController.instance.reachedAllLevel)
        {
            if (targetTransform != null)
            {
                Vector3 targetCamPos = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetCamPos, 0.5f);
            }
        }
        else
        {
            if (myCamera.orthographicSize > 9.0f)
            {
                myCamera.orthographicSize -= Time.deltaTime * 1.3f;
            }

            if (!triggerZoomCamera)
            {
                triggerZoomCamera = true;
                isLerping = true;
                timeStartedLerping = Time.time;
                startPoint = transform.position;
            }
        }
    }

    void FixedUpdate()
    {
        if (isLerping)
        {
            float timeSinceLerp = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceLerp / 1.0f;

            transform.position = Vector3.Lerp(startPoint, finishLine, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                isLerping = false;
                StartCoroutine(WaitSomeSeconds());
            }
        }
    }

    private IEnumerator WaitSomeSeconds()
    {
        yield return new WaitForSeconds(1.5f);
        GameController.instance.gameOverMenuNotifyText.text = "Play Again!";
        GameController.instance.gameOverMenu.SetActive(true);
    }
}
