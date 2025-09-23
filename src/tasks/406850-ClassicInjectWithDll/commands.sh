
cd ../../..

cd src/tasks/406850-ClassicInjectWithDll

cd src/apps/406850-ClassicInjectWithDll

start .

code .

code . -r

dir

devenv /rebuild Debug ClassicInjectWithDll.sln

dir

dotnet run --project ./ClassicInjectWithDll.csproj

