# Process Enumeration

## Notes
1. Why Enumerate Processe?

2. We will look at several functions or libraries in Windows Api which help enumerate processes. 

   1. The tool help functions

   2. The Windows Terminal Services WTS functions.

   3. The EnumProcesses Function

   4. Using NtQuerySystemInformation

   5.  

3. Why do we want to enumerate processes?
   1. To search for pirticular process, 
      1. having desired attributes.
      2. Targeting specific process(es)(e.g. Explorer.exe)
   2. Getting the layout of the land. What are process running around.
   3. Checking if running within a container.
   4. 
   5. 
4. Ways to enumerate proceses.
   1. Toolhelp funtions family of functions.
      1. Were introduced in Windows 2000
      2. Provide a convenient vway to enumerate processes and threads in the entire system.
      3. Also proide functions to enumerate modules and heaps in a specified proceses.
      4. They dont need any special previlages. We can run with standard user rights and still get this type of information.
      5. They also help enumerate threads within a system.
      6. They are as follows.
         1. CreateToolhelp32Snapshot, Process32First, Process32Next.
         2. 
   2. 
5. 

## References



