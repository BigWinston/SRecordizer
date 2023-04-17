using System;
using System.Collections.Generic;
using HabelaLabs.Utility;
using SRecordizer;

public class Util
{
    public static string RemoveSpaces(string inStr)
    {   
        return inStr.Replace(" ", "");
    }

    public static bool CheckStringIsHexOnly(string str)
    {
        // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
        return System.Text.RegularExpressions.Regex.IsMatch(str, @"\A\b[0-9a-fA-F]+\b\Z");
    }

    public static bool CheckStringIsCorrectByteLength(string str)
    {
        if ((str.Length % 2) == 0)
            return true;
        else
            return false;
    }

    public static bool CheckValidSrecInstruction(string str)
    {
        string x = str.ToUpper();
        if ((x == "S1") || (x == "S2") || (x == "S3") || (x == "S5") || 
            (x == "S7") || (x == "S8") || (x == "S9") || (x == "S0"))
            return true;
        else
            return false;
    }

    public static IEnumerable<string> Chunk(string str, int chunkSize)
    {
        if (str.Length < chunkSize)
        {
            string[] s = new string[] { str };
            return s;
        }
        else
        {
            try
            {
                List<string> s = new List<string>();
                for (int i = 0; i < str.Length; i += chunkSize)
                    s.Add(str.Substring(i, chunkSize));
                return s;
            }
            catch
            {
                ExceptionTrap.Trap("Error reading data! Check for the correct number of characters and for Hex only inputs.");
                return null;
            }
        }
    }

    public static bool OnlyHexInString(string value)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(value, @"\A\b[0-9a-fA-F]+\b\Z");
    }

    public static string GetAsciiFromBytes(string rawData)
    {
        if ((rawData != "") && (rawData != null))
        {
            List<String> chunks = (List<String>)Util.Chunk(rawData, 2);
            byte[] bytes = new byte[chunks.Count];
            for (int x = 0; x < bytes.Length; x++)
            {
                byte value = System.Convert.ToByte(chunks[x], 16);
                if (value < 0x20 || value > 0x7e)
                {
                    // Don't try to print non-printable characters and instead change them to '.'
                    value = (byte) '.';
                }

                bytes[x] = value;
            }
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
        else
            return "";
    }
}


