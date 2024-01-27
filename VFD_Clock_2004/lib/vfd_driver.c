/*
 * vfd_driver.c
 *
 * Created: 23.04.2023 16:53:37
 *  Author: User
 */ 
#include "vfd_driver.h"
#include <avr/pgmspace.h>

#include <stdio.h>
#include <util/delay.h>
#include <avr/io.h>
#include <stdlib.h>


#define PORT_CLK  PORTB
#define PORT_DAT  PORTB
#define PORT_STB  PORTB
#define PORT_CS   PORTC
#define PORT_RST   PORTC


#define _CLK      PORTB5
#define _DAT      PORTB3
#define _STB	  PORTB0
#define _CS		  PORTC1
#define _RST	  PORTC0

#define CLK_HI   PORT_CLK |= (1<<_CLK);
#define CLK_LO   PORT_CLK &=~(1<<_CLK);
#define DAT_HI   PORT_DAT |= (1<<_DAT);
#define DAT_LO   PORT_DAT &=~(1<<_DAT);
#define STB_HI   PORT_STB |= (1<<_STB);
#define STB_LO   PORT_STB &=~(1<<_STB);
#define CS_HI   PORT_CS |= (1<<_CS);
#define CS_LO   PORT_CS &=~(1<<_CS);
#define RST_HI   PORT_RST |= (1<<_RST);
#define RST_LO   PORT_RST &=~(1<<_RST);

#define nop() asm volatile ("nop")






void spiwrite(uint8_t c)
{
	unsigned char n;
	for(n=8; n>0; n--)
	{
		CLK_LO;;              //SCL = low
		if(c &(1<<(n-1))){DAT_HI;}
		else{ DAT_LO;}
		nop();
		CLK_HI;               //SCL rising edge trigger
		nop();
	}
}

void vfd_command(uint8_t c)
{
	STB_HI
	nop();
	STB_LO
	spiwrite(0xf8);
	spiwrite(c);
	STB_HI
	nop();
	STB_LO
}

void vfd_data(uint8_t c)
{
	STB_HI
	nop();
	STB_LO
	spiwrite(0xfa);
	//_delay_us(1);
	spiwrite(c);
	STB_HI
	nop();
	STB_LO
}


void vfd_init()
{
	RST_LO
	_delay_ms(10);
	RST_HI
	CS_LO
	vfd_command(0b00111000); //0x38		//Function Set address [0,0,1,DL,N,X,BR1,BR0]
	vfd_command(0b00000001); //0x01		//Clear Display
	vfd_data(0x00);						//sets DDRAM address at 00H
	vfd_command(0b00001100); //0x0c		//Display ON/Off [0,0,0,0,1,D,C,B] D-screen on/off, C-cursor on/off, B-blink cursor on/off
	vfd_command(0b10000000); //0x80		//set cursor to 0 position
	int i;
	for (i = 0; i < 64; i++) {
		vfd_data(0xff);
	}
	vfd_command(0b00000001); //0x01
	CS_HI
}

void vfd_set_cursor(uint8_t line, uint8_t position){
	if(line==2){position += 0x40;}
	CS_LO
	vfd_command(0b10000000 | position);
	CS_HI
}


void vfd_string(uint8_t *c){
	uint16_t i = 0;
	do{
		vfd_data(c[i]);
		i++;
		
	}while(c[i] != '\0');
	//uart_send_byte(c[i]);
}