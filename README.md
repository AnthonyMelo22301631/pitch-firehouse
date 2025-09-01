# Pitch – FireHouse (Eventos)

Aplicação web para criação, listagem e gerenciamento de **eventos**. O projeto segue **POO**, usa **arquitetura MVC** (Models + Razor Pages atuando como Views/Controllers) e implementa o **padrão Repository** com **Entity Framework Core + MySQL**.

**Repositório:** `https://github.com/AnthonyMelo22301631/pitch-firehouse`

---

## Integrantes
- Anthony Marcelo Mendoza de Melo – 22301631
- Daniel Ramos Nadalin Vaz da Costa – 22301739
- João Pedro de Freitas Carvalho – 22300953
- Gabriel Cédric Damázio Carvalho – 22301640
- Pedro do Nascimento Scarabelli – 12300187

> Se o repositório estiver privado, adicione **gleisonbt** em *Settings → Collaborators*.

---

## Arquitetura e Organização
- **POO** nas entidades `Evento`, `Comentario`, `Users`.
- **MVC** com Razor Pages:  
  - **Model:** `Pitch/Models/`  
  - **View/Controller:** `Pitch/Pages/*.cshtml(.cs)` (handlers `OnGet/OnPost`)  
- **Repository:** `Pitch/Repositories/` (`IEventoRepository/EventoRepository`, `IUserRepository/UserRepository`).  
- **Persistência:** `Pitch/Data/AppDbContext.cs` + **Migrations** (`Pitch/Migrations/`).

### Estrutura (resumo)
```
/
├─ Pitch.sln
├─ README.md
├─ appsettings.Development.example.json   # <- arquivo de EXEMPLO (raiz do repo)
└─ Pitch/
   ├─ Pitch.csproj
   ├─ Program.cs
   ├─ appsettings.json
   ├─ Data/
   ├─ Models/
   ├─ Repositories/
   ├─ Pages/
   ├─ wwwroot/
   ├─ Migrations/
   └─ Properties/
```

---

## Funcionalidades (10/10)
1. Cadastro, Login e Logout (Identity).  
2. Home (landing).  
3. Perfil do usuário.  
4. Criar evento.  
5. Listar eventos gerais.  
6. Meus eventos (apenas do logado).  
7. Detalhes do evento.  
8. Editar evento (somente criador).  
9. Cancelar evento (soft delete: `Cancelado = true`).  
10. Comentários no evento (criar e listar).

---

## Como Rodar (passos curtos)

### Pré-requisitos
- **.NET SDK 7+** (ou 8)  
- **MySQL 8** (ou compatível)  
- EF CLI:
  ```bash
  dotnet tool update -g dotnet-ef
  ```

### 1) Clonar e entrar no projeto
```bash
git clone https://github.com/AnthonyMelo22301631/pitch-firehouse.git
cd pitch-firehouse
```

### 2) Configurar a connection string (um arquivo só)
- Copie **este arquivo de exemplo** que está na raiz (`appsettings.Development.example.json`) para **`Pitch/appsettings.Development.json`** e edite **User Id**/**Password** do seu MySQL:

Windows (Explorer ou PowerShell):
```powershell
Copy-Item .\appsettings.Development.example.json .\Pitch\appsettings.Development.json
```
Linux/macOS:
```bash
cp appsettings.Development.example.json Pitch/appsettings.Development.json
```

Edite o arquivo **`Pitch/appsettings.Development.json`**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=pitchtest;User Id=SEU_USUARIO;Password=SUA_SENHA;TreatTinyAsBoolean=true;"
  }
}
```

> O `Pitch/appsettings.Development.json` **sobrescreve** a connection do `Pitch/appsettings.json` somente em DEV.

### 3) Criar o banco via migrations
```bash
cd Pitch
dotnet ef database update
```

### 4) Rodar
```bash
dotnet run
```
Abra **http://localhost:5000** (ou **https://localhost:5001**).  
Cadastre um usuário pela UI e use o sistema.

---

## Vídeo da demonstração
> Cole aqui o link do vídeo (YouTube “não listado”).

---

## Troubleshooting rápido
- **`Access denied for user`** → usuário/senha incorretos em `Pitch/appsettings.Development.json`.  
- **`Unknown column 'e.Cancelado'`** ou **tabela Comentarios não existe** → rode migrations:
  ```bash
  dotnet ef database update
  ```
- **MySQL em outra porta** → acrescente `Port=3307;` (ou a porta usada) na connection string.
- **Erro SSL/TLS** em alguns ambientes → adicione `SslMode=None;` na connection string.

---

## Observações
- Repo sem artefatos de build (`bin/`, `obj/`, `.vs/`).  
- Não versionamos senha real; use o arquivo de exemplo e copie para `Pitch/appsettings.Development.json`.
