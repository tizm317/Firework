# Firework
Graphics Team Project - PC game : Firework 불꽃놀이 게임 /
3-2학기 그래픽스 팀프로젝트 과제 /
4인 제작 PC 게임

**[게임명] : 불꽃놀이 게임**

**[제작 기간] : 2021/11/06 ~ 2021/11/30**
 
**[제작엔진 / 언어] : Unity / C#**

**[역할]**
  - UI 작업 : 타이머 / 거정 점령 / 에임 / 키 설명
  - 오브젝트에 shader 적용
    - 건물, 비행체 : 유니티 쉐이더 그래프(metalic, dissolve 효과)
    - 호수 : HLSL 쉐이더 코딩
  - 맵 제작 : 유니티 Terrain 활용
  - 점령 시스템
 
**[코드 요약]**
- Assets/Scripts/이호영/Scripts 폴더 내 1,2,3번 코드
- Assets/Scripts/이호영/Shader 폴더 내 4번 코드

**1. Timer**
  - 타이머
 
**2. lineRendererTest**
  - 점령 구역 표시 레이저
 
**3. conquest**
  - 점령 시스템
    - 플레이어와 거점 거리 계산 후 범위 내 위치 시 게이지 상승
    - 거점에서 벗어날 시 게이지 하강
    - 점령 완료 시 다음 거점 활성화
    - 점령지 레이저 색 변경
  - 점령 게이지 UI

**4. Water_transparent.shader**
  - 호수 물 쉐이더
 
**[제안서] : https://drive.google.com/file/d/1wsFwS-VyZS9rfg8xapuPuLjpPoO-I2SZ/view?usp=sharing**

**[결과 보고서] : https://docs.google.com/presentation/d/1drT0hjvvetkPvx_nuvuOdQSo6wZeC3gmTfu7Q5sVUZI/edit?usp=sharing**

**[게임 영상] : https://www.youtube.com/watch?v=ViY9VuqP8jI&ab_channel=%EC%9D%B4%ED%9D%90%EC%97%89**
