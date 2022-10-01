@echo off
PUSHD .

xcopy /s/e/v/y/z .\Updates\ .
rmdir /s /q .\Updates

start "Autodarts-Desktop" autodarts-desktop.exe