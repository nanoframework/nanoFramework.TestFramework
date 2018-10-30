//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Text.RegularExpressions;

namespace nanoFramework.TestPlatform.TestInterface
{
    public static class RegExHelper
    {
        public static readonly Regex Regex_TrueFalse = new Regex(@"^(?i:true)$|^(?i:false)$", RegexOptions.Singleline);

    }
}
