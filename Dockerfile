FROM microsoft/aspnetcore:1.1.2

EXPOSE 5000

WORKDIR /app

COPY ./src/EG.One.DotNetCoreTemplate.API/bin/Release/netcoreapp1.1/publish .

ENTRYPOINT ["dotnet", "EG.One.DotNetCoreTemplate.API.dll"]
