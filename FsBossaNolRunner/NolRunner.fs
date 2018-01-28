module NolRunner 

open canopy
open System
open System.Threading
open Types
open Utils

let validateEmptyCredentials credentials = 
    let username, password = credentials
    if String.IsNullOrEmpty(username) then Failure "User Name can't be empty!"
    elif String.IsNullOrEmpty(password) then Failure "Password can't be empty!"
    else Success credentials  

let validateInitialCredentials (optionalCredentials : Credentials Option) =
    consoleWriteLine "Validating initial credentials..." ConsoleColor.Blue
    match optionalCredentials with
    | Some credentials -> validateEmptyCredentials credentials
    | None -> Failure "No initial credentials provided!"

let getVariablesFromEnvironment () = 
    let bossaCredentials = Environment.GetEnvironmentVariable("bossaCredentials", EnvironmentVariableTarget.User)
    let username = bossaCredentials.Split(';').[0]
    let password = bossaCredentials.Substring(username.Length + 1)  // password might contain semicolons
    username, password    

let getCredentialsFromEnvironmentVariables getVariables (fakeInputCredentials : Credentials Option)  =
    // For security reasons keep username and password for your bossa account in environment variable 'bossaCredentials':
    //   Variable: bossaCredentials
    //   Value: user89098;tajneHaslo123 (user name first then password separated by semicolon ';')
    consoleWriteLine "Getting credentials from Environment variables..." ConsoleColor.Blue
    let credentialsFromEnvironment = getVariables ()
    validateEmptyCredentials credentialsFromEnvironment
    
let validateCredentials getVariables = 
    validateInitialCredentials
    ||| 
    (getCredentialsFromEnvironmentVariables getVariables)

let startBrowser (credentials : Credentials) =        
    consoleWriteLine "Starting browser..." ConsoleColor.Blue
    start ie // InternetExplorer, not Edge
    pin FullScreen
    url "https://www.bossa.pl/bossa/login"
    consoleWriteLine "Browser started!" ConsoleColor.Green
    credentials
    
let login  (credentials : Credentials) =    
    consoleWriteLine "Login to bossa.pl starting..." ConsoleColor.Blue
    let username, password = credentials
    let loginString = 
        String.Format(" var f = document.forms.login; 
            f.LgnUsrNIK.value='{0}'; 
            f.LgnUsrPIN.value='{1}'; 
            f.LgnUsrPIN.focus(); ", username, password)
    js loginString |> ignore
    press enter
    Thread.Sleep(TimeSpan.FromSeconds(1.0))
    consoleWriteLine "Login to bossa.pl finished!" ConsoleColor.Green
    username, password

let initNol (credentials : Credentials) = 
    consoleWriteLine "Initializing Nol 3..." ConsoleColor.Blue
    js @"javascript:parent.initNol();" |> ignore    
    Thread.Sleep(TimeSpan.FromSeconds(5.0))
    credentials


// main execution path

let runNol (credentials : Credentials Option) : unit =   
    
    let executionChain =         
        (validateCredentials getVariablesFromEnvironment)
        >=> tryCatch startBrowser "Error when starting browser!"
        >=> tryCatch login "Error when login to bossa account!"
        >=> tryCatch initNol "Error when initializing Nol 3!"
   
    let executionResult = credentials |> executionChain

    match executionResult with
    | Success (username, password) -> 
        consoleWriteLine (String.Format("Nol 3 initialized successfuly for user [{0}]!", username)) ConsoleColor.Green
    | Failure f -> 
       consoleWriteLine (String.Format("Initializing Nol 3 failed! {0}'", f)) ConsoleColor.Red
    
    Console.WriteLine() 
    Console.WriteLine("Press any key to continue...") 
    Console.ReadLine() |> ignore
    
    quit()
    