# Quickstart: Validação de Envio e Gerenciamento de Documentos

## Pré-requisitos

- .NET 8 SDK instalado.
- SQL Server LocalDB disponível.
- Repositório na raiz `D:\repo\ContosoDashboard`.
- Feature planejada em `specs/001-envio-documentos`.

## Preparar ambiente

```powershell
cd D:\repo\ContosoDashboard\ContosoDashboard
dotnet build
dotnet run
```

Abrir o navegador em `http://localhost:5000` ou na URL exibida pelo `dotnet run`.

## Estado limpo opcional

Use apenas se for necessário reiniciar os dados locais de treinamento:

```powershell
sqllocaldb stop mssqllocaldb
sqllocaldb delete mssqllocaldb
```

Na próxima execução, o banco será recriado pela aplicação.

## Usuários de validação

- `camille.nicole@contoso.com`: Gerente de Projeto
- `floris.kregel@contoso.com`: Líder de Equipe e membro do projeto
- `ni.kang@contoso.com`: Colaborador e membro do projeto
- `admin@contoso.com`: Administrador

## Cenário 1: Enviar documento pessoal

1. Entrar como `ni.kang@contoso.com`.
2. Abrir a área de documentos.
3. Selecionar um PDF menor que 25 MB.
4. Informar título e categoria.
5. Confirmar envio.

**Resultado esperado**:
- mensagem de sucesso aparece
- documento aparece em "Meus Documentos"
- lista mostra título, categoria, data de envio, tamanho e ausência de projeto associado

## Cenário 2: Validar envio múltiplo

1. Entrar como usuário autenticado.
2. Selecionar dois arquivos válidos.
3. Preencher título e categoria apenas para um deles.
4. Tentar confirmar envio.

**Resultado esperado**:
- envio é bloqueado
- erro indica que cada arquivo precisa de título e categoria
- nenhum documento incompleto fica disponível

## Cenário 3: Rejeitar arquivo inválido

1. Selecionar arquivo maior que 25 MB ou tipo não permitido.
2. Tentar confirmar envio.

**Resultado esperado**:
- sistema rejeita o arquivo
- mensagem informa o motivo
- nenhuma atividade de documento disponível é criada para arquivo rejeitado

## Cenário 4: Documento de projeto

1. Entrar como `camille.nicole@contoso.com`.
2. Enviar documento associado ao projeto "ContosoDashboard Development".
3. Entrar como `ni.kang@contoso.com`.
4. Abrir documentos do projeto.

**Resultado esperado**:
- membro do projeto visualiza e baixa o documento
- usuários fora do projeto não visualizam o documento em listas, busca ou acesso direto

## Cenário 5: Compartilhar documento

1. Entrar como proprietário de um documento.
2. Compartilhar com um usuário específico permitido.
3. Entrar como destinatário.

**Resultado esperado**:
- destinatário recebe notificação no aplicativo
- documento aparece em "Compartilhados Comigo"
- usuários não destinatários não acessam o documento

## Cenário 6: Bloquear compartilhamento fora do projeto

1. Enviar documento associado a projeto.
2. Tentar compartilhar com usuário que não é membro do projeto.

**Resultado esperado**:
- compartilhamento é rejeitado
- documento não aparece para o usuário fora do projeto

## Cenário 7: Download e pré-visualização

1. Entrar como usuário com acesso a PDF ou imagem.
2. Solicitar pré-visualização.
3. Solicitar download.

**Resultado esperado**:
- pré-visualização abre no navegador para PDF ou imagem
- download retorna o arquivo correto
- ambos os eventos ficam registrados como atividades

## Cenário 8: Painel

1. Entrar como usuário com documentos próprios, documentos de projeto e documentos compartilhados.
2. Abrir o painel inicial.

**Resultado esperado**:
- widget mostra os 5 documentos mais recentes enviados pelo usuário
- card de contagem mostra todos os documentos acessíveis ao usuário

## Cenário 9: Relatórios administrativos

1. Entrar como `admin@contoso.com`.
2. Abrir relatórios de documentos.

**Resultado esperado**:
- administrador visualiza tipos de documento mais enviados
- administrador visualiza usuários mais ativos
- administrador visualiza padrões de acesso

## Critérios de aceite de planejamento

- Todos os cenários acima possuem resultado esperado verificável.
- Nenhum cenário exige serviço em nuvem ou internet.
- A verificação de malware é explicitamente simulada no ambiente de treinamento.
- Arquivos não são servidos diretamente de diretórios públicos.
