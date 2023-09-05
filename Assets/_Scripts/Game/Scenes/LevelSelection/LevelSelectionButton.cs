using System;
using TMPro;
using UnityEngine;

namespace Game
{
    public class LevelSelectButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _label;
        int _index = -1;
        Action<int> _onClick;

        /// <param name="index">The index of the associated level</param>
        /// <param name="unlocked">Is the associated level locked?</param>
        /// <param name="onClick">callback method for this button</param>
        public void SetData(int index, bool unlocked, Action<int> onClick)
        {
        }

    }
}