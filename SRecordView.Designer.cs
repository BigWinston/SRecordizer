namespace SRecordizer
{
    partial class SRecordView
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
            try
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
            catch { }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SRecordView));
            this.invisibleControl = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.infoLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurrentAddress = new System.Windows.Forms.ToolStripStatusLabel();
            this.s19ListView = new BrightIdeasSoftware.FastObjectListView();
            this.lineNumColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.instructionColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.sizeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.addressColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.dataColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.checksumColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.asciiColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.editContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.insertRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertRowBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.s19ListView)).BeginInit();
            this.editContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // invisibleControl
            // 
            this.invisibleControl.Location = new System.Drawing.Point(748, 85);
            this.invisibleControl.Name = "invisibleControl";
            this.invisibleControl.Size = new System.Drawing.Size(75, 23);
            this.invisibleControl.TabIndex = 2;
            this.invisibleControl.Text = "button1";
            this.invisibleControl.UseVisualStyleBackColor = true;
            this.invisibleControl.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoLabel,
            this.toolStripStatusLabel1,
            this.lblCurrentAddress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 240);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1034, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // infoLabel
            // 
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(23, 17);
            this.infoLabel.Text = ">>";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(890, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // lblCurrentAddress
            // 
            this.lblCurrentAddress.Name = "lblCurrentAddress";
            this.lblCurrentAddress.Size = new System.Drawing.Size(106, 17);
            this.lblCurrentAddress.Text = "Current Address = ";
            // 
            // s19ListView
            // 
            this.s19ListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.s19ListView.AllColumns.Add(this.lineNumColumn);
            this.s19ListView.AllColumns.Add(this.instructionColumn);
            this.s19ListView.AllColumns.Add(this.sizeColumn);
            this.s19ListView.AllColumns.Add(this.addressColumn);
            this.s19ListView.AllColumns.Add(this.dataColumn);
            this.s19ListView.AllColumns.Add(this.checksumColumn);
            this.s19ListView.AllColumns.Add(this.asciiColumn);
            this.s19ListView.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
            this.s19ListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.s19ListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.s19ListView.CellEditEnterChangesRows = true;
            this.s19ListView.CellEditTabChangesRows = true;
            this.s19ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lineNumColumn,
            this.instructionColumn,
            this.sizeColumn,
            this.addressColumn,
            this.dataColumn,
            this.checksumColumn});
            this.s19ListView.ContextMenuStrip = this.editContextMenu;
            this.s19ListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.s19ListView.EmptyListMsg = "File is empty...";
            this.s19ListView.EmptyListMsgFont = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.s19ListView.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.s19ListView.FullRowSelect = true;
            this.s19ListView.GridLines = true;
            this.s19ListView.HasCollapsibleGroups = false;
            this.s19ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.s19ListView.HighlightBackgroundColor = System.Drawing.Color.Purple;
            this.s19ListView.HighlightForegroundColor = System.Drawing.Color.Yellow;
            this.s19ListView.Location = new System.Drawing.Point(0, 0);
            this.s19ListView.MultiSelect = false;
            this.s19ListView.Name = "s19ListView";
            this.s19ListView.ShowGroups = false;
            this.s19ListView.ShowItemToolTips = true;
            this.s19ListView.Size = new System.Drawing.Size(1034, 240);
            this.s19ListView.TabIndex = 4;
            this.s19ListView.UseCellFormatEvents = true;
            this.s19ListView.UseCompatibleStateImageBehavior = false;
            this.s19ListView.View = System.Windows.Forms.View.Details;
            this.s19ListView.VirtualMode = true;
            this.s19ListView.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.s19ListView_CellEditFinishing);
            this.s19ListView.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.s19ListView_CellEditStarting);
            this.s19ListView.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.s19ListView_FormatRow);
            this.s19ListView.SelectionChanged += new System.EventHandler(this.s19ListView_SelectionChanged);
            // 
            // lineNumColumn
            // 
            this.lineNumColumn.AspectName = "LineNumber";
            this.lineNumColumn.IsEditable = false;
            this.lineNumColumn.Text = "#";
            this.lineNumColumn.Width = 38;
            // 
            // instructionColumn
            // 
            this.instructionColumn.AspectName = "Instruction";
            this.instructionColumn.AutoCompleteEditor = false;
            this.instructionColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.instructionColumn.Text = "Sx";
            this.instructionColumn.Width = 66;
            // 
            // sizeColumn
            // 
            this.sizeColumn.AspectName = "Size";
            this.sizeColumn.AspectToStringFormat = "{0:X2}";
            this.sizeColumn.AutoCompleteEditor = false;
            this.sizeColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.sizeColumn.Text = "Size";
            this.sizeColumn.Width = 49;
            // 
            // addressColumn
            // 
            this.addressColumn.AspectName = "Address";
            this.addressColumn.AspectToStringFormat = "{0:X8}";
            this.addressColumn.AutoCompleteEditor = false;
            this.addressColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.addressColumn.Text = "Address";
            this.addressColumn.Width = 69;
            // 
            // dataColumn
            // 
            this.dataColumn.AspectName = "Data";
            this.dataColumn.AutoCompleteEditor = false;
            this.dataColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.dataColumn.Text = "Data";
            this.dataColumn.Width = 426;
            // 
            // checksumColumn
            // 
            this.checksumColumn.AspectName = "Checksum";
            this.checksumColumn.AspectToStringFormat = "{0:X2}";
            this.checksumColumn.AutoCompleteEditor = false;
            this.checksumColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.checksumColumn.Text = "Csum";
            // 
            // asciiColumn
            // 
            this.asciiColumn.AspectName = "Ascii";
            this.asciiColumn.DisplayIndex = 6;
            this.asciiColumn.IsEditable = false;
            this.asciiColumn.IsVisible = false;
            this.asciiColumn.Text = "ASCII";
            this.asciiColumn.Width = 232;
            // 
            // editContextMenu
            // 
            this.editContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertRowToolStripMenuItem,
            this.insertRowBelowToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteRowsToolStripMenuItem});
            this.editContextMenu.Name = "editContextMenu";
            this.editContextMenu.Size = new System.Drawing.Size(175, 76);
            // 
            // insertRowToolStripMenuItem
            // 
            this.insertRowToolStripMenuItem.Image = global::SRecordizer.Properties.Resources.document_hf_insert;
            this.insertRowToolStripMenuItem.Name = "insertRowToolStripMenuItem";
            this.insertRowToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.insertRowToolStripMenuItem.Text = "Insert Row (&Above)";
            this.insertRowToolStripMenuItem.Click += new System.EventHandler(this.insertRowToolStripMenuItem_Click);
            // 
            // insertRowBelowToolStripMenuItem
            // 
            this.insertRowBelowToolStripMenuItem.Image = global::SRecordizer.Properties.Resources.document_hf_insert_footer;
            this.insertRowBelowToolStripMenuItem.Name = "insertRowBelowToolStripMenuItem";
            this.insertRowBelowToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.insertRowBelowToolStripMenuItem.Text = "Insert Row (&Below)";
            this.insertRowBelowToolStripMenuItem.Click += new System.EventHandler(this.insertRowBelowToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(171, 6);
            // 
            // deleteRowsToolStripMenuItem
            // 
            this.deleteRowsToolStripMenuItem.Image = global::SRecordizer.Properties.Resources.document__minus;
            this.deleteRowsToolStripMenuItem.Name = "deleteRowsToolStripMenuItem";
            this.deleteRowsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.deleteRowsToolStripMenuItem.Text = "&Delete Row(s)";
            this.deleteRowsToolStripMenuItem.Click += new System.EventHandler(this.deleteRowsToolStripMenuItem_Click);
            // 
            // SRecordView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 262);
            this.Controls.Add(this.s19ListView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.invisibleControl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SRecordView";
            this.Text = "SrecordView";
            this.Load += new System.EventHandler(this.SRecordView_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.s19ListView)).EndInit();
            this.editContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button invisibleControl;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel infoLabel;
        private BrightIdeasSoftware.FastObjectListView s19ListView;
        private BrightIdeasSoftware.OLVColumn lineNumColumn;
        private BrightIdeasSoftware.OLVColumn instructionColumn;
        private BrightIdeasSoftware.OLVColumn sizeColumn;
        private BrightIdeasSoftware.OLVColumn addressColumn;
        private BrightIdeasSoftware.OLVColumn dataColumn;
        private BrightIdeasSoftware.OLVColumn checksumColumn;
        private BrightIdeasSoftware.OLVColumn asciiColumn;
        private System.Windows.Forms.ContextMenuStrip editContextMenu;
        private System.Windows.Forms.ToolStripMenuItem insertRowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem insertRowBelowToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblCurrentAddress;


    }
}
