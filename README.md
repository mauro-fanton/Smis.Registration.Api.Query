## Description

This is an Api for retrieving the registration application details. This is the Query part model of the Command Query Responsability Segregation (SQRS) architecture model.


## Build

You can use the build task to build the application (or the smis.sh file).

1.  press 
```
command + p
```
2. in the Search field type

```
>task
```

3.  Select "Tasks Run task"
4. Slect the "dotnet:build" task


## smis.sh

This shell script allow to build, run test and run the application:

```
./smis.sh clean build test run [release|debug]
```

**clean:** clean the solution

**build:** build the project

**test:** run all the tests

**run:** run the project

**release|debug**: configuration. If this is missed it will use the debug configuration

