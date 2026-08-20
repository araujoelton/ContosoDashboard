# Funcionalidade de Upload e Gerenciamento de Documentos - Requisitos

## Visão Geral

A Contoso Corporation precisa adicionar capacidades de upload e gerenciamento de documentos ao
aplicativo ContosoDashboard. Essa funcionalidade permitirá que colaboradores enviem documentos de
trabalho, organizem-nos por categoria e projeto, e compartilhem-nos com membros da equipe.

## Necessidade de Negócio

Atualmente, os colaboradores da Contoso armazenam documentos de trabalho em vários locais
(unidades locais, anexos de e-mail, unidades compartilhadas), o que causa:

- Dificuldade para localizar documentos importantes quando necessário
- Riscos de segurança decorrentes do compartilhamento não controlado de documentos
- Falta de visibilidade sobre quais documentos estão associados a projetos ou tarefas específicos

A funcionalidade de upload e gerenciamento de documentos resolve esses problemas ao fornecer um
local centralizado e seguro para documentos de trabalho dentro do aplicativo de dashboard que os
colaboradores já usam diariamente.

## Usuários-Alvo

Todos os colaboradores da Contoso que usam o aplicativo ContosoDashboard terão acesso às
funcionalidades de gerenciamento de documentos, com permissões baseadas em seus papéis existentes:

- **Colaboradores**: Enviar documentos pessoais e documentos de projetos aos quais estão atribuídos
- **Líderes de Equipe**: Enviar documentos e visualizar/gerenciar documentos enviados por membros
  de sua equipe
- **Gerentes de Projeto**: Enviar documentos e gerenciar todos os documentos associados a seus
  projetos
- **Administradores**: Acesso completo a todos os documentos para fins de auditoria e conformidade

## Requisitos Principais

### 1. Upload de Documentos

**Seleção e Upload de Arquivos**

- Usuários devem poder selecionar um ou mais arquivos de seu computador para upload
- Tipos de arquivo compatíveis: PDF, documentos do Microsoft Office (Word, Excel, PowerPoint),
  arquivos de texto e imagens (JPEG, PNG)
- Tamanho máximo do arquivo: 25 MB por arquivo
- Usuários devem ver um indicador de progresso durante o upload
- O sistema deve exibir mensagens de sucesso ou erro após a conclusão do upload

**Metadados do Documento**

- Ao fazer upload, os usuários devem informar:
  - Título do documento (obrigatório)
  - Descrição (opcional)
  - Seleção de categoria a partir de lista predefinida (obrigatória): Documentos de Projeto,
    Recursos da Equipe, Arquivos Pessoais, Relatórios, Apresentações, Outros
  - Projeto associado (opcional, se o documento estiver relacionado a um projeto específico)
  - Tags para facilitar a busca (opcional; usuários podem adicionar tags personalizadas)
- O sistema deve capturar automaticamente:
  - Data e hora do upload
  - Enviado por (nome do usuário)
  - Tamanho do arquivo
  - Tipo do arquivo (tipo MIME, por exemplo, "application/pdf"; o campo deve acomodar
    255 caracteres para documentos Office)

**Validação e Segurança**

- O sistema deve verificar arquivos enviados contra vírus e malware antes do armazenamento
- O sistema deve rejeitar arquivos que excedam os limites de tamanho com mensagens de erro claras
- O sistema deve rejeitar tipos de arquivo não compatíveis
- Arquivos enviados devem ser armazenados com segurança e controles de acesso apropriados

**Notas de Implementação para Armazenamento Local de Arquivos**

**Padrão de Armazenamento Offline:**

- Armazenar arquivos em um diretório dedicado fora de `wwwroot` por segurança
  (por exemplo, `AppData/uploads`)
- Gerar caminhos de arquivo únicos ANTES da inserção no banco de dados para evitar violações de
  chave duplicada
- Padrão recomendado: `{userId}/{projectId ou "personal"}/{uniqueId}.{extension}`, em que
  `uniqueId` é um GUID
- **Sequência de upload: Gerar caminho único -> Salvar arquivo em disco -> Salvar metadados no banco**
- **Isso evita registros órfãos no banco de dados se o salvamento do arquivo falhar**
- **Isso evita erros de chave duplicada causados por caminhos vazios ou não únicos**

**Considerações de Segurança:**

- Arquivos armazenados fora de `wwwroot` exigem endpoints de controller para servi-los
  (permitindo verificações de autorização)
- Validar extensões de arquivo contra uma lista permitida antes de salvar
- Usar nomes de arquivo baseados em GUID para prevenir ataques de travessia de caminho
- Nunca usar nomes de arquivo fornecidos pelo usuário diretamente em caminhos de arquivo
- Implementar verificações de autorização no endpoint de download para impedir acesso não autorizado

**Design para Migração ao Azure:**

- Criar a interface `IFileStorageService` com os métodos: `UploadAsync()`, `DeleteAsync()`,
  `DownloadAsync()`, `GetUrlAsync()`
- A implementação local (`LocalFileStorageService`) usa operações de `System.IO.File`
- A futura implementação `AzureBlobStorageService` usará o SDK Azure.Storage.Blobs
- O mesmo padrão de caminho funciona para nomes de blobs no Azure:
  `{userId}/{projectId}/{guid}.{ext}`
- Trocar implementações via configuração de injeção de dependência
- Nenhuma mudança na lógica de negócio, interface de usuário ou esquema de banco de dados é
  necessária para a migração

### 2. Organização e Navegação de Documentos

**Visualização Meus Documentos**

- Usuários devem poder visualizar uma lista de todos os documentos que enviaram
- A visualização deve exibir: título do documento, categoria, data de upload, tamanho do arquivo e
  projeto associado
- Usuários devem poder ordenar documentos por: título, data de upload, categoria e tamanho do arquivo
- Usuários devem poder filtrar documentos por: categoria, projeto associado e intervalo de datas

**Visualização de Documentos do Projeto**

- Ao visualizar um projeto específico, usuários devem ver todos os documentos associados a esse
  projeto
- Todos os membros da equipe do projeto devem poder visualizar e baixar documentos do projeto
- Gerentes de Projeto devem poder enviar documentos para seus projetos

**Busca**

- Usuários devem poder buscar documentos por: título, descrição, tags, nome de quem enviou e projeto
  associado
- A busca deve retornar resultados em até 2 segundos
- Usuários devem ver nos resultados apenas documentos que tenham permissão para acessar

### 3. Acesso e Gerenciamento de Documentos

**Download e Pré-visualização**

- Usuários devem poder baixar qualquer documento ao qual tenham acesso
- Para tipos de arquivo comuns (PDF, imagens), usuários devem poder pré-visualizar documentos no
  navegador sem baixá-los

**Edição de Metadados**

- Usuários que enviaram um documento devem poder editar os metadados do documento
  (título, descrição, categoria, tags)
- Usuários devem poder substituir um arquivo de documento por uma versão atualizada

**Exclusão de Documentos**

- Usuários devem poder excluir documentos que enviaram
- Gerentes de Projeto podem excluir qualquer documento em seus projetos
- Documentos excluídos devem ser removidos permanentemente após confirmação do usuário

**Compartilhamento de Documentos**

- Proprietários de documentos devem poder compartilhar documentos com usuários ou equipes específicos
- Usuários que receberem documentos compartilhados devem ser notificados por notificação no aplicativo
- Documentos compartilhados devem aparecer na seção "Compartilhados Comigo" dos destinatários

### 4. Integração com Funcionalidades Existentes

**Integração com Tarefas**

- Ao visualizar uma tarefa, usuários devem poder ver e anexar documentos relacionados
- Usuários devem poder enviar um documento diretamente a partir de uma página de detalhes da tarefa
- Documentos anexados a tarefas devem ser automaticamente associados ao projeto da tarefa

**Integração com o Dashboard**

- Adicionar um widget "Documentos Recentes" à página inicial do dashboard mostrando os últimos
  5 documentos enviados pelo usuário
- Adicionar contagem de documentos aos cards de resumo do dashboard

**Notificações**

- Usuários devem receber notificações quando alguém compartilhar um documento com eles
- Usuários devem receber notificações quando um novo documento for adicionado a um de seus projetos

### 5. Requisitos de Performance

- O upload de documentos deve ser concluído em até 30 segundos para arquivos de até 25 MB
  (em uma rede típica)
- Páginas de lista de documentos devem carregar em até 2 segundos para até 500 documentos
- A busca de documentos deve retornar resultados em até 2 segundos
- A pré-visualização de documentos deve carregar em até 3 segundos

### 6. Relatórios e Auditoria

**Rastreamento de Atividades**

- O sistema deve registrar todas as atividades relacionadas a documentos: uploads, downloads,
  exclusões e ações de compartilhamento
- Administradores devem poder gerar relatórios mostrando:
  - Tipos de documento mais enviados
  - Usuários que mais fazem upload
  - Padrões de acesso a documentos

## Objetivos de Experiência do Usuário

- **Simplicidade**: Enviar um documento deve exigir no máximo 3 cliques
- **Velocidade**: Operações comuns (upload, download, busca) devem parecer instantâneas
- **Clareza**: Usuários devem sempre saber o que acontece com os arquivos enviados
- **Confiança**: Usuários devem confiar que seus documentos estão seguros e não serão perdidos

## Métricas de Sucesso

A funcionalidade será considerada bem-sucedida se, em até 3 meses após o lançamento:

- 70% dos usuários ativos do dashboard tiverem enviado pelo menos um documento
- O tempo médio para localizar um documento for reduzido para menos de 30 segundos
- 90% dos documentos enviados estiverem corretamente categorizados
- Ocorrerem zero incidentes de segurança relacionados ao acesso a documentos

## Restrições Técnicas

- Deve funcionar **offline, sem serviços em nuvem**, para fins de treinamento
- Deve usar **armazenamento no sistema de arquivos local** para documentos enviados
- Deve implementar **abstrações por interface** (`IFileStorageService`) para futura migração
  para nuvem
- Deve funcionar dentro da arquitetura atual da aplicação (sem grandes reescritas)
- Deve estar em conformidade com o sistema atual de autenticação simulada
- Cronograma de desenvolvimento: a funcionalidade deve estar pronta para produção em 8 a 10 semanas
- **Banco de dados: DocumentId deve ser inteiro (não GUID) para consistência com as chaves
  existentes de User/Project**
- **Banco de dados: Category deve armazenar valores de texto (não enum inteiro) para simplicidade**

## Abordagem de Implementação

A funcionalidade de gerenciamento de documentos é construída usando uma **arquitetura em camadas**
que separa responsabilidades e permite futura migração para a nuvem:

**Camada de Dados:**

- A entidade Document armazena metadados (título, categoria, nome do arquivo, caminho do arquivo,
  data de upload, usuário que enviou)
- DocumentId usa chaves inteiras (consistentes com as tabelas User e Project existentes)
- Category armazena valores de texto ("Documentos de Projeto", "Arquivos Pessoais" etc.) para
  simplicidade
- O campo FileType acomoda tipos MIME longos (255 caracteres para documentos Office)
- FilePath acomoda nomes de arquivo baseados em GUID por segurança (prevenindo ataques de travessia
  de caminho)
- A entidade DocumentShare rastreia relações de compartilhamento entre usuários

**Camada de Armazenamento:**

- Arquivos são armazenados fora de diretórios acessíveis pela web (requisito de segurança)
- A interface IFileStorageService abstrai a implementação de armazenamento
- LocalFileStorageService para treinamento (usa sistema de arquivos local)
- Futuro: trocar para AzureBlobStorageService em produção (sem necessidade de alterações de código)
- Organização de arquivos: `{userId}/{projectId ou "personal"}/{guid}.{extension}`

**Camada de Lógica de Negócio:**

- DocumentService orquestra o fluxo de upload:
  1. Validar arquivo (limite de tamanho, lista permitida de extensões)
  2. Autorizar usuário (participação no projeto, se o upload for para um projeto)
  3. Gerar nome de arquivo único baseado em GUID
  4. Salvar arquivo em disco
  5. Criar registro no banco de dados com caminho do arquivo
  6. Enviar notificações aos membros do projeto
- Verificações de autorização impedem acesso não autorizado a documentos (proteção contra IDOR)
- A camada de serviços aplica todas as regras de segurança antes do acesso a dados

**Camada de Apresentação:**

- Página Blazor Server para upload e visualização de documentos
- Upload de arquivos usa o padrão MemoryStream (evita problemas de descarte no Blazor)
- Tabela responsiva exibe os documentos do usuário com metadados
- Modal de upload valida entradas antes do envio

Essa arquitetura garante segurança, manutenibilidade e prontidão para nuvem, mantendo a
implementação de treinamento simples e capaz de funcionar offline.

### Prontidão para Migração para a Nuvem

Embora essa funcionalidade deva funcionar offline para treinamento, ela deve ser projetada para
facilitar a migração para serviços Azure:

**Requisitos da Implementação Offline:**

- Armazenar arquivos em estrutura de diretórios local
  (por exemplo, `AppData/uploads/{userId}/{projectId}/{guid}.ext`)
- Implementar `LocalFileStorageService : IFileStorageService` usando operações de `System.IO`
- Caminhos de arquivo armazenados no banco de dados devem ser relativos e portáveis
- Nenhuma dependência de SDK do Azure na implementação de treinamento

**Padrão de Design para Migração ao Azure:**

```csharp
// Abstração por interface (implementar na versão de treinamento)
public interface IFileStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    Task DeleteAsync(string filePath);
    Task<Stream> DownloadAsync(string filePath);
    Task<string> GetUrlAsync(string filePath, TimeSpan expiration);
}

// Treinamento: implementação LocalFileStorageService
// Produção: implementação AzureBlobStorageService
// Alternar via appsettings.json e injeção de dependência
```

**Benefícios da Migração:**

- Trocar a implementação do serviço sem alterar controllers, páginas ou lógica de negócio
- O esquema do banco de dados permanece inalterado (a coluna FilePath funciona tanto para caminhos
  locais quanto para nomes de blobs)
- Implantação orientada por configuração (desenvolvimento = local, produção = Azure)
- Participantes aprendem padrões de abstração usados na indústria

### Requisitos de Implementação Específicos do Blazor

**Gerenciamento de Estado do Componente de Upload de Arquivo**

- Usar o atributo `@key` no componente `InputFile` para forçar nova renderização após upload
  bem-sucedido
- Extrair metadados do arquivo (nome, tamanho, contentType) para variáveis locais ANTES de abrir
  o stream
- Copiar o stream de `IBrowserFile` imediatamente para `MemoryStream` para evitar problemas de
  descarte
- Limpar a referência de `IBrowserFile` (definir como null) após copiar o stream para prevenir erros
  de reutilização
- Exemplo de padrão:

  ```csharp
  var fileName = SelectedFile.Name;
  var fileSize = SelectedFile.Size;
  var contentType = SelectedFile.ContentType;

  using var memoryStream = new MemoryStream();
  using (var fileStream = SelectedFile.OpenReadStream(maxFileSize))
  {
      await fileStream.CopyToAsync(memoryStream);
  }
  memoryStream.Position = 0;

  SelectedFile = null; // Limpar referência para prevenir reutilização
  StateHasChanged();
  ```

**Claims de Autenticação**

- Garantir que o fluxo de login inclua TODAS as claims necessárias: NameIdentifier, Name, Email,
  Role, Department
- A claim Department é necessária para autorização baseada em equipe no compartilhamento de
  documentos
- Claims ausentes causarão falhas de autorização nos métodos de DocumentService

### Requisitos de Configuração do Banco de Dados

**Estado Limpo para Testes:**

- Antes de testar upload de documentos pela primeira vez, garantir um estado limpo do banco de dados
- Se tentativas anteriores de upload falharam, excluir e recriar o banco para remover registros
  órfãos:

  ```powershell
  sqllocaldb stop mssqllocaldb
  sqllocaldb delete mssqllocaldb
  # O banco de dados será recriado automaticamente na próxima execução
  ```

- Registros órfãos com valores vazios em FilePath causarão violações de chave duplicada
- Para LocalDB: `dotnet ef database drop --force` também funciona se as ferramentas EF estiverem
  instaladas

## Premissas

- O ambiente de treinamento possui armazenamento em disco local disponível
- A maioria dos documentos terá menos de 10 MB
- Usuários conhecem conceitos básicos de gerenciamento de arquivos
- Armazenamento no sistema de arquivos local é aceitável para fins de treinamento
- A migração para Azure Blob Storage está planejada para implantação em produção
- Usuários podem trabalhar offline (nenhuma conexão com a internet é necessária para a
  funcionalidade principal)

## Fora do Escopo

As seguintes funcionalidades NÃO estão incluídas nesta versão inicial:

- Edição colaborativa de documentos em tempo real
- Histórico de versões e capacidade de rollback
- Fluxos avançados de documentos (processos de aprovação, roteamento de documentos)
- Integração com sistemas externos (SharePoint, OneDrive)
- Suporte a aplicativo móvel (a versão inicial é apenas web)
- Templates de documentos ou funcionalidades de geração de documentos
- Cotas de armazenamento e gerenciamento de cotas
- Exclusão lógica/lixeira com recuperação

Essas funcionalidades poderão ser consideradas para melhorias futuras com base no feedback dos
usuários e nas necessidades de negócio.

## Próximos Passos

Após aprovação, estes requisitos serão usados para criar especificações detalhadas usando a
metodologia de Desenvolvimento Guiado por Especificação com GitHub Spec Kit.
