﻿@ECHO OFF

REM The following directory is for .NET 2.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%

echo Uninstalling MyService...
echo ---------------------------------------------------
InstallUtil /u Vault.Integration.ResponseClient.exe
echo ---------------------------------------------------
echo Done.
pause