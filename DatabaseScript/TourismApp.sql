SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';


-- -----------------------------------------------------
-- Schema tourismappdb
-- -----------------------------------------------------


-- -----------------------------------------------------
-- Schema tourismappdb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `tourismappdb` DEFAULT CHARACTER SET utf8 ;
USE `tourismappdb` ;


-- -----------------------------------------------------
-- Table `tourismappdb`.`category`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`category` (
  `CategoryID` TINYINT(4) NOT NULL AUTO_INCREMENT,
  `CategoryName` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`CategoryID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`country`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`country` (
  `CountryID` SMALLINT(6) NOT NULL AUTO_INCREMENT,
  `CountryName` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`CountryID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`region`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`region` (
  `RegionID` SMALLINT(6) NOT NULL AUTO_INCREMENT,
  `RegionName` VARCHAR(20) NOT NULL,
  `CountryID` SMALLINT(6) NOT NULL,
  PRIMARY KEY (`RegionID`),
  INDEX `fk_Region_Country1_idx` (`CountryID` ASC),
  CONSTRAINT `fk_Region_Country1`
    FOREIGN KEY (`CountryID`)
    REFERENCES `tourismappdb`.`country` (`CountryID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`client`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`client` (
  `ClientID` SMALLINT(6) NOT NULL AUTO_INCREMENT,
  `FirstName` VARCHAR(60) NOT NULL,
  `FirstSurname` VARCHAR(60) NOT NULL,
  `SecondSurname` VARCHAR(60) NULL DEFAULT NULL,
  `Email` VARCHAR(255) NOT NULL,
  `DateOfBirth` DATE NOT NULL,
  `CountryID` SMALLINT(6) NOT NULL,
  `RegionID` SMALLINT(6) NULL DEFAULT NULL,
  `Password` VARBINARY(50) NULL DEFAULT NULL,
  `CreateDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` TIMESTAMP NULL DEFAULT NULL,
  `Status` TINYINT(4) NOT NULL DEFAULT '1',
  PRIMARY KEY (`ClientID`),
  INDEX `fk_Clients_Country_idx` (`CountryID` ASC),
  INDEX `fk_Clients_Region1_idx` (`RegionID` ASC),
  CONSTRAINT `fk_Clients_Country`
    FOREIGN KEY (`CountryID`)
    REFERENCES `tourismappdb`.`country` (`CountryID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Clients_Region1`
    FOREIGN KEY (`RegionID`)
    REFERENCES `tourismappdb`.`region` (`RegionID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`touristspottype`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`touristspottype` (
  `TouristSpotTypeID` TINYINT(4) NOT NULL AUTO_INCREMENT,
  `TouristSpotTypeName` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`TouristSpotTypeID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`touristspot`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`touristspot` (
  `TouristSpotID` SMALLINT(6) NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(100) NOT NULL,
  `Hierarchy` CHAR(4) NOT NULL,
  `Code` VARCHAR(20) NOT NULL,
  `CategoryID` TINYINT(4) NOT NULL,
  `TouristSpotTypeID` TINYINT(4) NOT NULL,
  `Latitude` FLOAT NOT NULL,
  `Longitude` FLOAT NOT NULL,
  `Altitude` DECIMAL(8,2) NOT NULL,
  `Description` VARCHAR(1500) NOT NULL,
  `CreateDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` TIMESTAMP NULL DEFAULT NULL,
  `Status` TINYINT(4) NOT NULL DEFAULT '1',
  PRIMARY KEY (`TouristSpotID`),
  INDEX `fk_TouristSpot_Category1_idx` (`CategoryID` ASC),
  INDEX `fk_TouristSpot_TouristSpotType1_idx` (`TouristSpotTypeID` ASC),
  CONSTRAINT `fk_TouristSpot_Category1`
    FOREIGN KEY (`CategoryID`)
    REFERENCES `tourismappdb`.`category` (`CategoryID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_TouristSpot_TouristSpotType1`
    FOREIGN KEY (`TouristSpotTypeID`)
    REFERENCES `tourismappdb`.`touristspottype` (`TouristSpotTypeID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`comment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`comment` (
  `ClientID` SMALLINT(6) NOT NULL,
  `TouristSpotID` SMALLINT(6) NOT NULL,
  `UserComment` VARCHAR(500) NOT NULL,
  `CreateDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` TIMESTAMP NULL DEFAULT NULL,
  PRIMARY KEY (`ClientID`, `TouristSpotID`),
  INDEX `fk_Clients_has_TouristSpot_TouristSpot1_idx` (`TouristSpotID` ASC),
  INDEX `fk_Clients_has_TouristSpot_Clients1_idx` (`ClientID` ASC),
  CONSTRAINT `fk_Clients_has_TouristSpot_Clients1`
    FOREIGN KEY (`ClientID`)
    REFERENCES `tourismappdb`.`client` (`ClientID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Clients_has_TouristSpot_TouristSpot1`
    FOREIGN KEY (`TouristSpotID`)
    REFERENCES `tourismappdb`.`touristspot` (`TouristSpotID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`filter`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`filter` (
  `FilteredWord` VARCHAR(45) NOT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`path`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`path` (
  `PathName` VARCHAR(50) NOT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`placesofinteresttype`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`placesofinteresttype` (
  `PlacesOfInterestTypeID` TINYINT(4) NOT NULL AUTO_INCREMENT,
  `POITypeName` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`PlacesOfInterestTypeID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`placesofinterest`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`placesofinterest` (
  `PlacesOfInterestID` SMALLINT(6) NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(100) NOT NULL,
  `Latitude` FLOAT NOT NULL,
  `Longitude` FLOAT NOT NULL,
  `PlacesOfInterestTypeID` TINYINT(4) NOT NULL,
  PRIMARY KEY (`PlacesOfInterestID`),
  INDEX `fk_PlacesOfInterest_PlacesOfInterestType1_idx` (`PlacesOfInterestTypeID` ASC),
  CONSTRAINT `fk_PlacesOfInterest_PlacesOfInterestType1`
    FOREIGN KEY (`PlacesOfInterestTypeID`)
    REFERENCES `tourismappdb`.`placesofinteresttype` (`PlacesOfInterestTypeID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`score`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`score` (
  `ClientID` SMALLINT(6) NOT NULL,
  `TouristSpotID` SMALLINT(6) NOT NULL,
  `ScoreTotal` TINYINT(4) NOT NULL,
  `CreateDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdate` TIMESTAMP NULL DEFAULT NULL,
  PRIMARY KEY (`ClientID`, `TouristSpotID`),
  INDEX `fk_Score_Clients1_idx` (`ClientID` ASC),
  INDEX `fk_Score_TouristSpot1_idx` (`TouristSpotID` ASC),
  CONSTRAINT `fk_Score_Clients1`
    FOREIGN KEY (`ClientID`)
    REFERENCES `tourismappdb`.`client` (`ClientID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Score_TouristSpot1`
    FOREIGN KEY (`TouristSpotID`)
    REFERENCES `tourismappdb`.`touristspot` (`TouristSpotID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`touristspotcontent`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`touristspotcontent` (
  `TouristSpotContent` INT(11) NOT NULL AUTO_INCREMENT,
  `TouristSpotID` SMALLINT(6) NOT NULL,
  `Extension` VARCHAR(5) NOT NULL,
  PRIMARY KEY (`TouristSpotContent`),
  INDEX `fk_TouristSpotContent_TouristSpot1_idx` (`TouristSpotID` ASC),
  CONSTRAINT `fk_TouristSpotContent_TouristSpot1`
    FOREIGN KEY (`TouristSpotID`)
    REFERENCES `tourismappdb`.`touristspot` (`TouristSpotID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `tourismappdb`.`touristspotpoi`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourismappdb`.`touristspotpoi` (
  `PlacesOfInterest` SMALLINT(6) NOT NULL,
  `TouristSpot` SMALLINT(6) NOT NULL,
  PRIMARY KEY (`PlacesOfInterest`, `TouristSpot`),
  INDEX `fk_PlacesOfInterest_has_touristspot_touristspot1_idx` (`TouristSpot` ASC),
  INDEX `fk_PlacesOfInterest_has_touristspot_PlacesOfInterest1_idx` (`PlacesOfInterest` ASC),
  CONSTRAINT `fk_PlacesOfInterest_has_touristspot_PlacesOfInterest1`
    FOREIGN KEY (`PlacesOfInterest`)
    REFERENCES `tourismappdb`.`placesofinterest` (`PlacesOfInterestID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_PlacesOfInterest_has_touristspot_touristspot1`
    FOREIGN KEY (`TouristSpot`)
    REFERENCES `tourismappdb`.`touristspot` (`TouristSpotID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
