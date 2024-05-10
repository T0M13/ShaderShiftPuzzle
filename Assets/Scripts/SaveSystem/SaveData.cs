namespace tomi.SaveSystem
{
    [System.Serializable]
    public class SaveData
    {
        private static SaveData _current;
        public static SaveData Current
        {
            get
            {
                if (_current == null)
                    _current = new SaveData();
                return _current;
            }
            set
            {
                _current = value;
            }
        }

        public PlayerProfile playerProfile;
        public PlayerGameData playerGameData;
        public SaveMetaData saveMetaData;

        public SaveData()
        {
            playerProfile = new PlayerProfile();
            playerGameData = new PlayerGameData();
            saveMetaData = new SaveMetaData();
        }
    }
}
