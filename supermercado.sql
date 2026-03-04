-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 04-03-2026 a las 02:15:36
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `supermercado`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cajas`
--

CREATE TABLE `cajas` (
  `ID_Caja` int(11) NOT NULL,
  `NombreCaja` varchar(50) NOT NULL,
  `ID_Usuario_Cajero` int(11) DEFAULT NULL,
  `SaldoInicial` decimal(10,2) NOT NULL DEFAULT 0.00,
  `EstadoActiva` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `cajas`
--

INSERT INTO `cajas` (`ID_Caja`, `NombreCaja`, `ID_Usuario_Cajero`, `SaldoInicial`, `EstadoActiva`) VALUES
(4, 'Caja 1', NULL, 0.00, 1),
(5, 'Caja 2', NULL, 0.00, 1),
(6, 'Caja 3', 3, 50.00, 1),
(7, 'Caja 4', NULL, 0.00, 1),
(8, 'Caja 5', 6, 50000.00, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `categorias`
--

CREATE TABLE `categorias` (
  `ID_Categoria` int(11) NOT NULL,
  `NombreCategoria` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `categorias`
--

INSERT INTO `categorias` (`ID_Categoria`, `NombreCategoria`) VALUES
(1, 'Lácteos'),
(2, 'Bebidas');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `detalles_pedido_web`
--

CREATE TABLE `detalles_pedido_web` (
  `ID_DetallePedido` int(11) NOT NULL,
  `CantidadPedida` int(11) NOT NULL,
  `PrecioAlMomento` decimal(10,2) NOT NULL,
  `ID_PedidoWeb` int(11) NOT NULL,
  `ID_Producto` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `detalles_pedido_web`
--

INSERT INTO `detalles_pedido_web` (`ID_DetallePedido`, `CantidadPedida`, `PrecioAlMomento`, `ID_PedidoWeb`, `ID_Producto`) VALUES
(1, 2, 1.25, 1, 1),
(2, 1, 1.80, 1, 5);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `detalles_venta_fisica`
--

CREATE TABLE `detalles_venta_fisica` (
  `ID_DetalleVenta` int(11) NOT NULL,
  `CantidadComprada` int(11) NOT NULL,
  `PrecioAlMomento` decimal(10,2) NOT NULL,
  `ID_Venta` int(11) NOT NULL,
  `ID_Producto` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `detalles_venta_fisica`
--

INSERT INTO `detalles_venta_fisica` (`ID_DetalleVenta`, `CantidadComprada`, `PrecioAlMomento`, `ID_Venta`, `ID_Producto`) VALUES
(1, 3, 1.25, 1, 1),
(2, 2, 4.50, 1, 15),
(3, 1, 1.75, 2, 12),
(4, 1, 2.75, 2, 4),
(5, 1, 1.50, 2, 3),
(6, 1, 1.50, 3, 3),
(7, 2, 2.20, 4, 10),
(8, 1, 4.50, 4, 15);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `historial_movimientos`
--

CREATE TABLE `historial_movimientos` (
  `ID_Movimiento` int(11) NOT NULL,
  `FechaHora` datetime DEFAULT current_timestamp(),
  `TipoMovimiento` varchar(50) NOT NULL,
  `CantidadMovida` int(11) NOT NULL,
  `Observaciones` varchar(255) DEFAULT NULL,
  `ID_Producto` int(11) NOT NULL,
  `ID_Lote_Afectado` int(11) DEFAULT NULL,
  `ID_Usuario_Responsable` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `historial_movimientos`
--

INSERT INTO `historial_movimientos` (`ID_Movimiento`, `FechaHora`, `TipoMovimiento`, `CantidadMovida`, `Observaciones`, `ID_Producto`, `ID_Lote_Afectado`, `ID_Usuario_Responsable`) VALUES
(1, '2026-03-02 21:52:15', 'TRASLADO_ESTANTE', 5, NULL, 2, 2, 2),
(2, '2026-03-03 07:36:08', 'VENTA', 1, 'Venta física - Refresco Cola 2L (1 unid. del lote BEB-005-A)', 12, 12, 3),
(3, '2026-03-03 07:36:08', 'VENTA', 1, 'Venta física - Queso Fresco 250g (1 unid. del lote LAC-004-A)', 4, 4, 3),
(4, '2026-03-03 07:36:08', 'VENTA', 1, 'Venta física - Yogur Natural 500g (1 unid. del lote LAC-003-A)', 3, 3, 3),
(5, '2026-03-03 08:40:35', 'TRASLADO_ESTANTE', 3, NULL, 1, 1, 2),
(6, '2026-03-03 09:08:50', 'VENTA', 1, 'Venta física - Yogur Natural 500g (1 unid. del lote LAC-003-A)', 3, 3, 3),
(7, '2026-03-03 12:23:15', 'DESCARTE', 10, 'se dejaron en calor', 1, 1, 2),
(8, '2026-03-03 12:25:25', 'ENTRADA_LOTE', 100, 'Ingreso de lote LAC-B12 del proveedor Distribuidora XYZ', 1, 16, 2),
(9, '2026-03-03 12:29:23', 'VENTA', 2, 'Venta física - Jugo de Naranja 1L (2 unid. del lote BEB-003-A)', 10, 10, 3),
(10, '2026-03-03 12:29:23', 'VENTA', 1, 'Venta física - Café Molido 250g (1 unid. del lote BEB-008-A)', 15, 15, 3),
(11, '2026-03-03 12:32:48', 'ENTRADA_LOTE', 500, 'Ingreso de lote QUE-50-SA del proveedor Samsung', 4, 17, 2),
(12, '2026-03-03 12:34:12', 'TRASLADO_ESTANTE', 5, NULL, 4, 17, 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inventario_lotes`
--

CREATE TABLE `inventario_lotes` (
  `ID_Lote` int(11) NOT NULL,
  `CodigoLoteFisico` varchar(50) NOT NULL,
  `FechaProduccion` date NOT NULL,
  `FechaVencimiento` date NOT NULL,
  `FechaIngreso` datetime DEFAULT current_timestamp(),
  `CantidadOriginal` int(11) NOT NULL,
  `UnidadesEnBodega` int(11) DEFAULT 0,
  `UnidadesEnEstante` int(11) DEFAULT 0,
  `UnidadesVendidas` int(11) DEFAULT 0,
  `ID_Producto` int(11) NOT NULL,
  `ID_Proveedor` int(11) NOT NULL,
  `ID_Usuario_Recibio` int(11) NOT NULL,
  `UnidadesDescartadas` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inventario_lotes`
--

INSERT INTO `inventario_lotes` (`ID_Lote`, `CodigoLoteFisico`, `FechaProduccion`, `FechaVencimiento`, `FechaIngreso`, `CantidadOriginal`, `UnidadesEnBodega`, `UnidadesEnEstante`, `UnidadesVendidas`, `ID_Producto`, `ID_Proveedor`, `ID_Usuario_Recibio`, `UnidadesDescartadas`) VALUES
(1, 'LAC-001-A', '2024-12-01', '2025-08-30', '2026-02-27 11:11:06', 50, 37, 20, 0, 1, 1, 2, 10),
(2, 'LAC-002-A', '2024-12-01', '2025-08-30', '2026-02-27 11:11:06', 40, 35, 20, 0, 2, 1, 2, 0),
(3, 'LAC-003-A', '2024-12-05', '2025-04-30', '2026-02-27 11:11:06', 30, 30, 10, 2, 3, 1, 2, 0),
(4, 'LAC-004-A', '2024-12-05', '2025-05-15', '2026-02-27 11:11:06', 25, 25, 9, 1, 4, 1, 2, 0),
(5, 'LAC-005-A', '2024-12-10', '2025-06-30', '2026-02-27 11:11:06', 35, 35, 18, 0, 5, 1, 2, 0),
(6, 'LAC-006-A', '2024-12-10', '2025-09-30', '2026-02-27 11:11:06', 20, 20, 8, 0, 6, 1, 2, 0),
(7, 'LAC-007-A', '2024-11-15', '2026-11-15', '2026-02-27 11:11:06', 45, 45, 15, 0, 7, 1, 2, 0),
(8, 'BEB-001-A', '2024-11-01', '2026-12-31', '2026-02-27 11:11:06', 100, 100, 40, 0, 8, 1, 2, 0),
(9, 'BEB-002-A', '2024-11-01', '2026-12-31', '2026-02-27 11:11:06', 80, 80, 30, 0, 9, 1, 2, 0),
(10, 'BEB-003-A', '2024-12-01', '2025-07-31', '2026-02-27 11:11:06', 60, 60, 23, 2, 10, 1, 2, 0),
(11, 'BEB-004-A', '2024-12-01', '2025-07-31', '2026-02-27 11:11:06', 55, 55, 20, 0, 11, 1, 2, 0),
(12, 'BEB-005-A', '2024-12-15', '2026-06-30', '2026-02-27 11:11:06', 70, 70, 34, 6, 12, 1, 2, 0),
(13, 'BEB-006-A', '2024-12-15', '2026-06-30', '2026-02-27 11:11:06', 90, 90, 40, 0, 13, 1, 2, 0),
(14, 'BEB-007-A', '2024-12-20', '2025-09-30', '2026-02-27 11:11:06', 40, 40, 18, 0, 14, 1, 2, 0),
(15, 'BEB-008-A', '2024-10-01', '2026-10-01', '2026-02-27 11:11:06', 30, 30, 11, 1, 15, 1, 2, 0),
(16, 'LAC-B12', '2026-03-02', '2026-03-25', '2026-03-03 12:25:25', 100, 100, 0, 0, 1, 1, 2, 0),
(17, 'QUE-50-SA', '2026-03-04', '2026-04-10', '2026-03-03 12:32:48', 500, 495, 5, 0, 4, 2, 2, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pedidos_web_pickup`
--

CREATE TABLE `pedidos_web_pickup` (
  `ID_PedidoWeb` int(11) NOT NULL,
  `FechaHoraPedido` datetime DEFAULT current_timestamp(),
  `TotalPedido` decimal(10,2) NOT NULL,
  `EstadoPedido` varchar(30) DEFAULT 'PENDIENTE',
  `ID_Usuario_Cliente` int(11) NOT NULL,
  `ID_Usuario_Cajero_Atendio` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pedidos_web_pickup`
--

INSERT INTO `pedidos_web_pickup` (`ID_PedidoWeb`, `FechaHoraPedido`, `TotalPedido`, `EstadoPedido`, `ID_Usuario_Cliente`, `ID_Usuario_Cajero_Atendio`) VALUES
(1, '2026-03-03 14:16:40', 4.30, 'ENTREGADO', 7, 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `productos`
--

CREATE TABLE `productos` (
  `ID_Producto` int(11) NOT NULL,
  `SKU_CodigoInterno` varchar(50) NOT NULL,
  `NombreProducto` varchar(150) NOT NULL,
  `Descripcion` varchar(255) DEFAULT NULL,
  `PrecioVenta` decimal(10,2) NOT NULL,
  `Stock_Bodega_Total` int(11) DEFAULT 0,
  `Stock_Estante_Total` int(11) DEFAULT 0,
  `Stock_Reservado_Total` int(11) DEFAULT 0,
  `ID_Categoria` int(11) NOT NULL,
  `ImagenUrl` varchar(500) DEFAULT NULL,
  `ID_Ubicacion` int(11) NOT NULL DEFAULT 1,
  `ID_Proveedor` int(11) DEFAULT NULL,
  `Stock_Descartado_Total` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `productos`
--

INSERT INTO `productos` (`ID_Producto`, `SKU_CodigoInterno`, `NombreProducto`, `Descripcion`, `PrecioVenta`, `Stock_Bodega_Total`, `Stock_Estante_Total`, `Stock_Reservado_Total`, `ID_Categoria`, `ImagenUrl`, `ID_Ubicacion`, `ID_Proveedor`, `Stock_Descartado_Total`) VALUES
(1, 'LAC-001', 'Leche Entera 1L', 'Leche entera pasteurizada 1 litro', 1.25, 137, 18, 0, 1, 'https://www.walmart.com.sv/leche-salud-3785ml/p?srsltid=AfmBOooRq8F2tQ51Uvnxr0HJ_ywP-Pf-YpGw2YKYN8DEhctH7i7U4cnG', 4, 1, 10),
(2, 'LAC-002', 'Leche Descremada 1L', 'Leche descremada pasteurizada 1 litro', 1.25, 35, 20, 0, 1, NULL, 4, 1, 0),
(3, 'LAC-003', 'Yogur Natural 500g', 'Yogur natural sin azúcar 500g', 1.50, 30, 10, 0, 1, NULL, 4, 1, 0),
(4, 'LAC-004', 'Queso Fresco 250g', 'Queso fresco artesanal 250 gramos', 2.75, 520, 14, 0, 1, NULL, 4, 1, 0),
(5, 'LAC-005', 'Crema de Leche 200ml', 'Crema de leche para cocinar 200ml', 1.80, 35, 17, 0, 1, NULL, 4, 1, 0),
(6, 'LAC-006', 'Mantequilla 100g', 'Mantequilla sin sal 100 gramos', 1.60, 20, 8, 0, 1, NULL, 6, 1, 0),
(7, 'LAC-007', 'Leche en Polvo 400g', 'Leche en polvo entera 400 gramos', 3.50, 45, 15, 0, 1, NULL, 5, 1, 0),
(8, 'BEB-001', 'Agua Pura 500ml', 'Agua purificada botella 500ml', 0.50, 100, 40, 0, 2, NULL, 1, 1, 0),
(9, 'BEB-002', 'Agua Pura 1.5L', 'Agua purificada botella 1.5 litros', 0.85, 80, 30, 0, 2, NULL, 1, 1, 0),
(10, 'BEB-003', 'Jugo de Naranja 1L', 'Jugo natural de naranja 1 litro', 2.20, 60, 23, 0, 2, NULL, 2, 1, 0),
(11, 'BEB-004', 'Jugo de Mango 500ml', 'Néctar de mango 500ml', 1.50, 55, 20, 0, 2, NULL, 2, 1, 0),
(12, 'BEB-005', 'Refresco Cola 2L', 'Refresco de cola 2 litros', 1.75, 70, 34, 5, 2, NULL, 1, 1, 0),
(13, 'BEB-006', 'Refresco Naranja 500ml', 'Refresco sabor naranja 500ml', 0.75, 90, 40, 0, 2, NULL, 1, 1, 0),
(14, 'BEB-007', 'Té Helado Limón 1L', 'Té helado sabor limón 1 litro', 1.95, 40, 18, 0, 2, NULL, 2, 1, 0),
(15, 'BEB-008', 'Café Molido 250g', 'Café molido premium 250 gramos', 4.50, 30, 9, 0, 2, NULL, 5, 1, 0),
(16, 'kiko', 'Dientes de ajo', 'Dientes de ajo', 0.15, 0, 0, 0, 2, 'dientesajo.png', 2, NULL, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proveedores`
--

CREATE TABLE `proveedores` (
  `ID_Proveedor` int(11) NOT NULL,
  `NombreEmpresa` varchar(150) NOT NULL,
  `ContactoAsignado` varchar(100) DEFAULT NULL,
  `Telefono` varchar(20) DEFAULT NULL,
  `EstadoActivo` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `proveedores`
--

INSERT INTO `proveedores` (`ID_Proveedor`, `NombreEmpresa`, `ContactoAsignado`, `Telefono`, `EstadoActivo`) VALUES
(1, 'Distribuidora XYZ', 'Pedro López', '22223333', 1),
(2, 'Samsung', 'Pablo Domínguez', '67676767', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `roles`
--

CREATE TABLE `roles` (
  `ID_Rol` int(11) NOT NULL,
  `NombreRol` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `roles`
--

INSERT INTO `roles` (`ID_Rol`, `NombreRol`) VALUES
(1, 'Administrador'),
(2, 'Bodeguero'),
(3, 'Cajero'),
(4, 'Cliente');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ubicaciones_bodega`
--

CREATE TABLE `ubicaciones_bodega` (
  `ID_Ubicacion` int(11) NOT NULL,
  `NombreUbicacion` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `ubicaciones_bodega`
--

INSERT INTO `ubicaciones_bodega` (`ID_Ubicacion`, `NombreUbicacion`) VALUES
(7, 'Almacén General'),
(4, 'Bodega Fría'),
(5, 'Bodega Seca'),
(6, 'Estante Principal'),
(1, 'Pasillo A'),
(2, 'Pasillo B'),
(3, 'Pasillo C');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `ID_Usuario` int(11) NOT NULL,
  `NombreCompleto` varchar(150) NOT NULL,
  `Correo_Usuario` varchar(100) NOT NULL,
  `Contrasena` varchar(255) NOT NULL,
  `EstadoActivo` tinyint(1) DEFAULT 1,
  `ID_Rol` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`ID_Usuario`, `NombreCompleto`, `Correo_Usuario`, `Contrasena`, `EstadoActivo`, `ID_Rol`) VALUES
(1, 'Administrador', 'admin@super.com', 'admin123', 1, 1),
(2, 'Carlos Bodega', 'bodeguero@super.com', 'bodega123', 1, 2),
(3, 'Maria Cajero', 'cajero@super.com', 'cajero123', 1, 3),
(4, 'Juan Pérez', 'juan@gmail.com', 'cliente123', 1, 4),
(5, 'Juan Pérez', 'juan1@gmail.com', 'cliente123', 1, 4),
(6, 'Samuel Eduardo Argueta Solís', 'ArguetaSolis@gmail.com', 'SamuelUwU', 1, 3),
(7, 'Jose Steven', 'akani@gmail.com', 'Jose2008', 1, 4);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ventas_fisicas`
--

CREATE TABLE `ventas_fisicas` (
  `ID_Venta` int(11) NOT NULL,
  `FechaHora` datetime DEFAULT current_timestamp(),
  `TotalVenta` decimal(10,2) NOT NULL,
  `EstadoVenta` varchar(20) DEFAULT 'APROBADA',
  `ID_Usuario_Cajero` int(11) NOT NULL,
  `MontoRecibido` decimal(10,2) NOT NULL DEFAULT 0.00,
  `Cambio` decimal(10,2) NOT NULL DEFAULT 0.00,
  `TipoPago` enum('Efectivo','Tarjeta') NOT NULL DEFAULT 'Efectivo',
  `ID_Caja` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `ventas_fisicas`
--

INSERT INTO `ventas_fisicas` (`ID_Venta`, `FechaHora`, `TotalVenta`, `EstadoVenta`, `ID_Usuario_Cajero`, `MontoRecibido`, `Cambio`, `TipoPago`, `ID_Caja`) VALUES
(1, '2026-03-02 20:51:16', 12.75, 'APROBADA', 3, 20.00, 7.25, 'Efectivo', 6),
(2, '2026-03-03 07:36:07', 6.00, 'APROBADA', 3, 7.00, 1.00, 'Efectivo', 6),
(3, '2026-03-03 09:08:50', 1.50, 'APROBADA', 3, 400.00, 398.50, 'Efectivo', 6),
(4, '2026-03-03 12:29:23', 8.90, 'APROBADA', 3, 10.00, 1.10, 'Efectivo', 6);

-- --------------------------------------------------------

--
-- Estructura Stand-in para la vista `vista_saldo_cajas`
-- (Véase abajo para la vista actual)
--
CREATE TABLE `vista_saldo_cajas` (
`ID_Caja` int(11)
,`NombreCaja` varchar(50)
,`NombreCajero` varchar(150)
,`SaldoInicial` decimal(10,2)
,`TotalEntradas` decimal(32,2)
,`TotalSalidas` decimal(32,2)
,`SaldoActual` decimal(34,2)
,`EstadoActiva` tinyint(1)
);

-- --------------------------------------------------------

--
-- Estructura para la vista `vista_saldo_cajas`
--
DROP TABLE IF EXISTS `vista_saldo_cajas`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vista_saldo_cajas`  AS SELECT `c`.`ID_Caja` AS `ID_Caja`, `c`.`NombreCaja` AS `NombreCaja`, `u`.`NombreCompleto` AS `NombreCajero`, `c`.`SaldoInicial` AS `SaldoInicial`, coalesce(sum(`v`.`MontoRecibido`),0) AS `TotalEntradas`, coalesce(sum(`v`.`Cambio`),0) AS `TotalSalidas`, `c`.`SaldoInicial`+ coalesce(sum(`v`.`MontoRecibido`),0) - coalesce(sum(`v`.`Cambio`),0) AS `SaldoActual`, `c`.`EstadoActiva` AS `EstadoActiva` FROM ((`cajas` `c` join `usuarios` `u` on(`c`.`ID_Usuario_Cajero` = `u`.`ID_Usuario`)) left join `ventas_fisicas` `v` on(`v`.`ID_Caja` = `c`.`ID_Caja`)) GROUP BY `c`.`ID_Caja`, `c`.`NombreCaja`, `u`.`NombreCompleto`, `c`.`SaldoInicial`, `c`.`EstadoActiva` ;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `cajas`
--
ALTER TABLE `cajas`
  ADD PRIMARY KEY (`ID_Caja`),
  ADD KEY `fk_caja_cajero` (`ID_Usuario_Cajero`);

--
-- Indices de la tabla `categorias`
--
ALTER TABLE `categorias`
  ADD PRIMARY KEY (`ID_Categoria`);

--
-- Indices de la tabla `detalles_pedido_web`
--
ALTER TABLE `detalles_pedido_web`
  ADD PRIMARY KEY (`ID_DetallePedido`),
  ADD KEY `ID_PedidoWeb` (`ID_PedidoWeb`),
  ADD KEY `ID_Producto` (`ID_Producto`);

--
-- Indices de la tabla `detalles_venta_fisica`
--
ALTER TABLE `detalles_venta_fisica`
  ADD PRIMARY KEY (`ID_DetalleVenta`),
  ADD KEY `ID_Venta` (`ID_Venta`),
  ADD KEY `ID_Producto` (`ID_Producto`);

--
-- Indices de la tabla `historial_movimientos`
--
ALTER TABLE `historial_movimientos`
  ADD PRIMARY KEY (`ID_Movimiento`),
  ADD KEY `ID_Producto` (`ID_Producto`),
  ADD KEY `ID_Lote_Afectado` (`ID_Lote_Afectado`),
  ADD KEY `ID_Usuario_Responsable` (`ID_Usuario_Responsable`);

--
-- Indices de la tabla `inventario_lotes`
--
ALTER TABLE `inventario_lotes`
  ADD PRIMARY KEY (`ID_Lote`),
  ADD KEY `ID_Producto` (`ID_Producto`),
  ADD KEY `ID_Proveedor` (`ID_Proveedor`),
  ADD KEY `ID_Usuario_Recibio` (`ID_Usuario_Recibio`);

--
-- Indices de la tabla `pedidos_web_pickup`
--
ALTER TABLE `pedidos_web_pickup`
  ADD PRIMARY KEY (`ID_PedidoWeb`),
  ADD KEY `ID_Usuario_Cliente` (`ID_Usuario_Cliente`),
  ADD KEY `ID_Usuario_Cajero_Atendio` (`ID_Usuario_Cajero_Atendio`);

--
-- Indices de la tabla `productos`
--
ALTER TABLE `productos`
  ADD PRIMARY KEY (`ID_Producto`),
  ADD UNIQUE KEY `SKU_CodigoInterno` (`SKU_CodigoInterno`),
  ADD KEY `ID_Categoria` (`ID_Categoria`),
  ADD KEY `fk_producto_ubicacion` (`ID_Ubicacion`),
  ADD KEY `fk_producto_proveedor` (`ID_Proveedor`);

--
-- Indices de la tabla `proveedores`
--
ALTER TABLE `proveedores`
  ADD PRIMARY KEY (`ID_Proveedor`);

--
-- Indices de la tabla `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`ID_Rol`);

--
-- Indices de la tabla `ubicaciones_bodega`
--
ALTER TABLE `ubicaciones_bodega`
  ADD PRIMARY KEY (`ID_Ubicacion`),
  ADD UNIQUE KEY `NombreUbicacion` (`NombreUbicacion`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`ID_Usuario`),
  ADD UNIQUE KEY `Correo_Usuario` (`Correo_Usuario`),
  ADD KEY `ID_Rol` (`ID_Rol`);

--
-- Indices de la tabla `ventas_fisicas`
--
ALTER TABLE `ventas_fisicas`
  ADD PRIMARY KEY (`ID_Venta`),
  ADD KEY `ID_Usuario_Cajero` (`ID_Usuario_Cajero`),
  ADD KEY `fk_venta_caja` (`ID_Caja`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `cajas`
--
ALTER TABLE `cajas`
  MODIFY `ID_Caja` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `categorias`
--
ALTER TABLE `categorias`
  MODIFY `ID_Categoria` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `detalles_pedido_web`
--
ALTER TABLE `detalles_pedido_web`
  MODIFY `ID_DetallePedido` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `detalles_venta_fisica`
--
ALTER TABLE `detalles_venta_fisica`
  MODIFY `ID_DetalleVenta` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `historial_movimientos`
--
ALTER TABLE `historial_movimientos`
  MODIFY `ID_Movimiento` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `inventario_lotes`
--
ALTER TABLE `inventario_lotes`
  MODIFY `ID_Lote` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT de la tabla `pedidos_web_pickup`
--
ALTER TABLE `pedidos_web_pickup`
  MODIFY `ID_PedidoWeb` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `productos`
--
ALTER TABLE `productos`
  MODIFY `ID_Producto` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `proveedores`
--
ALTER TABLE `proveedores`
  MODIFY `ID_Proveedor` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `roles`
--
ALTER TABLE `roles`
  MODIFY `ID_Rol` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `ubicaciones_bodega`
--
ALTER TABLE `ubicaciones_bodega`
  MODIFY `ID_Ubicacion` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `ID_Usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `ventas_fisicas`
--
ALTER TABLE `ventas_fisicas`
  MODIFY `ID_Venta` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `cajas`
--
ALTER TABLE `cajas`
  ADD CONSTRAINT `fk_caja_cajero` FOREIGN KEY (`ID_Usuario_Cajero`) REFERENCES `usuarios` (`ID_Usuario`);

--
-- Filtros para la tabla `detalles_pedido_web`
--
ALTER TABLE `detalles_pedido_web`
  ADD CONSTRAINT `detalles_pedido_web_ibfk_1` FOREIGN KEY (`ID_PedidoWeb`) REFERENCES `pedidos_web_pickup` (`ID_PedidoWeb`),
  ADD CONSTRAINT `detalles_pedido_web_ibfk_2` FOREIGN KEY (`ID_Producto`) REFERENCES `productos` (`ID_Producto`);

--
-- Filtros para la tabla `detalles_venta_fisica`
--
ALTER TABLE `detalles_venta_fisica`
  ADD CONSTRAINT `detalles_venta_fisica_ibfk_1` FOREIGN KEY (`ID_Venta`) REFERENCES `ventas_fisicas` (`ID_Venta`),
  ADD CONSTRAINT `detalles_venta_fisica_ibfk_2` FOREIGN KEY (`ID_Producto`) REFERENCES `productos` (`ID_Producto`);

--
-- Filtros para la tabla `historial_movimientos`
--
ALTER TABLE `historial_movimientos`
  ADD CONSTRAINT `historial_movimientos_ibfk_1` FOREIGN KEY (`ID_Producto`) REFERENCES `productos` (`ID_Producto`),
  ADD CONSTRAINT `historial_movimientos_ibfk_2` FOREIGN KEY (`ID_Lote_Afectado`) REFERENCES `inventario_lotes` (`ID_Lote`),
  ADD CONSTRAINT `historial_movimientos_ibfk_3` FOREIGN KEY (`ID_Usuario_Responsable`) REFERENCES `usuarios` (`ID_Usuario`);

--
-- Filtros para la tabla `inventario_lotes`
--
ALTER TABLE `inventario_lotes`
  ADD CONSTRAINT `inventario_lotes_ibfk_1` FOREIGN KEY (`ID_Producto`) REFERENCES `productos` (`ID_Producto`),
  ADD CONSTRAINT `inventario_lotes_ibfk_2` FOREIGN KEY (`ID_Proveedor`) REFERENCES `proveedores` (`ID_Proveedor`),
  ADD CONSTRAINT `inventario_lotes_ibfk_3` FOREIGN KEY (`ID_Usuario_Recibio`) REFERENCES `usuarios` (`ID_Usuario`);

--
-- Filtros para la tabla `pedidos_web_pickup`
--
ALTER TABLE `pedidos_web_pickup`
  ADD CONSTRAINT `pedidos_web_pickup_ibfk_1` FOREIGN KEY (`ID_Usuario_Cliente`) REFERENCES `usuarios` (`ID_Usuario`),
  ADD CONSTRAINT `pedidos_web_pickup_ibfk_2` FOREIGN KEY (`ID_Usuario_Cajero_Atendio`) REFERENCES `usuarios` (`ID_Usuario`);

--
-- Filtros para la tabla `productos`
--
ALTER TABLE `productos`
  ADD CONSTRAINT `fk_producto_proveedor` FOREIGN KEY (`ID_Proveedor`) REFERENCES `proveedores` (`ID_Proveedor`),
  ADD CONSTRAINT `fk_producto_ubicacion` FOREIGN KEY (`ID_Ubicacion`) REFERENCES `ubicaciones_bodega` (`ID_Ubicacion`),
  ADD CONSTRAINT `productos_ibfk_1` FOREIGN KEY (`ID_Categoria`) REFERENCES `categorias` (`ID_Categoria`);

--
-- Filtros para la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD CONSTRAINT `usuarios_ibfk_1` FOREIGN KEY (`ID_Rol`) REFERENCES `roles` (`ID_Rol`);

--
-- Filtros para la tabla `ventas_fisicas`
--
ALTER TABLE `ventas_fisicas`
  ADD CONSTRAINT `fk_venta_caja` FOREIGN KEY (`ID_Caja`) REFERENCES `cajas` (`ID_Caja`),
  ADD CONSTRAINT `ventas_fisicas_ibfk_1` FOREIGN KEY (`ID_Usuario_Cajero`) REFERENCES `usuarios` (`ID_Usuario`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
