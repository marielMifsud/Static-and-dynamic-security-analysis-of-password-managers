/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Project/Maven2/JavaApp/src/main/java/${packagePath}/${mainClassName}.java to edit this template
 */
package com.mycompany.passwordmanager;

import java.awt.Cursor;
import java.awt.Toolkit;
import java.awt.datatransfer.Clipboard;
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.SecureRandom;
import java.sql.Array.*;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author marie
 */
public class PasswordManager {

    public static int id;
    public static String masterPassword;

    public static void main(String[] args) {
        logIn logInForm = new logIn();
        logInForm.setVisible(true);

        Connection c = null;
        Statement stmt = null;

        try {
            Class.forName("org.sqlite.JDBC");
            c = DriverManager.getConnection("jdbc:sqlite:masterPassword.db");
            System.out.println("Opened database successfully");

            stmt = c.createStatement();
            String sql = "CREATE TABLE IF NOT EXISTS MasterPasswordManager "
                    + "(ID INTEGER PRIMARY KEY   AUTOINCREMENT,"
                    + "MASTERPASSWORD TEXT    NOT NULL)";
            stmt.executeUpdate(sql);
            stmt.close();

            stmt = c.createStatement();
            String sql2 = "CREATE TABLE IF NOT EXISTS passwordEntries "
                    + "(ID INTEGER PRIMARY KEY   AUTOINCREMENT,"
                    + "TITLE TEXT NOT NULL,"
                    + "USERNAME TEXT NOT NULL,"
                    + "PASSWORD TEXT NOT NULL,"
                    + "URL TEXT NOT NULL)";
            stmt.executeUpdate(sql2);
            stmt.close();

            stmt = c.createStatement();
            ResultSet rs = stmt.executeQuery("SELECT * FROM MasterPasswordManager;");

            if (rs.next() == false) {
                Register registerForm = new Register();
                registerForm.setVisible(true);
                logInForm.dispose();
            } else {
                while (rs.next()) {

                    Register registerData = new Register();

                    id = rs.getInt("id");
                    masterPassword = rs.getString("masterPassword");

                    System.out.println("ID = " + id);
                    System.out.println("Master password = " + masterPassword);

                  
                }
            }

            rs.close();
            stmt.close();

            c.close();

        } catch (Exception e) {
            System.err.println(e.getClass().getName() + ": " + e.getMessage());
            System.exit(0);
        }
        System.out.println("operation done successfully");

    }

}
