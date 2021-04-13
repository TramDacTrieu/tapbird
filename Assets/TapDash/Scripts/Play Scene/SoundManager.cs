using UnityEngine;
using System.Collections;
using Com.Studio2089.TapTapDash;

public class SoundManager : MonoBehaviour
{
	//this class will be manager your sound
    public static SoundManager instance = null;

    public AudioSource backGround;

    public AudioClip[] crystalTakeSounds;
    public AudioSource crystalTakeAudioSource;

    public AudioSource turnAudioSource;

    public AudioClip[] birdJumpSounds;
    public AudioClip[] catJumpSounds;
    public AudioClip[] bunnyJumpSounds;
    public AudioClip[] ratelJumpSounds;
    public AudioClip[] squirrelJumpSounds;
    public AudioClip[] pigJumpSounds;
    public AudioSource jumpAudioSource;

    public AudioClip[] fallSounds;
    public AudioSource fallAudioSource;

    public AudioSource levelFailAudioSource;
    public AudioSource pressedBtnAudioSource;
    public AudioSource levelWinAudioSource;
    public AudioSource heroUnlockAudioSource;

    [HideInInspector]
    public bool soundOn;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (GameData.Instance.SoundState == 1)
        {
            soundOn = true;
            SetMuteOnOff(false);
        }
        else
        {
            soundOn = false;
            SetMuteOnOff(true);
        }
    }

    private void SetMuteOnOff(bool state)
    {
        crystalTakeAudioSource.mute = state;
        turnAudioSource.mute = state;
        jumpAudioSource.mute = state;
        fallAudioSource.mute = state;
        levelFailAudioSource.mute = state;
        pressedBtnAudioSource.mute = state;
        levelWinAudioSource.mute = state;
        heroUnlockAudioSource.mute = state;
    }

    public void PlaySoundEffect(string sourceName)
    {
        switch (sourceName)
        {
            case Constants.CRYSTAL_TAKE_SOURCE_NAME:
                crystalTakeAudioSource.clip = crystalTakeSounds[(int)Mathf.Round(Random.Range(0.0f, crystalTakeSounds.Length - 1.0f))];
                crystalTakeAudioSource.Play();
                break;
            case Constants.TURN_SOURCE_NAME:
                turnAudioSource.Play();
                break;
            case Constants.BIRD_JUMP_SOURCE_NAME:
                jumpAudioSource.clip = birdJumpSounds[(int)Mathf.Round(Random.Range(0.0f, birdJumpSounds.Length - 1.0f))];
                jumpAudioSource.Play();
                break;
            case Constants.CAT_JUMP_SOURCE_NAME:
                jumpAudioSource.clip = catJumpSounds[(int)Mathf.Round(Random.Range(0.0f, catJumpSounds.Length - 1.0f))];
                jumpAudioSource.Play();
                break;
            case Constants.BUNNY_JUMP_SOURCE_NAME:
                jumpAudioSource.clip = bunnyJumpSounds[(int)Mathf.Round(Random.Range(0.0f, bunnyJumpSounds.Length - 1.0f))];
                jumpAudioSource.Play();
                break;
            case Constants.RATEL_JUMP_SOURCE_NAME:
                jumpAudioSource.clip = ratelJumpSounds[(int)Mathf.Round(Random.Range(0.0f, ratelJumpSounds.Length - 1.0f))];
                jumpAudioSource.Play();
                break;
            case Constants.SQUIRREL_JUMP_SOURCE_NAME:
                jumpAudioSource.clip = squirrelJumpSounds[(int)Mathf.Round(Random.Range(0.0f, squirrelJumpSounds.Length - 1.0f))];
                jumpAudioSource.Play();
                break;
            case Constants.PIG_JUMP_SOURCE_NAME:
                jumpAudioSource.clip = pigJumpSounds[(int)Mathf.Round(Random.Range(0.0f, pigJumpSounds.Length - 1.0f))];
                jumpAudioSource.Play();
                break;
            case Constants.FALL_SOURCE_NAME:
                fallAudioSource.clip = fallSounds[(int)Mathf.Round(Random.Range(0.0f, fallSounds.Length - 1.0f))];
                fallAudioSource.Play();
                break;
            case Constants.LEVEL_FAIL_SOURCE_NAME:
                levelFailAudioSource.Play();
                break;
            case Constants.BUTTON_SOURCE_NAME:
                pressedBtnAudioSource.Play();
                break;
            case Constants.LEVEL_WIN_SOURCE_NAME:
                levelWinAudioSource.Play();
                break;
            case Constants.HERO_UNLOCK_SOURCE_NAME:
                heroUnlockAudioSource.Play();
                break;
            case Constants.BG:
                backGround.Play();
                break;
        }
    }
}
