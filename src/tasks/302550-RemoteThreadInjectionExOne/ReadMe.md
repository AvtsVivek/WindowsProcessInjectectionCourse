# Remote Thread Injection

 
## Notes
1. Create a thread in the target process and instruct that thread to load a desired DLL. And that dll is going to be called, and we can do what ever we want.
2. This is one of the oldest, if not the oldest. This is not used these days, because its easily detectable by anti malvare solutions.
3. So here is the target process and attacker process. The attacker process is created using some social engineering, attacment, script etc.

4. The attacker process opens a handle to the target process. This enables us to do some stuff in that process. Then write the path to the dll into the target process, using the `WriteProcessMemory` function.

![Attacker and Target Process](Images/50_50_AttacketAndTargetProcesses.png)

5. Next the attacker will call the `CreateRemoteThread` function. This will create thread in the target process and tell the thread to start running, where the function `LoadLibrary` is.

![Create Remote Thread](Images/51_50_CreateThreadOnTargetProcesses.png)

6. The `LoadLibrary` function is the one that can load the dll. This function will be given the path to the dll written in the target process. Our dll is going to be loaded into memory. 

![Load Library](Images/52_50_LoadLibOnTargetProcesses.png)

7. Then we can remove the attcker process form existance. Our dll is hiding inside the target process. 





## References
1. 
2. 

