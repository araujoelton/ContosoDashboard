# Pesquisa: Envio e Gerenciamento de Documentos

## Decisão: Armazenar arquivos fora de `wwwroot`

**Racional**: Arquivos de usuário não devem ser servidos estaticamente sem autorização. Guardar o
conteúdo em `AppData/uploads` permite que download e pré-visualização passem por regras de acesso.

**Alternativas consideradas**:
- `wwwroot/uploads`: rejeitada porque permitiria acesso estático e contornaria autorização.
- Banco de dados para conteúdo binário: rejeitada por aumentar complexidade e não ser necessária para
  a escala de treinamento.

## Decisão: Gerar caminho único antes de salvar metadados

**Racional**: O caminho único baseado em GUID evita colisões, travessia de caminho e registros com
`FilePath` vazio. A sequência será validar, autorizar, gerar caminho, salvar arquivo e só então gravar
metadados.

**Alternativas consideradas**:
- Gravar metadados antes do arquivo: rejeitada por risco de registros órfãos.
- Usar nome original do arquivo no caminho físico: rejeitada por risco de colisão e segurança.

## Decisão: Usar `IFileStorageService`

**Racional**: A constituição exige infraestrutura abstraída. A interface permite implementação local
para treinamento e substituição futura por armazenamento em nuvem sem alterar páginas ou regras de
negócio.

**Alternativas consideradas**:
- Chamar `System.IO.File` diretamente no serviço de documentos: rejeitada por acoplar regra de
  negócio ao mecanismo de armazenamento.
- Introduzir provedor de nuvem agora: rejeitada por violar a exigência local/offline.

## Decisão: Verificação de malware simulada em treinamento

**Racional**: A feature precisa funcionar offline. A simulação preserva o ponto pedagógico e deve ser
documentada como não adequada para produção.

**Alternativas consideradas**:
- Motor real local obrigatório: rejeitada por adicionar dependência pesada e fora do escopo de
  treinamento.
- Sem verificação: rejeitada porque a especificação exige demonstração do fluxo de segurança.

## Decisão: Autorização centralizada no `DocumentService`

**Racional**: O projeto já usa serviços para prevenir IDOR. O serviço de documentos deve aplicar
permissões antes de listar, baixar, pré-visualizar, compartilhar, editar ou excluir documentos.

**Alternativas consideradas**:
- Autorizar apenas nas páginas: rejeitada porque chamadas futuras poderiam contornar a regra.
- Autorizar apenas por atributo de página: rejeitada porque não cobre propriedade de documento nem
  participação em projeto.

## Decisão: Compartilhamento apenas com usuários específicos

**Racional**: A clarificação removeu compartilhamento por equipe da versão inicial. Isso reduz o
modelo de permissão e evita ambiguidade com departamentos e membros de projeto.

**Alternativas consideradas**:
- Compartilhar com equipes/departamentos: rejeitada para a versão inicial.
- Permitir compartilhar documento de projeto fora do projeto: rejeitada por ampliar acesso além do
  limite de segurança do projeto.

## Decisão: Busca por metadados em banco de dados

**Racional**: A especificação exige busca por título, descrição, tags, responsável e projeto. Para a
versão inicial, buscar metadados no banco atende o objetivo sem indexação de conteúdo dos arquivos.

**Alternativas consideradas**:
- Indexar conteúdo completo dos arquivos: rejeitada por estar fora do escopo inicial.
- Buscar apenas por título: rejeitada por não atender os requisitos.

## Decisão: Contratos como fluxos de aplicação e operações de serviço

**Racional**: O projeto é uma aplicação Blazor Server, não uma API pública externa. O contrato útil
para planejamento é o comportamento esperado das telas, operações de serviço e endpoints internos
necessários para servir arquivos com autorização.

**Alternativas consideradas**:
- OpenAPI completo: rejeitado porque a feature é majoritariamente UI/serviço interno.
- Nenhum contrato: rejeitado porque download e pré-visualização exigem comportamento verificável.
