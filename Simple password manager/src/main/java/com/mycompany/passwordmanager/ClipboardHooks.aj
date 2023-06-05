package aspects;

import javax.swing.*;
import java.sql.*;
import java.awt.datatransfer.*;
import java.awt.datatransfer.StringSelection;
import java.io.*;
import java.time.LocalDateTime;

public aspect ClipboardHooks{


   //public void setContents(Transferable contents,ClipboardOwner owner)

   pointcut setContentsHook(Transferable contents, ClipboardOwner owner) :
    call(void Clipboard.setContents(Transferable, ClipboardOwner))
    && args(contents, owner);

    void around(Transferable contents, ClipboardOwner owner) : setContentsHook(contents, owner){
        proceed(contents, owner);
        try
        {
            log("Clipboard.setContents() Result: " +  contents.getTransferData(DataFlavor.stringFlavor));
        }
        catch (UnsupportedFlavorException ex) {
            System.out.println("");
        }
        catch (IOException ex) {
                    System.out.println("");
        }

    }

    void log(String msg)
    {
        LocalDateTime myDate = LocalDateTime.now();
        System.out.println(">>> Timeline: " + myDate + " " + msg);

    }
}