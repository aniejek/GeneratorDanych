drop table naprawy;
drop table tankowania;
drop table czesci;
drop table hangary;
drop table statki;

create table Czesci(
	id int not null primary key,
	typ varchar(20) not null,
	producent varchar(15) not null
);

create table Hangary(
	kolor varchar(10) not null primary key
);

create table Statki(
	id int not null primary key,
	model varchar(25) not null,
	rocznik int not null
);

create table Tankowania(
	id int not null primary key,
	litry int not null,
	id_statku int not null foreign key references Statki(id),
	rodzaj_paliwa varchar(20) not null,
	czas_tankowania int not null,
	czas_oczekiwania int not null
);

create table Naprawy(
	id int not null primary key,
	koszt int not null,
	czas int not null,
	cena int not null,
	id_inzyniera int not null,
	numer_platnosci_kliB int not null,
	data date not null,
	id_statku int not null foreign key references Statki(id),
	id_czesci int not null foreign key references Czesci(id),
	kolor_hangaru varchar(10) not null foreign key references Hangary(kolor)
);