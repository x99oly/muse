# 🚀 Configuração da YouTube Data API v3 com Chave de API

Este documento descreve o processo completo para obter uma **chave de API do YouTube**, necessária para acessar dados como vídeos de playlists, canais e mais. Também explica como configurar a chave como uma variável de ambiente no Windows (PowerShell).

---

## 📌 1. Criar um projeto no Google Cloud

1. Acesse: [https://console.cloud.google.com/](https://console.cloud.google.com/)
2. No topo da tela, clique em **"Selecionar projeto"** e depois em **"Novo projeto"**.
3. Dê um nome ao projeto (ex: `YoutubeMp3Tool`) e clique em **"Criar"**.

---

## 🔧 2. Ativar a YouTube Data API v3

A criação da chave é feita na plataforma cloud.console da google, veja a documentação completa [aqui](docs/apikey.md)

---

## 🔑 3. Criar uma chave de API

1. Vá em **"APIs e Serviços" > "Credenciais"**.
2. Clique em **"Criar credenciais"** > **"Chave da API"**.
3. Sua chave será exibida. **Copie e guarde com segurança**.

---

## 🛡️ (Opcional) Restringir uso da chave

Ainda na tela de credenciais, você pode:

- Clicar em **"Editar"** na chave criada.
- Restringir por:
  - IPs (ex: IP da sua máquina local `10.0.0.137`),
  - Aplicações (ex: apps web),
  - APIs permitidas (recomenda-se restringir para "YouTube Data API v3").

---

## ⚙️ 4. Configurar a variável de ambiente no Windows

### Temporariamente (válida só na sessão atual do terminal):

Salvar a chave:
```powershell
$env:YOUTUBE_API_KEY = "sua-chave-aqui" 
```

Verificar a variável salva
```powershell
echo $env:YOUTUBE_API_KEY
```

