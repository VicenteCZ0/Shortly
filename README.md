# Shortly

Acortador de URLs construido con ASP.NET Core 9 Razor Pages. Pega una URL larga, obtén una corta y rastrea cuántas veces fue visitada.

## Funcionalidades

- Acorta cualquier URL a un código de 6 caracteres
- Redirección automática al visitar `/{shortUrl}`
- Contador de clics por enlace
- Logging estructurado con Serilog

## Stack

| Capa | Tecnología |
|---|---|
| Framework | ASP.NET Core 9 Razor Pages |
| ORM | Entity Framework Core 9 |
| Base de datos | SQLite |
| Logging | Serilog |

## Arquitectura

Arquitectura en capas:

```
Shortly/
├── Domain/         # Entidades (Link, User)
├── Application/    # Interfaces + Servicios (lógica de negocio)
├── Infrastructure/ # DbContext de EF Core, migraciones, seed
└── Pages/          # Razor Pages (UI)
```

## Cómo correrlo

**Requisito:** .NET 9 SDK

```bash
git clone https://github.com/VicenteCZ0/Shortly-1.git
cd Shortly-1/Shortly
dotnet run
```

Al iniciar por primera vez, la app siembra un usuario administrador. Abre `http://localhost:5117/Login` en el navegador e inicia sesión con:

- **Email:** `admin@shortly.com`
- **Contraseña:** `123456`

## Cómo funciona

1. Ingresa una URL en el formulario de la página principal
2. Shortly genera un código aleatorio de 6 caracteres y guarda el mapeo
3. Al visitar `/{código}` se redirige a la URL original y se incrementa el contador de clics
