using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Common;
using System.Collections;

namespace Game.Audio
{
    /// <summary>
    /// Handles playing sounds and music based on their sound ID
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        [Serializable]
        class SoundIDClipPair
        {
            public SoundID m_SoundID;
            public AudioClip m_AudioClip;
        }

        [SerializeField] AudioSource _musicSource;

        [SerializeField] AudioSource _soundSource;

        [SerializeField, Min(0f)] float _minSoundInterval = 0.1f;

        [SerializeField] SoundIDClipPair[] _sounds;

        float m_LastSoundPlayTime;
        readonly Dictionary<SoundID, AudioClip> _clips = new();


        /// <summary>
        /// Unmute/mute the music
        /// </summary>
        public bool IsMusicOn
        {
            get => SettingManager.Instance.GameSettings.IsMusicOn;
            set
            {
                SettingManager.Instance.GameSettings.IsMusicOn = value;
                _musicSource.mute = !value;
            }
        }

        /// <summary>
        /// Unmute/mute all sound effects
        /// </summary>
        public bool IsSoundOn
        {
            get => SettingManager.Instance.GameSettings.IsSoundOn;
            set
            {
                SettingManager.Instance.GameSettings.IsSoundOn = value;
                _soundSource.mute = !value;
            }
        }

        /// <summary>
        /// The Master volume of the audio listener
        /// </summary>
        public float MasterVolume
        {
            get => SettingManager.Instance.GameSettings.MasterVolume;
            set
            {
                SettingManager.Instance.GameSettings.MasterVolume = value;
                AudioListener.volume = value;
            }
        }

        void Start()
        {
            foreach (var sound in _sounds)
            {
                _clips.Add(sound.m_SoundID, sound.m_AudioClip);
            }
        }

        void OnEnable()
        {
            IsMusicOn = SettingManager.Instance.GameSettings.IsMusicOn;
            IsSoundOn = SettingManager.Instance.GameSettings.IsSoundOn;
            MasterVolume = SettingManager.Instance.GameSettings.MasterVolume;
        }

        void OnDisable()
        {
            SettingManager.Instance.SaveSettings();
        }

        void PlayMusic(AudioClip audioClip, bool looping = true)
        {
            if (_musicSource.isPlaying)
                return;

            _musicSource.clip = audioClip;
            _musicSource.loop = looping;
            _musicSource.Play();
        }

        /// <summary>
        /// Play a music based on its sound ID
        /// </summary>
        /// <param name="soundID">The ID of the music</param>
        /// <param name="looping">Is music looping?</param>
        public void PlayMusic(SoundID soundID, bool looping = true)
        {
            PlayMusic(_clips[soundID], looping);
        }

        /// <summary>
        /// Stop the current music
        /// </summary>
        public void StopMusic()
        {
            _musicSource.Stop();
        }

        void PlayEffect(AudioClip audioClip)
        {
            if (Time.time - m_LastSoundPlayTime >= _minSoundInterval)
            {
                _soundSource.PlayOneShot(audioClip);
                m_LastSoundPlayTime = Time.time;
            }
        }

        /// <summary>
        /// Play a sound effect based on its sound ID
        /// </summary>
        /// <param name="soundID">The ID of the sound effect</param>
        public void PlayEffect(SoundID soundID)
        {
            if (soundID == SoundID.None)
                return;

            PlayEffect(_clips[soundID]);
        }

        public void CrossfadeMusic(AudioClip newClip, float fadeDuration)
        {
            StartCoroutine(CrossfadeMusicCoroutine(newClip, fadeDuration));
        }

        private IEnumerator CrossfadeMusicCoroutine(AudioClip newClip, float fadeDuration)
        {
            if (_musicSource.clip == newClip)
            {
                yield break;
            }
            float startVolume = _musicSource.volume;
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
                yield return null;
            }

            _musicSource.Stop();
            _musicSource.clip = newClip;
            _musicSource.Play();

            timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(0f, startVolume, timer / fadeDuration);
                yield return null;
            }
        }
    }
}