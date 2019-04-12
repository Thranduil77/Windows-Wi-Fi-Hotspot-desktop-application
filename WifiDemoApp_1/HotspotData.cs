using System.Xml.Serialization;

namespace WifiDemoApp_1
{
    #region Using

    #endregion

    [XmlRoot("HotspotData")]
    public class HotspotData
    {
        public string Name;

        public string Password;
    }
}