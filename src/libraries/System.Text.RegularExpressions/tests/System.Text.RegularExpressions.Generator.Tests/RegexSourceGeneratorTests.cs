using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using SourceGenerators.Tests;
using Xunit;

namespace System.Text.RegularExpressions.Generator.Tests
{
    public class RegexSourceGeneratorTests
    {
        private static async Task<IReadOnlyList<Diagnostic>> RunGenerator(
            string code,
            bool wrap = true,
            bool inNamespace = true,
            bool includeBaseReferences = true,
            bool includeLoggingReferences = true,
            CancellationToken cancellationToken = default)
        {
            var text = code;
            if (wrap)
            {
                var nspaceStart = "namespace Test {";
                var nspaceEnd = "}";
                if (!inNamespace)
                {
                    nspaceStart = "";
                    nspaceEnd = "";
                }

                text = $@"
                    {nspaceStart}
                    using System.Text.RegularExpressions;
                    {code}
                    {nspaceEnd}
                ";
            }

            Assembly[]? refs = null;
            if (includeLoggingReferences)
            {
                refs = new[] { typeof(Regex).Assembly, typeof(RegexSourceGeneratorAttribute).Assembly };
            }

            var (d, r) = await RoslynTestUtils.RunGenerator(
                new RegexGenerator(),
                refs,
                new[] { text },
                includeBaseReferences: includeBaseReferences,
                cancellationToken: cancellationToken).ConfigureAwait(false);

            return d;
        }


        [Fact]
        public async Task Test()
        {
            Debugger.Launch();
            var diagnostics = await RunGenerator(
@"
    partial class C
    {
        [RegexSourceGenerator(""abcd"", RegexOptions.IgnoreCase)]
        public Regex? CreateIpAddressRegex;
    }
");

        }
    }
}
