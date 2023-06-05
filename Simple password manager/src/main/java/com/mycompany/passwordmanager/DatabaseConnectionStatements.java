/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package com.mycompany.passwordmanager;

import static com.mycompany.passwordmanager.PasswordManager.id;
import static com.mycompany.passwordmanager.PasswordManager.masterPassword;
import java.math.BigInteger;
import java.nio.charset.StandardCharsets;
import java.security.KeyPair;
import java.security.KeyPairGenerator;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.PublicKey;
import java.security.SecureRandom;
import java.security.Signature;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import static java.util.Objects.hash;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.crypto.Cipher;
import javax.swing.JOptionPane;
import javax.swing.JPanel;

/**
 *
 * @author marie
 */
public class DatabaseConnectionStatements {

    Connection c = null;
    public Statement stmt = null;
    PreparedStatement pst = null;


    public void insertStatement(String data) {
        try {
            connectionStatement();

            String insertStatement = "INSERT INTO MasterPasswordManager (MASTERPASSWORD) VALUES(?)";
            pst = c.prepareStatement(insertStatement);
            pst.setString(1, data);
            pst.execute();
            stmt.close();

        } catch (SQLException e) {
            System.err.println(e.getClass().getName() + ": " + e.getMessage());
            System.exit(0);
        }
    }
    
  

    public String selectStatementFromMasterPassword() {
        connectionStatement();
        try {
            connectionStatement();
          
            stmt = c.createStatement();
            ResultSet rs = stmt.executeQuery("SELECT * FROM MasterPasswordManager;");

            while (rs.next()) {
                id = rs.getInt("id");
                masterPassword = rs.getString("masterPassword");

            }

        } catch (SQLException e) {
          System.err.println(e.getClass().getName() + ": " + e.getMessage());
        }
        
        return masterPassword;
    }

    public Connection connectionStatement() {

        try {
            Class.forName("org.sqlite.JDBC");
            c = DriverManager.getConnection("jdbc:sqlite:masterPassword.db");

        } catch (ClassNotFoundException | SQLException e) {
            System.err.println(e.getClass().getName() + ": " + e.getMessage());
            System.exit(0);
        }
        
        return c;
    }
}
