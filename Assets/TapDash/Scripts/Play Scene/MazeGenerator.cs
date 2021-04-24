using UnityEngine;
using System.Collections;

public class MazeGenerator : MonoBehaviour
{
    public int level;

    public GameObject environment_obj;
    public GameObject genesisTile;
    public GameObject pathTile;
    public GameObject halfpathTile;
    public GameObject edgeTile;
    public GameObject leapTile;
    public GameObject circleLeapTile;
    public GameObject finishTile;
    public GameObject arrowInactive;
    public GameObject diamond;
    // ENEMY
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    public Level[] levelsManager;

    public Vector3 tileLastPosition;

    [HideInInspector]
    public float sizeOfPathTile;

    private int lenghtOfPath;

    private bool up_direct;
    private bool left_direct;
    private bool changeDirect;
    private bool leap;
    private bool justFinishedLeap;
    private bool circleLeap;
    private int currentLeap;

    private string lastDirect = null;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        sizeOfPathTile = pathTile.GetComponent<SpriteRenderer>().bounds.size.x;

        up_direct = true;
        left_direct = changeDirect = leap = justFinishedLeap = circleLeap = false;

        /* Save the position of last tile is drawn - initialize is Vector3(0.0f, 1.0f, 0.0f) */
        tileLastPosition = new Vector3(0.0f, 1.0f, 0.0f);
        //GenerateMap();
    }

    public void GenerateMap()
    {
        string levelName = "level " + level;
        GameObject level_obj = new GameObject(levelName);
        GameObject path_obj = new GameObject("Path");
        GameObject tapPoints_obj = new GameObject("Tap Points");
        GameObject diamonds_obj = new GameObject("Diamonds");
        HierarchicalObjects(ref level_obj, ref path_obj, ref tapPoints_obj, ref diamonds_obj);

        // Set length of level
        // SetLengthOfLevel();

        int maxScore = 0;
        Vector3 pos = new Vector3(0, 0, 0);
        /* Draw genesis tile - Start point */
        GameObject clone = CreateTile(genesisTile, pos, Quaternion.identity);
        clone.transform.SetParent(path_obj.transform);

        /* Draw number of level at start point tile, will be string "Victory" if this's final level */
        SetTextAtStartPoint(ref clone);


        int countLoop = 0;
        
        int randOpti = Random.Range(3, 10);
        int lastRand = 0;
        int randHalfPath = 0;
        bool halfRandFlag = false;
        while (true)
        {
            int localRandPath = Random.Range(1, 4);
            if (countLoop == 0)
            {
                currentLeap = randOpti;
            }
            if (countLoop == randOpti)
            {
                halfRandFlag = true;
                int localRand = 0;
                if (countLoop > 200)
                {
                    localRand = Random.Range(3, 10);
                } else
                {
                    localRand = Random.Range(3, 7);
                }

                randOpti = countLoop + localRand;
                lastRand = localRand;

                if (countLoop - currentLeap > 2)
                {
                    int behaviorRand = Random.Range(1, 4);
                    currentLeap = countLoop;
                    if (behaviorRand == 1)
                    {
                        // Turn Or Go Ahead
                        Bend(countLoop, localRand);
                    }
                    else if (behaviorRand == 2)
                    {
                        // Jump
                        Leap(countLoop);
                    }
                    else
                    {
                        // Turn Or Go Ahead
                        Bend(countLoop, localRand);
                        // Jump
                        Leap(countLoop);
                    }


                }
            }
            circleLeap = false;

            // TODO check half path
            bool hasHalfPath = false;
            if (lastRand >= 4 && randOpti > 100)
            {
                if (halfRandFlag)
                {
                    randHalfPath = Random.Range(1, 4) + countLoop;
                }
                halfRandFlag = false;
            }
            if (countLoop == randHalfPath && countLoop > 0)
            {
                hasHalfPath = true;
            }


            // Generate Enemy
            //bool hasEnemy = false;
            //if (lastRand >= 4 && randOpti > 10)
            //{
            //    if (hasEnemy)
            //    {
            //        randHalfPath = Random.Range(1, 4) + countLoop;
                    
            //    }
            //    hasEnemy = false;
            //}
            //// TODO thay đổi giá trị
            //if (countLoop == randHalfPath && countLoop > 10)
            //{
            //    Instantiate(enemy1, new Vector3(tileLastPosition.x + 0.5f, tileLastPosition.y, 0.0f), enemy1.transform.rotation);
            //    hasEnemy = true;
            //}



            // TODO check Pos
            //checkPosDer();






            if (up_direct)
                {

                    if (changeDirect)       /* If direction is left before then handle as follows */
                    {
                        changeDirect = false;
                        if (lastDirect == "left")
                        {
                            clone = CreateTile(edgeTile,
                                new Vector3(tileLastPosition.x - sizeOfPathTile + 0.1f, tileLastPosition.y, 0.0f),
                                Quaternion.Euler(0.0f, 0.0f, 90.0f));
                        }
                        else if (lastDirect == "right")
                        {
                            clone = CreateTile(edgeTile,
                                new Vector3(tileLastPosition.x + sizeOfPathTile - 0.1f, tileLastPosition.y, 0.0f),
                                Quaternion.Euler(0.0f, 0.0f, 180.0f));
                        }
                        clone.transform.SetParent(path_obj.transform);

                        /* Create "arrowInactive" tile at bend point or leap point */
                        GameObject arrow_clone = CreateTile(arrowInactive, clone.transform.position, Quaternion.identity) as GameObject;
                        arrow_clone.tag = "TapUp";
                        arrow_clone.transform.SetParent(tapPoints_obj.transform);
                        maxScore += 20;

                        tileLastPosition = clone.transform.position;
                    }
                    else      /* If going straight */
                    {
                        if (leap)    /* Handle to leap */
                        {
                            leap = false;
                            justFinishedLeap = true;
                            if (circleLeap)
                            {
                                circleLeap = false;
                                clone = CreateTile(circleLeapTile,
                                        new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                                        Quaternion.identity);
                            }
                            else
                            {
                                clone = CreateTile(leapTile,
                                        new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                                        Quaternion.Euler(0.0f, 0.0f, 0.0f));
                            }
                            clone.transform.SetParent(path_obj.transform);

                            /* Create "arrowInactive" tile at bend point or leap point */
                            GameObject arrow_clone = CreateTile(arrowInactive, clone.transform.position, clone.transform.rotation) as GameObject;
                            arrow_clone.tag = "TapJump";
                            arrow_clone.transform.SetParent(tapPoints_obj.transform);
                            maxScore += 20;

                            tileLastPosition = new Vector3((clone.transform.position.x),
                                clone.transform.position.y + sizeOfPathTile - 0.1f, 0.0f);
                        }
                        else
                        {
                            if (justFinishedLeap)    /* Just leaped */
                            {
                                justFinishedLeap = false;
                                clone = CreateTile(leapTile,
                                    new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                                        Quaternion.Euler(0.0f, 0.0f, 180.0f));
                            }
                            else   /* Create path tile */
                            {
                                if (hasHalfPath)
                                {
                                    clone = CreateTile(halfpathTile,
                                        new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                                        Quaternion.identity);
                                }
                                else
                                {
                                    clone = CreateTile(pathTile,
                                        new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                                        Quaternion.identity);
                                }
                                
                            }
                            clone.transform.SetParent(path_obj.transform);

                            /* Create diamond on path from second path tile */
                            if (countLoop > 5)
                            {
                                GameObject diamond_clone = CreateTile(diamond, clone.transform.position, Quaternion.identity) as GameObject;
                                diamond_clone.transform.SetParent(diamonds_obj.transform);
                                maxScore += 10;
                            }

                            tileLastPosition = clone.transform.position;
                        }
                    }
                }
                else
                {
                    if (left_direct)   /* Turn left */
                    {
                        if (changeDirect)
                        {
                            changeDirect = false;
                            clone = CreateTile(edgeTile,
                                new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                                Quaternion.Euler(0.0f, 0.0f, -90.0f));
                            clone.transform.SetParent(path_obj.transform);

                            /* Create "arrowInactive" tile at bend point or leap point */
                            GameObject arrow_clone = CreateTile(arrowInactive, clone.transform.position,
                                Quaternion.Euler(0.0f, 0.0f, 90.0f)) as GameObject;
                            arrow_clone.tag = "TapLeft";
                            arrow_clone.transform.SetParent(tapPoints_obj.transform);
                            maxScore += 20;

                            tileLastPosition = clone.transform.position;
                        }
                        else
                        {
                            if (leap)   /* Handle to leap */
                            {
                                leap = false;
                                justFinishedLeap = true;
                                if (circleLeap)
                                {
                                    circleLeap = false;
                                    clone = CreateTile(circleLeapTile,
                                            new Vector3(tileLastPosition.x - sizeOfPathTile - 0.1f, tileLastPosition.y, 0.0f),
                                            //Quaternion.Euler(0.0f, 90.0f, 0.0f));
                                            Quaternion.Euler(0.0f, 0.0f, 90.0f));
                                }
                                else
                                {
                                    clone = CreateTile(leapTile,
                                        new Vector3(tileLastPosition.x - sizeOfPathTile + 0.1f, tileLastPosition.y, 0.0f),
                                        //Quaternion.Euler(0.0f, 90.0f, 0.0f));
                                        Quaternion.Euler(0.0f, 0.0f, 90.0f));
                                }
                                clone.transform.SetParent(path_obj.transform);

                                /* Create "arrowInactive" tile at bend point or leap point */
                                GameObject arrow_clone = CreateTile(arrowInactive, clone.transform.position, clone.transform.rotation) as GameObject;
                                arrow_clone.tag = "TapJump";
                                arrow_clone.transform.SetParent(tapPoints_obj.transform);
                                maxScore += 20;

                                tileLastPosition = new Vector3((clone.transform.position.x - sizeOfPathTile + 0.1f),
                                    clone.transform.position.y, 0.0f);
                            }
                            else
                            {
                                if (justFinishedLeap)    /* Just leaped */
                                {
                                    justFinishedLeap = false;
                                    clone = CreateTile(leapTile,
                                        new Vector3(tileLastPosition.x - sizeOfPathTile + 0.1f, tileLastPosition.y, 0.0f),
                                        Quaternion.Euler(0.0f, 0.0f, -90.0f));
                                }
                                else  /* Create path tile */
                                {
                                    clone = CreateTile(pathTile,
                                        new Vector3(tileLastPosition.x - sizeOfPathTile + 0.1f, tileLastPosition.y, 0.0f),
                                        Quaternion.identity);
                                }
                                clone.transform.SetParent(path_obj.transform);

                                /* Create diamond on path */
                                GameObject diamond_clone = CreateTile(diamond, clone.transform.position, Quaternion.identity) as GameObject;
                                diamond_clone.transform.SetParent(diamonds_obj.transform);
                                maxScore += 10;

                                tileLastPosition = clone.transform.position;
                            }
                        }
                    }
                    else    /* Turn right */
                    {
                        if (changeDirect)
                        {
                            changeDirect = false;
                            clone = CreateTile(edgeTile,
                                new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                                Quaternion.Euler(0.0f, 0.0f, 0.0f));
                            clone.transform.SetParent(path_obj.transform);

                            /* Create "arrowInactive" tile at bend point or leap point */
                            GameObject arrow_clone = CreateTile(arrowInactive, clone.transform.position,
                                Quaternion.Euler(0.0f, 0.0f, -90.0f)) as GameObject;
                            arrow_clone.tag = "TapRight";
                            arrow_clone.transform.SetParent(tapPoints_obj.transform);
                            maxScore += 20;

                            tileLastPosition = clone.transform.position;
                        }
                        else
                        {
                            if (leap)   /* Handle to leap */
                            {
                                leap = false;
                                justFinishedLeap = true;
                                if (circleLeap)
                                {
                                    circleLeap = false;
                                    clone = CreateTile(circleLeapTile,
                                            new Vector3(tileLastPosition.x + sizeOfPathTile - 0.1f, tileLastPosition.y, 0.0f),
                                            Quaternion.Euler(0.0f, 0.0f, -90.0f));
                                }
                                else
                                {
                                    clone = CreateTile(leapTile,
                                        new Vector3(tileLastPosition.x + sizeOfPathTile - 0.1f, tileLastPosition.y, 0.0f),
                                        Quaternion.Euler(0.0f, 0.0f, -90.0f));
                                }
                                clone.transform.SetParent(path_obj.transform);

                                /* Create "arrowInactive" tile at bend point or leap point */
                                GameObject arrow_clone = CreateTile(arrowInactive, clone.transform.position, clone.transform.rotation) as GameObject;
                                arrow_clone.tag = "TapJump";
                                arrow_clone.transform.SetParent(tapPoints_obj.transform);
                                maxScore += 20;

                                tileLastPosition = new Vector3((clone.transform.position.x + sizeOfPathTile - 0.1f),
                                    clone.transform.position.y, 0.0f);
                            }
                            else
                            {
                                if (justFinishedLeap)    /* Just leaped */
                                {
                                    justFinishedLeap = false;
                                    clone = CreateTile(leapTile,
                                        new Vector3(tileLastPosition.x + sizeOfPathTile - 0.1f, tileLastPosition.y, 0.0f),
                                        Quaternion.Euler(0.0f, 0.0f, 90.0f));
                                }
                                else     /* Create path tile */
                                {
                                    clone = CreateTile(pathTile,
                                        new Vector3(tileLastPosition.x + sizeOfPathTile - 0.1f, tileLastPosition.y, 0.0f),
                                        Quaternion.identity);
                                }
                                clone.transform.SetParent(path_obj.transform);

                                /* Create diamond on path */
                                GameObject diamond_clone = CreateTile(diamond, clone.transform.position, Quaternion.identity) as GameObject;
                                diamond_clone.transform.SetParent(diamonds_obj.transform);
                                maxScore += 10;

                                tileLastPosition = clone.transform.position;
                            }
                        }
                    }
                }







            countLoop++;
            if (countLoop > 1000) break;
        }


        // Bonus a path tile to fill gap
        //GameObject bonus_clone = CreateTile(pathTile,
        //            new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
        //            Quaternion.identity);
        //bonus_clone.transform.SetParent(path_obj.transform);

        //levelsManager[level - 1].maxScoreOfLevel = maxScore;
    }

    private void HierarchicalObjects(ref GameObject level_obj, ref GameObject path_obj, ref GameObject tapPoints_obj, ref GameObject diamonds_obj)
    {
        level_obj.transform.parent = environment_obj.transform;
        path_obj.transform.parent = level_obj.transform;
        tapPoints_obj.transform.parent = level_obj.transform;
        diamonds_obj.transform.parent = level_obj.transform;
    }

    private void SetTextAtStartPoint(ref GameObject clone_tile)
    {
        TextMesh levelText = clone_tile.GetComponentInChildren<TextMesh>();
        levelText.GetComponent<Renderer>().sortingLayerName = "Path";
        levelText.GetComponent<Renderer>().sortingOrder = 1;
        levelText.characterSize = 0.3f;
        levelText.text = "GO!";
    }

    private void SetLengthOfLevel()
    {
        lenghtOfPath = levelsManager[level - 1].lenghtOfPath;
    }

    private void setBend(int randNumber)
    {
        if (randNumber == 1)
        {
            SetDirection(true, false);
            lastDirect = "up";
        }
        else if (randNumber == 2)
        {
            SetDirection(false, true);
            lastDirect = "left";
        }
        else
        {
            SetDirection(false, false);
            lastDirect = "right";
        }
    }


    private void Bend(int i, int localRand)
    {
        int randNumberGl = Random.Range(1, 4);

        // randNumber = 1 -> Go Ahead
        // randNumber = 2 -> Turn Left
        // randNumber = 3 -> Turn Right

        if (lastDirect == "left")
        {
            //int[] array = new[] { 1, 3 };
            //int value = Random.Range(0, array.Length);
            setBend(1);
        }
        else if(lastDirect == "right")
        {
            setBend(1);
        } else if (lastDirect == "up")
        {
            int randNumber = Random.Range(2, 4);
            setBend(randNumber);
        } else
        {
            setBend(randNumberGl);
        }

        //int bendPointAmout = levelsManager[level - 1].bendPoints.Length;
        //for (int j = 0; j < bendPointAmout; j++)
        //{
        //    if (i == levelsManager[level - 1].bendPoints[j].bendPoint)
        //    {
        //        if (levelsManager[level - 1].bendPoints[j].goAhead)
        //        {
        //            SetDirection(true, false);
        //        }
        //        else
        //        {
        //            if (levelsManager[level - 1].bendPoints[j].turnLeft)
        //            {
        //                SetDirection(false, true);
        //                lastDirect = "left";
        //            }
        //            else
        //            {
        //                if (levelsManager[level - 1].bendPoints[j].turnRight)
        //                {
        //                    SetDirection(false, false);
        //                    lastDirect = "right";
        //                }
        //            }
        //        }
        //        break;
        //    }
        //}
    }

    private void Leap(int i)
    {
        leap = true;
        circleLeap = true;
        //for (int j = 0; j < levelsManager[level - 1].leapPoints.Length; j++)
        //{
        //    if (i == levelsManager[level - 1].leapPoints[j])
        //    {
        //        leap = true;
        //        if (j != 0 && (i - 1) == levelsManager[level - 1].leapPoints[j - 1])
        //        {
        //            circleLeap = true;
        //        }
        //        else
        //        {
        //            circleLeap = false;
        //        }
        //        break;
        //    }
        //}
    }

    private void checkPosDer()
    {
        //SetDirection(true, false);
    }

    private GameObject CreateTile(GameObject tile, Vector3 position, Quaternion rotation)
    {
        return (Instantiate(tile, position, rotation) as GameObject);
        //return (Instantiate(tile, position, rotation) as GameObject);
    }

    private void SetDirection(bool up, bool left)
    {
        up_direct = up;
        left_direct = left;
        changeDirect = true;
    }
}

[System.Serializable]
public class Level
{
    public Vector3 startPointOfLevel;

    public int maxScoreOfLevel;
    public int lenghtOfPath;

    public PointAndDirection[] bendPoints;

    public int[] leapPoints;
}

[System.Serializable]
public class PointAndDirection
{
    public int bendPoint;

    public bool goAhead;
    public bool turnLeft;
    public bool turnRight;
}
