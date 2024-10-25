# Process Enumeration

## How this project is created.
1. Create console app as in an earlier example.

2. Next, as the [documentation](https://learn.microsoft.com/en-us/windows/win32/api/wtsapi32/nf-wtsapi32-wtsenumerateprocessesexa) explains (see bottom of page) this function requires you to link with wtsapi32.lib. This library isn't linked with by default, you need to add it to the linker settings in your project.

![Props linker](Images/50_50_Props_Linker_Input.png)

Here is the edit.

![Props linker](Images/51_50_Props_Linker_Input_Edit.png)

3. This changes the proj file as follows.

```xml
<AdditionalDependencies>%(AdditionalDependencies)</AdditionalDependencies>
```

to 

```xml
<AdditionalDependencies>wtsapi32.lib;%(AdditionalDependencies)</AdditionalDependencies>
```

## Notes
1. The example is from here. 
2. https://learn.microsoft.com/en-us/windows/win32/toolhelp/taking-a-snapshot-and-viewing-processes

## References
1. https://learn.microsoft.com/en-us/windows/win32/toolhelp/taking-a-snapshot-and-viewing-processes
2. https://winterdom.com/dev/security/tokens/
3. https://learn.microsoft.com/en-us/windows/win32/api/wtsapi32/nf-wtsapi32-wtsenumerateprocessesexa



