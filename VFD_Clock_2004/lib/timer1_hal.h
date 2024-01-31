/*
 * vfd_driver.c
 *
 * Created: 23.04.2023 16:53:37
 *  Author: Ivan Prints
 */ 
 


#ifndef TIMER1_HAL_H_
#define TIMER1_HAL_H_

#include <stdint.h>
#include <stdio.h>

#include "../config.h"
#include <avr/io.h>
#include <avr/interrupt.h>




void timer1_init(uint16_t counter_reg);
void timer_status_reset(void);
uint8_t timer_status(void);

#endif /* TIMER1_HAL_H_ */