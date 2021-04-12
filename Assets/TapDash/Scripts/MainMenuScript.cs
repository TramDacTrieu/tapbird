using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Com.Studio2089.TapTapDash;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject levelsMenu;
    public GameObject nextLvlBtn;
    public GameObject previousLvlBtn;

    public Animation lvlList_1_Anim;
    public Animation lvlList_2_Anim;
    public Animation lvlList_3_Anim;
    public Animation lvlList_4_Anim;
    public Animation lvlList_5_Anim;

    public Text playBtnTextInLevelList;

    public Sprite unlockedLevel;
    public Sprite[] soundSprites;

    public Image soundBtnImage;

    private Animation mainMenuAnim;
    private Animation optionsMenuAnim;
    private Animation levelsMenuAnim;
	public String iosRateUrl, androidRateUrl;

    public AudioSource audioSource;

    void Awake()
    {
        GameData.Instance.Init();

        mainMenuAnim = mainMenu.GetComponent<Animation>();
        optionsMenuAnim = optionsMenu.GetComponent<Animation>();
        levelsMenuAnim = levelsMenu.GetComponent<Animation>();
    }

    void Start()
    {
        //PlayerPrefs.DeleteAll();

        Init();

        /* Show buttons level unlocked or not */
        // List 1
        Animator[] childrens_list1 = lvlList_1_Anim.gameObject.GetComponentsInChildren<Animator>();
        SetUnlockedLevel(childrens_list1);

        // List 2
        Animator[] childrens_list2 = lvlList_2_Anim.gameObject.GetComponentsInChildren<Animator>();
        SetUnlockedLevel(childrens_list2);

        // List 3
        Animator[] childrens_list3 = lvlList_3_Anim.gameObject.GetComponentsInChildren<Animator>();
        SetUnlockedLevel(childrens_list3);

        // List 4
        Animator[] childrens_list4 = lvlList_4_Anim.gameObject.GetComponentsInChildren<Animator>();
        SetUnlockedLevel(childrens_list4);

        // List 5
        Animator[] childrens_list5 = lvlList_5_Anim.gameObject.GetComponentsInChildren<Animator>();
        SetUnlockedLevel(childrens_list5);

        AdsControl.Instance.HideBannerAds();
    }

    private void Init()
    {
        // Check sound on or off
        CheckSoundState();

        // Level 1 is the default unlocked
        GameData.Instance.StateLevels[0] = 1;
        GameData.Instance.SaveData("StateLevel1", GameData.Instance.StateLevels[0]);
        // displays first list by default
        GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 1);

        // Show levels list
        ShowLevelLsist();

        int highestLevel = GameData.Instance.HighestLevel;
        playBtnTextInLevelList.text = "Play Level " + highestLevel;
    }

    private void ShowLevelLsist()
    {
        if (GameData.Instance.MapBtnClickState == 0)
        {
            mainMenu.SetActive(true);
            nextLvlBtn.SetActive(true);
            previousLvlBtn.SetActive(false);
        }
        else
        {
            levelsMenu.SetActive(true);
            GameData.Instance.SaveData(Constants.MAP_BTN_CLICK_STATE, 0);

            int levelJustPlayed = GameData.Instance.LevelJustPlayed;
            if (1 <= levelJustPlayed && levelJustPlayed <= 12)
            {
                GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 1);
                lvlList_1_Anim.gameObject.SetActive(true);
                lvlList_1_Anim.Play("level_list_1_fade_in");

                lvlList_2_Anim.gameObject.SetActive(false);
                lvlList_3_Anim.gameObject.SetActive(false);
                nextLvlBtn.SetActive(true);
                previousLvlBtn.SetActive(false);
            }
            else if (13 <= levelJustPlayed && levelJustPlayed <= 24)
            {
                GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 2);
                lvlList_2_Anim.gameObject.SetActive(true);
                lvlList_2_Anim.Play("other_level_list_fade_in_right");

                lvlList_1_Anim.gameObject.SetActive(false);
                lvlList_3_Anim.gameObject.SetActive(false);
                nextLvlBtn.SetActive(true);
                previousLvlBtn.SetActive(true);
            }
            else if (25 <= levelJustPlayed && levelJustPlayed <= 36)
            {
                GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 3);
                lvlList_3_Anim.gameObject.SetActive(true);
                lvlList_3_Anim.Play("other_level_list_fade_in_right");

                lvlList_1_Anim.gameObject.SetActive(false);
                lvlList_2_Anim.gameObject.SetActive(false);
                nextLvlBtn.SetActive(true);
                previousLvlBtn.SetActive(true);
            }
            else if (37 <= levelJustPlayed && levelJustPlayed <= 48)
            {
                GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 4);
                lvlList_4_Anim.gameObject.SetActive(true);
                lvlList_4_Anim.Play("other_level_list_fade_in_right");

                lvlList_1_Anim.gameObject.SetActive(false);
                lvlList_2_Anim.gameObject.SetActive(false);
                lvlList_3_Anim.gameObject.SetActive(false);
                nextLvlBtn.SetActive(true);
                previousLvlBtn.SetActive(true);
            }
            else if (49 <= levelJustPlayed && levelJustPlayed <= 50)
            {
                GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 5);
                lvlList_5_Anim.gameObject.SetActive(true);
                lvlList_5_Anim.Play("other_level_list_fade_in_right");

                lvlList_1_Anim.gameObject.SetActive(false);
                lvlList_2_Anim.gameObject.SetActive(false);
                lvlList_3_Anim.gameObject.SetActive(false);
                lvlList_4_Anim.gameObject.SetActive(false);
                nextLvlBtn.SetActive(false);
                previousLvlBtn.SetActive(true);
            }
        }
    }

    private void CheckSoundState()
    {
        if (GameData.Instance.SoundState == 1)
        {
            soundBtnImage.sprite = soundSprites[1];
            audioSource.mute = false;
        }
        else
        {
            soundBtnImage.sprite = soundSprites[0];
            audioSource.mute = true;
        }
    }

    private void SetUnlockedLevel(Animator[] childrens_list)
    {
        string highestLevel = GameData.Instance.HighestLevel + "";
        for (int i = 0; i < childrens_list.Length; i++)
        {
            int indexStateLevel = Int32.Parse(childrens_list[i].name);
            if (GameData.Instance.StateLevels[indexStateLevel - 1] == 1)
            {
                childrens_list[i].GetComponentsInChildren<Image>()[1].sprite = unlockedLevel;
                if (childrens_list[i].name == highestLevel)
                {
                    childrens_list[i].GetComponentsInChildren<Image>()[2].enabled = true;
                    childrens_list[i].GetComponentsInChildren<Image>()[2].GetComponentsInChildren<Image>()[1].enabled = true;
                }
                else
                {
                    childrens_list[i].GetComponentsInChildren<Image>()[2].enabled = false;
                    childrens_list[i].GetComponentsInChildren<Image>()[2].GetComponentsInChildren<Image>()[1].enabled = false;
                }

                int indexStateMaxScore = Int32.Parse(childrens_list[i].name);
                if (GameData.Instance.StateMaxScore[indexStateMaxScore - 1] == 1)
                {
                    childrens_list[i].GetComponentsInChildren<Image>()[4].enabled = true;
                }
                else
                {
                    childrens_list[i].GetComponentsInChildren<Image>()[4].enabled = false;
                }
            }
        }
    }

    private void PlaySoundBtn()
    {
        audioSource.Play();
    }

    public void OptionBtn_Onclick()
    {
        mainMenuAnim.Play("main_menu_fade_out_right");
        if (!optionsMenu.activeInHierarchy)
        {
            optionsMenu.SetActive(true);
        }
        else
        {
            optionsMenuAnim.Play("options_fade_in_right");
        }
        PlaySoundBtn();
    }

    public void BackOptionBtn_Onclick()
    {
        optionsMenuAnim.Play("options_fade_out_left");
        mainMenuAnim.Play("main_menu_fade_in_right");
        PlaySoundBtn();
    }
		

    public void SoundBtn_Onclick()
    {
        if (GameData.Instance.SoundState == 1)
        {
            GameData.Instance.SaveData(Constants.SOUND_STATE, 0);
            soundBtnImage.sprite = soundSprites[0];
            audioSource.mute = true;
        }
        else
        {
            GameData.Instance.SaveData(Constants.SOUND_STATE, 1);
            soundBtnImage.sprite = soundSprites[1];
            audioSource.mute = false;
        }
        PlaySoundBtn();
    }

    public void BackCreditBtn_Onclick()
    {
        optionsMenuAnim.Play("options_fade_in_left");
        PlaySoundBtn();
    }

    public void ContinueBtn_Onclick()
    {
        mainMenuAnim.Play("main_menu_fade_out_left");
        if (!levelsMenu.activeInHierarchy)
        {
            levelsMenu.SetActive(true);
        }
        else
        {
            levelsMenuAnim.Play("levels_menu_fade_in");
        }
        PlaySoundBtn();
    }

    public void MenuBtn_Onclick()
    {
        int lvlListShowing = GameData.Instance.LvlListShowing;
        if (lvlListShowing == 2)
        {
            lvlList_1_Anim.gameObject.SetActive(false);
        }
        else if (lvlListShowing == 3)
        {
            lvlList_1_Anim.gameObject.SetActive(false);
            lvlList_2_Anim.gameObject.SetActive(false);
        }
        else if (lvlListShowing == 4)
        {
            lvlList_1_Anim.gameObject.SetActive(false);
            lvlList_2_Anim.gameObject.SetActive(false);
            lvlList_3_Anim.gameObject.SetActive(false);
        }
        else if (lvlListShowing == 5)
        {
            lvlList_1_Anim.gameObject.SetActive(false);
            lvlList_2_Anim.gameObject.SetActive(false);
            lvlList_3_Anim.gameObject.SetActive(false);
            lvlList_4_Anim.gameObject.SetActive(false);
        }

        levelsMenuAnim.Play("levels_menu_fade_out");
        if (!mainMenu.activeInHierarchy)
        {
            mainMenu.SetActive(true);
        }
        else
        {
            mainMenuAnim.Play("main_menu_fade_in_left");
        }
        PlaySoundBtn();
    }

    public void NextLvlBtn_Onclick()
    {
        int lvlListShowing = GameData.Instance.LvlListShowing;
        if (lvlListShowing == 1)
        {
            lvlList_1_Anim.Play("level_list_1_fade_out");
            if (!lvlList_2_Anim.gameObject.activeInHierarchy)
            {
                lvlList_2_Anim.gameObject.SetActive(true);
            }
            lvlList_2_Anim.Play("other_level_list_fade_in_right");
            GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 2);
            previousLvlBtn.SetActive(true);
        }
        else if (lvlListShowing == 2)
        {
            lvlList_2_Anim.Play("other_level_list_fade_out_left");
            if (!lvlList_3_Anim.gameObject.activeInHierarchy)
            {
                lvlList_3_Anim.gameObject.SetActive(true);
            }
            lvlList_3_Anim.Play("other_level_list_fade_in_right");
            GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 3);
            nextLvlBtn.SetActive(true);
            previousLvlBtn.SetActive(true);
        }
        else if (lvlListShowing == 3)
        {
            lvlList_3_Anim.Play("other_level_list_fade_out_left");
            if (!lvlList_4_Anim.gameObject.activeInHierarchy)
            {
                lvlList_4_Anim.gameObject.SetActive(true);
            }
            lvlList_4_Anim.Play("other_level_list_fade_in_right");
            GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 4);
            nextLvlBtn.SetActive(true);
            previousLvlBtn.SetActive(true);
        }
        else if (lvlListShowing == 4)
        {
            lvlList_4_Anim.Play("other_level_list_fade_out_left");
            if (!lvlList_5_Anim.gameObject.activeInHierarchy)
            {
                lvlList_5_Anim.gameObject.SetActive(true);
            }
            lvlList_5_Anim.Play("other_level_list_fade_in_right");
            GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 5);
            nextLvlBtn.SetActive(false);
            previousLvlBtn.SetActive(true);
        }
        PlaySoundBtn();
    }

    public void PreviousLvlBtn_Onclick()
    {
        int lvlListShowing = GameData.Instance.LvlListShowing;
        if (lvlListShowing == 2)
        {
            lvlList_2_Anim.Play("other_level_list_fade_out_right");
            if (!lvlList_1_Anim.gameObject.activeInHierarchy)
            {
                lvlList_1_Anim.gameObject.SetActive(true);
            }
            lvlList_1_Anim.Play("level_list_1_fade_in");
            GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 1);
            nextLvlBtn.SetActive(true);
            previousLvlBtn.SetActive(false);
        }
        else if (lvlListShowing == 3)
        {
            lvlList_3_Anim.Play("other_level_list_fade_out_right");
            if (!lvlList_2_Anim.gameObject.activeInHierarchy)
            {
                lvlList_2_Anim.gameObject.SetActive(true);
            }
            lvlList_2_Anim.Play("other_level_list_fade_in_left");
            GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 2);
            nextLvlBtn.SetActive(true);
            previousLvlBtn.SetActive(true);
        }
        else if (lvlListShowing == 4)
        {
            lvlList_4_Anim.Play("other_level_list_fade_out_right");
            if (!lvlList_3_Anim.gameObject.activeInHierarchy)
            {
                lvlList_3_Anim.gameObject.SetActive(true);
            }
            lvlList_3_Anim.Play("other_level_list_fade_in_left");
            GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 3);
            nextLvlBtn.SetActive(true);
            previousLvlBtn.SetActive(true);
        }
        else if (lvlListShowing == 5)
        {
            lvlList_5_Anim.Play("other_level_list_fade_out_right");
            if (!lvlList_4_Anim.gameObject.activeInHierarchy)
            {
                lvlList_4_Anim.gameObject.SetActive(true);
            }
            lvlList_4_Anim.Play("other_level_list_fade_in_left");
            GameData.Instance.SaveData(Constants.LVL_LIST_SHOWING, 4);
            nextLvlBtn.SetActive(true);
            previousLvlBtn.SetActive(true);
        }
        PlaySoundBtn();
    }

    public void PlayHighestLevelBtn_Onclick()
    {
        PlaySoundBtn();

        int highestLevel = GameData.Instance.HighestLevel;
        GameData.Instance.SaveData(Constants.SELECTED_LEVEL, highestLevel);
        SceneManager.LoadScene("Play");
    }

    public void PlaySelectedLevel(int level)
    {
        PlaySoundBtn();

        if (GameData.Instance.StateLevels[level - 1] == 1)
        {
            GameData.Instance.SaveData(Constants.SELECTED_LEVEL, level);
            SceneManager.LoadScene("Play");
        }
    }

    // Rate us Link
    public void RateUsBtn_Onlick()
    {
        // ....
		#if UNITY_ANDROID
		Application.OpenURL (androidRateUrl);
		#elif UNITY_IPHONE
		Application.OpenURL (iosRateUrl);
		#endif
    }
}
