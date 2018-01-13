// Mostly taken from: https://fsharpforfunandprofit.com/posts/recipe-part2/

module Utils

open System

type Result<'TSuccess, 'TFailure> =
    | Success of 'TSuccess
    | Failure of 'TFailure

let bothMustSucceed addSuccess addFailure switch1 switch2 x = 
    match (switch1 x), (switch2 x) with
    | Success s1, Success s2 -> Success (addSuccess s1 s2)
    | Failure f1, Success _  -> Failure f1
    | Success _ , Failure f2 -> Failure f2
    | Failure f1, Failure f2 -> Failure (addFailure f1 f2)

let (&&&) v1 v2 = 
    let addSuccess r1 r2 = r1 // return first
    let addFailure s1 s2 = s1 + "; " + s2  // concat
    bothMustSucceed addSuccess addFailure v1 v2 

let atLeastOneSuccess addSuccess addFailure switch1 switch2 x = 
    match (switch1 x), (switch2 x) with
    | Success s1, Success s2 -> Success (addSuccess s1 s2)
    | Failure f1, Success s2 -> Success s2
    | Success s1, Failure f2 -> Success s1
    | Failure f1, Failure f2 -> Failure (addFailure f1 f2)

let (|||) v1 v2 = 
    let addSuccess r1 r2 = r1 // return first
    let addFailure s1 s2 = s1 + "; " + s2  // concat
    atLeastOneSuccess addSuccess addFailure v1 v2 


let bind switchFunction = 
    function
    | Success s -> switchFunction s
    | Failure f -> Failure f

let (>>=) switchFunction = 
    bind switchFunction

let tryCatch f message x  =
    try 
        f x |> Success
    with
    | ex -> Failure (message + " " + ex.Message)

let switch f x = 
    f x |> Success

let tee f x = 
    f x |> ignore
    x

let map oneTrackFunction twoTrackInput = 
    match twoTrackInput with
    | Success s -> Success (oneTrackFunction s)
    | Failure f -> Failure f

let (>=>) switch1 switch2 x = 
    match switch1 x with
    | Success s -> switch2 s
    | Failure f -> Failure f 

let consoleWriteLine (message : string) (color : ConsoleColor) : unit = 
    let oldColor = Console.ForegroundColor
    Console.ForegroundColor <- color
    Console.WriteLine message
    Console.ForegroundColor <- oldColor
    