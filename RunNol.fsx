// Some useful F# scripting links:
// https://blogs.msdn.microsoft.com/chrsmith/2008/09/12/scripting-in-f/
// http://brandewinder.com/2016/02/06/10-fsharp-scripting-tips/

#I @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1"
#I @"C:\TFS\fs-bossa-nol-runner\packages" // your source path

#r @"System.dll"
#r @"System.Core.dll"
#r @"System.Core.dll"
#r @"Argu.4.0.0\lib\net40\Argu.dll"
#r @"Selenium.WebDriver.3.8.0\lib\net45\WebDriver.dll" // must be just before referencing canopy.dll
#r @"canopy.1.6.1\lib\canopy.dll"

#load "NolRunner.fs"

open NolRunner
runNol None