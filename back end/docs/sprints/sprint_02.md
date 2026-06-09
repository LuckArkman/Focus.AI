# Sprint 02: Configuração do Docker Compose e Containers Base

## Objetivo da Sprint
Orquestrar a infraestrutura local necessária para o Focus.AI. Esta sprint não lidará com o modelo de IA em si, mas sim com a configuração dos serviços acessórios indispensáveis que rodarão via Docker: PostgreSQL, MongoDB e Qdrant. Isso garante que o backend possa ser desenvolvido e testado de forma imutável em qualquer máquina.

## Tarefas de Desenvolvimento

### 1. Criação do arquivo docker-compose.yml
- Criar na pasta raiz do repositório `back end` um arquivo `docker-compose.yml`.
- Configurar as redes isoladas para garantir que apenas o Backend comunique com os bancos (ex: rede interna e externa).

### 2. Configuração do PostgreSQL (Relacional)
- Definir o serviço `postgres` utilizando a imagem `postgres:16-alpine` (para maior leveza).
- Expor a porta `5432`.
- Configurar variáveis de ambiente cruciais: `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB` (ex: `focus_ai_db`).
- Criar o mapeamento de volume persistente local (ex: `./.docker/data/pg_data:/var/lib/postgresql/data`) para não perder contas ao reiniciar o container.

### 3. Configuração do MongoDB (NoSQL)
- Definir o serviço `mongodb` utilizando a imagem oficial `mongo:7.0`.
- Expor a porta `27017`.
- Variáveis de ambiente: `MONGO_INITDB_ROOT_USERNAME`, `MONGO_INITDB_ROOT_PASSWORD`.
- Mapeamento de volume: `./.docker/data/mongo_data:/data/db`.

### 4. Configuração do Qdrant (Vetorial)
- Definir o serviço `qdrant` utilizando a imagem `qdrant/qdrant:latest`.
- Expor as portas `6333` (REST API) e `6334` (gRPC - essencial para alta performance em .NET).
- Mapeamento de volume: `./.docker/data/qdrant_data:/qdrant/storage`.

### 5. Configuração do Dockerfile da Aplicação (Opcional nesta fase)
- Desenvolver o `Dockerfile` multi-stage build para o projeto .NET 8 da API.
- Adicioná-lo ao docker-compose, passando as *connection strings* do Postgres, Mongo e Qdrant como environment variables, vinculando-as pelo nome dos hosts internos gerados pelo compose.

## Testes e Validações
- **Teste de Conectividade:** Executar `docker compose up -d` e verificar via `docker ps` se todos os três serviços de banco subiram sem erros.
- **Teste de Acesso Externo:** Usar um cliente como *DBeaver* ou *MongoDB Compass* para tentar conectar ao Postgres e ao Mongo locais e confirmar a persistência.
- **Teste do Qdrant:** Fazer um simples GET no endpoint do Qdrant (`http://localhost:6333`) e esperar um JSON de status "ok".

## Critérios de Aceite (DoD)
1. Rodar um único comando (`docker compose up`) sobe todo o ambiente necessário para a aplicação funcionar.
2. Os dados de todos os bancos sobrevivem à exclusão dos containers (persistem nos volumes locais).
3. Conexões entre containers pela rede interna do docker funcionam.
