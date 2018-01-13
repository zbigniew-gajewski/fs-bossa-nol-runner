module NolRunner 

open canopy
open System
open System.Threading
open Utils

let validateEmptyCredentials (username, password) = 
    if String.IsNullOrEmpty(username) then 
        Failure "User Name can't be empty!"
    elif String.IsNullOrEmpty(password) 
        then Failure "Password can't be empty!"
    else 
        Success (username, password)  

let validateInitialCredentials (credentials : (string * string) Option) =
    consoleWriteLine "Validating initial credentials..." ConsoleColor.Blue
    match credentials with
    | Some (username, password) -> 
        let validatedCredentials = validateEmptyCredentials (username, password)
        match validatedCredentials with
        | Success s -> Success s
        | Failure f -> Failure f
    | None -> 
        Failure "No initial credentials provided!"

let getCredentialsFromEnvironmentVariables (fakeInputCredentials : (string * string) Option)  =
    // For security reasons keep username and password for your bossa account in environment variable 'bossaCredentials':
    //   Variable: bossaCredentials
    //   Value: user89098;tajneHaslo123 (user name first then password separated by semicolon ';')
    consoleWriteLine "Getting credentials from Environment variables..." ConsoleColor.Blue
    let bossaCredentials = Environment.GetEnvironmentVariable("bossaCredentials", EnvironmentVariableTarget.User)
    let username = bossaCredentials.Split(';').[0]
    let password = bossaCredentials.Substring(username.Length + 1)  // password might contain semicolons
    validateEmptyCredentials (username, password)
    
let validateCredentials = 
    validateInitialCredentials
    ||| 
    getCredentialsFromEnvironmentVariables

let startBrowser (credentials : string * string) =        
    consoleWriteLine "Starting browser..." ConsoleColor.Blue
    start ie // InternetExplorer, not Edge
    pin FullScreen
    url "https://www.bossa.pl/bossa/login"
    consoleWriteLine "Browser started!" ConsoleColor.Green
    credentials
    
let login  ((username, password) : string*string) =    
    consoleWriteLine "Login to bossa.pl starting..." ConsoleColor.Blue
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

let initNol (credentials : string*string) = 
    consoleWriteLine "Initializing Nol 3..." ConsoleColor.Blue
    js @"javascript:parent.initNol();" |> ignore    
    Thread.Sleep(TimeSpan.FromSeconds(5.0))
    credentials
            
let runNol (credentials : (string * string) Option) : unit =   
    
    let executionChain =         
        validateCredentials
        >=> tryCatch startBrowser "Error when starting browser!"
        >=> tryCatch login "Error when login to bossa account!"
        >=> tryCatch initNol "Error when initializing Nol 3!"
   
    let executionResult = credentials |> executionChain

    match executionResult with
    | Success (username, password) -> 
        consoleWriteLine (String.Format("Nol 3 initialized successfuly for user [{0}]!", username)) ConsoleColor.Green
    | Failure f -> 
       consoleWriteLine (String.Format("Initializing Nol 3 failed! Message: {0}'", f)) ConsoleColor.Red
    
    Console.WriteLine() 
    Console.WriteLine("Press any key to continue...") 
    Console.ReadLine() |> ignore
    quit()
    