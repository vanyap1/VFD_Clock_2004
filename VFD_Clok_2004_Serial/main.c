/*
 * m328pb_usbC_plus_RF.cpp
 *
 * Created: 22.12.2023 14:51:17
 * Author : Vanya Prints
 */ 

#include "config.h"

#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>
#include <avr/sleep.h>

#include "string.h"
#include "stdbool.h"
#include "uart_hal.h"
#include "gpio_driver.h"
#include "twi_hal1.h"
#include "spi1_hall.h"
#include "rtc.h"
#include "adc_hal.h"
#include "stdint.h"
#include "vfd_driver.h"
#include <stdio.h>
static FILE mystdout = FDEV_SETUP_STREAM((void *)uart_send_byte, NULL, _FDEV_SETUP_WRITE);


uint8_t rtc_int_request = 0;
uint16_t BAT_VOLT = 0;

rtc_date sys_rtc = {
	.date = 3,
	.month = 2,
	.year = 24,
	.dayofweek = 6,
	.hour = 20,
	.minute = 29,
	.second = 00
};

rtc_date newDate;
display vfd = {.first_line = "Ivan Prints project",
	.second_line = "Message from PC !@#$",
};

gpio hp_led = {(uint8_t *)&PORTD , PORTD5};

ISR(INT0_vect){
	rtc_int_request=1;
}

bool underscoreAction = true;
uint8_t underscoreCounter = 0;

int main(void)
{
    uart_init(38400,0);
	twi1_init(400000);
	vfd_init();
	sei();
	stdout = &mystdout;
	EICRA |= (0b10 << ISC00);
	EIMSK = 0x01; //0b00000001
	//rtc_set(&sys_rtc);
	rtc_int_enable(&sys_rtc ,0);
	//adc_init();
	//gpio_set_pin_direction(&hp_led , PORT_DIR_OUT); gpio_set_pin_level(&hp_led, false);
	
	//sleep_enable();
	//set_sleep_mode(SLEEP_MODE_PWR_DOWN);

	printf("RUN\n\r");
    while (1) 
    {
		if (rtc_int_request != 0){
			rtc_sync(&sys_rtc);
			//BAT_VOLT = get_mVolt(ADC5_PIN);
			rtc_int_request = 0;
			//printf("Date: %02d:%02d:%02d\n\r", sys_rtc.hour, sys_rtc.minute, sys_rtc.second);
			
			sprintf(vfd.first_line, " %02d:%02d:%02d %02d-%02d-20%02d", sys_rtc.hour, sys_rtc.minute, sys_rtc.second, sys_rtc.date, sys_rtc.month, sys_rtc.year);
			//printf("My message ^) %1d %02d  \r", underscoreAction, underscoreCounter);
			
			vfd_set_cursor(1,1);
			vfd_string((uint8_t *)vfd.first_line);
			//vfd_set_cursor(2,1);
			//vfd_string((uint8_t *)vfd.second_line);
			
			
			
			
			underscoreCounter++;
			if(underscoreCounter >= 20){
				underscoreCounter=0;
				underscoreAction = (underscoreAction == true) ? false : true;				
			}
			vfd_set_underline_marker(underscoreCounter, underscoreAction);
			vfd_set_brightness(4);
		}
		
		if (serial_complete()){
			const char *data_p = serial_read_data();
			rtc_sync(&newDate);
			uint8_t res = parseString(data_p, &vfd, &newDate);
			uart_flush();
			switch (res) {
				case TIME_SYNC_REQUEST:							//Time synchronization request handler
				rtc_set(&newDate);
				break;

				case DATE_SYNC_REQUEST:							//Date synchronization request handler
				rtc_set(&newDate);
				break;
				
				case TEXT_SYNC_REQUEST:							//Write PC message
				//vfd_set_cursor(2,1);
				//vfd_string((uint8_t *)"                    "); //Clear before draw new line
				vfd_set_cursor(2,2);
				vfd_string((uint8_t *)vfd.second_line);
				break;

				case ERROR_SYNC_REQUEST:
				sprintf(vfd.second_line, "!!! data parse error");
				break;
			}
		}
		/*
		Here will be PC handler function
		for PC message handler,
		RTC sync
		
		*/
    }
}

