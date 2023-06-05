using AspectInjector.Broker;
using KeePassLib.Collections;
using KeePassLib.Interfaces;
using KeePassLib.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KeePassLib.Serialization
{
    //This is the database hook for saving an entry
    
    [Aspect(Scope.Global)]
    [Injection(typeof(DatabaseHooks))]
    public class DatabaseHooks : Attribute
    {
        [Advice(Kind.After, Targets = Target.Method)]
        public void savingToDatabase(
           [Argument(Source.Name)] string name,
           [Argument(Source.Arguments)] object[] args,
           [Argument(Source.ReturnValue)] object returnedVsalue)
        {

            var d = args[0];
            Type dType = d.GetType();
            MethodInfo[] dMethodInfo = dType.GetMethods();
            MethodInfo dSingleMethodInfo = dType.GetMethod("ReadSafe");
            object title = dSingleMethodInfo.Invoke(d, new object[] { "Title" });
            object username = dSingleMethodInfo.Invoke(d, new object[] { "UserName" });
            object password = dSingleMethodInfo.Invoke(d, new object[] { "Password" });
            object url = dSingleMethodInfo.Invoke(d, new object[] { "URL" });
            object notes = dSingleMethodInfo.Invoke(d, new object[] { "Notes" });
            Console.WriteLine(">> Title: " + title);
            Console.WriteLine(">> Username: " + username);
            Console.WriteLine(">> Password: " + password);
            Console.WriteLine(">> URL: " + url);
            Console.WriteLine(">> Notes: " + notes);
        }
    }
}
