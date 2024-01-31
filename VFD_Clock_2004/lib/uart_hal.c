/*
 * vfd_driver.c
 *
 * Created: 23.04.2023 16:53:37
 *  Author: Ivan Prints
 */ 


//sprintf(char_array , "Counter: %d", buff_count);
//uart_send_string((char *)char_array);


#include "uart_hal.h"
#include "string.h"
#include <stdbool.h>



volatile static uint8_t rx_buffer[RX_BUFFER_SIZE] = {0};
volatile static uint16_t rx_count = 0;	
volatile static uint8_t uart_tx_busy = 1;
static uint16_t rx_read_pos = 0;
char test_array[256]="";
bool read_complete = true;



bool serial_complete(void)
{
	return read_complete;
}

const char* serial_read_data(void){
	//uint8_t serial_read_data(void){
	read_complete = false;
	rx_count=0;
	return test_array;
}

ISR(USART_RXC_vect){
	
	//volatile static uint16_t rx_write_pos = 0;
	test_array[rx_count] = UDR;
	if (test_array[rx_count] == 0x0D){
		read_complete = true;
		test_array[rx_count+1] = 0;
	}
	rx_count +=1;
	
}

//memset(rx_buffer, 0, sizeof(rx_buffer));

ISR(USART_TXC_vect){
	uart_tx_busy = 1;
}


void uart_init(uint32_t baud,uint8_t high_speed){
	
	uint8_t speed = 16;
	
	if(high_speed != 0){
		speed = 8;
		UCSRA |= 1 << U2X;
	}
	
	baud = (F_CPU/(speed*baud)) - 1;
	
	UBRRH = (baud & 0x0F00) >> 8;
	UBRRL = (baud & 0x00FF);
	
	UCSRB |= (1 << TXEN) | (1 << RXEN) | (1 << RXCIE) | (1 << TXCIE);
	UCSRC=0b10000110;
}


void uart_send_byte(uint8_t c){
	while(uart_tx_busy == 0);
	uart_tx_busy = 0;
	UDR = c;
}

void uart_send_array(uint8_t *c,uint16_t len){
	for(uint16_t i = 0; i < len;i++){
		uart_send_byte(c[i]);
	}
}

void uart_send_string(uint8_t *c){
	uint16_t i = 0;
	do{
		uart_send_byte(c[i]);
		i++;
		
	}while(c[i] != '\0');
	//uart_send_byte(c[i]);
}

uint16_t uart_read_count(void){
	return rx_count;
}



uint8_t uart_flush(void){
	rx_count = 0;
	rx_read_pos = 0;
	//memset((char *)rx_buffer, 0, 128);
	return 0;
}

uint8_t uart_read(void){
	
	uint8_t data = 0;
	data = rx_buffer[rx_read_pos];
	rx_read_pos++;
	rx_count--;
	return data;
}


void parseString(const char* input, display* output) {
	char* token;
	char* rest = (char*)input;

	
	token = strtok_r(rest, "/", &rest);
	if (token) {
		strncpy(output->first_line, token, sizeof(output->first_line) - 1);
		output->first_line[sizeof(output->first_line) - 1] = '\0';
	}

	token = strtok_r(rest, "/", &rest);
	if (token) {
		strncpy(output->second_line, token, sizeof(output->second_line) - 1);
		output->second_line[sizeof(output->second_line) - 1] = '\0';
	}

	token = strtok_r(rest, "/", &rest);
	if (token) {
		output->ld_red = (uint8_t)atoi(token);
	}

	token = strtok_r(rest, "/", &rest);
	if (token) {
		output->ld_green = (uint8_t)atoi(token);
	}

	token = strtok_r(rest, "/", &rest);
	if (token) {
		output->ld_orange = (uint8_t)atoi(token);
	}
}