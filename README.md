# BUILD

1. Go to ```Shared ``` folder then build project two times.<i>(first one to build interface, the second one to generate interface and controller for client)</i>

2. Go to ```Services``` folder

<i>If you do not have dotnet-ef CLI you can install it through this 
command</i>
```
dotnet tool install --global dotnet-ef
```
if you have dotnet-ef CLI you just need to run <i>command</i>
```
dotnet ef --startup-project ../Server migrations add Initial
```
3. build project
4. goto server and run command 
```
dotnet watch
```
