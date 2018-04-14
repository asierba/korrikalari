deploy: publish
	docker build -t korrikalari . 
	docker tag korrikalari asierba/korrikalari 
	docker login -u $(DOCKER_USERNAME) -p $(DOCKER_PASSWORD)
	docker push asierba/korrikalari

publish: test
	cd src/Korrikalari && dotnet publish -o ../../deployment

test: build
	cd tests/Korrikalari.Tests && dotnet test

run: build
	cd src/Korrikalari && dotnet run

build: clean 
	cd src/Korrikalari && dotnet build

clean:
	cd src/Korrikalari && dotnet clean
	rm -rf deployment