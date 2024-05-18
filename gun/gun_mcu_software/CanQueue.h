#include <Arduino.h>

// 8바이트 데이터와 데이터 길이를 저장할 구조체
struct DataItem {
  uint8_t data[8]; // 8바이트 데이터
  uint8_t len;     // 데이터 길이
};

// DataItem을 저장하는 큐 클래스
class DataQueue {
  private:
    DataItem* queueArray; // 데이터 항목을 저장할 배열
    int capacity;         // 큐의 최대 용량
    int front;            // 큐의 맨 앞 인덱스
    int rear;             // 큐의 맨 뒤 인덱스
    int count;            // 큐에 저장된 항목 수

  public:
    DataQueue(int size = 10) {
      capacity = size;
      queueArray = new DataItem[capacity];
      front = 0;
      rear = -1;
      count = 0;
    }

    ~DataQueue() {
      delete[] queueArray;
    }

    // 큐에 데이터 항목 추가
    void push(DataItem item) {
      if (count < capacity) {
        rear = (rear + 1) % capacity;
        for(int i = 0; i < item.len; i++) {
          queueArray[rear].data[i] = item.data[i];
        }
        queueArray[rear].len = item.len;
        count++;
      }
    }

    // 큐에서 맨 앞의 데이터 항목 제거 및 반환
    bool pop(DataItem &item) {
      if (count > 0) {
        item = queueArray[front];
        front = (front + 1) % capacity;
        count--;
        return true;
      }
      return false; // 큐가 비어있을 경우 빈 DataItem 반환
    }

    bool use(DataItem &item) {
      if (count > 0) {
        item = queueArray[front];
        return true;
      }
      return false; // 큐가 비어있을 경우 빈 DataItem 반환
    }

    // 큐가 비어있는지 확인
    bool isEmpty() {
      return count == 0;
    }

    // 큐의 현재 크기 반환
    int size() {
      return count;
    }
};