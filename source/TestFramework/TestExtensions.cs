//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// Extension for array element comparison
    /// </summary>
    public static class TestExtensions
    {
        /// <summary>
        /// Compare the sequence of 2 arrays
        /// </summary>
        /// <param name="a">First array</param>
        /// <param name="b">Second array</param>
        /// <returns>True if the sequence is the same</returns>
        public static bool SequenceEqual(this Array a, Array b)
        {
            if ((a == null) || (b == null))
            {
                return false;
            }

            if (a.Length != b.Length)
            {
                return false;
            }


            for (int i = 0; i < a.Length; i++)
            {
                object obja = a.GetValue(i);
                object objb = b.GetValue(i);
                var typea = obja.GetType();
                var typeb = objb.GetType();
                if (typea != typeb)
                {
                    return false;
                }

                switch (typea.FullName)
                {
                    case "System.Int32":
                        if ((int)obja != (int)objb)
                        {
                            return false;
                        }
                        break;
                    case "System.UInt32":
                        if ((uint)obja != (uint)objb)
                        {
                            return false;
                        }
                        break;
                    case "System.Byte":
                        if ((byte)obja != (byte)objb)
                        {
                            return false;
                        }
                        break;
                    case "System.SByte":
                        if ((sbyte)obja != (sbyte)objb)
                        {
                            return false;
                        }
                        break;
                    case "System.Int16":
                        if ((short)obja != (short)objb)
                        {
                            return false;
                        }
                        break;
                    case "System.UInt16":
                        if ((ushort)obja != (ushort)objb)
                        {
                            return false;
                        }
                        break;
                    case "System.Int64":
                        if ((long)obja != (long)objb)
                        {
                            return false;
                        }
                        break;
                    case "System.UInt64":
                        if ((ulong)obja != (ulong)objb)
                        {
                            return false;
                        }
                        break;
                    case "System.Char":
                        if ((char)obja != (char)objb)
                        {
                            return false;
                        }
                        break;
                    case "System.Double":
                        if ((double)obja != (double)objb)
                        {
                            return false;
                        }
                        break;
                    case "System.Boolean":
                        if ((bool)obja != (bool)objb)
                        {
                            return false;
                        }
                        break;
                    case "System.Single":
                        if ((float)obja != (float)objb)
                        {
                            return false;
                        }
                        break;
                    default:
                        if (obja != objb)
                        {
                            return false;
                        }
                        break;
                }
            }

            return true;
        }
    }
}
