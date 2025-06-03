#Aplicación de Control de Inventario de Productos Electrónicos

Aplicación desarrollada por el para gestionar de forma eficiente el inventario de productos ferreteria, incluyendo control de productos, proveedores, ventas, alertas y reportes.

#Funcionalidades Principales

- **Gestión de Productos**: Registro, modificación, eliminación y consulta de productos electrónicos (marca, modelo, precio, inventario).
- **Gestión de Proveedores**: Registro de proveedores y su historial de compras.
- **Movimientos de Inventario**: Registro de entradas (compras) y salidas (ventas/ajustes).
- **Gestión de Ventas**: Registro de ventas, cálculo automático del stock y actualización del historial.
- **Alertas de Inventario Bajo**: Notificación automática cuando un producto llega a un nivel crítico.
- **Reportes**: Inventario actual, productos más vendidos, productos por proveedor y necesidades de reabastecimiento.

# Reglas de Negocio

1. Solo se permiten ventas si hay suficiente stock.
2. Se genera una alerta automática al llegar a stock mínimo.
3. Los administradores pueden gestionar todo; los empleados solo ventas.
4. Todos los movimientos se registran en el historial.
5. Solo los administradores pueden autorizar devoluciones.

#Estructura de la Base de Datos

#Tablas

- **Productos**: `ProductoID`, `Marca`, `Modelo`, `Precio`, `CantidadInventario`
- **Proveedores**: `ProveedorID`, `Nombre`, `Contacto`, `ProductoID`
- **MovimientosInventario**: `MovimientoID`, `ProductoID`, `FechaMovimiento`, `TipoMovimiento`, `Cantidad`
- **Ventas**: `VentaID`, `ProductoID`, `FechaVenta`, `Cantidad`, `PrecioVenta`
- **Alertas**: `AlertaID`, `ProductoID`, `Mensaje`, `FechaAlerta`

#Procedimientos Almacenados

- `SP_RegistrarMovimiento`: Agrega entradas/salidas y actualiza inventario.
- `SP_RegistrarVenta`: Registra ventas y actualiza inventario.
- `SP_GenerarReporteInventario`: Muestra productos más vendidos, stock bajo y movimientos.
- `SP_EnviarAlertaInventario`: Envía alertas automáticas.
- `SP_ObtenerHistorialComprasProveedor`: Muestra historial por proveedor.

#Requisitos Técnicos

- SQL Server Express / SQL Server Management Studio (SSMS)
- Opcional: Frontend en WinForms, Web, Console App o cualquier otro entorno .NET

##  Cómo Iniciar

1. Ejecuta el script `database.sql` para crear la base de datos.
2. Configura el backend (según tu stack) para conectarse a la base.
3. Compila y ejecuta el frontend.
4. ¡Listo! Ya puedes registrar productos, hacer ventas y controlar tu inventario.

#Autor

- Yimy Alexis Herrera Usuga
- Curso: Herramientas de programación 2

