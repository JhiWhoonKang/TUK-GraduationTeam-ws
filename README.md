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
> ## 목표 성능
> 10 m 거리에서 400 x 400 mm 표적을 맞추는 것을 목표로 합니다.
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
> ## 주요 기능
> (공란)
> ## 기대효과
> #### 1. 초병 화력 지원
> 추가 인원 투입 없이 초병의 화력을 지원하여 병사의 생존성을 향상시킬 수 있습니다.
> #### 2. 신속한 상황 판단 및 보고체계
> 초병과 RCWS의 유기적인 작전 수행을 통해 신속한 상황 판단 및 보고 체계를 구축할 수 있습니다.
> #### 3. 초기 대응 가능
> 현장 지원 병력이 도착하기 전에 초기 대응을 가능하게 하여, 경계 작전시 초동조치를 강화할 수 있습니다.


<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
## 시작하기

이 섹션에서는 프로젝트를 로컬 컴퓨터에서 실행하기 위한 사전 준비사항과 설치 방법을 안내합니다.

### 전제 조건

프로젝트를 시작하기 전에 아래의 환경이 준비되어 있어야 합니다.

- Ubuntu 20.04
- Python, C#, C/C++
- Arduino IDE, VSCode, Visual Studio C# .NET Windows Form
- SolidWorks (설계 도구)

### 설치

프로젝트를 시작하기 위해 필요한 라이브러리를 설치하려면 아래의 명령어를 실행하세요.

```bash
pip install pyserial
```


# TUK-GraduationTeam-ws

This project aims to develop software and hardware for remotely controlling weapon stations.

## Getting Started

This section guides you through getting the project up and running on your local machine for development and testing purposes.

### Prerequisites

Before you start, ensure you have the following environment set up:

- Ubuntu 20.04
- Python, C#, C/C++
- Arduino IDE, VSCode, Visual Studio C# .NET Windows Form
- SolidWorks (for design purposes)

### Installation

To install the necessary libraries for the project, run the following command:

```bash
pip install pyserial
