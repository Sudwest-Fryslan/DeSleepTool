using System.Xml;

namespace ZaakDocumentServices
{
    public class ZaakDocumentWrapper
    {
        private ZDSSoapService.ZDSXmlDocument document;
        private XmlNode zaakdocumentnode;

        public ZaakDocumentWrapper(ZDSSoapService.ZDSXmlDocument document, XmlNode zaakdocumentnode)
        {
            this.document = document;
            this.zaakdocumentnode = zaakdocumentnode;
        }

        public string Titel {
            get
            {
                var childnodes = zaakdocumentnode.ChildNodes;
                foreach(XmlNode node in childnodes)
                {
                    if (node.Name == "titel")
                    {
                        return node.InnerText;
                    }
                }
                return null;
            }
        }
    }
}