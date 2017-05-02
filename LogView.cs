using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace SRecordizer
{
    public partial class LogView : DockContent
    {
        #region _CONSTANTS_
        #endregion
        #region _DATA_TYPES_
        public enum LogType
        { Info, Warning, Error }
        #endregion
        #region _PUBLIC_PROPERTIES_
        #endregion
        #region _PRIVATE_MEMBERS_
        const int MAX_LOG_LENGTH = 50000;
        bool AutoClearLogLimit = true;
        #endregion
        #region _CONSTRUCTORS_
        /*********************************************************************/
        /// <summary>
        /// Constructor
        /// </summary>
        public LogView()
        {
            InitializeComponent();

        }
        #endregion
        #region _PUBLIC_METHODS_
        #endregion
        #region _PRIVATE_METHODS_
        #endregion
        #region _GUI_CALLBACKS_
        /*********************************************************************/
        /// <summary>
        /// Standard WinForms Gui Callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearLogButton_Click(object sender, EventArgs e)
        {
            logBox.ResetText();
        }

        /*********************************************************************/
        /// <summary>
        /// Standard WinForms Gui Callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveLogButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes == System.Windows.Forms.DialogResult.OK)
            {
                logBox.SaveFile(dlg.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        /*********************************************************************/
        /// <summary>
        /// Standard WinForms Gui Callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resumeLogUpdatesButton_Click(object sender, EventArgs e)
        {
            logBox.Enabled = true;
        }

        /*********************************************************************/
        /// <summary>
        /// Standard WinForms Gui Callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyLogButton_Click(object sender, EventArgs e)
        {
            if ((logBox.SelectedText != null) && (logBox.SelectedText != ""))
                Clipboard.SetText(logBox.SelectedText);
        }

        /*********************************************************************/
        /// <summary>
        /// Standard WinForms Gui Callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogView_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.DockState = WeifenLuo.WinFormsUI.Docking.DockState.Hidden;
        }

        /*********************************************************************/
        /// <summary>
        /// Standard WinForms Gui Callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logBox_TextChanged(object sender, EventArgs e)
        {
            logBox.Focus();
            logBox.SelectionStart = logBox.Text.Length;

            if (AutoClearLogLimit)
            {
                if (logBox.TextLength > MAX_LOG_LENGTH)
                {
                    logBox.ResetText();
                }
            }
            //logBox.ScrollToCaret();  
        }
        #endregion
    }
}
