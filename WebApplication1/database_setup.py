import subprocess
import sys
import getpass
import random

try:
    import mysql.connector
except ImportError:
    print("mysql-connector-python not found. Installing...")
    subprocess.check_call([sys.executable, "-m", "pip", "install", "mysql-connector-python"])
    import mysql.connector

password = getpass.getpass(prompt='entr your MySQL root password: ')

try:
    conn = mysql.connector.connect(
        host="localhost",
        user="root",
        password=password
    )
    cursor = conn.cursor()

    cursor.execute("CREATE DATABASE IF NOT EXISTS UserManagement;")
    cursor.execute("USE UserManagement;")

    cursor.execute("""
    CREATE TABLE Users (
        user_id INT AUTO_INCREMENT PRIMARY KEY,
        name VARCHAR(255) NOT NULL,
        email VARCHAR(255) NOT NULL UNIQUE,
        creation_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    );
    """)

    cursor.execute("""
    CREATE TABLE Leaderboard (
        user_id INT PRIMARY KEY,
        max_score INT NOT NULL,
        rank_position INT,
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
    );
    """)

    users = [
        ("Alice", "alice@gmail.com"),
        ("Bob", "bob@gmail.com"),
        ("Charlie", "charlie@gmail.com"),
        ("David", "david@gmail.com"),
        ("Eve", "eve@gmail.com")
    ]

    cursor.executemany("INSERT INTO Users (name, email) VALUES (%s, %s)", users)
    conn.commit()

    scores = [(1, 85, 1), (2, 70, 2), (3, 90, 1), (4, 60, 3), (5, 75, 2)]
    cursor.executemany("INSERT INTO Leaderboard (user_id, max_score, rank_position) VALUES (%s, %s, %s)", scores)
    conn.commit()

    cursor.execute("CREATE DATABASE IF NOT EXISTS WordsManagement;")
    cursor.execute("USE WordsManagement;")

    cursor.execute("""
    CREATE TABLE CorrectWords (
        word_id INT AUTO_INCREMENT PRIMARY KEY,
        word VARCHAR(255) NOT NULL UNIQUE,
        frequency INT DEFAULT 0
    );
    """)

    cursor.execute("""
    CREATE TABLE IncorrectWords (
        word_id INT AUTO_INCREMENT PRIMARY KEY,
        word VARCHAR(255) NOT NULL UNIQUE,
        frequency INT DEFAULT 0
    );
    """)

    correct_words = [
        ("benevolent", random.randint(1, 3)),
        ("candid", random.randint(1, 3)),
        ("diligent", random.randint(1, 3)),
        ("emulate", random.randint(1, 3)),
        ("fidelity", random.randint(1, 3)),
        ("genuine", random.randint(1, 3)),
        ("humble", random.randint(1, 3)),
        ("integrity", random.randint(1, 3)),
        ("jovial", random.randint(1, 3)),
        ("keen", random.randint(1, 3))
    ]

    cursor.executemany("INSERT INTO CorrectWords (word, frequency) VALUES (%s, %s)", correct_words)
    conn.commit()

    incorrect_words = [
        ("absentmindedness", random.randint(1, 3)),
        ("belligerent", random.randint(1, 3)),
        ("chaotic", random.randint(1, 3)),
        ("deceitful", random.randint(1, 3)),
        ("egocentric", random.randint(1, 3)),
        ("fallacious", random.randint(1, 3)),
        ("grumpy", random.randint(1, 3)),
        ("hasty", random.randint(1, 3)),
        ("inconsistent", random.randint(1, 3)),
        ("jaded", random.randint(1, 3))
    ]

    cursor.executemany("INSERT INTO IncorrectWords (word, frequency) VALUES (%s, %s)", incorrect_words)
    conn.commit()

    cursor.close()
    conn.close()
    print("Databases and tables created, and sample data inserted successfully.")

except mysql.connector.Error as err:
    print(f'Error: {err}')