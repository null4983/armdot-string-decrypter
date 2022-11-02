using dnlib.DotNet;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmDot {
    internal class Program {
        static void Main(string[] args) {
            string path = args[0];
            var module = ModuleDefMD.Load(path);
            Decryptor.Init(module);
            if(module.IsILOnly) {
                ModuleWriterOptions options = new ModuleWriterOptions(module);
                options.Logger = DummyLogger.NoThrowInstance;
                module.Write(path.Replace(".exe", "-deobf.exe"), options);
                return;
            }
            NativeModuleWriterOptions writer = new NativeModuleWriterOptions(module, false);
            writer.Logger = DummyLogger.NoThrowInstance;
            module.NativeWrite(path.Replace(".exe", "-deobf.exe"), writer);
        }
    }
}
