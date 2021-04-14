using UnityEngine;
using System;
using System.Collections;
using Com.Studio2089.TapTapDash;

public class PlayerController : MonoBehaviour
{
    private GameObject bodyCharacter;

    private Animator charAnim;

    [HideInInspector]
    public Vector3 startPointLerping;
    [HideInInspector]
    public Vector3 endPointLerping;

    [HideInInspector]
    public float distanceRun = 100.0f;

    [HideInInspector]
    public float timeStartedLerp;

    private float speedLerp;

    private int maxLevelLength;

    private int jumpHash;
    private int dieHash;
    private int layerMaskArrow;
    private int layerMaskWalkable;

    private int timeMeetArrow = 0;

    [HideInInspector]
    public bool isLerping;

    private bool isDie = false;
    private bool canBeMove = false;
    private bool canBeJump = false;
    private bool isJumping = false;
    private bool canGoAhead = false;
    private bool canTurnLeft = false;
    private bool canTurnRight = false;
    private bool goAhead = false;
    private bool turnLeft = false;
    private bool turnRight = false;
    private bool outMap = false;

    private bool canBeTap = true;

    void Awake()
    {
        bodyCharacter = GameObject.FindGameObjectWithTag("Body");
        charAnim = GetComponent<Animator>();
    }

    void Start()
    {
        maxLevelLength = GameController.instance.mazeGenerator.levelsManager.Length;
        jumpHash = Animator.StringToHash("Jump");
        dieHash = Animator.StringToHash("Die");
        layerMaskArrow = LayerMask.GetMask("Arrow");
        layerMaskWalkable = LayerMask.GetMask("Walkable");

        speedLerp = GameController.instance.charSpeedRun;
    }

    void Update()
    {
        if (!isDie)
        {
            Move();
            CheckOutOfPath();
            CheckBendAndLeap();
            if(GameController.instance.score % 50 == 0)
            {
                GameController.instance.charSpeedRun -= 1.0f;
            }

            if (Input.GetMouseButtonDown(0) && canBeTap)
            {
                if (canBeMove)
                {
                    timeStartedLerp = Time.time;
                    startPointLerping = transform.position;
                    if (canGoAhead)
                    {
                        endPointLerping = transform.position + Vector3.up * distanceRun;
                        goAhead = true;
                    }
                    else
                    {
                        if (canTurnLeft)
                        {
                            endPointLerping = transform.position - Vector3.right * distanceRun;
                            turnLeft = true;
                        }
                        else
                        {
                            endPointLerping = transform.position + Vector3.right * distanceRun;
                            turnRight = true;
                        }
                    }
                    SoundManager.instance.PlaySoundEffect(Constants.TURN_SOURCE_NAME);
                }
                if (canBeJump)
                {
                    isJumping = true;
                    canBeJump = canBeMove = false;
                    charAnim.SetBool(jumpHash, true);
                    StartCoroutine(Jumping());
                    PlaySoundJump();
                }
            }

            // Handle smooth lerp
            SmoothLerp();

            // Player came to finish line
            if (GameController.instance.gameState == GAMESTATE.PLAY)
            {
                if (!isLerping && !GameController.instance.reachedAllLevel)
                {
                    isLerping = true;
                    timeStartedLerp = Time.time;
                    startPointLerping = transform.position;
                    endPointLerping = transform.position + Vector3.up * distanceRun;
                    speedLerp = GameController.instance.charSpeedRun;
                    canBeTap = true;
                }
            }
        }
    }

    private void PlaySoundJump()
    {
        switch (GameController.instance.selectedCharacter)
        {
            case 0:
                SoundManager.instance.PlaySoundEffect(Constants.BIRD_JUMP_SOURCE_NAME); break;
            case 1:
                SoundManager.instance.PlaySoundEffect(Constants.CAT_JUMP_SOURCE_NAME);  break;
            case 2:
                SoundManager.instance.PlaySoundEffect(Constants.BUNNY_JUMP_SOURCE_NAME);    break;
            case 3:
                SoundManager.instance.PlaySoundEffect(Constants.RATEL_JUMP_SOURCE_NAME);    break;
            case 4:
                SoundManager.instance.PlaySoundEffect(Constants.SQUIRREL_JUMP_SOURCE_NAME);    break;
            case 5:
                SoundManager.instance.PlaySoundEffect(Constants.PIG_JUMP_SOURCE_NAME);    break;
        }
    }

    private void SmoothLerp()
    {
        if (goAhead)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), 0.5f);
            if (transform.rotation == Quaternion.Euler(0.0f, 0.0f, 0.0f))
            {
                goAhead = false;
            }
            if (Camera.main.transform.rotation != Quaternion.Euler(0.0f, 0.0f, 0.0f))
            {
                HandleCamera(Quaternion.Euler(0.0f, 0.0f, 0.0f));
            }
        }
        if (turnLeft)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 90.0f), 0.5f);
            if (transform.rotation == Quaternion.Euler(0.0f, 0.0f, 90.0f))
            {
                turnLeft = false;
            }
            if (3 <= GameController.instance.currentLevel && GameController.instance.currentLevel <= 7
                || 25 <= GameController.instance.currentLevel && GameController.instance.currentLevel <= 30
                || 45 <= GameController.instance.currentLevel && GameController.instance.currentLevel < maxLevelLength)
            {
                HandleCamera(Quaternion.Euler(0.0f, 0.0f, 45.0f));
            }
            else if (8 <= GameController.instance.currentLevel && GameController.instance.currentLevel <= 12
                || 16 <= GameController.instance.currentLevel && GameController.instance.currentLevel <= 21)
            {
                HandleCamera(Quaternion.Euler(0.0f, 0.0f, -45.0f));
            }
        }
        if (turnRight)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, -90.0f), 0.5f);
            if (transform.rotation == Quaternion.Euler(0.0f, 0.0f, -90.0f))
            {
                turnRight = false;
            }
            if (3 <= GameController.instance.currentLevel && GameController.instance.currentLevel <= 7
                || 16 <= GameController.instance.currentLevel && GameController.instance.currentLevel <= 21
                || 45 <= GameController.instance.currentLevel && GameController.instance.currentLevel < maxLevelLength)
            {
                HandleCamera(Quaternion.Euler(0.0f, 0.0f, -45.0f));
            }
            else if (8 <= GameController.instance.currentLevel && GameController.instance.currentLevel <= 11
                || 25 <= GameController.instance.currentLevel && GameController.instance.currentLevel < maxLevelLength)
            {
                HandleCamera(Quaternion.Euler(0.0f, 0.0f, 45.0f));
            }
        }
    }

    private void HandleCamera(Quaternion destinationRotation)
    {
        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, destinationRotation, 0.5f);
    }

    private void Move()
    {
        if (isLerping && !isDie)
        {
            float timeSinceLerp = Time.time - timeStartedLerp;
            float percentageComplete = timeSinceLerp / speedLerp;

            transform.position = Vector3.Lerp(startPointLerping, endPointLerping, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                isLerping = false;
            }
        }
    }

    private void CheckBendAndLeap()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 6.5f, layerMaskArrow);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "TapUp" || hit.collider.tag == "TapLeft" || hit.collider.tag == "TapRight" || hit.collider.tag == "TapJump")
            {
                timeMeetArrow++;
                if (timeMeetArrow == 2)
                {
                    timeMeetArrow = 0;
                    // Disable older levels
                    if (GameController.instance.currentLevel < maxLevelLength)
                    {
                        string levelName = "level " + (GameController.instance.currentLevel - 1);
                        try
                        {
                            GameObject olderLevel = GameController.instance.mazeGenerator.environment_obj.transform.Find(levelName).gameObject;
                            olderLevel.SetActive(false);
                        }
                        catch { }
                    }
                }

                if (hit.collider.tag == "TapUp")
                {
                    canBeMove = true;
                    canGoAhead = true;
                    canTurnLeft = canTurnRight = false;
                }
                else if (hit.collider.tag == "TapLeft")
                {
                    canBeMove = true;
                    canTurnLeft = true;
                    canGoAhead = canTurnRight;
                }
                else if (hit.collider.tag == "TapRight")
                {
                    canBeMove = true;
                    canTurnRight = true;
                    canGoAhead = canTurnLeft = false;
                }
                else
                {
                    if (!isJumping)
                    {
                        canBeJump = true;
                    }
                }
                hit.collider.gameObject.GetComponent<ShowUpArrow>().enabled = true;
            }
            else
            {
                canBeMove = canBeJump = false;
                canGoAhead = canTurnLeft = canTurnRight = false;
            }
        }
    }

    private void CheckOutOfPath()
    {
        outMap = Physics2D.OverlapCircle(bodyCharacter.transform.position, 0.1f, layerMaskWalkable);
        if (!outMap && !isJumping)
        {
            SoundManager.instance.PlaySoundEffect(Constants.FALL_SOURCE_NAME);
            isDie = true;
            charAnim.SetTrigger(dieHash);
            GameController.instance.score = GameController.instance.maxScoreOfLevel = 0;
            GameController.instance.gameState = GAMESTATE.OVER;
			if (AdsControl.Instance != null) AdsControl.Instance.showAds ();
            GameData.Instance.SaveData(Constants.LEVEL_JUST_PLAYED, GameController.instance.currentLevel);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Diamond")
        {
            SoundManager.instance.PlaySoundEffect(Constants.CRYSTAL_TAKE_SOURCE_NAME);
            col.gameObject.SetActive(false);
            AddScore(1);
        }
        if (col.tag == "TapUp" || col.tag == "TapLeft" || col.tag == "TapRight" || col.tag == "TapJump")
        {
            AddScore(2);
        }
        if (col.tag == "FinishLevel")
        {
            SoundManager.instance.PlaySoundEffect(Constants.LEVEL_WIN_SOURCE_NAME);
            // Check Max score of level
            if (GameController.instance.maxScoreOfLevel >=
                GameController.instance.mazeGenerator.levelsManager[GameController.instance.currentLevel - 1].maxScoreOfLevel)
            {
                string stateMaxScore = "StateMaxScore" + GameController.instance.currentLevel;
                GameData.Instance.SaveData(stateMaxScore, 1);
            }
            GameController.instance.maxScoreOfLevel = 0;
            GameController.instance.currentLevel++;

            CheckUnlock();

            if (GameController.instance.currentLevel < maxLevelLength)
            {
                GameController.instance.currentLevelText.text = GameController.instance.currentLevel + " / " + (maxLevelLength - 1);

                /* Check if level unlocked or not */
                UnlockLevel();
                // Change background color
                ChangeBackGroundColor();
                // Set next level will be drawn on scene
                GameController.instance.mazeGenerator.level = GameController.instance.currentLevel + 1;
                // Set start point for next level
                SetStartPointNextLevel();
                // Lerp player to center of the path
                LerpToCenterOfPath();
            }
            else
            {
                if (!GameController.instance.reachedAllLevel)
                {
                    UnlockAllLevel();
                }
            }
        }
    }

    private void UnlockAllLevel()
    {
        GameController.instance.reachedAllLevel = true;
        SoundManager.instance.PlaySoundEffect(Constants.HERO_UNLOCK_SOURCE_NAME);
        isLerping = false;
        charAnim.enabled = false;

        GameController.instance.mazeGenerator.level = maxLevelLength;
        GameController.instance.mazeGenerator.levelsManager[maxLevelLength - 1].startPointOfLevel = new Vector3(
            GameController.instance.mazeGenerator.tileLastPosition.x,
            GameController.instance.mazeGenerator.tileLastPosition.y + GameController.instance.mazeGenerator.sizeOfPathTile * 2.0f - 0.1f, 0.0f);
        GameController.instance.mazeGenerator.GenerateMap();

        GameController.instance.victoryEffect.transform.position = new Vector3(
            GameController.instance.mazeGenerator.levelsManager[maxLevelLength - 1].startPointOfLevel.x,
            GameController.instance.mazeGenerator.levelsManager[maxLevelLength - 1].startPointOfLevel.y + 3.0f,
            0.0f);
        GameController.instance.victoryEffect.GetComponent<ParticleSystem>().Play();

        Camera.main.GetComponent<CameraFollow>().finishLine = new Vector3(
            GameController.instance.mazeGenerator.levelsManager[maxLevelLength - 1].startPointOfLevel.x,
            GameController.instance.mazeGenerator.levelsManager[maxLevelLength - 1].startPointOfLevel.y,
            -10.0f);
    }

    private void LerpToCenterOfPath()
    {
        timeStartedLerp = Time.time;
        startPointLerping = transform.position;
        endPointLerping = GameController.instance.mazeGenerator.levelsManager[GameController.instance.mazeGenerator.level - 2].startPointOfLevel;
        speedLerp = GameController.instance.charSpeedRunToFinish;
        canBeTap = false;
    }

    private void SetStartPointNextLevel()
    {
        GameController.instance.mazeGenerator.levelsManager[GameController.instance.mazeGenerator.level - 1].startPointOfLevel = new Vector3(
                    GameController.instance.mazeGenerator.tileLastPosition.x,
                    GameController.instance.mazeGenerator.tileLastPosition.y + GameController.instance.mazeGenerator.sizeOfPathTile * 2.0f - 0.1f, 0.0f);
        GameController.instance.mazeGenerator.GenerateMap();
    }

    private void ChangeBackGroundColor()
    {
        GameController.instance.indexBackgroundColor++;
        if (GameController.instance.indexBackgroundColor > 6)
        {
            GameController.instance.indexBackgroundColor = 0;
        }
        Camera.main.GetComponent<Camera>().backgroundColor = GameController.instance.backgrounds[GameController.instance.indexBackgroundColor];
    }

    private void UnlockLevel()
    {
        string stateLevel = "StateLevel" + GameController.instance.currentLevel;
        int state = GameData.Instance.StateLevels[GameController.instance.currentLevel - 1];

        if (state == 0)
        {
            GameData.Instance.SaveData(stateLevel, 1);
            GameData.Instance.SaveData(Constants.HIGHEST_LEVEL, GameController.instance.currentLevel);
        }
        GameData.Instance.SaveData(Constants.SELECTED_LEVEL, GameController.instance.currentLevel);
    }

    private void CheckUnlock()
    {
        if (GameController.instance.currentLevel == 5)
        {
            if (GameData.Instance.StateCharacters[1] == 0)
            {
                UnlockCharacter(1, "StateCharacter2", "SelectedCharacter2");
                UIManager.instance.character2FaceSprite.sprite = UIManager.instance.charactersUnlockedSprites[1];
                UIManager.instance.checkedCharacter2.enabled = true;
            }
        }
        else if (GameController.instance.currentLevel == 10)
        {
            if (GameData.Instance.StateCharacters[2] == 0)
            {
                UnlockCharacter(2, "StateCharacter3", "SelectedCharacter3");
                UIManager.instance.character3FaceSprite.sprite = UIManager.instance.charactersUnlockedSprites[2];
                UIManager.instance.checkedCharacter3.enabled = true;
            }
        }
        else if (GameController.instance.currentLevel == 15)
        {
            if (GameData.Instance.StateCharacters[3] == 0)
            {
                UnlockCharacter(3, "StateCharacter4", "SelectedCharacter4");
                UIManager.instance.character4FaceSprite.sprite = UIManager.instance.charactersUnlockedSprites[3];
                UIManager.instance.checkedCharacter4.enabled = true;
            }
        }
        else if (GameController.instance.currentLevel == 20)
        {
            if (GameData.Instance.StateCharacters[4] == 0)
            {
                UnlockCharacter(4, "StateCharacter5", "SelectedCharacter5");
                UIManager.instance.character5FaceSprite.sprite = UIManager.instance.charactersUnlockedSprites[4];
                UIManager.instance.checkedCharacter5.enabled = true;
            }
        }
        else if (GameController.instance.currentLevel == 25)
        {
            if (GameData.Instance.StateCharacters[5] == 0)
            {
                UnlockCharacter(5, "StateCharacter6", "SelectedCharacter6");
                UIManager.instance.character6FaceSprite.sprite = UIManager.instance.charactersUnlockedSprites[5];
                UIManager.instance.checkedCharacter6.enabled = true;
            }
        }
    }

    private void AddScore(int score)
    {
        GameController.instance.score += score;
        GameController.instance.currentScoreText.text = GameController.instance.score + "";
        GameController.instance.maxScoreOfLevel += score;
    }

    private void UnlockCharacter(int indexChar, string key1, string key2)
    {
        GameData.Instance.StateCharacters[indexChar] = 1;
        GameData.Instance.SaveData(key1, GameData.Instance.StateCharacters[indexChar]);
        GameData.Instance.SelectedCharacters[indexChar] = 1;
        GameData.Instance.SaveData(key2, GameData.Instance.SelectedCharacters[indexChar]);
        SoundManager.instance.PlaySoundEffect(Constants.HERO_UNLOCK_SOURCE_NAME);
    }

    private IEnumerator Jumping()
    {
        if (GameController.instance.currentLevel < 4)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.25f);
        }

        charAnim.SetBool(jumpHash, false);
        yield return new WaitForSeconds(0.1f);
        isJumping = false;
    }
}
