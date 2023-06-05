package aspects;

import javax.swing.*;
import java.sql.*;
import java.awt.datatransfer.*;
import java.awt.datatransfer.StringSelection;
import java.io.*;
import java.time.LocalDateTime;



public aspect Hooks {
    pointcut setLabelHook(String lbl) :
        call(void JLabel.setText(String))
        && args(lbl);

    Object around(String lbl) : setLabelHook(lbl) {
        lbl = "modified_by_hook";
        log("Label hook");
        return proceed(lbl);
    }

    pointcut getPwHook(): call(char[] JPasswordField.getPassword());

    char[] around() : getPwHook() {

        char[] pw = proceed();
        log("JPasswordField.getPassword() Result: " + new String(pw));
        return pw;
    }



    void log(String msg)
    {
        LocalDateTime myDate = LocalDateTime.now();
        System.out.println(">>> Timeline: " + myDate + " " + msg);

    }
}