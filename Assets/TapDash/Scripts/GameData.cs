using UnityEngine;
using System.Collections;

namespace Com.Studio2089.TapTapDash
{
    public class GameData
    {
        private static GameData _instance = null;
        public static GameData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameData();
                }

                return _instance;
            }
        }

        // Constructor
        private GameData() { }

        public int SoundState
        { get { return PlayerPrefs.GetInt(Constants.SOUND_STATE, 1); } }

        public int LvlListShowing
        { get { return PlayerPrefs.GetInt(Constants.LVL_LIST_SHOWING, 1); } }

        public int MapBtnClickState { get { return PlayerPrefs.GetInt(Constants.MAP_BTN_CLICK_STATE, 0); } }

        public int LevelJustPlayed { get { return PlayerPrefs.GetInt(Constants.LEVEL_JUST_PLAYED, 1); } }

        public int HighestLevel { get { return PlayerPrefs.GetInt(Constants.HIGHEST_LEVEL, 1); } }

        public int SelectedLevel { get { return PlayerPrefs.GetInt(Constants.SELECTED_LEVEL, 1); } }

        public int[] StateCharacters { get; set; }

        public int[] SelectedCharacters { get; set; }

        public int[] StateLevels { get; set; }

        public int[] StateMaxScore { get; set; }

        public void Init()
        {
            // StateCharacters is the status of the characters: unlocked or not
            // default is 6 character
            StateCharacters = new int[6];
            // SelectedCharacters is the status of the characters: checked or not
            SelectedCharacters = new int[6];

            for (int i = 0; i < 6; i++)
            {
                string stateCharacter = "StateCharacter" + (i + 1);
                StateCharacters[i] = PlayerPrefs.GetInt(stateCharacter, 0);

                string selectedCharacter = "SelectedCharacter" + (i + 1);
                SelectedCharacters[i] = PlayerPrefs.GetInt(selectedCharacter, 0);
            }

            // StateLevels is the status of the levels: unlocked or not
            // default is 50 level
            StateLevels = new int[50];
            // StateMaxScore is the status of the levels: maximum diamond or not
            // default is 50 level
            StateMaxScore = new int[50];

            for (int i = 0; i < 50; i++)
            {
                string stateLevel = "StateLevel" + (i + 1);
                StateLevels[i] = PlayerPrefs.GetInt(stateLevel, 0);

                string stateMaxScore = "StateMaxScore" + (i + 1);
                StateMaxScore[i] = PlayerPrefs.GetInt(stateMaxScore, 0);
            }
        }

        public void SaveData(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
    }
}
