﻿namespace DeSleepTool
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtZaaktypeCode = new System.Windows.Forms.TextBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.txtZaakOmschrijving = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtZaakTypeOmschrijving = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAfzender = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtZaakIdentificatie = new System.Windows.Forms.TextBox();
            this.lvDocumenten = new System.Windows.Forms.ListView();
            this.lvTasks = new System.Windows.Forms.ListView();
            this.chState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDocument = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chZaak = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Location = new System.Drawing.Point(0, 601);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(444, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtZaaktypeCode);
            this.panel1.Controls.Add(this.btnPaste);
            this.panel1.Controls.Add(this.txtZaakOmschrijving);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtZaakTypeOmschrijving);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtAfzender);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtZaakIdentificatie);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(444, 272);
            this.panel1.TabIndex = 1;
            // 
            // txtZaaktypeCode
            // 
            this.txtZaaktypeCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtZaaktypeCode.Enabled = false;
            this.txtZaaktypeCode.Location = new System.Drawing.Point(303, 100);
            this.txtZaaktypeCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtZaaktypeCode.Name = "txtZaaktypeCode";
            this.txtZaaktypeCode.Size = new System.Drawing.Size(115, 26);
            this.txtZaaktypeCode.TabIndex = 11;
            // 
            // btnPaste
            // 
            this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
            this.btnPaste.Location = new System.Drawing.Point(378, 34);
            this.btnPaste.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(44, 35);
            this.btnPaste.TabIndex = 10;
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // txtZaakOmschrijving
            // 
            this.txtZaakOmschrijving.BackColor = System.Drawing.SystemColors.Control;
            this.txtZaakOmschrijving.Enabled = false;
            this.txtZaakOmschrijving.Location = new System.Drawing.Point(18, 158);
            this.txtZaakOmschrijving.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtZaakOmschrijving.Name = "txtZaakOmschrijving";
            this.txtZaakOmschrijving.Size = new System.Drawing.Size(402, 26);
            this.txtZaakOmschrijving.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 134);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Zaakomschrijving:";
            // 
            // txtZaakTypeOmschrijving
            // 
            this.txtZaakTypeOmschrijving.BackColor = System.Drawing.SystemColors.Control;
            this.txtZaakTypeOmschrijving.Enabled = false;
            this.txtZaakTypeOmschrijving.Location = new System.Drawing.Point(18, 98);
            this.txtZaakTypeOmschrijving.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtZaakTypeOmschrijving.Name = "txtZaakTypeOmschrijving";
            this.txtZaakTypeOmschrijving.Size = new System.Drawing.Size(274, 26);
            this.txtZaakTypeOmschrijving.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 74);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Zaaktype omschrijving:";
            // 
            // txtAfzender
            // 
            this.txtAfzender.BackColor = System.Drawing.SystemColors.Control;
            this.txtAfzender.Enabled = false;
            this.txtAfzender.Location = new System.Drawing.Point(18, 218);
            this.txtAfzender.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAfzender.Name = "txtAfzender";
            this.txtAfzender.Size = new System.Drawing.Size(402, 26);
            this.txtAfzender.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 194);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Afzender:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Zaakidentificatie:";
            // 
            // txtZaakIdentificatie
            // 
            this.txtZaakIdentificatie.Location = new System.Drawing.Point(18, 38);
            this.txtZaakIdentificatie.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtZaakIdentificatie.Name = "txtZaakIdentificatie";
            this.txtZaakIdentificatie.Size = new System.Drawing.Size(349, 26);
            this.txtZaakIdentificatie.TabIndex = 3;
            this.txtZaakIdentificatie.TextChanged += new System.EventHandler(this.txtZaakNummer_TextChanged);
            this.txtZaakIdentificatie.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtZaakIdentificatie_KeyDown);
            // 
            // lvDocumenten
            // 
            this.lvDocumenten.AllowDrop = true;
            this.lvDocumenten.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDocumenten.HideSelection = false;
            this.lvDocumenten.Location = new System.Drawing.Point(0, 272);
            this.lvDocumenten.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvDocumenten.Name = "lvDocumenten";
            this.lvDocumenten.Size = new System.Drawing.Size(444, 147);
            this.lvDocumenten.TabIndex = 2;
            this.lvDocumenten.UseCompatibleStateImageBehavior = false;
            this.lvDocumenten.View = System.Windows.Forms.View.List;
            this.lvDocumenten.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvDocumenten_DragDrop);
            this.lvDocumenten.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvDocumenten_DragEnter);
            this.lvDocumenten.DoubleClick += new System.EventHandler(this.lvDocumenten_DoubleClick);
            // 
            // lvTasks
            // 
            this.lvTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chState,
            this.chZaak,
            this.chDocument});
            this.lvTasks.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lvTasks.HideSelection = false;
            this.lvTasks.Location = new System.Drawing.Point(0, 419);
            this.lvTasks.Name = "lvTasks";
            this.lvTasks.Size = new System.Drawing.Size(444, 182);
            this.lvTasks.TabIndex = 3;
            this.lvTasks.UseCompatibleStateImageBehavior = false;
            this.lvTasks.View = System.Windows.Forms.View.Details;
            // 
            // chState
            // 
            this.chState.Text = "State";
            // 
            // chDocument
            // 
            this.chDocument.Text = "Document";
            // 
            // chZaak
            // 
            this.chZaak.Text = "Zaak";
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(444, 623);
            this.Controls.Add(this.lvDocumenten);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lvTasks);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.Text = "Zaakdocument Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtZaakIdentificatie;
        private System.Windows.Forms.TextBox txtZaakOmschrijving;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtZaakTypeOmschrijving;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAfzender;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lvDocumenten;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.TextBox txtZaaktypeCode;
        private System.Windows.Forms.ListView lvTasks;
        private System.Windows.Forms.ColumnHeader chState;
        private System.Windows.Forms.ColumnHeader chZaak;
        private System.Windows.Forms.ColumnHeader chDocument;
    }
}

