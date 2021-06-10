# Branch specific info:
This branch contains the code for Regex Source Generators. See https://github.com/dotnet/runtime/issues/44676 for motivation. Currently the work to set up the boilerplate for source generators has been done and a simple unit test has been set up as a starting point to drive the implementation. Starting with this branch should be easy: Just clone, build System.Text.RegularExpressions.sln and debug System.Text.RegularExpressions.Generator.Tests.Test.  The following work is pending:

1. Define the exact public API that the source generator exposes. In this branch, I have created a `RegexSourceGenerator` attribute that takes a pattern. The exact public API, once defined, will need to go through API review. However, this is not important right now to get started with the source generator implementation
2. A parser to parse RegexGenerator.cs:SyntaxReceiver.RegexFields and obtain the regex pattern.
3. Once the pattern is parsed, the source generator needs to produce a type that inherits from `Regex`. For ex: Given a pattern like "abcd", this looks similar to:

```csharp
// test1
using System;
using System.Text.RegularExpressions;

internal class test1 : Regex
{
	public test1()
	{
		pattern = "abcd";
		roptions = RegexOptions.Compiled;
		factory = new test1Factory1();
		TimeSpan infiniteMatchTimeout = Regex.InfiniteMatchTimeout;
		((Regex)/*Error near IL_002d: Stack underflow*/).internalMatchTimeout = infiniteMatchTimeout;
		capsize = 1;
		InitializeReferences();
	}
}
```
4. The source generator also needs to produce the corresponding factory and runner methods. It should similar to:

```csharp
// test1Factory1
using System.Text.RegularExpressions;

internal sealed class test1Factory1 : RegexRunnerFactory
{
	protected override RegexRunner CreateInstance()
	{
		return new test1Runner1();
	}
}

```
and 
```csharp
// test1Runner1
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

internal sealed class test1Runner1 : RegexRunner
{
	protected override void Go()
	{
		string text = runtext;
		int num = runtextend;
		int num2 = runtextpos;
		int start = num2;
		ReadOnlySpan<char> span = MemoryExtensions.AsSpan(text, num2, num - num2);
		if (3u < (uint)span.Length && MemoryMarshal.GetReference(span) == 'a' && System.Runtime.CompilerServices.Unsafe.Add(ref MemoryMarshal.GetReference(span), 1) == 'b' && System.Runtime.CompilerServices.Unsafe.Add(ref MemoryMarshal.GetReference(span), 2) == 'c' && System.Runtime.CompilerServices.Unsafe.Add(ref MemoryMarshal.GetReference(span), 3) == 'd')
		{
			Capture(0, start, runtextpos = num2 + 4);
		}
	}

	protected override bool FindFirstChar()
	{
		int num = runtextpos;
		int num2 = runtextend;
		if (num <= num2 - 4)
		{
			string text = runtext;
			num += 3;
			while (num < num2)
			{
				int num3 = text[num];
				int num4;
				if (num3 != 100)
				{
					num3 -= 97;
					num4 = (((uint)num3 <= 3u) ? "\u0003\u0002\u0001\0"[num3] : '\u0004');
				}
				else
				{
					num3 = num;
					num3--;
					if (text[num3] == 'c')
					{
						num3--;
						if (text[num3] == 'b')
						{
							num3--;
							if (text[num3] == 'a')
							{
								runtextpos = num3;
								return true;
							}
						}
					}
					num4 = 1;
				}
				num = num4 + num;
			}
		}
		runtextpos = num2;
		return false;
	}

	protected override void InitTrackCount()
	{
		runtrackcount = 3;
	}
}

```
5. Add the emitted code to the current compilation.
6. Wire up the new source generated type to the Regex cache so the regex engine finds it? I haven't thought this through yet.
7. When the implementation is complete and we are ready to ship, the commit e0285cc3d20d960542a3997e9bf929a3d3249d8a (Add dependency on Lokad/ILPack to save assembly to disk) needs to be reverted.

# .NET Runtime
[![Build Status](https://dnceng.visualstudio.com/public/_apis/build/status/dotnet/runtime/runtime?branchName=main)](https://dnceng.visualstudio.com/public/_build/latest?definitionId=686&branchName=main)
[![Help Wanted](https://img.shields.io/github/issues/dotnet/runtime/up-for-grabs?style=flat-square&color=%232EA043&label=help%20wanted)](https://github.com/dotnet/runtime/issues?q=is%3Aissue+is%3Aopen+label%3A%22up-for-grabs%22)
[![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/dotnet/runtime)
[![Discord](https://img.shields.io/discord/732297728826277939?style=flat-square&label=Discord&logo=discord&logoColor=white&color=7289DA)](https://aka.ms/dotnet-discord)

This repo contains the code to build the .NET runtime, libraries and shared host (`dotnet`) installers for
all supported platforms, as well as the sources to .NET runtime and libraries.

## What is .NET?

Official Starting Page: https://dotnet.microsoft.com/

* [How to use .NET](https://docs.microsoft.com/dotnet/core/get-started) (with VS, VS Code, command-line CLI)
  * [Install official releases](https://dotnet.microsoft.com/download)
  * [Install daily builds](docs/project/dogfooding.md)
  * [Documentation](https://docs.microsoft.com/dotnet/core) (Get Started, Tutorials, Porting from .NET Framework, API reference, ...)
    * [Deploying apps](https://docs.microsoft.com/dotnet/core/deploying)
  * [Supported OS versions](https://github.com/dotnet/core/blob/master/os-lifecycle-policy.md)
* [Roadmap](https://github.com/dotnet/core/blob/master/roadmap.md)
* [Releases](https://github.com/dotnet/core/tree/master/release-notes)

## How can I contribute?

We welcome contributions! Many people all over the world have helped make this project better.

* [Contributing](CONTRIBUTING.md) explains what kinds of changes we welcome
- [Workflow Instructions](docs/workflow/README.md) explains how to build and test
* [Get Up and Running on .NET Core](docs/project/dogfooding.md) explains how to get nightly builds of the runtime and its libraries to test them in your own projects.

## Reporting security issues and security bugs

Security issues and bugs should be reported privately, via email, to the Microsoft Security Response Center (MSRC) <secure@microsoft.com>. You should receive a response within 24 hours. If for some reason you do not, please follow up via email to ensure we received your original message. Further information, including the MSRC PGP key, can be found in the [Security TechCenter](https://www.microsoft.com/msrc/faqs-report-an-issue).

Also see info about related [Microsoft .NET Core and ASP.NET Core Bug Bounty Program](https://www.microsoft.com/msrc/bounty-dot-net-core).

## Filing issues

This repo should contain issues that are tied to the runtime, the class libraries and frameworks, the installation of the `dotnet` binary (sometimes known as the `muxer`) and installation of the .NET runtime and libraries.

For other issues, please use the following repos:

- For overall .NET SDK issues, file in the [dotnet/sdk](https://github.com/dotnet/sdk) repo
- For ASP.NET issues, file in the [dotnet/aspnetcore](https://github.com/dotnet/aspnetcore) repo.

## Useful Links

* [.NET Core source index](https://source.dot.net) / [.NET Framework source index](https://referencesource.microsoft.com)
* [API Reference docs](https://docs.microsoft.com/dotnet/api/?view=netcore-3.1)
* [.NET API Catalog](http://apisof.net) (incl. APIs from daily builds and API usage info)
* [API docs writing guidelines](https://github.com/dotnet/dotnet-api-docs/wiki) - useful when writing /// comments
* [.NET Discord Server](https://aka.ms/dotnet-discord) - a place to talk and hang out with .NET community

## .NET Foundation

.NET Runtime is a [.NET Foundation](https://www.dotnetfoundation.org/projects) project.

There are many .NET related projects on GitHub.

- [.NET home repo](https://github.com/Microsoft/dotnet) - links to 100s of .NET projects, from Microsoft and the community.
- [ASP.NET Core home](https://docs.microsoft.com/aspnet/core/?view=aspnetcore-3.1) - the best place to start learning about ASP.NET Core.

This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior in our community. For more information, see the [.NET Foundation Code of Conduct](http://www.dotnetfoundation.org/code-of-conduct).

General .NET OSS discussions: [.NET Foundation forums](https://forums.dotnetfoundation.org)

## License

.NET (including the runtime repo) is licensed under the [MIT](LICENSE.TXT) license.
