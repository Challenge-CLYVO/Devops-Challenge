drop table Aplicacao_vacina cascade constraints;
drop table Clinica cascade constraints;
drop table Consulta cascade constraints;
drop table Historico_saude cascade constraints;
drop table Log_erro cascade constraints;
drop table Pet cascade constraints;
drop table Tutor cascade constraints;
drop table Vacina cascade constraints;

--Tabelas SQL criadas pelo MER
CREATE TABLE Aplicacao_vacina 
    ( 
     id_aplicacao   INTEGER  NOT NULL , 
     data_aplicacao DATE  NOT NULL , 
     id_vacina      INTEGER  NOT NULL , 
     id_pet         INTEGER  NOT NULL 
    ) 
;
 
ALTER TABLE Aplicacao_vacina 
    ADD CONSTRAINT Aplicacao_vacina_PK PRIMARY KEY ( id_aplicacao ) ;
 
CREATE TABLE Clinica 
    ( 
     id_clinica INTEGER  NOT NULL , 
     nome       VARCHAR2 (100)  NOT NULL , 
     endereco   VARCHAR2 (200)  NOT NULL , 
     telefone   VARCHAR2 (20) 
    ) 
;
 
ALTER TABLE Clinica 
    ADD CONSTRAINT id_clinica_PK PRIMARY KEY ( id_clinica ) ;
 
CREATE TABLE Consulta 
    ( 
     id_consulta   INTEGER  NOT NULL , 
     data_consulta DATE  NOT NULL , 
     descricao     VARCHAR2 (200) , 
     id_pet        INTEGER  NOT NULL , 
     id_clinica    INTEGER  NOT NULL 
    ) 
;
 
ALTER TABLE Consulta 
    ADD CONSTRAINT Consulta_PK PRIMARY KEY ( id_consulta ) ;
 
CREATE TABLE Historico_saude 
    ( 
     id_historico  INTEGER  NOT NULL , 
     descricao     VARCHAR2 (255)  NOT NULL , 
     data_registro DATE  NOT NULL , 
     id_pet        INTEGER  NOT NULL 
    ) 
;
 
ALTER TABLE Historico_saude 
    ADD CONSTRAINT Historico_saude_PK PRIMARY KEY ( id_historico ) ;
 
CREATE TABLE Log_erro 
    ( 
     id_log         INTEGER GENERATED ALWAYS AS IDENTITY, 
     nome_procedure VARCHAR2 (100)  NOT NULL , 
     usuario        VARCHAR2 (100)  NOT NULL , 
     data_erro      DATE  NOT NULL , 
     codigo_erro    INTEGER , 
     mensagem_erro  VARCHAR2 (4000)  NOT NULL 
    ) 
;
 
ALTER TABLE Log_erro 
    ADD CONSTRAINT Log_erro_PK PRIMARY KEY ( id_log ) ;
 
CREATE TABLE Pet 
    ( 
     id_pet   INTEGER  NOT NULL , 
     nome     VARCHAR2 (100)  NOT NULL , 
     idade    INTEGER , 
     especie  VARCHAR2 (50)  NOT NULL , 
     raca     VARCHAR2 (50) , 
     id_tutor INTEGER  NOT NULL 
    ) 
;
 
ALTER TABLE Pet 
    ADD CONSTRAINT Pet_PK PRIMARY KEY ( id_pet ) ;
 
CREATE TABLE Tutor 
    ( 
     id_tutor INTEGER  NOT NULL , 
     nome     VARCHAR2 (100)  NOT NULL , 
     telefone VARCHAR2 (20) , 
     email    VARCHAR2 (100) 
    ) 
;
 
ALTER TABLE Tutor 
    ADD CONSTRAINT Tutor_PK PRIMARY KEY ( id_tutor ) ;
 
CREATE TABLE Vacina 
    ( 
     id_vacina INTEGER  NOT NULL , 
     nome      VARCHAR2 (100)  NOT NULL , 
     descricao VARCHAR2 (200) 
    ) 
;
 
ALTER TABLE Vacina 
    ADD CONSTRAINT Vacina_PK PRIMARY KEY ( id_vacina ) ;
 
ALTER TABLE Aplicacao_vacina 
    ADD CONSTRAINT Aplicacao_vacina_Pet_FK FOREIGN KEY 
    ( 
     id_pet
    ) 
    REFERENCES Pet 
    ( 
     id_pet
    ) 
;
 
ALTER TABLE Aplicacao_vacina 
    ADD CONSTRAINT Aplicacao_vacina_Vacina_FK FOREIGN KEY 
    ( 
     id_vacina
    ) 
    REFERENCES Vacina 
    ( 
     id_vacina
    ) 
;
 
ALTER TABLE Consulta 
    ADD CONSTRAINT Consulta_id_clinica_FK FOREIGN KEY 
    ( 
     id_clinica
    ) 
    REFERENCES Clinica 
    ( 
     id_clinica
    ) 
;
 
ALTER TABLE Consulta 
    ADD CONSTRAINT Consulta_Pet_FK FOREIGN KEY 
    ( 
     id_pet
    ) 
    REFERENCES Pet 
    ( 
     id_pet
    ) 
;
 
ALTER TABLE Historico_saude 
    ADD CONSTRAINT Historico_saude_Pet_FK FOREIGN KEY 
    ( 
     id_pet
    ) 
    REFERENCES Pet 
    ( 
     id_pet
    ) 
;
 
ALTER TABLE Pet 
    ADD CONSTRAINT Pet_Tutor_FK FOREIGN KEY 
    ( 
     id_tutor
    ) 
    REFERENCES Tutor 
    ( 
     id_tutor
    ) 
;