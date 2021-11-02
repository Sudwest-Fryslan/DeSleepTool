using System.Xml;

namespace ZaakDocumentServices
{
    public class ZaakDocumentBytesWrapper
    {
        private ZDSSoapService.ZDSXmlNode node;

        public ZaakDocumentBytesWrapper(XmlDocument document)
        {
            var d = new ZDSSoapService.ZDSXmlNode(document);
            this.node = d.GetNode("//ZKN:object");
        }

        public string Bestandsnaam {
            get
            {
                return node.GetAttributeText("./ZKN:inhoud", "ns1:bestandsnaam");
            }
        }
        public byte[] Bytes {
            get
            {
                return node.GetNodeBytes("./ZKN:inhoud");
            }
        }
    }
}