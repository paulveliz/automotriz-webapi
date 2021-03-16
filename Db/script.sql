---------------------- CATALOGOS --------------------------

CREATE TABLE Estados_civiles( -- CAN BE : SOLTERO, CASADO
    Id int primary key identity(1,1),
    tipo varchar(25) not null
)

CREATE TABLE Marcas(
    Id int primary key identity(1,1),
    Nombre varchar(55) not null
)

CREATE TABLE Modelos(
    Id int primary key identity(1,1),
    Nombre varchar(55) not null,
    id_marca int not null,
    FOREIGN KEY (id_marca) REFERENCES Marcas(Id)
)
------------------------------------------------------------

-------------------------- LOGISTICA ------------------------

CREATE TABLE Planes_financiamientos(
    Id int primary key identity(1,1),
    Descripcion TEXT not null,
    Precio_inicial numeric(18,2),
    Precio_limite numeric(18,2)
)

CREATE TABLE Autos(
    Id int primary key identity(1,1),
    Valor_Comecial numeric(18,2) not null,
    Url_imagen varchar(max) not null,
    id_plan_financiamiento int not null,
    id_modelo int not null,
    FOREIGN KEY (id_plan_financiamiento) REFERENCES Planes_financiamientos(Id),
    FOREIGN KEY (id_modelo) REFERENCES Modelos(Id)
)

CREATE TABLE Clientes(
    Id int primary key identity(1,1),
    Nombre_completo varchar(max) not null,
    Fecha_nacimiento Date not null,
    Domicilio varchar(max) not null,
    Curp varchar(18) not null,
    Ingresos_mensuales numeric(10,2),
    Url_imagen varchar(max),
    Edad tinyint not null
)

CREATE TABLE Hijos(
    Id int primary key identity(1,1),
    Nombre_completo varchar(200) not null,
    Fecha_nacimiento Date not null,
    Edad tinyint not null,
    Trabaja bit not null,
    id_cliente int not null,
    FOREIGN KEY (id_cliente) REFERENCES Clientes(Id)
)
-------------------------------------------------------------------------------



------------------------------- LOGICA DE SOLICITUD ----------------------------

CREATE TABLE Solicitudes(
    Id int primary key identity(1,1),
    id_cliente int not null,
    Fecha DateTime DEFAULT CURRENT_TIMESTAMP, 
    Aprobado bit not null,
    id_plan_financiamiento int null,
    FOREIGN KEY (id_cliente) REFERENCES Clientes(Id),
    FOREIGN KEY (id_plan_financiamiento) REFERENCES Planes_financiamientos(Id)
)









----------------------- FIXES ---------------------------------
ALTER TABLE Marcas ADD Url_imagen varchar(max);

ALTER TABLE Clientes ADD id_estado_civil int

ALTER TABLE Clientes
ADD FOREIGN KEY (id_estado_civil) REFERENCES Estados_civiles(Id);