/*
 * VFD_Clock_2004.c
 *
 * Created: 23.04.2023 14:57:41
 * Author : User
 */ 



#include "config.h"
#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>
#include <avr/sleep.h>
#include <stdbool.h>


#include "string.h"
#include "lib/uart_hal.h"
#include "lib/gpio_driver.h"
#include "lib/twi_hal.h"
#include "lib/timer1_hal.h"
#include "lib/vfd_driver.h"
#include "lib/rtc.h"


uint8_t expander_data[] = {0x00 , 0xAA};
#define EXPANDER_ADDR  	0x22
#define EXPANDER_REG  	0x00
uint8_t led_port = 0;

gpio vfd_reset = {(uint8_t *)&PORTC , PORTC1}; 
gpio vfd_cs = {(uint8_t *)&PORTC , PORTC0};
gpio vfd_stb = {(uint8_t *)&PORTB , PORTB0};
gpio vfd_mosi = {(uint8_t *)&PORTB , PORTB3};
gpio vfd_clk = {(uint8_t *)&PORTB , PORTB5};
gpio vfd_40v = {(uint8_t *)&PORTD , PORTD5};

	
gpio grn_led = {(uint8_t *)&led_port , PORTC0};
gpio red_led = {(uint8_t *)&led_port , PORTC1};	
gpio orange_led = {(uint8_t *)&led_port , PORTC2};

display vfd = {.first_line = "Ivan Prints project",
			.second_line = "03/07/2023   {38400}",
			.ld_green = 2,
			.ld_orange = 2,
			.ld_red = 2
};

rtc_date sys_rtc = {
	.date = 27,
	.month = 10,
	.year = 23,
	.dayofweek = 6,
	.hour = 16,
	.minute = 52,
	.second = 00
};







int main(void)
{
   
   
   sei();
   uart_init(38400,0);
   twi_init(400000);
   //char char_array[128]="\0";
   
   //DDRC = 255;
   set_pin_dir(&vfd_reset , PORT_DIR_OUT);	set_pin_level(&vfd_reset , false);
   set_pin_dir(&vfd_cs , PORT_DIR_OUT);		set_pin_level(&vfd_cs , true);
   set_pin_dir(&vfd_stb , PORT_DIR_OUT);	set_pin_level(&vfd_stb , false);
   set_pin_dir(&vfd_mosi , PORT_DIR_OUT);	set_pin_level(&vfd_mosi , false);
   set_pin_dir(&vfd_clk , PORT_DIR_OUT);	set_pin_level(&vfd_clk , true);
   set_pin_dir(&vfd_40v , PORT_DIR_OUT);	set_pin_level(&vfd_40v , true);
   _delay_ms(500);
   timer1_init(0);
   uint8_t blink_divider = 0;
   rtc_set(&sys_rtc);
   rtc_int_enable(&sys_rtc ,0);
   
   
   
   vfd_init();
   vfd_set_cursor(1,0);
   vfd_string((uint8_t *)"");
   vfd_set_cursor(2,0);
   vfd_string((uint8_t *)"");
   
   while (1) 
   {



	if (serial_complete()){
		const char *data_p = serial_read_data();
		parseString(data_p, &vfd);
	}






	_delay_ms(25);
	blink_divider++;
	if (blink_divider >= 15){
		rtc_sync(&sys_rtc);
		sprintf(vfd.first_line, "%02d:%02d:%02d  %02d-%02d-20%02d", sys_rtc.hour, sys_rtc.minute, sys_rtc.second, sys_rtc.date, sys_rtc.month, sys_rtc.year);
		
		blink_divider = 0;
		vfd_set_cursor(1,0);
		vfd_string((uint8_t *)vfd.first_line);
		vfd_set_cursor(2,0);
		vfd_string((uint8_t *)vfd.second_line);
		//sprintf(char_array, "%d; ", led_port);			
		//vfd_set_cursor(1,0);
		//vfd_string((uint8_t *)char_array);
		    
		
		
	
	}}
}

