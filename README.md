# Blog

### Descrição
<p>API de um blog em arquitetura MVC utilizando ASP.NET6, Entity Framework e SqlServer.</p>
<strong> Status do Projeto: </strong> Em desenvolvimento 🚧

<br>

### Funcionalidades

- [x] CRUD de Categorias
- [x] CRUD de Tags
- [x] CRUD de Papéis do Usuário
- [ ] CRUD de Posts
- [x] Cadastro de Usuários
- [x] Autenticação e Autorização do Usuário com JWT
- [ ] Envio de e-mail
- [ ] Upload de imagens


### Features
- [x] Fluent Mapping
- [x] Migrations
- [x] Autenticação com API Key
- [x] ViewModels
- [x] Atributos personalizados
- [x] Padronização de retornos
- [x] Geração de senha no cadastro do usuário


### Requisitos
EF Tools: 
``` 
dotnet tool install --global dotnet-ef 
```
EF SqlServer: 
``` 
add package Microsoft.EntityFrameworkCore.SqlServer
```
EF Design: 
``` 
add package Microsoft.EntityFrameworkCore.Design
```
Asp.Net Authentication: 
``` 
add package Microsoft.AspNetCore.Authentication
```
Asp.Net JwtBearer: 
``` 
add package Microsoft.AspNetCore.Authentication.JwtBearer
```
SecureIdentity: 
``` 
add package SecureIdentity
```


