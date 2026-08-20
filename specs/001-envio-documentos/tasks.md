# Tarefas: Envio e Gerenciamento de Documentos

**Entrada**: Documentos de desenho em `specs/001-envio-documentos/`
**Pré-requisitos**: [plan.md](./plan.md), [spec.md](./spec.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/](./contracts/)

**Testes**: A especificação não solicita TDD formal. As tarefas abaixo incluem validação manual pelo
[quickstart.md](./quickstart.md) e deixam testes automatizados para uma solicitação posterior.

**Organização**: As tarefas são agrupadas por história de usuário para permitir implementação e
validação independentes.

## Formato: `[ID] [P?] [História] Descrição`

- **[P]**: Pode rodar em paralelo porque altera arquivos diferentes e não depende de tarefa incompleta
- **[História]**: História de usuário associada, como [US1], [US2], [US3], [US4]
- Todas as tarefas incluem caminhos exatos de arquivos

## Fase 1: Setup (Infraestrutura Compartilhada)

**Objetivo**: Preparar configuração e estrutura local de armazenamento.

- [X] T001 Criar marcador de diretório de uploads local em `ContosoDashboard/AppData/uploads/.gitkeep`
- [X] T002 Configurar caminho base de armazenamento e limite de 25 MB em `ContosoDashboard/appsettings.json`
- [X] T003 [P] Criar arquivo de categorias permitidas em `ContosoDashboard/Models/DocumentCategory.cs`
- [X] T004 [P] Criar pasta de controllers para arquivos protegidos em `ContosoDashboard/Controllers/.gitkeep`

---

## Fase 2: Fundação (Pré-requisitos Bloqueantes)

**Objetivo**: Criar entidades, persistência, serviços base e registro de dependências usados por todas as histórias.

**CRÍTICO**: Nenhuma história de usuário deve começar antes desta fase estar concluída.

- [ ] T005 [P] Criar entidade Documento com validações em `ContosoDashboard/Models/Document.cs`
- [ ] T006 [P] Criar entidade Compartilhamento de Documento em `ContosoDashboard/Models/DocumentShare.cs`
- [ ] T007 [P] Criar entidade Atividade de Documento em `ContosoDashboard/Models/DocumentActivity.cs`
- [ ] T008 Atualizar navegações de documentos em `ContosoDashboard/Models/User.cs`
- [ ] T009 Atualizar navegações de documentos em `ContosoDashboard/Models/Project.cs`
- [ ] T010 Atualizar navegações de documentos em `ContosoDashboard/Models/TaskItem.cs`
- [ ] T011 Registrar DbSets, relacionamentos e índices de documentos em `ContosoDashboard/Data/ApplicationDbContext.cs`
- [ ] T012 [P] Criar abstração e implementação local de armazenamento em `ContosoDashboard/Services/FileStorageService.cs`
- [ ] T013 [P] Criar serviço de verificação de segurança simulada em `ContosoDashboard/Services/DocumentSecurityScanService.cs`
- [ ] T014 Criar interface e esqueleto do serviço de documentos em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T015 Registrar serviços de documentos, armazenamento, verificação simulada e controllers em `ContosoDashboard/Program.cs`
- [ ] T016 Atualizar tipos de notificação para documentos em `ContosoDashboard/Models/Notification.cs`

**Checkpoint**: Fundação pronta; as histórias de usuário podem ser implementadas em ordem de prioridade.

---

## Fase 3: História de Usuário 1 - Enviar e classificar documentos (Prioridade: P1) MVP

**Objetivo**: Permitir que usuários autenticados enviem um ou mais documentos com título e categoria por arquivo.

**Teste independente**: Enviar arquivo válido, rejeitar arquivo inválido e confirmar que documentos salvos aparecem em "Meus Documentos".

### Implementação da História de Usuário 1

- [ ] T017 [US1] Implementar validação de tipo, tamanho, título e categoria em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T018 [US1] Implementar sequência validar-autorizar-gerar caminho-salvar arquivo-salvar metadados em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T019 [US1] Implementar bloqueio quando verificação simulada falhar ou não concluir em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T020 [US1] Implementar registro de atividade de envio em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T021 [US1] Criar página de documentos com formulário de envio em `ContosoDashboard/Pages/Documents.razor`
- [ ] T022 [US1] Implementar seleção múltipla com metadados por arquivo em `ContosoDashboard/Pages/Documents.razor`
- [ ] T023 [US1] Implementar mensagens de sucesso e erro do envio em `ContosoDashboard/Pages/Documents.razor`
- [ ] T024 [US1] Adicionar link de navegação para documentos em `ContosoDashboard/Shared/NavMenu.razor`
- [ ] T025 [US1] Validar cenários 1, 2 e 3 do quickstart em `specs/001-envio-documentos/quickstart.md`

**Checkpoint**: MVP funcional e validável de forma independente.

---

## Fase 4: História de Usuário 2 - Encontrar e acessar documentos permitidos (Prioridade: P2)

**Objetivo**: Permitir listagem, filtros, ordenação, busca, download e pré-visualização respeitando permissões.

**Teste independente**: Usuário acessa apenas documentos próprios, de projeto ou compartilhados; filtros, busca, download e pré-visualização funcionam.

### Implementação da História de Usuário 2

- [ ] T026 [US2] Implementar consulta de documentos acessíveis em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T027 [US2] Implementar filtros por categoria, projeto e intervalo de datas em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T028 [US2] Implementar ordenação por título, data, categoria e tamanho em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T029 [US2] Implementar busca por título, descrição, tags, responsável e projeto em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T030 [US2] Implementar listagem, filtros, ordenação e busca na página `ContosoDashboard/Pages/Documents.razor`
- [ ] T031 [US2] Criar endpoints protegidos de download e pré-visualização em `ContosoDashboard/Controllers/DocumentFilesController.cs`
- [ ] T032 [US2] Mapear controllers de arquivos protegidos em `ContosoDashboard/Program.cs`
- [ ] T033 [US2] Registrar atividades de download e pré-visualização em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T034 [US2] Adicionar seção de documentos do projeto em `ContosoDashboard/Pages/ProjectDetails.razor`
- [ ] T035 [US2] Validar cenários 4 e 7 do quickstart em `specs/001-envio-documentos/quickstart.md`

**Checkpoint**: Documentos acessíveis podem ser encontrados e abertos sem expor arquivos públicos.

---

## Fase 5: História de Usuário 3 - Compartilhar e receber documentos (Prioridade: P3)

**Objetivo**: Permitir compartilhamento com usuários específicos, notificação e seção "Compartilhados Comigo".

**Teste independente**: Proprietário compartilha com usuário permitido; destinatário recebe notificação e usuários não autorizados não acessam.

### Implementação da História de Usuário 3

- [ ] T036 [US3] Implementar criação de compartilhamento sem duplicidade em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T037 [US3] Bloquear compartilhamento por equipe e destinatários fora do projeto em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T038 [US3] Criar notificação de documento compartilhado em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T039 [US3] Adicionar ação de compartilhar com usuário específico em `ContosoDashboard/Pages/Documents.razor`
- [ ] T040 [US3] Adicionar seção "Compartilhados Comigo" em `ContosoDashboard/Pages/Documents.razor`
- [ ] T041 [US3] Validar cenários 5 e 6 do quickstart em `specs/001-envio-documentos/quickstart.md`

**Checkpoint**: Compartilhamento explícito funciona sem ampliar acesso de documentos de projeto.

---

## Fase 6: História de Usuário 4 - Gerenciar documentos e atividades (Prioridade: P4)

**Objetivo**: Permitir edição de metadados, substituição, exclusão permanente e relatórios administrativos.

**Teste independente**: Usuário autorizado gerencia documentos e administrador consulta atividades.

### Implementação da História de Usuário 4

- [ ] T042 [US4] Implementar edição de título, descrição, categoria e tags em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T043 [US4] Implementar substituição de arquivo mantendo validação e armazenamento seguro em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T044 [US4] Implementar exclusão permanente autorizada de metadados e arquivo em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T045 [US4] Registrar atividades de edição, substituição, exclusão e compartilhamento em `ContosoDashboard/Services/DocumentService.cs`
- [ ] T046 [US4] Adicionar ações de editar, substituir e excluir na página `ContosoDashboard/Pages/Documents.razor`
- [ ] T047 [US4] Criar página administrativa de relatórios em `ContosoDashboard/Pages/DocumentReports.razor`
- [ ] T048 [US4] Adicionar link administrativo de relatórios em `ContosoDashboard/Shared/NavMenu.razor`
- [ ] T049 [US4] Validar cenário 9 do quickstart em `specs/001-envio-documentos/quickstart.md`

**Checkpoint**: Governança de documentos e auditoria administrativa estão funcionais.

---

## Fase 7: Polimento e Integração Transversal

**Objetivo**: Integrar painel, notificações, documentação e validação final.

- [ ] T050 Atualizar resumo do painel com contagem de documentos acessíveis em `ContosoDashboard/Services/DashboardService.cs`
- [ ] T051 Atualizar modelo de resumo do painel com documentos recentes em `ContosoDashboard/Services/DashboardService.cs`
- [ ] T052 Atualizar cards e widget de documentos recentes no painel em `ContosoDashboard/Pages/Index.razor`
- [ ] T053 Revisar textos de limitação da verificação simulada em `ContosoDashboard/Pages/Documents.razor`
- [ ] T054 Validar todos os cenários do quickstart em `specs/001-envio-documentos/quickstart.md`
- [ ] T055 Executar build final em `ContosoDashboard/ContosoDashboard.csproj`

---

## Dependências e Ordem de Execução

### Dependências de Fase

- **Fase 1 Setup**: sem dependências.
- **Fase 2 Fundação**: depende da Fase 1 e bloqueia todas as histórias.
- **US1 MVP**: depende da Fase 2.
- **US2**: depende da Fase 2 e reutiliza documentos criados na US1 para validação.
- **US3**: depende da Fase 2 e fica mais fácil validar após US1/US2.
- **US4**: depende da Fase 2 e fica mais fácil validar após documentos existirem.
- **Polimento**: depende das histórias desejadas para entrega.

### Dependências por História

- **US1 (P1)**: primeira entrega útil; não depende de outras histórias.
- **US2 (P2)**: pode iniciar após Fundação, mas validação completa usa documentos enviados pela US1.
- **US3 (P3)**: pode iniciar após Fundação, mas validação completa usa documentos listáveis pela US2.
- **US4 (P4)**: pode iniciar após Fundação, mas validação completa usa documentos e atividades das histórias anteriores.

### Ordem Interna Recomendada

- Modelos antes de serviços.
- Serviços antes de páginas e controllers.
- Persistência e índices antes de consultas complexas.
- Validação e autorização antes de download, pré-visualização e compartilhamento.
- Quickstart após cada checkpoint de história.

## Oportunidades de Paralelismo

- T003 e T004 podem rodar em paralelo após T001.
- T005, T006, T007, T012 e T013 podem rodar em paralelo por alterarem arquivos diferentes.
- Após a Fundação, US2, US3 e US4 podem avançar parcialmente em paralelo se US1 fornecer dados de exemplo ou seed manual.
- T047 pode avançar em paralelo com T046 após T045.
- T050, T052 e T053 devem ser coordenadas, mas podem ser preparadas em paralelo se a interface de `DashboardService` estiver definida.

## Exemplo Paralelo: História de Usuário 1

```text
Task: "Implementar validação de tipo, tamanho, título e categoria em ContosoDashboard/Services/DocumentService.cs"
Task: "Criar página de documentos com formulário de envio em ContosoDashboard/Pages/Documents.razor"
Task: "Adicionar link de navegação para documentos em ContosoDashboard/Shared/NavMenu.razor"
```

## Exemplo Paralelo: Fundação

```text
Task: "Criar entidade Documento em ContosoDashboard/Models/Document.cs"
Task: "Criar entidade Compartilhamento de Documento em ContosoDashboard/Models/DocumentShare.cs"
Task: "Criar entidade Atividade de Documento em ContosoDashboard/Models/DocumentActivity.cs"
Task: "Criar abstração e implementação local de armazenamento em ContosoDashboard/Services/FileStorageService.cs"
```

## Estratégia de Implementação

### MVP Primeiro

1. Concluir Fase 1.
2. Concluir Fase 2.
3. Implementar US1.
4. Validar cenários 1, 2 e 3 do quickstart.
5. Demonstrar envio e listagem básica antes de avançar.

### Entrega Incremental

1. US1 entrega envio classificado.
2. US2 entrega localização e acesso seguro.
3. US3 entrega compartilhamento controlado.
4. US4 entrega manutenção e auditoria.
5. Polimento conecta painel, mensagens finais e validação completa.

## Observações

- Tarefas marcadas com [P] alteram arquivos diferentes e podem ser executadas em paralelo.
- Cada história tem checkpoint independente.
- Não servir arquivos diretamente de `wwwroot`.
- Não introduzir dependências de nuvem nesta versão.
- Documentar a verificação de malware simulada como treinamento, não produção.
