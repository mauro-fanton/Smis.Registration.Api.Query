#!/bin/sh


if [[ ${@} =~ (^| )"release"( |$) ]] 
then
  CONFIG=Release
else
  CONFIG=Debug
fi


if [[ "${@}" =~ (^| )"clean"( |$) ]] 
then
  dotnet clean $(pwd)/Smis.Registration.Api.Query/Smis.Registration.Api.Query.csproj -c $CONFIG
fi

if [[ ${@} =~ (^| )"build"( |$) ]] 
then
  dotnet build $(pwd)/Smis.Registration.Api.Query/Smis.Registration.Api.Query.csproj -c $CONFIG
fi

if [[ "${@}" =~ (^| )"test"( |$) ]] 
then
  dotnet test $(pwd)/Smis.Registration.Api.Query.sln -c $CONFIG
fi

if [[ "${@}" =~ (^| )"run"( |$) ]] 
then
  dotnet run --project $(pwd)/Smis.Registration.Api.Query/Smis.Registration.Api.Query.csproj -c $CONFIG
fi