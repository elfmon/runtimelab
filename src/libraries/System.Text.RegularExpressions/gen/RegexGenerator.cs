// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Text.RegularExpressions.Generator
{
    public partial class RegexGenerator : ISourceGenerator
    {
        public Regex? CreateIpAddressRegex;

        [ExcludeFromCodeCoverage]
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(SyntaxReceiver.Create);
        }

        [ExcludeFromCodeCoverage]
        public void Execute(GeneratorExecutionContext context)
        {
            Debugger.Launch();
            if (context.SyntaxReceiver is not SyntaxReceiver receiver || receiver.RegexFields.Count == 0)
            {
                return;
            }
        }

        private const string RegexSourceGeneratorAttributeName = "System.Text.RegularExpressions.RegexSourceGeneratorAttribute";

        [ExcludeFromCodeCoverage]
        private sealed class SyntaxReceiver : ISyntaxReceiver
        {
            internal static ISyntaxReceiver Create()
            {
                return new SyntaxReceiver();
            }

            public List<FieldDeclarationSyntax> RegexFields { get; } = new();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is FieldDeclarationSyntax field && field.AttributeLists.Count > 0)
                {
                    RegexFields.Add(field);
                    //foreach (AttributeListSyntax fieldAttributeList in field.AttributeLists)
                    //{
                    //    foreach (AttributeSyntax fieldAttribute in fieldAttributeList.Attributes)
                    //    {
                    //        if (fieldAttribute.Name.Equals(RegexSourceGeneratorAttributeName))
                    //    }
                    //}
                }
            }
        }
    }
}
