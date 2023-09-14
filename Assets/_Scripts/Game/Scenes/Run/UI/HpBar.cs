using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Run.UI
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Image _hpBar;
        
        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
        public void OnUpdateHp(float remainingHpRatio)
        {
            _hpBar.fillAmount = remainingHpRatio;
        }

    }
}
