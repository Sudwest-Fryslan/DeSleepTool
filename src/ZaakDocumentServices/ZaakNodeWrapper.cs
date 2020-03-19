using System.Xml;

namespace ZaakDocumentServices
{
    public class ZaakNodeWrapper
    {
        //private ZDSSoapService.ZDSXmlDocument document;
        private ZDSSoapService.ZDSXmlNode node;

        public ZaakNodeWrapper(XmlDocument document)
        {
            var d = new ZDSSoapService.ZDSXmlNode(document);
            this.node = d.GetNode("//ZKN:object");
        }

        public ZaakNodeWrapper(XmlNode node)
        {
            this.node = new ZDSSoapService.ZDSXmlNode(node);
        }

        public string ZaakTypeOmschrijving
        {
            get
            {
                return node.GetNodeText("./ZKN:isVan/ZKN:gerelateerde/ZKN:omschrijving");
            }
        }


        public string ZaakOmschrijving {
            get
            {
                return node.GetNodeText("./ZKN:omschrijving");
            }
        }
        public string Afzender {
            get
            {
                if(node.NodeExists("./ZKN:heeftAlsInitiator/ZKN:gerelateerde/ZKN:natuurlijkPersoon")) { 
                    return node.GetNodeText("./ZKN:heeftAlsInitiator/ZKN:gerelateerde/ZKN:natuurlijkPersoon/BG:geslachtsnaam")
                         + ", " +
                        node.GetNodeText("./ZKN:heeftAlsInitiator/ZKN:gerelateerde/ZKN:natuurlijkPersoon/BG:voorletters");
                }
                else if (node.NodeExists("./ZKN:heeftAlsInitiator/ZKN:gerelateerde/ZKN:nietNatuurlijkPersoon"))
                {
                    return node.GetNodeText("./ZKN:heeftAlsInitiator/ZKN:gerelateerde/ZKN:nietNatuurlijkPersoon/BG:statutaireNaam");
                }
                else return "geen natuurlijkPersoon/nietNatuurlijkPersoon als initiator";
            }
        }

        public string ZaakIdentificatie {
            get
            {
                return node.GetNodeText("./ZKN:identificatie");
            }
        }

        public string ZaakTypeCode
        {
            get
            {
                return node.GetNodeText("./ZKN:isVan/ZKN:gerelateerde/ZKN:code");
            }
        }
    }
}