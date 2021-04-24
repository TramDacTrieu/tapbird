using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Com.Studio2089.TapTapDash;
using UnityEngine.EventSystems;

public enum GAMESTATE
{
    START, PLAY, PAUSE, OVER
}

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    public Color[] backgrounds;

    public MazeGenerator mazeGenerator;

    public GameObject[] characters;
    public GameObject gameOverMenu;
    public GameObject victoryEffect;
    public GameObject bullet;

    public Sprite arrowActive;

    public Text currentLevelText;
    public Text currentScoreText;

    public GameObject player;

    [HideInInspector]
    public GAMESTATE gameState;

    private Animator charAnim;

    public Text gameOverMenuNotifyText;

    [HideInInspector]
    public int indexBackgroundColor = 0;

    public float charSpeedRun = 7.0f;
    public float charSpeedRunToFinish = 3.0f;

    [HideInInspector]
    public int selectedCharacter = 1;

    //[HideInInspector]
    public int score = 0;
    public int coin = 0;
    [HideInInspector]
    public int maxScoreOfLevel = 0;

    [HideInInspector]
    public int currentLevel;

    [HideInInspector]
    public bool reachedAllLevel = false;

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

        GameData.Instance.Init();
        Application.targetFrameRate = 60;
        coin = GameData.Instance.Coin;
        score = GameData.Instance.Coin;
    }

    void Start()
    {
        // PlayerPrefs.DeleteAll();
        // Debug.Log(coin);
        // Initialize selected level
        int selectedLevel = 10;//GameData.Instance.SelectedLevel;
        // Set first level will be drawn on scene
        mazeGenerator.level = selectedLevel;
        mazeGenerator.GenerateMap();
        // Set second level will ve drawn on scene
        int nexteLevel = selectedLevel + 1;
        mazeGenerator.level = nexteLevel;
        // Set start point for second level to connect with first level
        mazeGenerator.levelsManager[nexteLevel - 1].startPointOfLevel = new Vector3(
            mazeGenerator.tileLastPosition.x, mazeGenerator.tileLastPosition.y + mazeGenerator.sizeOfPathTile * 2.0f - 0.1f, 0.0f);
        mazeGenerator.GenerateMap();

        // Initialize character of player
        InitializeCharacter();

        // Initialize background color
        //indexBackgroundColor = (int)Mathf.Round(Random.Range(0.0f, 6.0f));
        //Camera.main.GetComponent<Camera>().backgroundColor = backgrounds[indexBackgroundColor];
        currentLevel = selectedLevel;
        currentLevelText.text = currentLevel + " / " + (mazeGenerator.levelsManager.Length - 1);
        currentScoreText.text = "" + score;
        SetSpeedRun();
        gameState = GAMESTATE.START;
        StartCoroutine(WaitBeforeStartGame());

        //AdsControl.Instance.ShowBannerAds();
        SoundManager.instance.PlaySoundEffect(Constants.BG);
    }

    void Update()
    {
        SetSpeedRun();
    }

    private void InitializeCharacter()
    {
        selectedCharacter = 0;
        int[] temp = new int[characters.Length];
        int indexTemp = 0;
        int length = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            if (GameData.Instance.SelectedCharacters[i] == 1)
            {
                temp[indexTemp] = i;
                indexTemp++;
                length++;
            }
        }

        int[] indexCharacterArray = new int[length];
        for (int i = 0; i < length; i++)
        {
            indexCharacterArray[i] = temp[i];
        }
        selectedCharacter = indexCharacterArray[(int)Mathf.Round(Random.Range(0.0f, indexCharacterArray.Length - 1))];

        characters[selectedCharacter].SetActive(true);
        player = characters[selectedCharacter];

        // Add script 'PlayerController' to character of player
        player.AddComponent<PlayerController>();
        Camera.main.GetComponent<CameraFollow>().targetTransform = player.transform;
        charAnim = characters[selectedCharacter].GetComponent<Animator>();

        if (selectedCharacter == 0)
        {
            AdjustBtn_CharacterFace(0, new Vector2(95.0f, 122.0f), new Vector2(57.0f, 11.0f));
        }
        else if (selectedCharacter == 1)
        {
            AdjustBtn_CharacterFace(1, new Vector2(95.0f, 102.8f), new Vector2(57.0f, 4.0f));
        }
        else if (selectedCharacter == 2)
        {
            AdjustBtn_CharacterFace(2, new Vector2(95.0f, 152.5f), new Vector2(58.5f, 27.0f));
        }
        else if (selectedCharacter == 3)
        {
            AdjustBtn_CharacterFace(3, new Vector2(123.55f, 105.0f), new Vector2(56.5f, 3.0f));
        }
        else if (selectedCharacter == 4)
        {
            AdjustBtn_CharacterFace(4, new Vector2(95.0f, 105.25f), new Vector2(57.0f, 3.3f));
        }
        else if (selectedCharacter == 5)
        {
            AdjustBtn_CharacterFace(5, new Vector2(95.0f, 98.3f), new Vector2(57.0f, 0.0f));
        }
    }

    private void AdjustBtn_CharacterFace(int indexSprite, Vector2 sizeDelta, Vector2 anchoredPosition)
    {
        UIManager.instance.characterBtnFace.sprite = UIManager.instance.charactersUnlockedSprites[indexSprite];
        UIManager.instance.characterBtnFace.rectTransform.sizeDelta = sizeDelta;
        UIManager.instance.characterBtnFace.rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetSpeedRun()
    {
        if (score <= 0)
        {
            charSpeedRun = 7.0f;
        } else
        {
            charSpeedRun = GameController.instance.charSpeedRun;
        }
        //if (currentLevel == 1)
        //{
        //    charSpeedRun = 10.0f;
        //}
        //else if (currentLevel == 2)
        //{
        //    charSpeedRun = 9.5f;
        //}
        //else if (currentLevel == 3)
        //{
        //    charSpeedRun = 9.1f;
        //}
        //else
        //{
        //    charSpeedRun = 8.7f;
        //}
    }

    private IEnumerator WaitBeforeStartGame()
    {
        yield return new WaitForSeconds(1.0f);
        charAnim.enabled = true;
        gameState = GAMESTATE.PLAY;
        // Make player begins to move
        PlayerController pl = player.GetComponent<PlayerController>();
        pl.isLerping = true;
        pl.timeStartedLerp = Time.time;
        pl.startPointLerping = player.transform.position;
        pl.endPointLerping = player.transform.position + Vector3.up * pl.distanceRun;
    }
    public void spawmBullet()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Instantiate(GameController.instance.bullet, GameController.instance.player.transform.position, GameController.instance.player.transform.rotation);
        }
        
    }
}
