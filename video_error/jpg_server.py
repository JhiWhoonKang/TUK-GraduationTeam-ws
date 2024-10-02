import socket
import cv2
import numpy as np
import math

# UDP 클라이언트 설정
UDP_IP = "127.0.0.1"  # 서버 IP 주소 (수신 측)
UDP_PORT = 12345
MAX_LENGTH = 65000  # 최대 패킷 크기 설정

# 카메라 열기
cap = cv2.VideoCapture(1)

# UDP 소켓 생성
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

while True:
    ret, frame = cap.read()
    if not ret:
        break

    # 프레임을 JPEG로 인코딩
    retval, buffer = cv2.imencode('.jpeg', frame)
    if retval:
        # 인코딩된 데이터의 크기 계산
        buffer = buffer.tobytes()
        buffer_size = len(buffer)

        print(f"Sending data size: {buffer_size} bytes")

        # 데이터가 MAX_LENGTH를 초과하는 경우 나누어 전송
        if buffer_size > MAX_LENGTH:
            num_of_packs = math.ceil(buffer_size / MAX_LENGTH)
            left = 0
            right = MAX_LENGTH

            for i in range(num_of_packs):
                packet_data = buffer[left:right]
                sock.sendto(packet_data, (UDP_IP, UDP_PORT))

                left = right
                right += MAX_LENGTH
                # 마지막 패킷 처리
                if left >= buffer_size:
                    break
        else:
            # 데이터가 MAX_LENGTH 이하일 경우 한 번에 전송
            sock.sendto(buffer, (UDP_IP, UDP_PORT))

# 리소스 해제
cap.release()
sock.close()
