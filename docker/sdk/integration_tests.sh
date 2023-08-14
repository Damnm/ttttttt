#!/usr/bin/env bash

chmod +x wait_for_db.sh
./wait_for_db.sh postgreSQL:5432 -t 120

echo "Waiting for the schema to be published to the database before running the tests..." 
sleep 60s

for proj in src/IntegrationTests/*.IntegrationTests;
do
  PROJ_NAME=$( echo "${proj}"|cut -d '/' -f 2 )
  echo "Run ${PROJ_NAME}";

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
    "-targetdir:/code/coverage/coverage-report/integration-tests-coverage" \
    "-reporttypes:Html"