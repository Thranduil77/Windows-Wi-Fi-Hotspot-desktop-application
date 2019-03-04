#region Using

using System.Xml.Serialization;

#endregion

namespace WifiDemoApp_1
{
    [XmlRoot("HotspotData")]
    public class HotspotData
    {
        public string Name;

        public string Password;
	}
}