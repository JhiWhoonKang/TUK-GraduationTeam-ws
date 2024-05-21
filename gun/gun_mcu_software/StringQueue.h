#include <Arduino.h>

struct Command {
  String data;
  bool used;
};

class CommandQueue {
  private:

    Command* queueArray;
    usb_serial_class *serial;
    int capacity;
    int front;
    int rear;
    int count;

    void resize(int newSize) {
      Command* newQueue = new Command[newSize];
      int currentSize = min(count, newSize);
      for (int i = 0; i < currentSize; i++) {
        newQueue[i] = queueArray[(front + i) % capacity];
      }
      capacity = newSize;
      delete[] queueArray;
      queueArray = newQueue;
      front = 0;
      rear = count - 1;
    }

  public:
    CommandQueue(int size = 10, usb_serial_class *_serial = &Serial) {
      serial = _serial;
      capacity = size;
      queueArray = new Command[capacity];
      front = 0;
      rear = -1;
      count = 0;
    }

    ~CommandQueue() {
      delete[] queueArray;
    }

    void push(String item) {
      if (count == capacity) {
        resize(capacity * 2); // 큐가 꽉 찼을 때 크기를 2배로 증가
      }
      rear = (rear + 1) % capacity;
      queueArray[rear].data = item;
      queueArray[rear].used = false;
      count++;
    }

    void pop() {
      if (count == 0) {
        return; // 큐가 비어 있으면 함수 종료
      }
      front = (front + 1) % capacity;
      count--;
    }

    int findIndex(String item) {
      for (int i = 0; i < count; i++) {
        int idx = (front + i) % capacity;
        if (queueArray[idx].data == item) {
          return i;
        }
      }
      return -1; // 데이터를 찾지 못한 경우
    }

    String useIndex(int index) {
      if (index < 0 || index >= count) {
        return ""; // 유효하지 않은 인덱스
      }
      int idx = (front + index) % capacity;
      queueArray[idx].used = true;
      return queueArray[idx].data;
    }

    String use() {
      for (int i = 0; i < count; i++) {
        int idx = (front + i) % capacity;
        if (!queueArray[idx].used) {
          queueArray[idx].used = true;
          return queueArray[idx].data;
        }
      }
      return ""; // 사용되지 않은 데이터가 없는 경우
    }

    void popIndex(int index) {
      if (index < 0 || index >= count) {
        return; // 유효하지 않은 인덱스
      }
      for (int i = index; i < count - 1; i++) {
        int idx = (front + i) % capacity;
        queueArray[idx] = queueArray[(idx + 1) % capacity];
      }
      rear = (rear - 1 + capacity) % capacity;
      count--;
      // serial->println("Delete");
    }

    int size() {
      return count;
    }

    // 전체 큐의 내용을 출력하는 함수
    void printQueue() {
      serial->println("Queue Contents:");
      for (int i = 0; i < count; i++) {
        int idx = (front + i) % capacity;
        serial->print("Data: ");
        serial->print(queueArray[idx].data);
        serial->print(", Used: ");
        serial->println(queueArray[idx].used ? "true" : "false");
      }
    }
};