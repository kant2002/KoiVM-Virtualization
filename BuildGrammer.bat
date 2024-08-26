@echo off
java -jar libs/grammatica-1.5.jar docs/IRGrammer.txt --csoutput KoiVM/VMIR/Compiler --csnamespace KoiVM.VMIR.Compiler --csclassname IR
pause