
DB_HOST="agenda-mysql"
DB_PORT="3306"

echo "Aguardando o MySQL em $DB_HOST:$DB_PORT..."
until nc -z $DB_HOST $DB_PORT; do
  echo "MySQL ainda indisponível... esperando 5 segundos."
  sleep 5
done
echo "MySQL está pronto! Iniciando migrações..."

echo "Aplicando migrações (dotnet ef database update)..."
dotnet ef database update --project Agenda.Infrastructure.csproj --startup-project Agenda.Api.csproj

if [ $? -ne 0 ]; then
    echo "Falha ao aplicar as migrações! Encerrando."
    exit 1
fi

echo "Migrações aplicadas com sucesso. Iniciando a API..."
exec dotnet Agenda.Api.dll