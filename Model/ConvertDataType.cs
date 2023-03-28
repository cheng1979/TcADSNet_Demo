using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcADSNet_Demo.Model
{
    public class ConvertDataType
    {


        #region Method
        public static Type ConvertTwinCATDataTypeToCSharpType(string sType)
        {
            bool typeDetected = false;
            Type retType = null;
            /// Array Check
            if (sType.IndexOf("array", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                /// Need implementation --> get size and type of array
                retType = typeof(int[]);
                typeDetected = true;
            }
            /// String Check
            if (sType.IndexOf("string", StringComparison.OrdinalIgnoreCase) >= 0 && !typeDetected)
            {
                /// Is String Type
                retType = typeof(String);
                typeDetected = true;
            }

            /// Others
            if (!typeDetected)
            {
                switch (sType)
                {
                    case "BOOL":
                        retType = typeof(bool);
                        break;
                    case "BYTE":
                        retType = typeof(byte);
                        break;
                    case "WORD":
                        retType = typeof(ushort);
                        break;
                    case "DWORD":
                        retType = typeof(uint);
                        break;
                    case "SINT":
                        retType = typeof(sbyte);
                        break;
                    case "INT":
                        retType = typeof(short);
                        break;
                    case "DINT":
                        retType = typeof(int);
                        break;
                    case "LINT":
                        retType = typeof(long);
                        break;
                    case "UINT":
                        retType = typeof(ushort);
                        break;
                    case "UDINT":
                        retType = typeof(uint);
                        break;
                    case "ULINT":
                        retType = typeof(ulong);
                        break;
                    case "REAL":
                        retType = typeof(float);
                        break;
                    case "LREAL":
                        retType = typeof(double);
                        break;
                    case "TIME":
                        retType = typeof(TimeSpan);
                        break;
                    default:
                        break;

                }
            }

            return retType;
        }


        public static Type ConvertTypeNameToTwinCATDataType(string sType)
        {
            bool typeDetected = false;
            Type retType = null;
            /// String Check
            if (sType.IndexOf("string", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                /// Is String Type
                retType = typeof(String);
                typeDetected = true;
            }

            /// Others
            if (!typeDetected)
            {
                switch (sType)
                {
                    case "Boolean":
                        retType = typeof(bool);
                        break;
                    case "Byte":
                        retType = typeof(byte);
                        break;
                    case "UInt16":
                        retType = typeof(ushort);
                        break;
                    case "UInt32":
                        retType = typeof(uint);
                        break;
                    case "SByte":
                        retType = typeof(sbyte);
                        break;
                    case "Int16":
                        retType = typeof(short);
                        break;
                    case "Int32":
                        retType = typeof(int);
                        break;
                    case "Int64":
                        retType = typeof(long);
                        break;
                    case "UInt64":
                        retType = typeof(ulong);
                        break;
                    case "Single":
                        retType = typeof(float);
                        break;
                    case "Double":
                        retType = typeof(double);
                        break;
                    case "TimeSpan":
                        retType = typeof(TimeSpan);
                        break;
                    default:
                        break;

                }
            }

            return retType;
        }

        public static bool ConvertTypeNameAndValueToTwinCATDataType(string sType, string value, ref Type type,ref Object cvVal)
        {
            bool convertedOk = false;
            bool typeDetected = false;
            type = null;
            /// String Check
            if (sType.IndexOf("string", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                /// Is String Type
                type = typeof(String);
                cvVal = value;
                typeDetected = true;
                convertedOk = true;
            }

            /// Others
            if (!typeDetected)
            {
                try
                {
                    switch (sType)
                    {
                        case "Boolean":
                            type = typeof(bool);
                            if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                                cvVal = true;
                            }
                            else
                            {
                                cvVal = false;
                            }
                            convertedOk = true;
                            break;
                        case "Byte":
                            type = typeof(byte);
                            cvVal = Convert.ToByte(value);
                            convertedOk = true;
                            break;
                        case "UInt16":
                            type = typeof(ushort);
                            cvVal = Convert.ToUInt16(value);
                            convertedOk = true;
                            break;
                        case "UInt32":
                            type = typeof(uint);
                            cvVal = Convert.ToUInt32(value);
                            break;
                        case "SByte":
                            type = typeof(sbyte);
                            cvVal = Convert.ToSByte(value);
                            break;
                        case "Int16":
                            type = typeof(short);
                            cvVal = Convert.ToUInt16(value);
                            convertedOk = true;
                            break;
                        case "Int32":
                            type = typeof(int);
                            cvVal = Convert.ToInt32(value);
                            convertedOk = true;
                            break;
                        case "Int64":
                            type = typeof(long);
                            cvVal = Convert.ToInt64(value);
                            convertedOk = true;
                            break;
                        case "UInt64":
                            type = typeof(ulong);
                            cvVal = Convert.ToUInt64(value);
                            convertedOk = true;
                            break;
                        case "Single":
                            type = typeof(float);
                            cvVal = Convert.ToSingle(value);
                            convertedOk = true;
                            break;
                        case "Double":
                            type = typeof(double);
                            cvVal = Convert.ToDouble(value);
                            convertedOk = true;
                            break;
                        case "TimeSpan":
                            type = typeof(TimeSpan);
                            Int64 numTicks = Convert.ToInt64(value) * 10000; /// 1ms = 10000 ticks
                            TimeSpan ts = new TimeSpan(numTicks);
                            cvVal = ts;
                            convertedOk = true;
                            break;
                        default:
                            convertedOk = false;
                            break;

                    }
                }
                catch(FormatException fe)
                {
                    convertedOk = false;
                    Console.WriteLine(fe.Message);
                }
                catch (OverflowException oe)
                {
                    convertedOk = false;
                    Console.WriteLine(oe.Message);
                }
            }

            return convertedOk;
        }

        #endregion
    }
}
