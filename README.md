# Documentação do Desafio Backend C#

## Visão Geral

Este projeto implementa dois microsserviços utilizando .NET C# e arquitetura limpa para gerenciar intenções de pagamento. As tecnologias utilizadas incluem Amazon SQS para fila de mensagens e MongoDB Atlas para persistência de dados.

---

## Configuração de Variáveis de Ambiente

### Serviço de Intenção de Pagamento
No serviço de intenção de pagamento, configure as seguintes variáveis de ambiente:

- `QueueSettings:QueueUrl` = `<URL_DA_FILA>`
- `GrpcSettings:PaymentServiceUrl` = `<URL_DO_SERVIÇO_GRPC>`
- `AWS:Region` = `<REGIÃO_AWS>`
- `AWS:Profile` = `<PERFIL_AWS>`

### Serviço de Processamento de Pagamento
No serviço de processamento de pagamento, configure as seguintes variáveis de ambiente:

- `QueueSettings:QueueUrl` = `<URL_DA_FILA>`
- `DatabaseSettings:DatabaseName` = `<NOME_DO_BANCO_DE_DADOS>`
- `DatabaseSettings:ConnectionString` = `<STRING_DE_CONEXÃO_DO_MONGODB>`
- `AWS:Region` = `<REGIÃO_AWS>`
- `AWS:Profile` = `<PERFIL_AWS>`

---

## Arquitetura

- **PaymentIntentService (Intenção de Pagamento):**
  - Recebe intenções de pagamento via uma API REST.
  - Gera um UUID único para cada intenção.
  - Envia os dados validados para uma fila no Amazon SQS.
  - Retorna o UUID como resposta.

- **PaymentProcessorService (Processamento de Pagamento):**
  - Consome mensagens da fila do Amazon SQS.
  - Persiste intenções de pagamento no MongoDB Atlas após sucesso no processamento.
  - Fornece um endpoint gRPC para consultar o status do pagamento por UUID.

---

## Tecnologias Utilizadas

- **Amazon SQS**: Para envio e consumo de mensagens entre os serviços.
- **MongoDB Atlas**: Para persistência de dados do PaymentProcessorService.
- **gRPC**: Para comunicação entre os microsserviços.
- **NUnit**: Para testes unitários.
- **ASP.NET Core**: Para construção das APIs.

---

## Padrões de Projeto

- **DTO (Data Transfer Object)**: Utilizado para transferir dados entre os serviços.
- **Repository Pattern**: Implementado para acesso ao MongoDB Atlas.

---

## Testes Unitários

Os testes foram implementados utilizando o NUnit 3.

---

## Como Executar

1. Configure as variáveis de ambiente conforme descrito acima.
2. Inicie o PaymentIntentService para receber intenções de pagamento.
3. Inicie o PaymentProcessorService para processar mensagens da fila e persistir os dados.
4. Use o Postman ou outro cliente HTTP para enviar intenções de pagamento ao PaymentIntentService .
5. Consulte o status do pagamento no PaymentProcessorService utilizando gRPC.
