using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZaakDocumentServices;

namespace DeSleepTool
{
    public partial class Main : Form
    {

        private ZaakDocumentServices.ZaakDocumentServices zds;
        private string ZaakIdentificatie = null;
        public Main()
        {
            InitializeComponent();
            var exe = new System.IO.FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Text += "-" + exe.LastWriteTime.ToString("yyyyMMdd");

            zds = new ZaakDocumentServices.ZaakDocumentServices(
                Properties.Settings.Default.ApplicationDataDirectory,
                Properties.Settings.Default.StandaardZaakDocumentServicesVrijBerichtService,
                Properties.Settings.Default.StandaardZaakDocumentServicesOntvangAsynchroonService,
                Properties.Settings.Default.StandaardZaakDocumentServicesBeantwoordVraagService,
                Properties.Settings.Default.UploadInBackground
            );
            zds.ErrorMessage += (message) =>
            {
                MessageBox.Show(message, "Error message");
            };
            zds.InfoMessage += (message) =>
            {
                MessageBox.Show(message, "Info message");
            };
            zds.Progress += () =>
            {
                FillTasks();
            };
        }

        private void FillTasks()
        {           
            // Update lvTasks on the UI thread
            lvTasks.Invoke(new Action(() =>
            {
                try { 
                    lvTasks.Visible = Properties.Settings.Default.UploadInBackground;
                    lvTasks.Items.Clear();
                    foreach (ZaakDocumentServices.Task task in zds.getTasks())
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.ImageIndex = (int) task.State;
                        lvi.SubItems.Add(task.Zaakidentificatie);
                        lvi.SubItems.Add(task.Documentidentificatie);
                        lvi.SubItems.Add(task.Filename);
                        lvTasks.Items.Add(lvi);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }));

            lvDocumenten.Invoke(new Action(() =>
            {
                try
                {
                    if (ZaakIdentificatie == null)
                    {
                        return;
                    }
                    lvDocumenten.Items.Clear();

                    var documenten = zds.GeefLijstZaakdocumenten(ZaakIdentificatie);
                    foreach (var document in documenten)
                    {
                        var lvi = new ListViewItem(document.Titel);
                        lvi.Tag = document;
                        lvDocumenten.Items.Add(lvi);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }));
        }

        private void txtZaakNummer_TextChanged(object sender, EventArgs e)
        {
            // RefreshData();
        }

        void RefreshData()
        {
            txtZaakTypeOmschrijving.Text = "";
            txtZaaktypeCode.Text = "";
            txtZaakOmschrijving.Text = "";
            txtAfzender.Text = "";
            lvDocumenten.Clear();
            lvDocumenten.Enabled = false;

            if (ZaakIdentificatie == null)
            {
                return;
            }

            ZaakDocumentServices.ZaakNodeWrapper zaak = null;
            try
            {
                zaak = zds.GeefZaakDetails(ZaakIdentificatie);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                ZaakIdentificatie = null;
                return;
            }
            txtZaakTypeOmschrijving.Text = zaak.ZaakTypeOmschrijving;
            txtZaaktypeCode.Text = zaak.ZaakTypeCode;
            txtZaakOmschrijving.Text = zaak.ZaakOmschrijving;            
            txtAfzender.Text = zaak.Afzender;


            var documenten = zds.GeefLijstZaakdocumenten(ZaakIdentificatie);
            foreach (var document in documenten)
            {
                var lvi = new ListViewItem(document.Titel);
                lvi.Tag = document;
                lvDocumenten.Items.Add(lvi);
            }
            lvDocumenten.Enabled = true;
        }

        private void AddFileToZds(UploadDocument document)
        {
            bool allowedExtension = false;
            foreach (string extension in Properties.Settings.Default.AllowedExtensions)
            {
                if (document.fileinfo.Extension.Equals(extension))
                {
                    allowedExtension = true;
                }
            }
            if (!allowedExtension)
            {
                MessageBox.Show("De extensie van het bestand: '" + document.fileinfo + "' wordt niet ondersteund. De ondersteunde extenties zijn:" + string.Join(", ", Properties.Settings.Default.AllowedExtensions.Cast<string>().ToArray()), "Niet ondersteunde extensie");
                return;
            }

            var zaakdocumentid = zds.GenereerDocumentidentificatie(txtZaakIdentificatie.Text);
            var mimetype = ZaakDocumentServices.Mime.GetMime(document.fileinfo.FullName, document.filedata);

            //var documentmapping = new ZaakDocumentServices.DocumentMapping(txtZaaktypeCode.Text, documentfile.Name, documentfile.CreationTime);
            var documentmapping = ZaakDocumentServices.ZaakDocumentAttributes.ExtractDocumentAttributes(txtZaaktypeCode.Text, document.fileinfo.Name, mimetype, document.fileinfo.CreationTime);

            zds.VoegZaakdocumentToe(
                txtZaakIdentificatie.Text,
                zaakdocumentid,
                documentmapping.Documenttype,
                documentmapping.CreationTime,
                documentmapping.Titel,
                documentmapping.Formaat,
                documentmapping.Taal,
                documentmapping.Vertrouwelijkheid,
                documentmapping.Mimetype,
                documentmapping.Bestandsnaam,
                document.filedata);
        }

        struct UploadDocument {
            public System.IO.FileInfo fileinfo;
            public byte[] filedata;

        }
        UploadDocument[] filesToUpload;

        private void tmrUploadFiles_Tick(object sender, EventArgs e)
        {
            tmrUploadFiles.Stop();
            this.Activate();

            if (filesToUpload != null)
            {
#if !DEBUG
            try
            {
#endif

                if (txtZaakIdentificatie.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Geen Zaakidentificatie ingevoerd!");
                    return;
                }

                foreach (UploadDocument document in filesToUpload)
                {
                    AddFileToZds(document);
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fout bij toevoegen van documenten aan de zaak", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
                filesToUpload = null;
            }
            RefreshData();
        }

        private void lvDocumenten_DragDrop(object sender, DragEventArgs e)
        {
            var uploads = new List<UploadDocument>();

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files= (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach(String file in files)
                {
                    var upload = new UploadDocument();
                    upload.fileinfo = new System.IO.FileInfo(file);
                    upload.filedata = System.IO.File.ReadAllBytes(upload.fileinfo.FullName);
                    uploads.Add(upload);
                }
            }
            else if (e.Data.GetDataPresent("FileGroupDescriptor"))
            {
                OutlookDataObject dataObject = new OutlookDataObject(e.Data);
                string[] filenames = (string[])dataObject.GetData("FileGroupDescriptor");
                System.IO.MemoryStream[] streams = (System.IO.MemoryStream[])dataObject.GetData("FileContents");
                for (int i = 0; i < filenames.Length; i++)
                {
                    var upload = new UploadDocument();
                    upload.fileinfo = new System.IO.FileInfo(filenames[i]);
                    System.IO.MemoryStream stream = streams[i];
                    upload.filedata = stream.ToArray();
                    uploads.Add(upload);
                }
            }
            else throw new Exception("unexpected format in drap/drop");            

            if(uploads.Count > 0) {
                filesToUpload = uploads.ToArray();
                tmrUploadFiles.Start();
            }
        }

        private void lvDocumenten_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent("FileGroupDescriptor"))
            {
                    e.Effect = DragDropEffects.Copy;
            }
            else { 
                e.Effect = DragDropEffects.None;
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            txtZaakIdentificatie.Text = Clipboard.GetText();
            ZaakIdentificatie = txtZaakIdentificatie.Text;
            RefreshData();
        }

        private void txtZaakIdentificatie_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ZaakIdentificatie = txtZaakIdentificatie.Text;
                RefreshData();
            }
        }

        private void ShowZaakDocument(ListViewItem lvi)
        {
            if (Properties.Settings.Default.OpenZaakDocuments)
            {

                Cursor.Current = Cursors.WaitCursor;

                ZaakDocumentWrapper zdw = (ZaakDocumentWrapper)lvi.Tag;
                ZaakDocumentBytesWrapper zdbw = zds.geefZaakdocumentLezen(zdw.Identificatie);
                //var filename = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + zdbw.Bestandsnaam;                
                var filename = System.IO.Path.GetTempPath() + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + zdbw.Bestandsnaam;

                System.IO.File.WriteAllBytes(filename, zdbw.Bytes);


                // Start the associated application
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = filename,
                        UseShellExecute = true // Let the OS decide what to do with the file
                    };

                    process.EnableRaisingEvents = true; // Enable the Exited event

                    process.Exited += (psender, pe) =>
                    {
                        // Delete the temporary file when the application is closed
                        File.Delete(filename);
                    };

                    Cursor.Current = Cursors.Default;
                    process.Start();
                }
            }
        }

        private void lvDocumenten_DoubleClick(object sender, EventArgs e)
        {
            // Check if an item is selected
            if (lvDocumenten.SelectedItems.Count > 0)
            {
                ShowZaakDocument(lvDocumenten.SelectedItems[0]);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            zds.Init();
            FillTasks();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            var tasks = zds.getTasks();
            if(tasks.Length > 0) { 
                MessageBox.Show("Er zijn nog:" + tasks.Length + " taken te verzenden. Zorg ervoor dat deze nog verzonden worden!");
            }
            zds.Close();
        }
    }
}