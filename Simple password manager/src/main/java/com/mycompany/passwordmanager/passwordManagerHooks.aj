package com.mycompany.passwordmanager;
import javax.swing.*;
import java.util.*;
import java.lang.NullPointerException;
import java.io.*;
import javax.swing.JFrame.*;
public aspect passwordManagerHooks{




    pointcut getTextHook() :
    (
        call(String javax.swing.JComponent.getText()) ||
        call(String javax.swing.JTextField.getText())

    );

    Object around () throws NullPointerException : getTextHook()
    {
        System.out.println(">>Before hook");
        char[] line = proceed();
        System.out.println(">> " + line);
        System.out.println(">>After hook");
        return line;
    }





}