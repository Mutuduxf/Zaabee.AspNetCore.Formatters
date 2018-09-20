FROM microsoft/dotnet:2.1-aspnetcore-runtime
COPY . /publish
WORKDIR /publish
ENTRYPOINT ["dotnet", "Demo.dll"]