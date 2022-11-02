using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ArmDot {
    internal class Decryptor {
        public static int Init(ModuleDefMD Module) {
            int count = 0;

            foreach(var type in Module.Types) {
                foreach(var method in type.Methods.Where(x => !x.IsNative && x.HasBody)) {
                    var body = method.Body;
                    var instrs = body.Instructions;
                    int startingIndex = 0;
                    for(int i=0;i<instrs.Count;i++) {
                        var instr = instrs[i];
                        if(instr.OpCode == OpCodes.Ldc_I4) {
                            if(instrs[i+1].OpCode == OpCodes.Newarr) {
                                bool isChar = instrs[i+1].Operand.ToString().Contains("Char");
                                if(isChar) {
                                    var times = instr.GetLdcI4Value();
                                    startingIndex = i;
                                    char[] chars = new char[times];
                                    i += 5;
                                    for(var j=0;j<times;j++) {
                                        chars[j] = (char)(instrs[i+1].GetLdcI4Value() ^ instrs[i+2].GetLdcI4Value());
                                        i += 6;
                                    }
                                    var obj = instrs.First(x => x.OpCode == OpCodes.Newobj && x.Operand.ToString().Contains("String"));
                                    var lol = instrs.IndexOf(obj);
                                    if(instrs[lol-1].OpCode == OpCodes.Stelem_I2 && instrs[lol-2].OpCode == OpCodes.Xor) {
                                        obj.OpCode = OpCodes.Ldstr;
                                        obj.Operand = new string(chars);
                                        count++;
                                    }
                                    while(startingIndex < lol) {
                                        instrs[startingIndex].OpCode = OpCodes.Nop;
                                        startingIndex++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return count;
        }
    }
}
