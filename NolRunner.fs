module NolRunner 

open canopy
open System
open System.Threading

let runNol () = 
    
    // you can add firefox or chrome support here 
    start ie // InternetExplorer, not Edge
    pin FullScreen
    url "https://www.bossa.pl/bossa/login"

    // For security reasons keep username and password for your bossa account in environment variable 'bossaCredentials':
    //   Variable: bossaCredentials
    //   Value: abc9197;admin1234  (user name first then password separated by semicolon ';')
    let bossaCredentials = Environment.GetEnvironmentVariable("bossaCredentials", EnvironmentVariableTarget.User)
    let username = bossaCredentials.Split(';').[0]
    let password = bossaCredentials.Substring(username.Length + 1)  // password might contain semicolons
    
    let loginString = 
        String.Format(" var f = document.forms.login; 
            f.LgnUsrNIK.value='{0}'; 
            f.LgnUsrPIN.value='{1}'; 
            f.LgnUsrPIN.focus(); ", username, password)
    
    js loginString |> ignore

    press enter

    js @"javascript:parent.initNol();" |> ignore
   
    Thread.Sleep(TimeSpan.FromSeconds(5.0));
    
    quit() // This closes the browser and finishes the app. Comment it if you want to keep the browser opened