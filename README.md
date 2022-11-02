# ArmDot String Deobfuscator
Before
```cs
object obj;
char[] array = (obj = new char[4]);
obj[0] = 441090186 ^ 441090247;
obj[1] = 253233999 ^ 253233966;
obj[2] = 1089525869 ^ 1089525764;
obj[3] = 699363612 ^ 699363698;
Console.WriteLine(new string(array));
```
After
```cs
Console.WriteLine("Main");
```
