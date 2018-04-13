FROM microsoft/dotnet:2.1-sdk
COPY /deployment .
WORKDIR .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "Korrikalari.dll"]