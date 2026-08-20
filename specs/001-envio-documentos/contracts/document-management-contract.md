# Contrato: Gerenciamento de Documentos

Este contrato descreve o comportamento esperado das telas, serviços e endpoints internos da feature.
Ele não define uma API pública externa.

## Tela: Documentos

### Enviar documentos

**Entrada do usuário**:
- um ou mais arquivos permitidos
- título obrigatório por arquivo
- categoria obrigatória por arquivo
- descrição opcional
- projeto associado opcional
- tags opcionais

**Resultado esperado**:
- arquivos válidos são enviados e aparecem em "Meus Documentos"
- cada documento exibe título, categoria, data de envio, tamanho e projeto associado quando existir
- envios inválidos mostram erro claro e não criam documento disponível

**Regras**:
- cada arquivo deve ter no máximo 25 MB
- todos os arquivos de um envio múltiplo precisam de título e categoria próprios
- verificação de segurança simulada deve concluir antes de disponibilizar o documento
- falha ou indisponibilidade da verificação bloqueia o envio

### Listar, filtrar e ordenar

**Filtros**:
- categoria
- projeto associado
- intervalo de datas

**Ordenações**:
- título
- data de envio
- categoria
- tamanho

**Resultado esperado**:
- o usuário vê somente documentos aos quais tem acesso
- listas de até 500 documentos ficam disponíveis em até 2 segundos

### Buscar

**Campos pesquisáveis**:
- título
- descrição
- tags
- responsável pelo envio
- projeto associado

**Resultado esperado**:
- busca retorna em até 2 segundos
- resultados respeitam as permissões do usuário atual

## Operações de Serviço

### EnviarDocumento

**Entrada**:
- usuário solicitante
- arquivo
- metadados obrigatórios e opcionais

**Saída**:
- documento criado quando válido
- erro de validação, autorização ou verificação quando inválido

**Regras de autorização**:
- usuário deve estar autenticado
- se houver projeto associado, usuário deve ser gerente ou membro autorizado do projeto

### ObterDocumentosAcessiveis

**Entrada**:
- usuário solicitante
- filtros e ordenação opcionais

**Saída**:
- documentos enviados pelo usuário
- documentos de projetos acessíveis ao usuário
- documentos compartilhados explicitamente com o usuário

### CompartilharDocumento

**Entrada**:
- usuário solicitante
- documento
- usuário destinatário

**Saída**:
- compartilhamento criado e notificação enviada
- erro quando destinatário não é permitido

**Regras**:
- compartilhamento por equipe não é permitido na versão inicial
- documento de projeto só pode ser compartilhado com membro autorizado do mesmo projeto

### ExcluirDocumento

**Entrada**:
- usuário solicitante
- documento
- confirmação explícita

**Saída**:
- documento removido permanentemente quando autorizado
- erro quando confirmação ou autorização estiver ausente

**Regras de autorização**:
- usuário que enviou pode excluir o próprio documento
- gerente do projeto pode excluir documentos do projeto gerenciado

## Endpoints Internos de Arquivo

### Download

**Comportamento esperado**:
- recebe identificador de documento
- valida permissão no serviço de documentos
- registra atividade de download
- retorna arquivo quando autorizado
- nega acesso quando o usuário não possui permissão

### Pré-visualização

**Comportamento esperado**:
- recebe identificador de documento
- valida permissão no serviço de documentos
- permite pré-visualização para PDF e imagens
- registra atividade de pré-visualização
- informa quando pré-visualização não está disponível para o tipo de arquivo

## Notificações

**Eventos que geram notificação**:
- documento compartilhado com usuário específico
- novo documento adicionado a projeto do qual o usuário participa

**Resultado esperado**:
- notificação aparece no centro de notificações do destinatário
- documentos compartilhados aparecem em "Compartilhados Comigo"

## Relatórios Administrativos

**Consultas esperadas**:
- tipos de documento mais enviados
- usuários que mais enviam documentos
- padrões de acesso a documentos

**Regras**:
- apenas administradores podem consultar relatórios globais
- as atividades devem preservar usuário, ação, documento e data/hora
