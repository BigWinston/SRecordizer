using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HabelaLabs.Utility;


namespace SRecordizer.Objects
{
    public class S19
    {
        #region _CONSTANTS_
        public const int LEN_32_BIT_ADDR = 32;
        public const int LEN_24_BIT_ADDR = 24;
        public const int LEN_16_BIT_ADDR = 16;
        #endregion
        #region _DATA_TYPES_
        #endregion
        #region _PUBLIC_PROPERTIES_
        public List<S19Line> SRecordLines { get; set; }
        public int MaxAddressLength { get { return _MaxAddressLength; } }
        public string FileName { get { return _FileName; } }
        #endregion
        #region _PRIVATE_MEMBERS_
        string _FileName;
        int _MaxAddressLength = 16;
        #endregion
        #region _CONSTRUCTORS_
        public S19(string file, bool newFile)
        {
            if (!newFile)
            {
                FileInfo inf = new FileInfo(file);
                if (inf.Exists)
                {
                    _FileName = inf.FullName;
                    if (!ParseFile())
                        throw new Exception("Invalid S19 File!");
                }
                else
                {
                    throw new Exception("File Not Found!");
                }
            }

        }
        #endregion
        #region _PUBLIC_METHODS_
        public void InsertRow(int rowNum)
        {
            S19Line line = new S19Line();
            SRecordLines.Insert(rowNum, line);
            UpdateLineNumbering();
        }

        public void DeleteRow(int rowNum)
        {
            SRecordLines.RemoveAt(rowNum);
            UpdateLineNumbering();
        }

        public bool ParseFile()
        {
            try
            {
                SRecordLines = new List<S19Line>();
                bool flag24BitAddrUsed = false, flag32BitAddrUsed = false; 
                string line;

                StreamReader reader = new StreamReader(_FileName);
                while ((line = reader.ReadLine()) != null)
                {
                    /* ignore blank or space line */
                    if (line.Trim().Length == 0) continue;

                    /* create the new line */
                    S19Line newLine = new S19Line(line);

                    /* if there was a load error, abort */
                    if (newLine.ErrorInRow == S19Line.S19LineError.LoadError)
                        return false;

                    /* count the occurance of each instruction for analysis */
                    switch (newLine.Instruction)
                    {
                        case S19Line.S19Instruction.S2:
                        case S19Line.S19Instruction.S8:
                            flag24BitAddrUsed = true;
                            break;

                        case S19Line.S19Instruction.S3:
                        case S19Line.S19Instruction.S7:
                            flag32BitAddrUsed = true;
                            break;
                    }

                    /* add the new line to the S-Record object */
                    SRecordLines.Add(newLine);
                }
                reader.Close();
                UpdateLineNumbering();

                /* Set the address length to use */
                if (flag32BitAddrUsed)
                    _MaxAddressLength = LEN_32_BIT_ADDR;
                else if (flag24BitAddrUsed)
                    _MaxAddressLength = LEN_24_BIT_ADDR;
                else
                    _MaxAddressLength = LEN_16_BIT_ADDR;

                return true;
            }
            catch
            {
                ExceptionTrap.Trap("Error Parsing S19 File!!");
                return false;
            }
        }

        public bool Output()
        {
            return Output(_FileName);       
        }

        public bool Output(string outFile)
        {
            try
            {
                InstantTimer t = new InstantTimer();
                System.IO.StreamWriter file;
                using (file = new System.IO.StreamWriter(outFile, false))
                {
                    foreach (S19Line line in SRecordLines)
                    {
                        string outLine = line.ToString();
                        file.WriteLine(outLine);
                    }
                }
                file.Close();

                FileInfo fi = new FileInfo(outFile);
                _FileName = fi.Name;

                return true;
            }
            catch
            {
                ExceptionTrap.Trap("Error writing S19 file.  Your changes have not been saved!");
                return false;
            }
        }

        public void CheckForErrors()
        {
            foreach (S19Line line in SRecordLines)
            {
                bool isLineError = line.CheckLineForErrors();
            }
        }

        public void ResetErrorStates()
        {
            foreach (S19Line line in SRecordLines)
            {
                line.ErrorInRow = S19Line.S19LineError.NotTested;
            }
        }

        public S19Line SearchAscii(string asciiToSearch, int startLine)
        {
            // Make sure the startLine is valid.
            if (startLine < 0)
            {
                startLine = 0;
            }

            // Build single string to contain the Ascii for all S19Line items.
            StringBuilder asciiStringBuilder = new StringBuilder();
            foreach (var sRecord in SRecordLines)
            {
                if (sRecord.LineNumber >= startLine)
                {
                    asciiStringBuilder.Append(sRecord.Ascii);
                }
            }
            string asciiString = asciiStringBuilder.ToString();

            // Find the text within the big string
            int stringPosition = asciiString.IndexOf(asciiToSearch, 0);
            if (stringPosition >= 0)
            {
                // Now find the sRecord that owns that position.
                int currentStartPosition = 0;
                foreach (var sRecord in SRecordLines)
                {
                    // Skip all records prior to the start line.
                    if (sRecord.LineNumber >= startLine)
                    {
                        int currentEndPosition = currentStartPosition + sRecord.Ascii.Length;
                        if (currentStartPosition <= stringPosition && stringPosition < currentEndPosition)
                        {
                            // Return the found record.
                            return sRecord;
                        }
                        else
                        {
                            // Continue with next record.
                            currentStartPosition = currentEndPosition;
                        }
                    }
                }
            }

            // Not found
            return null;
        }
        #endregion
        #region _PRIVATE_METHODS_
        private void UpdateLineNumbering()
        {
            int cnt = 0;
            foreach (S19Line l in SRecordLines)
                l.LineNumber = cnt++;
        }
        #endregion
    }





    
}
