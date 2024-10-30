
cd ../../..

cd src/tasks/302550-RemoteThreadInjectionExOne

cd src/apps/302550-RemoteThreadInjectionExOne

start .

code .

code . -r

dir

devenv /rebuild Debug CreateRemoteThreadEx.sln

dir

cd x64/Debug/

dir

# Just to start from a clean slate, stop all of the existing note pad processes.
Stop-Process -Name "notepad"

# Start a note pad process
Start-Process notepad

# Get all the notepad processes running and get its id as array.
$processes = Get-Process -Name notepad | Select -ExpandProperty ID

# Get the length of the array, ensure its 1.
$processes.length

# Get the id. Ensure this by opening the process explorer and searching for notepad.
$processes[0]

.\CreateRemoteThread.exe $processes[0] C:\Trials\Ex\LearnWinProcInject\src\apps\302550-RemoteThreadInjectionExOne\x64\Debug\InjectedDll.dll

# Close all of the notepad processes, just clean up.
Stop-Process -Name "notepad"

cd ../../

