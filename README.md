# 🎧 MUSE
Transforme suas playlists de vídeos em coleções de músicas para ouvir onde e quando quiser.

## 🧠 Objetivo

Desenvolver uma ferramenta simples e eficiente em C# para **baixar o áudio de vídeos de uma playlist do YouTube**, convertendo-os automaticamente para o formato **.mp3**, com foco em uso pessoal e offline.

---

## Como usar

1. Crie uma chave de API no console cloud da google.Veja a documentação completa [aqui](docs/apikey.md)


## 💡 Motivação

Com os custos de plataformas de streaming cada vez mais altos, o acesso à música e conteúdo sonoro pode se tornar limitado, especialmente para jovens adultos em situações financeiras apertadas.

A proposta deste projeto é oferecer uma alternativa simples e prática para escutar músicas ou conteúdos favoritos offline, **sem infringir direitos autorais** (uso pessoal apenas, sem redistribuição).

---

## ✅ Requisitos do Projeto

### Funcionais
- [ ] Permitir que o usuário informe uma **URL de playlist do YouTube**.
- [ ] Obter todos os links de vídeos da playlist automaticamente usando a biblioteca `YoutubeExplode`.
- [ ] Baixar apenas o **áudio** de cada vídeo.
- [ ] Converter o áudio para o formato **MP3** utilizando `FFmpeg`.
- [ ] Salvar os arquivos com o nome do vídeo, de forma organizada.

### Não-funcionais
- [ ] Interface simples em linha de comando (CLI) para começar.
- [ ] Código modular, claro e fácil de manter.
- [ ] Uso de bibliotecas estáveis e bem documentadas.

---

## 🛠️ Tecnologias e Ferramentas

- **C# (.NET 6 ou superior)**
- **YoutubeExplode** – Biblioteca para acessar vídeos e playlists do YouTube.
- **FFmpeg** – Para converter os arquivos de áudio para `.mp3`.
- **Xabe.FFmpeg** (opcional) – Wrapper para usar FFmpeg no C# de forma simplificada.
- **YouTube Data API v3** (opcional) – Para listar vídeos de uma playlist, se desejar usar a API oficial.

---

## 🔒 Observações legais

Este projeto é voltado exclusivamente para **uso pessoal e educacional**. Não se destina à redistribuição de conteúdo protegido por direitos autorais. Respeite sempre os termos de uso do YouTube.

---
