using UnityEngine;
using System.Collections;

namespace tomi.SaveSystem
{

    [System.Serializable]
    public class SaveData
    {

        #region SaveData - Other
        private static SaveData _current;

        public SaveData(PlayerProfile profile, PlayerGameData gameData)
        {
            _playerProfile = profile;
            _playerGameData = gameData;
        }

        public static SaveData Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new SaveData(_playerProfile, _playerGameData);
                }
                return _current;
            }
            set
            {
                if (value != null)
                {
                    _current = value;
                }
            }
        }

        #endregion

        #region Player Profile

        private static PlayerProfile _playerProfile;

        public static PlayerProfile PlayerProfile
        {
            get
            {
                if (_playerProfile == null)
                {
                    _playerProfile = new PlayerProfile();
                }
                return _playerProfile;
            }
            set
            {
                if (value != null)
                {
                    _playerProfile = value;
                }
            }
        }
        #endregion

        #region Player Game Data 
        private static PlayerGameData _playerGameData;

        public static PlayerGameData PlayerGameData
        {
            get
            {
                if (_playerGameData == null)
                {
                    _playerGameData = new PlayerGameData();
                }
                return _playerGameData;
            }
            set
            {
                if (value != null)
                {
                    _playerGameData = value;
                }
            }
        }
        #endregion

    }
}