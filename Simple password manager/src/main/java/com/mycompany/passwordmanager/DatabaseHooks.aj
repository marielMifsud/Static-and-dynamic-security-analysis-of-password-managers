package aspects;

import javax.swing.*;
import java.sql.*;
import java.awt.datatransfer.*;
import java.awt.datatransfer.StringSelection;
import java.io.*;
import java.time.LocalDateTime;

public aspect DatabaseHooks{

    pointcut sqlStatementHook() :
        call(PreparedStatement Connection.prepareStatement(String));


    Object around() : sqlStatementHook(){


        Object stmt = proceed();
        log("Connection.prepareStatement() Result: " + stmt.toString());
        return stmt;

    }

    pointcut setStringHook(int index , String data) :
        call(void PreparedStatement.setString(int, String))
        && args(index, data);


    Object around(int index, String data) : setStringHook(index , data){

        log("PreparedStatement.setString() Result: " + index + " " + data);
        return proceed(index, data);
    }

    pointcut sqlSelectStatement(String statement) :
        call(ResultSet Statement.executeQuery(String))
        && args(statement);

    ResultSet around(String statement) : sqlSelectStatement(statement)
    {
       log("ResultSet Statement.executeQuery() Result: " + statement);
        return proceed(statement);
    }

    // String getString(String columnLabel) throws SQLException

    pointcut getStringHook(String columnName) :
        call(String ResultSet.getString(String))
        && args(columnName);

    String around(String columnName) : getStringHook(columnName){

        log("Resultset.getString() Result: " + columnName);
        return proceed(columnName);
    }

    void log(String msg)
    {
       LocalDateTime myDate = LocalDateTime.now();
       System.out.println(">>> Timeline: " + myDate + " " + msg);

    }

}