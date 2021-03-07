using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour {
    public int level;

    public static int UP = 1;
    public static int DOWN = 2;
    public static int LEFT = 3;
    public static int RIGHT = 4;

    public static int YES = 1;
    public static int NO = 0;
    public static int NULL = -1;

    public List<int> directions;
    public List<int> changes;
    public List<int> leaps;

    public GameObject environment_obj;
    public GameObject genesisTile;
    public GameObject pathTile;
    public GameObject edgeTile;
    public GameObject leapTile;
    public GameObject circleLeapTile;
    public GameObject finishTile;
    public GameObject arrowInactive;
    public GameObject diamond;

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

    private string lastDirect = null;

    string levelName;
    GameObject level_obj;
    GameObject path_obj;
    GameObject tapPoints_obj;
    GameObject diamonds_obj;

    int maxScore = 0;
    Vector3 pos = new Vector3(0, 0, 0);
    int countLoop = 0;
    bool firstChangeDirection = false;

    void Start() {
        Init();
        directions = new List<int> { UP, UP, UP, UP, UP, UP, UP, UP, LEFT, LEFT, LEFT, LEFT, LEFT, UP, UP, UP, UP, UP, UP, UP, UP };
        changes = new List<int>     { NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, YES, NO, NO, NO, NO, YES, NO, NO, NO, NO, NO, NO, NO, NO };
        leaps = new List<int>     { NO, NO, NO, NO, NO, NO, NO, YES, NO, NO, NO, NO, NO, NO, YES, NO, NO, NO, NO, NO, NO, NO, NO };
    }

    private void Init() {
        sizeOfPathTile = pathTile.GetComponent<SpriteRenderer>().bounds.size.x;

        up_direct = true;
        left_direct = changeDirect = leap = justFinishedLeap = circleLeap = false;

        /* Save the position of last tile is drawn - initialize is Vector3(0.0f, 1.0f, 0.0f) */
        tileLastPosition = new Vector3(0.0f, 1.0f, 0.0f);
        //GenerateMap();
    }

    public void GenerateMap() {
        levelName = "level " + level;
        level_obj = new GameObject(levelName);
        path_obj = new GameObject("Path");
        tapPoints_obj = new GameObject("Tap Points");
        diamonds_obj = new GameObject("Diamonds");

        HierarchicalObjects(ref level_obj, ref path_obj, ref tapPoints_obj, ref diamonds_obj);
        SetLengthOfLevel();

        Vector3 pos = new Vector3(0, 0, 0);
        int maxScore = 0;
        int countLoop = 0;

        up_direct = true;
        left_direct = false;
        changeDirect = false;
        lastDirect = null;

        while (true) {
            if (countLoop == 0) {
                /* Draw genesis tile - Start point */
                GameObject clone = CreateTile(genesisTile, pos, Quaternion.identity);
                clone.transform.parent = path_obj.transform;

                /* Draw number of level at start point tile, will be string "Victory" if this's final level */
                SetTextAtStartPoint(ref clone);
            } else {
                GenerateTile(countLoop);
            }
            ///* Create diamond on path */
            //GameObject diamond_clone = CreateTile(diamond, clone.transform.position, Quaternion.identity) as GameObject;
            //diamond_clone.transform.parent = diamonds_obj.transform;
            //maxScore += 10;

            //tileLastPosition = clone.transform.position;
            countLoop++;
            if (countLoop >= directions.Count) break;
        }

        // Bonus a path tile to fill gap
        GameObject bonus_clone = CreateTile(pathTile,
                    new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                    Quaternion.identity);
        bonus_clone.transform.parent = path_obj.transform;

        levelsManager[level - 1].maxScoreOfLevel = maxScore;
    }

    public void GenerateTile(int i) {
        //Bend(i);

        UpdateUpLeftDirectionStat(i);
        //ReadDirection(i);
        Leap(i);

        //SetUpLeftDirection(true, false);
        //changeDirect = changes[i];

        

        if (directions[i] == UP) {
            GenerateTileUp();
        } else {
            if (directions[i] == LEFT) {
                GenerateTileLeft();
            } else {
                GenerateTileRight();
            }
        }

    }

    private void GenerateTileRight() {
        GameObject clone = null;
        if (changeDirect) {
            changeDirect = false;
            clone = CreateTile(edgeTile,
                new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, 0.0f));
            clone.transform.parent = path_obj.transform;

            /* Create "arrowInactive" tile at bend point or leap point */
            GameObject arrow_clone = CreateTile(arrowInactive, clone.transform.position,
                Quaternion.Euler(0.0f, 0.0f, -90.0f)) as GameObject;
            arrow_clone.tag = "TapRight";
            arrow_clone.transform.parent = tapPoints_obj.transform;
            maxScore += 20;

            tileLastPosition = clone.transform.position;
        } else {
            if (leap)   /* Handle to leap */
            {
                leap = false;
                justFinishedLeap = true;
                if (circleLeap) {
                    circleLeap = false;
                    clone = CreateTile(circleLeapTile,
                            new Vector3(tileLastPosition.x + sizeOfPathTile - 0.1f, tileLastPosition.y, 0.0f),
                            Quaternion.Euler(0.0f, 0.0f, -90.0f));
                } else {
                    clone = CreateTile(leapTile,
                        new Vector3(tileLastPosition.x + sizeOfPathTile - 0.1f, tileLastPosition.y, 0.0f),
                        Quaternion.Euler(0.0f, 0.0f, -90.0f));
                }
                clone.transform.parent = path_obj.transform;

                /* Create "arrowInactive" tile at bend point or leap point */
                GameObject arrow_clone = CreateTile(arrowInactive, clone.transform.position, clone.transform.rotation) as GameObject;
                arrow_clone.tag = "TapJump";
                arrow_clone.transform.parent = tapPoints_obj.transform;
                maxScore += 20;

                tileLastPosition = new Vector3((clone.transform.position.x + sizeOfPathTile - 0.1f),
                    clone.transform.position.y, 0.0f);
            } else {
                if (justFinishedLeap)    /* Just leaped */
                {
                    justFinishedLeap = false;
                    clone = CreateTile(leapTile,
                        new Vector3(tileLastPosition.x + sizeOfPathTile - 0.1f, tileLastPosition.y, 0.0f),
                        Quaternion.Euler(0.0f, 0.0f, 90.0f));
                } else     /* Create path tile */
                  {
                    clone = CreateTile(pathTile,
                        new Vector3(tileLastPosition.x + sizeOfPathTile - 0.1f, tileLastPosition.y, 0.0f),
                        Quaternion.identity);
                }
                clone.transform.parent = path_obj.transform;

                /* Create diamond on path */
                GameObject diamond_clone = CreateTile(diamond, clone.transform.position, Quaternion.identity) as GameObject;
                diamond_clone.transform.parent = diamonds_obj.transform;
                maxScore += 10;

                tileLastPosition = clone.transform.position;
            }
        }
    }

    private void GenerateTileLeft() {
        GameObject clone = null;
        if (changeDirect) {
            changeDirect = false;
            clone = CreateTile(edgeTile,
                new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, -90.0f));
            clone.transform.parent = path_obj.transform;

            /* Create "arrowInactive" tile at bend point or leap point */
            GameObject arrow_clone = CreateTile(arrowInactive, clone.transform.position,
                Quaternion.Euler(0.0f, 0.0f, 90.0f)) as GameObject;
            arrow_clone.tag = "TapLeft";
            arrow_clone.transform.parent = tapPoints_obj.transform;
            maxScore += 20;

            tileLastPosition = clone.transform.position;
        } else {
            if (leap)   /* Handle to leap */
            {
                leap = false;
                justFinishedLeap = true;
                if (circleLeap) {
                    circleLeap = false;
                    clone = CreateTile(circleLeapTile,
                            new Vector3(tileLastPosition.x - sizeOfPathTile - 0.1f, tileLastPosition.y, 0.0f),
                            Quaternion.Euler(0.0f, 0.0f, 90.0f));
                } else {
                    clone = CreateTile(leapTile,
                        new Vector3(tileLastPosition.x - sizeOfPathTile + 0.1f, tileLastPosition.y, 0.0f),
                        Quaternion.Euler(0.0f, 0.0f, 90.0f));
                }
                clone.transform.parent = path_obj.transform;

                /* Create "arrowInactive" tile at bend point or leap point */
                GameObject arrow_clone = CreateTile(arrowInactive, clone.transform.position, clone.transform.rotation) as GameObject;
                arrow_clone.tag = "TapJump";
                arrow_clone.transform.parent = tapPoints_obj.transform;
                maxScore += 20;

                tileLastPosition = new Vector3((clone.transform.position.x - sizeOfPathTile + 0.1f),
                    clone.transform.position.y, 0.0f);
            } else {
                if (justFinishedLeap)    /* Just leaped */
                {
                    justFinishedLeap = false;
                    clone = CreateTile(leapTile,
                        new Vector3(tileLastPosition.x - sizeOfPathTile + 0.1f, tileLastPosition.y, 0.0f),
                        Quaternion.Euler(0.0f, 0.0f, -90.0f));
                } else  /* Create path tile */
                  {
                    clone = CreateTile(pathTile,
                        new Vector3(tileLastPosition.x - sizeOfPathTile + 0.1f, tileLastPosition.y, 0.0f),
                        Quaternion.identity);
                }
                clone.transform.parent = path_obj.transform;

                /* Create diamond on path */
                GameObject diamond_clone = CreateTile(diamond, clone.transform.position, Quaternion.identity) as GameObject;
                diamond_clone.transform.parent = diamonds_obj.transform;
                maxScore += 10;

                tileLastPosition = clone.transform.position;
            }
        }
    }

    private void GenerateTileUp() {
        GameObject clone = null;

        Debug.Log("THIS IS GENERATE TILE UP");

        if (changeDirect)       /* If direction is left before then handle as follows */
        {
            Debug.Log("THIS IS CHANGE DIRECT TRUE - LAST DIRECTION " + lastDirect);
            changeDirect = false;
            if (lastDirect == "left") {
                clone = CreateTile(edgeTile,
                    new Vector3(tileLastPosition.x - sizeOfPathTile + 0.1f, tileLastPosition.y, 0.0f),
                    Quaternion.Euler(0.0f, 0.0f, 90.0f));
            } else if (lastDirect == "right") {
                clone = CreateTile(edgeTile,
                    new Vector3(tileLastPosition.x + sizeOfPathTile - 0.1f, tileLastPosition.y, 0.0f),
                    Quaternion.Euler(0.0f, 0.0f, 180.0f));
            }
            clone.transform.parent = path_obj.transform;

            /* Create "arrowInactive" tile at bend point or leap point */
            GameObject arrow_clone = CreateTile(arrowInactive, clone.transform.position, Quaternion.identity) as GameObject;
            arrow_clone.tag = "TapUp";
            arrow_clone.transform.parent = tapPoints_obj.transform;
            maxScore += 20;

            tileLastPosition = clone.transform.position;
        } else      /* If going straight */
        {
            Debug.Log("THIS IS CHANGE DIRECT FALSE - LAST DIRECTION: " + lastDirect);
            if (leap)    /* Handle to leap */
            {
                leap = false;
                justFinishedLeap = true;
                if (circleLeap) {
                    circleLeap = false;
                    clone = CreateTile(circleLeapTile,
                            new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                            Quaternion.identity);
                } else {
                    clone = CreateTile(leapTile,
                            new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                            Quaternion.Euler(0.0f, 0.0f, 0.0f));
                }
                clone.transform.parent = path_obj.transform;

                /* Create "arrowInactive" tile at bend point or leap point */
                GameObject arrow_clone = CreateTile(arrowInactive, clone.transform.position, clone.transform.rotation) as GameObject;
                arrow_clone.tag = "TapJump";
                arrow_clone.transform.parent = tapPoints_obj.transform;
                maxScore += 20;

                tileLastPosition = new Vector3((clone.transform.position.x),
                    clone.transform.position.y + sizeOfPathTile - 0.1f, 0.0f);
            } else {
                if (justFinishedLeap)    /* Just leaped */
                {
                    justFinishedLeap = false;
                    clone = CreateTile(leapTile,
                        new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                            Quaternion.Euler(0.0f, 0.0f, 180.0f));
                } else   /* Create path tile */
                  {
                    clone = CreateTile(pathTile,
                        new Vector3(tileLastPosition.x, tileLastPosition.y + sizeOfPathTile - 0.1f, 0.0f),
                        Quaternion.identity);
                }
                clone.transform.parent = path_obj.transform;

                /* Create diamond on path from second path tile */
                if (countLoop != 1) {
                    GameObject diamond_clone = CreateTile(diamond, clone.transform.position, Quaternion.identity) as GameObject;
                    diamond_clone.transform.parent = diamonds_obj.transform;
                    maxScore += 10;
                }

                tileLastPosition = clone.transform.position;
            }
        }
    }

    private void HierarchicalObjects(ref GameObject level_obj, ref GameObject path_obj, ref GameObject tapPoints_obj, ref GameObject diamonds_obj) {
        level_obj.transform.parent = environment_obj.transform;
        path_obj.transform.parent = level_obj.transform;
        tapPoints_obj.transform.parent = level_obj.transform;
        diamonds_obj.transform.parent = level_obj.transform;
    }

    private void SetTextAtStartPoint(ref GameObject clone_tile) {
        TextMesh levelText = clone_tile.GetComponentInChildren<TextMesh>();
        levelText.GetComponent<Renderer>().sortingLayerName = "Path";
        levelText.GetComponent<Renderer>().sortingOrder = 1;
        levelText.characterSize = 0.3f;
        levelText.text = "GO!";
    }

    private void SetLengthOfLevel() {
        lenghtOfPath = levelsManager[level - 1].lenghtOfPath;
    }

    private void Bend(int i) {
        int bendPointAmout = levelsManager[level - 1].bendPoints.Length;
        for (int j = 0; j < bendPointAmout; j++) {
            if (i == levelsManager[level - 1].bendPoints[j].bendPoint) {
                if (levelsManager[level - 1].bendPoints[j].goAhead) {
                    SetUpLeftDirection(true, false);
                } else {
                    if (levelsManager[level - 1].bendPoints[j].turnLeft) {
                        SetUpLeftDirection(false, true);
                        lastDirect = "left";
                    } else {
                        if (levelsManager[level - 1].bendPoints[j].turnRight) {
                            SetUpLeftDirection(false, false);
                            lastDirect = "right";
                        }
                    }
                }
                break;
            }
        }
    }

    public void UpdateUpLeftDirectionStat(int i) {
        //if (i == 0) {
        //    lastDirect = "up";
        //    up_direct = true;
        //    left_direct = false;
        //    changeDirect = false;
        //    return;
        //}

        List<string> dir = new List<string> { "up", "down", "left", "right" };
        lastDirect = dir[directions[i - 1]];

        up_direct = directions[i] == UP;
        left_direct = directions[i] == LEFT;

        if (changes[i] == NULL) {
            lastDirect = null;
            changeDirect = false;
        } else if (changes[i] == YES) {
            changeDirect = true;
        }else 
        {
            changeDirect = false;
        }
    }

    public void ReadDirection(int i) {
        List<string> dir = new List<string> { "up", "down", "left", "right" };
        //lastDirect = dir[directions[i - 1]];

        up_direct = directions[i] == UP;
        left_direct = directions[i] == LEFT;

        changeDirect = directions[i] != directions[i - 1];

        if (changeDirect && !firstChangeDirection) {
            firstChangeDirection = true;
        }

        if (!firstChangeDirection) {
            lastDirect = dir[directions[i - 1]];
        }

        //if (lastDirect)

        //if (changes[i] == NULL) {
        //    lastDirect = null;
        //    changeDirect = false;
        //} else if (changes[i] == YES) {
        //    changeDirect = true;
        //} else {
        //    changeDirect = false;
        //}
    }

    //public void UpdateUpLeftDirectionStat(int i) {
    //    //iint bendPointAmout = levelsManager[level - 1].bendPoints.Length;
    //    if (directions[i] == UP) {
    //        SetUpLeftDirection(true, false);
    //    } else if (directions[i] == LEFT) {
    //        SetUpLeftDirection(false, true);
    //        lastDirect = "left";
    //    } else {
    //        SetUpLeftDirection(false, false);
    //        lastDirect = "right";
    //    }

    //    //for (int j = 0; j < bendPointAmout; j++) {
    //    //    if (i == levelsManager[level - 1].bendPoints[j].bendPoint) {
    //    //        if (levelsManager[level - 1].bendPoints[j].goAhead) {
    //    //            SetUpLeftDirection(true, false);
    //    //        } else {
    //    //            if (levelsManager[level - 1].bendPoints[j].turnLeft) {
    //    //                SetUpLeftDirection(false, true);
    //    //                lastDirect = "left";
    //    //            } else {
    //    //                if (levelsManager[level - 1].bendPoints[j].turnRight) {
    //    //                    SetUpLeftDirection(false, false);
    //    //                    lastDirect = "right";
    //    //                }
    //    //            }
    //    //        }
    //    //        break;
    //    //    }
    //    //}
    //}

    private void Leap(int i) {
        for (int j = 0; j < levelsManager[level - 1].leapPoints.Length; j++) {
            if (i == levelsManager[level - 1].leapPoints[j]) {
                leap = true;
                if (j != 0 && (i - 1) == levelsManager[level - 1].leapPoints[j - 1]) {
                    circleLeap = true;
                } else {
                    circleLeap = false;
                }
                break;
            }
        }
        //leap = true;
        //circleLeap = true;
    }

    //private void checkPosDer()
    //{
    //    //SetDirection(true, false);
    //}

    private GameObject CreateTile(GameObject tile, Vector3 position, Quaternion rotation) {
        return (Instantiate(tile, position, rotation) as GameObject);
        //return (Instantiate(tile, position, rotation) as GameObject);
    }

    private void SetUpLeftDirection(bool up, bool left) {
        up_direct = up;
        left_direct = left;
        changeDirect = true;
    }
}

[System.Serializable]
public class Level {
    public Vector3 startPointOfLevel;

    public int maxScoreOfLevel;
    public int lenghtOfPath;

    public PointAndDirection[] bendPoints;

    public int[] leapPoints;
}

[System.Serializable]
public class PointAndDirection {
    public int bendPoint;

    public bool goAhead;
    public bool turnLeft;
    public bool turnRight;
}
