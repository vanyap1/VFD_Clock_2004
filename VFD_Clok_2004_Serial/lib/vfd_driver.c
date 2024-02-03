/*
 * vfd_driver.c
 *
 * Created: 23.04.2023 16:53:37
 *  Author: Ivan Prints
 */ 
#include "vfd_driver.h"

#include <stdio.h>
#include <util/delay.h>
#include <avr/io.h>
#include <stdlib.h>



void vfd_data(uint8_t c)
{
	while(!( UCSR1A & (1 << UDRE1)));
	UDR1 = c;
	//while (!(UCSR1A & (1 << TXC1)));
}


void vfd_init()
{
	UBRR1L = (F_CPU/SCREEN_BAUDRATE/16-1); 
	UBRR1H = (F_CPU/SCREEN_BAUDRATE/16-1) >> 8;
	
	UCSR1B |= (1 << TXEN1);
	UCSR1C=0b00000110;
	vfd_clear();
		
}

void vfd_set_cursor(uint8_t line, uint8_t position){
	vfd_data(VFD_CMD);
	vfd_data(MOVE_CURSOR);
	vfd_data(position);
	vfd_data(line);
}

void vfd_set_brightness(uint8_t br){
	vfd_data(VFD_CMD);
	vfd_data(SET_BRIGHTNESS);
	vfd_data(br);
}

void vfd_set_underline_marker(uint8_t pos, uint8_t value){
	pos = (pos >= 20) ? 20 : pos;
	value = (value >= 1) ? 1 : value;
	vfd_data(VFD_CMD);
	vfd_data(SET_ANNUNICATOR);
	vfd_data(value);
	vfd_data(pos);
	
}

void vfd_clear(){
	vfd_data(VFD_CMD);
	vfd_data(CLEAR_SCREEN);
}

void vfd_string(uint8_t *c){
	uint16_t i = 0;
	do{
		vfd_data(c[i]);
		i++;
		//_delay_us(300);
		
	}while(c[i] != '\0');
}