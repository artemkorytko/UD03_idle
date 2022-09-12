namespace Idle
{
    [System.Serializable]
    public class BuldingData
    {
        public readonly bool IsUnlock;
        public readonly int UpgradeLevel;

        public BuldingData()
        {
            IsUnlock = false;
            UpgradeLevel = 0;
        }

        public BuldingData(bool isUnlock, int upgradeLevel)
        {
            IsUnlock = isUnlock;
            UpgradeLevel = upgradeLevel;
        }
    }
}