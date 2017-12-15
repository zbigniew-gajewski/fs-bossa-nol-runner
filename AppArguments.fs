module AppArguments

open Argu

[<CliPrefix(CliPrefix.DoubleDash)>]
[<NoAppSettings>]
type CLIArguments = 
    | Credentials of username:string * password:string
with 
    interface IArgParserTemplate with
        member arg.Usage = 
            match arg with
            | Credentials _ -> "bossa.pl username and password."

