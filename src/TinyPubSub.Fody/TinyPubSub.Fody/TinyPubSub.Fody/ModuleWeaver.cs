using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Cecil.Cil;
using Fody;

namespace TinyPubSub.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            var ns = GetNamespace();
            var objectRef = ModuleDefinition.ImportReference(FindType("System.Object"));
            var newType = new TypeDefinition(ns, "Hello", TypeAttributes.Public, objectRef);

            AddConstructor(newType);

            AddHelloWorld(newType);

            ModuleDefinition.Types.Add(newType);
            LogInfo("Added type 'Hello' with method 'World'.");
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield return "netstandard";
            yield return "mscorlib";
        }

        string GetNamespace()
        {
            var attributes = ModuleDefinition.Assembly.CustomAttributes;
            var namespaceAttribute = attributes.FirstOrDefault(x => x.AttributeType.FullName == "NamespaceAttribute");
            if (namespaceAttribute == null)
            {
                return null;
            }
            attributes.Remove(namespaceAttribute);
            return (string)namespaceAttribute.ConstructorArguments.First().Value;
        }

        void AddConstructor(TypeDefinition newType)
        {
            var voidRef = ModuleDefinition.ImportReference(FindType("System.Void"));
            var method = new MethodDefinition(".ctor", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, voidRef);
            var objectConstructor = ModuleDefinition.ImportReference(FindType("System.Object").GetConstructors().First());
            var processor = method.Body.GetILProcessor();
            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Call, objectConstructor);
            processor.Emit(OpCodes.Ret);
            newType.Methods.Add(method);
        }

        void AddHelloWorld(TypeDefinition newType)
        {
            var stringRef = ModuleDefinition.ImportReference(FindType("System.String"));
            var method = new MethodDefinition("World", MethodAttributes.Public, stringRef);
            var processor = method.Body.GetILProcessor();
            processor.Emit(OpCodes.Ldstr, "Hello World");
            processor.Emit(OpCodes.Ret);
            newType.Methods.Add(method);
        }

        public override bool ShouldCleanReference => true;
    }
}
