module Types

open System

//type NolRunnerException =
//    | EmptyUserNameException of ArgumentException 
//    | EmptyPasswordException of ArgumentException 
//    | NoInitialCredentialsException of ArgumentException 

type Result<'TSuccess, 'TFailure> =
    | Success of 'TSuccess
    | Failure of 'TFailure

type UserName = string

type Password = string

type Credentials = UserName * Password



