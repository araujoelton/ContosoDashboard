# Modelo de Dados: Envio e Gerenciamento de Documentos

## Documento

Representa um arquivo de trabalho enviado por um usuário.

**Campos**:
- `DocumentId`: inteiro, chave primária.
- `Title`: texto obrigatório, máximo 255 caracteres.
- `Description`: texto opcional, máximo 2000 caracteres.
- `Category`: texto obrigatório, máximo 100 caracteres.
- `Tags`: texto opcional com tags normalizadas para busca simples.
- `OriginalFileName`: texto obrigatório, nome original exibido ao usuário.
- `StoredFileName`: texto obrigatório, nome físico baseado em GUID.
- `FilePath`: texto obrigatório, caminho relativo portátil fora de `wwwroot`.
- `FileSizeBytes`: número obrigatório, máximo 25 MB.
- `FileType`: texto obrigatório, máximo 255 caracteres para tipos MIME longos.
- `UploadedByUserId`: usuário que enviou o documento.
- `ProjectId`: projeto associado opcional.
- `TaskId`: tarefa associada opcional.
- `UploadedDate`: data/hora do envio.
- `UpdatedDate`: data/hora da última alteração de metadados ou substituição.
- `IsDeleted`: falso na versão inicial; exclusão funcional remove acesso e arquivo de forma
  permanente.

**Relacionamentos**:
- Muitos documentos pertencem a um usuário responsável pelo envio.
- Um documento pode pertencer a um projeto.
- Um documento pode estar associado a uma tarefa.
- Um documento pode ter vários compartilhamentos.
- Um documento pode ter várias atividades.

**Validações**:
- `Title` e `Category` são obrigatórios para cada arquivo.
- Cada arquivo deve ter no máximo 25 MB.
- Tipos permitidos: PDF, Microsoft Office, texto, JPEG e PNG.
- `FilePath` deve ser gerado pelo sistema, nunca derivado diretamente do nome informado pelo usuário.
- Documento de projeto só pode ser compartilhado com membros autorizados do mesmo projeto.

## Compartilhamento de Documento

Representa acesso explícito concedido a um usuário específico.

**Campos**:
- `DocumentShareId`: inteiro, chave primária.
- `DocumentId`: documento compartilhado.
- `SharedWithUserId`: usuário destinatário.
- `SharedByUserId`: usuário que realizou o compartilhamento.
- `SharedDate`: data/hora do compartilhamento.

**Relacionamentos**:
- Muitos compartilhamentos pertencem a um documento.
- Um compartilhamento aponta para exatamente um destinatário.
- Um compartilhamento registra o usuário que concedeu o acesso.

**Validações**:
- Não deve haver compartilhamento duplicado para o mesmo documento e destinatário.
- Compartilhamento por equipe não faz parte da versão inicial.
- Para documento de projeto, `SharedWithUserId` deve ser membro autorizado do projeto.

## Atividade de Documento

Representa um evento auditável relacionado a documento.

**Campos**:
- `DocumentActivityId`: inteiro, chave primária.
- `DocumentId`: documento relacionado.
- `UserId`: usuário que realizou a ação.
- `ActivityType`: texto obrigatório; valores esperados: `Upload`, `Download`, `Preview`,
  `MetadataUpdated`, `FileReplaced`, `Deleted`, `Shared`.
- `OccurredAt`: data/hora do evento.
- `Details`: texto opcional para contexto adicional.

**Relacionamentos**:
- Muitas atividades pertencem a um documento.
- Cada atividade pertence a um usuário.

**Validações**:
- Toda ação de envio, download, pré-visualização, edição, substituição, exclusão e compartilhamento
  deve gerar atividade.
- Atividades administrativas devem preservar o usuário que executou a ação.

## Categoria de Documento

Categoria é armazenada como texto para simplicidade e consistência com a especificação.

**Valores iniciais**:
- `Documentos de Projeto`
- `Recursos da Equipe`
- `Arquivos Pessoais`
- `Relatórios`
- `Apresentações`
- `Outros`

## Transições de Estado

```text
Selecionado -> Validado -> Verificado -> Armazenado -> Disponível
Selecionado -> Rejeitado
Disponível -> Metadados Atualizados
Disponível -> Arquivo Substituído
Disponível -> Compartilhado
Disponível -> Excluído
```

**Regras de transição**:
- Se validação de tipo, tamanho, título ou categoria falhar, o documento vai para `Rejeitado`.
- Se a verificação de segurança simulada falhar ou não concluir, o envio é bloqueado.
- Um documento só fica `Disponível` depois que arquivo e metadados forem salvos com sucesso.
- Exclusão remove acesso e conteúdo de forma permanente após confirmação.

## Índices Recomendados

- `UploadedByUserId, UploadedDate` para "Meus Documentos" e documentos recentes.
- `ProjectId, UploadedDate` para documentos do projeto.
- `Category` para filtros por categoria.
- `Title`, `Tags` e `Description` para busca simples por metadados.
- `DocumentId, SharedWithUserId` único em compartilhamentos.
- `UserId, OccurredAt` em atividades para relatórios.
