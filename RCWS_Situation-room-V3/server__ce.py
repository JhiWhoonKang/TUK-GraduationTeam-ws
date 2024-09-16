import cv2
import socket
import math

max_length = 65000
host = "127.0.0.1"
port = 9000

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind((host, port))

cap = cv2.VideoCapture(1)

while True:
    ret, frame = cap.read()
    if ret:
        retval, buffer = cv2.imencode(".jpg", frame)
        if retval:
            buffer = buffer.tobytes()
            buffer_size = len(buffer)

            num_of_packs = math.ceil(buffer_size / (max_length - 2))  # 패킷 번호를 위해 2바이트 남김

            left = 0

            for i in range(num_of_packs):
                right = min(left + (max_length - 2), buffer_size)  # 마지막 패킷의 크기 조정
                data = bytearray(buffer[left:right])
                data.insert(0, num_of_packs)  # 전체 패킷 수
                data.insert(1, i)  # 현재 패킷 번호

                try:
                    sock.sendto(data, (host, 8000))
                    print(f'Sent packet {i + 1}/{num_of_packs}, size: {len(data)} bytes')
                except Exception as e:
                    print(f'Error sending packet {i}: {e}')

                left = right
