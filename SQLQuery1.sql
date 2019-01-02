drop table naprawy;
drop table tankowania;
drop table czesci;
drop table hangary;
drop table statki;
drop table modele;
drop table serwisy;

create table Czesci(
	id int identity(1,1) not null primary key,
	typ varchar(20) not null,
	producent varchar(20) not null
);

create table Hangary(
	kolor varchar(10) not null primary key
);

create table Statki(
	id int identity(1,1) not null primary key,
	rocznik int not null
);

create table Modele(
	id int identity(1,1) not null primary key,
	nazwa varchar(15) not null,
	marka varchar(10) not null,
	rodzaj varchar(20) not null
);

create table Serwisy(
	id int identity(1,1) not null primary key,
	klucze bit not null,
	wiertarki bit not null,
	srubokrety bit not null,
	smary int not null,
	sruby int not null
)

create table Tankowania(
	id int identity(1,1) not null primary key,
	litry int not null,
	id_statku int not null foreign key references Statki(id),
	rodzaj_paliwa varchar(20) not null,
	czas_tankowania real not null,
	czas_oczekiwania real not null
);

create table Naprawy(
	id int identity(1,1) not null primary key,
	koszt int not null,
	czas real not null,
	cena int not null,
	id_inzyniera int not null,
	numer_platnosci_kliB int not null,
	data date not null,
	id_statku int not null foreign key references Statki(id),
	id_czesci int not null foreign key references Czesci(id),
	kolor_hangaru varchar(10) not null foreign key references Hangary(kolor),
	id_modelu int not null foreign key references Modele(id),
	id_serwisu int not null foreign key references Serwisy(id)
);