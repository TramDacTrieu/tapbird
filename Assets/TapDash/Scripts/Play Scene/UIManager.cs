using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Com.Studio2089.TapTapDash;

public class UIManager : MonoBehaviour
{
	//this class will manager UI element
    public static UIManager instance = null;

    public GameObject gameOverMenu;
    public GameObject characterMenu;

    public Animation gameOverMenuAnim;
    public Animation characterMenuAnim;

    public Sprite[] charactersUnlockedSprites;

    public Image characterBtnFace;
    public Image checkedCharacter1;

    public Image character2FaceSprite;
    public Image checkedCharacter2;

    public Image character3FaceSprite;
    public Image checkedCharacter3;

    public Image character4FaceSprite;
    public Image checkedCharacter4;

    public Image character5FaceSprite;
    public Image checkedCharacter5;

    public Image character6FaceSprite;
    public Image checkedCharacter6;

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
        GameData.Instance.SaveData("StateCharacter1", 1);
        GameData.Instance.StateCharacters[0] = 1;
        if (GameData.Instance.StateCharacters[1] == 0 && GameData.Instance.StateCharacters[2] == 0
            && GameData.Instance.StateCharacters[3] == 0 && GameData.Instance.StateCharacters[4] == 0
            && GameData.Instance.StateCharacters[5] == 0)
        {
            GameData.Instance.SaveData("SelectedCharacter1", 1);
            GameData.Instance.SelectedCharacters[0] = 1;
        }

        /* Check if character is unlocked and checked */
        UnlockedAndChecked();
    }

    private void UnlockedAndChecked()
    {
        if (GameData.Instance.SelectedCharacters[0] == 1)
        {
            checkedCharacter1.enabled = true;
        }
        else
        {
            checkedCharacter1.enabled = false;
        }

        if (GameData.Instance.StateCharacters[1] == 1)
        {
            character2FaceSprite.sprite = charactersUnlockedSprites[1];
            if (GameData.Instance.SelectedCharacters[1] == 1) checkedCharacter2.enabled = true;
            else
            {
                checkedCharacter2.enabled = false;
            }
        }

        if (GameData.Instance.StateCharacters[2] == 1)
        {
            character3FaceSprite.sprite = charactersUnlockedSprites[2];
            if (GameData.Instance.SelectedCharacters[2] == 1) checkedCharacter3.enabled = true;
            else
            {
                checkedCharacter3.enabled = false;
            }
        }

        if (GameData.Instance.StateCharacters[3] == 1)
        {
            character4FaceSprite.sprite = charactersUnlockedSprites[3];
            if (GameData.Instance.SelectedCharacters[3] == 1) checkedCharacter4.enabled = true;
            else
            {
                checkedCharacter4.enabled = false;
            }
        }

        if (GameData.Instance.StateCharacters[4] == 1)
        {
            character5FaceSprite.sprite = charactersUnlockedSprites[4];
            if (GameData.Instance.SelectedCharacters[4] == 1) checkedCharacter5.enabled = true;
            else
            {
                checkedCharacter5.enabled = false;
            }
        }

        if (GameData.Instance.StateCharacters[5] == 1)
        {
            character6FaceSprite.sprite = charactersUnlockedSprites[5];
            if (GameData.Instance.SelectedCharacters[5] == 1) checkedCharacter6.enabled = true;
            else
            {
                checkedCharacter6.enabled = false;
            }
        }
    }

    public void RestartGame()
    {
        SoundManager.instance.PlaySoundEffect(Constants.BUTTON_SOURCE_NAME);
        SceneManager.LoadScene("Play");
    }

    public void MapBtn_Onclick()
    {
        SoundManager.instance.PlaySoundEffect(Constants.BUTTON_SOURCE_NAME);
        GameData.Instance.SaveData(Constants.MAP_BTN_CLICK_STATE, 1);
        SceneManager.LoadScene("MainMenu");
    }

    public void CharacterBtn_Onclick()
    {
        gameOverMenuAnim.Play("game_over_fade_out");
        if (!characterMenu.activeInHierarchy)
        {
            characterMenu.SetActive(true);
        }
        else
        {
            characterMenuAnim.Play("character_menu_fade_in");
        }
        SoundManager.instance.PlaySoundEffect(Constants.BUTTON_SOURCE_NAME);
    }

    public void BackBtn_Onclick()
    {
        characterMenuAnim.Play("character_menu_fade_out");
        gameOverMenuAnim.Play("game_over_fade_in");
        SoundManager.instance.PlaySoundEffect(Constants.BUTTON_SOURCE_NAME);
    }

    public void SelectCharacterBtn_Onclick(int i)
    {
        if (GameData.Instance.StateCharacters[i - 1] == 1)
        {
            switch (i)
            {
                case 1:
                    if (GameData.Instance.SelectedCharacters[0] == 1 &&
                        (GameData.Instance.SelectedCharacters[1] == 1 || GameData.Instance.SelectedCharacters[2] == 1
                        || GameData.Instance.SelectedCharacters[3] == 1 || GameData.Instance.SelectedCharacters[4] == 1
                        || GameData.Instance.SelectedCharacters[5] == 1))
                    {
                        checkedCharacter1.enabled = false;
                        SelectCharacter(0, 0, "SelectedCharacter1");
                        SoundManager.instance.PlaySoundEffect(Constants.BUTTON_SOURCE_NAME);
                    }
                    else
                    {
                        checkedCharacter1.enabled = true;
                        SelectCharacter(0, 1, "SelectedCharacter1");
                        SoundManager.instance.PlaySoundEffect(Constants.BIRD_JUMP_SOURCE_NAME);
                    }
                    break;
                case 2:
                    if (GameData.Instance.SelectedCharacters[1] == 1 &&
                    (GameData.Instance.SelectedCharacters[0] == 1 || GameData.Instance.SelectedCharacters[2] == 1
                    || GameData.Instance.SelectedCharacters[3] == 1 || GameData.Instance.SelectedCharacters[4] == 1
                    || GameData.Instance.SelectedCharacters[5] == 1))
                    {
                        checkedCharacter2.enabled = false;
                        SelectCharacter(1, 0, "SelectedCharacter2");
                        SoundManager.instance.PlaySoundEffect(Constants.BUTTON_SOURCE_NAME);
                    }
                    else
                    {
                        checkedCharacter2.enabled = true;
                        SelectCharacter(1, 1, "SelectedCharacter2");
                        SoundManager.instance.PlaySoundEffect(Constants.CAT_JUMP_SOURCE_NAME);
                    }
                    break;
                case 3:
                    if (GameData.Instance.SelectedCharacters[2] == 1 &&
                    (GameData.Instance.SelectedCharacters[0] == 1 || GameData.Instance.SelectedCharacters[1] == 1
                    || GameData.Instance.SelectedCharacters[3] == 1 || GameData.Instance.SelectedCharacters[4] == 1
                    || GameData.Instance.SelectedCharacters[5] == 1))
                    {
                        checkedCharacter3.enabled = false;
                        SelectCharacter(2, 0, "SelectedCharacter3");
                        SoundManager.instance.PlaySoundEffect(Constants.BUTTON_SOURCE_NAME);
                    }
                    else
                    {
                        checkedCharacter3.enabled = true;
                        SelectCharacter(2, 1, "SelectedCharacter3");
                        SoundManager.instance.PlaySoundEffect(Constants.BUNNY_JUMP_SOURCE_NAME);
                    }
                    break;
                case 4:
                    if (GameData.Instance.SelectedCharacters[3] == 1 &&
                    (GameData.Instance.SelectedCharacters[0] == 1 || GameData.Instance.SelectedCharacters[1] == 1
                    || GameData.Instance.SelectedCharacters[2] == 1 || GameData.Instance.SelectedCharacters[4] == 1
                    || GameData.Instance.SelectedCharacters[5] == 1))
                    {
                        checkedCharacter4.enabled = false;
                        SelectCharacter(3, 0, "SelectedCharacter4");
                        SoundManager.instance.PlaySoundEffect(Constants.BUTTON_SOURCE_NAME);
                    }
                    else
                    {
                        checkedCharacter4.enabled = true;
                        SelectCharacter(3, 1, "SelectedCharacter4");
                        SoundManager.instance.PlaySoundEffect(Constants.RATEL_JUMP_SOURCE_NAME);
                    }
                    break;
                case 5:
                    if (GameData.Instance.SelectedCharacters[4] == 1 &&
                    (GameData.Instance.SelectedCharacters[0] == 1 || GameData.Instance.SelectedCharacters[1] == 1
                    || GameData.Instance.SelectedCharacters[2] == 1 || GameData.Instance.SelectedCharacters[3] == 1
                    || GameData.Instance.SelectedCharacters[5] == 1))
                    {
                        checkedCharacter5.enabled = false;
                        SelectCharacter(4, 0, "SelectedCharacter5");
                        SoundManager.instance.PlaySoundEffect(Constants.BUTTON_SOURCE_NAME);
                    }
                    else
                    {
                        checkedCharacter5.enabled = true;
                        SelectCharacter(4, 1, "SelectedCharacter5");
                        SoundManager.instance.PlaySoundEffect(Constants.SQUIRREL_JUMP_SOURCE_NAME);
                    }
                    break;
                case 6:
                    if (GameData.Instance.SelectedCharacters[5] == 1 &&
                    (GameData.Instance.SelectedCharacters[0] == 1 || GameData.Instance.SelectedCharacters[1] == 1
                    || GameData.Instance.SelectedCharacters[2] == 1 || GameData.Instance.SelectedCharacters[3] == 1
                    || GameData.Instance.SelectedCharacters[4] == 1))
                    {
                        checkedCharacter6.enabled = false;
                        SelectCharacter(5, 0, "SelectedCharacter6");
                        SoundManager.instance.PlaySoundEffect(Constants.BUTTON_SOURCE_NAME);
                    }
                    else
                    {
                        checkedCharacter6.enabled = true;
                        SelectCharacter(5, 1, "SelectedCharacter6");
                        SoundManager.instance.PlaySoundEffect(Constants.PIG_JUMP_SOURCE_NAME);
                    }
                    break;

            }
        }
        else
        {
            SoundManager.instance.PlaySoundEffect(Constants.BUTTON_SOURCE_NAME);
        }
    }

    private void SelectCharacter(int indexSelected, int valueSelected, string key)
    {
        GameData.Instance.SelectedCharacters[indexSelected] = valueSelected;
        GameData.Instance.SaveData(key, valueSelected);
    }
}
