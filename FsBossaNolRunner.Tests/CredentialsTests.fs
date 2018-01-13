// https://mnie.github.io/2016-09-26-UnitTestsInFSharp/
// plese install XUnit using Package Manager Console !!!

namespace FsBossaNolRunner.Tests

open System
open Xunit
open FsCheck
open FsCheck.Xunit
open Types
open Utils

module Specification = 
    open NolRunner
    
    [<Property>]
    let ``user name should not be empty``(credentials : Credentials) =
        true ==> true

    [<Fact>]
    let ``not empty user name and not empty password validation returns Success``() =
        let credentials = ("userName", "password")
        let result = validateEmptyCredentials credentials
        Assert.Equal(Success credentials, result)

    [<Fact>]
    let ``empty user name validation returns Failure``() =
        let credentials = ("", "password")
        let result = validateEmptyCredentials credentials
        Assert.Equal(Failure "User Name can't be empty!", result)

    [<Fact>]
    let ``empty password validation returns Failure``() =
        let credentials = ("username", "")
        let result = validateEmptyCredentials credentials
        Assert.Equal(Failure "Password can't be empty!", result)

  
        