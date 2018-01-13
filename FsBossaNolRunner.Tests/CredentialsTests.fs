// https://mnie.github.io/2016-09-26-UnitTestsInFSharp/
// please install XUnit using Package Manager Console !!! Otherwise Test Explorer in VS will not discover tests.

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

    [<Fact>]
    let ``some credentials with empty username validation returns Failure``() =
        let credentials = "", "password"
        let optionalCredentials = Some credentials
        let expected = Failure "User Name can't be empty!"
        let actual = validateInitialCredentials optionalCredentials
        Assert.Equal(expected, actual)

    [<Fact>]
    let ``some credentials with empty password validation returns Failure``() =
        let credentials = "username", ""
        let optionalCredentials = Some credentials
        let expected = Failure "Password can't be empty!"
        let actual = validateInitialCredentials optionalCredentials
        Assert.Equal(expected, actual)

    [<Fact>]
    let ``some not empty credentials validation returns Success``() =
        let credentials = "username", "password"
        let optionalCredentials = Some credentials
        let expected = Success credentials
        let actual = validateInitialCredentials optionalCredentials
        Assert.Equal(expected, actual)

    [<Fact>]
    let ``none credentials validation returns Failure``() =
        let optionalCredentials = None
        let expected = Failure "No initial credentials provided!"
        let actual = validateInitialCredentials optionalCredentials
        Assert.Equal(expected, actual)

  
        