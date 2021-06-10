// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Text.RegularExpressions
{
    /// <summary>
    /// This attribute kicks off the Regex Source Generator to produce a specialized, compiled and performant Regex type for the given pattern
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class RegexSourceGeneratorAttribute : Attribute
    {
        // TODO: Figure out the best way to expose this attribute. Will have to go through API review eventually
        public RegexSourceGeneratorAttribute(string pattern, RegexOptions options) { }
    }
}
