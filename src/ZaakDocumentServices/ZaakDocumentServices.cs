using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace ZaakDocumentServices
{
    public delegate void Message(string message);
    public delegate void Notify();

    public enum TaskState { ToSendTask = 0, SendTask = 1, ErrorTask = 2}
    public class Task
    {
        public String Zaakidentificatie { get; set; }
        public String Documentidentificatie { get; set; }
        public String Filename { get; set; }
        public TaskState State { get; set; }
    }

    public class ZaakDocumentServices
    {

        public event Message InfoMessage;
        public event Message ErrorMessage;
        public event Notify Progress;

        private string dataDirectory;
        private string standaardZaakDocumentServicesVrijBerichtService;
        private string standaardZaakDocumentServicesOntvangAsynchroonService;
        private string standaardZaakDocumentServicesBeantwoordVraagService;
        private bool uploadInBackground;

        private Thread backgroundThread;

        private string FOLDER_ERROR;
        private string FOLDER_SENDING;
        private string FOLDER_TOSEND;
        private string FOLDER_SEND;


        public ZaakDocumentServices(string dataDirectory, string standaardZaakDocumentServicesVrijBerichtService, string standaardZaakDocumentServicesOntvangAsynchroonService, string standaardZaakDocumentServicesBeantwoordVraagService, bool uploadInBackground)
        {
            this.dataDirectory = dataDirectory;
            this.standaardZaakDocumentServicesVrijBerichtService = standaardZaakDocumentServicesVrijBerichtService;
            this.standaardZaakDocumentServicesOntvangAsynchroonService = standaardZaakDocumentServicesOntvangAsynchroonService;
            this.standaardZaakDocumentServicesBeantwoordVraagService = standaardZaakDocumentServicesBeantwoordVraagService;

            this.uploadInBackground = uploadInBackground;
        }

        public void Init()
        {
            if (this.uploadInBackground)
            {
                dataDirectory = Environment.ExpandEnvironmentVariables(dataDirectory);
                var di = new System.IO.DirectoryInfo(dataDirectory + Properties.Settings.Default.FOLDER_ERROR);
                di.Create();
                if (di.GetFiles().Length > 0)
                {
                    var message = "Found #" + di.GetFiles().Length + " errors in the directory:" + di.FullName;
                    Console.WriteLine(message);
                    ErrorMessage?.Invoke(message);
                }
                FOLDER_ERROR = di.FullName;

                di = new System.IO.DirectoryInfo(dataDirectory + Properties.Settings.Default.FOLDER_TOSEND);
                di.Create();
                if (di.GetFiles().Length > 0)
                {
                    var message = "Found #" + di.GetFiles().Length + " files to send in the directory:" + di.FullName;
                    Console.WriteLine(message);
                    InfoMessage?.Invoke(message);
                }
                FOLDER_TOSEND = di.FullName;

                di = new System.IO.DirectoryInfo(dataDirectory + Properties.Settings.Default.FOLDER_SENDING);
                di.Create();
                if (di.GetFiles().Length > 0)
                {
                    var message = "Found #" + di.GetFiles().Length + " files are being send in the directory:" + di.FullName;
                    Console.WriteLine(message);
                    InfoMessage?.Invoke(message);
                }
                FOLDER_SENDING = di.FullName;

                di = new System.IO.DirectoryInfo(dataDirectory + Properties.Settings.Default.FOLDER_SEND);
                di.Create();
                FOLDER_SEND = di.FullName;

                backgroundThread = new Thread(() => BackgroundThreadFunction());
                backgroundThread.Start();
            }
        }

        private string ExtractString(string input, string startDelimiter, string endDelimiter)
        {
            int startDelimiterIndex = input.IndexOf(startDelimiter) + startDelimiter.Length;
            int endDelimiterIndex = input.IndexOf(endDelimiter);

            if (startDelimiterIndex >= 0 && endDelimiterIndex >= 0)
            {
                int substringStartIndex = startDelimiterIndex;
                int substringLength = endDelimiterIndex - startDelimiterIndex;

                string extractedString = input.Substring(substringStartIndex, substringLength).Trim();
                return extractedString;
            }
            else
            {
                return null;
            }
        }

        private Task getTask(FileInfo fi, TaskState state)
        {
            var name = fi.Name;
            var zaakidentificatie = ExtractString(name, "-zaak", "-document");
            var documentidentificatie = ExtractString(name, "-document", ".xml");

            return new Task { Zaakidentificatie = zaakidentificatie, Documentidentificatie = documentidentificatie, Filename = fi.Name, State = state };
        }

        public Task[] getTasks()
        {
            var result = new List<Task>();
            var di = new DirectoryInfo(FOLDER_ERROR);
            foreach (FileInfo fi in di.GetFiles())
            {
                result.Add(getTask(fi, TaskState.ErrorTask));
            }
            di = new DirectoryInfo(FOLDER_SENDING);
            foreach (FileInfo fi in di.GetFiles())
            {
                result.Add(getTask(fi, TaskState.ToSendTask));
            }
            di = new DirectoryInfo(FOLDER_TOSEND);
            foreach (FileInfo fi in di.GetFiles())
            {
                result.Add(getTask(fi, TaskState.ToSendTask));
            }
            return result.ToArray();
        }

        public void BackgroundThreadFunction()
        {
            do
            {
                Progress?.Invoke();

                foreach (FileInfo filelocation in new DirectoryInfo(FOLDER_TOSEND).GetFiles())
                {
                    Console.WriteLine("Processing:" + filelocation.FullName);
                    var sendinglocation = new FileInfo(FOLDER_SENDING + filelocation.Name);
                    filelocation.MoveTo(sendinglocation.FullName);                    

                    Console.WriteLine("Sending:" + sendinglocation.FullName);
                    var soapservice = new ZDSSoapService(
                        standaardZaakDocumentServicesOntvangAsynchroonService,
                        "http://www.egem.nl/StUF/sector/zkn/0310/voegZaakdocumentToe_Lk01");
                    var requestdocument = new ZDSSoapService.ZDSXmlDocument(sendinglocation.FullName);
                    try { 
                        var responsedocument = soapservice.PerformRequest(requestdocument);
                        var sendlocation = new FileInfo(FOLDER_SEND + filelocation.Name);
                        Console.WriteLine("Send:" + sendlocation.FullName);
                        sendinglocation.MoveTo(sendlocation.FullName);
                    }
                    catch (Exception ex)
                    {
                        var errorlocation = new FileInfo(FOLDER_ERROR + filelocation.Name);
                        Console.WriteLine("Error:" + errorlocation.FullName);
                        sendinglocation.MoveTo(errorlocation.FullName);
                        ErrorMessage?.Invoke(ex.ToString());
                    }
                    Progress?.Invoke();
                }
                Console.WriteLine("...Waiting...");
                System.Threading.Thread.Sleep(100);
            } while (true);
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
            if (zaakidentificatie.Length == 0) return new ZaakDocumentWrapper[] { };

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
            // check if name exists
            var documenten = GeefLijstZaakdocumenten(zaakidentificatie);
            var dict = new Dictionary<string, ZaakDocumentWrapper>();
            foreach (var document in documenten)
            {
                if (!dict.ContainsKey(document.Titel))
                {
                    dict.Add(document.Titel, document);
                }
            }
            // and rename if necessary 
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
                    else
                    {
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


            if (uploadInBackground)
            {
                var filename = "voegZaakdocumentToe-zaak" + zaakidentificatie + "-document" + zaakdocumentidentificatie + ".xml";
                filename = FOLDER_TOSEND + filename;
                requestdocument.Save(filename);
            }
            else
            {
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
                        if (!dict.ContainsKey(document.Titel))
                        {
                            dict.Add(document.Titel, document);
                        }
                    }
                }
                while (!dict.ContainsKey(titel));
            }
        }

        public void VoegZaakdocumentToeUpload(string zaakIdentificatie, string zaakdocumentidentificatie, ZaakDocument document)
        {
            VoegZaakdocumentToe(zaakIdentificatie, zaakdocumentidentificatie, document.attributes.Documenttype, document.attributes.CreationTime, document.attributes.Titel, document.attributes.Formaat, document.attributes.Taal, document.attributes.Vertrouwelijkheid, document.attributes.Mimetype, document.attributes.Bestandsnaam, document.data);
        }

        public ZaakDocumentBytesWrapper geefZaakdocumentLezen(string documentidentificatie)
        {
            var soapservice = new ZDSSoapService(
                standaardZaakDocumentServicesBeantwoordVraagService,
                "http://www.egem.nl/StUF/sector/zkn/0310/geefZaakdocumentLezen_Lv01	");
            var requestdocument = new ZDSSoapService.ZDSXmlDocument("geefZaakdocumentLezen_Lv01.xml");

            requestdocument.SetNodeText("//StUF:referentienummer", DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            requestdocument.SetNodeText("//StUF:tijdstipBericht", DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            requestdocument.SetNodeText("//ZKN:gelijk/ZKN:identificatie", documentidentificatie);

            //return new ZaakNodeWrapper(soapservice.PerformRequest(requestdocument));

            return new ZaakDocumentBytesWrapper(soapservice.PerformRequest(requestdocument));
        }

        public void Close()
        {
            // niet zo netjes, maar werkt vast
            backgroundThread.Abort();
        }
    }
}