@echo off
echo Cleaning bin and obj folders...

if exist bin (
    echo Deleting bin...
    rmdir /s /q bin
)

if exist obj (
    echo Deleting obj...
    rmdir /s /q obj
)

echo Done.
pause
