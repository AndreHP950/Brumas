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

- Navegação por **toque/clique** (mobile-first)
- Sistema de **escolhas** (ramificações narrativas)
- Botão para **avançar** e **voltar** na história
- Botão para **reler** o trecho atual
- **Histórico de decisões** tomadas
- **Transições entre cenas** com áudio e efeitos
- Sistema de **seleção de personagem** (inicialmente apenas Benício)

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

### Paleta de Cores

| Cor | Hex | Uso |
|-----|-----|-----|
| 🟡 Amarelo Sol | `#F2C94C` | Cor primária / destaque |
| 🟠 Amarelo Escuro | `#D4A017` | Cor secundária / ênfase |
| 🟤 Marrom Terra | `#8B6914` | Apoio / elementos naturais |
| 🟢 Verde Musgo | `#556B2F` | Complementar / vegetação |
| ⚪ Cinza Concreto | `#808080` | Neutro / fundos |

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

### Estrutura de Pastas Unity

```
Assets/
├── Scripts/
│   ├── Core/          # GameManager, SceneLoader
│   ├── UI/            # MenuController, NavigationUI
│   ├── Narrative/     # NarrativeManager, ChoiceSystem
│   └── Audio/         # AudioManager
├── Scenes/            # MainMenu, GameScene, SelectionScene
├── Prefabs/           # Prefabs reutilizáveis
├── Audio/
│   ├── SFX/           # Efeitos sonoros
│   └── Narration/     # Áudios de narração
├── UI/
│   └── Sprites/       # Ícones, botões, frames
├── Models/            # Modelos 3D low poly
├── Textures/          # Texturas
└── Materials/         # Materiais
```

---

## 📖 Narrativa

A história principal acompanha o personagem **Benício**, explorando sua rotina antes do evento crítico.

A narrativa é construída de forma fragmentada, permitindo:

- Diferentes caminhos
- Releitura de trechos
- Exploração de decisões e consequências

### Estrutura da Narrativa

| Ato | Momento | Descrição |
|-----|---------|-----------|
| **Ato 1** | Manhã | Acordar, café da manhã, saída de casa |
| **Ato 2** | Tarde | Trabalho, almoço, volta para casa |
| **Ato 3** | Evento | Sinais do rompimento, decisões críticas, desfecho |

---

## 🧪 Escopo da Primeira Build (Entrega 19/04)

Para a primeira entrega, o projeto deve conter:

- [x] GDR atualizado com referências e identidade visual
- [x] Interface básica totalmente funcional
- [x] Menu principal implementado
- [x] História completa do Benício navegável
- [x] Sistema de escolhas funcional
- [x] Pelo menos um cenário blocado (sandbox)
- [x] Mecânicas principais implementadas
- [x] Build executável funcional (APK)

---

## 📊 Planilha de Produção

A planilha de planejamento da produção está disponível no arquivo [`Planilha_Producao_Brumas.csv`](./Planilha_Producao_Brumas.csv).

### Como usar a planilha:

1. **Abra o arquivo CSV** no Excel ou Google Sheets
2. **Aplique cores** nas células para indicar status:
   - 🟢 **Verde** = Concluído
   - 🟡 **Amarelo** = Em andamento
   - 🔴 **Vermelho** = Não iniciado
3. **Preencha o link do commit** em cada descrição de tarefa
4. Siga a **linha de exemplo** no topo da planilha para entender o nível de detalhamento esperado

### Cronograma Resumido

| Semana | Período | Foco |
|--------|---------|------|
| **Semana 1** | 05/04 – 11/04 | Estrutura base + planejamento |
| **Semana 2** | 12/04 – 18/04 | **FOCO TOTAL** – Integração e build |
| **Semana 3** | 19/04 – 25/04 | Pós-entrega + melhorias |
| **Semana 4** | 26/04 – 02/05 | Polimento + próximo ciclo |

---

## 🔄 Metodologia de Desenvolvimento

O desenvolvimento está organizado em **ciclos semanais**, com:

- Divisão clara de tarefas por integrante
- Uso de **commits obrigatórios** no GitHub
- Documentação contínua do progresso
- Validação com instituição parceira a cada build

Cada tarefa deve conter:

- ✏️ Descrição do que foi feito
- 🔧 Detalhamento técnico (scripts, assets, etc.)
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
├── Planilha_Producao_Brumas.csv     # Planilha de planejamento da produção
├── .gitignore                       # Configuração de arquivos ignorados (Unity)
└── Assets/                          # Pasta principal do projeto Unity (a ser criada)
```

---

## 🚀 Como Executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/AndreHP950/Brumas.git
   ```
2. Abra o projeto no **Unity 2022.3 LTS**
3. Abra a cena `MainMenu` em `Assets/Scenes/`
4. Pressione **Play** para testar no editor
5. Para build Android: `File > Build Settings > Android > Build`

---

## 📄 Licença

Este projeto é desenvolvido para fins educacionais e de conscientização social.
