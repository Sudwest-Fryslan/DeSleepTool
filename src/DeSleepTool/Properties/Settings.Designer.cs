﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeSleepTool.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.5.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool OpenZaakDocuments {
            get {
                return ((bool)(this["OpenZaakDocuments"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:8080/translate/generic/zds/VrijBericht")]
        public string StandaardZaakDocumentServicesVrijBerichtService {
            get {
                return ((string)(this["StandaardZaakDocumentServicesVrijBerichtService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:8080/translate/generic/zds/OntvangAsynchroon")]
        public string StandaardZaakDocumentServicesOntvangAsynchroonService {
            get {
                return ((string)(this["StandaardZaakDocumentServicesOntvangAsynchroonService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:8080/translate/generic/zds/BeantwoordVraag")]
        public string StandaardZaakDocumentServicesBeantwoordVraagService {
            get {
                return ((string)(this["StandaardZaakDocumentServicesBeantwoordVraagService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool UploadInBackground {
            get {
                return ((bool)(this["UploadInBackground"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("%AppData%")]
        public string ApplicationDataDirectory {
            get {
                return ((string)(this["ApplicationDataDirectory"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <string>.docx</string>
  <string>.xlsx</string>
  <string>.pptx</string>
  <string>.odt</string>
  <string>.ods</string>
  <string>.odp</string>
  <string>.pdf</string>
  <string>.jpg</string>
  <string>.jpeg</string>
  <string>.png</string>
  <string>.gif</string>
  <string>.bmp</string>
  <string>.tif</string>
  <string>.tiff</string>
  <string>.txt</string>
  <string>.rtf</string>
  <string>.csv</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection AllowedExtensions {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["AllowedExtensions"]));
            }
        }
    }
}
