import socket
import numpy as np
import cv2

# UDP 서버 설정
UDP_IP = "127.0.0.1"  # 모든 IP에서 수신
UDP_PORT = 12347
MAX_LENGTH = 65000  # 최대 패킷 크기 설정

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind((UDP_IP, UDP_PORT))

print("UDP receiver is running...")

buffer = bytearray()  # 수신한 데이터를 저장할 버퍼

while True:
    # 클라이언트로부터 데이터 수신
    data, addr = sock.recvfrom(MAX_LENGTH)  # 최대 UDP 패킷 크기

    # 수신한 데이터를 버퍼에 추가
    #buffer += data  # 데이터 복사하여 새로운 메모리 공간에 저장
    buffer =buffer+data

    # JPEG 이미지가 수신되었는지 확인
    if len(buffer) > 0:
        # 버퍼를 NumPy 배열로 변환
        nparr = np.frombuffer(buffer, np.uint8)
        
        # JPEG 이미지를 디코딩
        frame = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

        if frame is not None:
            # 영상 출력
            cv2.imshow("Received Frame", frame)
            buffer = bytearray()  # 버퍼를 새롭게 초기화 (다음 프레임을 위해)
            #buffer.clear()  # 버퍼를 비웁니다 (다음 프레임을 위해)

            if cv2.waitKey(1) & 0xFF == ord('q'):
                break

sock.close()
cv2.destroyAllWindows()