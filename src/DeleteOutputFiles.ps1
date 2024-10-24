
$files = get-childitem -path . -file -recurse -include ('*.exe', '*.pdb', '*.ilk', '*.obj', '*.dll', '*.exp', '*.lib', '*.o', '*.pcm')
"Here are the file that will be deleted."
($files).fullname
""

"Performing delete."
$files | Remove-Item

""
"Delete done..."

""
"Deleting bin and obj folders..."
Get-ChildItem .\ -include bin, obj, Debug, Release, x64, gcm.cache -Recurse | ForEach-Object ($_) { Remove-Item $_.FullName -Force -Recurse }

""
"Done......."

