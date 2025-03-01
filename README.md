# TUK-GraduationTeam-ws
'까불면 메카 약이다'팀 협업 공간입니다.
<br/>
<h1 align="center"> TUK 메카트로닉스공학과 2025 졸업 작품 </h1>
<h2 align="center"> 초소 유무인 복합 무기 조종 시스템 🪖  </h2>
<p align="right"> 정지우(팀장), 강지훈 고경륜 박성(팀원)</p>

# 개발 목표 <!-- omit in toc -->
![IMPORTANT](https://img.shields.io/badge/SUMMARY-ffcc00?style=for-the-badge&logoColor=black)
> 본 프로젝트는 원격에서 초소에 배치된 무기 시스템을 제어하기 위한 소프트웨어 및 하드웨어 개발을 목표로 합니다.

![IMPORTANT](https://img.shields.io/badge/DETAILS-00CC66?style=for-the-badge&logoColor=black)
> ## 필요성
> 초소 유무인 복합 무기 조종 시스템이란 **기존 유인 초소 시스템에서 RCWS를 적용한 유무인 복합 무기 조종 시스템**을 의미합니다.
> #### 1. 방어 무기 수요 증가
> 전세계적으로 신냉전 시대에 접어들면서 전쟁이 빈번히 발생하고 있습니다. 이로 인해 방어 무기의 수요가 증가하고 있습니다.
> #### 2. 대한민국의 급격한 저출산
> 저출산에 따른 군병력 감소로 인해 군단 및 사단이 통폐합이 진행되고 있으며 이로 인해 소초당 책임지는 작전 반경 범위가 증가하였습니다.
> #### 3. 유·무인 복합전투체계 단계별 전환
> 과학기술 발전으로 전쟁 양상이 변화하는 현 시점에서 이에 대한 중요성을 인지하고 전투 효율성 향상을 목표로 합니다.
> #### 4. 장단점 보완
> 기존 유인 초소 시스템 및 무인 초소 시스템 각각의 단점을 보완하고 장점을 극대화합니다.

# 개발 기간 <!-- omit in toc -->
> 2023.09. ~ 2024.12.

# 개발 내용 <!-- omit in toc -->
> ## 핵심 구현 기능
> #### 1. 원격 제어
> 상황실에서도 초소에 있는 RCWS를 제어할 수 있습니다.
> #### 2. 레이저 트래킹
> 초병의 병기에 부착되어 있는 레이저 사이트를 이용해 레이저를 조사하면 RCWS 광학부에서 이를 포착하고 트래킹을 합니다.
> #### 3. 휴먼 트래킹
> RCWS 광학부에서 사람이 인식될 경우 사용자 판단 하에 트래킹을 활성화할 수 있습니다.
> #### 4. 자동조준
> ToF 센서를 통해 목표 지점까지의 거리를 측정한 후 RCWS 기총을 제어하여 목표물을 조준합니다.

# 개발 결과 <!-- omit in toc -->
## 하드웨어 제원
> ### 📌 주요 성능  
> | 항목 | 사양 |
> |------|------|
> | **작동 회전 속도** | 최대 67.96˚/sec (고저/선회) |
> | **회전 정밀도** | 최대 ±0.0075˚ |
> | **회전 각도** | 베이스 ±180˚ / 광학부 ±45˚ / 기총부 -30˚ ~ +45˚ |
> | **조준 속도** | 최대 1.5275 sec |
> | **탐지 및 조준 속도** | 최대 3.6328 sec (사람 및 레이저 트래킹) |
> | **조준 거리** | 3 ~ 40 m |
> | **인식 가능 인원** | 최대 6명 (사람 인식) |
> | **레이저 인식 거리** | 최대 40 m |
> ### 📌 광학 성능  
> | 배율 단계 | 카메라 배율 | 화각 (FOV) |
> |-----------|------------|------------|
> | **1단계** | 4.5배율 | 13.884˚ |
> | **2단계** | 4.5배율 | 13.774˚ |
> | **3단계** | 5.7배율 | 10.428˚ |
> | **4단계** | 8.2배율 | 7.317˚ |
> | **5단계** | 8.2배율 | 7.383˚ |
## 수상 내역
> | 상훈명 | 수여기관 | 수상일자 | 수상내역 |
> |------|------|------|------|
> | **ICROS(제어로봇시스템학회) 학부생 논문 경진대회** | **ICROS** | **2024-07-24** | **학부생 논문상** |
> | **한국공학대전** | **한국공학대학교** | **2024-09-25** | **한국산업기술평가관리원장상** |
> | **2024 창의적 종합설계 경진대회** | **한양대학교 공학교육혁신센터** | **2024-11-12** | **우수상((한국산업기술진흥원장상))** |
> | **2024 로봇 제작 경진대회 SHARE Challenge** | **한국연구재단** | **2024-11-20** | **최우수상(한국연구재단 이사장상)** |
> | **2024 창의적 종합설계 경진대회** | **-** | **2024-11-22** | **수상(한국공과대학장협의회장)** |
> ## 기대효과

