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
        # 기존 기록 중 동일 nickname이 있는지 확인
        existing_index = next((i for i, r in enumerate(ranking[key]) if r['name'] == name), None)

        if existing_index is not None:
            old_time = ranking[key][existing_index]['time']
            if time < old_time:
                ranking[key][existing_index]['time'] = time  # 더 빠르면 갱신
                print(f"Updated {name}'s record: {old_time} → {time}")
            else:
                print(f"Ignored slower time for {name}: {time} ≥ {old_time}")
                return jsonify({'status': 'ignored (slower time)'})
        else:
            # 새로운 이름이면 그냥 추가
            ranking[key].append({'name': name, 'time': time})
            print(f"Added new record for {name}: {time}")
            
        save_to_file()

    return jsonify({'status': 'ok'})

@app.route('/ranking/<int:map_num>', methods=['GET'])
def get_ranking(map_num):
    key = f"map{map_num}"
    nickname = request.args.get("nickname")  # 쿼리에서 nickname 받기

    with lock:
        # 중복 닉네임 제거: 가장 빠른 기록만 유지
        unique = {}
        for r in ranking.get(key, []):
            name = r["name"]
            time = r["time"]
            if name not in unique or time < unique[name]["time"]:
                unique[name] = {"name": name, "time": time}

        # 정렬
        sorted_list = sorted(unique.values(), key=lambda x: x["time"])

        # 랭킹 번호 부여
        ranked = [{"rank": i + 1, "name": r["name"], "time": r["time"]} for i, r in enumerate(sorted_list)]

        # 상위 10명 추출
        top10 = ranked[:10]

        # 닉네임이 있다면, 내 기록을 찾아서 추가 (단, 중복이면 제외)
        if nickname:
            my_record = next((r for r in ranked if r["name"] == nickname), None)
            if my_record and all(r["name"] != nickname for r in top10):
                top10.append(my_record)  # 내 기록이 top10에 없을 때만 추가

    return jsonify(top10)

if __name__ == '__main__':
    load_from_file()
    app.run(host='0.0.0.0', port=8080)
