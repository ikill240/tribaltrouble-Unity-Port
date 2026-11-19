@echo off
rem Makes (or refreshes) the single-game projects next to the merged one (Tribal Trouble - the Steam release -,
rem Tribal Trouble 1.4, New Tides and Tiki Madness): see split_games.ps1.
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0split_games.ps1"
pause
