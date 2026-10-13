# --- Build : .NET 8 SDK -------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restauration d'abord (couche Docker mise en cache tant que les .csproj ne changent pas)
COPY global.json Directory.Build.props ./
COPY src/DGsys.UI/DGsys.UI.csproj src/DGsys.UI/
COPY src/DGsys.Web/DGsys.Web.csproj src/DGsys.Web/
RUN dotnet restore src/DGsys.Web/DGsys.Web.csproj

COPY src/DGsys.UI/ src/DGsys.UI/
COPY src/DGsys.Web/ src/DGsys.Web/
RUN dotnet publish src/DGsys.Web/DGsys.Web.csproj -c Release -o /app --no-restore

# --- Runtime : fichiers statiques servis par nginx ----------------------------
FROM nginx:1.27-alpine
COPY docker/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/wwwroot /usr/share/nginx/html
EXPOSE 80
