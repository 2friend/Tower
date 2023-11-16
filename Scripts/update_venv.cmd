@echo off

cd ..

git config core.hooksPath .githooks

REM Creating Virtual Env
python -m venv .venv

REM Activate Virtual Env
call .venv\Scripts\activate

REM Install Req
pip install -r requirements.txt

call .venv\Scripts\deactivate
