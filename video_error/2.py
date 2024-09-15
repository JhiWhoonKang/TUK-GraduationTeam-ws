import socket
import numpy as np
import cv2

# UDP 서버 설정
UDP_IP = "127.0.0.1"  # 모든 IP에서 수신
UDP_PORT = 12345
MAX_LENGTH = 65000  # 최대 패킷 크기 설정

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind((UDP_IP, UDP_PORT))

print("UDP receiver is running...")

buffer = bytearray()  # 수신한 데이터를 저장할 버퍼

while True:
    # 클라이언트로부터 데이터 수신
    data, addr = sock.recvfrom(MAX_LENGTH)  # 최대 UDP 패킷 크기

    # 수신한 데이터를 버퍼에 추가
    buffer.extend(data)

    # 데이터 크기 확인 (여기서는 단순히 buffer의 크기를 체크)
    if len(buffer) >= 921600:  # 640 x 480 x 3 (BGR)
        # 프레임 재구성
        frame = np.frombuffer(buffer[:921600], dtype=np.uint8).reshape((480, 640, 3))
        
        # 나머지 데이터는 버퍼에서 제거
        buffer = buffer[921600:]

        # 영상 출력
        cv2.imshow("Received Frame", frame)
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

sock.close()
cv2.destroyAllWindows()
