open NolRunner
open System
open Argu
open AppArguments

[<EntryPoint>]
let main argv = 
    
    // parse arguments
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)
    let parser = ArgumentParser.Create<CLIArguments>(programName = "FsBossaNolRunner.exe", errorHandler = errorHandler)

    let arguments = parser.ParseCommandLine argv   
    let username, password = arguments.GetResult(<@ Credentials @>, defaultValue = (String.Empty, String.Empty))
    
    let credentials = 
        if String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password) then
            Option.None
        else 
            Option.Some (username, password)

    runNol credentials

    0 // return an integer exit code
