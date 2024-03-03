/*
 * vfd_driver.c
 *
 * Created: 23.04.2023 16:53:37
 *  Author: Ivan Prints
 */ 

#ifndef UART_HAL_H_
#define UART_HAL_H_


#include <stdint.h>
#include "../config.h"
#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>
#include <stdbool.h>
#include "vfd_driver.h"
#include "rtc.h"


#define  TIME_SYNC_REQUEST		1
#define  DATE_SYNC_REQUEST		2
#define  TEXT_SYNC_REQUEST		3
#define  ERROR_SYNC_REQUEST		4






#define RX_BUFFER_SIZE 128

bool serial_complete(void);
const char* serial_read_data(void);
void uart_init(uint32_t baud,uint8_t high_speed);
void uart_send_byte(uint8_t c);
void uart_send_array(uint8_t *c,uint16_t len);
void uart_send_string(uint8_t *c);
uint16_t uart_read_count(void);
uint8_t uart_read(void);
uint8_t uart_flush(void);
uint8_t parseString(const char* input, display* output, rtc_date* newDate);


#endif /* UART_HAL_H_ */