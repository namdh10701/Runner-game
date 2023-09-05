using System;
using Game.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// A base class for the buttons of the hyper-casual game template that
    /// contains basic functionalities like button sound effect
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ButtonBase : MonoBehaviour
    {
        [SerializeField]
        protected Button _button;
        [SerializeField]
        SoundID m_ButtonSound = SoundID.Button_Click;

        Action m_Action;

        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        protected virtual void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        /// <summary>
        /// Adds a listener the button event.
        /// </summary>
        /// <param name="handler">callback function</param>
        public void AddListener(Action handler)
        {
            m_Action += handler;
        }

        /// <summary>
        /// Removes a listener from the button event.
        /// </summary>
        /// <param name="handler">callback function</param>
        public void RemoveListener(Action handler)
        {
            m_Action -= handler;
        }

        protected virtual void OnClick()
        {
            m_Action?.Invoke();
            PlayButtonSound();
        }

        protected void PlayButtonSound()
        {
            AudioManager.Instance.PlayEffect(m_ButtonSound);
        }
    }
}
