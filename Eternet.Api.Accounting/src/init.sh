#!/usr/bin/env bash
set -euo pipefail

echo "[DEBUG] $(date) - Iniciando instalación de .NET"
curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- \
  --channel 9.0 --install-dir "$HOME/.dotnet"

echo "[DEBUG] $(date) - Actualizando paquetes"
sudo apt-get update

echo "[DEBUG] $(date) - Instalando dependencias del sistema"
sudo apt-get install -y libicu-dev

echo "[DEBUG] $(date) - Configurando variables de entorno"
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$PATH:$DOTNET_ROOT"

SOLUTION=tests/Eternet.Accounting.Api.Tests.sln

echo "[DEBUG] $(date) - Restaurando solución: $SOLUTION"
dotnet restore "$SOLUTION" --runtime linux-x64 --verbosity minimal
dotnet build src/Eternet.Accounting.Api/Eternet.Accounting.Api.csproj --runtime linux-x64
dotnet build tests/Eternet.Accounting.Api.Tests/Eternet.Accounting.Api.Tests.csproj --runtime linux-x64
dotnet build tests/Eternet.Accounting.Contracts.Tests/Eternet.Accounting.Contracts.Tests.csproj --runtime linux-x64

echo "[DEBUG] $(date) - Persistiendo configuración de entorno"
echo 'export DOTNET_ROOT="$HOME/.dotnet"' >> ~/.profile
echo 'export PATH="$PATH:$HOME/.dotnet"'  >> ~/.profile
echo 'export DOTNET_NOLOGO=1'            >> ~/.profile
echo 'export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1' >> ~/.profile
echo 'export LD_LIBRARY_PATH="$(pwd)/tests/Eternet.Accounting.Api.Tests/bin/Debug/net9.0/firebird/linux-x64/V3/lib:${LD_LIBRARY_PATH:-}"' >> ~/.profile


echo "[DEBUG] $(date) - Información de instalación de .NET"
dotnet --info | head -n 20

echo "[DEBUG] $(date) - Proceso completado con éxito"
exit 0
