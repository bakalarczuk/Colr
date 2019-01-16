﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HabticActivityLibrary;

namespace Habtic.Games.Colr
{
    public class Game : HabticActivity
    {

        public override Dictionary<string, string> LocalizedStrings { get; set; } = new Dictionary<string, string>()
            {
                {"game_introlevel_text_tutorialone","Select color <color=\"red\">is written</color> in the center of color wheel" },
                {"game_introlevel_text_tutorialtwo","Select <color=\"red\">text color</color> written in the center of color wheel" },
                {"game_introlevel_text_tutorialthree","Select <color=\"red\">text color</color> written in the center of color wheel" },
                {"game_introlevel_text_tutorialfour","Select <color=\"red\">text color</color> written in the center of color wheel" },
                {"game_flash_text_level", "LEVEL"},
                {"game_scoremenu_text_messagenewhighscore", "NEW HIGHSCORE!"},
                {"game_level_text_questionfacing", "Swipe in the direction the central fish is <color=\"red\">facing</color>"},
                {"game_level_text_questionmoving", "Swipe in the direction the central fish is <color=\"red\">moving</color>"},
                {"game_mainmenu_buttontext_startgame", "START GAME" },
                {"game_mainmenu_buttontext_starttutorial", "PLAY TUTORIAL" },
                {"game_notify_time_out", "Time out" },
                {"game_notify_wrong_answer", "Wrong answer" },
                {"game_notify_good", "Good answer" },
			};

        private void Start()
        {
            StartActivity();

            int _curHighScore = -1;
            if (inputVariables.ContainsKey("inputhighscore"))
            {
                if (!string.IsNullOrEmpty(inputVariables["inputhighscore"]))
                {
                    if (int.TryParse(inputVariables["inputhighscore"], out _curHighScore))
                    {
                        if (_curHighScore != -1)
                        {
                            DataHolder.highscore = _curHighScore;
                            Debug.Log(DataHolder.highscore);
                        }
                    }
                }
            }

            int _curScore = -1;
            if (inputVariables.ContainsKey("inputscore"))
            {
                if (!string.IsNullOrEmpty(inputVariables["inputscore"]))
                {
                    if (int.TryParse(inputVariables["inputscore"], out _curScore))
                    {
                        if (_curScore != -1)
                        {
                            DataHolder.Score = _curScore;
                            Debug.Log(DataHolder.Score);
                        }
                    }
                }
            }

            int _playedIntroLevel = -1;
            if (inputVariables.ContainsKey("playedintrolevel"))
            {
                if (!string.IsNullOrEmpty(inputVariables["playedintrolevel"]))
                {
                    if (int.TryParse(inputVariables["playedintrolevel"], out _playedIntroLevel))
                    {
                        if (_playedIntroLevel != -1)
                        {
                            if (_playedIntroLevel == 0)
                            {
                                DataHolder.PlayedIntroLevel = false;
                            }
                            else if (_playedIntroLevel == 1)
                            {
                                DataHolder.PlayedIntroLevel = true;
                            }
                            Debug.Log(DataHolder.PlayedIntroLevel);
                        }
                    }
                }
            }

            int _showLevelMessage = -1;
            if (inputVariables.ContainsKey("showlevelmessage"))
            {
                if (!string.IsNullOrEmpty(inputVariables["showlevelmessage"]))
                {
                    if (int.TryParse(inputVariables["showlevelmessage"], out _showLevelMessage))
                    {
                        if (_showLevelMessage != -1)
                        {
                            if (_showLevelMessage == 0)
                            {
                                DataHolder.ShowLevelMessage = false;
                            }
                            else if (_playedIntroLevel == 1)
                            {
                                DataHolder.ShowLevelMessage = true;
                            }
                            Debug.Log(DataHolder.ShowLevelMessage);
                        }
                    }
                }
            }
        }

        public void Exit()
        {
            outputVariables["outputhighscore"] = DataHolder.highscore.ToString();
            outputVariables["outputscore"] = DataHolder.Score.ToString();


            if (DataHolder.PlayedIntroLevel)
            {
                outputVariables["playedintrolevel"] = "1";
            }
            else
            {
                outputVariables["playedintrolevel"] = "0";
            }

            if (DataHolder.ShowLevelMessage)
            {
                outputVariables["showlevelmessage"] = "1";
            }
            else
            {
                outputVariables["showlevelmessage"] = "0";
            }

            StopActivity();
        }
    }
}
