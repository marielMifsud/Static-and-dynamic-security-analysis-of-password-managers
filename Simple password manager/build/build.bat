SET JAVA_HOME=C:\Workspace\jdk1.8.0_152
SET AJPATH=C:\Workspace\bin
SET CLASSPATH=.;C:\Workspace\sqlite-jdbc-3.36.0.3.jar;C:\Workspace\lib\aspectjtools.jar;C:\Workspace\lib\aspectjrt.jar;C:\Users\User\Documents\GitHub\dissertationRepo\passwordManager;



cd ..
cd src\main\java\com\mycompany\passwordmanager
pause

javac -d C:\Users\User\Documents\GitHub\dissertationRepo\passwordManager\bin DatabaseConnectionStatements.java logIn.java NewEntry.java PasswordManager.java passwordManagerList.java Register.java

call ajc -d C:\Users\User\Documents\GitHub\dissertationRepo\passwordManager\bin passwordManagerHooks.aj

java -javaagent:C:\Workspace\lib\aspectjweaver.jar -jar C:\Users\User\Documents\GitHub\dissertationRepo\passwordManager\bin\com\mycompany\passwordmanager\passwordManager-1.0-SNAPSHOT.jar