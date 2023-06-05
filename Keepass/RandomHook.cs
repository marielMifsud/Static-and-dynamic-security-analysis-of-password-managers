using AspectInjector.Broker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace KeePassLib.Cryptography.PasswordGenerator
{
    [Aspect(Scope.Global)]
    [Injection(typeof(RandomHook))]
    public class RandomHook : Attribute
    {
        [Advice(Kind.After, Targets = Target.Method)]
        public void GetRandomChar(
            [Argument(Source.Name)] string name,
            [Argument(Source.ReturnValue)] object returnedVsalue)
        {
            Console.WriteLine($">>>>[{DateTime.UtcNow}] Method  started hooking method: " + name);
            
            byte[] asciiBytes = Encoding.ASCII.GetBytes(returnedVsalue.ToString());

            for (int i = 0; i < asciiBytes.Length; i++)
            {
                Console.WriteLine(">>> Random generated characther with ascii code: " + asciiBytes[i] + " with characther: " + returnedVsalue);
            }

        }
    }
}
