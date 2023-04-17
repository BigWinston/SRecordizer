using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;

using WeifenLuo.WinFormsUI.Docking;
//using NLog;
using HabelaLabs.Utility;

namespace SRecordizer
{
    public partial class SRecordizer : Form
    {
        #region _CONSTANTS_
        #endregion
        #region _DATA_TYPES_
        #endregion
        #region _PUBLIC_PROPERTIES_
        #endregion
        #region _PRIVATE_MEMBERS_
        private static Settings _Settings = new Settings();
        //private static LogView _Logger;
        //private static Logger NLogger;
        List<SRecordView> _SRecPanes;
        private static int _OpenDocCounter = 0;
        #endregion
        #region _CONSTRUCTORS_
        /*********************************************************************/
        /// <summary>
        /// Constructor
        /// </summary>
        public SRecordizer()
        {
            InitializeComponent();

            _SRecPanes = new List<SRecordView>();  /* intialize the document management structure */
            toolStripContainer1.BringToFront();  /* ensure the toolbars are always on top */
            s19Toolstrip.AutoSize = true;
            //OpenLogWindow(true);  /* create the log window */
        }
        #endregion
        #region _PUBLIC_METHODS_
        /*********************************************************************/
        /// <summary>
        /// Writes a message to the log window
        /// </summary>
        /// <param name="t">Type of message to write (Error, Warning, Info)</param>
        /// <param name="m">The message to write</param>
        /*public static void LogIt(LogView.LogType t, string m)
        {
            try
            {
                switch (t)
                {
                    case LogView.LogType.Error:
                        NLogger.Error("[ERROR]: " + m);
                        break;
                    case LogView.LogType.Warning:
                        NLogger.Warn("[WARNING]: " + m);
                        break;
                    default:
                        NLogger.Info("[INFO]: " + m);
                        break;
                }
            }
            catch { ; }
        }*/

        /*********************************************************************/
        /// <summary>
        /// Opens the log window as a dockable form inside the main window.
        /// Log window is static and will always be writing, even if not visible,
        /// hence the special handling below.
        /// </summary>
        //void OpenLogWindow(bool autoHide)
        //{
        //    /* Create the log window if neccessary */
        //    if (_Logger == null)
        //        _Logger = new LogView();

        //    /* Show it (default position is docked to bottom of dock panel */
        //    if ((_Logger.DockState == DockState.Hidden) ||
        //        (_Logger.DockState == DockState.Unknown))
        //    {
        //        _Logger.Show(dockPanel);
        //        if (autoHide)
        //            _Logger.DockState = DockState.DockBottomAutoHide;
        //        else
        //            _Logger.DockState = DockState.DockBottom;
        //    }
        //}
        #endregion
        #region _PRIVATE_METHODS_
        /*********************************************************************/
        /// <summary>
        /// Creates a new S19 files in memory using a standard base template
        /// </summary>
        void NewSrecordFile()
        {
            /* todo open doc using S19 template */
            _OpenDocCounter++;
        }

        /*********************************************************************/
        /// <summary>
        /// Opens a selected S19 file and starts the parsing for it
        /// </summary>
        /// <param name="fileNames">File names array.</param>
        void OpenSrecordFile(string[] fileNames = null)
        {
            if (fileNames == null)
            {
                OpenFileDialog openFiles = new OpenFileDialog();
                openFiles.Filter = "S-Record Files|*.s19;*.mhx|All Files (*.*)|*.*";
                openFiles.FilterIndex = 1;
                openFiles.Multiselect = true;
                openFiles.RestoreDirectory = true;
                openFiles.ShowDialog();
                fileNames = openFiles.FileNames;
            }

            if (fileNames.Length > 0)
            {
                foreach (string filename in fileNames)
                {
                    SRecordView pane = new SRecordView(filename, false);
                    pane.Show(dockPanel);
                    _SRecPanes.Add(pane);
                    _OpenDocCounter++;
                }
            }
        }

        /*********************************************************************/
        /// <summary>
        /// Closes (and saves where neccessary) an active S19 file
        /// </summary>
        /// <param name="file">Name and path to save the file to.</param>
        void CloseSrecordFile()
        {
            dockPanel.ActivePane.CloseActiveContent();
            _OpenDocCounter--;
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        private void SaveFile()
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
                if (!activeDoc.SaveFile())
                    MessageBox.Show("Error saving file!", "Save Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        private void SaveFileAs()
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "S-Record Files|*.s19;*.mhx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.RestoreDirectory = true;
                saveFile.ShowDialog();

                if (saveFile.FileNames.Length > 0)
                {
                    if (activeDoc.SaveFileAs(saveFile.FileNames[0]))
                        activeDoc.Text = activeDoc.ActiveFile;
                    else
                        MessageBox.Show("Error saving file!", "Save Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /*********************************************************************/
        /// <summary>
        /// Quits the application, performing the pre-requisite checks
        /// </summary>
        void ExitProgram()
        {

        }
        #endregion
        #region _GUI_HANDLERS_
        #region _FORM_CALLBACKS_
        /*********************************************************************/
        /// <summary>
        /// Gui call back.  Called when the Form is loaded */
        /// </summary>
        /// <param name="sender">Object of sender</param>
        /// <param name="e">Arguments for the calling event</param>
        private void SRecordizer_Load(object sender, EventArgs e)
        {
            //NLogger = LogManager.GetCurrentClassLogger();
            //LogIt(LogView.LogType.Info, "Welcome to SRecordizer!!");

            mainToolstrip.Location = new Point(0, 0);
            s19Toolstrip.Location = new Point(mainToolstrip.Width, 0);
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SRecordizer_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            OpenSrecordFile(fileNames);
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SRecordizer_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ?
                       DragDropEffects.All : DragDropEffects.None;
        }


        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lineNumberBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
            {
                if (e.KeyChar == '\r')
                {
                    try
                    {
                        int num = System.Convert.ToInt32(lineNumberBox.Text);
                        activeDoc.GoToLine(num);
                        lineNumberBox.Visible = false;
                    }
                    catch
                    {
                        ExceptionTrap.Trap("Error with line number entered!");
                    }
                }
            }
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addressBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
            {
                if (e.KeyChar == '\r')
                {
                    int addr = System.Convert.ToInt32(addressBox.Text, 16);
                    activeDoc.GoToAddress(addr);
                    addressBox.Visible = false;
                }
            }
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void asciiSearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
            {
                if (e.KeyChar == '\r')
                {
                    activeDoc.SearchAscii(asciiSearchBox.Text);
                }
            }
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lineNumberBox_Leave(object sender, EventArgs e)
        {
            lineNumberBox.Visible = false;
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addressBox_Leave(object sender, EventArgs e)
        {
            addressBox.Visible = false;
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void asciiSearchBox_Leave(object sender, EventArgs e)
        {
            asciiSearchBox.Visible = false;
        }

        #endregion
        #region _TOOLSTRIP_BUTTONS_
        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewTsButton_Click(object sender, EventArgs e)
        {
            NewSrecordFile();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewSrecordFile();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openTsButton_Click(object sender, EventArgs e)
        {
            OpenSrecordFile();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSrecordFile();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitProgram();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SRecordizer_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitProgram();
        }


        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkTsButton_Click(object sender, EventArgs e)
        {
            /* get the active S19 open record and perform the check */
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
            {
                InstantTimer t = new InstantTimer();
                activeDoc.CheckFile();
                //SRecordizer.LogIt(LogView.LogType.Info, "Checked " + activeDoc.Text + " Ok!   (Time = " + t.Stop() + " ms)");
            }
            else
                SystemSounds.Beep.Play();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnByteSpacing_Click(object sender, EventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
            {
                activeDoc.ToggleDataByteSpacing();
            }
            else
                SystemSounds.Beep.Play();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jumpToTopTsButton_Click(object sender, EventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
                activeDoc.GoToLine(0);
            else
                SystemSounds.Beep.Play();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jumpToAddressTsButton_Click(object sender, EventArgs e)
        {
            lineNumberBox.Text = "";
            lineNumberBox.Visible = true;
            lineNumberBox.Focus();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findTsButton_Click(object sender, EventArgs e)
        {
            // Do not clear .Text here, since the user might want to search again.
            asciiSearchBox.Visible = true;
            asciiSearchBox.Focus();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void overlayTsButton_Click(object sender, EventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
                SystemSounds.Asterisk.Play();
            else
                SystemSounds.Beep.Play();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mergeTsButton_Click(object sender, EventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
                SystemSounds.Asterisk.Play();
            else
                SystemSounds.Beep.Play();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitTsButton_Click(object sender, EventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
                SystemSounds.Asterisk.Play();
            else
                SystemSounds.Beep.Play();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filterTsButton_Click(object sender, EventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
                SystemSounds.Asterisk.Play();
            else
                SystemSounds.Beep.Play();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearRowColorsTsButton_Click(object sender, EventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
                activeDoc.ClearHighlighting();
            else
                SystemSounds.Beep.Play();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void asciiTsButton_Click(object sender, EventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
            {
                activeDoc.ToggleAscii();
                if (findTsButton.Visible == true)
                {
                    findTsButton.Visible = false;
                    //asciiSearchBox.Visible = false;
                }
                else
                {
                    findTsButton.Visible = true;
                    //asciiSearchBox.Visible = true;
                }
            }
            else
                SystemSounds.Beep.Play();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logTsButton_Click(object sender, EventArgs e)
        {
            //OpenLogWindow(false);
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionsTsButton_Click(object sender, EventArgs e)
        {

        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveTsButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsTsButton_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToAddressTsButton_Click(object sender, EventArgs e)
        {
            addressBox.Text = "";
            addressBox.Visible = true;
            addressBox.Focus();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void insertAboveTsButton_Click(object sender, EventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
                activeDoc.InsertRowAbove();
            else
                SystemSounds.Beep.Play();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void insertBelowTsButton_Click(object sender, EventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
                activeDoc.InsertRowBelow();
            else
                SystemSounds.Beep.Play();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteRowTsButton_Click(object sender, EventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
                activeDoc.DeleteRow();
            else
                SystemSounds.Beep.Play();
        }
        #endregion
        #region _FILE_MENU_BUTTONS_
        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog(this);
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkS19ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* get the active S19 open record and perform the check */
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
            {
                InstantTimer t = new InstantTimer();
                activeDoc.CheckFile();
                //SRecordizer.LogIt(LogView.LogType.Info, "Checked " + activeDoc.Text + " Ok!   (Time = " + t.Stop() + " ms)");
            }
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SRecordView activeDoc = (SRecordView)dockPanel.ActiveDocument;
            if (activeDoc != null)
            {
                activeDoc.GoToLine(0);
            }
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lineNumberBox.Text = "";
            lineNumberBox.Visible = true;
            lineNumberBox.Focus();
        }
        #endregion

        private void insertRowAboveToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
        #endregion     
    }
}
