# 🌍 ViajesAPI

**ViajesAPI** es una API desarrollada en C# con .NET que gestiona todo el flujo de compra y devolución de viajes, incluyendo pagos reales con **Bizum** a través de la plataforma **Redsys**.

---

## 🚀 Funcionalidades principales

- 📋 **Registro de usuarios** mediante correo electrónico validado por expresión regular.
- 🔐 **Autenticación con roles**: usuario y administrador.
- 🔍 **Búsqueda de viajes** según criterios personalizados.
- 💳 **Compra de viajes** con integración de pago real mediante Bizum (a través de Redsys).
- 📦 **Gestión de compras y devoluciones**.
- 🧑‍💼 **Panel de administración** con las siguientes capacidades:
  - Añadir, modificar y eliminar viajes.
  - Convertir usuarios en administradores.
  - Aprobar devoluciones.

> 🛑 **Importante:** Las devoluciones están condicionadas por las siguientes reglas:
> - Solo pueden realizarse si la compra ha sido pagada.
> - Las devoluciones no pueden hacerse dentro de los 3 días anteriores al viaje.
> - Si una compra no se paga en menos de 5 minutos, esta se cancela automáticamente.

---

## 🔐 Gestión de usuarios y roles

- Los usuarios se registran de forma libre con un email válido.
- Un administrador puede convertir cualquier usuario en admin.
- Se utiliza un sistema básico de control de acceso para limitar funcionalidades según el rol.

---

## 🛠️ Tecnologías utilizadas

| Tecnología         | Descripción                            |
|--------------------|----------------------------------------|
| **Lenguaje**       | C#                                     |
| **Framework**      | .NET                                   |
| **ORM**            | Entity Framework                       |
| **IDE**            | Microsoft Visual Studio                |
| **Base de datos**  | SQL Server                             |
| **API de pagos**   | Redsys con Bizum                       |
| **Documentación**  | Swagger (incluido en el proyecto)      |

---

## 💸 Integración con Bizum

La API está conectada con **Bizum** a través de la pasarela **Redsys**, permitiendo realizar pagos y devoluciones **reales y automáticas** desde la propia aplicación.

---

## 📄 Documentación técnica

- ✔️ **Swagger** está incluido para explorar y probar los endpoints.
- ❌ Actualmente **no hay documentación externa** ni clave de API requerida para consumir los endpoints.

---

## ⚙️ Estado del proyecto

✅ *Proyecto finalizado y funcional.*

---


