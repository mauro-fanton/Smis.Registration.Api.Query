#!/bin/bash



if [[ ${@} =~ (^| )"coverage"( |$) ]] 
then
  dotnet coverage collect dotnet test -f xml -o test-coverage/coverage.xml
  reportgenerator \
  -reports:"test-coverage/coverage.xml" \
  -targetdir:"test-coverage/coveragereport" \
  -reporttypes:Html \
  "-filefilters:-*Program*;-*Wrapper*;-*EventStoreDBClient*"
# -assemblyfilters:-EventStore.dll
else
  dotnet test
fi
