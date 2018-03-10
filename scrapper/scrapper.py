from bs4 import BeautifulSoup
import psycopg2
import urllib.request
from sys import argv


def read_file(file_name):
    file = open(file_name, "r")
    team_ids = []
    for team_id in file:
        team_ids.append(team_id)
    return team_ids


def scrape(id):
    url = 'http://stats.ncaa.org/teams/' + id
    user_agent = 'Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.7) Gecko/2009021910 Firefox/3.0.7'
    headers = {'User-Agent': user_agent}

    request=urllib.request.Request(url, None, headers)
    response = urllib.request.urlopen(request)
    data = response.read() # The data u need


    soup = BeautifulSoup(data, "html.parser")

    lst =[]

    for txt in soup.find_all('a'):
        lst.append(txt.get_text())

    print(type(lst[5]))
    team_name = lst[5]
    team_id = set_team_key(team_name)
    tbls = soup.find_all('table', attrs={'class': 'mytable'}, limit=2)

    rows =[]

    for row in tbls[1].find_all('tr'):
        cells = []
        for cell in row.find_all('td'):
            txt = cell.text
            txt = txt.strip()
            print(txt)
            cells.append(txt)
        rows.append(cells)

    # for rauw in rows[1:]:
    #     if len(rauw) is 3:
    #         temp = rauw[2]
    #         rauw[2] = temp[7:]
    #         rauw[2] = rauw[2]
    #         print(rauw[2])

    return team_name, rows


def set_team_key(team_name):
    conn = psycopg2.connect(host="192.168.0.5", port=5432, database="basketball", user="postgres", password="password")
    cursor = conn.cursor()
    query = r"Insert Into teams(name) VALUES ('{0}') RETURNING id;".format(team_name)
    cursor.execute(query)
    id = conn.commit()
    return id


def write_to_db(rows):
    conn = psycopg2.connect(host="192.168.0.5", port=5432, database="basketball", user="postgres", password="password")
    cursor = conn.cursor()

    query = "Insert Into stats(scoring_offense, scoring_defense, scoring_margin, rebound_margin, assists, blocks, turnover_margin, assist_turnover_ratio, fg_percentage, fg_percentage_defense, three_pointers, three_pointers_percent, ft_percent, wl_percent, team_id) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}');".format(rows[1][2][2], rows[1][3][2], rows[1][4][2], rows[1][5][2], rows[1][6][2], rows[1][7][2], rows[1][8][2], rows[1][9][2], rows[1][10][2], rows[1][11][2], rows[1][12][2], rows[1][13][2], rows[1][14][2], rows[1][15][2], str(rows[0]))
    print(query)
    cursor.execute(query)
    #query = "Insert Into stats(assist_turnover_ratio, fg_percentage, fg_percentage_defense, three_pointers, three_pointers_percent, ft_percent, wl_percent, team_id) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');".format(rows[1][10][2], rows[1][11][2], rows[1][12][2], rows[1][13][2], rows[1][14][2], rows[1][15][2], rows[1][15][2], str(rows[0]))
    #print(query)
    #cursor.execute(query)
    conn.commit()
    print(rows[0])

if __name__ == '__main__':
    args = argv[1]
    team_ids = read_file(args)
    for id in team_ids:
        team = scrape(id)
        write_to_db(team)

