using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using WeifenLuo.WinFormsUI.Docking;
using BrightIdeasSoftware;

using SRecordizer.Objects;


namespace SRecordizer
{
    public partial class SRecordView : DockContent
    {
        #region _CONSTANTS_
        const int INST_COL_IDX = 0;
        const int SIZE_COL_IDX = 1;
        const int ADDR_COL_IDX = 2;
        const int DATA_COL_IDX = 3;
        const int CSUM_COL_IDX = 4;
        #endregion
        #region _DATA_TYPES_
        #endregion
        #region _PUBLIC_PROPERTIES_
        public string ActiveFile { get { return _S19Record.FileName; } }
        #endregion
        #region _PRIVATE_MEMBERS_
        S19 _S19Record;
        bool _DataByteSpacingEnabled = false;
        #endregion
        #region _CONSTRUCTORS_
        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="newFile"></param>
        public SRecordView(string filename, bool newFile)
        {
            InitializeComponent();

            try
            {
                FileInfo info = new FileInfo(filename);
                _S19Record = new S19(filename, newFile);
                this.Text = info.Name;
                //SRecordizer.LogIt(LogView.LogType.Info, "Loaded \'" + filename + "\'...");

                /* set highlighted row style */
                RowBorderDecoration rbd = new RowBorderDecoration();
                rbd.BorderPen = new Pen(Color.DarkOliveGreen, 1);
                rbd.BoundsPadding = new Size(0, -1);

                s19ListView.SelectedRowDecoration = rbd;
            }
            catch
            {
                ExceptionTrap.Trap("Error Parsing S19 File!!");
            }
        }
        #endregion
        #region _PUBLIC_METHODS_
        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        public void UpdateView()
        {
            try
            {
                if (_S19Record != null)
                    s19ListView.SetObjects(_S19Record.SRecordLines);
                else
                    throw new Exception("Invalid S19 File");

                /* set the address length to display */
                int addrDispLen = (int)((_S19Record.MaxAddressLength / 8) * 2);
                addressColumn.AspectToStringFormat = "{0:X" + addrDispLen.ToString() + "}";
            }
            catch
            {
                ExceptionTrap.Trap("Error Displaying S19 File!!");
            }
        }

        /*********************************************************************/
        /// <summary>
        /// Intitiates the checking of an S19 record for errors.
        /// </summary>
        public void CheckFile()
        {
            
            _S19Record.CheckForErrors();
            s19ListView.Refresh();
        }

        /*********************************************************************/
        /// <summary>
        /// Clears row error status highlighting
        /// </summary>
        public void ClearHighlighting()
        {
            _S19Record.ResetErrorStates();
            s19ListView.Refresh();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        public void ToggleAscii()
        {
            if (asciiColumn.IsVisible)
                asciiColumn.IsVisible = false;
            else
                asciiColumn.IsVisible = true;
            s19ListView.RebuildColumns();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SaveFile()
        {
            if (_S19Record.Output())
                return true;
            else
                return false;
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SaveFileAs(string file)
        {
            if (_S19Record.Output(file))
                return true;
            else
                return false;
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        public void ToggleDataByteSpacing()
        {
            _DataByteSpacingEnabled = !_DataByteSpacingEnabled;
            foreach (S19Line l in _S19Record.SRecordLines)
                l.ByteSpacingEnabled = _DataByteSpacingEnabled;
            s19ListView.CancelCellEdit();
            s19ListView.DeselectAll();
            s19ListView.Refresh();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineNumber"></param>
        public void GoToLine(int lineNumber)
        {
            foreach (S19Line l in _S19Record.SRecordLines)
            {
                if (l.LineNumber == lineNumber)
                {                    
                    s19ListView.EnsureVisible(lineNumber);
                    s19ListView.SelectObject(l);
                    return;
                }
            }
            MessageBox.Show("Line number not found!", "Information.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        public void GoToAddress(int address)
        {
            foreach (S19Line l in _S19Record.SRecordLines)
            {
                if ((l.Instruction == S19Line.S19Instruction.S1) ||
                    (l.Instruction == S19Line.S19Instruction.S2) ||
                    (l.Instruction == S19Line.S19Instruction.S3))
                {
                    /* calculate the start and end addresses */
                    int startAddr = System.Convert.ToInt32(l.Address, 16);
                    int endAddr = startAddr + l.Size;
                    switch (l.Instruction)
                    {
                        /* subtract from the size the bytes that aren't part of data */
                        case S19Line.S19Instruction.S0:
                        case S19Line.S19Instruction.S1:
                        case S19Line.S19Instruction.S5:
                        case S19Line.S19Instruction.S9:
                            endAddr -= 4;
                            break;

                        case S19Line.S19Instruction.S2:
                        case S19Line.S19Instruction.S8:
                            endAddr -= 5;
                            break;

                        case S19Line.S19Instruction.S3:
                        case S19Line.S19Instruction.S7:
                            endAddr -= 6;
                            break;
                    }

                    if ((address >= startAddr) && (address <= endAddr))
                    {
                        /* show the object containing the search results */
                        s19ListView.EnsureVisible(l.LineNumber);                        
                        s19ListView.SelectObject(l);

                        /* calculate where to move the caret to within the row*/
                        int moveLeft;
                        if (_DataByteSpacingEnabled == false)
                            moveLeft = (endAddr - address + 1) * 2;                   
                        else
                            moveLeft = ((endAddr - address + 1) * 3) - 1;

                        /* move the caret to the address position */
                        string action = "";
                        for (int i = 0; i < moveLeft; i++)
                            action += "{LEFT}";

                        s19ListView.StartCellEdit(s19ListView.SelectedItem, DATA_COL_IDX + 1);
                        SendKeys.Send(action);


                        /* test - highlight the searched for byte */
                        string g = s19ListView.SelectedItem.Text;

                        return;
                    }
                }
            }
            MessageBox.Show("Address not found!", "Information.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        public void SearchAscii(string asciiText)
        {
            var line = _S19Record.SearchAscii(asciiText, s19ListView.SelectedIndex + 1);
            if (line != null)
            {
                s19ListView.EnsureVisible(line.LineNumber);
                s19ListView.SelectObject(line);
            }
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        public void InsertRowAbove()
        {
            int row = s19ListView.SelectedIndex;
            _S19Record.InsertRow(row);
            s19ListView.SetObjects(_S19Record.SRecordLines);
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        public void InsertRowBelow()
        {
            int row = s19ListView.SelectedIndex;
            _S19Record.InsertRow(row + 1);
            s19ListView.SetObjects(_S19Record.SRecordLines);
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        public void DeleteRow()
        {
            int row = s19ListView.SelectedIndex;
            _S19Record.DeleteRow(row);
            s19ListView.SetObjects(_S19Record.SRecordLines);
        }
        #endregion
        #region _PRIVATE_METHODS_
        #endregion
        #region _GUI_HANDLERS_

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "Editor Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);     
        }


        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SRecordView_Load(object sender, EventArgs e)
        {
            UpdateView();
        }

        /*********************************************************************/
        /// <summary>
        /// Called when a cells editing context has completed. If it wasn't cancelled
        /// the update will be performed.
        /// </summary>
        /// <param name="sender">Called of the handler</param>
        /// <param name="e">Cell editing event information</param>
        private void s19ListView_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            /* if the editing was cancelled, return with out any updating (the cell will
               only hold onto the new value if is gets assigned to the row object) */
            if (e.Cancel)
                return;

            /* get the changed line object and an identifier to which column was changed */
            S19Line s19line = (S19Line)e.RowObject;
            OLVColumn col = e.Column;

            /* process the column and the parent row that was changed */                
            if (col == instructionColumn)
            {
                try
                {
                    string newValue = Util.RemoveSpaces(e.NewValue.ToString());
                    if(Util.CheckValidSrecInstruction(newValue))
                    {
                        s19line.UpdateLine(S19Line.S19ElementType.Instruction, e.NewValue);
                    }
                    else
                    {
                        ShowWarning("Invalid SRecord Instruction!");
                    }
                }
                catch { ExceptionTrap.Trap("Input Error! (Instruction)"); }
            }
            else if (col == sizeColumn)
            {
                try
                {
                    string newValue = Util.RemoveSpaces(e.NewValue.ToString());
                    if ((Util.CheckStringIsHexOnly(newValue)) &&
                        (Util.CheckStringIsCorrectByteLength(newValue)))
                    {
                        s19line.UpdateLine(S19Line.S19ElementType.Size, e.NewValue);
                    }
                    else
                    {
                        ShowWarning("Invalid HEX byte(s) entered in 'Size'!\n\n - Check all bytes are complete (e.g. '0xAA')\n - Check all characters are Hex (0-F)");
                    }
                }
                catch { ExceptionTrap.Trap("Input Error! (Size)"); }
            }
            else if (col == addressColumn)
            {
                try
                {
                    string newValue = Util.RemoveSpaces(e.NewValue.ToString());
                    if ((Util.CheckStringIsHexOnly(newValue)) &&
                        (Util.CheckStringIsCorrectByteLength(newValue)))
                    {
                        s19line.UpdateLine(S19Line.S19ElementType.Address, e.NewValue);
                    }
                    else
                    {
                        ShowWarning("Invalid HEX byte(s) entered in 'Address'!\n\n - Check all bytes are complete (e.g. '0xAA')\n - Check all characters are Hex (0-F)");
                    }
                }
                catch { ExceptionTrap.Trap("Input Error! (Address)"); }

            }
            else if (col == dataColumn)
            {
                try
                {
                    string newValue = Util.RemoveSpaces(e.NewValue.ToString());
                    if ((Util.CheckStringIsHexOnly(newValue)) &&
                        (Util.CheckStringIsCorrectByteLength(newValue)))
                    {
                        s19line.UpdateLine(S19Line.S19ElementType.Data, newValue);
                    }
                    else
                    {
                        ShowWarning("Invalid HEX byte(s) entered in 'Data'!\n\n - Check all bytes are complete (e.g. '0xAA')\n - Check all characters are Hex (0-F)");
                    }
                }
                catch { ExceptionTrap.Trap("Input Error! (Data)"); }
            }
            else if (col == checksumColumn)
            {
                try
                {
                    string newValue = Util.RemoveSpaces(e.NewValue.ToString());
                    if ((Util.CheckStringIsHexOnly(newValue)) &&
                        (Util.CheckStringIsCorrectByteLength(newValue)))
                    {
                        s19line.UpdateLine(S19Line.S19ElementType.Checksum, e.NewValue); ;
                    }
                    else
                    {
                        ShowWarning("Invalid HEX byte(s) entered in 'CSum'!\n\n - Check all bytes are complete (e.g. '0xAA')\n - Check all characters are HEX");
                    }
                }
                catch { ExceptionTrap.Trap(" Input Error! (Checksum)"); }

            }
            else
            { /* should never get here - do nothing! */  }
        }


        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void s19ListView_FormatRow(object sender, FormatRowEventArgs e)
        {
            S19Line line = (S19Line)e.Model;
            switch (line.ErrorInRow)
            {
                case S19Line.S19LineError.NotTested:
                    e.Item.BackColor = System.Drawing.Color.White;
                    infoLabel.Text = "";
                    break;

                case S19Line.S19LineError.InstructionError:
                    e.Item.BackColor = System.Drawing.Color.LightCoral;
                    infoLabel.Text = ">> ERROR: Instruction Error!";
                    break;

                case S19Line.S19LineError.SizeError:
                    e.Item.BackColor = System.Drawing.Color.LightCoral;
                    infoLabel.Text = ">> ERROR: Size Error!";
                    break;

                case S19Line.S19LineError.AddressError:
                    e.Item.BackColor = System.Drawing.Color.LightCoral;
                    infoLabel.Text = ">> ERROR: General Address Error!";
                    break;

                case S19Line.S19LineError.AddressLengthError:
                    e.Item.BackColor = System.Drawing.Color.LightCoral;
                    infoLabel.Text = ">> ERROR: Address Length Error!";
                    break;
                    
                case S19Line.S19LineError.DataError:
                    e.Item.BackColor = System.Drawing.Color.LightCoral;
                    infoLabel.Text = ">> ERROR: Data Error!";
                    break;

                case S19Line.S19LineError.ChecksumError:
                    e.Item.BackColor = System.Drawing.Color.LightCoral;
                    infoLabel.Text = ">> ERROR: Checksum Error!";
                    break;

                case S19Line.S19LineError.NoError:
                default:
                    e.Item.BackColor = System.Drawing.Color.YellowGreen;
                    infoLabel.Text = ">> Line OK!";
                    break;
            }
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void insertRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertRowAbove();            
        }


        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void insertRowBelowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertRowBelow();
        }


        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteRow();            
        }


        #endregion


        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inEditMode"></param>
        private void UpdateCurrentAddress()
        {
            S19Line l;
            if (s19ListView.SelectedItem != null)
            {
                /* get the address of the current line */
                l = (S19Line)s19ListView.SelectedObject;
                lblCurrentAddress.Text = "Address = 0x" + l.Address;
            }
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void UpdateCurrentAddress(OLVColumn col, S19Line row)
        {

        }
                    

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void s19ListView_SelectionChanged(object sender, EventArgs e)
        {            
            UpdateCurrentAddress();
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void s19ListView_CellEditStarting(object sender, CellEditEventArgs e)
        {
            OLVColumn col = e.Column;
            if (col == dataColumn)
            {
                UpdateCurrentAddress(col, (S19Line)e.RowObject);
            }
        }
    }
}
