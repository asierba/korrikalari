rm -rf deployment
cd src/Korrikalari
dotnet publish -o ../../deployment
cd ../..
docker build -t korrikalari .
docker tag korrikalari asierba/korrikalari
docker push asierba/korrikalari