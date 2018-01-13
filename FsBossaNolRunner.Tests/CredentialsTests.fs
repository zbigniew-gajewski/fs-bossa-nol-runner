// https://mnie.github.io/2016-09-26-UnitTestsInFSharp/
// plese install XUnit using Package Manager Console !!!

namespace FsBossaNolRunner.Tests

open System
open Xunit
open FsCheck
open FsCheck.Xunit
open Types
open Utils
open NolRunner

module Specification = 
    
    
    [<Property>]
    let ``FsCheck infrastructure works``(credentials : Credentials) =
        true ==> true

    [<Fact>]
    let ``not empty user name and not empty password validation returns Success``() =
        let credentials = ("userName", "password")
        let expected = Success credentials
        let actual = validateEmptyCredentials credentials
        Assert.Equal(expected, actual)

    [<Fact>]
    let ``empty user name validation returns Failure``() =
        let credentials = ("", "password")
        let expected = Failure "User Name can't be empty!"
        let actual = validateEmptyCredentials credentials
        Assert.Equal(expected, actual)

    [<Fact>]
    let ``empty password validation returns Failure``() =
        let credentials = ("username", "")
        let expected = Failure "Password can't be empty!"
        let actual = validateEmptyCredentials credentials
        Assert.Equal(expected, actual)

  
        