FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# copy sln, csproj and restore as distinct layers
COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

RUN dotnet restore
 
# copy everything else and build
COPY . .
RUN dotnet publish ./RegularApi/RegularApi.csproj -c Release -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

RUN useradd -ms /bin/bash dorefactor

WORKDIR /home/dorefactor/rd-api
COPY --from=build-env /app/RegularApi/out .

USER dorefactor

EXPOSE 5000
ENTRYPOINT ["dotnet", "RegularApi.dll"]
