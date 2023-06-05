using AspectInjector.Broker;
using KeePassLib.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KeePassLib.Serialization
{
    [Aspect(Scope.Global)]
    [Injection(typeof(ReadingDatabaseHook))] 
    public class ReadingDatabaseHook : Attribute
    {
        [Advice(Kind.After, Targets = Target.Method)]
        public void readingFromDatabase(
         [Argument(Source.Name)] string name,
         [Argument(Source.Arguments)] object[] args,
         [Argument(Source.ReturnValue)] object returnedVsalue)
        {
      
            Console.WriteLine(">> : " + returnedVsalue.GetType().GetMethod("CloneShallowToList").Invoke(returnedVsalue, null));
            bool result = true;

            object list = returnedVsalue.GetType().GetMethod("CloneShallowToList").Invoke(returnedVsalue, null);
            List<object> mlist = ((IEnumerable)list).Cast<object>().ToList();
            
            Console.WriteLine(">> Count of entries: " + mlist.Count);
            for(int i =0; i < mlist.Count; i++)
            {
                object mListObject = mlist[i];
                Type mListObjectType = mListObject.GetType();

                object resultMList = mListObjectType.InvokeMember("Strings", BindingFlags.GetProperty, null, mlist[i], null);
                Type resultMListType = resultMList.GetType();

                MemberInfo[] resultMListInfo = resultMListType.GetMembers();
                List<string> getKeys = (List<string>)resultMListType.InvokeMember("GetKeys", BindingFlags.InvokeMethod, null, resultMList, null);

                string title = (string)resultMListType.InvokeMember("ReadSafe", BindingFlags.InvokeMethod, null, resultMList, new object[] { getKeys[0] });
                title = (string)resultMListType.InvokeMember("ReadSafe", BindingFlags.InvokeMethod, null, resultMList, new object[] { "Title" });

                string username = (string)resultMListType.InvokeMember("ReadSafe", BindingFlags.InvokeMethod, null, resultMList, new object[] { getKeys[0] });
                username = (string)resultMListType.InvokeMember("ReadSafe", BindingFlags.InvokeMethod, null, resultMList, new object[] { "Username" });

                string pass = (string)resultMListType.InvokeMember("ReadSafe", BindingFlags.InvokeMethod, null, resultMList, new object[] { getKeys[0] });
                pass = (string)resultMListType.InvokeMember("ReadSafe", BindingFlags.InvokeMethod, null, resultMList, new object[] { "Password" });

                string notes = (string)resultMListType.InvokeMember("ReadSafe", BindingFlags.InvokeMethod, null, resultMList, new object[] { getKeys[0] });
                notes = (string)resultMListType.InvokeMember("ReadSafe", BindingFlags.InvokeMethod, null, resultMList, new object[] { "Notes" });

                string url = (string)resultMListType.InvokeMember("ReadSafe", BindingFlags.InvokeMethod, null, resultMList, new object[] { getKeys[0] });
                url = (string)resultMListType.InvokeMember("ReadSafe", BindingFlags.InvokeMethod, null, resultMList, new object[] { "URL" });


                Console.WriteLine("\n \n>> Title of entry " + i + ": " + title);
                Console.WriteLine(">> Username of entry " + i + ": " + username);
                Console.WriteLine(">> Password of entry " + i + ": " + pass);
                Console.WriteLine(">> Notes of entry " + i + ": " + notes);
                Console.WriteLine(">> URL of entry " + i + ": " + url);
            }
            


        }

    }
}
