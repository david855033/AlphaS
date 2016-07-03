namespace AlphaS.Settings
{
    public interface ISettingManager
    {
        string getSetting(string fieldName);
        void saveSetting(string fieldName, string Content);
    }
}