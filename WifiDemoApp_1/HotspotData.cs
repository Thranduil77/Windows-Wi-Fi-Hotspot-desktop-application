namespace WifiDemoApp_1
{
    #region Using

    using System.Xml.Serialization;

    #endregion

    [XmlRoot("HotspotData")]
    public class HotspotData
    {
        public string Name;

        public string Password;
    }
}