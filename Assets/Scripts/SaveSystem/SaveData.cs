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
        }

        public PlayerProfile playerProfile;
        public PlayerGameData playerGameData;

        public SaveData()
        {
            playerProfile = new PlayerProfile();
            playerGameData = new PlayerGameData();
        }
    }
}
