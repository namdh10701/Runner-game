using System;
using System.Collections.Generic;
using Game.Run.Level;
using Game.Run;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;
using Game.UI;

namespace Game
{
    /// <summary>
    /// This View contains level selection screen functionalities
    /// </summary>
    public class LevelSelectionScreen : View
    {
        public static LevelDefinition current;
        [SerializeField] LevelDefinition _lvl1Def;
        [SerializeField] LevelDefinition _lvl2Def;

        [Space]
        [SerializeField] RectTransform m_LevelButtonsRoot;
#if UNITY_EDITOR
        [SerializeField] bool m_UnlockAllLevels;
#endif
        readonly List<LevelSelectButton> m_Buttons = new();

        void Start()
        {
            //TODO: Progress
            /*var levels = SequenceManager.Instance.Levels;
            for (int i = 0; i < levels.Length; i++)
            {
                m_Buttons.Add(Instantiate(m_LevelButtonPrefab, m_LevelButtonsRoot));
            }
            ResetButtonData();*/
        }

        void OnEnable()
        {
            //ResetButtonData();
        }

        void OnDisable()
        {
        }

        void ResetButtonData()
        {
            //TODO: Progress
            /*var levelProgress = SaveManager.Instance.LevelProgress;
            for (int i = 0; i < m_Buttons.Count; i++)
            {
                var button = m_Buttons[i];
                var unlocked = i <= levelProgress;
#if UNITY_EDITOR
                unlocked = unlocked || m_UnlockAllLevels;
#endif
                button.SetData(i, unlocked, OnClick);
            }*/
        }

        void OnClick(int startingIndex)
        {
            if (startingIndex < 0)
                throw new Exception("Button is not initialized");
            //TODO: LOAD RUN SCENE HERE
            //SequenceManager.Instance.SetStartingLevel(startingIndex);
        }

        void OnBackButtonClicked()
        {
            SceneManager.LoadScene("Menu");
        }

        public void OnLVL1Clicked()
        {
            current = _lvl1Def;
            SceneManager.LoadScene("RunScene");
        }
        public void OnLVL2Clicked()
        {
            current = _lvl2Def;
            SceneManager.LoadScene("RunScene");
        }
    }
}
