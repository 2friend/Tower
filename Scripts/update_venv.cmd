@echo off

cd ..

git config core.hooksPath .githooks

REM Creating Virtual Env
python -m venv .venv

REM Activate Virtual Env
call .venv\Scripts\activate

REM Install Req
pip install -r requirements.txt

REM Update Submodules
git submodule update --init --recursive

call .venv\Scripts\deactivate
