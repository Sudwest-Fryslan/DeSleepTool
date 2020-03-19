using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZaakDocumentServices
{
    public class StufZkn0310
    {
        private string standaardStufZkn0310BeantwoordVraag;

        public StufZkn0310(string standaardStufZkn0310BeantwoordVraag)
        {
            this.standaardStufZkn0310BeantwoordVraag = standaardStufZkn0310BeantwoordVraag;
        }

        public ZaakNodeWrapper[] GeefZakenMetInitiator(string inititiatoridentificatie)
        {
            //Beginnen met de code, gevolgd door de identificatie
            //                11: Ingezetene
            //                12: Niet - ingezetene
            //                13: Ander natuurlijk persoon
            //                21: Ingeschreven niet-natuurlijk persoon
            //                23: Ander buitenlands niet - natuurlijk persoon
            //                31: Vestiging

            var soapservice = new ZDSSoapService(
                standaardStufZkn0310BeantwoordVraag,
                "http://www.egem.nl/StUF/sector/zkn/0310/zakLv01");
            var requestdocument = new ZDSSoapService.ZDSXmlDocument("zakLv01.xml");

            requestdocument.SetNodeText("//StUF:referentienummer", DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            requestdocument.SetNodeText("//StUF:tijdstipBericht", DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            //requestdocument.SetNodeText("//ZKN:gelijk/ZKN:identificatie", zaakidentificatie);
            requestdocument.SetNodeText("//ZKN:gelijk/ZKN:heeftAlsInitiator/ZKN:gerelateerde/ZKN:identificatie", inititiatoridentificatie);

            var responsedocument = soapservice.PerformRequest(requestdocument);
            // return new Zaak(soapservice.PerformRequest(requestdocument));

            // var zaaknodes = responsedocument.SelectNodes("//ZKN:object[StUF:entiteittype='ZAK']", responsedocument.NamespaceManager);
            var zaaknodes = responsedocument.SelectNodes("//ZKN:antwoord/ZKN:object", responsedocument.NamespaceManager);
            var list = new List<ZaakNodeWrapper>();
            foreach (XmlNode documentnode in zaaknodes)
            {
                var zaak = new ZaakNodeWrapper(documentnode);
                list.Add(zaak);
                Console.WriteLine(zaak.ZaakOmschrijving);
                //list.Add(new Zaak(documentnode));
            }
            return list.ToArray();
        }
    }
}