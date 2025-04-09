CREATE DATABASE  IF NOT EXISTS `cheeseandmore` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `cheeseandmore`;
-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: localhost    Database: cheeseandmore
-- ------------------------------------------------------
-- Server version	8.0.34

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `carrito`
--

DROP TABLE IF EXISTS `carrito`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `carrito` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int DEFAULT NULL,
  `id_producto` int DEFAULT NULL,
  `cantidad` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_usuario` (`id_usuario`),
  KEY `id_producto` (`id_producto`),
  CONSTRAINT `carrito_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `carrito_ibfk_2` FOREIGN KEY (`id_producto`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `carrito`
--

LOCK TABLES `carrito` WRITE;
/*!40000 ALTER TABLE `carrito` DISABLE KEYS */;
/*!40000 ALTER TABLE `carrito` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `catalogo`
--

DROP TABLE IF EXISTS `catalogo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `catalogo` (
  `id` int NOT NULL AUTO_INCREMENT,
  `categorias` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `catalogo`
--

LOCK TABLES `catalogo` WRITE;
/*!40000 ALTER TABLE `catalogo` DISABLE KEYS */;
INSERT INTO `catalogo` VALUES (1,'Tablas de queso'),(2,'Brochetas'),(3,'Canapés');
/*!40000 ALTER TABLE `catalogo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `detallepedido`
--

DROP TABLE IF EXISTS `detallepedido`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `detallepedido` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_pedido` int NOT NULL,
  `id_producto` int NOT NULL,
  `cantidad` int NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_pedido` (`id_pedido`),
  KEY `id_producto` (`id_producto`),
  CONSTRAINT `detallepedido_ibfk_1` FOREIGN KEY (`id_pedido`) REFERENCES `pedido` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `detallepedido_ibfk_2` FOREIGN KEY (`id_producto`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `detallepedido`
--

LOCK TABLES `detallepedido` WRITE;
/*!40000 ALTER TABLE `detallepedido` DISABLE KEYS */;
INSERT INTO `detallepedido` VALUES (12,8,23,3,809.91),(13,8,26,1,89.99),(14,9,17,1,959.99),(15,10,25,1,119.99),(16,10,18,1,859.99),(17,10,23,1,89.99);
/*!40000 ALTER TABLE `detallepedido` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `detalleseventosv2`
--

DROP TABLE IF EXISTS `detalleseventosv2`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `detalleseventosv2` (
  `id` int NOT NULL AUTO_INCREMENT,
  `descripcion` varchar(100) NOT NULL,
  `id_evento` int DEFAULT NULL,
  `nombre` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_evento` (`id_evento`),
  CONSTRAINT `detalleseventosv2_ibfk_1` FOREIGN KEY (`id_evento`) REFERENCES `eventos` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `detalleseventosv2`
--

LOCK TABLES `detalleseventosv2` WRITE;
/*!40000 ALTER TABLE `detalleseventosv2` DISABLE KEYS */;
INSERT INTO `detalleseventosv2` VALUES (1,'Evento donde se ofrecen brochetas variadas, incluyendo opciones vegetarianas y de carne.',1,'Brochetas '),(2,'Evento con mesas llenas de diferentes tipos de quesos, ideales para degustación.',2,'Quesos'),(3,'Evento con una seleccion de diferentes tipos de embutidos y quesos servidos en vasos.',3,'Embutidos y quesos');
/*!40000 ALTER TABLE `detalleseventosv2` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventos`
--

DROP TABLE IF EXISTS `eventos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `eventos` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(60) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventos`
--

LOCK TABLES `eventos` WRITE;
/*!40000 ALTER TABLE `eventos` DISABLE KEYS */;
INSERT INTO `eventos` VALUES (1,'Mesas de brochetas'),(2,'Mesas de queso'),(3,'Vaso charcuteria');
/*!40000 ALTER TABLE `eventos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pedido`
--

DROP TABLE IF EXISTS `pedido`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pedido` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `fecha` date NOT NULL,
  `instrucciones` varchar(500) DEFAULT NULL,
  `telefono` varchar(15) NOT NULL,
  `total` decimal(10,2) NOT NULL,
  `estado` enum('pendiente','entregado','cancelado') NOT NULL DEFAULT 'pendiente',
  PRIMARY KEY (`id`),
  KEY `id_usuario` (`id_usuario`),
  CONSTRAINT `pedido_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pedido`
--

LOCK TABLES `pedido` WRITE;
/*!40000 ALTER TABLE `pedido` DISABLE KEYS */;
INSERT INTO `pedido` VALUES (8,6,'2025-04-23','bla bla bla bla','1234567890',0.00,'pendiente'),(9,6,'2025-04-30','bla bla bla','1234567890',959.99,'pendiente'),(10,6,'2025-04-26','bla bla bla','0987654321',1069.97,'pendiente');
/*!40000 ALTER TABLE `pedido` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productos`
--

DROP TABLE IF EXISTS `productos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `productos` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `id_categoria` int NOT NULL,
  `stock` int NOT NULL,
  `descripcion` text NOT NULL,
  `preciopromocion` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id_categoria` (`id_categoria`),
  CONSTRAINT `productos_ibfk_1` FOREIGN KEY (`id_categoria`) REFERENCES `catalogo` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productos`
--

LOCK TABLES `productos` WRITE;
/*!40000 ALTER TABLE `productos` DISABLE KEYS */;
INSERT INTO `productos` VALUES (16,'Tabla de Queso Gourmet Extra Grande',2350.00,1,8,'aTabla de quesos para 6 personas. Disfruta de una selección de quesos franceses, italianos y mexicanos. La tabla incluye: Fromager d’affinois con trufa francés, 120gr;',NULL),(17,'Tabla de quesos grande',959.99,1,13,'Tabla de quesos para 6 personas. Disfruta de una selección de quesos franceses, italianos y mexicanos. La tabla incluye: Fromager d’affinois con trufa francés, 120gr;',NULL),(18,'Tabla de quesos y carnes frías',859.99,1,13,'Tabla de quesos para 6 personas. Disfruta de una selección de quesos franceses, italianos y mexicanos. La tabla incluye: Fromager d’affinois con trufa francés, 120gr;',NULL),(19,'Pinchos morunos',79.99,2,12,'Tabla de quesos para 6 personas. Disfruta de una selección de quesos franceses, italianos y mexicanos. La tabla incluye: Fromager d’affinois con trufa francés, 120gr;',NULL),(20,'Brochetas de cerdo caramelizado con piña',77.00,2,0,'Tabla de quesos para 6 personas. Disfruta de una selección de quesos franceses, italianos y mexicanos. La tabla incluye: Fromager d’affinois con trufa francés, 120gr;',NULL),(22,'Brochetas de nabo a la parrilla',59.99,2,12,'Tabla de quesos para 6 personas. Disfruta de una selección de quesos franceses, italianos y mexicanos. La tabla incluye: Fromager d’affinois con trufa francés, 120gr;',NULL),(23,'Hojaldres y otras masas',89.99,3,14,'Tabla de quesos para 6 personas. Disfruta de una selección de quesos franceses, italianos y mexicanos. La tabla incluye: Fromager d’affinois con trufa francés, 120gr;',NULL),(25,'Canapés de crema de aguacate y bacalao',119.99,3,17,'Tabla de quesos para 6 personas. Disfruta de una selección de quesos franceses, italianos y mexicanos. La tabla incluye: Fromager d’affinois con trufa francés, 120gr;',NULL),(26,'Canapé de salmón',89.99,3,1,'Tabla de quesos para 6 personas. Disfruta de una selección de quesos franceses, italianos y mexicanos. La tabla incluye: Fromager d’affinois con trufa francés, 120gr;',NULL);
/*!40000 ALTER TABLE `productos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recomendaciones`
--

DROP TABLE IF EXISTS `recomendaciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recomendaciones` (
  `id` int NOT NULL AUTO_INCREMENT,
  `estrellas` int NOT NULL,
  `descripcion` varchar(60) NOT NULL,
  `id_usuarios` int NOT NULL,
  `fecha` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `recomendaciones_ibfk_1` (`id_usuarios`),
  CONSTRAINT `recomendaciones_ibfk_1` FOREIGN KEY (`id_usuarios`) REFERENCES `usuarios` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recomendaciones`
--

LOCK TABLES `recomendaciones` WRITE;
/*!40000 ALTER TABLE `recomendaciones` DISABLE KEYS */;
INSERT INTO `recomendaciones` VALUES (11,5,'xdxdxdxdxddxdxdxddxxdxddxddxdddddddddddddddddd',5,NULL),(13,4,'sss',4,NULL),(14,5,'Que bien',3,NULL),(15,3,'JEje',2,NULL);
/*!40000 ALTER TABLE `recomendaciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre_usuario` varchar(50) NOT NULL,
  `contrasena` varchar(255) NOT NULL,
  `correo` varchar(100) NOT NULL,
  `rol` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `correo` (`correo`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (1,'admin','admin123','admin@queseria.com',1),(2,'cliente1','cliente123','cliente1@dominio.com',2),(3,'cliente2','cliente456','cliente2@dominio.com',2),(4,'Emmanuel Rocha','6BD44836198DFDB9142343BBB627CFF3C74F5B5AB2CD0255EC1A7CB10C3AAF27611E13E7A0756B22A3524BF112A30631778AFEFB520FF35CA92DABF251FE92D6','emma@gmail.com',1),(5,'123','B2FD667EDB44F69C26646EB08F42A7C6F0D475A110BD000433EDEFEE46AB2C3DACC6D21DB3B9A8C74B5B347C9DE47529DB4061C2BAE4D1E5C58C7DD356A2DCDC','123@gmail.com',2),(6,'a','60BAFC00A61641E0FDE307E265740B28AF43D0888376F1283EDCEC46C31A09AA993230205CB648C8E18E816122AD6E4013DCD96354F4854DF81C0E622F5CA9D2','user1@gmail.com',2),(7,'administrdor','EB641D3E5BF35BE212A53152CA45CCC7B15D1F8DD4247BF580347577584104C44C8BDD6EC8E0717FEEB79C070E523A6D8CCF163E6582CA181C7A7383C6B25BCA','admin1@gmail.com',1);
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-08 18:15:18
