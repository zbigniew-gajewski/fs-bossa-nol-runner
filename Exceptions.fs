module Exceptions

type RunningResult = 
    | Success
    | EmptyCredentialsException of string
    | LoginException of string

