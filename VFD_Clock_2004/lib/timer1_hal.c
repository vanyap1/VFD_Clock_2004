/*
Copyright (c) 2021 Devilbinder (Binder Tronics) https://bindertronics.com/

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */ 
 
#include "timer1_hal.h"


volatile static uint8_t timer_counter = 0;
volatile static uint16_t timer_compare_reg = 0;
ISR(TIMER1_OVF_vect){
	
	//update_pwm_ready = 0;
}

ISR(TIMER1_COMPB_vect){
	//if (timer_counter <= 255){
	//	timer_counter++;
	//	TCNT1 = 0;
	//}
	//OCR1BH = (timer_compare_reg & 0xFF00) >> 8;
	//OCR1BL = (timer_compare_reg & 0x00FF);
}

void timer_status_reset(void){
	//timer_counter=0;
	
}

uint8_t timer_status(void){
	return timer_counter;
	//sei();
}

void timer1_init(uint16_t counter_reg){
	sei();
	DDRB = DDRB | (0b1 << PORTB1)|(0b1 << PORTB2);
	OCR1A=119;
	OCR1B=135;
	TCCR1A =TCCR1A|(0b10 <<COM1A0)|(0b11 <<COM1B0)|(0b01 << WGM10);
	TCCR1B |= (0b1 << CS10) | (0b00 << WGM12);
	//TIMSK |= (1<<OCIE1A) | (1<<OCIE1B);
	//return 0;
}








