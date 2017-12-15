module NolRunner 

open canopy
open System
open System.Threading

let runNol (credentials : (string * string) Option) = 
    
    let username, password = 
        match credentials with 
        | Some (user, pass) -> 
            user, pass
        | None _ -> 
            // For security reasons keep username and password for your bossa account in environment variable 'bossaCredentials':
            //   Variable: bossaCredentials
            //   Value: abc9197;admin1234  (user name first then password separated by semicolon ';')
            let bossaCredentials = Environment.GetEnvironmentVariable("bossaCredentials", EnvironmentVariableTarget.User)
            let user = bossaCredentials.Split(';').[0]
            let pass = bossaCredentials.Substring(user.Length + 1)  // password might contain semicolons
            user, pass

    let exitNolRunner () = 
        Console.WriteLine("No Credentials!")
        Console.WriteLine("Press any key to exit...")
        Console.ReadLine() |> ignore
        quit()

    let startNol () = 

        start ie // InternetExplorer, not Edge
        pin FullScreen
        url "https://www.bossa.pl/bossa/login"
    
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

    
    if String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password) then
        exitNolRunner()
    else
        startNol()
       