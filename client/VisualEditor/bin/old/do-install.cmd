@echo off
reg import pattern.reg
reg add HKEY_CLASSES_ROOT\htpfile\DefaultIcon /ve /t REG_SZ /d "%~dp0htpfile.ico" /f
reg add HKEY_CLASSES_ROOT\htpfile\shell\open\command /ve /t REG_SZ /d "\"%~dp0VisualEditor.exe\" \"%%1\"" /f
