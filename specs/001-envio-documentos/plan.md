# Plano de Implementação: Envio e Gerenciamento de Documentos

**Ramo**: `[001-envio-documentos]` | **Data**: 2026-08-20 | **Spec**: [spec.md](./spec.md)
**Entrada**: Especificação da funcionalidade em `specs/001-envio-documentos/spec.md`

## Resumo

Adicionar gerenciamento de documentos ao ContosoDashboard para que usuários autenticados possam
enviar, classificar, localizar, baixar, pré-visualizar, compartilhar com usuários específicos e
auditar documentos de trabalho. A implementação seguirá a arquitetura atual do Blazor Server com EF
Core, serviços de domínio e autorização por usuário/projeto, usando armazenamento local fora de
`wwwroot` e abstração de armazenamento para futura migração para nuvem.

## Contexto Técnico

**Linguagem/Versão**: C# com .NET 8.0  
**Dependências Principais**: ASP.NET Core 8.0, Blazor Server, Razor Pages, Entity Framework Core
8.0, SQL Server LocalDB, autenticação por cookie, Bootstrap 5.3 e Bootstrap Icons  
**Armazenamento**: SQL Server LocalDB para metadados; sistema de arquivos local fora de `wwwroot`
para conteúdo dos arquivos  
**Testes**: Validação manual guiada por `quickstart.md`; testes automatizados focados devem ser
adicionados na fase de implementação para serviços, autorização e fluxos críticos  
**Plataforma Alvo**: Aplicação web local/offline para treinamento  
**Tipo de Projeto**: Aplicação web ASP.NET Core Blazor Server em projeto único  
**Metas de Performance**: envio de arquivo até 25 MB em até 30 segundos; listas até 500 documentos
em até 2 segundos; busca em até 2 segundos; pré-visualização em até 3 segundos  
**Restrições**: funcionar sem nuvem e sem internet; verificação de malware simulada e documentada;
arquivos não podem ser servidos diretamente de `wwwroot`; acesso deve respeitar usuário, projeto e
compartilhamento explícito  
**Escala/Escopo**: versão inicial para usuários e projetos já existentes, documentos até 25 MB por
arquivo, sem cotas, sem histórico de versões e sem compartilhamento por equipe

## Verificação da Constituição

*GATE: Deve passar antes da pesquisa da Fase 0. Reavaliado após o desenho da Fase 1.*

- **Escopo de treinamento explícito**: PASSA. A funcionalidade é planejada para ambiente local/offline
  e documenta a verificação de malware como simulada, não adequada para produção.
- **Desenvolvimento local em primeiro lugar**: PASSA. O desenho usa LocalDB e sistema de arquivos
  local, sem serviços externos obrigatórios.
- **Limites de segurança demonstrados**: PASSA. O plano inclui autorização por usuário/projeto,
  bloqueio de acesso indevido, arquivos fora de `wwwroot` e controle de download/pré-visualização.
- **Infraestrutura abstraída**: PASSA. O armazenamento de arquivos será isolado por
  `IFileStorageService`, mantendo a lógica de negócio independente do provedor.
- **Mudanças focadas e guiadas por especificação**: PASSA. O escopo fica restrito a modelos,
  serviços, páginas, contexto de dados, configuração e documentação da funcionalidade.

## Estrutura do Projeto

### Documentação desta feature

```text
specs/001-envio-documentos/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── document-management-contract.md
└── tasks.md
```

### Código-fonte esperado

```text
ContosoDashboard/
├── Data/
│   └── ApplicationDbContext.cs
├── Models/
│   ├── Document.cs
│   ├── DocumentActivity.cs
│   └── DocumentShare.cs
├── Pages/
│   ├── Documents.razor
│   └── ProjectDetails.razor
├── Services/
│   ├── DocumentService.cs
│   ├── FileStorageService.cs
│   └── DashboardService.cs
├── Program.cs
└── appsettings.json
```

**Decisão de Estrutura**: manter projeto único Blazor Server. Novas entidades entram em `Models`,
persistência e índices em `ApplicationDbContext`, regras e autorização em `Services`, experiência de
usuário em `Pages`, registro de serviços em `Program.cs` e configuração de armazenamento em
`appsettings.json`.

## Fase 0: Pesquisa

Artefato gerado: [research.md](./research.md)

Decisões resolvidas:
- armazenamento local seguro fora de `wwwroot`
- abstração `IFileStorageService`
- sequência de envio sem registros órfãos
- autorização por propriedade, projeto e compartilhamento
- verificação de malware simulada para treinamento
- busca, filtros e métricas de painel

## Fase 1: Desenho e Contratos

Artefatos gerados:
- [data-model.md](./data-model.md)
- [contracts/document-management-contract.md](./contracts/document-management-contract.md)
- [quickstart.md](./quickstart.md)

### Reavaliação da Constituição pós-desenho

- **Escopo de treinamento explícito**: PASSA. `research.md` e `quickstart.md` mantêm a limitação da
  verificação simulada.
- **Desenvolvimento local em primeiro lugar**: PASSA. O desenho depende apenas de LocalDB e disco
  local.
- **Limites de segurança demonstrados**: PASSA. O contrato documenta bloqueios de acesso, regras de
  compartilhamento e endpoints controlados para arquivo.
- **Infraestrutura abstraída**: PASSA. O modelo separa metadados de conteúdo e o contrato usa serviço
  de armazenamento.
- **Mudanças focadas e guiadas por especificação**: PASSA. Não há necessidade de reescrita ampla ou
  mudança de framework.

## Rastreamento de Complexidade

Nenhuma violação constitucional identificada. Não há complexidade adicional que exija justificativa.
