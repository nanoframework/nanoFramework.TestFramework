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

                if (typea == typeof(int))
                {
                    if ((int)obja != (int)objb)
                    {
                        return false;
                    }

                    continue;
                }

                if (typea == typeof(uint))
                {
                    if ((uint)obja != (uint)objb)
                    {
                        return false;
                    }

                    continue;
                }

                if (typea == typeof(byte))
                {
                    if ((byte)obja != (byte)objb)
                    {
                        return false;
                    }

                    continue;
                }

                if (typea == typeof(sbyte))
                {
                    if ((sbyte)obja != (sbyte)objb)
                    {
                        return false;
                    }

                    continue;
                }

                if (typea == typeof(short))
                {
                    if ((short)obja != (short)objb)
                    {
                        return false;
                    }

                    continue;
                }

                if (typea == typeof(ushort))
                {
                    if ((ushort)obja != (ushort)objb)
                    {
                        return false;
                    }

                    continue;
                }

                if (typea == typeof(long))
                {
                    if ((long)obja != (long)objb)
                    {
                        return false;
                    }

                    continue;
                }

                if (typea == typeof(ulong))
                {
                    if ((ulong)obja != (ulong)objb)
                    {
                        return false;
                    }

                    continue;
                }

                if (typea == typeof(char))
                {
                    if ((char)obja != (char)objb)
                    {
                        return false;
                    }

                    continue;
                }

                if (typea == typeof(double))
                {
                    if ((double)obja != (double)objb)
                    {
                        return false;
                    }

                    continue;
                }

                if (typea == typeof(bool))
                {
                    if ((bool)obja != (bool)objb)
                    {
                        return false;
                    }

                    continue;
                }

                if (typea == typeof(float))
                {
                    if ((float)obja != (float)objb)
                    {
                        return false;
                    }

                    continue;
                }

                if (obja != objb)
                {
                    return false;
                }

                continue;
            }

            return true;
        }
    }
}
