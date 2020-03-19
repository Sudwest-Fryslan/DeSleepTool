using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZaakDocumentServices
{
    public class ZaakDocumentServices
    {
        private string standaardZaakDocumentServicesVrijBerichtService;
        private string standaardZaakDocumentServicesOntvangAsynchroonService;
        private string standaardZaakDocumentServicesBeantwoordVraagService;

        public ZaakDocumentServices(string standaardZaakDocumentServicesVrijBerichtService, string standaardZaakDocumentServicesOntvangAsynchroonService, string standaardZaakDocumentServicesBeantwoordVraagService)
        {
            this.standaardZaakDocumentServicesVrijBerichtService = standaardZaakDocumentServicesVrijBerichtService;
            this.standaardZaakDocumentServicesOntvangAsynchroonService = standaardZaakDocumentServicesOntvangAsynchroonService;
            this.standaardZaakDocumentServicesBeantwoordVraagService = standaardZaakDocumentServicesBeantwoordVraagService;
        }

        public ZaakNodeWrapper GeefZaakDetails(string zaakidentificatie)
        {
            var soapservice = new ZDSSoapService(
                standaardZaakDocumentServicesBeantwoordVraagService,
                "http://www.egem.nl/StUF/sector/zkn/0310/geefZaakdetails_Lv01");
            var requestdocument = new ZDSSoapService.ZDSXmlDocument("geefZaakdetails_Lv01.xml");

            requestdocument.SetNodeText("//StUF:referentienummer", DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            requestdocument.SetNodeText("//StUF:tijdstipBericht", DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            requestdocument.SetNodeText("//ZKN:gelijk/ZKN:identificatie", zaakidentificatie);

            return new ZaakNodeWrapper(soapservice.PerformRequest(requestdocument));
        }

        public ZaakDocumentWrapper[] GeefLijstZaakdocumenten(string zaakidentificatie)
        {
            var soapservice = new ZDSSoapService(
                standaardZaakDocumentServicesBeantwoordVraagService,
                "http://www.egem.nl/StUF/sector/zkn/0310/geefLijstZaakdocumenten_Lv01");
            var requestdocument = new ZDSSoapService.ZDSXmlDocument("geefLijstZaakdocumenten_Lv01.xml");

            requestdocument.SetNodeText("//StUF:referentienummer", DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            requestdocument.SetNodeText("//StUF:tijdstipBericht", DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            requestdocument.SetNodeText("//ZKN:gelijk/ZKN:identificatie", zaakidentificatie);
            //requestdocument.SetNodeText("//ZKN:scope/ZKN:object/ZKN:heeftRelevant/ZKN:gerelateerde/ZKN:identificatie", zaakidentificatie);

            var responsedocument = soapservice.PerformRequest(requestdocument);

            var documentnodes = responsedocument.SelectNodes("//ZKN:object/ZKN:heeftRelevant/ZKN:gerelateerde", responsedocument.NamespaceManager);
            var list = new List<ZaakDocumentWrapper>();
            foreach(XmlNode documentnode in documentnodes)
            {
                list.Add(new ZaakDocumentWrapper(responsedocument, documentnode));
            }
            return list.ToArray();
        }

        public string GenereerDocumentidentificatie(string zaakidentificatie)
        {
            var soapservice = new ZDSSoapService(
                standaardZaakDocumentServicesVrijBerichtService,
                "http://www.egem.nl/StUF/sector/zkn/0310/genereerDocumentIdentificatie_Di02");
            var requestdocument = new ZDSSoapService.ZDSXmlDocument("genereerDocumentIdentificatie_Di02.xml");
            var responsedocument = soapservice.PerformRequest(requestdocument);

            return responsedocument.GetNodeText("//ZKN:document/ZKN:identificatie");
        }

        public void VoegZaakdocumentToe(string zaakidentificatie, 
            string zaakdocumentidentificatie, 
            string zaakdocumenttype,
            DateTime creatiedatum,
            string titel,
            string formaat,
            string taal,
            string vertrouwelijkheid,
            string contenttype,
            string bestandsnaam,
            byte[] documentdata
            )
        {
            //TODO: check if exists
            var documenten = GeefLijstZaakdocumenten(zaakidentificatie);
            var dict = new Dictionary<string, ZaakDocumentWrapper>();
            foreach(var document in documenten)
            {
                if(!dict.ContainsKey(document.Titel)) {
                    dict.Add(document.Titel, document);
                }                
            }
            if (dict.ContainsKey(titel))
            {
                var name = titel;
                int i = 1;
                do
                {
                    // enige "rare" logica om toch netjes met extensies om te kunnen gaan
                    if (name.Length > 4 && name[name.Length - 4] == '.')
                    {
                        var begin = System.IO.Path.GetFileNameWithoutExtension(name);
                        var end = System.IO.Path.GetExtension(name);

                        titel = begin + "_" + i + end;
                    }
                    else { 
                        titel = name + "_" + i;
                    }
                    i++;
                }
                while (dict.ContainsKey(titel));
            }

            var soapservice = new ZDSSoapService(
                standaardZaakDocumentServicesOntvangAsynchroonService,
                "http://www.egem.nl/StUF/sector/zkn/0310/voegZaakdocumentToe_Lk01");
            var requestdocument = new ZDSSoapService.ZDSXmlDocument("voegZaakdocumentToe_Lk01.xml");

            requestdocument.SetNodeText("//StUF:referentienummer", DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            requestdocument.SetNodeText("//StUF:tijdstipBericht", DateTime.Now.ToString("yyyyMMddhhmmssfff"));

            requestdocument.SetNodeText("//ZKN:object/ZKN:identificatie", zaakdocumentidentificatie);
            requestdocument.SetNodeText("//ZKN:object/ZKN:dct.omschrijving", zaakdocumenttype);
            requestdocument.SetNodeText("//ZKN:object/ZKN:creatiedatum", creatiedatum.ToString("yyyyMMdd"));
            requestdocument.SetNodeText("//ZKN:object/ZKN:titel", titel);
            requestdocument.SetNodeText("//ZKN:object/ZKN:formaat", formaat);
            requestdocument.SetNodeText("//ZKN:object/ZKN:taal", taal);
            requestdocument.SetNodeText("//ZKN:object/ZKN:vertrouwelijkAanduiding", vertrouwelijkheid);
            requestdocument.SetNodeText("//ZKN:object/ZKN:auteur", System.Security.Principal.WindowsIdentity.GetCurrent().Name);

            requestdocument.SetAttributeText("//ZKN:object/ZKN:inhoud", "xmime:contentType", contenttype);
            requestdocument.SetAttributeText("//ZKN:object/ZKN:inhoud", "StUF:bestandsnaam", bestandsnaam);
            requestdocument.SetNodeText("//ZKN:object/ZKN:inhoud", Convert.ToBase64String(documentdata));

            requestdocument.SetNodeText("//ZKN:object/ZKN:isRelevantVoor/ZKN:gerelateerde/ZKN:identificatie", zaakidentificatie);

            var responsedocument = soapservice.PerformRequest(requestdocument);


            // TODO: use the documentid!!!!
            do
            {
                System.Threading.Thread.Sleep(500);

                documenten = GeefLijstZaakdocumenten(zaakidentificatie);
                dict = new Dictionary<string, ZaakDocumentWrapper>();
                System.Diagnostics.Debug.WriteLine("*** documenten in de zaak # " + zaakidentificatie + " , we zoeken naar: '" + titel + "' ***");
                foreach (var document in documenten)
                {
                    System.Diagnostics.Debug.WriteLine("\tgevonden document met titel: '" + document.Titel + "'");
                    if(!dict.ContainsKey(document.Titel)) {
                        dict.Add(document.Titel, document);
                    }                    
                }
            }
            while (!dict.ContainsKey(titel));
        }


        public void VoegZaakdocumentToe(string zaakIdentificatie, string zaakdocumentidentificatie, ZaakDocument document)
        {
            VoegZaakdocumentToe(zaakIdentificatie, zaakdocumentidentificatie, document.attributes.Documenttype, document.attributes.CreationTime, document.attributes.Titel, document.attributes.Formaat, document.attributes.Taal, document.attributes.Vertrouwelijkheid, document.attributes.Mimetype, document.attributes.Bestandsnaam, document.data);
        }
    }
}