#!/bin/bash

set -euo pipefail

echo "Start running unit tests";

for proj in src/UnitTests/*.UnitTests;
do
  PROJ_NAME=$( echo "${proj}"|cut -d '/' -f 2 )
  echo "Running unit test ${PROJ_NAME}";

  dotnet test $proj \
    --nologo \
    --logger "trx;LogFileName=/code/coverage/test-results/${PROJ_NAME}-testresults.trx" \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=\"opencover,teamcity,json\" \
    /p:CoverletOutput=/code/coverage/ \
    /p:MergeWith=/code/coverage/coverage.json \

done

/tools/reportgenerator \
    "-reports:/code/coverage/coverage.opencover.xml" \
    "-targetdir:/code/coverage/coverage-report/unit-tests-coverage" \
    "-reporttypes:Html"
