using System.Reflection;

[assembly: AssemblyProduct("RqCalc")]
[assembly: AssemblyCompany("Anon")]

[assembly: AssemblyVersion("3.0.0.0")]
[assembly: AssemblyInformationalVersion("changes at build")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
