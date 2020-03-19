
using System;
using System.IO;
using System.Xml;

namespace ZaakDocumentServices
{
    public class ZaakDocumentAttributes
    {
        public string ZaaktypeCode;
        public string Mimetype;
        public string Documenttype;
        public DateTime CreationTime;
        public string Titel;
        public string Taal;
        public string Vertrouwelijkheid;
        // Goed, we hebben nu titel en name maar in één gestopt... 
        // Dit omdat het raar deed
        public string Bestandsnaam;
        public string Formaat;

        public static ZaakDocumentAttributes ExtractDocumentAttributes(string zaaktypecode, string documentfilename, string mimetype, DateTime creationdate)
        {
            var result = new ZaakDocumentAttributes();
            result.ZaaktypeCode = zaaktypecode;

            result.Mimetype = mimetype;
            result.Documenttype = "documenttype";
            result.CreationTime = creationdate;

            result.Taal = "nld";
            result.Vertrouwelijkheid = "VERTROUWELIJK";

            result.Titel = documentfilename.Contains(".") ? documentfilename.Substring(0, documentfilename.IndexOf(".")) : documentfilename;
            result.Bestandsnaam = documentfilename;
            result.Formaat = documentfilename.Contains(".") ? documentfilename.Substring(documentfilename.IndexOf(".") + 1) : documentfilename;

            var config = new System.Xml.XmlDocument();
            config.Load("mapping.xml");

            var zaaktypes = config.SelectNodes("//zaaktype");
            foreach (System.Xml.XmlNode zt in zaaktypes)
            {
                if (zt.Attributes["matchfield"] == null ||
                    (zt.Attributes["matchvalue"] != null
                        && zt.Attributes["matchfield"] != null
                        && zt.Attributes["matchfield"].Value == "code"
                        && result.ZaaktypeCode.ToLower().Contains(zt.Attributes["matchvalue"].Value.ToLower())))
                {
                    ChooseDocumentType(zt, result);
                    return result;
                }
            }
            throw new Exception("Aan het zaaktype met code: " + zaaktypecode + " kunnen geen documenten worden toegevoegd, omdat dit niet is ingesteld");
        }

        public static ZaakDocumentAttributes ChooseDocumentType(XmlNode zaaktype, ZaakDocumentAttributes result)
        {
            var documenten = zaaktype.SelectNodes("document");
            foreach (System.Xml.XmlNode document in documenten)
            {
                if(document.Attributes["matchfield"] != null && document.Attributes["matchfield"].Value == "naam")
                {
                    throw new Exception("matchfield='naam' mag niet langer gebruikt worden, gebruik nu: matchfield='bestandsnaam' in de mapping.xml"); 
                }

                if(document.Attributes["matchfield"] == null ||
                    document.Attributes["matchfield"] != null
                    && document.Attributes["matchvalue"] != null
                    && document.Attributes["matchfield"].Value == "bestandsnaam"
                    && result.Bestandsnaam.ToLower().Contains(document.Attributes["matchvalue"].Value.ToLower() ))
                {
                    var titelnode = document.SelectSingleNode("titel");
                    if (titelnode != null)
                    {
                        if (titelnode.Attributes["bestandsnaam"] != null && titelnode.Attributes["bestandsnaam"].Value == "true")
                        {
                            result.Titel = result.Bestandsnaam;
                        }
                        else { 
                            result.Titel = titelnode.InnerText;
                        }
                    }
                    var typenode = document.SelectSingleNode("type");
                    if(typenode == null)
                    {
                        throw new Exception("fout in mapping.xml: type is verplicht bij alle document elementen (bij zaaktype:" + result.ZaaktypeCode + ")");
                    }
                    result.Documenttype = typenode.InnerText;
                    var vertrouwelijkheidnode = document.SelectSingleNode("vertrouwelijkheid");
                    if (vertrouwelijkheidnode != null) result.Vertrouwelijkheid = vertrouwelijkheidnode.InnerText.ToUpper();

                    return result;
                }
            }
            throw new Exception("Aan het zaaktype met code: " + result.ZaaktypeCode + " kon niet een document met bestandsnaam:" + result.Bestandsnaam + " worden toegevoegd, omdat dit niets kon worden gevonden in de mapping.xml");
        }

        public static string[] ZaakDocumentTypes(string ZaakTypeCode)
        {
            var config = new System.Xml.XmlDocument();
            config.Load("mapping.xml");

            var zaaktypes = config.SelectNodes("//zaaktype");

            var founddocumenttypes = new System.Collections.Generic.List<string>();
            foreach (System.Xml.XmlNode zt in zaaktypes)
            {
                if (zt.Attributes["matchfield"] == null && zt.Attributes["matchvalue"] == null)
                {
                    // catch-all doen we alleen wanneer geen andere documenttypes gevonden
                    if (founddocumenttypes.Count == 0) { 
                        foreach (System.Xml.XmlNode type in zt.SelectNodes("document/type"))
                        {
                            var name = type.InnerText;
                            if (!founddocumenttypes.Contains(name))
                            {
                                founddocumenttypes.Add(name);
                            }
                        }
                    }
                }
                else if (zt.Attributes["matchfield"] != null && zt.Attributes["matchvalue"] != null)
                {
                    if (zt.Attributes["matchfield"].Value == "code" && zt.Attributes["matchvalue"].Value == ZaakTypeCode)
                    {
                        // de code is gevonden
                        foreach (System.Xml.XmlNode type in zt.SelectNodes("document/type"))
                        {
                            var name = type.InnerText;
                            if (!founddocumenttypes.Contains(name))
                            {
                                founddocumenttypes.Add(name);
                            }
                        }
                    }
                }
            }
            if (founddocumenttypes.Count == 0) throw new Exception("geen documenttypes gevonden voor zaaktype:" + ZaakTypeCode);

            founddocumenttypes.Sort();
            return founddocumenttypes.ToArray();
        }

        public static bool VisibleZaak(string ZaakTypeCode)
        {   
            var config = new System.Xml.XmlDocument();
            config.Load("mapping.xml");

            var zaaktypes = config.SelectNodes("//zaaktype");
            foreach (System.Xml.XmlNode zt in zaaktypes)
            {
                if (zt.Attributes["matchfield"] == null && zt.Attributes["matchvalue"] == null)
                {
                    // catch-all
                    return true;
                }                
                if (zt.Attributes["matchfield"] != null && zt.Attributes["matchvalue"] != null)
                {
                    if (zt.Attributes["matchfield"].Value == "code" && zt.Attributes["matchvalue"].Value == ZaakTypeCode)
                    {
                        // de code is gevonden
                        return true;
                    }
                }
            }
            return false;
        }
    }
}