import psycopg2
from psycopg2.extensions import ISOLATION_LEVEL_AUTOCOMMIT


def create_db(conn):
    cursor = conn.cursor()
    cursor.execute("Create Database Basketball")
    cursor.close()


def create_table(conn):
    cursor = conn.cursor()
    cursor.execute("Create Table teams(id SERIAL PRIMARY KEY, name TEXT);")
    cursor.execute("Create Table stats(id SERIAL PRIMARY KEY, scoring_offense TEXT, scoring_defense TEXT, scoring_margin TEXT, rebound_margin TEXT, assists TEXT, blocks TEXT, steals TEXT,turnover_margin TEXT, assist_turnover_ratio TEXT, fg_percentage TEXT, fg_percentage_defense TEXT, three_pointers TEXT, three_pointers_percent TEXT, ft_percent TEXT, wl_percent TEXT, team_id TEXT );")
    cursor.close()

if __name__ == '__main__':
    conn = psycopg2.connect(host="192.168.0.5", port=5432, database="postgres", user="postgres", password="")
    conn.set_isolation_level(ISOLATION_LEVEL_AUTOCOMMIT)
   # create_db(conn)
    print("Database created")
    conn = psycopg2.connect(host="192.168.0.5", port=5432, database="basketball", user="postgres", password="")
    conn.set_isolation_level(ISOLATION_LEVEL_AUTOCOMMIT)
    create_table(conn)
    print("Table created")
    conn.close()
