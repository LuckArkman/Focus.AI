using System;
using MediatR;
using System.Reflection;

class Program {
    static void Main() {
        var type = typeof(RequestHandlerDelegate<string>);
        var invoke = type.GetMethod("Invoke");
        var parameters = invoke.GetParameters();
        Console.WriteLine($"RequestHandlerDelegate takes {parameters.Length} arguments:");
        foreach (var p in parameters) {
            Console.WriteLine($"{p.ParameterType.Name} {p.Name}");
        }
    }
}
