rm -rf deployment
cd src/Korrikalari
dotnet publish -o ../../deployment
cd ../..
docker build -t korrikalari .
docker tag korrikalari asierba/korrikalari
docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker push asierba/korrikalari