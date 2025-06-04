from flask import Flask, request, jsonify
import threading
import json
import os

app = Flask(__name__)
lock = threading.Lock()

BACKUP_FILE = "ranking_backup.json"

ranking = {
    "map1": [],
    "map2": []
}

def save_to_file():
    with open(BACKUP_FILE, "w") as f:
        json.dump(ranking, f, indent=4)

def load_from_file():
    global ranking
    if os.path.exists(BACKUP_FILE):
        with open(BACKUP_FILE, "r") as f:
            data = json.load(f)
            
        if all(item.get("time", 0) <= 0 for item in data.get("map1", [])):
            data["map1"] = []
        if all(item.get("time", 0) <= 0 for item in data.get("map2", [])):
            data["map2"] = []

        ranking = data  

@app.route('/submit', methods=['POST'])
def submit():
    data = request.json
    print("Received:", data) # log 확인용
    name = data['name']
    time = data['time']
    map_num = data['map']
    key = f"map{map_num}"

    if time <= 0:
        return jsonify({'status': 'ignored (time <= 0)'})

    with lock:
        ranking[key].append({'name': name, 'time': time})
        ranking[key] = sorted(ranking[key], key=lambda x: x['time'])[:10]
        save_to_file()

    return jsonify({'status': 'ok'})

@app.route('/ranking/<int:map_num>', methods=['GET'])
def get_ranking(map_num):
    key = f"map{map_num}"
    return jsonify(ranking[key])

if __name__ == '__main__':
    load_from_file()
    app.run(host='0.0.0.0', port=8080)
