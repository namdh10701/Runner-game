using System;
namespace Game.Data
{
    [Serializable]
    public class GameSetting
    {
        public bool IsMusicOn;
        public bool IsSoundOn;
        public float MasterVolume;
        public float MusicVolume;
        public float SoundVolume;
        public bool IsVibrationOn;
        public GameSetting()
        {
            IsMusicOn = true;
            IsSoundOn = true;
            IsVibrationOn = true;
        }
    }
}
