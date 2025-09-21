
cd ../../..

cd src/tasks/405750-UnmanagedThreadFuncPointer

cd src/apps/405750-UnmanagedThreadFuncPointer

start .

code .

code . -r

dir

devenv /rebuild Debug UnmanagedThreadFuncPointer.sln

dir

dotnet run --project ./UnmanagedThreadFuncPointer.csproj

