# 📈 FinanceMiniAPP — C# / ASP.NET Web API

Aplicación full-stack orientada al consumo, almacenamiento y análisis de activos financieros (stocks, ETFs, etc.). El proyecto está construido con ASP.NET (MINIMAL API REST) y un frontend sencillo en Blazor WebAssembly (MudBlazor), siguiendo principios de arquitectura limpia y buenas prácticas.

La API obtiene los datos financieros desde YahooFinanceAPI, los persiste en MongoDB, utiliza Memory Cache para optimizar el rendimiento y se integra con Gemini (IA) para generar resúmenes automáticos a partir de los datos financieros obtenidos.

---

# 🚀 Funcionalidades principales
## 📈 Activos financieros (Assets / Tickers)

- Consulta de información financiera de un activo por símbolo (ej: AAPL, MSFT, TSLA)
- Persistencia automática del activo en MongoDB
- Cacheo en memoria para evitar llamadas repetidas a la API externa o DB
- Actualización de un activo existente (refresh de datos)
- Manejo unificado de errores mediante resultados tipados

## 🤖 Inteligencia Artificial

- Generación automática de un resumen financiero usando API de Gemini
- El resumen se construye a partir de métricas obtenidas desde Yahoo Finance
- El resultado se almacena junto al activo y se reutiliza desde cache o DB

## 🖥️ Frontend (Blazor WebAssembly)

- Interfaz sencilla y clara construida con MudBlazor
- Visualización de información financiera del activo
- Actualización manual del asset desde la UI
- Consumo de la API mediante cliente generado desde Swagger/OpenAPI
- Estructura desacoplada con interfaces, servicios y DI

---

# 📦 Tecnologías y librerías utilizadas
## 🔧 Backend (.NET API)

- ASP.NET Core Minimal API — API REST
- MongoDB — Base de datos NoSQL para almacenamiento de activos con esquema no fijo
- MemoryCache — Cache en memoria para optimización de rendimiento
- HttpClient Factory — Consumo de APIs externas
- Swagger / OpenAPI — Documentación y generación de clientes

## 🌐 APIs externas

- Yahoo Finance API — Obtención de datos financieros
- Gemini API — Generación de resúmenes financieros mediante IA

## 🎨 Frontend

- Blazor WebAssembly — SPA moderna en C#
- MudBlazor — Componentes UI Material Design

---

# 🛠️ Instalación y configuración

## 1. Clonar o descargar el repositorio
Clona el proyecto con: git clone https://github.com/JSebas-11/FinanceMiniAPP.git; O descárgalo directamente desde GitHub.

## 2. Software requerido
El proyecto se ejecuta en localhost y requiere los siguientes componentes:

- .NET SDK
- MongoDB instalado y ejecutándose localmente
Asegúrate de que el servicio de MongoDB (mongod) esté activo antes de iniciar la API.

## 3. Configurar variables de entorno
- Desde directorio raiz dirigete a: WebApi/appsettings.json e ingresa tu apiKey de gemini en la seccion correspondiente (GeminiApiKey)

(Opcional) Puedes modificar:
- Puertos de ejecución
- Configuración de MongoDB
- URLs base del frontend y backend

Estos valores se encuentran en:
- WebApi/appsettings.json
- WebApi/Properties/launchSettings.json
- WebClient/Properties/launchSettings.json

Recomendación: dejar la configuración por defecto y únicamente agregar la API Key de Gemini para evitar inconsistencias entre proyectos.

## 4. Ejecutar proyecto
- Iniciar el servicio de mongod
- En proyectos (WebApi y WebClient) ejecutar en terminal {dotnet run} o correrlos independientemente desde VisualStudio

(La API estara disponible con Swagger para pruebas manuales en https: "https://localhost:7125/swagger" O tambien mediante http: "http://localhost:5199/swagger",)

---

# 🖼️ Previsualización
