using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRecordizer.Objects
{
    public class S19Line
    {
        #region _CONSTANTS_
        const int IDX_INSTRUCTION = 0;
        const int LEN_INSTRUCTION = 2;
        const int IDX_LINELENGTH = LEN_INSTRUCTION;
        const int LEN_LENGTH = 2;

        const int IDX_ADDR = IDX_LINELENGTH + LEN_LENGTH;
        const int LEN_S0_ADDR = 4;
        const int LEN_S1_ADDR = 4;
        const int LEN_S2_ADDR = 6;
        const int LEN_S3_ADDR = 8;
        const int LEN_S5_ADDR = 4;
        const int LEN_S7_ADDR = 8;
        const int LEN_S8_ADDR = 6;
        const int LEN_S9_ADDR = 4;

        const int IDX_S0_DATA = IDX_ADDR + LEN_S0_ADDR;
        const int IDX_S1_DATA = IDX_ADDR + LEN_S1_ADDR;
        const int IDX_S2_DATA = IDX_ADDR + LEN_S2_ADDR;
        const int IDX_S3_DATA = IDX_ADDR + LEN_S3_ADDR;
        const int IDX_S5_DATA = IDX_ADDR + LEN_S5_ADDR;
        const int IDX_S7_DATA = IDX_ADDR + LEN_S7_ADDR;
        const int IDX_S8_DATA = IDX_ADDR + LEN_S8_ADDR;
        const int IDX_S9_DATA = IDX_ADDR + LEN_S9_ADDR;

        const int LEN_CSUM = 2;
        #endregion
        #region _DATA_TYPES_
        /* http://en.wikipedia.org/wiki/SREC_(file_format) */
        public enum S19Instruction
        {
            Unknown,    /* Error */
            S0,         /* Vendor specific data, commonly a string with file name and version info */
            S1,         /* Data sequence with 16-bit addressing */
            S2,         /* Data sequence with 24-bit addressing */
            S3,         /* Data sequence with 32-bit addressing */
            S5,         /* The count of S1, S2 and S3 records in the file */
            S7,         /* End of Block (32 bit addressing if applicable) */
            S8,         /* End of Block (24 bit addressing if applicable) */
            S9          /* End of Block (16 bit addressing if applicable) */
        }
        public enum S19ElementType
        {
            Instruction,
            Size,
            Address,
            Data,
            Checksum,
            ASCII
        }
        public enum S19LineError
        {
            NotTested,
            NoError,
            InstructionError,
            SizeError,
            AddressError,
            AddressLengthError,
            DataError,
            ChecksumError,
            LoadError
        }
        #endregion
        #region _PUBLIC_PROPERTIES_
        public S19LineError ErrorInRow { get { return _ErrorInRow; } set { _ErrorInRow = value; } }
        public int LineNumber { get { return _LineNumber; } set { _LineNumber = value; } }
        public S19Instruction Instruction { get { return _Instruction; } }
        public byte Size { get { return _Size; } }
        public string Address
        {
            get
            {
                return _Address.ToString(GetAddressFormat());
            }
            set
            {
                try { this._Address = UInt32.Parse(value, System.Globalization.NumberStyles.HexNumber); }
                catch { ExceptionTrap.Trap("Input Error! (Address)"); }
            }
        }
        public bool ByteSpacingEnabled { get; set; }
        public string Data
        {
            get
            {
                if (ByteSpacingEnabled == false)
                    return _Data;
                else
                {
                    /* add byte spacing */
                    string data = "";
                    int i = 1;
                    foreach (char c in _Data)
                    {
                        data += c;
                        if ((i++ % 2 == 0) &&(i < _Data.Length))
                            data += " ";
                    }
                    return data;
                }
            }
        }
        public byte Checksum { get { return _Checksum; } }
        public uint AddressLength { get { return _AddressLength; } }
        public string Ascii { get { return Util.GetAsciiFromBytes(_Data); } }
        #endregion
        #region _PRIVATE_MEMBERS_
        S19LineError _ErrorInRow;
        int _LineNumber;
        S19Instruction _Instruction;
        byte _Size;
        uint _Address;
        string _Data;
        byte _Checksum;
        uint _AddressLength;
        string _RawLine;
        #endregion
        #region _CONSTRUCTORS_
        /*********************************************************************/
        /// <summary>
        /// Constructor - Blank, for use when creating a brand new S19 line
        /// </summary>
        public S19Line()
        {
            _LineNumber = 0;
            _Instruction = S19Instruction.Unknown;
            _Size = 0;
            _Address = 0;
            _Data = "";
            _Checksum = 0;
            _AddressLength = 0;
            _RawLine = "";

            ByteSpacingEnabled = false;
        }

        /*********************************************************************/
        /// <summary>
        /// Constructor - Creates an S19 line from the passed raw data string
        /// </summary>
        /// <param name="lineNum">The lines position (line number) in the S19
        /// file being loaded</param>
        /// <param name="line">The raw string containing the S19 line</param>
        public S19Line(string line)
        {
            UpdateLine(line);
        }
        #endregion
        #region _PUBLIC METHODS_

        public override string ToString()
        {
            return CreateRawLine();
        }

        /*********************************************************************/
        /// <summary>
        /// Updates the S19 line object given a raw data string
        /// </summary>
        /// <param name="line"></param>
        public bool UpdateLine(string line)
        {
            try
            {
                _RawLine = line;
                string strInstr = line.Substring(IDX_INSTRUCTION, LEN_INSTRUCTION);
                _Size = System.Convert.ToByte(line.Substring(IDX_LINELENGTH, IDX_LINELENGTH), 16);
                _Checksum = System.Convert.ToByte(line.Substring(line.Length - LEN_CSUM, LEN_CSUM), 16);
                switch (strInstr)
                {
                    case "S0":
                        this._Instruction = S19Instruction.S0;
                        this._Address = System.Convert.ToUInt32(line.Substring(IDX_ADDR, LEN_S0_ADDR), 16);
                        this._Data = line.Substring(IDX_S0_DATA, line.Length - LEN_CSUM - IDX_S0_DATA);
                        this._AddressLength = LEN_S0_ADDR;
                        break;

                    case "S1":
                        this._Instruction = S19Instruction.S1;
                        this._Address = System.Convert.ToUInt32(line.Substring(IDX_ADDR, LEN_S1_ADDR), 16);
                        this._Data = line.Substring(IDX_S1_DATA, line.Length - LEN_CSUM - IDX_S1_DATA);
                        this._AddressLength = LEN_S1_ADDR;
                        break;

                    case "S2":
                        this._Instruction = S19Instruction.S2;
                        this._Address = System.Convert.ToUInt32(line.Substring(IDX_ADDR, LEN_S2_ADDR), 16);
                        this._Data = line.Substring(IDX_S2_DATA, line.Length - LEN_CSUM - IDX_S2_DATA);
                        this._AddressLength = LEN_S2_ADDR;
                        break;

                    case "S3":
                        this._Instruction = S19Instruction.S3;
                        this._Address = System.Convert.ToUInt32(line.Substring(IDX_ADDR, LEN_S3_ADDR), 16);
                        this._Data = line.Substring(IDX_S3_DATA, line.Length - LEN_CSUM - IDX_S3_DATA);
                        this._AddressLength = LEN_S3_ADDR;
                        break;

                    case "S5":
                        this._Instruction = S19Instruction.S5;
                        this._Address = System.Convert.ToUInt32(line.Substring(IDX_ADDR, LEN_S5_ADDR), 16);
                        this._Data = line.Substring(IDX_S5_DATA, line.Length - LEN_CSUM - IDX_S5_DATA);
                        this._AddressLength = LEN_S5_ADDR;
                        break;

                    case "S7":
                        this._Instruction = S19Instruction.S7;
                        this._Address = System.Convert.ToUInt32(line.Substring(IDX_ADDR, LEN_S7_ADDR), 16);
                        this._Data = line.Substring(IDX_S7_DATA, line.Length - LEN_CSUM - IDX_S7_DATA);
                        this._AddressLength = LEN_S7_ADDR;
                        break;

                    case "S8":
                        this._Instruction = S19Instruction.S8;
                        this._Address = System.Convert.ToUInt32(line.Substring(IDX_ADDR, LEN_S8_ADDR), 16);
                        this._Data = line.Substring(IDX_S8_DATA, line.Length - LEN_CSUM - IDX_S8_DATA);
                        this._AddressLength = LEN_S8_ADDR;
                        break;

                    case "S9":
                        this._Instruction = S19Instruction.S9;
                        this._Address = System.Convert.ToUInt32(line.Substring(IDX_ADDR, LEN_S9_ADDR), 16);
                        this._Data = line.Substring(IDX_S9_DATA, line.Length - LEN_CSUM - IDX_S9_DATA);
                        this._AddressLength = LEN_S9_ADDR;
                        break;

                    default:
                        this._Instruction = S19Instruction.Unknown;
                        break;
                }
                return true;
            }
            catch
            {
                ExceptionTrap.Trap("Error reading S19 data!");
                this.ErrorInRow = S19LineError.LoadError;
                throw;
            }
        }

        /*********************************************************************/
        /// <summary>
        /// Updates the S19 line object given a defined value from an
        /// ObjectListView column
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public void UpdateLine(S19ElementType element, object value)
        {
            switch (element)
            {
                case S19ElementType.Instruction:
                    try
                    {
                        /* update the instruction and the address length expected for that instruction */
                        _Instruction = (S19Instruction)value;
                        switch (_Instruction)
                        {
                            case S19Instruction.S0:
                            case S19Instruction.S1:
                            case S19Instruction.S5:
                            case S19Instruction.S9:
                            default:
                                _AddressLength = LEN_S0_ADDR;
                                break;

                            case S19Instruction.S2:
                            case S19Instruction.S8:
                                _AddressLength = LEN_S2_ADDR;
                                break;

                            case S19Instruction.S3:
                            case S19Instruction.S7:
                                _AddressLength = LEN_S3_ADDR;
                                break;
                        }
                    }
                    catch { ; }
                    break;

                case S19ElementType.Size:
                    if (Util.OnlyHexInString((string)value))
                        try { _Size = Byte.Parse((string)value, System.Globalization.NumberStyles.HexNumber); }
                        catch { ; }
                    break;

                case S19ElementType.Address:
                    if (Util.OnlyHexInString((string)value))
                        this.Address = (string)value; /* (write to the property which will perform the conversion) */
                    break;

                case S19ElementType.Data:
                    if ((((string)value).Length % 2) != 0)
                        _Data = (string)value;
                    else
                        _Data = (string)value;
                        if ((!Util.OnlyHexInString((string)value)) && ((string)value != ""))
                            throw new Exception("Non-hex characters found in data!");
                    break;

                case S19ElementType.Checksum:
                    if (Util.OnlyHexInString((string)value))
                        try { _Checksum = Byte.Parse((string)value, System.Globalization.NumberStyles.HexNumber); }
                        catch { ; }
                    break;

                case S19ElementType.ASCII:
                    /* todo */
                    break;

                default:
                    { ; }
                    break;
            }

            /* with the element updated, update the rest of the row, including the new checksum (dont
               update the checksum if the cell being changed is the checksum as we'll assume the user
               wants to overrwrite the calculated checksum */
            _RawLine = CreateRawLine();
            if (element != S19ElementType.Checksum)
                CalculateChecksum(true);
        }


        /*********************************************************************/
        /// <summary>
        /// Checks an S19 line for errors
        /// </summary>
        /// <returns>True if the line contains an error, else false</returns>
        public bool CheckLineForErrors()
        {
            /* check for a valid instruction type */
            if (LineHasErrorsInstruction())
            {
                _ErrorInRow = S19LineError.InstructionError;
                return true;
            }

            /* check the size is specified correctly */
            if (LineHasErrorsSizeLength())
            {
                _ErrorInRow = S19LineError.AddressError;
                return true;
            }

            /* check the size of the line matches what is specified in the size byte */
            if (LineHasErrorsSizeIncorrect())
            {
                _ErrorInRow = S19LineError.SizeError;
                return true;
            }

            /* check the line has a valid address */
            if (LineHasErrorsAddress())
            {
                _ErrorInRow = S19LineError.AddressError;
                return true;
            }

            /* check the length of address is correct for the instruction type */
            if (LineHasErrorsAddressLength())
            {
                _ErrorInRow = S19LineError.AddressLengthError;
                return true;
            }

            /* check if the data contains any errors */
            if (LineHasErrorsData())
            {
                _ErrorInRow = S19LineError.DataError;
                return true;
            }

            /* check the checksum matches correctly */
            if (LineHasErrorsChecksum())
            {
                _ErrorInRow = S19LineError.ChecksumError;
                return true;
            }

            /* if we get here, there are no errors and the line is good! */
            _ErrorInRow = S19LineError.NoError;
            return false;
        }

        #endregion
        #region _PRIVATE_METHODS_
        /*********************************************************************/
        /// <summary>
        /// Creates a raw string containing all information of the S19Line
        /// objects information in the usual format. The information is not
        /// checked for correctness or consistency, it just glues all members
        /// together into the string.
        /// </summary>
        /// <returns>The complete raw line</returns>
        private string CreateRawLine()
        {
            string raw = _Instruction.ToString();
            raw += _Size.ToString("X2");
            raw += _Address.ToString(GetAddressFormat());
            raw += _Data.ToString();
            raw += _Checksum.ToString("X2");
            return raw;
        }


        /*********************************************************************/
        /// <summary>
        /// Calculates the checksum for the S19 line (uses _RawLine)
        /// </summary>
        /// <param name="write">Write the checksum the S19 object when
        /// completed, else just performs the calculation</param>
        /// <returns>Result of the S19 calculation</returns>
        private byte CalculateChecksum(bool write)
        {
            try
            {
                /* remove the first and last bytes and checksum the rest */
                byte csum = 0;
                List<String> bytes = (List<String>)Util.Chunk(_RawLine.Substring(2, _RawLine.Length - 4), 2);
                foreach (string s in bytes)
                    csum += System.Convert.ToByte(s, 16);
                csum = (byte)~csum;  /* perform the one's complement */

                /* if required, save off the checksum value */
                if (write)
                    _Checksum = csum;

                return csum;
            }
            catch
            {
                ExceptionTrap.Trap("Error calculating checksum!");
                return 0;
            }
        }


        /*********************************************************************/
        /// <summary>
        /// Creates a string to be used to display the addresses for the 
        /// different S-Record types.  The string shall be used with the 
        /// 'ToString' method for native types.
        /// </summary>
        /// <returns>The string formatting to use</returns>
        private string GetAddressFormat()
        {
            string fmt;
            switch (_Instruction)
            {
                case S19Instruction.S0:
                case S19Instruction.S1:
                case S19Instruction.S5:
                case S19Instruction.S9:
                    fmt = "X4";
                    break;

                case S19Instruction.S2:
                case S19Instruction.S8:
                    fmt = "X6";
                    break;

                case S19Instruction.S3:
                case S19Instruction.S7:
                    fmt = "X8";
                    break;

                default:
                    fmt = "ERR";  /* shall throw exception is somehow used */
                    break;
            }
            return fmt;
        }



        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if an error is found, else false.</returns>
        private bool LineHasErrorsInstruction()
        {
            switch (_Instruction)
            {
                case S19Instruction.S0:
                case S19Instruction.S1:
                case S19Instruction.S2:
                case S19Instruction.S3:
                case S19Instruction.S5:
                case S19Instruction.S7:
                case S19Instruction.S8:
                case S19Instruction.S9:
                    return false;

                default:
                    return true;
            }
        }



        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if an error is found, else false.</returns>
        private bool LineHasErrorsSizeLength()
        {
            if ((_Size < 0) || (_Size > 0xFF))
                return true;
            else
                return false;
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if an error is found, else false.</returns>
        private bool LineHasErrorsSizeIncorrect()
        {
            if (_Data == null)
                _Data = "";

            if (_Size != (_Address.ToString(GetAddressFormat()).Length / 2) + (_Data.Length / 2) + (LEN_CSUM / 2))
                return true;
            else
                return false;
        }

        /*********************************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if an error is found, else false.</returns>
        private bool LineHasErrorsAddress()
        {
            /* calculate the maximum address for the specified address length */
            uint tmp = 1;
            for (int x = 0; x < _AddressLength; x++)
                tmp = tmp << 4; /* working with character nibbles here */
            tmp -= 1;

            /* check we do not exceed it */
            if ((_Address < 0) || (_Address > tmp))
                return true;
            else
                return false;
        }

        /*********************************************************************/
        /// <summary>
        /// Checks to make sure the address length is correct for this instruction
        /// </summary>
        /// <returns>True if an error is found, else false.</returns>
        private bool LineHasErrorsAddressLength()
        {
            switch (_Instruction)
            {
                case S19Instruction.S0:
                case S19Instruction.S1:
                case S19Instruction.S5:
                case S19Instruction.S9:
                    if (_AddressLength != LEN_S0_ADDR)
                        return true;
                    break;

                case S19Instruction.S2:
                case S19Instruction.S8:
                    if (_AddressLength != LEN_S2_ADDR)
                        return true;
                    break;

                case S19Instruction.S3:
                case S19Instruction.S7:
                    if (_AddressLength != LEN_S3_ADDR)
                        return true;
                    break;

                default:
                    return true;  /* will only get here in error */
            }
            return false;  /* tests passed */
        }

        /*********************************************************************/
        /// <summary>
        /// Checks the data string for errors.  The only check we can perform
        /// on the data is ensuring all data is in a valid hex notation.
        /// </summary>
        /// <returns>True if an error is found, else false.</returns>
        private bool LineHasErrorsData()
        {
            if ((Util.OnlyHexInString(_Data)) || (_Data == ""))
                return false;
            else
                return true;
        }

        /*********************************************************************/
        /// <summary>
        /// Checks the data string for errors.  The only check we can perform
        /// on the data is ensuring all data is in a valid hex notation.
        /// </summary>
        /// <returns>True if an error is found, else false.</returns>
        private bool LineHasErrorsChecksum()
        {
            if (CalculateChecksum(false) == _Checksum)
                return false;
            else
                return true;
        }
        #endregion
    }
}
