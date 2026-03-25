@echo off
:: السطر ده بيخلي السكربت يروح لمكان الفولدر اللي هو فيه بالظبط
cd /d "%~dp0"

echo --- Adding changes ---
git add .

echo --- Committing ---
git commit -m "Auto-update: %date% %time%"

echo --- Pushing to GitHub ---
git push origin master

echo --- Done! Check your GitHub ---
pause