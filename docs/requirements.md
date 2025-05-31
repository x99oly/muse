# 🎵 Projeto: YouTube Playlist Downloader

## ✅ Requisitos Originais

### Funcionais
- [x] Permitir que o usuário informe uma **URL de playlist do YouTube**.
- [x] Obter todos os links de vídeos da playlist automaticamente.
- [x] Permitir baixar apenas **uma música específica** via link de vídeo.
- [x] Baixar apenas o **áudio** de cada vídeo.
- [x] Converter o áudio para o formato **MP3**.
- [x] Salvar os arquivos com o **nome do vídeo**, de forma organizada.

### Não-funcionais
- [x] Interface simples em **linha de comando (CLI)**.
- [x] Código **modular, claro e fácil de manter**.
- [x] Uso de **bibliotecas estáveis e bem documentadas**.

---

## ➕ Requisitos Adicionais

### Entrada e Configuração
- [ ] Permitir que o usuário informe a **chave da API do YouTube**.
- [ ] Suporte a **paginação completa** (mais de 50 músicas da playlist).
- [ ] Permitir baixar apenas um **intervalo de músicas** (ex: da 3 à 10).
- [ ] Aceitar argumentos via CLI (`--playlist`, `--apikey`, `--range`, etc).

### Download e Conversão
- [x] Ignorar músicas que **já foram baixadas** (verificação por nome do arquivo).
- [ ] Permitir escolha de **formato de saída** (`.mp3`, `.ogg`, `.wav`, etc).
- [ ] Permitir configuração da **qualidade do áudio**.
- [ ] Opção para sobrescrever ou não arquivos já existentes.

### Organização
- [ ] Criar **subpastas por playlist** ou por **canal** automaticamente.
- [ ] Gerar um **relatório (JSON ou CSV)** com os dados das músicas baixadas.
- [ ] Permitir configurar **diretório de destino**.

---

## 🧰 Requisitos Técnicos

- [x] Usar `YoutubeExplode` para busca e download dos vídeos.
- [x] Usar `FFmpeg` para conversão de áudio.
- [ ] Logs para observabilidade da aplicação.
- [ ] Ter **testes unitários** nas partes críticas.
- [-] Adotar boas práticas de organização de pastas e separação de responsabilidades.

---

## 🔒 Requisitos de Segurança

- [ ] Não expor ou armazenar a **chave da API** diretamente no código.
- [ ] Validar URLs e strings externas para evitar injeções ou falhas.

---

## 🔮 Possíveis melhorias futuras

- [ ] Interface gráfica (GUI) básica para controle visual.
- [ ] Watcher de playlists (baixar novas músicas automaticamente).
- [ ] Suporte a **outros serviços** de música (SoundCloud, Spotify).
- [ ] Exportar/Importar configurações.

