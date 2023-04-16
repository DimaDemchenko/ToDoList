create database db_todolist;
use db_todolist;

CREATE TABLE Tasks (
    id INT PRIMARY KEY IDENTITY(1, 1),
    category_id INT NOT NULL,
    title VARCHAR(255) NOT NULL,
    deadline DATETIME,
    is_completed BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (category_id) REFERENCES Categories(id)
);

CREATE TABLE Categories (
    id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255) NOT NULL,
);

INSERT INTO Categories Values('Home');
INSERT INTO Categories Values('Busines');
INSERT INTO Categories Values('Study');
INSERT INTO Categories Values('Hobby');
INSERT INTO Categories Values('Work');
INSERT INTO Tasks Values(1, 'HomeTask', '2023-04-30 15:30:45', 0);

select title, deadline, is_completed, name from Categories
join Tasks on Categories.id = Tasks.category_id

