# 🏎️ 분노의 질주: 짱구의 탈출 대작전

**"저한테 좋은 생각이 있어요!"**  
 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; - 짱구는 못말려 극장판: 폭풍을 부르는 정글 中 -

엉덩이로 세상을 구하는 짱구의 긴박한 탈출 레이싱 게임입니다.  
※ 본 게임에 등장하는 캐릭터, 음악, 효과음은 짱구는 못말려, 테일즈런너, 마리오 카트를 참고했으며 교육용의 목적으로 사용했습니다. ※

['분노의 질주: 짱구의 탈출 대작전' 플레이 링크](https://alpakar02.itch.io/2025swpp-team02) (실시간 랭킹 서버가 현재는 닫혀 있습니다.)

<img width="720" height="450" alt="image" src="https://github.com/user-attachments/assets/be8b6b1a-2d19-4c4e-ba34-0b25d82383dd" />

---

## 🛠 Skills & Tech Stack

프로젝트에 사용된 주요 기술 스택입니다.

### Development

![Unity](https://img.shields.io/badge/unity-%23ffffff.svg?style=for-the-badge&logo=unity&logoColor=black)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![Python](https://img.shields.io/badge/python-3670A0?style=for-the-badge&logo=python&logoColor=ffdd54)
![Flask](https://img.shields.io/badge/flask-%23000000.svg?style=for-the-badge&logo=flask&logoColor=white)

### Graphics & Art

![Blender](https://img.shields.io/badge/blender-%23F5792A.svg?style=for-the-badge&logo=blender&logoColor=white)

### Collaboration & DevOps

![Git](https://img.shields.io/badge/git-%23F05033.svg?style=for-the-badge&logo=git&logoColor=white)
![GitHub](https://img.shields.io/badge/github-%23121011.svg?style=for-the-badge&logo=github&logoColor=white)

---

## 👨‍👩‍👧‍👦 Contributors & Roles (2조는 못말려)

| 이름       | 담당 역할 및 기술적 기여                                                        |
| :--------- | :------------------------------------------------------------------------------ |
| **김태림** | 장애물 동작 로직 및 코드 구현, 장애물 배치, UI\UX 작업                          |
| **안지후** | 짱구 움직임 코드 구현 및 조작감 개선, 카메라 동선 배치 작업, 파티클 및 BGM 작업 |
| **이나경** | 짱구/원숭이 3D 모델링 및 애니메이션 제작                                        |
| **이준용** | 동굴/숲 맵 기획 및 제작, 실시간 랭킹 서버 구축, 기말 발표 및 자료 준비          |
| **임정빈** | 숲 맵 기획 및 제작, 중간 발표 및 자료 준비, 스프린트 보고서 및 최종 정리 문서   |

---

## 🎮 Game Overview

본 프로젝트는 **서울대학교 소프트웨어 개발의 원리와 실습** 교과목의 일환으로 제작되었습니다.  
남녀노소 누구나 쉽게 즐길 수 있는 캐주얼 게임으로, 짧은 플레이 타임과 유머 코드를 특징으로 합니다.

### 🍑 핵심 차별점: 엉덩이 추진력

- **속도 비례 애니메이션**: 짱구가 빠르게 이동할수록 엉덩이의 씰룩거림도 비례하여 빨라지도록 설정하여 시각적 재미를 극대화했습니다.
- **원작 고증 모델링**: Blender를 활용하여 짱구 특유의 눈썹과 눈, 엉덩이 모션을 자연스럽게 구현했습니다.

---

## 🗺️ Stages & Mechanics

### 1. 스테이지 구성

- **동굴 맵**: **어두컴컴한 동굴을 지나가요.** (어두운 조명과 좁은 경로를 통과하는 정교한 조작이 필요합니다.)
- **숲 맵**: **원숭이가 우글대는 숲을 지나가요** (울창한 나무와 바위, 움직이는 장애물이 배치된 야외 스테이지입니다. 스테이지 끝에 짱구가 좋아하는 것이 있습니다.)

### 2. 조작법 (Controls)

| 기능            | 키       | 설명                                                                 |
| :-------------- | :------- | :------------------------------------------------------------------- |
| **이동**        | `방향키` | 3인칭 시점으로 플레이어 이동                                         |
| **아이템 사용** | `(없음)` | 초코비 획득 시 자동 사용 (4초간 투명화/장애물 통과)                  |
| **위치 초기화** | `R`      | 장애물에 끼었을 때 눌러 카메라 위치로 순간이동                       |
| **메뉴/이전**   | `ESC`    | 이전 화면으로 넘어감. (인게임 중 누르면 스테이지 선택 화면으로 복귀) |

---

## 🛠️ Technical Challenges

### 1. Git Scene Conflict 해결

동일한 씬(Scene)에서 여러 명이 작업하며 발생하는 충돌을 방지하기 위해 다음 전략을 사용했습니다.

- **프리펩(Prefab)화**: 각자의 작업물을 프리펩으로 관리하여 씬이 날아가더라도 손쉽게 복구 가능하도록 설계했습니다.
- **작업 우선순위**: 충돌 시 작업량이 더 많은 팀원의 씬을 살리고 나머지는 재작업하는 방식으로 조율했습니다.

### 2. 실시간 랭킹 시스템

- **Stack**: Python Flask
- **Logic**: 동일 닉네임 유저의 기록 중 최단 기록만 저장하여 랭킹 도배를 방지했습니다.
- **UX**: 자신이 10위권 밖($> 10$)인 경우, 상위 10명과 함께 본인의 순위/최고 기록을 하단에 별도로 표시합니다.

---

## 🏃 게임 화면

### 시작 화면

- 시작하기: 스테이지 선택 화면으로 넘어갑니다. 유저 닉네임이 아직 설정되어 있지 않다면, 닉네임 설정 화면으로 넘어갑니다.
- 설정: 설정 화면으로 넘어갑니다.

<img width="720" height="450" alt="image" src="https://github.com/user-attachments/assets/0927d351-e411-4e1e-b657-4e2550cae4a5" />  
<br><br>

- 유저 변경: 닉네임 설정이 초기화 됩니다. (새로운 유저에게 전달할 떄 사용합니다.)
- 게임 종료: 게임이 종료됩니다.

<img width="720" height="450" alt="image" src="https://github.com/user-attachments/assets/c55b5e29-54b0-41dc-822b-c895cb65c45e" />
<br><br>

### 시작 화면 > 설정

- 볼륨: 슬라이드 바로 볼륨을 조절할 수 있습니다.
- 전체화면: 전체화면 ON/OFF 설정을 할 수 있습니다.
- IP: 서버 IP 주소를 입력하면 기록이 서버에 업로드되며, 랭킹을 볼 수 있습니다.
- 완료: 시작 화면으로 돌아갑니다.

<img width="720" height="450" alt="image" src="https://github.com/user-attachments/assets/1b2f1a1f-1c81-4e10-ac5c-f8834d933f57" />  
<br><br>

### 시작 화면 > 닉네임 설정

<img width="720" height="450" alt="image" src="https://github.com/user-attachments/assets/260edb10-9631-49e9-9210-42ddbad406f6" />
<br><br>

### 시작 화면 > 닉네임 설정 > 게임 설명

<img width="720" height="450" alt="image" src="https://github.com/user-attachments/assets/20c23fc3-a930-4fae-a500-15ab6160a8a4" />
<img width="720" height="450" alt="image" src="https://github.com/user-attachments/assets/1e7c50b9-2287-43c7-bd7b-92875f09a171" />
<img width="720" height="450" alt="image" src="https://github.com/user-attachments/assets/62a2054e-e0a1-40d9-9b55-978fee98b1e4" />

스테이지 선택으로 넘어갑니다.
<br><br>

### 시작 화면 (> 닉네임 설정 > 게임 설명) > 스테이지 선택

- 동굴/숲 맵을 선택하면 동굴/숲 맵이 시작됩니다.
  <img width="720" height="450" alt="image" src="https://github.com/user-attachments/assets/d0623b45-0a31-4486-9ff0-e92c9ff6ebc9" />

- 랭킹: 해당 맵의 랭킹이 표시됩니다.

**예시 1**: 기록이 10위권 안에 속한 경우  
<img width="720" height="450" alt="image" src="https://github.com/user-attachments/assets/272ddfe9-96b9-41e5-98a8-c0124de7687b" />

**예시 2**: 기록이 10위권 밖에 있는 경우  
<img width="720" height="450" alt="image" src="https://github.com/user-attachments/assets/952d4f61-345b-4f29-b815-5bf1e42b6d07" />
<br><br>

---

**2조는 못말려** | **Principles and Practices of Software Development, SNU**
