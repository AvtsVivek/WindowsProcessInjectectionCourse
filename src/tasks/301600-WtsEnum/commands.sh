
cd ../../..

cd src/tasks/301600-WtsEnum

cd src/apps/301600-WtsEnum

start .

code .

code . -r

dir

# Command line compilation is not working. 
# Please use Visual Studio IDE to compile and run the code.

g++ --version

# -o flag means, compile as well as link.
g++ "-static" -o main.exe .\*.cpp

# Try the following, if the above does not work.
g++ "-static" -o main.exe .\*.cpp -std=c++20

dir

.\main.exe


