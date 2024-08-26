using System;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace KoiVM.Driver
{
    internal class Program
    {
#if DEBUG
        private const bool Debug = true;
#else
		const bool Debug = false;
#endif

        private static int Main(string[] args)
        {
            var resolver = new AssemblyResolver();
            resolver.EnableTypeDefCache = true;
            resolver.DefaultModuleContext = new ModuleContext(resolver);

            if (args.Length < 2)
            {
                Console.WriteLine("Usage: ");
                Console.WriteLine("    driver <fileToEncrypt> <runtimeLibrary>");
                return -1;
            }
            var fileToEncrypt = args[0];
            var runtimeLibrary = args[1];
            var module = ModuleDefMD.Load(fileToEncrypt, resolver.DefaultModuleContext);
            //if(Debug)
            //    module.LoadPdb();
            var vr = new Virtualizer(100, Debug);
            vr.ExportDbgInfo = Debug;
            vr.Initialize(ModuleDefMD.Load(runtimeLibrary, resolver.DefaultModuleContext));
            vr.AddModule(module);

            vr.ProcessMethods(module);
            var listener = vr.CommitModule(module);
            vr.CommitRuntime();

            var dir = Path.GetDirectoryName(fileToEncrypt);
            vr.SaveRuntime(dir);
            var options = new ModuleWriterOptions(module);
            options.WriterEvent += (s, e) => listener.OnWriterEvent((ModuleWriter)s, e.Event);
            module.Write(Path.Combine(dir, "Test.virtualized.exe"), options);
            if(Debug)
                File.WriteAllBytes(Path.Combine(dir, "Test.virtualized.map"), vr.Runtime.DebugInfo);
            return 0;
        }
    }
}