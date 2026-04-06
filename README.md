# 🎮 Brumas – Jogo Sério sobre Brumadinho

## 📌 Visão Geral

Este projeto consiste no desenvolvimento de um **jogo sério com foco narrativo**, baseado em eventos inspirados na tragédia de Brumadinho. A proposta é criar uma experiência interativa que permita ao jogador acompanhar a rotina de um personagem e tomar decisões ao longo do dia, até o momento do rompimento da barragem.

O objetivo principal **não é o entretenimento tradicional**, mas sim a **conscientização, reflexão e conexão emocional** com a história, utilizando a interatividade como ferramenta de engajamento.

---

## 🎯 Objetivo do Projeto

Desenvolver uma experiência acessível e envolvente que:

- Promova **empatia** através da vivência da rotina de um personagem
- Reforce a importância da **memória coletiva**
- Explore o **impacto de decisões** em contextos cotidianos
- Seja **acessível** a diferentes faixas etárias, incluindo pessoas mais velhas
- Sirva como **base para futuras expansões** com outras histórias

---

## 👥 Público-Alvo

- População de Brumadinho
- Comunidades próximas a áreas de mineração
- Público geral interessado em narrativas interativas
- Pessoas com pouca familiaridade com jogos (interface simples e acessível)

---

## 🧠 Conceito de Gameplay

O jogo é baseado em uma **narrativa não linear**, no estilo **"escolha seu caminho"**.

O jogador acompanha a rotina de **Benício**, tomando decisões em diferentes momentos do dia. Cada escolha leva a diferentes caminhos e eventos, criando múltiplas possibilidades dentro da mesma história.

A experiência é guiada por:

- **Texto narrativo**
- **Áudio** (narração e ambientação)
- **Representações visuais** (cenários 2D ou 3D low poly)

---

## 🔁 Loop de Jogabilidade

1. Apresentação de um trecho da história (texto + áudio + visual)
2. Interação do jogador através de escolhas
3. Transição para o próximo evento com base na decisão
4. Atualização do cenário conforme o progresso da narrativa
5. Repetição do ciclo até o desfecho

---

## ⚙️ Mecânicas Principais

### Essenciais (Build 1)
- Navegação por **toque/clique** (mobile-first)
- Sistema de **escolhas** (ramificações narrativas — cada escolha leva ao texto correto)
- Botão para **avançar** e **voltar** na história
- Menu principal funcional (Iniciar, Sair)

### Futuras (a partir da Build 2)
- Botão para **reler** o trecho atual
- **Histórico de decisões** tomadas
- **Transições entre cenas** com efeitos visuais
- Sistema de **seleção de personagem** (inicialmente apenas Benício)
- Interações com elementos do cenário (ex: clicar em objetos)
- Sistema de áudio e narração (a ser decidido pela equipe)

---

## 🖥️ Interface e Experiência

A interface foi pensada para ser:

- **Simples e intuitiva**
- **Legível** (tipografia clara e adequada)
- **Acessível** para diferentes idades
- **Responsiva** para dispositivos móveis

### Elementos principais:

- Menu inicial (Iniciar, Configurações, Sair)
- Tela de seleção de história/personagem
- Área principal de narrativa
- Botões de decisão
- Sistema de histórico

---

## 🎨 Direção de Arte

O projeto utiliza um estilo **low poly**, com foco em:

- Simplicidade visual
- Clareza na leitura do ambiente
- Performance em dispositivos mobile

O cenário funciona como uma **representação visual da narrativa**, sendo modificado dinamicamente conforme a história avança, reforçando o contexto de cada momento.

### Identidade Visual

- Paleta com destaque para tons de **amarelo**
- Integração entre UI e cenário
- Uso de formas simples e cores sólidas

### Paleta de Cores (Sugestão Inicial – a ser definida pela equipe)

> ⚠️ **Nota:** Esta paleta é uma **sugestão inicial**. A paleta oficial deve ser pesquisada e definida pela equipe (Antonio e Laura) antes de ser aplicada em qualquer asset ou interface. Após a definição, atualizar esta seção e o GDR.

| Cor | Hex | Uso sugerido |
|-----|-----|-----|
| 🟡 Amarelo Sol | `#F2C94C` | Cor primária / destaque |
| 🟠 Amarelo Escuro | `#D4A017` | Cor secundária / ênfase |
| 🟤 Marrom Terra | `#8B6914` | Apoio / elementos naturais |
| 🟢 Verde Musgo | `#556B2F` | Complementar / vegetação |
| ⚪ Cinza Concreto | `#808080` | Neutro / fundos |

---

## 📎 Links e Recursos Importantes

> Preencher conforme o projeto avança. Manter sempre atualizado.

| Recurso | Link / Informação |
|---------|-------------------|
| 🎨 **Paleta de Cores Oficial** | *(a ser definida pela equipe – ver sugestão acima)* |
| 📄 **GDR (Game Design Document)** | *(inserir link aqui)* |
| 📌 **Moodboard Pinterest** | *(inserir link da pasta aqui)* |
| 💻 **Repositório GitHub** | [github.com/AndreHP950/Brumas](https://github.com/AndreHP950/Brumas) |
| 🎮 **Projeto Unity** | *(inserir caminho ou instruções aqui)* |
| 🎨 **Referências Visuais** | *(inserir link ou pasta aqui)* |
| 📝 **Roteiro do Benício** | *(inserir link ou localização do arquivo)* |
| 📊 **Planilha de Produção** | [`Planilha_Producao_Brumas.xlsx`](./Planilha_Producao_Brumas.xlsx) |

---

## 🧱 Estrutura Técnica

### Ferramentas Utilizadas

| Ferramenta | Uso |
|------------|-----|
| **Unity** | Desenvolvimento principal (engine) |
| **Blender** | Modelagem 3D (low poly) |
| **GIMP** | Criação de texturas e UI |
| **GitHub** | Versionamento de código |
| **Pinterest** | Moodboard e referências visuais |

---

## 🧩 Estrutura do Sistema

O jogo é dividido em módulos principais:

```
📦 Brumas
├── 🎮 Sistema de Navegação (menus e transições)
├── 📖 Sistema de Narrativa (estrutura de dados da história)
├── 🔀 Sistema de Escolhas (ramificações)
├── 🖼️ Sistema de UI (interface e interação)
├── 🔊 Sistema de Áudio (narração e efeitos)
└── 🏔️ Sistema de Cenário (representação visual dinâmica)
```

---

## 📖 Narrativa

A história principal acompanha o personagem **Benício**, explorando sua rotina antes do evento crítico.

A narrativa é construída de forma fragmentada, permitindo:

- Diferentes caminhos
- Releitura de trechos
- Exploração de decisões e consequências


---

## 🧪 Escopo da Primeira Build (Entrega 19/04)

Para a primeira entrega, o foco é no **essencial funcional**. Não precisamos de assets finais, efeitos visuais elaborados, áudio ou cenários detalhados neste momento.

### Obrigatório:
- [ ] GDR atualizado com referências, paleta de cores definida e direção de arte
- [ ] Interface básica funcional (menu → narrativa → escolhas)
- [ ] Sistema de narrativa funcionando (exibir texto, avançar, voltar)
- [ ] Sistema de escolhas funcionando (cada escolha leva ao trecho correto)
- [ ] Build executável funcional (APK)

### Desejável (se der tempo):
- [ ] Blocagem simples de um cenário (formas geométricas com cores da paleta)
- [ ] Moodboard no Pinterest com referências visuais

### NÃO é prioridade para Build 1:
- ❌ Modelagem 3D detalhada de cenários
- ❌ Iluminação, câmera e materiais refinados
- ❌ Efeitos sonoros e música
- ❌ Narração em áudio
- ❌ Transições visuais elaboradas
- ❌ Sistema de histórico de decisões
- ❌ Tela de seleção de personagem
- ❌ Material de apresentação (fica para depois da build pronta)

---

## 📅 Cronograma de Entregas

| Entrega | Data | Pontos |
|---------|------|--------|
| **Build Inicial** | 19/04 | 15 pts |
| **Build Intermediária** | 17/05 | 15 pts |
| **Build Pré-banca** | 14/06 | 15 pts |
| **Build Final para Banca** | 28/06 | 50 pts |

> A última build deve estar 100% completa para o final de junho. Não há necessidade de pressa — vamos fazer com calma e planejamento.

---

## 📋 Diretrizes para Organização de Tarefas

### Princípios importantes:

1. **Respeitar dependências:** Não pedir para alguém implementar algo que depende de outra tarefa não concluída. Exemplo: não pedir para programar a interface de escolhas antes de um artista ter definido como ela deve ficar.

2. **Máximo 1-3 tarefas por pessoa por semana:** Todos estudam, trabalham e têm outras responsabilidades. Tarefas devem ser realistas e alcançáveis.

3. **Dividir tarefas pesadas:** Modelagem 3D de cenários completos, por exemplo, deve ser dividida entre mais pessoas e mais tempo. Nunca alocar como tarefa única de uma semana.

4. **Definir antes de usar:** Paleta de cores, estilo visual, layout de interface — tudo isso precisa ser **definido e aprovado** pela equipe antes de ser implementado em código ou assets.

5. **A história já existe:** O roteiro do Benício já está pronto em formato de texto. O trabalho agora é transformá-lo em formato jogável, não reescrever.

6. **Focar no funcional:** Para as primeiras builds, o importante é que funcione. Visual bonito, efeitos e polimento vêm depois.

7. **Nem todos precisam ter a mesma quantidade de tarefas:** Uma pessoa pode ter 3 tarefas simples enquanto outra tem 1 tarefa mais complexa. O importante é o equilíbrio de esforço.

---

## 📊 Planilha de Produção

A planilha de planejamento da produção está disponível no arquivo [`Planilha_Producao_Brumas.xlsx`](./Planilha_Producao_Brumas.xlsx).

### Como usar a planilha:

1. **Abra o arquivo** no Excel ou Google Sheets
2. **Aplique cores** nas células para indicar status:
   - 🟢 **Verde** = Concluído
   - 🟡 **Amarelo** = Em andamento
   - 🔴 **Vermelho** = Não iniciado
3. **A linha de tarefa** contém a descrição detalhada do que precisa ser feito
4. **A linha abaixo** (em branco) é onde o membro preenche o que fez, como fez e o link do commit
5. Siga a **linha de exemplo** no topo da planilha para entender o formato

### Cronograma Resumido (Ciclo da Build 1)

| Semana | Período | Foco |
|--------|---------|------|
| **Semana 1** | 05/04 – 11/04 | Planejamento + estrutura base (definir paleta, referências, wireframes da interface) |
| **Semana 2** | 12/04 – 18/04 | **FOCO TOTAL** – Implementar narrativa + escolhas, integrar e gerar build |
| **Semana 3** | 19/04 – 25/04 | Pós-entrega + correções com base no feedback |
| **Semana 4** | 26/04 – 02/05 | Planejamento do próximo ciclo (Build 2) |

---

## 🔄 Metodologia de Desenvolvimento

O desenvolvimento está organizado em **ciclos semanais**, com:

- Divisão clara de tarefas por integrante
- Uso de **commits obrigatórios** no GitHub
- Documentação contínua do progresso
- Validação com instituição parceira a cada build

Cada tarefa deve conter:

- ✏️ Linha de tarefa com descrição detalhada do que precisa ser feito
- 🔧 Linha em branco para o membro preencher o que fez, como fez
- 🔗 Link do commit correspondente

---

## 👨‍💻 Equipe

| Integrante | Responsabilidades |
|------------|-------------------|
| **André Henriques** | Organização geral, programação de mecânicas, UI e SFX |
| **Antonio Araujo** | Modelagem 3D, VFX, SFX, level design, documentação (GDR) |
| **Arthur Doffémond** | Programação de mecânicas, UI e VFX |
| **Laura Alairce** | Narrativa, artes 2D, UI e level design |

> **OBS:** Todos podem colaborar em todas as áreas, mas as responsabilidades acima são as prioridades de cada integrante.

---

## 📌 Considerações Finais

Este projeto busca utilizar o desenvolvimento de jogos como **ferramenta de impacto social**, indo além do entretenimento e explorando o potencial da interatividade para gerar **reflexão e conscientização**.

A estrutura foi pensada para permitir **expansão futura**, com inclusão de novas histórias, personagens e contextos.

---

## 📂 Estrutura do Repositório

```
Brumas/
├── README.md                        # Este documento
├── Planilha_Producao_Brumas.xlsx    # Planilha de planejamento da produção
├── .gitignore                       # Configuração de arquivos ignorados (Unity)
└── Assets/                          # Pasta principal do projeto Unity (a ser criada)
```

---

## 🚀 Como Executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/AndreHP950/Brumas.git
   ```
2. Abra o projeto na **Unity**
3. Abra a cena principal em `Assets/Scenes/`
4. Pressione **Play** para testar no editor

---

## 📄 Licença

Este projeto é desenvolvido para fins educacionais e de conscientização social.
