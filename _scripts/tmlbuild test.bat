set /p TMLServerPath=<tmlpath.user

call "%TMLServerPath%" -build %*

@echo off
set /p end=Press any key to continue. . .