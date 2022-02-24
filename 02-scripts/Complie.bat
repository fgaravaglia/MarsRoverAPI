@ECHO OFF

cd ..\01-src\MarsROver.Driving
ECHO Building MarsRover.Driving....
dotnet restore -v=q
dotnet build -v=q
if %errorlevel% neq 0 goto :error

ECHO Building MarsRover.Driving.Tests....
cd ..\MarsRover.Driving.Tests
dotnet build -v=q
if %errorlevel% neq 0 goto :error

ECHO Building MarsRover.Driving.Api....
cd ..\MarsRover.Driving.Api
dotnet build -v=q
if %errorlevel% neq 0 goto :error

ECHO Executing Tests...
cd ..\MarsRover.Driving.Tests
dotnet test --no-build --logger trx --filter="TestCategory=DomainService"
if %errorlevel% neq 0 goto :error

goto :success

:success
ECHO ---------------------------------------
ECHO .
ECHO Build Succeeded!
ECHO .
ECHO ---------------------------------------
exit

:error
ECHO ***************************************************
ECHO  Failed with error #%errorlevel% !!!!
exit /b %errorlevel%