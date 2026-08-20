<!--
Relatório de Impacto da Sincronização
Alteração de versão: 1.0.0 -> 1.0.1
Princípios modificados:
- I. Training Scope Is Explicit -> I. O Escopo de Treinamento É Explícito
- II. Local-First Development -> II. Desenvolvimento Local em Primeiro Lugar
- III. Security Boundaries Are Demonstrated -> III. Limites de Segurança São Demonstrados
- IV. Infrastructure Is Abstracted -> IV. A Infraestrutura É Abstraída
- V. Spec-Driven, Focused Changes -> V. Mudanças Focadas e Guiadas por Especificação
Seções adicionadas:
- Nenhuma
Seções removidas:
- Nenhuma
TODOs de acompanhamento:
- Nenhum
-->
# Constituição do ContosoDashboard

## Princípios Fundamentais

### I. O Escopo de Treinamento É Explícito
O ContosoDashboard DEVE permanecer claramente identificado como uma aplicação de treinamento em
documentação, especificações, planos e orientações voltadas ao usuário quando o contexto de
implantação for relevante. Funcionalidades NÃO DEVEM declarar prontidão para produção, salvo quando
os controles necessários de produção forem implementados e documentados.

Racional: O repositório ensina Desenvolvimento Guiado por Especificação com padrões de aplicação
simplificados, autenticação simulada e limitações conhecidas.

### II. Desenvolvimento Local em Primeiro Lugar
Todos os fluxos principais DEVEM executar localmente, sem contas em nuvem, dependências de serviços
externos ou infraestrutura paga. Novas capacidades que possam usar serviços em nuvem DEVEM fornecer
primeiro uma implementação local para treinamento, com qualquer caminho de migração para nuvem
documentado separadamente.

Racional: A disponibilidade offline mantém o treinamento repetível e acessível, enquanto permite que
os participantes aprendam design orientado à migração.

### III. Limites de Segurança São Demonstrados
Autenticação, autorização, acesso baseado em papéis e verificações de propriedade no nível de serviço
DEVEM permanecer visíveis no design de funcionalidades protegidas. Qualquer comportamento de
segurança simulado ou intencionalmente incompleto DEVE ser documentado próximo à funcionalidade e
NÃO DEVE ser apresentado como adequado para uso em produção.

Racional: A aplicação ensina conceitos de segurança; portanto, os exemplos precisam ser claros,
delimitados e honestos sobre suas limitações.

### IV. A Infraestrutura É Abstraída
Acesso a banco de dados, armazenamento de arquivos, autenticação, notificações e outras preocupações
de infraestrutura DEVEM ser isolados por serviços, interfaces, configuração ou fronteiras de
framework já usadas pelo projeto. A lógica de negócio NÃO DEVE depender diretamente de
implementações específicas de nuvem.

Racional: A base de código demonstra uma implementação local com caminho futuro de migração para
Azure; portanto, trocas de infraestrutura não podem exigir reescrita da lógica das funcionalidades.

### V. Mudanças Focadas e Guiadas por Especificação
Trabalho material em funcionalidades DEVE começar com uma especificação aprovada do Spec Kit e seguir
pelo fluxo de plano e tarefas do projeto antes da implementação. Mudanças DEVEM permanecer no escopo
da funcionalidade, reutilizar os padrões existentes de Blazor Server, Entity Framework Core,
Bootstrap e camada de serviços, e evitar refatorações não relacionadas.

Racional: O repositório existe para ensinar Desenvolvimento Guiado por Especificação disciplinado, e
mudanças estreitas tornam os resultados de treinamento mais fáceis de revisar.

## Restrições Técnicas

A aplicação usa ASP.NET Core 8.0 com Blazor Server, Entity Framework Core, SQL Server LocalDB,
Bootstrap 5.3 e Bootstrap Icons, salvo quando uma especificação justificar explicitamente uma
alteração. Autenticação simulada é permitida apenas para fluxos de treinamento. Orientações voltadas
à produção DEVEM nomear os controles ausentes quando forem relevantes, incluindo provedores reais de
identidade, tratamento de senhas, MFA, limitação de taxa, trilhas de auditoria e gerenciamento de
sessão endurecido.

O acesso a dados DEVE preservar o isolamento de usuários e as expectativas de papéis demonstradas
pelos usuários semeados. Funcionalidades que introduzam persistência, uploads ou visibilidade entre
usuários DEVEM definir autorização e comportamento de falha na especificação antes da implementação.

## Fluxo de Desenvolvimento

Especificações DEVEM descrever valor para o usuário, expectativas por papel, cenários de aceitação,
casos de borda e limitações de treinamento. Planos DEVEM identificar serviços, modelos, páginas,
fluxo de dados e critérios de qualidade afetados. Tarefas DEVEM ser ordenadas por dependência e
pequenas o suficiente para revisão.

A implementação DEVE incluir validação adequada ao risco da mudança. Alterações sensíveis a
segurança, autorização, persistência de dados e fluxos entre páginas DEVEM incluir testes focados ou
um caminho de verificação documentado. A documentação DEVE ser atualizada quando comportamento,
configuração, limitações ou orientação de treinamento mudarem.

## Governança

Esta constituição prevalece sobre convenções conflitantes do projeto para trabalho com Spec Kit
neste repositório. Emendas DEVEM ser propostas com racional claro, impacto esperado e notas de
migração para especificações, planos, tarefas ou documentação afetados. A aprovação exige aceite
explícito de mantenedor ou responsável pelo treinamento antes que trabalhos dependentes tratem a
emenda como ativa.

O versionamento segue versionamento semântico. Versões MAJOR são exigidas para mudanças
incompatíveis de governança ou remoções de princípios. Versões MINOR são exigidas para novos
princípios, novas seções de governança ou orientações materialmente expandidas. Versões PATCH são
exigidas para esclarecimentos e ajustes de redação sem mudança semântica.

Toda especificação, plano, lista de tarefas e revisão DEVE verificar os princípios aplicáveis.
Qualquer exceção DEVE ser documentada com motivo, responsável e prazo de expiração ou caminho de
remediação.

**Versão**: 1.0.1 | **Ratificada em**: 2026-08-19 | **Última Emenda**: 2026-08-19
