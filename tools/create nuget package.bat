set local=%~dp0

set sourceDir=..\src\AsyncWindowsClipboard
set nuget=%local%nuget.exe

cd %sourceDir%

%nuget% pack AsyncWindowsClipboard.csproj -Prop Configuration=Release
 pause