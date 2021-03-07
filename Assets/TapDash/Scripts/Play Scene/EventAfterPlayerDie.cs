using UnityEngine;
using System.Collections;
using Com.Studio2089.TapTapDash;

public class EventAfterPlayerDie : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Animation event
    public void DieDone()
    {
        anim.enabled = false;
        GameController.instance.gameOverMenu.SetActive(true);
        SoundManager.instance.PlaySoundEffect(Constants.LEVEL_FAIL_SOURCE_NAME);
    }
}
