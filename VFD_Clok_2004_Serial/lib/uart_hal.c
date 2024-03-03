
//sprintf(char_array , "Counter: %d", buff_count);
//uart_send_string((char *)char_array);


#include "uart_hal.h"
#include "string.h"
#include <stdbool.h>

volatile static uint8_t rx_buffer[RX_BUFFER_SIZE] = {0};
volatile static uint16_t rx_count = 0;	
volatile static uint8_t uart_tx_busy = 1;
static uint16_t rx_read_pos = 0;
char test_array[128]="";
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

ISR(USART0_RX_vect){
	
	//volatile static uint16_t rx_write_pos = 0;
	
	uint8_t res_char = UDR0;
	
	if (res_char == '\n'){
		read_complete = true;
		test_array[rx_count+1] = 0;
	}else{
		test_array[rx_count] = res_char;
		rx_count++;
	}
	
	
}

//memset(rx_buffer, 0, sizeof(rx_buffer));

ISR(USART0_TX_vect){
	uart_tx_busy = 1;
}


void uart_init(uint32_t baud,uint8_t high_speed){
	
	uint8_t speed = 16;
	
	if(high_speed != 0){
		speed = 8;
		UCSR0A |= 1 << U2X0;
	}
	
	baud = (F_CPU/(speed*baud)) - 1;
	
	UBRR0H = (baud & 0x0F00) >> 8;
	UBRR0L = (baud & 0x00FF);
	
	UCSR0B |= (1 << TXEN0) | (1 << RXEN0) | (1 << RXCIE0) | (1 << TXCIE0);
	UCSR0C=0b10000110;
}


void uart_send_byte(uint8_t c){
	while(uart_tx_busy == 0);
	uart_tx_busy = 0;
	UDR0 = c;
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
	for (int i = 0; i < sizeof(test_array) - 1; i++) {
        test_array[i] = ' ';
    }
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

uint8_t parseString(const char* input, display* output, rtc_date* newDate) {
	while (*input == '\r' || *input == '\n') {
        input++;
    }
	
	if (strncmp(input, "$lim", 4) == 0) {
		
		strncpy(output->second_line, input + 4, sizeof(output->second_line) - 1);
		
		output->second_line[sizeof(output->second_line) - 1] = '\0';
		
		
		return TEXT_SYNC_REQUEST;
	}
	
	if (strncmp(input, "$tim", 4) == 0){
		char* token = strtok((char*)(input + 4), ":");
		if (token != NULL) {
			newDate->hour = atoi(token);
			token = strtok(NULL, ":");
			
			if (token != NULL) {
				newDate->minute = atoi(token);
				token = strtok(NULL, ":");
				
				if (token != NULL) {
					newDate->second = atoi(token);
				}
			}
		}
		return TIME_SYNC_REQUEST;
	}
	if (strncmp(input, "$dat", 4) == 0){
		char* token = strtok((char*)(input + 4), "-");
		if (token != NULL) {
			newDate->date = atoi(token);
			token = strtok(NULL, "-");
			
			if (token != NULL) {
				newDate->month = atoi(token);
				token = strtok(NULL, "-");
				if (token != NULL) {
					newDate->year = atoi(token);
				}
			}
		}
		return DATE_SYNC_REQUEST;
	}
	
	return ERROR_SYNC_REQUEST;
}