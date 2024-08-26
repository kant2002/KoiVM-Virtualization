@echo off
REM grammatica-1.5.jar is from https://github.com/cederberg/grammatica/releases/download/v1.5/grammatica-1.5.zip
java -jar libs/grammatica-1.5.jar docs/IRGrammer.txt --csoutput KoiVM/VMIR/Compiler --csnamespace KoiVM.VMIR.Compiler --csclassname IR
pause