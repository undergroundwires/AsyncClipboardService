set local=%~dp0

set sourceDir=..\src\AsyncWindowsClipboard
set nuget=%local%nuget.exe

cd %sourceDir%

%nuget% spec 
 pause