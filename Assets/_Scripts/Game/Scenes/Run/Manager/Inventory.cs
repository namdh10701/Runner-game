using UnityEngine;
using Common;
using Game.Run.Level;
using TMPro;

namespace Game.Run
{
    /// <summary>
    /// A simple inventory class that listens to game events and keeps track of the amount of in-game currencies
    /// collected by the player
    /// </summary>
    public class Inventory : Singleton<Inventory>
    {
        [SerializeField] TextMeshProUGUI goldText;

        int m_TempGold;
        float m_TempXp;

        /*        Hud m_Hud;
                LevelCompleteScreen m_LevelCompleteScreen;*/

        void Start()
        {
            m_TempGold = 0;
            m_TempXp = 0;
            //TODO: Progess
            /*m_TotalGold = SaveManager.Instance.Currency;
            m_TotalXp = SaveManager.Instance.XP;*/

            /*            m_LevelCompleteScreen = UIManager.Instance.GetView<LevelCompleteScreen>();
                        m_Hud = UIManager.Instance.GetView<Hud>();
                        UIManager.Instance.Show<Hud>();*/
        }

        void OnEnable()
        {
        }

        void OnDisable()
        {
        }

        public void OnGoldPicked(Collectable collectables)
        {
            m_TempGold += 1; goldText.text = m_TempGold.ToString();
            //m_Hud.GoldValue = m_TempGold;
        }
        void OnWin()
        {
            //TODO: Progess
            /*m_LevelCompleteScreen.GoldValue = m_TempGold;
            m_LevelCompleteScreen.XpValue = m_TempXp;*/
        }

        void OnLose()
        {
            //TODO: Progess
            m_TempGold = 0;
        }
    }
}
