using System;
using System.Reflection;

class Program {
    static void Main() {
        var asm = Assembly.LoadFrom(@"C:\Users\MPLopes\.nuget\packages\microsoft.ml.tokenizers\2.0.0\lib\net8.0\Microsoft.ML.Tokenizers.dll");
        foreach(var t in asm.GetTypes()) {
            if (t.IsPublic) Console.WriteLine(t.FullName);
        }
    }
}
