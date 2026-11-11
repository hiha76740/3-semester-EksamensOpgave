# Byg og publicér Blazor
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore "src/VidensVejen.Kiosk.Web/VidensVejen.Kiosk.Web.csproj"

RUN dotnet publish "src/VidensVejen.Kiosk.Web/VidensVejen.Kiosk.Web.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Servér de publicerede filer med Nginx
FROM nginx:stable-alpine AS final

COPY src/VidensVejen.Kiosk.Web/nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=build /app/publish/wwwroot/ /usr/share/nginx/html/

EXPOSE 80