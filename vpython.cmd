@echo off
set original_dir=%CD%
set venv_root_dir=%CD%\.venv

cd %venv_root_dir%

call %venv_root_dir%\Scripts\activate.bat

echo Run command "python %*"
python %*
call %venv_root_dir%\Scripts\deactivate.bat

cd %original_dir%

exit /B 1