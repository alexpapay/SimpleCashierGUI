-- phpMyAdmin SQL Dump
-- version 3.5.1
-- http://www.phpmyadmin.net
--
-- Хост: 127.0.0.1
-- Время создания: Июн 11 2017 г., 15:20
-- Версия сервера: 5.5.25
-- Версия PHP: 5.3.13

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- База данных: `cashierbase`
--

-- --------------------------------------------------------

--
-- Структура таблицы `docs`
--

CREATE TABLE IF NOT EXISTS `docs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DocNum` int(128) NOT NULL,
  `DocQty` int(11) NOT NULL,
  `DocSum` double NOT NULL,
  `SaleDate` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=12 ;

--
-- Дамп данных таблицы `docs`
--

INSERT INTO `docs` (`Id`, `DocNum`, `DocQty`, `DocSum`, `SaleDate`) VALUES
(1, 1, 1, 150, '2017-06-10 23:16:14'),
(2, 2, 2, 300, '2017-06-10 23:22:19'),
(3, 3, 0, 0, '2017-06-10 23:22:32'),
(4, 3, 3, 450, '2017-06-10 23:23:46'),
(5, 4, 3, 1050, '2017-06-10 23:24:18'),
(6, 5, 2, 800, '2017-06-11 00:36:38'),
(7, 6, 2, 1250, '2017-06-11 00:54:34'),
(8, 7, 1, 900, '2017-06-11 00:56:17'),
(9, 8, 1, 650, '2017-06-11 00:57:27'),
(10, 9, 1, 650, '2017-06-11 00:57:33'),
(11, 10, 1, 900, '2017-06-11 00:59:12');

-- --------------------------------------------------------

--
-- Структура таблицы `salestables`
--

CREATE TABLE IF NOT EXISTS `salestables` (
  `Id` int(128) NOT NULL AUTO_INCREMENT,
  `GoodName` varchar(256) NOT NULL,
  `Cost` double NOT NULL,
  `Qty` int(11) NOT NULL,
  `CostSum` double NOT NULL,
  `Code` int(11) NOT NULL,
  `DocNum` int(11) NOT NULL,
  `SaleDate` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=13 ;

--
-- Дамп данных таблицы `salestables`
--

INSERT INTO `salestables` (`Id`, `GoodName`, `Cost`, `Qty`, `CostSum`, `Code`, `DocNum`, `SaleDate`) VALUES
(1, 'Печенье "Мальчиш", арахис. 100 гр.', 150, 1, 150, 3245, 1, '2017-06-10 23:16:14'),
(2, 'Печенье "Мальчиш", арахис. 100 гр.', 150, 2, 300, 3245, 2, '2017-06-10 23:22:19'),
(3, 'Печенье "Мальчиш", арахис. 100 гр.', 150, 3, 450, 3245, 3, '2017-06-10 23:23:46'),
(4, 'Варенье "Кибальчиш", стекло. 350 гр.', 350, 3, 1050, 5360, 4, '2017-06-10 23:24:18'),
(5, 'Варенье "Кибальчиш", стекло. 500 гр.', 650, 1, 650, 5361, 5, '2017-06-11 00:36:38'),
(6, 'Печенье "Мальчиш", арахис. 100 гр.', 150, 1, 150, 3245, 5, '2017-06-11 00:36:38'),
(7, 'Варенье "Кибальчиш", стекло. 350 гр.', 350, 1, 350, 5360, 6, '2017-06-11 00:54:34'),
(8, 'Варенье "Кибальчиш", стекло. 1000 гр.', 900, 1, 900, 5362, 6, '2017-06-11 00:54:34'),
(9, 'Варенье "Кибальчиш", стекло. 1000 гр.', 900, 1, 900, 5362, 7, '2017-06-11 00:56:17'),
(10, 'Варенье "Кибальчиш", стекло. 500 гр.', 650, 1, 650, 5361, 8, '2017-06-11 00:57:27'),
(11, 'Варенье "Кибальчиш", стекло. 500 гр.', 650, 1, 650, 5361, 9, '2017-06-11 00:57:33'),
(12, 'Варенье "Кибальчиш", стекло. 1000 гр.', 900, 1, 900, 5362, 10, '2017-06-11 00:59:12');

-- --------------------------------------------------------

--
-- Структура таблицы `storetables`
--

CREATE TABLE IF NOT EXISTS `storetables` (
  `Id` int(128) NOT NULL AUTO_INCREMENT,
  `GoodName` varchar(256) NOT NULL,
  `Cost` double NOT NULL,
  `Qty` int(11) NOT NULL,
  `Code` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=7 ;

--
-- Дамп данных таблицы `storetables`
--

INSERT INTO `storetables` (`Id`, `GoodName`, `Cost`, `Qty`, `Code`) VALUES
(1, 'Печенье "Мальчиш", арахис. 100 гр.', 150, 3, 3245),
(3, 'Варенье "Кибальчиш", стекло. 350 гр.', 350, 6, 5360),
(5, 'Варенье "Кибальчиш", стекло. 1000 гр.', 900, 3, 5362),
(6, 'Варенье "Кибальчиш", стекло. 500 гр.', 650, 5, 5361);

-- --------------------------------------------------------

--
-- Структура таблицы `users`
--

CREATE TABLE IF NOT EXISTS `users` (
  `Id` int(128) NOT NULL AUTO_INCREMENT,
  `Login` varchar(256) NOT NULL,
  `Password` varchar(256) NOT NULL,
  `AccessLevel` int(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;

--
-- Дамп данных таблицы `users`
--

INSERT INTO `users` (`Id`, `Login`, `Password`, `AccessLevel`) VALUES
(1, 'saler', 'E10ADC3949BA59ABBE56E057F20F883E', 1);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
