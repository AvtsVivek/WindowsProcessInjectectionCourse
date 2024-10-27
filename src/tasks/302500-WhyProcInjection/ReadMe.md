# Nt Why Process Injection

 
## Notes
1. Attackers want to be stealthy to avoid detection
2. Running an unrecognized executable is easily detected
3. Malicious code would rather hide inside a legitimate process
4. Injected payload maybe shellcode or a DLL
5. Most Windows systems today are 64-bit
6. However, 32-bit executables can run normally thanks to an interception layer called “Wow64” (Windows on Windows 64)
7. Injecting code into a process must take this into consideration
   1. A 32-bit DLL cannot be loaded by a 64-bit process and vice versa
   2. 32-bit shellcode is different from 64-bit shellcode
8. Several injection techniques follow
9. User mode only
10. Not an exhaustive list
11. Roughly ordered by increasing complexity





## References
1. 
2. 

